using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : MeleeWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    void Update()
    {
        // 활성화되어 있으면 공격 시도
        if (isActivate)
            TryAttack();
    }

    // 적중 코루틴 구현
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            // 충돌한 물체가 있으면
            if (CheckObject())
            {
                // 충돌한 물체가 'Rock' 태그를 가지고 있다면
                if (hitInfo.transform.CompareTag("Rock"))
                {
                    // 해당하는 Rock 스크립트의 Mining() 메서드 호출
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }

                // 공격 완료 후 isSwing을 false로 변경하여 반복 종료
                isSwing = false;
                Debug.Log(hitInfo.transform.name); // 충돌한 오브젝트의 이름을 출력
            }
            yield return null; // 한 프레임 대기
        }
    }

    // 무기 교체 시 호출되는 메서드
    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon); // 부모 클래스의 MeleeWeaponChange 메서드 호출
        isActivate = true; // 무기가 활성화됨
    }
}
