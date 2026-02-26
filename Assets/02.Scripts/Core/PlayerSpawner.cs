using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    public static PlayerSpawner Instance { get; private set; }

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

    public override void Spawn()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        PhotonNetwork.Instantiate(_prefabName, spawnPosition, Quaternion.identity);
    }
}
