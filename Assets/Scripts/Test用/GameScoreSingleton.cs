// "GameScene"でGameObjectにアタッチされてる想定
using UnityEngine;

public sealed class GameScoreSingleton : MonoBehaviour
{
    private static GameScoreSingleton instance;
    public static GameScoreSingleton Instance => instance;

    public int Score = 0;

    private void Awake()
    {
        // instanceがすでにあったら自分を消去する。
        if (instance && this != instance)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        // Scene遷移で破棄されなようにする。      
        DontDestroyOnLoad(this);
    }
}

// "GameScene"側
public class ScoreChanger
{
    public void ScorePlusOne()
    {
        GameScoreSingleton.Instance.Score++;
    }
}

// "ResultScene"側
public class ScoreGetter
{
    public int GetScore()
    {
        return GameScoreSingleton.Instance.Score;
    }
}
