using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpForce = 2.5f;
    private const float GRAVITY = 9.8f;
    private float _yVeocity = 0f;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(h, 0, v);
        direction.Normalize();

        _yVeocity -= GRAVITY * Time.deltaTime;
        direction.y = _yVeocity;

        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVeocity = JumpForce;
        }

        _characterController.Move(direction * Time.deltaTime * MoveSpeed);
    }
}