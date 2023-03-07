using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    //물고기 속성
    public float fishExp;
    public float swimSpeed;
    public int swimDirection;

    //참조 오브젝트 및 스크립트
    private GameObject hook;
    private GameObject line;
    private GameObject gameManager;
    private FishingRodController fishingRodController;
    private ExpManager expManager;

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
        expManager = gameManager.GetComponent<ExpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHooked) //낚인 경우
        {
            //갈고리를 따라 이동
            transform.position = hook.transform.TransformPoint(hookPositionOffset);

            //갈고리 도달 시 소멸
            if (fishingRodController.gotFishFlag)
            {
                expManager.addExp(fishExp);
                this.gameObject.SetActive(false);
                isHooked = false;
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