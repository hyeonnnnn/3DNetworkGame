using System;
using UnityEngine;

[Serializable]
public class BearStat
{
    public float Damage;

    public float MoveSpeed;
    public float RotationSpeed;
    
    public float AttackCoolTime;
    
    public float MaxHealth;
    public float Health;

    public void ApplyDamage(float amount)
    {
        Health = Mathf.Max(0, Health - amount);
    }
}
