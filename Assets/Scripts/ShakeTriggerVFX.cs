using UnityEngine;
using UnityEngine.XR;

public class ShakeTriggerVFX : MonoBehaviour
{
    public GameObject effectPrefab;
    public float velocityThreshold = 1.0f;
    public float cooldown = 0.25f;
    public XRNode controllerNode = XRNode.RightHand; // コントローラ選択

    private float lastPlayTime = -999f;
    private ParticleSystem ps;
    private GameObject pooledEffect;

    void Start()
    {
        if (effectPrefab != null)
        {
            pooledEffect = Instantiate(effectPrefab, transform);
            pooledEffect.SetActive(true);
            ps = pooledEffect.GetComponentInChildren<ParticleSystem>();
            pooledEffect.SetActive(false);
        }
    }

    void Update()
    {
        if (Time.time - lastPlayTime < cooldown) return;

        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
        {
            if (velocity.magnitude >= velocityThreshold)
            {
                PlayEffect();
                lastPlayTime = Time.time;
            }
        }
    }

    void PlayEffect()
    {
        if (pooledEffect != null && ps != null)
        {
            if (!pooledEffect.activeSelf)
                pooledEffect.SetActive(true);

            // 一度クリアする
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play(true);
        }
    }


}
