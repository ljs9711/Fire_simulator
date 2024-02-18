using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float SecondPerRealTimeSecond; // ���� ������ 100�� = ���� 1��

    private bool isNight = false; // ���� ������ ���θ� ��Ÿ���� ����

    [SerializeField]
    private float fogDensityCalc; // �Ȱ� ���� ����

    [SerializeField]
    private float nightFogDensity; // ���� �Ȱ� �е�
    [SerializeField]
    private float dayFogDensity; // ���� �Ȱ� �е�
    private float currentFogDensity; // ���� �Ȱ� �е�

    void Start()
    {
        // ���� �� ���� �Ȱ� �е��� ���� ���������� �ʱ�ȭ
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        // �ϴ� ȸ�� (�ð� �帧�� ���� ���� ���� ��ȭ ǥ��)
        transform.Rotate(Vector3.right, 0.1f * SecondPerRealTimeSecond * Time.deltaTime);

        // ���� �ϴ��� ������ ���� �� ���� �Ǵ�
        if (transform.eulerAngles.x >= 170 && transform.eulerAngles.x < 360)
            isNight = true;
        else
            isNight = false;

        // ���簡 ���� ���
        if (isNight)
        {
            // ���� �Ȱ� �е��� �ε巴�� ���� �Ȱ� �е��� ����
            currentFogDensity = Mathf.Lerp(currentFogDensity, nightFogDensity, 0.1f * fogDensityCalc * Time.deltaTime);
            RenderSettings.fogDensity = currentFogDensity; // ������ ������ �ݿ�
        }
        else // ���簡 ���� ���
        {
            // ��ħ���� �Ȱ��� ����
            RenderSettings.fogDensity = 0f;
        }
    }
}
