using UnityEngine;

public class SwordFireEffect : MonoBehaviour
{
    [Header("�ݒ�")]
    public float speedThreshold = 1f;          // ���̑��x�𒴂�����΂̃G�t�F�N�g���o��
    public ParticleSystem fireEffectPrefab;    // Inspector�Őݒ肷��΂̃p�[�e�B�N��
    public Transform effectSpawnPoint;         // �΂��o�������ʒu�i���̐�[�Ȃǁj

    private ParticleSystem currentEffect;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        if (effectSpawnPoint == null)
        {
            // �w�肪�Ȃ���Γ��̈ʒu�ɐݒ�
            effectSpawnPoint = transform;
        }
    }

    void Update()
    {
        // ���̈ړ����x���v�Z
        Vector3 displacement = transform.position - lastPosition;
        float speed = displacement.magnitude / Time.deltaTime;

        // ���x��臒l�𒴂�����΂��o��
        if (speed > speedThreshold)
        {
            if (currentEffect == null)
            {
                // �΂̃G�t�F�N�g����
                currentEffect = Instantiate(fireEffectPrefab, effectSpawnPoint.position, effectSpawnPoint.rotation, effectSpawnPoint);
                currentEffect.Play();
            }
        }
        else
        {
            // ���x��臒l�ȉ��ɂȂ�����΂��~�߂�
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
