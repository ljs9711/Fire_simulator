using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    void Update()
    {
        // Ȱ��ȭ�Ǿ��� ���� ���� �õ�
        if (isActivate)
            TryAttack();
    }

    // ���� ���� �ڷ�ƾ
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            // �浹 ���� Ȯ��
            if (CheckObject())
            {
                // �浹�� ����� "Wood" �±׸� ������ �ִ��� Ȯ�� �� ä�� �õ�
                if (hitInfo.transform.CompareTag("Wood"))
                {
                    hitInfo.transform.GetComponent<Wood>().Mining();
                }

                // ���� ���� ����
                isSwing = false;
                Debug.Log(hitInfo.transform.name); // �浹�� ����� �̸� �α� ���
            }
            yield return null;
        }
    }

    // ���� ���� �� ȣ��Ǵ� �Լ�
    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon);
        isActivate = true; // ���Ⱑ Ȱ��ȭ��
    }
}
