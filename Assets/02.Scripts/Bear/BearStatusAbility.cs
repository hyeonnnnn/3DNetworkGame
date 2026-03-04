using UnityEngine;
using UnityEngine.UI;

public class BearStatusAbility : BearAbility
{
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Image _healthImage;

    private Transform _cameraTransform;

    protected override void Awake()
    {
        base.Awake();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        UpdateHealthUI();

        _healthBar.transform.forward = _cameraTransform.forward;
    }

    private void UpdateHealthUI()
    {
        if (_healthImage == null) return;

        _healthImage.fillAmount = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }
}
