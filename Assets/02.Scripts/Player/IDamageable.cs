using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, int attackerActorNumber);
    void TakeDamageRPC(float damage, int attackerActorNumber);
}
