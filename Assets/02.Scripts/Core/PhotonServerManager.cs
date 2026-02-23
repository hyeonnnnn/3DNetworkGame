using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    private string _version = "0.0.1";
    private string _nickname = "hy";

    private void Start()
    {
        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.NickName = _nickname;

        // 방장이 로드한 씬 게임에 다른 유저들도 똑같이 그 씬을 로드하도록 동기화해준다.
        // 방장(마스터 클라이언트): 방을 만든 소유자 (방에는 하나의 마스터 클라이언트가 존재)
        // 방장이 씬을 옮기면 다른 사람들도 자동으로 옮겨진다.
        PhotonNetwork.AutomaticallySyncScene = true;

        // 위에 설정한 값을 이용해서 서버로 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속되면 호출되면 콜백 함수
    public override void OnConnected()
    {
        Debug.Log("네임서버 접속 완료");
        
        // ping 테스트를 통해서 가장 빠른 리전으로 자동 연결된다.
        // Debug.Log(PhotonNetwork.CloudRegion);
        
    }

    public override void OnConnectedToMaster()
    {
        // 포톤 서버는 로비(채널)라는 개념이 있다.
        // TypedLobby lobby = new TypedLobby("3channel", LobbyType.Default);

        PhotonNetwork.JoinLobby(); // 디폴트 로비 입장 시도
    }

    // 로비 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        Debug.Log(PhotonNetwork.InLobby);
    
        // 랜덤 방 입장 시도
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료");

        Debug.Log($"룸 이름: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");

        /*        
        Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach(KeyValuePair<int, Player> player in roomPlayers)
        {
            Debug.Log($"");
        }*/
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패했습니다. {returnCode} - {message}");

        // 룸 옵션 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        // 룸 만들기
        PhotonNetwork.CreateRoom("test", roomOptions);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다. {returnCode} - {message}");
    }
}
