using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    public Text scoreText;

    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("Score", 0);
        scoreText.text = "êÿÇ¡ÇΩêî : " + savedScore.ToString();
    }
}
