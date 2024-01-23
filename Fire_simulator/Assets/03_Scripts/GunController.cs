using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = true;

    //���� ������ ��
    [SerializeField]
    private Gun currentGun;

    //����ӵ� ���
    private float currentFireRate;

    //���º���
    private bool isReload = false;
    public bool isFineSightMode = false;

    //������ �� ������ ��
    private Vector3 originPos; 

    //ȿ����
    private AudioSource audioSource;

    //������ �浹 ������ �޾ƿ�
    private RaycastHit hitInfo;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //�ǰ�����Ʈ
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



    //����ӵ� ����
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; //60���� 1 = 1
        }
    }

    // �߻� �õ�
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //�߻��� ���
    private void Fire() //�߻���
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

    //�߻��� ���
    private void Shoot() //�߻���
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--; //�Ѿ� ����
        currentFireRate = currentGun.fireRate; //���� �ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine()); //�ѱ�ݵ� �ڷ�ƾ

    }


    private void Hit()
    {                       //���� ��ġ����             forward �� ��µ�
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward 
            //�������� foward�� ������.(�� ��Ȯ��)         X�� �ּ� ��                                          X�� �ִ� ��
            + new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                                                         //Y�� �ּ� ��                                          Y�� �ִ� ��
                          Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                          0)
            , out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); //�ǰ� ����Ʈ ����
            Destroy(clone, 2f);
        }
    }


    //������ �õ�
    private void TryReload()
    {   //RŰ ������ reload ���� / isReload�� false �϶��� / ���� źâ�� reload �� źâ���� ������쿡�� ����
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


    //������
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount; //�����ϸ� ź������ ���� �Ѿ��� �����Ѿ˷� �Ѿ
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount) //���� ������ �Ѿ��� ������ �Ѿ˺��� ũ��
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; //ź������ reloadBulletCount:��������
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; //�׸��� ���� ������ �Ѿ˿��� �������� ��ŭ ��
            }
            else //������ �Ѿ��� 30�� �̸��϶�
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; //źâ�� ������ �Ѿ� �������ϰ�
                currentGun.carryBulletCount = 0; // ������ �Ѿ� 0 ���� ����
            }
            isReload = false;
        }
        else
        {
            Debug.Log("�Ѿ� ����");
        }
    }

    //������ �õ�
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    //������ ���
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }


    //������ ���� ����
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines(); //Lerp ���� �� �ȶ������� while�� ���������� �ߺ������. �ڷ�ƾ �������
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }

    }

    //������ Ȱ��ȭ
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos) //������ �ڼ��϶����� �ݺ�
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f); //������ vector3 ��
            yield return null; //1������ ���
        }

    }

    //������ ��Ȱ��ȭ
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos) //���� �ڼ��϶����� �ݺ�
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f); //���� vector3 ��

            yield return null; //1������ ���
        }
    }


    //�ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine() //�ݵ�
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, -currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y, -currentGun.retroActionFineSightForce);

        if (!isFineSightMode) //������ �ƴ� ���� �ݵ�
        {
            currentGun.transform.localPosition = originPos; //���ڸ� �Դٰ� �ٽ� �ݵ��ֱ� ����

            //�ݵ�����
            while (Vector3.Distance(currentGun.transform.localPosition, recoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // ��ġ�� ��Ȯ�ϰ� ��ġ��Ű��
            currentGun.transform.localPosition = originPos;
        }
        else //������ ���� �ݵ�
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos; //���ڸ� �Դٰ� �ٽ� �ݵ��ֱ� ����

            //�ݵ�����
            while (Vector3.Distance(currentGun.transform.localPosition, retroActionRecoilBack) > 0.001f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // ��ġ�� ��Ȯ�ϰ� ��ġ��Ű��
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
        }
    }


    //���� ���
    private void PlaySE(AudioClip _clip) //�Ҹ� �־� ������ ���
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
        if (WeaponManager.currentWeapon != null) //���� ���������
            WeaponManager.currentWeapon.gameObject.SetActive(false); //���� ������Ʈ ��Ȱ��ȭ

        currentGun = _gun; //�ٲ� ���Ⱑ ���� ����� �ٲ�
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero; //��ġ �ٲ���� ���� ������ 0,0,0���� �ʱ�ȭ
        currentGun.gameObject.SetActive(true);
        isActivate = true; //���� Ȱ��ȭ ����
    }
}
