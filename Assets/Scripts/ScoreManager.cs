using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // シングルトン化
    public int score = 0;

    [Header("スコア表示用テキスト")]
    public Text scoreText; // InspectorでUIのTextを設定

    private static bool initialized = false; // 初期化済みかどうか

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ★最初の生成時にスコアをリセット
            if (!initialized)
            {
                score = 0;
                initialized = true;
            }

            // シーン切り替えイベント登録
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンでスコアTextを探し直す
        if (scoreText == null)
        {
            // "ScoreText" タグをUIのTextにつけておくと自動で取得可能
            GameObject obj = GameObject.FindWithTag("ScoreText");
            if (obj != null)
            {
                scoreText = obj.GetComponent<Text>();
            }
        }
        UpdateScoreText();
    }

    private void Start()
    {
        UpdateScoreText();
    }

    // スコア加算
    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateScoreText();
    }

    // スコアを保存（リザルト画面用）
    public void SaveScore()
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }

    // UIテキスト更新
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "切った数：" + score + "枚";
        }
    }

    // 任意でスコアをリセットしたいとき用
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}
