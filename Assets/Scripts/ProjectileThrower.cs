using UnityEngine;
using System.Collections;

public class MoveUIRight : MonoBehaviour
{
    public float speed = 600f;          // 移動速度（画面座標単位/秒）
    public float rotationSpeed = 360f;  // 回転速度（度/秒）
    public float popScale = 4f;       // 大きくする倍率
    public float popDuration = 0.4f;    // 大きくして元に戻す時間

    private RectTransform rectTransform;
    private bool hasPopped = false;     // 一度だけポップさせるフラグ

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 左から右に移動してx=0で止まる
        if (rectTransform.anchoredPosition.x < 0f)
        {
            rectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(0f, rectTransform.anchoredPosition.y);

            // 一度だけポップアニメーションを実行
            if (!hasPopped)
            {
                StartCoroutine(PopAnimation());
                hasPopped = true;
            }
        }

        // 回転（Z軸回転でUI回転）
        rectTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator PopAnimation()
    {
        Vector3 originalScale = rectTransform.localScale;
        Vector3 targetScale = originalScale * popScale;

        // 大きくする
        float timer = 0f;
        while (timer < popDuration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, timer / popDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = targetScale;

        // 元の大きさに戻す
        timer = 0f;
        while (timer < popDuration)
        {
            rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, timer / popDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = originalScale;
    }
}
