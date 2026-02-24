using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private bool _x, _y, _z;

    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target == null) return;

        transform.position = new Vector3(
            _x ? _target.position.x : transform.position.x,
            _y ? _target.position.y : transform.position.y,
            _z ? _target.position.z : transform.position.z);

        transform.rotation = Quaternion.Euler(90f, _target.eulerAngles.y, 0f);
    }
}
