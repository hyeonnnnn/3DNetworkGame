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
        var spec = new RoomCreateSpec
        {
            Nickname = _nicknameInputField.text,
            RoomName = _roomNameInputField.text
        };

        if (!spec.IsValid) return;
        if (!PhotonNetwork.InLobby) return;

        PhotonNetwork.NickName = spec.Nickname;

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { Constants.RoomProperty.MasterName, spec.Nickname }
            },
            CustomRoomPropertiesForLobby = new string[] { Constants.RoomProperty.MasterName }
        };

        PhotonNetwork.CreateRoom(spec.RoomName, roomOptions);
    }
}
