using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW Item", menuName = "NEW Item/item")]
public class Item : ScriptableObject
{
    public string itemName; // �������� �̸�
    public ItemType itemType; // �������� ����
    public Sprite itemImage; // �������� �̹���
    public GameObject itemPrefab; // �������� ������

    public string weaponType; // ���� ����

    public enum ItemType
    {
        Equipment,   // ��� ������
        Used,        // �Ҹ�ǰ ������
        Ingredient,  // ��� ������
        ETC          // ��Ÿ ������
    }
}
