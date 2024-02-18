using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float SecondPerRealTimeSecond; // 게임 세계의 100초 = 현실 1초

    private bool isNight = false; // 현재 밤인지 여부를 나타내는 변수

    [SerializeField]
    private float fogDensityCalc; // 안개 증가 비율

    [SerializeField]
    private float nightFogDensity; // 밤의 안개 밀도
    [SerializeField]
    private float dayFogDensity; // 낮의 안개 밀도
    private float currentFogDensity; // 계산된 안개 밀도

    void Start()
    {
        // 시작 시 낮의 안개 밀도를 현재 설정값으로 초기화
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        // 하늘 회전 (시간 흐름에 따른 낮과 밤의 변화 표현)
        transform.Rotate(Vector3.right, 0.1f * SecondPerRealTimeSecond * Time.deltaTime);

        // 현재 하늘의 각도에 따라 밤 여부 판단
        if (transform.eulerAngles.x >= 170 && transform.eulerAngles.x < 360)
            isNight = true;
        else
            isNight = false;

        // 현재가 밤인 경우
        if (isNight)
        {
            // 현재 안개 밀도를 부드럽게 밤의 안개 밀도로 변경
            currentFogDensity = Mathf.Lerp(currentFogDensity, nightFogDensity, 0.1f * fogDensityCalc * Time.deltaTime);
            RenderSettings.fogDensity = currentFogDensity; // 렌더링 설정에 반영
        }
        else // 현재가 낮인 경우
        {
            // 아침에는 안개를 없앰
            RenderSettings.fogDensity = 0f;
        }
    }
}
