using Photon.Pun;
using System;
using UnityEngine;

public class BearController : MonoBehaviour, IPunObservable, IDamageable
{
    public BearStat Stat;
    public PhotonView PhotonView;
    private Animator _animator;

    public bool IsDead { get; private set; }

    public static event Action<int> OnBearDied;

    private float _lastSyncedHealth;

    private const string DieStateName = "Die";
    private static readonly int s_dieTrigger = Animator.StringToHash(DieStateName);

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
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

        OnBearDied?.Invoke(attackerActorNumber);
        PhotonView.RPC(nameof(RPC_PlayDie), RpcTarget.All);
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
