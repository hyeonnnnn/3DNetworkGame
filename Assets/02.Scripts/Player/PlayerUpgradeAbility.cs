using UnityEngine;

public class PlayerUpgradeAbility : PlayerAbility
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private float _weaponScaleIncrement = 0.1f;
    [SerializeField] private int _scoreThreshold = 1000;

    private Vector3 _initialScale;

    private void Start()
    {
        if (_weapon != null)
        {
            _initialScale = _weapon.transform.localScale;
        }

        _owner.OnScoreChanged += UpdateWeaponScale;
    }

    private void OnDestroy()
    {
        _owner.OnScoreChanged -= UpdateWeaponScale;
    }

    public override void OnUpdate() { }

    private void UpdateWeaponScale(int score)
    {
        if (_weapon == null) return;

        int upgradeLevel = score / _scoreThreshold;
        _weapon.transform.localScale = _initialScale + Vector3.one * _weaponScaleIncrement * upgradeLevel;
    }
}
