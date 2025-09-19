using UnityEngine;

public class SpeedEffectController3Levels : MonoBehaviour
{
    [Header("�G�t�F�N�g�ݒ�")]
    public ParticleSystem midEffect;    // �����p�G�t�F�N�g
    public ParticleSystem fastEffect;   // �����p�G�t�F�N�g

    [Header("���x�̂������l")]
    public float midThreshold = 5f;   // �G�t�F�N�g�����J�n�i�����j
    public float fastThreshold = 10f; // �����ɂȂ鑬�x

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
            // 5�����Ȃ�S���I�t
            StopAllEffects();
        }
        else if (speed < fastThreshold)
        {
            // �����G�t�F�N�g
            PlayEffect(midEffect);
            Debug.Log("�����ł�");
        }
        else
        {
            // �����G�t�F�N�g
            PlayEffect(fastEffect);
            Debug.Log("�����ł�");
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
