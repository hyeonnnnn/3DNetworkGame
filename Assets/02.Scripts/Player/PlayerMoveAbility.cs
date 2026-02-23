using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [Header("움직임 세팅")]
    [SerializeField] private float MoveSpeed = 7f;
    [SerializeField] private float JumpForce = 2.5f;
    [SerializeField] private const float GRAVITY = 9.8f;
    [SerializeField] private float groundedStickForce = -2f;

    private float _yVelocity = 0f;
    private CharacterController _characterController;
    private Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleGravityAndJump();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 수평 이동
        Vector3 moveDirection = transform.forward * v + transform.right * h;
        moveDirection.Normalize();

        float speed = Mathf.Clamp01(new Vector2(h, v).magnitude);
        _animator.SetFloat("Speed", speed);

        // 최종 이동 적용
        Vector3 velocity = moveDirection * MoveSpeed;
        velocity.y = _yVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleGravityAndJump()
    {
        // 중력 및 점프
        if (_characterController.isGrounded)
        {
            _yVelocity = groundedStickForce; // 바닥에 붙어있도록

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = JumpForce;
            }
        }
        else
        {
            _yVelocity -= GRAVITY * Time.deltaTime;
        }
    }
}