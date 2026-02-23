using UnityEngine;

public enum AttackSequenceMode
{
    Sequential,
    Random
}

public class PlayerAttackAbility : PlayerAbility
{
    [SerializeField] private AttackSequenceMode _attackSequenceMode;

    private Animator _animator;
    private float _attackCoolTimer = 0f;

    private int _currentIndex = 0;
    private readonly string[] _attackTriggers = { "Attack1", "Attack2", "Attack3" };

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _attackCoolTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (_attackCoolTimer > 0f)
            return;

        _attackCoolTimer = _owner.Stat.AttackCoolTime;
        Attack();
    }

    private void Attack()
    {
        if (_attackSequenceMode == AttackSequenceMode.Sequential)
        {
            string trigger = _attackTriggers[_currentIndex];
            _animator.SetTrigger(trigger);

            _currentIndex = (_currentIndex + 1) % _attackTriggers.Length;
        }
        else if (_attackSequenceMode == AttackSequenceMode.Random)
        {
            int randomIndex = Random.Range(0, _attackTriggers.Length);
            _animator.SetTrigger(_attackTriggers[randomIndex]);
        }
    }
}
