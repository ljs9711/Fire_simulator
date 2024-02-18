using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // 기존 위치
    private Vector3 originPos;

    // 현재 위치
    private Vector3 currentPos;

    // Sway 한계
    [SerializeField] private Vector3 limitPos;

    // 정조준 Sway 한계
    [SerializeField] private Vector3 fineSightLimitPos;

    // Sway 부드러움 값
    [SerializeField] private Vector3 smoothSway;

    // 필요한 컴포넌트
    [SerializeField] private GunController theGunController;

    private bool isPaused = false; // 일시정지 여부

    void Start()
    {
        // 현재 로컬 포지션을 기존 위치로 초기화
        originPos = transform.localPosition;
    }

    void Update()
    {
        // 일시정지 상태가 아니라면 Sway 시도
        if (!isPaused)
            TrySway();
    }

    // Sway 시도 함수
    private void TrySway()
    {
        // 마우스 움직임이 감지되면 Swaying() 호출, 아니면 원위치로 복귀하는 BackToOriginPos() 호출
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    // 무기 sway 일시정지 함수
    public void PauseSway()
    {
        // 일시정지 상태로 설정하고 현재 위치를 기존 위치로 초기화하여 멈추도록 함
        isPaused = true;
        currentPos = originPos;
    }

    // 무기 sway 재개 함수
    public void ResumeSway()
    {
        // 일시정지 상태 해제
        isPaused = false;
    }

    // 무기 sway 적용 함수
    private void Swaying()
    {
        // 마우스 입력에 따른 이동 값 변수에 저장
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        // 정조준 상태 여부에 따라 적용할 sway 한계 범위 결정
        Vector3 targetSwayPos = !theGunController.isFineSightMode ? limitPos : fineSightLimitPos;

        // 현재 위치를 부드러운 움직임을 통해 새로운 위치로 이동시킴
        currentPos.Set(
            Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -targetSwayPos.x, targetSwayPos.x),
            Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -targetSwayPos.y, targetSwayPos.y),
            originPos.z
        );

        // 현재 위치를 적용하여 무기 sway 적용
        transform.localPosition = currentPos;
    }

    // 무기 sway 원위치 복귀 함수
    private void BackToOriginPos()
    {
        // 현재 위치를 기존 위치로 일정한 비율로 부드럽게 이동시킴
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        // 현재 위치를 적용하여 무기 sway 적용
        transform.localPosition = currentPos;
    }
}
