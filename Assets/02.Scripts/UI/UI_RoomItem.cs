using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameTextUI;
    [SerializeField] private TextMeshProUGUI _masterNicknameTextUI;
    [SerializeField] private TextMeshProUGUI _playerCountTextUI;
    [SerializeField] private Button _roomEnterButtonUI;

    private RoomInfo _roomInfo;

    private void Start()
    {
        _roomEnterButtonUI.onClick.AddListener(EnterRoom);
    }

    public void Init(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _roomNameTextUI.text = roomInfo.Name;
        _playerCountTextUI.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";

        if (roomInfo.CustomProperties.TryGetValue("MasterName", out object masterName))
        {
            _masterNicknameTextUI.text = masterName.ToString();
        }
    }

    private void EnterRoom()
    {
        if (_roomInfo == null) return;
    }
}
