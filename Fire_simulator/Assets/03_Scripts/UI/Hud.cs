using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    // �ʿ��� ������Ʈ
    [SerializeField] private GunController theGunController;
    private Gun currentGun;

    // �ʿ��ϸ� HUD ȣ��, �ʿ������ ��Ȱ��ȭ
    [SerializeField] private GameObject go_BulletHUD;

    // �Ѿ� ���� �ؽ�Ʈ �ݿ�
    [SerializeField] private Text[] text_Bullet;

    void Update()
    {
        CheckBullet();
    }

    // �Ѿ� ���� Ȯ�� �� �ؽ�Ʈ ������Ʈ
    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString(); // �Ѿ� ������
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString(); // �������� �Ѿ� ��
        text_Bullet[2].text = currentGun.currentBulletCount.ToString(); // ���� źâ�� �ִ� �Ѿ� ��
    }
}
