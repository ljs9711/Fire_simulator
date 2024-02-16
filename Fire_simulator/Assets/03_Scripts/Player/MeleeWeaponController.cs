using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract 추상클래스/ 다른 근접무기에서 기능 완성할것
public abstract class MeleeWeaponController : MonoBehaviour
{
    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    //공격중?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());//코루틴
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger("Attack"); //Animator 안에 있는 Attack 트리커 사용

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_in); //currentHand 에 있는 attackDelay_in 를 사용 (공격 히트 지점 활성화)
        isSwing = true;
        StartCoroutine(HitCoroutine()); //적중여부 코루틴

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_out); //currentHand 에 있는 attackDelay_out 를 사용 (공격 히트 지점 비활성화)
        isSwing = false;


        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelay_in - currentMeleeWeapon.attackDelay_out); //(공격 자체 비활성화)
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject() //조건문이라 bool 사용
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
        {
            return true;
        }
        return false;
    }

    //완성했지만 수정 가능한 함수 virtual
    public virtual void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        if (WeaponManager.currentWeapon != null) //무언가 들고있으면
            WeaponManager.currentWeapon.gameObject.SetActive(false); //게임 오브젝트 비활성화

        currentMeleeWeapon = _MeleeWeapon; //바꿀 무기가 현재 무기로 바뀜
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero; //위치 바뀌었을 수도 있으니 0,0,0으로 초기화
        currentMeleeWeapon.gameObject.SetActive(true);
    }
}
