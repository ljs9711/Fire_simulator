using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }




    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; //60���� 1 = 1
        }
    }


    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }


    private void Fire() //�߻���
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }


    private void Shoot() //�߻���
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("�Ѿ� �߻���");
    }

    private void PlaySE(AudioClip _clip) //�Ҹ� �־� ������ ���
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
