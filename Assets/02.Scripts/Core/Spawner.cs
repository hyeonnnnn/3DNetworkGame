using Photon.Pun;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Transform[] _spawnPoints;
    [SerializeField] protected string _prefabName;
    [SerializeField] protected GameObject _root;

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

    protected GameObject SpawnPrefab(Vector3 position, Quaternion rotation)
    {
        GameObject instance = PhotonNetwork.Instantiate(_prefabName, position, rotation);
        if (_root != null)
        {
            instance.transform.SetParent(_root.transform);
        }
        return instance;
    }

    protected GameObject SpawnRoomObject(Vector3 position, Quaternion rotation)
    {
        GameObject instance = PhotonNetwork.InstantiateRoomObject(_prefabName, position, rotation);
        if (_root != null)
        {
            instance.transform.SetParent(_root.transform);
        }
        return instance;
    }
}
