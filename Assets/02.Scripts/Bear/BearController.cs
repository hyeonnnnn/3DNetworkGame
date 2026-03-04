using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviour, IPunObservable, IDamageable
{
    public BearStat Stat;
    public PhotonView PhotonView;

    private Animator _animator;
    private NavMeshAgent _agent;
    private MonsterFSM _fsm;

    public bool IsDead { get; private set; }

    public static event Action<int> OnBearDied;

    private float _lastSyncedHealth;

    private const string DieStateName = "Die";
    private static readonly int s_dieTrigger = Animator.StringToHash(DieStateName);
    private const float DieAnimationDelay = 3.2f;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _fsm = GetComponent<MonsterFSM>();

        InitializeFSM();
    }

    private void InitializeFSM()
    {
        var states = new Dictionary<MonsterState, IMonsterState>
        {
            { MonsterState.Patrol, new PatrolState(_fsm, _agent, this, _animator) },
            { MonsterState.Chase, new ChaseState(_fsm, _agent, this, _animator) },
            { MonsterState.Attack, new AttackState(_fsm, _agent, this, _animator) },
            { MonsterState.Hit, new HitState(_fsm, _agent, _animator) },
            { MonsterState.Dead, new DeadState(_fsm, _agent) }
        };

        _fsm.Initialize(states, MonsterState.Patrol);
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackerActorNumber)
    {
        if (IsDead) return;

        Stat.ApplyDamage(damage);

        if (Stat.Health <= 0)
        {
            Die(attackerActorNumber);
        }
    }

    private void Die(int attackerActorNumber)
    {
        IsDead = true;

        _fsm.ChangeState(MonsterState.Dead);
        OnBearDied?.Invoke(attackerActorNumber);
        PhotonView.RPC(nameof(RPC_PlayDie), RpcTarget.All);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DestroyAfterDelay(DieAnimationDelay));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void RPC_PlayDie()
    {
        _animator.SetTrigger(s_dieTrigger);
    }

    public void TakeDamageRPC(float damage, int attackerActorNumber)
    {
        PhotonView.RPC(nameof(TakeDamage), RpcTarget.All, damage, attackerActorNumber);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (Stat == null) return;

        if (stream.IsWriting)
        {
            bool healthChanged = !Mathf.Approximately(_lastSyncedHealth, Stat.Health);

            stream.SendNext(healthChanged);

            if (healthChanged)
            {
                stream.SendNext(Stat.Health);
                _lastSyncedHealth = Stat.Health;
            }
        }
        else if (stream.IsReading)
        {
            bool healthChanged = (bool)stream.ReceiveNext();

            if (healthChanged)
            {
                Stat.Health = (float)stream.ReceiveNext();
            }
        }
    }
}
