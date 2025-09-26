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
        Debug.Log("衝突: " + collision.gameObject.name + " (tag: " + collision.gameObject.tag + ")");

        if (collision.gameObject.CompareTag(groundTag))
        {
            hitCount++;
            Debug.Log("地面に着地: " + hitCount + "回");

            if (hitCount >= 3)
            {
                Debug.Log("シーン遷移を開始します: " + nextSceneName);
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }


    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }
}
