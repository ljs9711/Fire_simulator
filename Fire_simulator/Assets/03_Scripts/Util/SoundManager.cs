using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // �� �̸�
    public AudioClip clip; // ��
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    static public SoundManager instance; // �̱��� �ν��Ͻ�

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
            Destroy(gameObject); // �̹� �����ϴ� ��� �ߺ� ���� ����
    }
    #endregion Singleton

    public AudioSource[] audioSourcesEffects; // ȿ������ ����� �ҽ� �迭
    public AudioSource[] audioSourcesBGM; // ������� ����� �ҽ� �迭

    public string[] playSoundName; // ��� ���� ���� �̸� �迭

    public Sound[] effectSounds; // ȿ���� �迭
    public Sound[] bgmSounds; // ����� �迭

    void Start()
    {
        playSoundName = new string[audioSourcesEffects.Length]; // ��� ���� ���� �̸� �迭 �ʱ�ȭ
    }

    // ȿ���� ��� �Լ�
    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name) // �Է��� �̸��� ��ġ�ϴ� ȿ���� ã��
            {
                for (int j = 0; j < audioSourcesEffects.Length; j++)
                {
                    if (!audioSourcesEffects[j].isPlaying) // ��� ������ ���� ����� �ҽ� ã��
                    {
                        playSoundName[j] = effectSounds[i].name; // ��� ���� ���� �̸� �迭�� ���
                        audioSourcesEffects[j].clip = effectSounds[i].clip; // �ش� ����� �ҽ��� Ŭ�� ����
                        audioSourcesEffects[j].Play(); // ȿ���� ���
                        return;
                    }
                }
                Debug.Log("��� AudioSource�� ��� ���Դϴ�."); // ��� ����� �ҽ��� ��� ���� ��� ����� ���
                return;
            }
            Debug.Log(_name + " ���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�."); // ��ϵ��� ���� ������ ��� ����� ���
        }
    }

    // ��� ȿ���� ���� �Լ�
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop(); // ��� ȿ���� ����
        }
    }

    // Ư�� ȿ���� ���� �Լ�
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if (playSoundName[i] == _name) // ��� ���� ���� �̸��� �Է��� �̸��� ��ġ�� ���
            {
                audioSourcesEffects[i].Stop(); // �ش� ȿ���� ����
                return;
            }
        }
        Debug.Log("��� ���� " + _name + " ���尡 �����ϴ�."); // ��� ���� ���尡 ���� ��� ����� ���
    }
}
