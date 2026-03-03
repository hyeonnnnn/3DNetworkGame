using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    private List<UI_ScoreItem> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<UI_ScoreItem>(true).ToList();

        foreach (var item in _items)
        {
            item.Hide();
        }

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


        // 1등부터 3등까지 내림차순 정렬
        List<ScoreData> scoreDatas = ScoreManager.Instance.GetSortedScores();

        int count = Mathf.Min(_items.Count, scoreDatas.Count);
        for (int i = 0; i < _items.Count; i++)
        {
            if (i < count)
            {
                ScoreData data = scoreDatas[i];
                string ranking = (i + 1).ToString();
                _items[i].Set(ranking, data.Nickname, data.Score);
                _items[i].Show();
            }
            else
            {
                _items[i].Hide();
            }
        }
    }
}
