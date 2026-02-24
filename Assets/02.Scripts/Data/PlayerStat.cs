using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public float MoveSpeed;
    public float RotationSpeed;

    public float JumpPower;
    public float JumpStaminaCost;

    public float AttackCoolTime;
    public float AttackStaminaCost;

    public float MaxHealth;
    public float Health;

    public float MaxStamina;
    public float Stamina;

    public float Damage;

    public bool TryConsumeStamina(float amount)
    {
        if (Stamina < amount) return false;

        Stamina -= amount;
        return true;
    }

    public void RecoverStamina(float amount)
    {
        Stamina = Mathf.Min(MaxStamina, Stamina + amount);
    }

    public void DrainStamina(float amount)
    {
        Stamina = Mathf.Max(0, Stamina - amount);
    }
}