using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSlice : MonoBehaviour
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
    public AudioClip sliceSound;
    public float sliceVolume = 1f;

    [Header("�V�[���ړ�")]
    public string nextSceneName;   // �J�ڐ�V�[��
    public float sceneDelay = 3f;  // 3�b��Ƀ��[�h

    [Header("���[�hUI")]
    public GameObject loadingUI;   // Inspector��Canvas����ݒ�
    public Slider slider;          // Inspector��Slider��ݒ�

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

        // �ؒf�p�[�c�̉�]
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

        // �ؒf��
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

        // �X�R�A���Z
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        // ���I�u�W�F�N�g��\��
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (objectCollider != null) objectCollider.enabled = false;

        // 3�b��Ƀ��[�h�J�n
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneName, sceneDelay));
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float minDelay)
    {
        float elapsed = 0f;

        // ���[�hUI��\��
        if (loadingUI != null)
            loadingUI.SetActive(true);

        // �V�[���񓯊����[�h�J�n
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; // �����؂�ւ����~

        if (slider != null)
            slider.value = 0f;

        // ���[�h���ƍŏ��ҋ@���Ԃ̗�����҂�
        while (!async.isDone || elapsed < minDelay)
        {
            elapsed += Time.deltaTime;

            // �X���C�_�[�X�V�i���[�h�i���ƍŒ᎞�Ԃ̐i�s�x�̍ő�l��\���j
            if (slider != null)
            {
                float progress = Mathf.Clamp01(async.progress / 0.9f); // async.progress��0.9�Ŏ~�܂�
                slider.value = Mathf.Max(progress, elapsed / minDelay);
            }

            // 3�b�ȏ�o���ă��[�h���������Ă���ΏI��
            if (async.progress >= 0.9f && elapsed >= minDelay)
                break;

            yield return null;
        }

        // ���[�h����������
        async.allowSceneActivation = true;

        // �X���C�_�[���m����100%��
        if (slider != null)
            slider.value = 1f;
    }

}
