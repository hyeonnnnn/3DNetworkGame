using Photon.Pun;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _scoreValue = 10;

    private bool _isCollected;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponentInParent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;
        if (player.IsMine == false) return;
        if (player.IsDead) return;

        _isCollected = true;
        ScoreManager.Instance.AddScore(_scoreValue);
        Debug.Log(_scoreValue);

        ItemObjectFactory.Instance.RequestDelete(_photonView.ViewID);
    }
}
