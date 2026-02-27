using UnityEngine;
using Photon.Pun;

public class PlayerCoinDropAbility : PlayerAbility
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Vector3 SpawnPosition = new Vector3(0, 0f, 0);

    private void OnEnable()
    {
        PlayerController.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied(int attackerActorNumber)
    {
        if (_owner.IsMine == false) return;

        ItemObjectFactory.Instance.RequestMakeScoreItems(transform.position + SpawnPosition);
    }

    public override void OnUpdate() { }
}
