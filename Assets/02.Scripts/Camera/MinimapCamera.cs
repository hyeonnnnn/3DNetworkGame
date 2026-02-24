using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public static MinimapCamera Instance { get; private set; }

    [SerializeField] private bool _followX, _followY, _followZ;

    private Transform _target;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (_target == null) return;

        transform.position = new Vector3(
            _followX ? _target.position.x : transform.position.x,
            _followY ? _target.position.y : transform.position.y,
            _followZ ? _target.position.z : transform.position.z);

        transform.rotation = Quaternion.Euler(90f, _target.eulerAngles.y, 0f);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
