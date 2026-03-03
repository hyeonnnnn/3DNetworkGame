using TMPro;
using UnityEngine;

public class UI_ScoreItem : MonoBehaviour
{
    public TextMeshProUGUI NicknameTextUI;
    public TextMeshProUGUI ScoreTextUI;

    public void Set(string nickname, string score)
    {
        NicknameTextUI.text = nickname;
        ScoreTextUI.text = $"{score:N0}";
    }
}
