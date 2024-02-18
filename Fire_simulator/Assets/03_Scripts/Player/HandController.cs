using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MeleeWeaponController
{
    // 활성화 여부
    public static bool isActivate = true;

    void Start()
    {
        // 현재 무기를 WeaponManager에 등록
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;
    }

    void Update()
    {
        // 활성화되었을 때만 공격 시도
        if (isActivate)
            TryAttack();
    }

    // 공격 코루틴
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            // 무기가 충돌했는지 확인하고, 충돌했다면 충돌 정보 출력
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    // 무기 교체 메서드
    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon);
        isActivate = true;
    }
}
