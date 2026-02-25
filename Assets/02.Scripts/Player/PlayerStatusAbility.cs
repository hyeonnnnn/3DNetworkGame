using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusAbility : PlayerAbility
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _staminaImage;

    private void Update()
    {
        if (_owner.IsMine == false) return;

        UpdateHealthUI();
        UpdateStaminaUI();
    }

    public override void OnUpdate() { }

    private void UpdateHealthUI()
    {
        if (_healthImage == null) return;

        _healthImage.fillAmount = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }

    private void UpdateStaminaUI()
    {
        if (_staminaImage == null) return;

        _staminaImage.fillAmount = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
