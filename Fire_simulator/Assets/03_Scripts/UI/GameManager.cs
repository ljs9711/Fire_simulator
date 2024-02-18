using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPause = false; // 일시 정지 여부

    void Update()
    {
        if (isPause) // 일시 정지 상태일 때
        {
            Cursor.lockState = CursorLockMode.None; // 마우스 커서 해제
            Cursor.visible = true; // 마우스 커서 표시
        }
        else // 일시 정지 상태가 아닐 때
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 잠금
            Cursor.visible = false; // 마우스 커서 숨김
        }
    }
}
