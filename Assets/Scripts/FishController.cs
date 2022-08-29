using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    //물고기 속성
    public float swimSpeed = 1;
    public int fishScore = 10;
    private int swimDirection = 1;

    //참조 오브젝트 및 스크립트
    private GameObject hook;
    private GameObject line;
    private GameObject gameManager;
    private FishingRodController fishingRodController;
    private ScoreManager scoreManager;
    
    //상태
    private bool isHooked;

    //보조 변수
    private Vector2 hookPositionOffset;

    // Start is called before the first frame update
    void Start()
    {
        isHooked = false;
        hook = GameObject.Find("Hook");
        line = GameObject.Find("Line");
        gameManager = GameObject.Find("GameManager");
        fishingRodController = line.GetComponent<FishingRodController>();
        scoreManager = gameManager.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHooked)
        {
            //낚인 경우 낚싯대를 따라 이동
            transform.position = hook.transform.TransformPoint(hookPositionOffset);

            if (fishingRodController.gotFishFlag)
            {
                scoreManager.addScore(fishScore);
                this.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        //물고기 이동
        Vector2 position = transform.position;
        position.x = position.x + swimSpeed * swimDirection * Time.deltaTime;
        transform.position = position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //물고기 잡힘
        if (!fishingRodController.isFishCaughtFlag)
        {
            isHooked = true;
            fishingRodController.SetFishCaughtTrue();

            hookPositionOffset = transform.position - collision.gameObject.transform.position;

        }
    }
   
}
