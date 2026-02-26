using Photon.Pun;
using UnityEngine;

public class Coin : MonoBehaviourPun
{
    [SerializeField] private int _scoreValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        if (player.IsMine == false) return;

        player.Stat.AddScore(_scoreValue);
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
    }
}
