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
    public event Action<string, string> OnPlayerDeath; // 플레이어가 죽었을 때

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
    public override void OnEnable()
    {
        base.OnEnable();
        PlayerController.OnPlayerDied += NotifyPlayerDeath;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerController.OnPlayerDied -= NotifyPlayerDeath;
    }

    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        OnDataChanged?.Invoke();

        SpawnManager.Instance.SpawnPlayer();
    }

    // 새로운 플레이어가 방에 들어오면 호출되는 함수
    public override void NotifyPlayerEnteredRoom(Player player)
    {
        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(player);
    }

    // 플레이어가 방에서 나가면 호출되는 함수
    public override void NotifyPlayerLeftRoom(Player player)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(player);
    }

    public void NotifyPlayerDeath(int attackerActorNumber)
    {
        if (_room.Players.TryGetValue(attackerActorNumber, out Player attacker))
        {
            string victimNickname = PhotonNetwork.LocalPlayer.NickName;
            OnPlayerDeath?.Invoke(attacker.NickName, victimNickname);
        }
        else
        {
            Debug.LogWarning($"공격자 정보를 찾을 수 없습니다. ActorNumber: {attackerActorNumber}");
        }
    }
}
