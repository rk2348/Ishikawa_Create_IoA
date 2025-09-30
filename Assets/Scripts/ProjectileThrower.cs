using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour
{
    [Header("投げる対象")]
    public GameObject projectilePrefab;

    [Header("出現座標設定")]
    public Vector3 spawnCenter = new Vector3(0f, 3f, 6f);
    public Vector3 spawnRange = new Vector3(5f, 0f, 5f);

    [Header("投げる設定")]
    public float throwSpeed = 10f;
    public float throwAngle = 45f;
    public float interval = 4f;

    [Header("開始ディレイ設定")]
    public float startDelay = 3f;

    [HideInInspector]
    public bool isActive = true; // 停止用フラグ

    private void Start()
    {
        StartCoroutine(StartWithDelay());
    }

    private IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        StartCoroutine(ThrowLoop());
        StartCoroutine(SpeedUpLoop());
    }

    private IEnumerator ThrowLoop()
    {
        while (true)
        {
            if (isActive)
                Throw();
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator SpeedUpLoop()
    {
        while (true)
        {
            if (isActive)
            {
                yield return new WaitForSeconds(10f);
                interval /= 1.5f;
                Debug.Log("スピードアップ！ 現在の間隔: " + interval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Throw()
    {
        if (projectilePrefab == null) return;

        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRange.x, spawnRange.x),
            Random.Range(-spawnRange.y, spawnRange.y),
            0f
        );

        Vector3 spawnPos = spawnCenter + randomOffset;

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null) rb = projectile.AddComponent<Rigidbody>();

        Vector3 dir = Vector3.back;
        Quaternion rot = Quaternion.AngleAxis(throwAngle, Vector3.right);
        Vector3 throwDir = rot * dir;

        rb.velocity = throwDir.normalized * throwSpeed;
    }
}
