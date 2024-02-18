using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // 슬롯에 있는 아이템
    public int itemCount; // 슬롯에 있는 아이템의 개수
    public Image itemImage; // 아이템의 이미지 표시를 위한 이미지 컴포넌트

    // 필요한 UI 컴포넌트
    [SerializeField] private Text text_count; // 아이템 개수를 표시하는 텍스트
    [SerializeField] private GameObject go_CountImage; // 아이템 개수를 표시하는 이미지 오브젝트

    // 이미지의 투명도 조절을 위한 함수
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템을 슬롯에 추가하는 함수
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item; // 아이템 할당
        itemCount = _count; // 아이템 개수 할당
        itemImage.sprite = item.itemImage; // 아이템 이미지 설정

        // 장비 아이템이 아닌 경우에만 아이템 개수를 표시
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true); // 아이템 개수를 표시하는 UI 활성화
            text_count.text = itemCount.ToString(); // 아이템 개수를 텍스트로 표시
        }
        else
        {
            text_count.text = "0"; // 장비 아이템인 경우 아이템 개수를 숨기고 텍스트를 0으로 설정
            go_CountImage.SetActive(false); // 아이템 개수를 표시하는 UI 비활성화
        }

        SetColor(1); // 이미지 투명도를 1로 설정하여 아이템이 표시되도록 함
    }

    // 슬롯에 있는 아이템 개수를 조정하는 함수
    public void SetSlotCount(int _count)
    {
        itemCount += _count; // 아이템 개수 변경
        text_count.text = itemCount.ToString(); // 변경된 아이템 개수를 텍스트로 업데이트

        // 아이템 개수가 0 이하이면 슬롯을 초기화하여 아이템을 없는 상태로 만듦
        if (itemCount <= 0)
            ClearSlot();
    }

    // 슬롯을 초기화하는 함수 (아이템이 없는 상태로 만듦)
    private void ClearSlot()
    {
        item = null; // 아이템 할당 해제
        itemCount = 0; // 아이템 개수 초기화
        itemImage.sprite = null; // 아이템 이미지 해제
        SetColor(0); // 이미지 투명도를 0으로 설정하여 아이템을 숨김

        text_count.text = "0"; // 텍스트를 0으로 설정하여 아이템 개수 숨김
        go_CountImage.SetActive(false); // 아이템 개수를 표시하는 UI 비활성화
    }
}
