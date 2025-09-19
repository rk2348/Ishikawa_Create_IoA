using UnityEngine;

public class PlayerBladeSlice : MonoBehaviour
{
    [Header("切断後のプレハブ")]
    public GameObject slicedPartAPrefab;
    public GameObject slicedPartBPrefab;

    [Header("Bladeのタグ")]
    public string bladeTag = "Blade";

    [Header("生成パーツに加える力")]
    public float forceMultiplier = 2f;
    public float upwardForce = 0.5f;

    [Header("生成パーツの回転")]
    public float rotationAngle = 30f;

    [Header("切断音")]
    public AudioClip sliceSound; // Inspectorで設定
    public float sliceVolume = 1f;

    private MeshRenderer meshRenderer;
    private Collider objectCollider;
    private bool sliced = false; // 一度だけ切るフラグ

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

        // Blade の進行方向を取得
        Vector3 bladeDirection = collision.relativeVelocity.normalized;

        // 切断パーツの回転を計算
        Quaternion rotationA = Quaternion.Euler(bladeDirection * rotationAngle);
        Quaternion rotationB = Quaternion.Euler(-bladeDirection * rotationAngle);

        // パーツA生成
        if (slicedPartAPrefab != null)
        {
            GameObject partA = Instantiate(slicedPartAPrefab, transform.position, transform.rotation * rotationA);
            Rigidbody rbA = partA.AddComponent<Rigidbody>();
            rbA.AddForce((bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        // パーツB生成
        if (slicedPartBPrefab != null)
        {
            GameObject partB = Instantiate(slicedPartBPrefab, transform.position, transform.rotation * rotationB);
            Rigidbody rbB = partB.AddComponent<Rigidbody>();
            rbB.AddForce((-bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        // 切断音を再生（最後まで鳴らす）
        if (sliceSound != null)
        {
            GameObject audioObject = new GameObject("SliceSound");
            audioObject.transform.position = transform.position;
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = sliceSound;
            audioSource.volume = sliceVolume;
            audioSource.Play();
            Destroy(audioObject, sliceSound.length); // 再生終了後に破棄
        }

        // 元オブジェクト非表示
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (objectCollider != null) objectCollider.enabled = false;
    }
}
