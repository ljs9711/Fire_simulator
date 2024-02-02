using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float SecondPerRealTimeSecond; //���Ӽ��� 100�� = ���� 1��

    private bool isNight = false;

    [SerializeField]
    private float fogDensityCalc; //fog ���� ����

    [SerializeField]
    private float nightFogDensity; //�� ������ Fog 
    private float dayFogDensity; //�� ������ Fog 
    private float currentFogDensity; //���



    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * SecondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            isNight = true;
        else if (transform.eulerAngles.x >= 340)
            isNight = false;


        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
