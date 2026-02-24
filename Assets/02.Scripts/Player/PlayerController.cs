using Photon.Pun;
using UnityEngine;

// 플레이어 대표로서 외부와의 소통 또는 어빌리티들을 관리하는 역할
public class PlayerController : MonoBehaviour, IPunObservable
{
    public PhotonView PhotonView;
    public PlayerStat Stat;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    // 데이터 동기화를 위한 데이터 읽기, 쓰기 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 이 PhotonView의 데이터를 보내줘야 하는 상황
            // 박싱, 언박싱이 일어나고 있음
            // JSON을 사용하면 한 번만 일어나겠지만 무거울 수도 -> 상황에 따라
            stream.SendNext(Stat.Health);
            stream.SendNext(Stat.Stamina);
        }
        else if (stream.IsReading)
        {
            // 이 PhotonView의 데이터를 받아야 하는 상황
            // 보내주는 순서대로 맵핑된다.
            Stat.Health = (float)stream.ReceiveNext(); // Health
            Stat.Stamina = (float)stream.ReceiveNext(); // Stamina
        } 
    }
}
