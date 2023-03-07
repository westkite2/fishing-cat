using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public Text expText;
    public Image expBarFill;

    private float exp;

    private void Start()
    {
        exp = 0;
        expBarFill.fillAmount = exp;
    }

    // Update is called once per frame
    void Update()
    {
        //점수 설정
        expText.text = (exp * 10).ToString();
    }
    
    public void addExp(float amount)
    {
        //점수 가산
        exp += amount;
        expBarFill.fillAmount = exp;
    }
}
