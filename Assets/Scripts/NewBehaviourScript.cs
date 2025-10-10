using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public static class GameScoreStatic
    {
        public static int Score = 0;
    }

    public class ScoreChanger
    {
        public void ScorePlusOne()
        {
            GameScoreStatic.Score++;
        }
    }

    public class ScoreGetter
    {
        public int GetScore()
        {
            return GameScoreStatic.Score;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
