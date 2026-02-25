using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    private void Start()
    {
        _logText.text = "You have entered the room.";

        PhotonRoomManager.Instance.OnPlayerEnter += OnPlayerEnteredRoom;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeftRoom;
    }

    private void OnPlayerEnteredRoom(Player player)
    {
        _logText.text += "\n" + $"{player.NickName} has joined the room.";
    }

    private void OnPlayerLeftRoom(Player player)
    {
        _logText.text += "\n" + $"{player.NickName} has left the room.";
    }
}
