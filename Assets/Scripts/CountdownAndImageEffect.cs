using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CountdownAndImageEffect : MonoBehaviour
{
    [Header("UI設定")]
    public TMP_Text countdownText;   // カウントダウン表示用 (TextMeshPro)
    public Image targetImage;        // 演出するImage

    [Header("カウントダウン設定")]
    public float countdownTime = 3f; // カウントダウン秒数 (例: 3秒前から)

    [Header("画像演出設定")]
    public float fadeDuration = 1f;  // 画像が濃くなるまでの時間
    public float holdDuration = 1f;  // フルで表示しておく時間
    public Vector3 startScale = new Vector3(2f, 2f, 2f); // 開始時の拡大率
    public Vector3 endScale = Vector3.one;               // 最終サイズ

    [Header("SE設定")]
    public AudioSource audioSource;  // 再生用AudioSource
    public AudioClip seClip;         // 再生するSEクリップ

    private void Start()
    {
        // 最初は透明＆拡大、ただしアクティブは維持
        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = 0;
            targetImage.color = c;
            targetImage.transform.localScale = startScale;
            targetImage.gameObject.SetActive(true);
        }

        // カウントダウン開始
        StartCoroutine(CountdownAndShowImage());
    }

    private IEnumerator CountdownAndShowImage()
    {
        // カウントダウン表示
        int count = Mathf.CeilToInt(countdownTime);
        while (count > 0)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();

            yield return new WaitForSeconds(1f);
            count--;
        }

        // カウント終了 → テキスト消す
        if (countdownText != null)
            countdownText.text = "";

        // 画像フェードイン＋縮小アニメーション
        if (targetImage != null)
        {
            float t = 0f;
            Color c = targetImage.color;
            bool sePlayed = false; // SEを一度だけ鳴らすフラグ

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / fadeDuration);

                // α値（透明度）を上げる
                c.a = normalized;
                targetImage.color = c;

                // スケールを小さくしていく
                targetImage.transform.localScale = Vector3.Lerp(startScale, endScale, normalized);

                // 完全に濃くなった瞬間にSE再生（1回だけ）
                if (!sePlayed && Mathf.Approximately(normalized, 1f))
                {
                    if (audioSource != null && seClip != null)
                        audioSource.PlayOneShot(seClip);

                    sePlayed = true;
                }

                yield return null;
            }

            // 画像をフル表示で1秒キープ
            yield return new WaitForSeconds(holdDuration);

            // 消す
            targetImage.gameObject.SetActive(false);
        }
    }
}
