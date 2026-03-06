using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    // Nickname
    [SerializeField] private TMP_InputField _nicknameInputField;

    //  RoomID
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private Button CreateRoomButton;

    // Gender
    [SerializeField] private GameObject _maleCharacter;
    [SerializeField] private GameObject _femaleCharacter;
    private ECharacterType _characterType;

    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);

    private void Start()
    {
        CreateRoomButton.onClick.AddListener(MakeRoom);
        _maleCharacter.SetActive(true);
        _femaleCharacter.SetActive(false);
    }

    private void OnClickCharacterButton(ECharacterType _newCharacterType)
    {
        _characterType = _newCharacterType;

        _maleCharacter.SetActive(_characterType == ECharacterType.Male);
        _femaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }

    public void MakeRoom()
    {
        // 명세로 만들어서 빼기
        string nickname = _nicknameInputField.text;
        string roomName = _roomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))  return;

        // GameServer에 있으면 Master Server로 돌아가기
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            return;
        }

        // Master Server에 연결되어 있지 않으면 재연결
        if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.InLobby)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        PhotonNetwork.NickName = nickname;

        // 룸 옵션 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;  // 룸 최대 접속자 수
        roomOptions.IsVisible = true; // 로비에서 룸을 보여줄 것인지
        roomOptions.IsOpen = true;    // 룸의 오픈 여부
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "MasterName", nickname }
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "MasterName" };

        // 룸 만들기
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
}
