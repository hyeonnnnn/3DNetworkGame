using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    private List<UI_ScoreItem> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<UI_ScoreItem>().ToList();

        ScoreManager.OnDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        // 리드온리가 아니면 원본을 수정할 수 있으므로 무결성 문제가 생긴다.
        // -> 리드온리를 반환하도록 하자.
        // 그러면 왜 게터, 세터를 쓰는지?
        // -> null을 방어하려고

        var scores = ScoreManager.Instance.Scores;


        // Todo. 1등부터 3등까지 정렬
        // ㄴ 필수: Linq에서 사용 (정리 과제)
        //      ㄴ 무엇인지, 언제 쓰이는지, 장단점은 무엇인지
        // Todo. 3명이 있는지 적절하게 반복문
        List<ScoreData> scoreDatas = scores.Values.ToList();

        int count = Mathf.Min(_items.Count, scoreDatas.Count);
        for (int i = 0; i < count; i++)
        {
            ScoreData data = scoreDatas[i];
            _items[i].Set(data.Nickname, data.Score.ToString());
        }

    }
}
