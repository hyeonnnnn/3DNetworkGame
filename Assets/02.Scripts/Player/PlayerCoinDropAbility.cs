using UnityEngine;

public class PlayerCoinDropAbility : PlayerAbility
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _minDropCount = 1;
    [SerializeField] private int _maxDropCount = 6;

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

        DropCoins();
    }

    private void DropCoins()
    {
        int count = Random.Range(_minDropCount, _maxDropCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
            Instantiate(_coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public override void OnUpdate() { }
}
