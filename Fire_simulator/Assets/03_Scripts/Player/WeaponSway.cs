using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //기존위치
    private Vector3 originPos;

    //현재위치
    private Vector3 currentPos;

    //sway한계
    [SerializeField]
    private Vector3 limitPos;

    //정조준 sway 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    //sway 부드러움 값
    [SerializeField]
    private Vector3 smoothSway;

    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;

    private bool isPaused = false; // 일시정지 여부




    void Start()
    {
        //현재 로컬포지션
        originPos = this.transform.localPosition;
    }

    void Update()
    {
        if (!isPaused)
            TrySway();
    }


    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) //마우스가 움직이면
            Swaying();
        else
            BackToOriginPos();
    }


    public void PauseSway()
    {
        isPaused = true;
        currentPos = originPos; // 현재 위치를 기본 위치로 초기화하여 멈추도록 설정
    }

    public void ResumeSway()
    {
        isPaused = false;
    }



    private void Swaying()
    {
        // 마우스값 변수 대입
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {
            // Mathf는 화면 밖으로 나가지 않기 위해 사용
            currentPos.Set(
                Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -limitPos.y, limitPos.y),
                originPos.z
            );
        }
        else
        {
            currentPos.Set(
                Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                originPos.z
            );
        }
        transform.localPosition = currentPos;
    }


    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
