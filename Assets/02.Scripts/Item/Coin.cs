using Photon.Pun;
using UnityEngine;

public class Coin : MonoBehaviourPun
{
    [SerializeField] private int _scoreValue = 1;

    private bool _isCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;
        if (player.IsMine == false) return;
        if (player.IsDead) return;

        _isCollected = true;
        player.Stat.AddScore(_scoreValue);

        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
    }
}
