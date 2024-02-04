using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameScene";

    public void ClickStart()
    {
        Debug.Log("���ӽ� ����");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickExit()
    {
        Debug.Log("����");
        Application.Quit();
    }
}
