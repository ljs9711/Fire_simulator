using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격중?
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
