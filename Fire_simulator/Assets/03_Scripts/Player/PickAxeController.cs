using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : MeleeWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    void Update()
    {
        // Ȱ��ȭ�Ǿ� ������ ���� �õ�
        if (isActivate)
            TryAttack();
    }

    // ���� �ڷ�ƾ ����
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            // �浹�� ��ü�� ������
            if (CheckObject())
            {
                // �浹�� ��ü�� 'Rock' �±׸� ������ �ִٸ�
                if (hitInfo.transform.CompareTag("Rock"))
                {
                    // �ش��ϴ� Rock ��ũ��Ʈ�� Mining() �޼��� ȣ��
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }

                // ���� �Ϸ� �� isSwing�� false�� �����Ͽ� �ݺ� ����
                isSwing = false;
                Debug.Log(hitInfo.transform.name); // �浹�� ������Ʈ�� �̸��� ���
            }
            yield return null; // �� ������ ���
        }
    }

    // ���� ��ü �� ȣ��Ǵ� �޼���
    public override void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        base.MeleeWeaponChange(_MeleeWeapon); // �θ� Ŭ������ MeleeWeaponChange �޼��� ȣ��
        isActivate = true; // ���Ⱑ Ȱ��ȭ��
    }
}
