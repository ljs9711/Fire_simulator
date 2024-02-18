using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    // 플레이어 값
    public Vector3 playerPos; // 플레이어 위치
    public Vector3 playerRot; // 플레이어 회전

    // 인벤토리 리스트 값
    public List<int> invenArrayNumber = new List<int>(); // 인벤토리 슬롯 인덱스
    public List<string> invenItemName = new List<string>(); // 아이템 이름
    public List<int> invenItemNumber = new List<int>(); // 아이템 개수
}

public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData(); // 저장된 데이터를 담을 변수

    private string SAVE_DATA_DIRECTORY; // 저장된 데이터의 디렉토리
    private string SAVE_FILENAME = "/SaveFile.txt"; // 저장된 파일명

    private PlayerController thePlayer; // 플레이어 컨트롤러
    private Inventory theInven; // 인벤토리

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; // 저장된 데이터의 디렉토리 경로

        // 디렉토리가 없으면 자동으로 생성
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    // 데이터 저장 함수
    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerController>(); // 플레이어 컨트롤러 찾기
        theInven = FindObjectOfType<Inventory>(); // 인벤토리 찾기

        // 플레이어 값 저장
        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        // 인벤토리 저장
        Slot[] slots = theInven.GetSlots(); // 인벤토리의 슬롯들 가져오기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) // 슬롯에 아이템이 있다면
            {
                saveData.invenArrayNumber.Add(i); // 슬롯 인덱스 추가
                saveData.invenItemName.Add(slots[i].item.itemName); // 아이템 이름 추가
                saveData.invenItemNumber.Add(slots[i].itemCount); // 아이템 개수 추가
            }
        }

        // 플레이어 위치를 Json으로 변환
        string json = JsonUtility.ToJson(saveData);

        // SAVE_DATA_DIRECTORY + SAVE_FILENAME 경로에 Json화된 플레이어 위치값 저장
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    // 데이터 불러오기 함수
    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME)) // 저장된 파일이 존재하면
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME); // 파일 내용 읽기
            saveData = JsonUtility.FromJson<SaveData>(loadJson); // Json을 SaveData 형식으로 변환

            thePlayer = FindObjectOfType<PlayerController>(); // 플레이어 컨트롤러 찾기
            theInven = FindObjectOfType<Inventory>(); // 인벤토리 찾기

            // 로드된 위치로 플레이어 이동
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            // 인벤토리 아이템 로드
            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

            Debug.Log("로드 완료");
        }
        else
            Debug.Log("세이브 파일이 없음");
    }
}
