using UnityEngine;
using UnityEngine.Events;

public class FakeSlice : MonoBehaviour
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
    public AudioClip sliceSound;
    public float sliceVolume = 1f;

    private MeshRenderer meshRenderer;
    private Collider objectCollider;
    private bool sliced = false;

    // UnityEvent はオプションとして残しておく（他のイベントをつけたい場合用）
    [SerializeField] private UnityEvent OnScore;

    // 自動参照用
    private ScoreManager scoreManager;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objectCollider = GetComponent<Collider>();

        if (meshRenderer != null) meshRenderer.enabled = true;
        if (objectCollider != null) objectCollider.enabled = true;

        // シーン内から ScoreManager を自動検索
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager がシーン内に見つかりません。スコア加算は行われません。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sliced) return;
        if (!collision.gameObject.CompareTag(bladeTag)) return;

        sliced = true;

        Vector3 bladeDirection = collision.relativeVelocity.normalized;

        Quaternion rotationA = Quaternion.Euler(bladeDirection * rotationAngle);
        Quaternion rotationB = Quaternion.Euler(-bladeDirection * rotationAngle);

        if (slicedPartAPrefab != null)
        {
            GameObject partA = Instantiate(slicedPartAPrefab, transform.position, transform.rotation * rotationA);
            Rigidbody rbA = partA.AddComponent<Rigidbody>();
            rbA.AddForce((bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        if (slicedPartBPrefab != null)
        {
            GameObject partB = Instantiate(slicedPartBPrefab, transform.position, transform.rotation * rotationB);
            Rigidbody rbB = partB.AddComponent<Rigidbody>();
            rbB.AddForce((-bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        if (sliceSound != null)
        {
            GameObject audioObject = new GameObject("SliceSound");
            audioObject.transform.position = transform.position;
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = sliceSound;
            audioSource.volume = sliceVolume;
            audioSource.Play();
            Destroy(audioObject, sliceSound.length);
        }

        if (meshRenderer != null) meshRenderer.enabled = false;
        if (objectCollider != null) objectCollider.enabled = false;

        // スコア加算を自動呼び出し
        scoreManager?.Water();

        // UnityEventも併用できるように残す
        OnScore?.Invoke();
    }
}
