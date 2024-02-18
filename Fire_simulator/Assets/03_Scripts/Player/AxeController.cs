using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    void Update()
    {
        // 활성화되었을 때만 공격 시도
        if (isActivate)
            TryAttack();
    }

    // 공격 동작 코루틴
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            // 충돌 여부 확인
            if (CheckObject())
            {
                // 충돌한 대상이 "Wood" 태그를 가지고 있는지 확인 후 채굴 시도
                if (hitInfo.transform.CompareTag("Wood"))
                {
                    hitInfo.transform.GetComponent<Wood>().Mining();
                }

                // 공격 동작 종료
                isSwing = false;
                Debug.Log(hitInfo.transform.name); // 충돌한 대상의 이름 로그 출력
            }
            yield return null;
        }
    }

    // 무기 변경 시 호출되는 함수
    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon);
        isActivate = true; // 무기가 활성화됨
    }
}
