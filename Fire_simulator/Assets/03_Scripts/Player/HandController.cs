using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MeleeWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = true;

    void Start()
    {
        // ���� ���⸦ WeaponManager�� ���
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;
    }

    void Update()
    {
        // Ȱ��ȭ�Ǿ��� ���� ���� �õ�
        if (isActivate)
            TryAttack();
    }

    // ���� �ڷ�ƾ
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            // ���Ⱑ �浹�ߴ��� Ȯ���ϰ�, �浹�ߴٸ� �浹 ���� ���
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    // ���� ��ü �޼���
    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon);
        isActivate = true;
    }
}
