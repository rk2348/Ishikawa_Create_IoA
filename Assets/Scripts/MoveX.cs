using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveX : MonoBehaviour
{
    [SerializeField] private float speed = 1f; // �C���X�y�N�^�[�Őݒ�\�ȑ���
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // �����I�ɉ�]���Ȃ��悤�ɌŒ�i�K�v�Ȃ�j
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // x�������ɏ�Ɉ�葬�x�ňړ�
        rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
    }
}
