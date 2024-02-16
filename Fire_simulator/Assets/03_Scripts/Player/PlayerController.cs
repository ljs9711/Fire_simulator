using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // 이동 관련 변수들
    [SerializeField]
    private float walkSpeed; // 걷기 속도
    [SerializeField]
    private float runSpeed; // 달리기 속도
    private float applySpeed; // 현재 적용 중인 속도
    [SerializeField]
    private float crouchSpeed; // 앉았을 때 속도

    [SerializeField]
    private float jumpForce; // 점프 힘

    // 플레이어 상태 관련 변수들
    private bool isWalk = false; // 걷기 여부
    private bool isRun = false; // 달리기 여부
    private bool isGround = true; // 땅에 닿았는지 여부
    private bool isCrouch = false; // 앉았는지 여부

    // 움직임 체크 변수
    private Vector3 lastPos; // 이전 위치 저장

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY; // 앉은 상태의 카메라 높이
    private float originPosY; // 원래 카메라 높이
    private float applyCrouchY; // 적용 중인 카메라 높이

    // 민감도
    [SerializeField]
    private float lookSensitivity; // 마우스 감도

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit; // 카메라 회전 한계
    private float currentCameraRotationX = 0; // 현재 카메라의 상하 회전값
    private float pausedCameraRotationX; // 일시정지 전의 상하 회전값

    // 컴포넌트
    [SerializeField]
    private Camera theCamera; // 메인 카메라
    private Rigidbody myRigid; // Rigidbody 컴포넌트
    private GunController theGunController; // 총기 컨트롤러
    private Crosshair theCrosshair; // 크로스헤어
    private StatusController theStatusController; // 상태 컨트롤러
    private WeaponSway weaponSway; // 무기 흔들림

    // 땅 착지 여부 체크를 위한 콜라이더
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        // 컴포넌트 초기화
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        // 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // 플레이어 기준으로 카메라가 내려가야하므로 localPosition 사용
        applyCrouchY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround(); // 땅에 닿았는지 체크
        TryJump(); // 점프 시도
        TryRun(); // 달리기 시도
        TryCrouch(); // 앉기 시도
        Move(); // 이동
        CameraRotation(); // 카메라 회전
        CharacterRotation(); // 캐릭터 회전
    }

    private void FixedUpdate()
    {
        MoveCheck(); // 이동 체크
    }

    // 앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // 앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch; // 앉았는지 여부 토글
        theCrosshair.CrouchingAnimation(isCrouch); // 크로스헤어 애니메이션 적용

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchY = crouchPosY;
        }
        else
        {
            crouchSpeed = walkSpeed;
            applyCrouchY = originPosY;
        }

        // 걷기 중 앉기로 변경 시 걷기 애니메이션 실행되는 문제 해결
        if (isWalk)
        {
            isWalk = false;
            theCrosshair.WalkingAnimation(isWalk);
        }

        StartCoroutine(CrouchCoroutine()); // 부드러운 앉기 동작 실행
    }

    // 부드러운 앉기 동작 실행
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchY, 0.015f); // 보간 함수를 사용하여 자연스럽게 앉기
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 500) // 무한루프 방지를 위한 카운터
            {
                break;
            }

            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchY, 0f); // 최종 앉은 위치로 설정
    }

    // 점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
            isGround = false;
        }
    }

    // 점프
    private void Jump()
    {
        // 앉은 상태에서 점프 시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }

        theStatusController.DecreaseStamina(100); // 스태미너 감소
        myRigid.velocity = transform.up * jumpForce; // 점프 힘 적용
    }

    // 땅 착지 여부 체크
    private void IsGround()
    {
        // 캡슐 콜라이더의 아래 방향으로 레이캐스트하여 땅에 닿았는지 판별
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 1f);
        theCrosshair.JumpingAnimation(!isGround); // 크로스헤어의 점프 애니메이션 적용
    }

    // 달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    // 달리기 실행
    private void Running()
    {
        // 앉은 상태에서 달리기 시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }

        theGunController.CancelFineSight(); // 정조준 취소

        isRun = true;
        theCrosshair.RunningAnimation(isRun); // 크로스헤어의 달리기 애니메이션 적용
        theStatusController.DecreaseStamina(10); // 스태미너 감소
        applySpeed = runSpeed; // 속도 적용
    }

    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun); // 크로스헤어의 달리기 애니메이션 취소
        applySpeed = walkSpeed; // 속도 원래대로 복구
    }

    // 상하 카메라 회전
    public void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        if (!GameManager.isPause)
        {
            currentCameraRotationX -= _cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        }
        else
        {
            currentCameraRotationX = pausedCameraRotationX;
        }

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 좌우 캐릭터 회전
    public void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 움직임 실행
    public void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 Velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + Velocity * Time.deltaTime); // Time.deltaTime 시간동안 velocity 만큼 움직임
    }

    // 움직임 체크
    public void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) // 전 프레임과 현재 위치값이 0.01f 보다 크면 걷는 것으로 판단
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk); // 크로스헤어의 걷기 애니메이션 적용
            lastPos = transform.position;
        }
    }

    // 일시정지 시 움직임 멈춤
    public void PauseMovement()
    {
        isWalk = false;
        isRun = false;
        isCrouch = false;
        applySpeed = 0f; // 움직임 속도를 0으로 설정
        pausedCameraRotationX = currentCameraRotationX; // 현재의 상하 회전값을 저장
        currentCameraRotationX = 0f; // 카메라의 상하 회전값을 일시정지

        // WeaponSway의 sway 효과 일시정지
        if (weaponSway != null)
            weaponSway.PauseSway();
    }

    // 일시정지 해제 시 움직임 복구
    public void ResumeMovement()
    {
        applySpeed = walkSpeed; // 움직임 속도를 다시 초기화
        currentCameraRotationX = pausedCameraRotationX; // 저장된 상하 회전값을 다시 적용

        // WeaponSway의 sway 효과 해제
        if (weaponSway != null)
            weaponSway.ResumeSway();
    }
}
