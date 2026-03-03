using Photon.Pun;
using UnityEngine;

public class PlayerUpgradeAbility : PlayerAbility
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private float _weaponScaleIncrement = 0.1f;
    [SerializeField] private int _scoreThreshold = 1000;

    private int _tempScore;

    private void Start()
    {
        if (_owner.PhotonView.IsMine)
        {
            ScoreManager.OnScoreAdded += OnScoreAdded;
        }
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreAdded -= OnScoreAdded;
    }

    public override void OnUpdate() { }

    private void OnScoreAdded(int score)
    {
        _tempScore += score;

        while (_tempScore >= _scoreThreshold)
        {
            _tempScore -= _scoreThreshold;
            IncreaseWeaponScale();
        }
    }

    private void IncreaseWeaponScale()
    {
        _owner.PhotonView.RPC(nameof(RPC_IncreaseWeaponScale), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_IncreaseWeaponScale()
    {
        if (_weapon == null) return;

        _weapon.transform.localScale += Vector3.one * _weaponScaleIncrement;
    }
}
