using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text scoreText;

    private int score;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //점수 설정
        scoreText.text = score.ToString();
    }
    
    public void addScore(int amount)
    {
        score += amount;
    }
}
