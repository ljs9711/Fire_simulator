using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameScene";

    private SaveAndLoad theSaveAndLoad;
    //�̱���//
    public static Title instance;
    
    private void Awake()
    {
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
        
    }

    public void ClickStart()
    {
        Debug.Log("���ӽ� ����");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
        StartCoroutine(LoadCoroutine()); 
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            yield return null;
        }

        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        theSaveAndLoad.LoadData();
        Destroy(gameObject);
    }

    public void ClickExit()
    {
        Debug.Log("����");
        Application.Quit();
    }
}
