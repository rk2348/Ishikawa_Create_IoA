using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    public Text scoreText;

    public TextMeshProUGUI Text;

    private void Awake()
    {
        if (scoreText != null) return;
        if (Text != null) return;
    }

    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("Score", 0);

        if (scoreText != null)
        {
            scoreText.text = "êÿÇ¡ÇΩêî : " + savedScore.ToString() + "ñá";
        }

        if (Text != null)
        {
            Text.text = savedScore.ToString();
        }
    }
}
