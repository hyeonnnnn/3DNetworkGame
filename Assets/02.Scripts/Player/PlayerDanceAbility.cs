using Photon.Pun;
using UnityEngine;

public class PlayerDanceAbility : PlayerAbility
{
    private Animator _animator;
    private PhotonView _photonView;

    private const string DanceStateName = "Dance";
    private const string MoveTreeStateName = "MoveTree";

    private static readonly int s_danceTrigger = Animator.StringToHash(DanceStateName);
    private static readonly int s_moveTreeState = Animator.StringToHash(MoveTreeStateName);

    protected override void Awake()
    {
        base.Awake();
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
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(DanceStateName))
        {
            _animator.Play(s_moveTreeState, 0, 0f);
        }
    }

    private void PlayDance()
    {
        _animator.SetTrigger(s_danceTrigger);
        _photonView.RPC(nameof(RPC_PlayDance), RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_PlayDance()
    {
        _animator.SetTrigger(s_danceTrigger);
    }
}