using TMPro;
using UnityEngine;

public class UI_ScoreItem : MonoBehaviour
{
    public TextMeshProUGUI RankingTextUI;
    public TextMeshProUGUI NicknameTextUI;
    public TextMeshProUGUI ScoreTextUI;

    public void Set(string ranking, string nickname, string score)
    {
        RankingTextUI.text = ranking;
        NicknameTextUI.text = nickname;
        ScoreTextUI.text = $"{score:N0}";
    }
}
