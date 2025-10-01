using UnityEngine;

public class SwordFireEffect : MonoBehaviour
{
    [Header("設定")]
    public float speedThreshold = 1f;          // この速度を超えたら火のエフェクトを出す
    public ParticleSystem fireEffectPrefab;    // Inspectorで設定する火のパーティクル
    public Transform effectSpawnPoint;         // 火を出したい位置（刀の先端など）

    private ParticleSystem currentEffect;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        if (effectSpawnPoint == null)
        {
            // 指定がなければ刀の位置に設定
            effectSpawnPoint = transform;
        }
    }

    void Update()
    {
        // 刀の移動速度を計算
        Vector3 displacement = transform.position - lastPosition;
        float speed = displacement.magnitude / Time.deltaTime;

        // 速度が閾値を超えたら火を出す
        if (speed > speedThreshold)
        {
            if (currentEffect == null)
            {
                // 火のエフェクト生成
                currentEffect = Instantiate(fireEffectPrefab, effectSpawnPoint.position, effectSpawnPoint.rotation, effectSpawnPoint);
                currentEffect.Play();
            }
        }
        else
        {
            // 速度が閾値以下になったら火を止める
            if (currentEffect != null)
            {
                currentEffect.Stop();
                Destroy(currentEffect.gameObject, currentEffect.main.startLifetime.constantMax);
                currentEffect = null;
            }
        }

        lastPosition = transform.position;
    }
}
