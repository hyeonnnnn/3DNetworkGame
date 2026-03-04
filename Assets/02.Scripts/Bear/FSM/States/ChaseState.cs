using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IMonsterState
{
    public MonsterState Type => MonsterState.Chase;

    private readonly MonsterFSM _fsm;
    private readonly NavMeshAgent _agent;
    private readonly BearController _bear;
    private readonly Animator _animator;

    private static readonly int s_runForward = Animator.StringToHash("Run Forward");

    public ChaseState(MonsterFSM fsm, NavMeshAgent agent, BearController bear, Animator animator)
    {
        _fsm = fsm;
        _agent = agent;
        _bear = bear;
        _animator = animator;
    }

    public void Enter()
    {
        _agent.speed = _bear.Stat.RunSpeed;
        _animator.SetBool(s_runForward, true);
        _agent.isStopped = false;
    }

    public void Tick()
    {
        if (_fsm.Target == null)
        {
            _fsm.ClearTarget();
            _fsm.ChangeState(MonsterState.Patrol);
            return;
        }

        float distance = Vector3.Distance(_fsm.transform.position, _fsm.Target.position);

        if (distance > _bear.Stat.DetectionRange)
        {
            _fsm.ClearTarget();
            _fsm.ChangeState(MonsterState.Patrol);
            return;
        }

        if (distance <= _bear.Stat.AttackRange)
        {
            _fsm.ChangeState(MonsterState.Attack);
            return;
        }

        _agent.SetDestination(_fsm.Target.position);
        RotateTowardsTarget();
    }

    public void Exit()
    {
        _animator.SetBool(s_runForward, false);
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
