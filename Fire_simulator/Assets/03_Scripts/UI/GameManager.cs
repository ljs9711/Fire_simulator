using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPause = false; // �Ͻ� ���� ����

    void Update()
    {
        if (isPause) // �Ͻ� ���� ������ ��
        {
            Cursor.lockState = CursorLockMode.None; // ���콺 Ŀ�� ����
            Cursor.visible = true; // ���콺 Ŀ�� ǥ��
        }
        else // �Ͻ� ���� ���°� �ƴ� ��
        {
            Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ���
            Cursor.visible = false; // ���콺 Ŀ�� ����
        }
    }
}
