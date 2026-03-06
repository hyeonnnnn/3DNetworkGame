using Photon.Pun;
using UnityEngine;

public class BearSpawner : Spawner
{
    public static BearSpawner Instance { get; private set; }

    [Header("Bear Spawn Settings")]
    [SerializeField] private float _spawnInterval = 60f;

    private float _timer;

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

        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            Spawn();
        }
    }

    public override void Spawn()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        SpawnRoomObject(spawnPosition, Quaternion.identity);
    }
}
