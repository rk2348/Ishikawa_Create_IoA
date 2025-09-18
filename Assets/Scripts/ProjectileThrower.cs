using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour
{
    [Header("投げる対象")]
    public GameObject projectilePrefab;   // 投げるアイテムのPrefab

    [Header("出現座標設定")]
    public Vector3 spawnCenter = new Vector3(0f, 3f, 6f); // 出現中心座標
    public Vector3 spawnRange = new Vector3(5f, 0f, 5f);  // X,Yのランダム幅（Zは固定で奥に投げる）

    [Header("投げる設定")]
    public float throwSpeed = 10f;        // 投射初速度
    public float throwAngle = 45f;        // 投げる角度（度数法）
    public float interval = 3f;           // 投げる間隔（秒）

    private void Start()
    {
        // 自動投げ開始
        StartCoroutine(ThrowLoop());
    }

    private IEnumerator ThrowLoop()
    {
        while (true)
        {
            Throw();
            yield return new WaitForSeconds(interval);
        }
    }

    private void Throw()
    {
        if (projectilePrefab == null) return;

        // ランダムなX,Yのずれを付ける
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRange.x, spawnRange.x),
            Random.Range(-spawnRange.y, spawnRange.y),
            0f // Zは固定
        );

        Vector3 spawnPos = spawnCenter + randomOffset;

        // アイテムを生成
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Rigidbodyを取得（なければ追加）
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null) rb = projectile.AddComponent<Rigidbody>();

        // 投げる方向（ワールドの -Z 方向へ角度を付ける）
        Vector3 dir = Vector3.back; // (0,0,-1)
        Quaternion rot = Quaternion.AngleAxis(throwAngle, Vector3.right);
        Vector3 throwDir = rot * dir;

        // 速度を直接与える（射法投射）
        rb.velocity = throwDir.normalized * throwSpeed;
    }
}
