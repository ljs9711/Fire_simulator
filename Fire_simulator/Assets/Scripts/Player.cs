using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;

    private void Update()
    {
        // 캐릭터 이동
        MoveCharacter();

        // 카메라 회전
        RotateCamera();
    }

    void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(-mouseY, mouseX, 0.0f) * mouseSensitivity;
        transform.Rotate(rotation);

        // 수직 회전 제한
        float currentXRotation = transform.rotation.eulerAngles.x;
        if (currentXRotation > 90.0f && currentXRotation < 180.0f)
        {
            currentXRotation = 90.0f;
        }
        else if (currentXRotation > 180.0f && currentXRotation < 270.0f)
        {
            currentXRotation = 270.0f;
        }

        transform.rotation = Quaternion.Euler(currentXRotation, transform.rotation.eulerAngles.y, 0.0f);
    }
}
