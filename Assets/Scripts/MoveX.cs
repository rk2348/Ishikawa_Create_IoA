using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveX : MonoBehaviour
{
    [SerializeField] private float speed = 1f; // インスペクターで設定可能な速さ
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 物理的に回転しないように固定（必要なら）
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // x軸方向に常に一定速度で移動
        rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
    }
}
