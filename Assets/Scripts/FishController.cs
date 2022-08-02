using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float swimSpeed = 1;
    public GameObject hook;
    public GameObject line;

    private int swimDirection = 1;
    private bool isHooked = false;
    private Vector2 hookPositionOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHooked)
        {
            //낚인 경우 낚싯대를 따라 이동
            transform.position = hook.transform.TransformPoint(hookPositionOffset);
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
        isHooked = true;

        FishingRodController fishingRodController = line.GetComponent<FishingRodController>();
        fishingRodController.SetFishCaughtTrue();

        hookPositionOffset = transform.position - hook.transform.position;



    }

}
