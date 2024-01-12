using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    private Rigidbody playerRigid;


    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.right * moveDirectionZ;

        Vector3 Velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;

        playerRigid.MovePosition(transform.position + Velocity * Time.deltaTime); // Time.deltaTime 시간동안 velocity 만큼 움직임


    }
}
