using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;

    private PlayerController playerController; // PlayerController 스크립트 참조 변수
    //private WeaponSway weaponSway;

    private void Start()
    {
        // PlayerController 스크립트를 찾아서 참조
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
                PauseGame();
            else
                ResumeGame();
        }
    }

    private void PauseGame()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f; // 일시정지

        // 캐릭터 움직임을 일시정지
        if (playerController != null)
            playerController.PauseMovement();

        // WeaponSway의 sway 효과 일시정지
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.PauseSway();
        }
    }

    private void ResumeGame()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f; // 일시정지 해제

        // 캐릭터 움직임을 다시 활성화
        if (playerController != null)
            playerController.ResumeMovement();

        // WeaponSway의 sway 효과 해제
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.ResumeSway();
        }
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
    }

    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
