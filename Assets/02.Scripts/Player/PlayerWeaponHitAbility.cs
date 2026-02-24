using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerWeaponHitAbility : PlayerAbility
{
    public override void OnUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _owner.transform) return;

        Debug.Log("충돌");

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            other.GetComponent<IDamageable>().TakeDamage(_owner.Stat.Damage);
        }
    }
}
