using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject fish;

    private GameObject[] fishList;
    private int fishNum = 5;
    private int idx = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreateFishList(ref fishList, 0.2f, 1);

        StartCoroutine(ActivateFish());
    }
    IEnumerator ActivateFish()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            spawnFish(ref fishList[idx]);
            fishList[idx++].SetActive(true);
            if (idx == fishNum) idx = 0;
        }
    }

    private void CreateFishList(ref GameObject[] fishList, float exp, float speed)
    {
        fishList = new GameObject[fishNum];
        for (int i = 0; i < fishNum; i++)
        {
            GameObject fishObject = Instantiate(fish);
            fishObject.GetComponent<FishController>().fishExp = exp;
            fishObject.GetComponent<FishController>().swimSpeed = speed;

            fishObject.SetActive(false);

            fishList[i] = fishObject;
        }
    }

    private void spawnFish(ref GameObject fishObject)
    {
        //물고기 헤엄 방향 설정
        int direction = GetRandomDirection();
        fishObject.GetComponent<FishController>().swimDirection = direction;
        fishObject.transform.localScale = new Vector3(direction, 1f, 1f);

        //물고기 생성 위치 설정
        fishObject.transform.position = GetRandomPosition(direction);
    }

    private int GetRandomDirection()
    {
        if (Random.value >= 0.5) return 1;
        else return -1;
    }

    private Vector3 GetRandomPosition(float direction)
    {
        float x, y;
        if (direction == 1)
        {
            x = Random.Range(-5.0f, -4.0f);
            y = Random.Range(-4.0f, 1.5f);
        }
        else
        {
            x = Random.Range(4.0f, 5.0f);
            y = Random.Range(-4.0f, 1.5f);
        }

        Vector3 randomPosition = new Vector3(x, y, 0);
        return randomPosition;
    }
}
