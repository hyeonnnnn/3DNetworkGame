using Photon.Pun;
using UnityEngine;

// 플레이어 대표로서 외부와의 소통 또는 어빌리티들을 관리하는 역할
public class PlayerController : MonoBehaviour, IPunObservable, IDamageable
{
    public PhotonView PhotonView;
    public PlayerStat Stat;

    private float _lastSyncedHealth;
    private float _lastSyncedStamina;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PhotonView.IsMine)
        {
            RegisterToMinimapCamera();
        }
    }

    public void TakeDamage(float damage)
    {
        Stat.Health -= damage;
    }

    private void RegisterToMinimapCamera()
    {
        if (MinimapCamera.Instance != null)
        {
            MinimapCamera.Instance.SetTarget(transform);
        }
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
