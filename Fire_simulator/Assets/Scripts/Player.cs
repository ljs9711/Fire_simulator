using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //�̵�
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed; //�������� ���ǵ�

    [SerializeField]
    private float jumpForce;

    //���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //�ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchY;

    //�ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;

    //�� ���� ����
    private CapsuleCollider capsuleCollider;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();

        //�ʱ�ȭ
        originPosY = theCamera.transform.localPosition.y; //player �������� ī�޶� ���������ؼ� localPosition ���
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


    //*****************�Լ�*****************


    
    //�ɱ�õ�
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

    }
    //�ɱ� ����
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


    //�ε巯�� �ɱ� ���� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchY, 0.015f); // �����Լ� ����Ͽ� �ڿ������� �ɱ� / ������ 0.9999���� 1�� �ö��� �ʾ� count �������
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 500)
            {
                break;
            }

            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchY, 0f);
        
    }


    //���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
        }
    }


    //����
    private void Jump()
    {
        //���� ���¿��� ������ ���� ���� ����
        if(isCrouch)
        {
            Crouch();
        }


        myRigid.velocity = transform.up * jumpForce;
    }

    //����üũ
    private void IsGround() 
    {                                                                //ĸ���ݶ��̴� ������ Y �� ����+ 0.1f(���,�밢�� ���� ����)
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f); //������ ��ǥ ����ϱ����� Vector3 ���
    }


    //�޸��� �õ�
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

    //�޸��� ����
    private void Running()
    {
        //���� ���¿��� �޸���� ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }
        isRun = true;
        applySpeed = runSpeed;
    }

    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }


    //���� ī�޶� ȸ��
    private void CameraRotation() 
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //currentCameraRotationX �ȿ� -cameraRotationLimit, cameraRotationLimit �� Mathf.Clamp ����� ���δ� ��

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //�¿� ĳ����ȸ��
    private void CharacterRotation() 
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }


    //������ ����
    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 Velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + Velocity * Time.deltaTime); // Time.deltaTime �ð����� velocity ��ŭ ������
    }
}
