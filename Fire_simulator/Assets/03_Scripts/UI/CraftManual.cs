using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; // 제작품 이름
    public GameObject go_Prefab; // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab; // 미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    // 상태 변수
    private bool isActivated = false; // 메뉴 활성화 여부
    private bool isPreviewActivated = false; // 미리보기 활성화 여부

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; // 모닥불용 탭에 대한 제작품 배열
    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 설치될 프리팹

    [SerializeField]
    private Transform tf_Player; // 플레이어 위치

    // 레이캐스트를 사용하여 프리팹 위치를 따라가게 함
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    // 슬롯 클릭 시 실행되는 함수
    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false); // 기본 베이스 UI 비활성화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            Window(); // 창 열기

        if (isPreviewActivated)
            PreviewPositionUpdate(); // 미리보기 위치 업데이트

        if (Input.GetButtonDown("Fire1"))
            Build(); // 건설

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel(); // 취소
    }

    private void Build()
    {
        if (isPreviewActivated)
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity); // 프리팹 설치
            Destroy(go_Preview); // 미리보기 제거
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location; // 미리보기 위치 업데이트
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview); // 미리보기 제거

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false); // 기본 베이스 UI 비활성화
    }

    private void Window()
    {
        if (!isActivated)
            OpenWindow(); // 창 열기
        else
            CloseWindow(); // 창 닫기
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true); // 기본 베이스 UI 활성화
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false); // 기본 베이스 UI 비활성화
    }
}
