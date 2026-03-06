using UnityEngine;
using UnityEngine.AI;

public class HitState : IMonsterState
{
    public MonsterState Type => MonsterState.Hit;

    private readonly MonsterFSM _fsm;
    private readonly NavMeshAgent _agent;
    private readonly Animator _animator;
    private readonly float _hitStunDuration;

    private float _timer;

    private static readonly int s_hitTrigger = Animator.StringToHash("Hit");

    public HitState(MonsterFSM fsm, NavMeshAgent agent, Animator animator, float hitStunDuration = 0.5f)
    {
        _fsm = fsm;
        _agent = agent;
        _animator = animator;
        _hitStunDuration = hitStunDuration;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _timer = 0f;
        _animator.SetTrigger(s_hitTrigger);
    }

    public void Tick()
    {
        _timer += Time.deltaTime;

        if (_timer >= _hitStunDuration)
        {
            if (_fsm.Target != null)
            {
                _fsm.ChangeState(MonsterState.Chase);
            }
            else
            {
                _fsm.ChangeState(MonsterState.Patrol);
            }
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }
}
