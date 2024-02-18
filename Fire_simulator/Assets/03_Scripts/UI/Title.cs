using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameScene";

    private SaveAndLoad theSaveAndLoad;

    // �̱��� �ν��Ͻ�
    public static Title instance;

    private void Awake()
    {
        // ���� �ν��Ͻ��� ���� ���, �ڽ��� �ν��Ͻ��� �����ϰ� �ı����� �ʵ��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���� �ν��Ͻ��� �����ϴ� ���, �ڽ��� �ı��Ͽ� �ߺ� ������ ����
            Destroy(gameObject);
        }
    }

    public void ClickStart()
    {
        Debug.Log("���� ����");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        // �񵿱������� �� �ε带 ����
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // �� �ε尡 �Ϸ�� ������ ���
        while (!operation.isDone)
        {
            yield return null;
        }

        // ���� �ε�� �Ŀ� SaveAndLoad ������Ʈ�� ã�� �ε� ������ ����
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        if (theSaveAndLoad != null)
        {
            theSaveAndLoad.LoadData();
        }
        else
        {
            Debug.LogWarning("SaveAndLoad ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // �ڽ��� �ı��Ͽ� �ߺ� ������ ����
        Destroy(gameObject);
    }

    public void ClickExit()
    {
        Debug.Log("����");
        Application.Quit();
    }
}
