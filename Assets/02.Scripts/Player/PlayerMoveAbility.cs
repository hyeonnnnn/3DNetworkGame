using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    [SerializeField] private float _sprintMultiplier = 1.3f;
    [SerializeField] private float _staminaDrainRate = 20f;
    [SerializeField] private float _staminaRecoveryRate = 10f;

    private const float GRAVITY = 9.8f;
    private const float GROUNDED_STICK_FORCE = -2f;

    private float _yVelocity;
    private float _baseMoveSpeed;
    private bool _isSprinting;

    private CharacterController _characterController;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _baseMoveSpeed = _owner.Stat.MoveSpeed;
    }

    public override void OnUpdate()
    {
        HandleMovement();
        HandleSprint();
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
    
    private void HandleSprint()
    {
        bool trySprint = Input.GetKey(KeyCode.LeftShift);
        bool hasStamina = _owner.Stat.Stamina > 0;

        if (trySprint && hasStamina && !_isSprinting)
        {
            _isSprinting = true;
            _owner.Stat.MoveSpeed = _baseMoveSpeed * _sprintMultiplier;
        }
        else if ((!trySprint || !hasStamina) && _isSprinting)
        {
            _isSprinting = false;
            _owner.Stat.MoveSpeed = _baseMoveSpeed;
        }

        if (_isSprinting)
        {
            _owner.Stat.Stamina -= _staminaDrainRate * Time.deltaTime;
            _owner.Stat.Stamina = Mathf.Max(0, _owner.Stat.Stamina);
        }
        else
        {
            _owner.Stat.Stamina += _staminaRecoveryRate * Time.deltaTime;
            _owner.Stat.Stamina = Mathf.Min(_owner.Stat.MaxStamina, _owner.Stat.Stamina);
        }
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