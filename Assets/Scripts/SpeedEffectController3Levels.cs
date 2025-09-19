using UnityEngine;

public class SpeedEffectController3Levels : MonoBehaviour
{
    [Header("エフェクト設定")]
    public ParticleSystem midEffect;    // 中速用エフェクト
    public ParticleSystem fastEffect;   // 高速用エフェクト

    [Header("速度のしきい値")]
    public float midThreshold = 5f;   // エフェクト発生開始（中速）
    public float fastThreshold = 10f; // 高速になる速度

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StopAllEffects();
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;

        if (speed < midThreshold)
        {
            // 5未満なら全部オフ
            StopAllEffects();
        }
        else if (speed < fastThreshold)
        {
            // 中速エフェクト
            PlayEffect(midEffect);
            Debug.Log("中速です");
        }
        else
        {
            // 高速エフェクト
            PlayEffect(fastEffect);
            Debug.Log("高速です");
        }
    }

    private void PlayEffect(ParticleSystem target)
    {
        if (midEffect != null)
        {
            if (midEffect == target && !midEffect.isPlaying) midEffect.Play();
            else if (midEffect != target && midEffect.isPlaying) midEffect.Stop();
        }

        if (fastEffect != null)
        {
            if (fastEffect == target && !fastEffect.isPlaying) fastEffect.Play();
            else if (fastEffect != target && fastEffect.isPlaying) fastEffect.Stop();
        }
    }

    private void StopAllEffects()
    {
        if (midEffect != null) midEffect.Stop();
        if (fastEffect != null) fastEffect.Stop();
    }
}
