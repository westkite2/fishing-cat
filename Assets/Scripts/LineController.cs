using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public GameObject targetPoint;
    public GameObject hook;
    public Image barFill;

    //플레이 모드
    private bool isTargeting;

    //던지기 속성
    private LineRenderer lineRenderer;

    private Vector2 startPosition; //던지는 방향
    private Vector2 pressPosition;
    private Vector2 endPosition;

    private float throwPower; //던지는 힘
    private float throwLength; //던지는 거리
    private float throwSpeed = 5.0f; //던지는 속도
    private float throwStartTime; //던지기 시작 시간

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        startPosition = new Vector2(-0.4f, 4f);
        lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, 0f));
        lineRenderer.SetPosition(1, new Vector3(startPosition.x, startPosition.y, 0f));
        hook.transform.position = startPosition;
        
        isTargeting = true;

        targetPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargeting)
        {
            SetTargetPoint();
        }
        else
        {
            ExpandLine();

            
            //RewindLine();
            //isTargeting = true;
        }




    }

    private void SetTargetPoint()
    {
        //설명: 타겟 지점의 좌표와 던지는 힘 구하기

        if (Input.GetMouseButtonDown(0)) //누르는 순간 수행
        {
            //던지는 힘 초기화
            throwPower = 0;

            //타겟 포인트 표시
            Vector2 mousePosition = Input.mousePosition;
            targetPoint.transform.position = mousePosition;
            targetPoint.SetActive(true);

            //타겟 포인트 좌표 저장
            pressPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        }

        if (Input.GetMouseButton(0)) //누르는 동안 수행
        {
            //던지는 힘 측정        
            if (throwPower <= 1)
            {
                throwPower += Time.deltaTime;
                barFill.fillAmount = throwPower;
            }
        }

        if (Input.GetMouseButtonUp(0)) //누르기 멈추면 수행
        {
            //타겟 포인트 표시 해제
            targetPoint.SetActive(false);

            //던지기 시작 시간 측정
            throwStartTime = Time.time;

            //타게팅 모드 중지
            isTargeting = false;
        }
    }

    void ExpandLine()
    {
        //설명: 목표 지점까지 선을 확장

        //던지는 지점 계산
        Vector2 throwDirection = pressPosition - startPosition;
        endPosition = startPosition + (throwDirection * throwPower);

        //던지기 애니메이션
        throwLength = Vector2.Distance(startPosition, endPosition);
        float coverdLength = (Time.time - throwStartTime) * throwSpeed;
        float coveredRatio = coverdLength / throwLength;
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, coveredRatio);
        lineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y, 0f));

        //갈고리 이동
        hook.transform.position = newPosition;

        //갈고리 각도 설정
        float throwAngle = Vector2.Angle(new Vector2(0, -1), throwDirection);
        if (throwDirection.x < 0 )
        {
            throwAngle = throwAngle * -1;
        }
        hook.transform.rotation = Quaternion.Euler(0, 0, throwAngle);
    }

}
