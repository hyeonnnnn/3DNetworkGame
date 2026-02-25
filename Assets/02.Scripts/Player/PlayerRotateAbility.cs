using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    public Transform CameraRoot;

    private float _mx;
    private float _my;

    private float _minY = -70f;
    private float _maxY = 70f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;

        var vcam = Object.FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null) vcam.Follow = CameraRoot.transform;
    }

    public override void OnUpdate()
    {
        HandleRotate();
    }

    private void HandleRotate()
    {
        _mx += Input.GetAxis("Mouse X") * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += Input.GetAxis("Mouse Y") * _owner.Stat.RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, _minY, _maxY);

        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        CameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }
}
