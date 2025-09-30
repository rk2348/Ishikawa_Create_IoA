using UnityEngine;

public class ShakeTriggerVFX : MonoBehaviour
{
    [Tooltip("再生するエフェクトのPrefab (ParticleSystem をルートに持つ)")]
    public GameObject effectPrefab;

    [Tooltip("振ったと判定する速度差の閾値 (m/s)")]
    public float linearVelocityThreshold = 1.0f;

    [Tooltip("振ったと判定する角速度の閾値 (rad/s)")]
    public float angularVelocityThreshold = 3.0f;

    [Tooltip("連続再生を防ぐ最小間隔 (秒)")]
    public float cooldown = 0.25f;

    [Tooltip("エフェクトを親のどこに置くか (true = 子として配置)")]
    public bool attachAsChild = true;

    Rigidbody rb;
    float lastPlayTime = -999f;
    GameObject pooledEffect;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>(); // 必要なら追加
    }

    void Start()
    {
        if (effectPrefab != null && attachAsChild)
        {
            pooledEffect = Instantiate(effectPrefab, transform);
            pooledEffect.SetActive(false);
        }
    }

    void Update()
    {
        if (Time.time - lastPlayTime < cooldown) return;

        // 線形速度の変化量で判定したい場合は別途差分を使うが、
        // シンプルに現在の速度/角速度で閾値判定する例:
        bool linearExceeded = rb.velocity.magnitude >= linearVelocityThreshold;
        bool angularExceeded = rb.angularVelocity.magnitude >= angularVelocityThreshold;

        if (linearExceeded || angularExceeded)
        {
            PlayEffect();
            lastPlayTime = Time.time;
        }
    }

    void PlayEffect()
    {
        if (effectPrefab == null) return;

        if (attachAsChild && pooledEffect != null)
        {
            pooledEffect.SetActive(false); // 再生が残っているならリセット
            var ps = pooledEffect.GetComponentInChildren<ParticleSystem>();
            if (ps != null) ps.Clear();
            pooledEffect.SetActive(true);
            if (ps != null) ps.Play();
        }
        else
        {
            var go = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            go.transform.SetParent(attachAsChild ? transform : null);
            var ps = go.GetComponentInChildren<ParticleSystem>();
            if (ps != null) ps.Play();
            Destroy(go, 5f); // 適切な寿命に合わせて調整
        }
    }
}