using UnityEngine;

public class ControllerSpeedEffect : MonoBehaviour
{
    [Header("�G�t�F�N�g�ݒ�")]
    public ParticleSystem midEffect;    // �����p�G�t�F�N�g
    public ParticleSystem fastEffect;   // �����p�G�t�F�N�g

    [Header("���x�̂������l (m/s)")]
    public float midThreshold = 1.0f;   // �����J�n���x
    public float fastThreshold = 2.0f;  // �����J�n���x

    [Header("�ΏۃR���g���[���[")]
    public OVRInput.Controller targetController = OVRInput.Controller.RTouch;

    void Update()
    {
        // �R���g���[���[�̑��x���擾
        Vector3 velocity = OVRInput.GetLocalControllerVelocity(targetController);
        float speed = velocity.magnitude;

        // ���x�ɉ����ăG�t�F�N�g����
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

        // ���̃X�N���v�g���A�^�b�`�����I�u�W�F�N�g���R���g���[���[�ɒǏ]������
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
