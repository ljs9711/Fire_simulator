using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    private Hand currentHand;

    //������?
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    void Update()
    {
        TryAttack();
        
    }

    private void TryAttack()
    {
        if(Input.GetButton("Fire1"))
        {

        }
    }
}
