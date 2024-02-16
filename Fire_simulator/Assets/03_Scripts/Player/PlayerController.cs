using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // �̵� ���� ������
    [SerializeField]
    private float walkSpeed; // �ȱ� �ӵ�
    [SerializeField]
    private float runSpeed; // �޸��� �ӵ�
    private float applySpeed; // ���� ���� ���� �ӵ�
    [SerializeField]
    private float crouchSpeed; // �ɾ��� �� �ӵ�

    [SerializeField]
    private float jumpForce; // ���� ��

    // �÷��̾� ���� ���� ������
    private bool isWalk = false; // �ȱ� ����
    private bool isRun = false; // �޸��� ����
    private bool isGround = true; // ���� ��Ҵ��� ����
    private bool isCrouch = false; // �ɾҴ��� ����

    // ������ üũ ����
    private Vector3 lastPos; // ���� ��ġ ����

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY; // ���� ������ ī�޶� ����
    private float originPosY; // ���� ī�޶� ����
    private float applyCrouchY; // ���� ���� ī�޶� ����

    // �ΰ���
    [SerializeField]
    private float lookSensitivity; // ���콺 ����

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit; // ī�޶� ȸ�� �Ѱ�
    private float currentCameraRotationX = 0; // ���� ī�޶��� ���� ȸ����
    private float pausedCameraRotationX; // �Ͻ����� ���� ���� ȸ����

    // ������Ʈ
    [SerializeField]
    private Camera theCamera; // ���� ī�޶�
    private Rigidbody myRigid; // Rigidbody ������Ʈ
    private GunController theGunController; // �ѱ� ��Ʈ�ѷ�
    private Crosshair theCrosshair; // ũ�ν����
    private StatusController theStatusController; // ���� ��Ʈ�ѷ�
    private WeaponSway weaponSway; // ���� ��鸲

    // �� ���� ���� üũ�� ���� �ݶ��̴�
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        // ������Ʈ �ʱ�ȭ
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        // �ʱ�ȭ
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // �÷��̾� �������� ī�޶� ���������ϹǷ� localPosition ���
        applyCrouchY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround(); // ���� ��Ҵ��� üũ
        TryJump(); // ���� �õ�
        TryRun(); // �޸��� �õ�
        TryCrouch(); // �ɱ� �õ�
        Move(); // �̵�
        CameraRotation(); // ī�޶� ȸ��
        CharacterRotation(); // ĳ���� ȸ��
    }

    private void FixedUpdate()
    {
        MoveCheck(); // �̵� üũ
    }

    // �ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // �ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch; // �ɾҴ��� ���� ���
        theCrosshair.CrouchingAnimation(isCrouch); // ũ�ν���� �ִϸ��̼� ����

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

        // �ȱ� �� �ɱ�� ���� �� �ȱ� �ִϸ��̼� ����Ǵ� ���� �ذ�
        if (isWalk)
        {
            isWalk = false;
            theCrosshair.WalkingAnimation(isWalk);
        }

        StartCoroutine(CrouchCoroutine()); // �ε巯�� �ɱ� ���� ����
    }

    // �ε巯�� �ɱ� ���� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchY, 0.015f); // ���� �Լ��� ����Ͽ� �ڿ������� �ɱ�
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 500) // ���ѷ��� ������ ���� ī����
            {
                break;
            }

            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchY, 0f); // ���� ���� ��ġ�� ����
    }

    // ���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
            isGround = false;
        }
    }

    // ����
    private void Jump()
    {
        // ���� ���¿��� ���� �� ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }

        theStatusController.DecreaseStamina(100); // ���¹̳� ����
        myRigid.velocity = transform.up * jumpForce; // ���� �� ����
    }

    // �� ���� ���� üũ
    private void IsGround()
    {
        // ĸ�� �ݶ��̴��� �Ʒ� �������� ����ĳ��Ʈ�Ͽ� ���� ��Ҵ��� �Ǻ�
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 1f);
        theCrosshair.JumpingAnimation(!isGround); // ũ�ν������ ���� �ִϸ��̼� ����
    }

    // �޸��� �õ�
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

    // �޸��� ����
    private void Running()
    {
        // ���� ���¿��� �޸��� �� ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }

        theGunController.CancelFineSight(); // ������ ���

        isRun = true;
        theCrosshair.RunningAnimation(isRun); // ũ�ν������ �޸��� �ִϸ��̼� ����
        theStatusController.DecreaseStamina(10); // ���¹̳� ����
        applySpeed = runSpeed; // �ӵ� ����
    }

    // �޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun); // ũ�ν������ �޸��� �ִϸ��̼� ���
        applySpeed = walkSpeed; // �ӵ� ������� ����
    }

    // ���� ī�޶� ȸ��
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

    // �¿� ĳ���� ȸ��
    public void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // ������ ����
    public void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 Velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + Velocity * Time.deltaTime); // Time.deltaTime �ð����� velocity ��ŭ ������
    }

    // ������ üũ
    public void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) // �� �����Ӱ� ���� ��ġ���� 0.01f ���� ũ�� �ȴ� ������ �Ǵ�
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk); // ũ�ν������ �ȱ� �ִϸ��̼� ����
            lastPos = transform.position;
        }
    }

    // �Ͻ����� �� ������ ����
    public void PauseMovement()
    {
        isWalk = false;
        isRun = false;
        isCrouch = false;
        applySpeed = 0f; // ������ �ӵ��� 0���� ����
        pausedCameraRotationX = currentCameraRotationX; // ������ ���� ȸ������ ����
        currentCameraRotationX = 0f; // ī�޶��� ���� ȸ������ �Ͻ�����

        // WeaponSway�� sway ȿ�� �Ͻ�����
        if (weaponSway != null)
            weaponSway.PauseSway();
    }

    // �Ͻ����� ���� �� ������ ����
    public void ResumeMovement()
    {
        applySpeed = walkSpeed; // ������ �ӵ��� �ٽ� �ʱ�ȭ
        currentCameraRotationX = pausedCameraRotationX; // ����� ���� ȸ������ �ٽ� ����

        // WeaponSway�� sway ȿ�� ����
        if (weaponSway != null)
            weaponSway.ResumeSway();
    }
}
