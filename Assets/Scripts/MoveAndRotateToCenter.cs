using UnityEngine;

public class MoveAndRotateToCenter : MonoBehaviour
{
    public float moveSpeed = 5f;      // 移動速度
    public float rotationSpeed = 180f; // 回転速度（度/秒）
    private Vector3 targetPosition;    // 目的地（画面中央）

    void Start()
    {
        // 画面左端に初期配置
        Vector3 leftScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 10f));
        transform.position = leftScreen;

        // 目的地は画面中央
        targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
    }

    void Update()
    {
        // 中央まで移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 回転
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // 中央に到達したら止める
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition; // 正確に中央で止める
            enabled = false; // スクリプトを停止
        }
    }
}
