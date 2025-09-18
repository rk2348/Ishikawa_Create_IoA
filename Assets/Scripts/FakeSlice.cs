using UnityEngine;

public class FakeSlice : MonoBehaviour
{
    public GameObject slicedPartA;
    public GameObject slicedPartB;

    private void Start()
    {
        slicedPartA.SetActive(false);
        slicedPartB.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade"))
        {
            // 元のオブジェクトを非表示
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // 切断後のモデルを表示
            slicedPartA.SetActive(true);
            slicedPartB.SetActive(true);

            // 物理挙動を追加して自然に落下させる
            Rigidbody rbA = slicedPartA.AddComponent<Rigidbody>();
            Rigidbody rbB = slicedPartB.AddComponent<Rigidbody>();

            rbA.AddForce(Vector3.left * 2f, ForceMode.Impulse);
            rbB.AddForce(Vector3.right * 2f, ForceMode.Impulse);
        }
    }
}
