using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;

    private void Start()
    {
        _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;

        if (_owner.PhotonView.IsMine)
        {
            _nicknameTextUI.color = PlayerVisuals.MyColor;
        }
        else
        {
            _nicknameTextUI.color = PlayerVisuals.OtherColor;
        }
    }

    public override void OnUpdate()
    {

    }
}
