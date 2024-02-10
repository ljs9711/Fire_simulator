using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // ���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;
    // ���� ����, ���繫�� �ִϸ��̼�
    public static Transform currentWeapon; // �������� ���� Ű�� ��
    public static Animator currentWeaponAnim;

    // ���� ������ Ÿ��
    [SerializeField]
    private string currentWeaponType;

    // ���� ��ü ������
    [SerializeField]
    private float changeWeaponDelayTime = 1;
    // ���� ��ü�� ���� ���� ����
    [SerializeField]
    private float changeWeaponEndDelayTime;

    // ���� ������ ����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private MeleeWeapon[] hands;
    [SerializeField]
    private MeleeWeapon[] axes;
    [SerializeField]
    private MeleeWeapon[] pickaxes;

    // ���� �������� ���� ���� ������ �����ϵ��� ����.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, MeleeWeapon> handDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> axeDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> pickaxeDictionary = new Dictionary<string, MeleeWeapon>();

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickAxeController thePickAxeController;


    void Start()
    {
        // ���� ��ųʸ� �ʱ�ȭ
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].MeleeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].MeleeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].MeleeWeaponName, pickaxes[i]);
        }
    }

    void Update()
    {
        // Ű �Է¿� ���� ���� ��ü �õ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1")); // ����ӽŰ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe")); // ����
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe")); // ���
        }
    }


    // ���� ��ü �ڷ�ƾ
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out"); // ���� ���� �ִϸ��̼� ���
        yield return new WaitForSeconds(changeWeaponDelayTime);

        // ���� ���� ���� ���
        CancelPreWeaponAction();
        // ���ο� ����� ��ü
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        // ��ü �Ϸ� �� ���� �ʱ�ȭ
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    // ���� ���� ���� ���
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

    // ���� ��ü ó��
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
