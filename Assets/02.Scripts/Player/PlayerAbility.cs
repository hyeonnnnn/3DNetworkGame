using UnityEngine;

abstract public class PlayerAbility : MonoBehaviour
{
    protected PlayerController _owner {  get; private set; }

    protected virtual void Awake()
    {
        _owner = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_owner.PhotonView.IsMine == true) OnUpdate();
    }

    abstract public void OnUpdate();
}
