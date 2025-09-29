using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    public Text resultText;

    private void Start()
    {
        // PlayerPrefs から取得
        int finalScore = PlayerPrefs.GetInt("LastScore", 0);
        if (resultText != null)
        {
            resultText.text = "最終スコア：" + finalScore + "枚";
        }
    }
}
