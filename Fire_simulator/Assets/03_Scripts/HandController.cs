using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = false;

    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격중?
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
                StartCoroutine(AttackCoroutine());//코루틴
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack"); //Animator 안에 있는 Attack 트리커 사용

        yield return new WaitForSeconds(currentHand.attackDelay_in); //currentHand 에 있는 attackDelay_in 를 사용 (공격 히트 지점 활성화)
        isSwing = true;
        StartCoroutine(HitCoroutine()); //적중여부 코루틴

        yield return new WaitForSeconds(currentHand.attackDelay_out); //currentHand 에 있는 attackDelay_out 를 사용 (공격 히트 지점 비활성화)
        isSwing = false;


        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelay_in - currentHand.attackDelay_out); //(공격 자체 비활성화)
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

    private bool CheckObject() //조건문이라 bool 사용
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }


    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null) //무언가 들고있으면
            WeaponManager.currentWeapon.gameObject.SetActive(false); //게임 오브젝트 비활성화

        currentHand = _hand; //바꿀 무기가 현재 무기로 바뀜
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero; //위치 바뀌었을 수도 있으니 0,0,0으로 초기화
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
