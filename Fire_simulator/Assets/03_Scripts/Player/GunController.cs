using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    // ���� ������ ��
    [SerializeField]
    private Gun currentGun;

    // ����ӵ� ���
    private float currentFireRate;

    // ���º���
    private bool isReload = false; // ������ ������ ����
    public bool isFineSightMode = false; // ������ ������ ����

    // ������ �� ������ ��
    private Vector3 originPos;

    // ȿ����
    private AudioSource audioSource;

    // ������ �浹 ������ �޾ƿ�
    private RaycastHit hitInfo;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    // �ǰ� ����Ʈ
    [SerializeField]
    private GameObject hit_effect_prefab;



    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ����
        theCrosshair = FindObjectOfType<Crosshair>(); // Crosshair ��ũ��Ʈ�� ã�ƿͼ� ����
    }

    void Update()
    {
        // Ȱ��ȭ�Ǿ��� ���� ����
        if (isActivate)
        {
            GunFireRateCalc(); // ����ӵ� ���
            TryFire(); // �߻� �õ�
            TryReload(); // ������ �õ�
            TryFineSight(); // ������ �õ�
        }
    }

    // ����ӵ� ����
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // ����ӵ��� �ð��� ���缭 ���ҽ�Ŵ
        }
    }

    // �߻� �õ�
    private void TryFire()
    {
        // Fire1(���콺 ��Ŭ��)�� ������, ����ӵ��� 0���� ũ�ų� ����, ������ ���� �ƴ� �� �߻�
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    // �߻��� ���
    private void Fire()
    {
        // ������ ���� �ƴ� �� �߻�
        if (!isReload)
        {
            // ���� ���� �Ѿ��� 0���� ũ�� �߻��ϰ�, �ƴϸ� ������ ����
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight(); // ������ ���
                StartCoroutine(ReloadCoroutine()); // ������ ����
            }
        }
    }

    // �߻��� ���
    private void Shoot()
    {
        // ũ�ν���� �ִϸ��̼� ����
        theCrosshair.FireAnimation();

        currentGun.currentBulletCount--; // ���� ���� �Ѿ� �� ����
        currentFireRate = currentGun.fireRate; // ���� �߻������ �ð��� ����ӵ��� ����
        PlaySE(currentGun.fire_Sound); // �߻� �Ҹ� ���
        currentGun.muzzleFlash.Play(); // �ѱ� ȭ�� ȿ�� ���
        Hit(); // �Ѿ��� ���� ������ ȿ�� ���

        // �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    // �ǰ� ����Ʈ ���
    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward
            + new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                          Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                          0)
            , out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // �ǰ� ����Ʈ ����
            Destroy(clone, 2f);
        }
    }

    // ������ �õ�
    private void TryReload()
    {
        // RŰ�� ������, ������ ���� �ƴϸ�, ���� źâ�� �������� źâ���� ���� ��� ������ ����
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight(); // ������ ���
            StartCoroutine(ReloadCoroutine()); // ������ ����
        }
    }

    // ������ ���
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // ������
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");

            // �����ϸ� ź������ ���� �Ѿ��� �����Ѿ˷� �Ѿ
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            // ���� ������ �Ѿ��� ������ �Ѿ˺��� ũ��
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; // ź������ �������� �Ѿ� ���� ����
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; // ������ �Ѿ˿��� �������� �Ѿ� ���� ��
            }
            else // ������ �Ѿ��� 30�� �̸��϶�
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // źâ�� ������ �Ѿ� ���� ����
                currentGun.carryBulletCount = 0; // ������ �Ѿ� ���� 0���� ����
            }
            isReload = false;
        }
        else
        {
            Debug.Log("�Ѿ� ����");
        }
    }

    // ������ �õ�
    private void TryFineSight()
    {
        // Fire2(���콺 ��Ŭ��)�� ������, ������ ���� �ƴ� �� ������ ����
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // ������ ���
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    // ������ ���� ����
    private void FineSight()
    {
        // ������ ���� ����
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        // ������ ���� �Ǵ� ������ ��ҿ� ���� �ڷ�ƾ ����
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

    // ������ Ȱ��ȭ
    IEnumerator FineSightActivateCoroutine()
    {
        // ������ �ڼ����� Lerp �Լ��� �ε巴�� �̵�
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    // ������ ��Ȱ��ȭ
    IEnumerator FineSightDeactivateCoroutine()
    {
        // ���� �ڼ����� Lerp �Լ��� �ε巴�� �̵�
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // �ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine()
    {
        // �ݵ� ���� ��ġ ���
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, -currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y, -currentGun.retroActionFineSightForce);

        // ������ ���¿� ���� �ݵ� ����
        if (!isFineSightMode) // ������ ���°� �ƴ� ��
        {
            currentGun.transform.localPosition = originPos; // �ݵ� ���� ���� ��ġ�� �̵�

            // �ݵ� ����
            while (Vector3.Distance(currentGun.transform.localPosition, recoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // ��ġ�� ��Ȯ�ϰ� ��ġ��Ŵ
            currentGun.transform.localPosition = originPos;
        }
        else // ������ ������ ��
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos; // �ݵ� ���� ���� ��ġ�� �̵�

            // �ݵ� ����
            while (Vector3.Distance(currentGun.transform.localPosition, retroActionRecoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // ��ġ�� ��Ȯ�ϰ� ��ġ��Ŵ
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
        }
    }

    // ���� ���
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    // ���� ������ ���� ��ȯ
    public Gun GetGun()
    {
        return currentGun;
    }

    // ������ ���¸� ��ȯ
    public bool GetFindSightMode()
    {
        return isFineSightMode;
    }

    // ���� ��ü�� �� ȣ��Ǵ� �Լ�
    public void GunChange(Gun _gun)
    {
        // ���� ������ ���� ������
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false); // ���� ���� ��Ȱ��ȭ

        currentGun = _gun; // ���ο� ������ ����
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>(); // ���� ���� WeaponManager�� ���
        WeaponManager.currentWeaponAnim = currentGun.anim; // ���� ���� �ִϸ��̼��� WeaponManager�� ���

        currentGun.transform.localPosition = Vector3.zero; // ���� ��ġ �ʱ�ȭ
        currentGun.gameObject.SetActive(true); // ���ο� ���� Ȱ��ȭ
        isActivate = true; // �� Ȱ��ȭ ���� ����
    }
}
