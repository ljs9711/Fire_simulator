using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = false;

    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    private Hand currentHand;

    //������?
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    void Update()
    {
        if(isActivate)
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


    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null) //���� ���������
            WeaponManager.currentWeapon.gameObject.SetActive(false); //���� ������Ʈ ��Ȱ��ȭ

        currentHand = _hand; //�ٲ� ���Ⱑ ���� ����� �ٲ�
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero; //��ġ �ٲ���� ���� ������ 0,0,0���� �ʱ�ȭ
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
