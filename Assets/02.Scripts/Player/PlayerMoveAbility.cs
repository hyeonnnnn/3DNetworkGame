using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private const float GRAVITY = 9.8f;
    private const float GROUNDED_STICK_FORCE = -2f;

    private float _yVelocity = 0f;
    private CharacterController _characterController;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
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
        Vector3 velocity = moveDirection * _owner.Stat.MoveSpeed;
        velocity.y = _yVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleGravityAndJump()
    {
        // 중력 및 점프
        if (_characterController.isGrounded)
        {
            _yVelocity = GROUNDED_STICK_FORCE; // 바닥에 붙어있도록

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _owner.Stat.JumpPower;
            }
        }
        else
        {
            _yVelocity -= GRAVITY * Time.deltaTime;
        }
    }
}