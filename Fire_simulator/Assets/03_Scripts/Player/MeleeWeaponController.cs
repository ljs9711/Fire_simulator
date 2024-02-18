using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract 추상 클래스 / 다른 근접 무기에서 기능을 완성할 것
public abstract class MeleeWeaponController : MonoBehaviour
{
    // 현재 장착된 Hand 형태의 무기
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    // 공격 중?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    // 공격 시도 메서드
    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine()); // 코루틴 시작
            }
        }
    }

    // 공격 코루틴
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger("Attack"); // Animator 내의 Attack 트리거 활성화

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_in); // 공격 히트 지점 활성화 지연
        isSwing = true;
        StartCoroutine(HitCoroutine()); // 적중 여부 체크 코루틴 시작

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_out); // 공격 히트 지점 비활성화 지연
        isSwing = false;

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelay_in - currentMeleeWeapon.attackDelay_out); // 공격 자체 비활성화
        isAttack = false;
    }

    // 적중 여부 체크 코루틴 (추상 메서드)
    protected abstract IEnumerator HitCoroutine();

    // 물체 충돌 체크
    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
        {
            return true; // 충돌한 경우 true 반환
        }
        return false; // 충돌하지 않은 경우 false 반환
    }

    // 완성했지만 수정 가능한 함수 (가상 메서드)
    public virtual void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        if (WeaponManager.currentWeapon != null) // 무언가 들고 있는 경우
            WeaponManager.currentWeapon.gameObject.SetActive(false); // 현재 무기 비활성화

        currentMeleeWeapon = _MeleeWeapon; // 새로운 무기로 교체
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero; // 위치 초기화
        currentMeleeWeapon.gameObject.SetActive(true); // 새로운 무기 활성화
    }
}
