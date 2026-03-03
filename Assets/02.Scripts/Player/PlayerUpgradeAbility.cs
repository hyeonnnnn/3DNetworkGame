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

        ScoreManager.OnDataChanged += UpdateWeaponScale;
    }

    private void OnDestroy()
    {
        ScoreManager.OnDataChanged -= UpdateWeaponScale;
    }

    public override void OnUpdate() { }

    private void UpdateWeaponScale()
    {
        if (_weapon == null) return;

        int actorNumber = _owner.PhotonView.OwnerActorNr;
        var scores = ScoreManager.Instance.Scores;

        if (!scores.TryGetValue(actorNumber, out ScoreData data)) return;

        int upgradeLevel = data.Score / _scoreThreshold;
        _weapon.transform.localScale = _initialScale + Vector3.one * _weaponScaleIncrement * upgradeLevel;
    }
}
