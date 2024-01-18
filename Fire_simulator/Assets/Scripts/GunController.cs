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
            currentFireRate -= Time.deltaTime; //60분의 1 = 1
        }
    }


    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }


    private void Fire() //발사전
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }


    private void Shoot() //발사후
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");
    }

    private void PlaySE(AudioClip _clip) //소리 넣어 놓은거 재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
