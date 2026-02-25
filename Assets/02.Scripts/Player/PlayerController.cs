using Photon.Pun;
using System.Collections;
using UnityEngine;

// 플레이어 대표로서 외부와의 소통 또는 어빌리티들을 관리하는 역할
public class PlayerController : MonoBehaviour, IPunObservable, IDamageable
{
    public PlayerStat Stat;
    public PhotonView PhotonView;
    private Animator _animator;

    public bool IsMine => PhotonView.IsMine;
    public float Damage => Stat.Damage;
    public bool IsDead { get; private set; }

    private float _lastSyncedHealth;
    private float _lastSyncedStamina;

    private const string DieStateName = "Die";
    private static readonly int s_dieTrigger = Animator.StringToHash(DieStateName);

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PhotonView.IsMine)
        {
            RegisterToMinimapCamera();
        }
    }

    private void RegisterToMinimapCamera()
    {
        if (MinimapCamera.Instance != null)
        {
            MinimapCamera.Instance.SetTarget(transform);
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackActorNumber)
    {
        if (IsDead) return;

        Debug.Log("데미지 입음");
        Stat.ApplyDamage(damage);

        if (Stat.Health <= 0)
        {
            Die(attackActorNumber);
        }
    }

    private void Die(int attackActorNumber)
    {
        IsDead = true;

        PhotonRoomManager.Instance.NotifyPlayerDeath(attackActorNumber);
        RPC_PlayDie();

        // 5초 후 체력, 스태미나 Max로 랜덤한 위치에 리스폰
        StartCoroutine(Coroutine_Respawn());
    }

    private IEnumerator Coroutine_Respawn()
    {
        yield return new WaitForSeconds(5f);

        Respawn();
    }

    private void Respawn()
    {
        // 체력, 스태미나 회복
        Stat.Health = Stat.MaxHealth;
        Stat.Stamina = Stat.MaxStamina;

        // 랜덤 위치로 이동
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;
        transform.position = SpawnManager.Instance.GetRandomSpawnPoint();
        if (characterController != null) characterController.enabled = true;

        // 상태 초기화
        IsDead = false;
        _animator.Rebind();
    }

    [PunRPC]
    private void RPC_PlayDie()
    {
        _animator.SetTrigger(s_dieTrigger);
    }

    public void TakeDamageRPC(float damage, int attackActorNumber)
    {
        PhotonView.RPC(nameof(TakeDamage), RpcTarget.All, damage, attackActorNumber);
    }

    public void DeactiveWeaponCollider()
    {
        GetAbility<PlayerWeaponColliderAbility>().DeactiveCollider();
    }

    public T GetAbility<T>() where T : PlayerAbility
    {
        return GetComponentInChildren<T>();
    }


    // 데이터 동기화를 위한 데이터 읽기, 쓰기 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (Stat == null) return;

        if (stream.IsWriting)
        {
            bool healthChanged = !Mathf.Approximately(_lastSyncedHealth, Stat.Health);
            bool staminaChanged = !Mathf.Approximately(_lastSyncedStamina, Stat.Stamina);

            // 이 PhotonView의 데이터를 보내줘야 하는 상황
            // 박싱, 언박싱이 일어나고 있음
            // JSON을 사용하면 한 번만 일어나겠지만 무거울 수도 -> 상황에 따라
            stream.SendNext(healthChanged);
            stream.SendNext(staminaChanged);

            if (healthChanged)
            {
                stream.SendNext(Stat.Health);
                _lastSyncedHealth = Stat.Health;
            }
            if (staminaChanged)
            {
                stream.SendNext(Stat.Stamina);
                _lastSyncedStamina = Stat.Stamina;
            }
        }
        else if (stream.IsReading)
        {
            // 이 PhotonView의 데이터를 받아야 하는 상황
            // 어떤 데이터가 오는지 확인한다.
            // 보내주는 순서대로 맵핑된다.
            bool healthChanged = (bool)stream.ReceiveNext(); // Health
            bool staminaChanged = (bool)stream.ReceiveNext(); // Stamina

            if (healthChanged)
            {
                Stat.Health = (float)stream.ReceiveNext();
            }
            if (staminaChanged)
            {
                Stat.Stamina = (float)stream.ReceiveNext();
            }
        } 
    }
}
