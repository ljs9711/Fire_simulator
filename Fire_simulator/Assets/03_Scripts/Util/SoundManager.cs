using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 곡 이름
    public AudioClip clip; // 곡
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    static public SoundManager instance; // 싱글톤 인스턴스

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
            Destroy(gameObject); // 이미 존재하는 경우 중복 생성 방지
    }
    #endregion Singleton

    public AudioSource[] audioSourcesEffects; // 효과음용 오디오 소스 배열
    public AudioSource[] audioSourcesBGM; // 배경음용 오디오 소스 배열

    public string[] playSoundName; // 재생 중인 사운드 이름 배열

    public Sound[] effectSounds; // 효과음 배열
    public Sound[] bgmSounds; // 배경음 배열

    void Start()
    {
        playSoundName = new string[audioSourcesEffects.Length]; // 재생 중인 사운드 이름 배열 초기화
    }

    // 효과음 재생 함수
    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name) // 입력한 이름과 일치하는 효과음 찾기
            {
                for (int j = 0; j < audioSourcesEffects.Length; j++)
                {
                    if (!audioSourcesEffects[j].isPlaying) // 재생 중이지 않은 오디오 소스 찾기
                    {
                        playSoundName[j] = effectSounds[i].name; // 재생 중인 사운드 이름 배열에 등록
                        audioSourcesEffects[j].clip = effectSounds[i].clip; // 해당 오디오 소스에 클립 설정
                        audioSourcesEffects[j].Play(); // 효과음 재생
                        return;
                    }
                }
                Debug.Log("모든 AudioSource가 사용 중입니다."); // 모든 오디오 소스가 사용 중일 경우 디버그 출력
                return;
            }
            Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다."); // 등록되지 않은 사운드일 경우 디버그 출력
        }
    }

    // 모든 효과음 정지 함수
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop(); // 모든 효과음 정지
        }
    }

    // 특정 효과음 정지 함수
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if (playSoundName[i] == _name) // 재생 중인 사운드 이름과 입력한 이름이 일치할 경우
            {
                audioSourcesEffects[i].Stop(); // 해당 효과음 정지
                return;
            }
        }
        Debug.Log("재생 중인 " + _name + " 사운드가 없습니다."); // 재생 중인 사운드가 없을 경우 디버그 출력
    }
}
