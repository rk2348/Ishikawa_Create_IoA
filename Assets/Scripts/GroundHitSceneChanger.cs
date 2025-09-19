using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // �� IEnumerator ���g�����ߕK�v

public class GroundHitSceneChanger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // �J�ڐ�̃V�[����
    public string groundTag = "Ground";        // �n�ʃI�u�W�F�N�g�̃^�O
    private int hitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            hitCount++;
            Debug.Log("�n�ʂɒ��n: " + hitCount + "��");

            if (hitCount >= 3)
            {
                // �R���[�`�����Ă�
                StartCoroutine(LoadSceneAfterDelay(0));
            }
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }
}
