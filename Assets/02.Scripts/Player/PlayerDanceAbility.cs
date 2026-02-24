using Photon.Pun;
using UnityEngine;

public class PlayerDanceAbility : PlayerAbility
{
    private Animator _animator;
    private PhotonView _photonView;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayDance();
        }

        if (IsMoving())
        {
            StopDance();
        }
    }

    private bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f
            || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;
    }

    private void StopDance()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
        {
            _animator.Play("MoveTree", 0, 0f);
        }
    }

    private void PlayDance()
    {
        _animator.SetTrigger("Dance");
        _photonView.RPC(nameof(RPC_PlayDance), RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_PlayDance()
    {
        _animator.SetTrigger("Dance");
    }
}