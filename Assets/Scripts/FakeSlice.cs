using UnityEngine;

public class PlayerBladeSlice : MonoBehaviour
{
    [Header("�ؒf��̃v���n�u")]
    public GameObject slicedPartAPrefab;
    public GameObject slicedPartBPrefab;

    [Header("Blade�̃^�O")]
    public string bladeTag = "Blade";

    [Header("�����p�[�c�ɉ������")]
    public float forceMultiplier = 2f;
    public float upwardForce = 0.5f;

    [Header("�����p�[�c�̉�]")]
    public float rotationAngle = 30f;

    [Header("�ؒf��")]
    public AudioClip sliceSound; // Inspector�Őݒ�
    public float sliceVolume = 1f;

    private MeshRenderer meshRenderer;
    private Collider objectCollider;
    private bool sliced = false; // ��x�����؂�t���O

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objectCollider = GetComponent<Collider>();

        if (meshRenderer != null) meshRenderer.enabled = true;
        if (objectCollider != null) objectCollider.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sliced) return;
        if (!collision.gameObject.CompareTag(bladeTag)) return;

        sliced = true;

        // Blade �̐i�s�������擾
        Vector3 bladeDirection = collision.relativeVelocity.normalized;

        // �ؒf�p�[�c�̉�]���v�Z
        Quaternion rotationA = Quaternion.Euler(bladeDirection * rotationAngle);
        Quaternion rotationB = Quaternion.Euler(-bladeDirection * rotationAngle);

        // �p�[�cA����
        if (slicedPartAPrefab != null)
        {
            GameObject partA = Instantiate(slicedPartAPrefab, transform.position, transform.rotation * rotationA);
            Rigidbody rbA = partA.AddComponent<Rigidbody>();
            rbA.AddForce((bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        // �p�[�cB����
        if (slicedPartBPrefab != null)
        {
            GameObject partB = Instantiate(slicedPartBPrefab, transform.position, transform.rotation * rotationB);
            Rigidbody rbB = partB.AddComponent<Rigidbody>();
            rbB.AddForce((-bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        // �ؒf�����Đ��i�Ō�܂Ŗ炷�j
        if (sliceSound != null)
        {
            GameObject audioObject = new GameObject("SliceSound");
            audioObject.transform.position = transform.position;
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = sliceSound;
            audioSource.volume = sliceVolume;
            audioSource.Play();
            Destroy(audioObject, sliceSound.length); // �Đ��I����ɔj��
        }

        // ���I�u�W�F�N�g��\��
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (objectCollider != null) objectCollider.enabled = false;
    }
}
