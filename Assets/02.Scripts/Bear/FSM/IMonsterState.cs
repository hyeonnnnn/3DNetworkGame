using UnityEngine;

public interface IMonsterState
{
    MonsterState Type { get; }
    void Enter();
    void Tick();
    void Exit();
}
