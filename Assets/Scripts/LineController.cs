using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public GameObject targetPoint;
    public GameObject hook;
    public Image barFill;

    //모드 (exclusive)
    private bool isTargeting; //타게팅
    private bool isExpanding; //던지기
    private bool isRewinding; //되감기
    
    //모드 보조 변수
    private float throwStartTime; //던지기 시작 시간
    private float rewindStartTime; //되감기 시작 시간
    private bool isNewClick; //새로운 클릭인지 여부 (타게팅 전 클릭이 입력되는 것을 방지)

    //플레이어가 결정하는 값
    private Vector2 pressPosition; //조준한 위치
    private float throwPower; //던지기 힘

    //다른 변수들로 계산되는 값
    private Vector2 endPosition; //실제 던진 위치
    private Vector2 throwDirection; //던진 선의 방향
    private float throwAngle; //던진 선의 각도
    private float throwLength; //던진 선의 길이 (start-end 지점 거리)

    //낚싯대 특성
    private LineRenderer lineRenderer; //낚싯대 선
    private Vector2 startPosition; //선 초기 위치
    private float throwSpeed = 5.0f; //던지기 속도
    private float rewindSpeed = 6.0f; //되감기 속도


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
        isExpanding = false;
        isRewinding = false;

        targetPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargeting)
        {
            bool isDone = SetTargetPoint();
            if (isDone)
            {
                //필요한 변수 값 계산
                CalculateProperties();

                //던지기 모드 시작 시간 측정
                throwStartTime = Time.time;

                //타게팅 모드에서 던지기 모드로 전환
                isTargeting = false;
                isExpanding = true;
            }
        }
        else if (isExpanding)
        {
            bool isAtEndPoint = ExpandLineToEndPosition();
            if (isAtEndPoint)
            {
                //되감기 모드 시작 시간 측정
                rewindStartTime = Time.time;

                //던지기 모드에서 되감기 모드로 전환
                isExpanding = false;
                isRewinding = true;
            }
        }
        else if (isRewinding)
        {
            bool isAtStartPoint = RewindLineToStartPosition();
            if (isAtStartPoint)
            {
                isRewinding = false;
                isTargeting = true;
            }
        }
    }
    
    private bool SetTargetPoint()
    {
        //설명: 타겟 지점(pressPosition)과 던지는 힘(throwPower) 구하기. 완료 시 true 반환

        if (Input.GetMouseButtonDown(0)) //누르는 순간 수행
        {
            isNewClick = true;

            //타겟 포인트 좌표 저장
            Vector2 mousePosition = Input.mousePosition;
            pressPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (pressPosition.y <= 2.5) //바다 영역 클릭 시 유효
            {
                //던지는 힘 초기화
                throwPower = 0;

                //타겟 포인트 표시
                targetPoint.transform.position = mousePosition;
                targetPoint.SetActive(true);

            }
        }

        if (Input.GetMouseButton(0)) //누르는 동안 수행
        {
            if(pressPosition.y <= 2.5)
            {
                //던지는 힘 측정        
                if (throwPower <= 1)
                {
                    throwPower += Time.deltaTime;
                    barFill.fillAmount = throwPower;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) //누르기 멈추면 수행
        {
            if (pressPosition.y <= 2.5 & isNewClick)
            {
                //타겟 포인트 표시 해제
                targetPoint.SetActive(false);

                isNewClick = false;
                return true;
            }
        }
        return false;
    }

    private void CalculateProperties()
    {
        //설명: 던지기 및 되감기에 필요한 변수 값 계산

        throwDirection = pressPosition - startPosition;
        
        throwAngle = Vector2.Angle(new Vector2(0, -1), throwDirection);
        if (throwDirection.x < 0)
        {
            throwAngle = throwAngle * -1;
        }

        endPosition = startPosition + (throwDirection * throwPower);

        throwLength = Vector2.Distance(startPosition, endPosition);

    }

    private bool ExpandLineToEndPosition()
    {
        //설명: 던진 위치까지 선을 확장. 던지기가 끝나면 true 반환

        //선 이동
        float coveredLength = (Time.time - throwStartTime) * throwSpeed;
        float coveredRatio = coveredLength / throwLength;
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, coveredRatio);
        lineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y, 0f));
        
        //갈고리 각도 설정
        hook.transform.rotation = Quaternion.Euler(0, 0, throwAngle);

        //갈고리 이동
        hook.transform.position = newPosition;

        //던지기 완료
        if (coveredRatio >= 1)
        {
            return true;
        }
        return false;
    }

    private bool RewindLineToStartPosition()
    {
        //설명: 초기 위치까지 선을 축소. 되감기가 끝나면 true 반환

        //선 이동
        float restoredLength = (Time.time - rewindStartTime) * rewindSpeed;
        float restoredRatio = restoredLength / throwLength;
        Vector3 newPosition = Vector3.Lerp(endPosition, startPosition, restoredRatio);
        lineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y, 0f));

        //갈고리 각도 설정
        hook.transform.rotation = Quaternion.Euler(0, 0, throwAngle);

        //갈고리 이동
        hook.transform.position = newPosition;

        //되감기 완료
        if (restoredRatio >= 1)
        {
            return true;
        }
        return false;
    }
}
