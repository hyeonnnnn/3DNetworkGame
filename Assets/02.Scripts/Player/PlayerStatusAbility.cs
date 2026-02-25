using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerStatusAbility : PlayerAbility
{
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _staminaBar;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _staminaImage;

    private Transform _cameraTransform;

    protected override void Awake()
    {
        base.Awake();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        UpdateHealthUI();
        UpdateStaminaUI();

        _healthBar.transform.forward = _cameraTransform.forward;
        _staminaBar.transform.forward = _cameraTransform.forward;
    }

    public override void OnUpdate() { }

    [PunRPC]
    private void UpdateHealthUI()
    {
        if (_healthImage == null) return;

        _healthImage.fillAmount = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }

    [PunRPC]
    private void UpdateStaminaUI()
    {
        if (_staminaImage == null) return;

        _staminaImage.fillAmount = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
