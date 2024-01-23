using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = true;

    //현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    //연사속도 계산
    private float currentFireRate;

    //상태변수
    private bool isReload = false;
    public bool isFineSightMode = false;

    //정조준 전 포지션 값
    private Vector3 originPos; 

    //효과음
    private AudioSource audioSource;

    //레이저 충돌 정보를 받아옴
    private RaycastHit hitInfo;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //피격이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;



    void Start()
    {
        //originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();

        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

    }

    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
            
    }



    //연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; //60분의 1 = 1
        }
    }

    // 발사 시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //발사전 계산
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

    //발사후 계산
    private void Shoot() //발사후
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--; //총알 감소
        currentFireRate = currentGun.fireRate; //연사 속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine()); //총기반동 코루틴

    }


    private void Hit()
    {                       //현재 위치에서             forward 로 쏘는데
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward 
            //랜덤값을 foward로 더해줌.(총 정확도)         X축 최소 값                                          X축 최대 값
            + new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                                                         //Y축 최소 값                                          Y축 최대 값
                          Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                          0)
            , out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); //피격 이펙트 생성
            Destroy(clone, 2f);
        }
    }


    //재장전 시도
    private void TryReload()
    {   //R키 누르면 reload 실행 / isReload가 false 일때만 / 현재 탄창이 reload 할 탄창보다 작을경우에만 실행
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }


    //재장전
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

    //정조준 시도
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    //정조준 취소
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }


    //정조준 로직 가동
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

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

    //정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos) //정조준 자세일때까지 반복
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f); //정조준 vector3 값
            yield return null; //1프레임 대기
        }

    }

    //정조준 비활성화
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos) //원래 자세일때까지 반복
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f); //원래 vector3 값

            yield return null; //1프레임 대기
        }
    }


    //반동 코루틴
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


    //사운드 재생
    private void PlaySE(AudioClip _clip) //소리 넣어 놓은거 재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFindSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null) //무언가 들고있으면
            WeaponManager.currentWeapon.gameObject.SetActive(false); //게임 오브젝트 비활성화

        currentGun = _gun; //바꿀 무기가 현재 무기로 바뀜
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero; //위치 바뀌었을 수도 있으니 0,0,0으로 초기화
        currentGun.gameObject.SetActive(true);
        isActivate = true; //무기 활성화 여부
    }
}
