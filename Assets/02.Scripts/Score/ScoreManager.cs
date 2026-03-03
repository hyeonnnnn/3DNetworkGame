using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance { get; private set; }

    private int _score;

    private Dictionary<int, ScoreData> _scores = new();
    public ReadOnlyDictionary<int, ScoreData> Scores => new ReadOnlyDictionary<int, ScoreData>(_scores); // 외부에서 수정 못하게 ReadOnlyDictionary로 반환

    public static event Action OnDataChanged;
    public static event Action<int> OnScoreAdded;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
    {
        // 기존 플레이어들의 점수 정보 가져오기
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("score", out object scoreObj))
            {
                ScoreData scoreData = new ScoreData()
                {
                    Nickname = player.NickName,
                    Score = (int)scoreObj
                };
                _scores[player.ActorNumber] = scoreData;
            }
        }
        OnDataChanged?.Invoke();

        // 자신의 점수 초기화
        Refresh();
    }

    private void Refresh()
    {
        // 해시테이블은 딕셔너리와 같은 키-값 형태로 저장하는데
        // 키-값에 있어서 자료형이 object다.
        Hashtable hashtable = new Hashtable();
        hashtable.Add("score", _score);

        // 프로퍼티 등록
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public List<ScoreData> GetSortedScores()
    {
        return _scores.Values.OrderByDescending(data => data.Score).ToList();
    }

    public void AddScore(int score)
    {
        _score += score;
        OnScoreAdded?.Invoke(score);
        Refresh();
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 자동으로 호출되는 함수
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score")) return;

        ScoreData scoreData = new ScoreData()
        {
            Nickname = targetPlayer.NickName,
            Score = (int)changedProps["score"]
        };

        _scores[targetPlayer.ActorNumber] = scoreData;
        OnDataChanged?.Invoke();
    }

    // [데이터 공유]
    // 1. OnSerializeView
    // ㄴ TransformView, AnimatorView, ...
    // ㄴ C# 기본 타입, Vector,
    // ㄴ PhotonNetwork, Rate에 따라..

    // 2. RPC -> 매개변수를 활용해서 동기화
    // ㄴ 주로 변화가 빈번하지 않은 데이터를 함수 호출을 이용해서 동기화

    // 3. 커스텀 프로퍼티
    // ㄴ 주로 변화가 빈번하지 않은 데이터'들'을 해시 테이블로 동기화
    // ㄴ 포톤의 마지막 기능!
    // ㄴ 플레이어 준비 상태, 점수, 룸의 모드, 맵 선택 등..
}
