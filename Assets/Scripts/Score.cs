using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ← 追加

public class Score : MonoBehaviour
{
    [SerializeField, Header("スコア表示テキスト")]
    public int score = 0;
    [SerializeField, Header("最終スコア表示テキスト")]
    public Text scoreText;

    public static Score Instance; // シングルトン化

    void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded; // シーンが切り替わった時に呼ばれるイベント登録
    }

    void Start()
    {
        if (ResulText == null) return;
        if (ScoreText == null) return;
    }

    void Update()
    {
        if (ResulText != null)
        {
            ResulText.text = "最終スコア：" + score.ToString() + "枚";
        }

        if (ScoreText != null)
        {
            ScoreText.text = "切った数: " + score.ToString() + "枚";
        }
    }

    // シーン切り替え時に呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title")
        {
            Clear();
        }
    }

    public void Clear()
    {
        score = 0;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // イベント解除
    }
}
