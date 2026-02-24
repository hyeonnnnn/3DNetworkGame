using UnityEngine;

public class PlayerWeaponColliderAbility : PlayerAbility
{
    [SerializeField] private Collider _collider;

    private void Start()
    {
        DeactiveCollider();
    }

    public override void OnUpdate()
    {

    }

    private void DeactiveCollider()
    {
        _collider.enabled = false;
    }

    private void ActiveCollider()
    {
        _collider.enabled = true;
    }
}
