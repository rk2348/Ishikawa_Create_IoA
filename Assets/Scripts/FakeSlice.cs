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
            // ���̃I�u�W�F�N�g���\��
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // �ؒf��̃��f����\��
            slicedPartA.SetActive(true);
            slicedPartB.SetActive(true);

            // ����������ǉ����Ď��R�ɗ���������
            Rigidbody rbA = slicedPartA.AddComponent<Rigidbody>();
            Rigidbody rbB = slicedPartB.AddComponent<Rigidbody>();

            rbA.AddForce(Vector3.left * 2f, ForceMode.Impulse);
            rbB.AddForce(Vector3.right * 2f, ForceMode.Impulse);
        }
    }
}
