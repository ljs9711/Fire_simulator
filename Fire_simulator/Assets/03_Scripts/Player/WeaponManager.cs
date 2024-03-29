using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    // 현재 무기, 현재무기 애니메이션
    public static Transform currentWeapon; // 기존무기 껐다 키기 용
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입
    [SerializeField] private string currentWeaponType;

    // 무기 교체 딜레이
    [SerializeField] private float changeWeaponDelayTime = 1;
    // 무기 교체가 완전 끝난 시점
    [SerializeField] private float changeWeaponEndDelayTime;

    // 무기 종류들 관리
    [SerializeField] private Gun[] guns;
    [SerializeField] private MeleeWeapon[] hands;
    [SerializeField] private MeleeWeapon[] axes;
    [SerializeField] private MeleeWeapon[] pickaxes;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, MeleeWeapon> handDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> axeDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> pickaxeDictionary = new Dictionary<string, MeleeWeapon>();

    // 필요한 컴포넌트
    [SerializeField] private GunController theGunController;
    [SerializeField] private HandController theHandController;
    [SerializeField] private AxeController theAxeController;
    [SerializeField] private PickAxeController thePickAxeController;

    void Start()
    {
        // 무기 딕셔너리 초기화
        foreach (Gun gun in guns)
            gunDictionary.Add(gun.gunName, gun);

        foreach (MeleeWeapon hand in hands)
            handDictionary.Add(hand.MeleeWeaponName, hand);

        foreach (MeleeWeapon axe in axes)
            axeDictionary.Add(axe.MeleeWeaponName, axe);

        foreach (MeleeWeapon pickaxe in pickaxes)
            pickaxeDictionary.Add(pickaxe.MeleeWeaponName, pickaxe);
    }

    void Update()
    {
        // 키 입력에 따른 무기 교체 시도
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1")); // 서브머신건
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe")); // 도끼
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe")); // 곡괭이
        }
    }

    // 무기 교체 코루틴
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out"); // 현재 무기 애니메이션 재생
        yield return new WaitForSeconds(changeWeaponDelayTime);

        // 이전 무기 동작 취소
        CancelPreWeaponAction();
        // 새로운 무기로 교체
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        // 교체 완료 후 상태 초기화
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    // 이전 무기 동작 취소
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickAxeController.isActivate = false;
                break;
        }
    }

    // 무기 교체 처리
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if (_type == "HAND")
            theHandController.MeleeWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.MeleeWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            thePickAxeController.MeleeWeaponChange(pickaxeDictionary[_name]);
    }
}
