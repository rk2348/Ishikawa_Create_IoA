using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // �V���O���g����
    public int score = 0;

    [Header("�X�R�A�\���p�e�L�X�g")]
    public Text scoreText; // Inspector��UI��Text��ݒ�

    private void Awake()
    {
        // �V���O���g���ݒ�
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

    // �X�R�A���Z���\�b�h
    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateScoreText();
    }

    // Text���X�V���郁�\�b�h
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "�؂������F"+score+"��";
        }
    }
}
