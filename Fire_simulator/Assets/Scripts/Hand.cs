using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand: MonoBehaviour
{

    public string handName; //�Ǽ����� �ƴ��� ����
    public float range; //���� ����
    public int damage; // ���ݷ�
    public float workSpeed; //�۾� �ӵ�
    public float attackDelay; //���� ������
    public float attackDelay_in; //���� Ȱ��ȭ ����
    public float attackDelay_out; //���� ��Ȱ��ȭ ����

    public Animator anim; //�ִϸ��̼�
}
