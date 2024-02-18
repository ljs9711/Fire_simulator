using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI; // 일시정지 메뉴의 기본 UI
    [SerializeField] private SaveAndLoad theSaveAndLoad; // SaveAndLoad 스크립트 참조 변수

    private PlayerController playerController; // PlayerController 스크립트 참조 변수

    private void Start()
    {
        // PlayerController 스크립트를 찾아서 참조
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isPause)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // 게임 일시정지
    private void PauseGame()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f; // 시간 흐름 멈춤

        // 캐릭터 움직임 일시정지
        if (playerController != null)
            playerController.PauseMovement();

        // 모든 무기의 sway 효과 일시정지
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.PauseSway();
        }
    }

    // 게임 재개
    private void ResumeGame()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f; // 시간 흐름 재개

        // 캐릭터 움직임 다시 활성화
        if (playerController != null)
            playerController.ResumeMovement();

        // 모든 무기의 sway 효과 해제
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.ResumeSway();
        }
    }

    // 저장 버튼 클릭 시 호출될 함수
    public void ClickSave()
    {
        Debug.Log("세이브");
        theSaveAndLoad.SaveData();
    }

    // 로드 버튼 클릭 시 호출될 함수
    public void ClickLoad()
    {
        Debug.Log("로드");
        theSaveAndLoad.LoadData();
    }

    // 종료 버튼 클릭 시 호출될 함수
    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
