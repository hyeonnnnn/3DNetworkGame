using UnityEngine;
using UnityEngine.AI;

public class AttackState : IMonsterState
{
    public MonsterState Type => MonsterState.Attack;

    private readonly MonsterFSM _fsm;
    private readonly NavMeshAgent _agent;
    private readonly BearController _bear;
    private readonly Animator _animator;
    private readonly float _attackRange;

    private float _attackTimer;
    private bool _hasAttacked;

    private static readonly int s_attackTrigger = Animator.StringToHash("Attack");

    public AttackState(MonsterFSM fsm, NavMeshAgent agent, BearController bear, Animator animator, float attackRange = 2f)
    {
        _fsm = fsm;
        _agent = agent;
        _bear = bear;
        _animator = animator;
        _attackRange = attackRange;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _attackTimer = 0f;
        _hasAttacked = false;

        RotateTowardsTarget();
        _animator.SetTrigger(s_attackTrigger);
    }

    public void Tick()
    {
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

            if (distance > _attackRange)
            {
                _fsm.ChangeState(MonsterState.Chase);
            }
            else
            {
                _attackTimer = 0f;
                _animator.SetTrigger(s_attackTrigger);
            }
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
            _fsm.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
