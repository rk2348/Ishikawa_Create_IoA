using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ← IEnumerator を使うため必要

public class GroundHitSceneChanger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // 遷移先のシーン名
    public string groundTag = "Ground";        // 地面オブジェクトのタグ
    private int hitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            hitCount++;
            Debug.Log("地面に着地: " + hitCount + "回");

            if (hitCount >= 3)
            {
                // コルーチンを呼ぶ
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
