using UnityEngine;
using UnityEngine.AI;

public class DeadState : IMonsterState
{
    public MonsterState Type => MonsterState.Dead;

    private readonly MonsterFSM _fsm;
    private readonly NavMeshAgent _agent;

    public DeadState(MonsterFSM fsm, NavMeshAgent agent)
    {
        _fsm = fsm;
        _agent = agent;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _agent.enabled = false;
    }

    public void Tick()
    {
        // Dead 상태에서는 아무것도 하지 않음
    }

    public void Exit()
    {
    }
}
