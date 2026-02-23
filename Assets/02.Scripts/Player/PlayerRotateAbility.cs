using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    public Transform CameraRoot;

    private float _mx;
    private float _my;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleRotate();
    }

    private void HandleRotate()
    {
        _mx += Input.GetAxis("Mouse X") * _owner.stat.RotationSpeed * Time.deltaTime;
        _my += Input.GetAxis("Mouse Y") * _owner.stat.RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        // CameraRoot.localRotation = Quaternion.Euler(_my, 0f, 0f);
    }
}
