using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusAbility : PlayerAbility
{
    [SerializeField] private Image _staminaImage;

    public override void OnUpdate()
    {
        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        if (_staminaImage == null) return;

        _staminaImage.fillAmount = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
