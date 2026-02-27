using Photon.Pun;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Transform[] _spawnPoints;
    [SerializeField] protected string _prefabName;

    public Vector3 GetRandomSpawnPoint()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }
        int index = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[index].position;
    }

    public abstract void Spawn();
}
