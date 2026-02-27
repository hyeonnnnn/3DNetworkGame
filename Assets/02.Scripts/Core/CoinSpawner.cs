using Photon.Pun;
using UnityEngine;

public class CoinSpawner : Spawner
{
    public static CoinSpawner Instance { get; private set; }

    [Header("Coin Spawn Settings")]
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private int _coinsPerSpawn = 3;
    [SerializeField] private float _spreadRadius = 1.5f;

    private float _spawnTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnMultiple();
        }
    }

    private void SpawnMultiple()
    {
        Vector3 basePosition = GetRandomSpawnPoint();
        basePosition.y += _spawnHeight;

        for (int i = 0; i < _coinsPerSpawn; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * _spreadRadius;
            Vector3 spawnPosition = basePosition + new Vector3(randomOffset.x, 0f, randomOffset.y);
            PhotonNetwork.InstantiateRoomObject(_prefabName, spawnPosition, Quaternion.identity);
        }
    }

    public override void Spawn()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        spawnPosition.y += _spawnHeight;
        PhotonNetwork.InstantiateRoomObject(_prefabName, spawnPosition, Quaternion.identity);
    }
}
