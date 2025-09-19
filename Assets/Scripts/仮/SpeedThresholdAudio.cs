using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpeedThresholdAudio : MonoBehaviour
{
    public float speedThreshold = 1f;      // ���̑��x�𒴂����特���Đ�
    public AudioClip audioClip;            // Inspector�Őݒ肷�鉹��

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
        // �ړ����x���v�Z
        Vector3 displacement = transform.position - lastPosition;
        float speed = displacement.magnitude / Time.deltaTime;

        // ���x��臒l�𒴂����特���Đ��i�Đ����łȂ���΁j
        if (speed > speedThreshold && !audioSource.isPlaying && audioClip != null)
        {
            audioSource.Play();
        }

        lastPosition = transform.position;
    }
}
