using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private Transform[] _spawnPoints;

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

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
    }

    public Vector3 GetRandomSpawnPoint()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }
        int index = Random.Range(0, _spawnPoints.Length);

        return _spawnPoints[index].position;
    }
}
