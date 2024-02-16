using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    //Ȱ��ȭ ����
    public static bool isActivate = false;

    void Update()
    {
        if (isActivate)
            TryAttack();
    }


    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Wood")
                {
                    hitInfo.transform.GetComponent<Wood>().Mining();
                }

                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon);
        isActivate = true;
    }
}
