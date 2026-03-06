using UnityEngine;

public abstract class BearAbility : MonoBehaviour
{
    protected BearController _owner { get; private set; }

    protected virtual void Awake()
    {
        _owner = GetComponentInParent<BearController>();
    }

    private void Update()
    {
        if (_owner.IsDead) return;
        OnUpdate();
    }

    protected virtual void OnUpdate() { }
}
