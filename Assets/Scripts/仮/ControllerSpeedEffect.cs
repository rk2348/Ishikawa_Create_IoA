using UnityEngine;

public class ControllerSpeedEffect : MonoBehaviour
{
    [Header("エフェクト設定")]
    public ParticleSystem midEffect;    // 中速用エフェクト
    public ParticleSystem fastEffect;   // 高速用エフェクト

    [Header("速度のしきい値 (m/s)")]
    public float midThreshold = 1.0f;   // 中速開始速度
    public float fastThreshold = 2.0f;  // 高速開始速度

    [Header("対象コントローラー")]
    public OVRInput.Controller targetController = OVRInput.Controller.RTouch;

    void Update()
    {
        // コントローラーの速度を取得
        Vector3 velocity = OVRInput.GetLocalControllerVelocity(targetController);
        float speed = velocity.magnitude;

        // 速度に応じてエフェクト制御
        if (speed < midThreshold)
        {
            StopAllEffects();
        }
        else if (speed < fastThreshold)
        {
            PlayEffect(midEffect);
            StopEffect(fastEffect);
        }
        else
        {
            PlayEffect(midEffect);
            PlayEffect(fastEffect);
        }

        // このスクリプトをアタッチしたオブジェクトをコントローラーに追従させる
        transform.localPosition = OVRInput.GetLocalControllerPosition(targetController);
        transform.localRotation = OVRInput.GetLocalControllerRotation(targetController);
    }

    private void PlayEffect(ParticleSystem effect)
    {
        if (effect != null && !effect.isPlaying)
            effect.Play();
    }

    private void StopEffect(ParticleSystem effect)
    {
        if (effect != null && effect.isPlaying)
            effect.Stop();
    }

    private void StopAllEffects()
    {
        StopEffect(midEffect);
        StopEffect(fastEffect);
    }
}
