using UnityEngine;
using Photon.Pun;

public enum AttackSequenceMode
{
    Sequential,
    Random
}

public class PlayerAttackAbility : PlayerAbility
{
    [SerializeField] private AttackSequenceMode _attackSequenceMode;

    private Animator _animator;
    private PhotonView _photonView;
    private float _attackCoolTimer;
    private int _currentIndex;

    private readonly string[] _attackTriggers = { "Attack1", "Attack2", "Attack3" };

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
    }

    public override void OnUpdate()
    {
        _attackCoolTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (_attackCoolTimer > 0f) return;
        if (_owner.Stat.TryConsumeStamina(_owner.Stat.AttackStaminaCost) == false) return;

        _attackCoolTimer = _owner.Stat.AttackCoolTime;
        Attack();
    }

    private void Attack()
    {
        int attackIndex = GetAttackIndex();
        
        // 1. 일반 메서드 호출 방식
        PlayAttackAnimation(attackIndex);

        // 2. RPC 메서드 호출 방식
        // 다른 컴퓨터에 있는 내 플레이어 오브젝트의 RPC_PlayAttack()를 실행한다.
        _photonView.RPC(nameof(RPC_PlayAttack), RpcTarget.Others, attackIndex);
    }

    private int GetAttackIndex()
    {
        if (_attackSequenceMode == AttackSequenceMode.Sequential)
        {
            int index = _currentIndex;
            _currentIndex = (_currentIndex + 1) % _attackTriggers.Length;
            return index;
        }

        return Random.Range(0, _attackTriggers.Length);
    }

    private void PlayAttackAnimation(int index)
    {
        _animator.SetTrigger(_attackTriggers[index]);
    }

    [PunRPC]
    private void RPC_PlayAttack(int index)
    {
        PlayAttackAnimation(index);
    }
}
