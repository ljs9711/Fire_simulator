using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;

    private PlayerController playerController; // PlayerController ��ũ��Ʈ ���� ����
    //private WeaponSway weaponSway;

    private void Start()
    {
        // PlayerController ��ũ��Ʈ�� ã�Ƽ� ����
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
        Time.timeScale = 0f; // �Ͻ�����

        // ĳ���� �������� �Ͻ�����
        if (playerController != null)
            playerController.PauseMovement();

        // WeaponSway�� sway ȿ�� �Ͻ�����
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
        Time.timeScale = 1f; // �Ͻ����� ����

        // ĳ���� �������� �ٽ� Ȱ��ȭ
        if (playerController != null)
            playerController.ResumeMovement();

        // WeaponSway�� sway ȿ�� ����
        WeaponSway[] weaponSways = FindObjectsOfType<WeaponSway>();
        foreach (var sway in weaponSways)
        {
            sway.ResumeSway();
        }
    }

    public void ClickSave()
    {
        Debug.Log("���̺�");
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
