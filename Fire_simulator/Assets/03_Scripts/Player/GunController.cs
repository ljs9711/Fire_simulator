using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = false;

    // 현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    // 연사속도 계산
    private float currentFireRate;

    // 상태변수
    private bool isReload = false; // 재장전 중인지 여부
    public bool isFineSightMode = false; // 정조준 중인지 여부

    // 정조준 전 포지션 값
    private Vector3 originPos;

    // 효과음
    private AudioSource audioSource;

    // 레이저 충돌 정보를 받아옴
    private RaycastHit hitInfo;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    // 피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;



    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 참조
        theCrosshair = FindObjectOfType<Crosshair>(); // Crosshair 스크립트를 찾아와서 참조
    }

    void Update()
    {
        // 활성화되었을 때만 실행
        if (isActivate)
        {
            GunFireRateCalc(); // 연사속도 계산
            TryFire(); // 발사 시도
            TryReload(); // 재장전 시도
            TryFineSight(); // 정조준 시도
        }
    }

    // 연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 연사속도를 시간에 맞춰서 감소시킴
        }
    }

    // 발사 시도
    private void TryFire()
    {
        // Fire1(마우스 좌클릭)을 누르고, 연사속도가 0보다 크거나 같고, 재장전 중이 아닐 때 발사
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    // 발사전 계산
    private void Fire()
    {
        // 재장전 중이 아닐 때 발사
        if (!isReload)
        {
            // 현재 총의 총알이 0보다 크면 발사하고, 아니면 재장전 시작
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight(); // 정조준 취소
                StartCoroutine(ReloadCoroutine()); // 재장전 시작
            }
        }
    }

    // 발사후 계산
    private void Shoot()
    {
        // 크로스헤어 애니메이션 실행
        theCrosshair.FireAnimation();

        currentGun.currentBulletCount--; // 현재 총의 총알 수 감소
        currentFireRate = currentGun.fireRate; // 다음 발사까지의 시간을 연사속도로 설정
        PlaySE(currentGun.fire_Sound); // 발사 소리 재생
        currentGun.muzzleFlash.Play(); // 총구 화염 효과 재생
        Hit(); // 총알이 맞은 지점에 효과 재생

        // 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    // 피격 이펙트 재생
    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward
            + new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                          Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                          0)
            , out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // 피격 이펙트 생성
            Destroy(clone, 2f);
        }
    }

    // 재장전 시도
    private void TryReload()
    {
        // R키를 눌렀고, 재장전 중이 아니며, 현재 탄창이 재장전할 탄창보다 작을 경우 재장전 시작
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight(); // 정조준 취소
            StartCoroutine(ReloadCoroutine()); // 재장전 시작
        }
    }

    // 재장전 취소
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // 재장전
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");

            // 장전하면 탄알집에 남은 총알은 보유총알로 넘어감
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            // 현재 보유한 총알이 장전할 총알보다 크면
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; // 탄알집에 재장전한 총알 수를 저장
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; // 보유한 총알에서 재장전한 총알 수를 뺌
            }
            else // 보유한 총알이 30발 미만일때
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // 탄창에 보유한 총알 수를 저장
                currentGun.carryBulletCount = 0; // 보유한 총알 수를 0으로 변경
            }
            isReload = false;
        }
        else
        {
            Debug.Log("총알 없음");
        }
    }

    // 정조준 시도
    private void TryFineSight()
    {
        // Fire2(마우스 우클릭)를 눌렀고, 재장전 중이 아닐 때 정조준 시작
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // 정조준 취소
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    // 정조준 로직 가동
    private void FineSight()
    {
        // 정조준 상태 변경
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        // 정조준 시작 또는 정조준 취소에 따라 코루틴 실행
        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    // 정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
        // 정조준 자세까지 Lerp 함수로 부드럽게 이동
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    // 정조준 비활성화
    IEnumerator FineSightDeactivateCoroutine()
    {
        // 원래 자세까지 Lerp 함수로 부드럽게 이동
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // 반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        // 반동 적용 위치 계산
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, -currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y, -currentGun.retroActionFineSightForce);

        // 정조준 상태에 따라 반동 적용
        if (!isFineSightMode) // 정조준 상태가 아닐 때
        {
            currentGun.transform.localPosition = originPos; // 반동 적용 시작 위치로 이동

            // 반동 적용
            while (Vector3.Distance(currentGun.transform.localPosition, recoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 위치를 정확하게 일치시킴
            currentGun.transform.localPosition = originPos;
        }
        else // 정조준 상태일 때
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos; // 반동 적용 시작 위치로 이동

            // 반동 적용
            while (Vector3.Distance(currentGun.transform.localPosition, retroActionRecoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 위치를 정확하게 일치시킴
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
        }
    }

    // 사운드 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    // 현재 장착된 총을 반환
    public Gun GetGun()
    {
        return currentGun;
    }

    // 정조준 상태를 반환
    public bool GetFindSightMode()
    {
        return isFineSightMode;
    }

    // 총을 교체할 때 호출되는 함수
    public void GunChange(Gun _gun)
    {
        // 현재 장착된 총이 있으면
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false); // 현재 총을 비활성화

        currentGun = _gun; // 새로운 총으로 변경
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>(); // 현재 총을 WeaponManager에 등록
        WeaponManager.currentWeaponAnim = currentGun.anim; // 현재 총의 애니메이션을 WeaponManager에 등록

        currentGun.transform.localPosition = Vector3.zero; // 총의 위치 초기화
        currentGun.gameObject.SetActive(true); // 새로운 총을 활성화
        isActivate = true; // 총 활성화 여부 설정
    }
}
