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

    //상태
    private bool isRun = false;

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


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        TryRun();

        Move();

        CameraRotation();

        CharacterRotation();

        
    }



    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }




    //함수
    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 Velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + Velocity * Time.deltaTime); // Time.deltaTime 시간동안 velocity 만큼 움직임
    }


    private void CameraRotation() //상하 카메라 회전
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //currentCameraRotationX 안에 -cameraRotationLimit, cameraRotationLimit 를 Mathf.Clamp 사용해 가두는 것

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation() //좌우 캐릭터회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }



}
