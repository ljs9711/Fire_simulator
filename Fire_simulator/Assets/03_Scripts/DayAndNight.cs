using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float SecondPerRealTimeSecond; // 게임 세계 100초 = 현실 1초

    private bool isNight = false;

    [SerializeField]
    private float fogDensityCalc; // 안개 증가 비율

    [SerializeField]
    private float nightFogDensity; // 밤 안개 밀도
    [SerializeField]
    private float dayFogDensity; // 낮 안개 밀도
    private float currentFogDensity; // 계산된 안개 밀도

    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * SecondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170 && transform.eulerAngles.x < 360)
            isNight = true;
        else
            isNight = false;

        if (isNight)
        {
            currentFogDensity = Mathf.Lerp(currentFogDensity, nightFogDensity, 0.1f * fogDensityCalc * Time.deltaTime);
            RenderSettings.fogDensity = currentFogDensity;
        }
        else
        {
            // 아침에 Fog를 없애기
            RenderSettings.fogDensity = 0f;
        }
    }
}
