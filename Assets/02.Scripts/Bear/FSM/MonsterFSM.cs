using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MonsterFSM : MonoBehaviourPun, IPunObservable
{
    public MonsterState Current { get; private set; }
    public Transform Target { get; private set; }

    private Dictionary<MonsterState, IMonsterState> _states;    // 모든 상태
    private IMonsterState _currentState;                        // 현재 상태
    private int _targetActorNumber = -1;                        // 플레이어 번호

    public void Initialize(Dictionary<MonsterState, IMonsterState> states, MonsterState initialState)
    {
        _states = states;
        ChangeState(initialState);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _currentState?.Tick();
    }

    public void ChangeState(MonsterState newState)
    {
        if (Current == newState && _currentState != null) return;

        _currentState?.Exit();
        Current = newState;

        if (_states.TryGetValue(newState, out var state))
        {
            _currentState = state;
            _currentState.Enter();
        }
    }

    public void SetTarget(Transform target, int actorNumber)
    {
        Target = target;
        _targetActorNumber = actorNumber;
    }

    public void ClearTarget()
    {
        Target = null;
        _targetActorNumber = -1;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)Current);
            stream.SendNext(_targetActorNumber);
        }
        else
        {
            var netState = (MonsterState)(int)stream.ReceiveNext();
            _targetActorNumber = (int)stream.ReceiveNext();

            if (!PhotonNetwork.IsMasterClient && Current != netState)
            {
                ChangeState(netState);
            }

            UpdateTargetFromActorNumber();
        }
    }

    private void UpdateTargetFromActorNumber()
    {
        if (_targetActorNumber < 0)
        {
            Target = null;
            return;
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == _targetActorNumber)
            {
                var playerObject = FindPlayerByActorNumber(_targetActorNumber);
                Target = playerObject != null ? playerObject.transform : null;
                return;
            }
        }
    }

    private GameObject FindPlayerByActorNumber(int actorNumber)
    {
        foreach (var pv in FindObjectsByType<PhotonView>(FindObjectsSortMode.None))
        {
            if (pv.Owner != null && pv.Owner.ActorNumber == actorNumber && pv.CompareTag("Player"))
            {
                return pv.gameObject;
            }
        }
        return null;
    }
}
