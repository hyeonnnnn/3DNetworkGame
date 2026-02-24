using UnityEngine;
using Photon.Pun;

public class PlayerWeaponHitAbility : PlayerAbility
{
    public override void OnUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner.PhotonView.IsMine == false) return;
        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<PlayerController>(out var otherPlayer))
        {
            Debug.Log("충돌");
            
            // 상대방의 TakeDamage를 RPC로 호출한다.
            otherPlayer.PhotonView.RPC(nameof(PlayerController.TakeDamage), RpcTarget.All, _owner.Stat.Damage);

            _owner.GetAbility<PlayerWeaponColliderAbility>().DeactiveCollider();
        }
    }
}
