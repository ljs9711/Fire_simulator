using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // ���Կ� �ִ� ������
    public int itemCount; // ���Կ� �ִ� �������� ����
    public Image itemImage; // �������� �̹��� ǥ�ø� ���� �̹��� ������Ʈ

    // �ʿ��� UI ������Ʈ
    [SerializeField] private Text text_count; // ������ ������ ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] private GameObject go_CountImage; // ������ ������ ǥ���ϴ� �̹��� ������Ʈ

    // �̹����� ���� ������ ���� �Լ�
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �������� ���Կ� �߰��ϴ� �Լ�
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item; // ������ �Ҵ�
        itemCount = _count; // ������ ���� �Ҵ�
        itemImage.sprite = item.itemImage; // ������ �̹��� ����

        // ��� �������� �ƴ� ��쿡�� ������ ������ ǥ��
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true); // ������ ������ ǥ���ϴ� UI Ȱ��ȭ
            text_count.text = itemCount.ToString(); // ������ ������ �ؽ�Ʈ�� ǥ��
        }
        else
        {
            text_count.text = "0"; // ��� �������� ��� ������ ������ ����� �ؽ�Ʈ�� 0���� ����
            go_CountImage.SetActive(false); // ������ ������ ǥ���ϴ� UI ��Ȱ��ȭ
        }

        SetColor(1); // �̹��� ������ 1�� �����Ͽ� �������� ǥ�õǵ��� ��
    }

    // ���Կ� �ִ� ������ ������ �����ϴ� �Լ�
    public void SetSlotCount(int _count)
    {
        itemCount += _count; // ������ ���� ����
        text_count.text = itemCount.ToString(); // ����� ������ ������ �ؽ�Ʈ�� ������Ʈ

        // ������ ������ 0 �����̸� ������ �ʱ�ȭ�Ͽ� �������� ���� ���·� ����
        if (itemCount <= 0)
            ClearSlot();
    }

    // ������ �ʱ�ȭ�ϴ� �Լ� (�������� ���� ���·� ����)
    private void ClearSlot()
    {
        item = null; // ������ �Ҵ� ����
        itemCount = 0; // ������ ���� �ʱ�ȭ
        itemImage.sprite = null; // ������ �̹��� ����
        SetColor(0); // �̹��� ������ 0���� �����Ͽ� �������� ����

        text_count.text = "0"; // �ؽ�Ʈ�� 0���� �����Ͽ� ������ ���� ����
        go_CountImage.SetActive(false); // ������ ������ ǥ���ϴ� UI ��Ȱ��ȭ
    }
}
