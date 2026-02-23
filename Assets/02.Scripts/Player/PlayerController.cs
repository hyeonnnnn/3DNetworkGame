using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PhotonView PhotonView;
    public Stat Stat;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }
}
