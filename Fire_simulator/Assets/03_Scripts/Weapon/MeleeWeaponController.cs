using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract �߻�Ŭ����/ �ٸ� �������⿡�� ��� �ϼ��Ұ�
public abstract class MeleeWeaponController : MonoBehaviour
{
    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    //������?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());//�ڷ�ƾ
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger("Attack"); //Animator �ȿ� �ִ� Attack Ʈ��Ŀ ���

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_in); //currentHand �� �ִ� attackDelay_in �� ��� (���� ��Ʈ ���� Ȱ��ȭ)
        isSwing = true;
        StartCoroutine(HitCoroutine()); //���߿��� �ڷ�ƾ

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay_out); //currentHand �� �ִ� attackDelay_out �� ��� (���� ��Ʈ ���� ��Ȱ��ȭ)
        isSwing = false;


        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelay_in - currentMeleeWeapon.attackDelay_out); //(���� ��ü ��Ȱ��ȭ)
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject() //���ǹ��̶� bool ���
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
        {
            return true;
        }
        return false;
    }

    //�ϼ������� ���� ������ �Լ� virtual
    public virtual void MeleeWeaponChange(MeleeWeapon _MeleeWeapon)
    {
        if (WeaponManager.currentWeapon != null) //���� ���������
            WeaponManager.currentWeapon.gameObject.SetActive(false); //���� ������Ʈ ��Ȱ��ȭ

        currentMeleeWeapon = _MeleeWeapon; //�ٲ� ���Ⱑ ���� ����� �ٲ�
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero; //��ġ �ٲ���� ���� ������ 0,0,0���� �ʱ�ȭ
        currentMeleeWeapon.gameObject.SetActive(true);
    }
}
