using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSlice : MonoBehaviour
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

    [Header("シーン移動")]
    public string nextSceneName;   // 遷移先シーン
    public float sceneDelay = 3f;  // 3秒後にロード

    [Header("ロードUI")]
    public GameObject loadingUI;   // InspectorでCanvas等を設定
    public Slider slider;          // InspectorでSliderを設定

    private MeshRenderer meshRenderer;
    private Collider objectCollider;
    private bool sliced = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objectCollider = GetComponent<Collider>();

        if (meshRenderer != null) meshRenderer.enabled = true;
        if (objectCollider != null) objectCollider.enabled = true;

        if (loadingUI != null) loadingUI.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sliced) return;
        if (!collision.gameObject.CompareTag(bladeTag)) return;

        sliced = true;

        Vector3 bladeDirection = collision.relativeVelocity.normalized;

        // 切断パーツの回転
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

        // 切断音
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

        // スコア加算
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        // 元オブジェクト非表示
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (objectCollider != null) objectCollider.enabled = false;

        // 3秒後にロード開始
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneName, sceneDelay));
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float minDelay)
    {
        float elapsed = 0f;

        // ロードUIを表示
        if (loadingUI != null)
            loadingUI.SetActive(true);

        // シーン非同期ロード開始
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; // 自動切り替えを停止

        if (slider != null)
            slider.value = 0f;

        // ロード中と最小待機時間の両方を待つ
        while (!async.isDone || elapsed < minDelay)
        {
            elapsed += Time.deltaTime;

            // スライダー更新（ロード進捗と最低時間の進行度の最大値を表示）
            if (slider != null)
            {
                float progress = Mathf.Clamp01(async.progress / 0.9f); // async.progressは0.9で止まる
                slider.value = Mathf.Max(progress, elapsed / minDelay);
            }

            // 3秒以上経ってロードも完了していれば終了
            if (async.progress >= 0.9f && elapsed >= minDelay)
                break;

            yield return null;
        }

        // ロード完了を許可
        async.allowSceneActivation = true;

        // スライダーを確実に100%に
        if (slider != null)
            slider.value = 1f;
    }

}
