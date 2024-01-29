using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW Item", menuName = "NEW Item/item")]
public class Item : ScriptableObject //��ũ��Ʈ�� �Ⱥ����� ��
{
    public string itemNmae; //�������� �̸�
    public ItemType itemType; //�������� ����
    public Sprite itemImage; //�������� �̹���
    public GameObject itemPrefab; // �������� ������

    public string weaponType; //���� ����

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }


}
