using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpForce = 2.5f;
    private const float GRAVITY = 9.8f;

    private float _yVeocity = 0f;
    private CharacterController _characterController;
    private Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        Vector3 direction = transform.forward * v + transform.right * h;
        direction.Normalize();

        float speed = Mathf.Clamp01(new Vector2(h, v).magnitude);
        _animator.SetFloat("Speed", speed);

        // 중력 적용
        _yVeocity -= GRAVITY * Time.deltaTime;
        direction.y = _yVeocity;

        // 점프
        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVeocity = JumpForce;
        }

        _characterController.Move(direction * Time.deltaTime * MoveSpeed);
    }
}