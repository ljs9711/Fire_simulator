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
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());//�ڷ�ƾ
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack"); //Animator �ȿ� �ִ� Attack Ʈ��Ŀ ���

        yield return new WaitForSeconds(currentHand.attackDelay_in); //currentHand �� �ִ� attackDelay_in �� ��� (���� ��Ʈ ���� Ȱ��ȭ)
        isSwing = true;
        StartCoroutine(HitCoroutine()); //���߿��� �ڷ�ƾ

        yield return new WaitForSeconds(currentHand.attackDelay_out); //currentHand �� �ִ� attackDelay_out �� ��� (���� ��Ʈ ���� ��Ȱ��ȭ)
        isSwing = false;


        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelay_in - currentHand.attackDelay_out); //(���� ��ü ��Ȱ��ȭ)
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject() //���ǹ��̶� bool ���
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
