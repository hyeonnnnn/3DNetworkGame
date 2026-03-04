using System;
using UnityEngine;

[Serializable]
public class BearStat
{
    public float Damage = 20f;

    public float WalkSpeed = 2f;
    public float RunSpeed = 4f;
    public float RotationSpeed = 100f;
    
    public float AttackCoolTime = 5f;
    
    public float MaxHealth = 200f;
    public float Health;

    public float AttackRange = 2f;
    public float DetectionRange = 10f;
    public float PatrolRadius = 8f;

    public void ApplyDamage(float amount)
    {
        Health = Mathf.Max(0, Health - amount);
    }
}
