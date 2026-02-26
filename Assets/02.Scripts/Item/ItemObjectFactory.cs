using UnityEngine;
using Photon.Pun;

public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance { get; private set; }

    [SerializeField] private int _minDropCount = 1;
    [SerializeField] private int _maxDropCount = 6;

    private PhotonView _photonView;

    private void Awake()
    {
        Instance = this;
        _photonView = GetComponent<PhotonView>();
    }

    // 방장에게 룸 관련해서 무언가 요청을 할 때에는
    // 메서드 명이 Request로 시작하는 게 유지보수에 좋다.
    public void RequestMakeScoreItems(Vector3 makePosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 내가 방장이면 내 함수 호출
            MakeScoreItems(makePosition);
        }
        else
        {
            // 방장이 아니면 방장의 함수 호출
            _photonView.RPC(nameof(MakeScoreItems), RpcTarget.MasterClient, makePosition);
        }
    }

    [PunRPC]
    private void MakeScoreItems(Vector3 makePosition)
    {
        Debug.Log("MakeScoreItems");

        int count = Random.Range(_minDropCount, _maxDropCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 1.5f;
            Vector3 spawnPosition = makePosition + new Vector3(randomOffset.x, 0f, randomOffset.y);
            PhotonNetwork.InstantiateRoomObject("Coin", spawnPosition, Quaternion.identity);
        }
    }

    public void RequestDelete(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DeleteItem(viewID);
        }
        else
        {
            _photonView.RPC(nameof(DeleteItem), RpcTarget.MasterClient, viewID);
        }
    }

    [PunRPC]
    private void DeleteItem(int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        if (view == null) return;

        PhotonNetwork.Destroy(view.gameObject);
    }
}
