using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon: MonoBehaviour
{

    public string MeleeWeaponName; //근접무기

    //근접무기 종류
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;
   

    public float range; //공격 범위
    public int damage; // 공격력
    public float workSpeed; //작업 속도
    public float attackDelay; //공격 딜레이
    public float attackDelay_in; //공격 활성화 시점
    public float attackDelay_out; //공격 비활성화 시점

    public Animator anim; //애니메이션
}
