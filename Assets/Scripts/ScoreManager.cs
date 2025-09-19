using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // シングルトン化
    public int score = 0;

    [Header("スコア表示用テキスト")]
    public Text scoreText; // InspectorでUIのTextを設定

    private void Awake()
    {
        // シングルトン設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreText();
    }

    // スコア加算メソッド
    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateScoreText();
    }

    // Textを更新するメソッド
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "切った数："+score+"枚";
        }
    }
}
