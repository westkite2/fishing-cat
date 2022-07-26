using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public GameObject targetPoint;
    public Image barFill;

    private Vector2 pressPosition;
    private float power;

    //public int speed = 1;
    //private LineRenderer lineRenderer;
    //private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint.SetActive(false);
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {

        SetTargetPoint();
        
        //ExpandLine();
        //lineRenderer.SetPosition(1, new Vector3(targetPos.x, targetPos.y, 0f));

        //RewindLine();

    }

    private void SetTargetPoint()
    {
        if (Input.GetMouseButtonDown(0)) //누르는 순간 수행
        {
            //파워 초기화
            power = 0;

            //타겟 포인트 표시
            Vector2 mousePosition = Input.mousePosition;
            targetPoint.transform.position = mousePosition;
            targetPoint.SetActive(true);

            //타겟 포인트 좌표 저장
            pressPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        }

        if (Input.GetMouseButton(0)) //누르는 동안 수행
        {
            //파워 증가            
            if (power <= 1)
            {
                power += Time.deltaTime;
                barFill.fillAmount = power;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //타겟 포인트 표시 해제
            targetPoint.SetActive(false);
        }
    }
 
}
