using Photon.Pun;
using UnityEngine;

public class PlayerMinimapIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconRenderer;

    private void Start()
    {
        var photonView = GetComponentInParent<PhotonView>();
        if (photonView == null || _iconRenderer == null) return;

        if (photonView.IsMine)
        {
            _iconRenderer.color = PlayerVisuals.MyColor;
        }
        else
        {
            _iconRenderer.color = PlayerVisuals.OtherColor;
        }
    }
}
