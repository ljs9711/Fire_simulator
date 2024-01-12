using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //이동
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed; //앉은상태 스피드

    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchY;

    //민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();

        //초기화
        originPosY = theCamera.transform.localPosition.y; //player 기준으로 카메라가 내려가야해서 localPosition 사용
        applyCrouchY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();

        TryJump();

        TryRun();

        TryCrouch();

        Move();

        CameraRotation();

        CharacterRotation();


    }


    //*****************함수*****************


    
    //앉기시도
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

    }
    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;

        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchY = crouchPosY;

        }
        else
        {
            crouchSpeed = walkSpeed;
            applyCrouchY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }


    //부드러운 앉기 동작 실행
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchY, 0.015f); // 보간함수 사용하여 자연스럽게 앉기 / 단점은 0.9999에서 1로 올라가지 않아 count 변수사용
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 500)
            {
                break;
            }

            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchY, 0f);
        
    }


    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
        }
    }


    //점프
    private void Jump()
    {
        //앉은 상태에서 점프시 앉은 상태 해제
        if(isCrouch)
        {
            Crouch();
        }


        myRigid.velocity = transform.up * jumpForce;
    }

    //지면체크
    private void IsGround() 
    {                                                                //캡슐콜라이더 영역의 Y 의 절반+ 0.1f(계단,대각등 판정 위해)
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f); //고정된 좌표 사용하기위해 Vector3 사용
    }


    //달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    //달리기 실행
    private void Running()
    {
        //앉은 상태에서 달리기시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }
        isRun = true;
        applySpeed = runSpeed;
    }

    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }


    //상하 카메라 회전
    private void CameraRotation() 
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //currentCameraRotationX 안에 -cameraRotationLimit, cameraRotationLimit 를 Mathf.Clamp 사용해 가두는 것

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //좌우 캐릭터회전
    private void CharacterRotation() 
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }


    //움직임 실행
    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 Velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + Velocity * Time.deltaTime); // Time.deltaTime 시간동안 velocity 만큼 움직임
    }
}
