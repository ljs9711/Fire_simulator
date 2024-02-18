using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 획득 가능한 최대 거리

    private bool pickupActivated = false; // 획득 가능 여부

    private RaycastHit hitInfo; // 충돌체 정보 저장

    // 아이템 레이어에만 반응하도록 레이어 설정
    [SerializeField]
    private LayerMask layerMask;

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText; // 상호작용 액션 텍스트
    [SerializeField]
    private Inventory theInventory; // 인벤토리

    void Update()
    {
        CheckItem(); // 아이템 체크
        TryAction(); // 행동 시도
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E 키 입력 시
        {
            CheckItem(); // 아이템 체크
            CanPickup(); // 아이템 획득 시도
        }
    }

    private void CanPickup()
    {
        if (pickupActivated) // 획득 가능한 상태라면
        {
            if (hitInfo.transform != null && hitInfo.transform.CompareTag("Item")) // 충돌체가 아이템 태그를 가지고 있다면
            {
                Item item = hitInfo.transform.GetComponent<ItemPickup>().item; // 획득할 아이템 가져오기
                Debug.Log(item.itemName + "을(를) 획득했습니다");
                theInventory.AcquireItem(item); // 아이템 인벤토리에 추가
                Destroy(hitInfo.transform.gameObject); // 아이템 게임 오브젝트 파괴
                InfoDisappear(); // 획득 텍스트 숨기기
            }
        }
    }

    // 아이템 체크
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask)) // 플레이어 위치에서 정면으로 레이캐스트 발사
        {
            if (hitInfo.transform.CompareTag("Item")) // 충돌체가 아이템 태그를 가지고 있다면
            {
                ItemInfoAppear(); // 아이템 획득 텍스트 출력
            }
        }
        else
        {
            InfoDisappear(); // 아이템 획득 텍스트 숨기기
        }
    }

    // 아이템 획득 텍스트 출력
    private void ItemInfoAppear()
    {
        pickupActivated = true; // 획득 가능 상태로 설정
        actionText.gameObject.SetActive(true); // 액션 텍스트 활성화
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>"; // 텍스트 설정
    }

    // 아이템 획득 텍스트 숨기기
    private void InfoDisappear()
    {
        pickupActivated = false; // 획득 불가능 상태로 설정
        actionText.gameObject.SetActive(false); // 액션 텍스트 비활성화
    }
}
