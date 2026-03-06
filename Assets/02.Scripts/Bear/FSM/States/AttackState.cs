using UnityEngine;
using UnityEngine.AI;

public class AttackState : IMonsterState
{
    public MonsterState Type => MonsterState.Attack;

    private readonly MonsterFSM _fsm;
    private readonly NavMeshAgent _agent;
    private readonly BearController _bear;
    private readonly Animator _animator;

    private float _attackTimer;

    private static readonly int s_attackTrigger = Animator.StringToHash("Attack1");

    public AttackState(MonsterFSM fsm, NavMeshAgent agent, BearController bear, Animator animator)
    {
        _fsm = fsm;
        _agent = agent;
        _bear = bear;
        _animator = animator;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _attackTimer = 0f;

        RotateTowardsTarget();
        _animator.SetTrigger(s_attackTrigger);
        Attack();
    }

    public void Tick()
    {
        RotateTowardsTarget();
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _bear.Stat.AttackCoolTime)
        {
            if (_fsm.Target == null)
            {
                _fsm.ClearTarget();
                _fsm.ChangeState(MonsterState.Patrol);
                return;
            }

            float distance = Vector3.Distance(_fsm.transform.position, _fsm.Target.position);

            if (distance > _bear.Stat.AttackRange)
            {
                _fsm.ChangeState(MonsterState.Chase);
            }
            else // 공격
            {
                _attackTimer = 0f;
                _animator.SetTrigger(s_attackTrigger);
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (_fsm.Target == null) return;

        if (_fsm.Target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamageRPC(_bear.Stat.Damage, -1);
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }

    private void RotateTowardsTarget()
    {
        if (_fsm.Target == null) return;

        Vector3 direction = (_fsm.Target.position - _fsm.transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _fsm.transform.rotation = Quaternion.Slerp(
                _fsm.transform.rotation,
                targetRotation,
                _bear.Stat.RotationSpeed * Time.deltaTime
            );
        }
    }
}
