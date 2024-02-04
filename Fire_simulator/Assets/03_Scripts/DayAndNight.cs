using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float SecondPerRealTimeSecond; // ���� ���� 100�� = ���� 1��

    private bool isNight = false;

    [SerializeField]
    private float fogDensityCalc; // �Ȱ� ���� ����

    [SerializeField]
    private float nightFogDensity; // �� �Ȱ� �е�
    [SerializeField]
    private float dayFogDensity; // �� �Ȱ� �е�
    private float currentFogDensity; // ���� �Ȱ� �е�

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
            // ��ħ�� Fog�� ���ֱ�
            RenderSettings.fogDensity = 0f;
        }
    }
}
