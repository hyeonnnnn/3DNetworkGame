using Photon.Pun;
using UnityEngine;

public class GameScene : MonoBehaviourPunCallbacks
{
    private bool _isSpawned = false;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (_isSpawned) return;

        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        _isSpawned = true;
    }
}