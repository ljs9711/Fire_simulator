using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract �߻� Ŭ���� / �ٸ� ���� ���⿡�� ����� �ϼ��� ��
public abstract class MeleeWeaponController : MonoBehaviour
{
    // ���� ������ Hand ������ ����
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    // ���� ��?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    // ���� �õ� �޼���
    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine()); // �ڷ�ƾ ����
            }
        }
    }

    // ���� �ڷ�ƾ
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger("Attack"); // Animator ���� Attack Ʈ���� Ȱ��ȭ

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_in); // ���� ��Ʈ ���� Ȱ��ȭ ����
        isSwing = true;
        StartCoroutine(HitCoroutine()); // ���� ���� üũ �ڷ�ƾ ����

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_out); // ���� ��Ʈ ���� ��Ȱ��ȭ ����
        isSwing = false;

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelay_in - currentMeleeWeapon.attackDelay_out); // ���� ��ü ��Ȱ��ȭ
        isAttack = false;
    }

    // ���� ���� üũ �ڷ�ƾ (�߻� �޼���)
    protected abstract IEnumerator HitCoroutine();

    // ��ü �浹 üũ
    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
        {
            return true; // �浹�� ��� true ��ȯ
        }
        return false; // �浹���� ���� ��� false ��ȯ
    }

    // �ϼ������� ���� ������ �Լ� (���� �޼���)
    public virtual void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        if (WeaponManager.currentWeapon != null) // ���� ��� �ִ� ���
            WeaponManager.currentWeapon.gameObject.SetActive(false); // ���� ���� ��Ȱ��ȭ

        currentMeleeWeapon = _MeleeWeapon; // ���ο� ����� ��ü
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero; // ��ġ �ʱ�ȭ
        currentMeleeWeapon.gameObject.SetActive(true); // ���ο� ���� Ȱ��ȭ
    }
}
