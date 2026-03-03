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

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            damageable.TakeDamageRPC(_owner.Damage, actorNumber);
            _owner.DeactiveWeaponCollider();
        }
    }
}
