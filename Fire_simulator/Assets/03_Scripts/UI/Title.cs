using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameScene";

    private SaveAndLoad theSaveAndLoad;

    // 싱글턴 인스턴스
    public static Title instance;

    private void Awake()
    {
        // 현재 인스턴스가 없는 경우, 자신을 인스턴스로 설정하고 파괴되지 않도록 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 현재 인스턴스가 존재하는 경우, 자신을 파괴하여 중복 생성을 방지
            Destroy(gameObject);
        }
    }

    public void ClickStart()
    {
        Debug.Log("게임 시작");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        // 비동기적으로 씬 로드를 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 씬 로드가 완료될 때까지 대기
        while (!operation.isDone)
        {
            yield return null;
        }

        // 씬이 로드된 후에 SaveAndLoad 컴포넌트를 찾아 로드 데이터 수행
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        if (theSaveAndLoad != null)
        {
            theSaveAndLoad.LoadData();
        }
        else
        {
            Debug.LogWarning("SaveAndLoad 컴포넌트를 찾을 수 없습니다.");
        }

        // 자신을 파괴하여 중복 생성을 방지
        Destroy(gameObject);
    }

    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}
