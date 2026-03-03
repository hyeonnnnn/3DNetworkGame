using TMPro;
using UnityEngine;

public class UI_ScoreItem : MonoBehaviour
{
    public TextMeshProUGUI RankingTextUI;
    public TextMeshProUGUI NicknameTextUI;
    public TextMeshProUGUI ScoreTextUI;

    public void Set(string ranking, string nickname, int score)
    {
        RankingTextUI.text = ranking;
        NicknameTextUI.text = nickname;
        ScoreTextUI.text = $"{score:N0}";
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
