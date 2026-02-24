using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

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
        _nicknameTextUI.transform.forward = _camera.transform.forward;
    }
}
