using UnityEngine;

public class ShakeTriggerVFX : MonoBehaviour
{
    [Tooltip("再生するエフェクトのPrefab (ParticleSystem をルートに持つ)")]
    public GameObject effectPrefab;

    [Tooltip("振ったと判定する速度の閾値 (m/s)")]
    public float linearVelocityThreshold = 1.0f;

    [Tooltip("連続再生を防ぐ最小間隔 (秒)")]
    public float cooldown = 0.25f;

    [Tooltip("エフェクトを親のどこに置くか (true = 子として配置)")]
    public bool attachAsChild = true;

    GameObject pooledEffect;
    ParticleSystem pooledPs;
    Vector3 prevPos;
    float lastPlayTime = -999f;

    void Start()
    {
        prevPos = transform.position;

        if (effectPrefab != null && attachAsChild)
        {
            pooledEffect = Instantiate(effectPrefab, transform);
            pooledEffect.transform.localPosition = Vector3.zero;
            pooledEffect.transform.localRotation = Quaternion.identity;
            pooledEffect.transform.localScale = Vector3.one;

            // 一度アクティブにして ParticleSystem をキャッシュし非アクティブ化
            pooledEffect.SetActive(true);
            pooledPs = pooledEffect.GetComponentInChildren<ParticleSystem>();
            pooledEffect.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (Time.time - lastPlayTime < cooldown)
        {
            prevPos = transform.position;
            return;
        }

        Vector3 vel = (transform.position - prevPos) / Time.fixedDeltaTime;
        prevPos = transform.position;

        if (vel.magnitude >= linearVelocityThreshold)
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
            if (pooledPs != null)
            {
                pooledEffect.SetActive(true);
                pooledPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                pooledPs.Clear();
                pooledPs.Play();
            }
            else
            {
                pooledEffect.SetActive(true);
                var ps = pooledEffect.GetComponentInChildren<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Clear();
                    ps.Play();
                }
            }
        }
        else
        {
            var go = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            if (attachAsChild) go.transform.SetParent(transform, worldPositionStays: true);
            var ps = go.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.Clear();
                ps.Play();
            }
            Destroy(go, 5f);
        }
    }
}