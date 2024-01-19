using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;
    private bool isFineSightMode = false;


    [SerializeField]
    private Vector3 originPos; //정조준 전 포지션 값

    private AudioSource audioSource;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
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
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }


    private void Fire() //발사전
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
                
        }
    }


    private void Shoot() //발사후
    {
        currentGun.currentBulletCount--; //총알 감소
        currentFireRate = currentGun.fireRate; //연사 속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine()); //총기반동 코루틴

    }

    private void TryReload()
    {   //R키 누르면 reload 실행 / isReload가 false 일때만 / 현재 탄창이 reload 할 탄창보다 작을경우에만 실행
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }


    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount; //장전하면 탄알집에 남은 총알은 보유총알로 넘어감
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount) //현재 보유한 총알이 장전할 총알보다 크면
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; //탄알집에 reloadBulletCount:재장전함
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; //그리고 현재 보유한 총알에서 재장전한 만큼 뺌
            }
            else //보유한 총알이 30발 미만일때
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; //탄창에 보유한 총알 재장전하고
                currentGun.carryBulletCount = 0; // 보유한 총알 0 으로 변경
            }
            isReload = false;
        }
        else
        {
            Debug.Log("총알 없음");
        }
    }

    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }


    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines(); //Lerp 값이 딱 안떨어져서 while문 못빠져나와 중복실행됨. 코루틴 꺼줘야함
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }

    }

    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos) //정조준 자세일때까지 반복
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f); //정조준 vector3 값
            yield return null; //1프레임 대기
        }

    }
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos) //원래 자세일때까지 반복
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f); //원래 vector3 값

            yield return null; //1프레임 대기
        }
    }

    IEnumerator RetroActionCoroutine() //반동
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, -currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y, -currentGun.retroActionFineSightForce);

        if (!isFineSightMode) //정조준 아닌 상태 반동
        {
            currentGun.transform.localPosition = originPos; //제자리 왔다가 다시 반동주기 위함

            //반동시작
            while (Vector3.Distance(currentGun.transform.localPosition, recoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 위치를 정확하게 일치시키기
            currentGun.transform.localPosition = originPos;
        }
        else //정조준 상태 반동
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos; //제자리 왔다가 다시 반동주기 위함

            //반동시작
            while (Vector3.Distance(currentGun.transform.localPosition, retroActionRecoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 위치를 정확하게 일치시키기
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
        }
    }



    private void PlaySE(AudioClip _clip) //소리 넣어 놓은거 재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
