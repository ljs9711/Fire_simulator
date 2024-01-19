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
    private Vector3 originPos; //������ �� ������ ��

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
            currentFireRate -= Time.deltaTime; //60���� 1 = 1
        }
    }


    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }


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


    private void Shoot() //�߻���
    {
        currentGun.currentBulletCount--; //�Ѿ� ����
        currentFireRate = currentGun.fireRate; //���� �ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("�Ѿ� �߻���");

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine()); //�ѱ�ݵ� �ڷ�ƾ

    }

    private void TryReload()
    {   //RŰ ������ reload ���� / isReload�� false �϶��� / ���� źâ�� reload �� źâ���� ������쿡�� ����
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
            StopAllCoroutines(); //Lerp ���� �� �ȶ������� while�� ���������� �ߺ������. �ڷ�ƾ �������
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
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos) //������ �ڼ��϶����� �ݺ�
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f); //������ vector3 ��
            yield return null; //1������ ���
        }

    }
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos) //���� �ڼ��϶����� �ݺ�
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f); //���� vector3 ��

            yield return null; //1������ ���
        }
    }

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



    private void PlaySE(AudioClip _clip) //�Ҹ� �־� ������ ���
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
