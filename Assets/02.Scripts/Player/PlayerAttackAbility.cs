using UnityEngine;

public enum AttackSequenceMode
{
    Sequential,
    Random
}

public class PlayerAttackAbility : MonoBehaviour
{
    [SerializeField] private AttackSequenceMode _attackSequenceMode;

    private Animator _animator;
    private int _currentIndex = 0;
    private readonly string[] _attackTriggers = { "Attack1", "Attack2", "Attack3" };

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
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
