using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;

    private readonly Color _myColor = new Color(0f, 0.46f, 1f);
    private readonly Color _otherColor = new Color(1f, 0.15f, 0f);

    private void Start()
    {
        _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;

        if (_owner.PhotonView.IsMine)
        {
            _nicknameTextUI.color = _myColor;
        }
        else
        {
            _nicknameTextUI.color = _otherColor;
        }
    }

    public override void OnUpdate()
    {

    }
}
