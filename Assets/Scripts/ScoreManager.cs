using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    [SerializeField,Header("スコア表示用テキスト")]
    public Text scoreText;

    public TextMeshProUGUI Text;

    void Start()
    {
        // シーンが読み込まれたらスコアをリセット
        score =0;

        if (scoreText != null) return;
        if(Text != null)return;
    }

    public void Water()
    {
        score++;
        UpdateScoreText();
    }

    void OnDisable()
    {
        // シーン移動前にスコアを保存
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "切った数：" + score.ToString()+"枚";
        }

        if(Text != null)
        {
            Text.text = score.ToString();
        }
    }
}
