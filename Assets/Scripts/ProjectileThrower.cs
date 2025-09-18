using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour
{
    [Header("������Ώ�")]
    public GameObject projectilePrefab;   // ������A�C�e����Prefab

    [Header("�o�����W�ݒ�")]
    public Vector3 spawnCenter = new Vector3(0f, 3f, 6f); // �o�����S���W
    public Vector3 spawnRange = new Vector3(5f, 0f, 5f);  // X,Y�̃����_�����iZ�͌Œ�ŉ��ɓ�����j

    [Header("������ݒ�")]
    public float throwSpeed = 10f;        // ���ˏ����x
    public float throwAngle = 45f;        // ������p�x�i�x���@�j
    public float interval = 3f;           // ������Ԋu�i�b�j

    private void Start()
    {
        // ���������J�n
        StartCoroutine(ThrowLoop());
    }

    private IEnumerator ThrowLoop()
    {
        while (true)
        {
            Throw();
            yield return new WaitForSeconds(interval);
        }
    }

    private void Throw()
    {
        if (projectilePrefab == null) return;

        // �����_����X,Y�̂����t����
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRange.x, spawnRange.x),
            Random.Range(-spawnRange.y, spawnRange.y),
            0f // Z�͌Œ�
        );

        Vector3 spawnPos = spawnCenter + randomOffset;

        // �A�C�e���𐶐�
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Rigidbody���擾�i�Ȃ���Βǉ��j
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null) rb = projectile.AddComponent<Rigidbody>();

        // ����������i���[���h�� -Z �����֊p�x��t����j
        Vector3 dir = Vector3.back; // (0,0,-1)
        Quaternion rot = Quaternion.AngleAxis(throwAngle, Vector3.right);
        Vector3 throwDir = rot * dir;

        // ���x�𒼐ڗ^����i�˖@���ˁj
        rb.velocity = throwDir.normalized * throwSpeed;
    }
}
