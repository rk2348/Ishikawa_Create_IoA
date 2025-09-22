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
    public float sceneDelay = 3f;  // 既存の遅延（必要なら使用）

    [Header("タイトルロゴ表示")]
    public GameObject titleLogo;    // Canvas上のロゴオブジェクト（Inspectorで設定）
    public float titleDuration = 2f; // ロゴ表示時間（秒）
    public float titleFadeTime = 0.5f; // フェード時間（0でフェード無し）

    [Header("ロードUI")]
    public GameObject loadingUI;   // InspectorでCanvas等を設定
    public Slider slider;          // InspectorでSliderを設定

    private MeshRenderer meshRenderer;
    private Collider objectCollider;
    private bool sliced = false;
    private bool isLoading = false; // 多重遷移防止フラグ

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objectCollider = GetComponent<Collider>();

        if (meshRenderer != null) meshRenderer.enabled = true;
        if (objectCollider != null) objectCollider.enabled = true;

        if (loadingUI != null) loadingUI.SetActive(false);
        if (titleLogo != null) titleLogo.SetActive(false);
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
            Rigidbody rbA = partA.GetComponent<Rigidbody>();
            if (rbA == null) rbA = partA.AddComponent<Rigidbody>();
            rbA.AddForce((bladeDirection + Vector3.up * upwardForce) * forceMultiplier, ForceMode.Impulse);
        }

        // パーツB生成
        if (slicedPartBPrefab != null)
        {
            GameObject partB = Instantiate(slicedPartBPrefab, transform.position, transform.rotation * rotationB);
            Rigidbody rbB = partB.GetComponent<Rigidbody>();
            if (rbB == null) rbB = partB.AddComponent<Rigidbody>();
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

        // ロード開始（多重防止）
        if (!string.IsNullOrEmpty(nextSceneName) && !isLoading)
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneName, sceneDelay));
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        isLoading = true;

        // 既存の任意遅延を使う場合
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        // --- タイトルロゴ表示（フェード対応） ---
        if (titleLogo != null)
        {
            CanvasGroup cg = titleLogo.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 0f;
            titleLogo.SetActive(true);

            // フェードイン
            if (cg != null && titleFadeTime > 0f)
            {
                float t = 0f;
                while (t < titleFadeTime)
                {
                    t += Time.deltaTime;
                    cg.alpha = Mathf.Clamp01(t / titleFadeTime);
                    yield return null;
                }
                cg.alpha = 1f;
            }

            // 表示時間
            yield return new WaitForSeconds(titleDuration);

            // フェードアウト
            if (cg != null && titleFadeTime > 0f)
            {
                float t = 0f;
                while (t < titleFadeTime)
                {
                    t += Time.deltaTime;
                    cg.alpha = Mathf.Clamp01(1f - (t / titleFadeTime));
                    yield return null;
                }
                cg.alpha = 0f;
            }

            titleLogo.SetActive(false);
        }

        // --- ロードUI を表示して非同期読み込み ---
        if (loadingUI != null) loadingUI.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = true;

        if (slider != null)
        {
            while (!async.isDone)
            {
                // async.progress は 0.0～0.9 の範囲。見た目で 0..1 に正規化する。
                float progress = Mathf.Clamp01(async.progress / 0.9f);
                slider.value = progress;
                yield return null;
            }
        }
        else
        {
            yield return async;
        }

        isLoading = false;
    }
}