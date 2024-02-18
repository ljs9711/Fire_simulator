using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // ���� ��ġ
    private Vector3 originPos;

    // ���� ��ġ
    private Vector3 currentPos;

    // Sway �Ѱ�
    [SerializeField] private Vector3 limitPos;

    // ������ Sway �Ѱ�
    [SerializeField] private Vector3 fineSightLimitPos;

    // Sway �ε巯�� ��
    [SerializeField] private Vector3 smoothSway;

    // �ʿ��� ������Ʈ
    [SerializeField] private GunController theGunController;

    private bool isPaused = false; // �Ͻ����� ����

    void Start()
    {
        // ���� ���� �������� ���� ��ġ�� �ʱ�ȭ
        originPos = transform.localPosition;
    }

    void Update()
    {
        // �Ͻ����� ���°� �ƴ϶�� Sway �õ�
        if (!isPaused)
            TrySway();
    }

    // Sway �õ� �Լ�
    private void TrySway()
    {
        // ���콺 �������� �����Ǹ� Swaying() ȣ��, �ƴϸ� ����ġ�� �����ϴ� BackToOriginPos() ȣ��
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    // ���� sway �Ͻ����� �Լ�
    public void PauseSway()
    {
        // �Ͻ����� ���·� �����ϰ� ���� ��ġ�� ���� ��ġ�� �ʱ�ȭ�Ͽ� ���ߵ��� ��
        isPaused = true;
        currentPos = originPos;
    }

    // ���� sway �簳 �Լ�
    public void ResumeSway()
    {
        // �Ͻ����� ���� ����
        isPaused = false;
    }

    // ���� sway ���� �Լ�
    private void Swaying()
    {
        // ���콺 �Է¿� ���� �̵� �� ������ ����
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        // ������ ���� ���ο� ���� ������ sway �Ѱ� ���� ����
        Vector3 targetSwayPos = !theGunController.isFineSightMode ? limitPos : fineSightLimitPos;

        // ���� ��ġ�� �ε巯�� �������� ���� ���ο� ��ġ�� �̵���Ŵ
        currentPos.Set(
            Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -targetSwayPos.x, targetSwayPos.x),
            Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -targetSwayPos.y, targetSwayPos.y),
            originPos.z
        );

        // ���� ��ġ�� �����Ͽ� ���� sway ����
        transform.localPosition = currentPos;
    }

    // ���� sway ����ġ ���� �Լ�
    private void BackToOriginPos()
    {
        // ���� ��ġ�� ���� ��ġ�� ������ ������ �ε巴�� �̵���Ŵ
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        // ���� ��ġ�� �����Ͽ� ���� sway ����
        transform.localPosition = currentPos;
    }
}
