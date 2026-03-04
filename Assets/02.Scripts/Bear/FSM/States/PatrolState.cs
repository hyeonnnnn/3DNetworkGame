using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IMonsterState
{
    public MonsterState Type => MonsterState.Patrol;

    private readonly MonsterFSM _fsm;
    private readonly NavMeshAgent _agent;
    private readonly BearController _bear;
    private readonly Animator _animator;

    private Vector3 _patrolTarget;
    private float _waitTimer;
    private const float WaitDuration = 2f;

    private static readonly int s_walkForward = Animator.StringToHash("WalkForward");
    private static readonly int s_idle = Animator.StringToHash("Idle");

    private bool _isWaiting;

    public PatrolState(MonsterFSM fsm, NavMeshAgent agent, BearController bear, Animator animator)
    {
        _fsm = fsm;
        _agent = agent;
        _bear = bear;
        _animator = animator;
    }

    public void Enter()
    {
        _agent.speed = _bear.Stat.WalkSpeed * 0.5f;
        _animator.SetBool(s_walkForward, true);
        SetRandomPatrolTarget();
    }

    public void Tick()
    {
        if (TryDetectPlayer(out var target, out int actorNumber))
        {
            _fsm.SetTarget(target, actorNumber);
            _fsm.ChangeState(MonsterState.Chase);   // 플레이어 추격
            return;
        }

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                _agent.isStopped = true;
                _agent.ResetPath();

                _animator.SetBool(s_walkForward, false);
                _animator.SetBool(s_idle, true);
            }

            _waitTimer += Time.deltaTime;
            if (_waitTimer >= WaitDuration)
            {
                _waitTimer = 0f;
                _isWaiting = false;
                _agent.isStopped = false;

                _animator.SetBool(s_idle, false);
                _animator.SetBool(s_walkForward, true);
                SetRandomPatrolTarget();
            }
        }
    }

    public void Exit()
    {
        _waitTimer = 0f;
        _isWaiting = false;
        _agent.isStopped = false;

        _animator.SetBool(s_walkForward, false);
        _animator.SetBool(s_idle, false);
    }

    private void SetRandomPatrolTarget()
    {
        float patrolRadius = _bear.Stat.PatrolRadius;
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += _fsm.transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            _patrolTarget = hit.position;
            _agent.SetDestination(_patrolTarget);
        }
    }

    private bool TryDetectPlayer(out Transform target, out int actorNumber)
    {
        target = null;
        actorNumber = -1;

        Collider[] colliders = Physics.OverlapSphere(_fsm.transform.position, _bear.Stat.DetectionRange);
        float closestDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!col.CompareTag("Player")) continue;

            var photonView = col.GetComponent<Photon.Pun.PhotonView>();
            if (photonView == null || photonView.Owner == null) continue;

            float distance = Vector3.Distance(_fsm.transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = col.transform;
                actorNumber = photonView.Owner.ActorNumber;
            }
        }

        return target != null;
    }
}
