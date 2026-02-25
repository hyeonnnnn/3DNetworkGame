using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    private Room _room;
    public Room Room => _room;

    public event Action OnDataChanged; // 룸 정보가 바뀌었을 때
    public event Action<Player> OnPlayerEnter; // 플레이어가 들어왔을 때
    public event Action<Player> OnPlayerLeft; // 플레이어가 나갔을 때

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

    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        OnDataChanged?.Invoke();

        SpawnManager.Instance.SpawnPlayer(); // 1줄로 줄이기...
    }

    // 새로운 플레이어가 방에 들어오면 호출되는 함수
    public override void OnPlayerEnteredRoom(Player player)
    {
        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(player);
    }

    // 플레이어가 방에서 나가면 호출되는 함수
    public override void OnPlayerLeftRoom(Player player)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(player);
    }
}
