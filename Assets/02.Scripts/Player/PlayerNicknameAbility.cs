using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    private Transform _cameraTransform;

    protected override void Awake()
    {
        base.Awake();
        _cameraTransform = Camera.main.transform;
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

    public void Update()
    {
        _nicknameTextUI.transform.forward = _cameraTransform.forward;
    }

    public override void OnUpdate()
    {
        
    }
}
