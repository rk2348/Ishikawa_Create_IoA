using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpeedThresholdAudio : MonoBehaviour
{
    public float speedThreshold = 1f;      // この速度を超えたら音を再生
    public AudioClip audioClip;            // Inspectorで設定する音声

    private AudioSource audioSource;
    private Vector3 lastPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        lastPosition = transform.position;
    }

    void Update()
    {
        // 移動速度を計算
        Vector3 displacement = transform.position - lastPosition;
        float speed = displacement.magnitude / Time.deltaTime;

        // 速度が閾値を超えたら音を再生（再生中でなければ）
        if (speed > speedThreshold && !audioSource.isPlaying && audioClip != null)
        {
            audioSource.Play();
        }

        lastPosition = transform.position;
    }
}
