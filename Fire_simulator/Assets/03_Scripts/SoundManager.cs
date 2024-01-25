using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //�� �̸�
    public AudioClip clip;// ��
}
public class SoundManager : MonoBehaviour
{

    #region singleton
    static public SoundManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion singleton

    public AudioSource[] audioSourcesEffects;
    public AudioSource[] audioSourcesBGM;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;


    void Start()
    {
        playSoundName = new string[audioSourcesEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourcesEffects.Length; j++)
                {
                    if (!audioSourcesEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourcesEffects[j].clip = effectSounds[i].clip;
                        audioSourcesEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� AudioSource�� ������Դϴ�."); //������ҽ� ���� ������̸� �ø��� ���� �����
                return;
            }
            Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");//����Ŵ����� ��ġ�ϴ� ���尡 ����
        }
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if(playSoundName[i] == _name)
            {
                audioSourcesEffects[i].Stop();
                return;
            }
        }
        Debug.Log("�������" + _name + "���尡 �����ϴ�.");
    }
}
