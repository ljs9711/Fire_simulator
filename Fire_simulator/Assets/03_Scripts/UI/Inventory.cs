using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField] private GameObject go_InventoryBase;
    [SerializeField] private GameObject go_SlotsParent;

    // 슬롯들
    private Slot[] slots;

    public Slot[] GetSlots() { return slots; }

    [SerializeField] private Item[] items;

    // 인벤토리에 아이템 로드
    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                slots[_arrayNum].AddItem(items[i], _itemNum);
                break;
            }
        }
    }

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    // 인벤토리 열기 시도
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    // 인벤토리 열기
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    // 인벤토리 닫기
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    // 아이템 획득
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 장비 아이템이 아닌 경우에만 처리
        if (_item.itemType != Item.ItemType.Equipment)
        {
            // 이미 같은 아이템이 있으면 개수만 증가
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item.itemName == _item.itemName)
                {
                    slots[i].SetSlotCount(_count);
                    return;
                }
            }
        }

        // 빈 슬롯에 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
