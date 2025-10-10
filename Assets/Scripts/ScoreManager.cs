using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // シングルトン化

    [Header("スコア表示用テキスト（プレイ中）")]
    public Text scoreText;

    [Header("最終スコア表示用テキスト（リザルト画面など）")]
    public Text resultText;

    private int score = 0;

    private void Awake()
    {
        // シングルトン設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーン切り替え時に呼ばれる
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // タイトルシーンのときだけスコアをリセット
        if (scene.name == "Title" || scene.name == "TitleScene")
        {
            Clear();
        }

        // 新しいシーンでUIを探し直す（タグで管理するのが楽）
        if (scoreText == null)
        {
            GameObject scoreObj = GameObject.FindWithTag("ScoreText");
            if (scoreObj != null)
                scoreText = scoreObj.GetComponent<Text>();
        }

        if (resultText == null)
        {
            GameObject resultObj = GameObject.FindWithTag("ResultText");
            if (resultObj != null)
                resultText = resultObj.GetComponent<Text>();
        }

        UpdateUIText();
    }

    // スコア加算
    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateUIText();
    }

    // スコアクリア（タイトルシーン用）
    public void Clear()
    {
        score = 0;
        UpdateUIText();
    }

    // スコア保存（リザルト用など）
    public void SaveScore()
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }

    // UIテキスト更新
    private void UpdateUIText()
    {
        if (scoreText != null)
            scoreText.text = "切った数：" + score + "枚";

        if (resultText != null)
            resultText.text = "最終スコア：" + score + "枚";
    }
}
