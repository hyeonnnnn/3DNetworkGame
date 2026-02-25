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

    public void DeactiveCollider()
    {
        _collider.enabled = false;
    }

    public void ActiveCollider()
    {
        _collider.enabled = true;
    }
}
