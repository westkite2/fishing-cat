using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject fish;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateFish());
    }
    IEnumerator CreateFish()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(fish, GetRandomPosition(), Quaternion.identity);
        }
    }
    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-8.0f, -5.5f);
        float y = Random.Range(-4.5f, 2.0f);

        Vector3 randomPosition = new Vector3(x, y, 0);
        return randomPosition;
    }
}
