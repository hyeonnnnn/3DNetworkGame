using Photon.Pun;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void Start()
    {
        // 리소스 폴더 말고 다른 방법 찾아보기
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
}
