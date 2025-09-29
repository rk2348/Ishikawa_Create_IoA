using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // ← IEnumerator を使うため必要

public class GroundHitSceneChanger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // 遷移先のシーン名
    public string groundTag = "Ground";        // 地面オブジェクトのタグ
    private int hitCount = 0;

    public Image targetImage;        // 演出するImage

    [Header("画像演出設定")]
    public float fadeDuration = 1f;  // 画像が濃くなるまでの時間
    public float holdDuration = 1f;  // フルで表示しておく時間
    public Vector3 startScale = new Vector3(2f, 2f, 2f); // 開始時の拡大率
    public Vector3 endScale = Vector3.one;               // 最終サイズ

    [Header("SE設定")]
    public AudioSource audioSource;  // 再生用AudioSource
    public AudioClip seClip;         // 再生するSEクリップ

    private bool hasStarted = false; // コルーチン開始済みフラグ

    private void Start()
    {
        Color c = targetImage.color;
        c.a = 0;
        targetImage.color = c;
        targetImage.transform.localScale = startScale;
        targetImage.gameObject.SetActive(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            hitCount++;

            if (!hasStarted && hitCount >= 3)
            {
                hasStarted = true; // 一度だけ実行
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

            // フル表示で保持
            yield return new WaitForSeconds(holdDuration);

            // ここでさらに1秒待つ
            yield return new WaitForSeconds(1f);

            targetImage.gameObject.SetActive(false);
        }

        // 演出終了後にシーン遷移
        SceneManager.LoadScene(nextSceneName);
    }


}
