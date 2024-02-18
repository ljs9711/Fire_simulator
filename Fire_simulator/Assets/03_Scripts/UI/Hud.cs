using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField] private GunController theGunController;
    private Gun currentGun;

    // 필요하면 HUD 호출, 필요없으면 비활성화
    [SerializeField] private GameObject go_BulletHUD;

    // 총알 갯수 텍스트 반영
    [SerializeField] private Text[] text_Bullet;

    void Update()
    {
        CheckBullet();
    }

    // 총알 갯수 확인 및 텍스트 업데이트
    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString(); // 총알 보유량
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString(); // 재장전할 총알 수
        text_Bullet[2].text = currentGun.currentBulletCount.ToString(); // 현재 탄창에 있는 총알 수
    }
}
