using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;
    //���� ����, ���繫�� �ִϸ��̼�
    public static Transform currentWeapon; //�������� ���� Ű�� ��
    public static Animator currentWeaponAnim;

    //���� ������ Ÿ��
    [SerializeField]
    private string currentWeaponType;

    //���� ��ü ������
    [SerializeField]
    private float changeWeaponDelayTime = 1;
    //���� ��ü�� ���� ���� ����
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //���� ������ ����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    //���� �������� ���� ���� ������ �����ϵ��� ����.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;


    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));//����ӽŰ�
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

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
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        if (_type == "HAND")
            theHandController.HandChange(handDictionary[_name]);
    }
}
