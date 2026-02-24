using Photon.Pun;
using UnityEngine;

public class PlayerMinimapIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconRenderer;

    private readonly Color _myColor = new Color(0f, 0.46f, 1f);
    private readonly Color _otherColor = new Color(1f, 0.15f, 0f);

    private void Start()
    {
        var photonView = GetComponentInParent<PhotonView>();
        if (photonView == null || _iconRenderer == null) return;

        if (photonView.IsMine)
        {
            _iconRenderer.color = _myColor;
        }
        else
        {
            _iconRenderer.color = _otherColor;
        }
    }
}
