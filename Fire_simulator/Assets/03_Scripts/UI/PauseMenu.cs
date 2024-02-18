using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI; // �Ͻ����� �޴��� �⺻ UI
    [SerializeField] private SaveAndLoad theSaveAndLoad; // SaveAndLoad ��ũ��Ʈ ���� ����

    private PlayerController playerController; // PlayerController ��ũ��Ʈ ���� ����

    private void Start()
    {
        // PlayerController ��ũ��Ʈ�� ã�Ƽ� ����
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

    // ���� �Ͻ�����
    private void PauseGame()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f; // �ð� �帧 ����

        // ĳ���� ������ �Ͻ�����
        if (playerController != null)
            playerController.PauseMovement();

        // ��� ������ sway ȿ�� �Ͻ�����
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.PauseSway();
        }
    }

    // ���� �簳
    private void ResumeGame()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f; // �ð� �帧 �簳

        // ĳ���� ������ �ٽ� Ȱ��ȭ
        if (playerController != null)
            playerController.ResumeMovement();

        // ��� ������ sway ȿ�� ����
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.ResumeSway();
        }
    }

    // ���� ��ư Ŭ�� �� ȣ��� �Լ�
    public void ClickSave()
    {
        Debug.Log("���̺�");
        theSaveAndLoad.SaveData();
    }

    // �ε� ��ư Ŭ�� �� ȣ��� �Լ�
    public void ClickLoad()
    {
        Debug.Log("�ε�");
        theSaveAndLoad.LoadData();
    }

    // ���� ��ư Ŭ�� �� ȣ��� �Լ�
    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }
}
