using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW Item", menuName = "NEW Item/item")]
public class Item : ScriptableObject //스크립트를 안붙혀도 됨
{
    public string itemNmae; //아이템의 이름
    public ItemType itemType; //아이템의 유형
    public Sprite itemImage; //아이템의 이미지
    public GameObject itemPrefab; // 아이템의 프리팹

    public string weaponType; //무기 유형

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }


}
