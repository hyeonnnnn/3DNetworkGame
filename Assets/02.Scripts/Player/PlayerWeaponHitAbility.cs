using UnityEngine;
using Photon.Pun;

public class PlayerWeaponHitAbility : PlayerAbility
{
    public override void OnUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner.IsMine == false) return;
        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<PlayerController>(out var otherPlayer))
        {
            Debug.Log("충돌");

            // 포톤에서는 Room 안에서 플레이어마다 고유 식별자인 ActorNumber을 가지고 있다.
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            // int actorNumber = _owner.PhotonView.Owner.ActorNumber;

            otherPlayer.TakeDamageRPC(_owner.Damage, actorNumber);
            _owner.DeactiveWeaponCollider();
        }
    }
}
