using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon: MonoBehaviour
{

    public string MeleeWeaponName; //��������

    //�������� ����
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;
   

    public float range; //���� ����
    public int damage; // ���ݷ�
    public float workSpeed; //�۾� �ӵ�
    public float attackDelay; //���� ������
    public float attackDelay_in; //���� Ȱ��ȭ ����
    public float attackDelay_out; //���� ��Ȱ��ȭ ����

    public Animator anim; //�ִϸ��̼�
}
