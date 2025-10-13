using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    [SerializeField,Header("スコア表示用テキスト")]
    public Text scoreText;

    void Start()
    {
        // シーンが読み込まれたらスコアをリセット
        score = 0;
    }

    public void Water()
    {
        score++;
        Debug.Log("Score: " + score);
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
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
