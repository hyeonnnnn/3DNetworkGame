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

            otherPlayer.TakeDamageRPC(_owner.Damage);
            _owner.DeactiveWeaponCollider();
        }
    }
}
