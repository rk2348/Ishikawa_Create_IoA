using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GroundHitSceneChanger : MonoBehaviour
{
    public string nextSceneName = "NextScene";
    public string groundTag = "Ground";
    private int hitCount = 0;

    public Image targetImage;
    [Header("画像演出設定")]
    public float fadeDuration = 1f;
    public float holdDuration = 1f;
    public Vector3 startScale = new Vector3(2f, 2f, 2f);
    public Vector3 endScale = Vector3.one;

    [Header("SE設定")]
    public AudioSource audioSource;
    public AudioClip seClip;

    private bool hasStarted = false;

    private void Start()
    {
        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = 0;
            targetImage.color = c;
            targetImage.transform.localScale = startScale;
            targetImage.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            hitCount++;

            if (!hasStarted && hitCount >= 3)
            {
                hasStarted = true;
                StartCoroutine(ShowImageAndLoadScene());
            }
        }
    }

    private IEnumerator ShowImageAndLoadScene()
    {
        if (targetImage != null)
        {
            float t = 0f;
            Color c = targetImage.color;
            bool sePlayed = false;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / fadeDuration);

                c.a = normalized;
                targetImage.color = c;

                targetImage.transform.localScale = Vector3.Lerp(startScale, endScale, normalized);

                if (!sePlayed && Mathf.Approximately(normalized, 1f))
                {
                    if (audioSource != null && seClip != null)
                        audioSource.PlayOneShot(seClip);
                    sePlayed = true;
                }

                yield return null;
            }

            yield return new WaitForSeconds(holdDuration);
            targetImage.gameObject.SetActive(false);
        }

        ChangeScene(nextSceneName);
    }

    public void ChangeScene(string sceneName)
    {
        // 現在のスコアを PlayerPrefs に保存してシーン遷移
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            PlayerPrefs.SetInt("Score", scoreManager.score);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene(sceneName);
    }

}
