using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;
    private int currentHp;

    // 스태미나
    [SerializeField]
    private int sp;
    private int currentSp;

    // 스태미나 증가량
    [SerializeField]
    private int spIncreaseSpeed;

    // 스태미나 재회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    // 스태미나 감소 여부
    private bool spUsed;

    // 방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // 배고픔이 줄어드는 속도
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    // 목마름이 줄어드는 속도
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    void Update()
    {
        UpdateHungry();
        UpdateThirsty();
        UpdateSpRechargeTime();
        UpdateSpRecover();

        UpdateGauges();
    }

    // 스태미나 재회복 딜레이를 처리하는 함수
    private void UpdateSpRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    // 스태미나를 회복하는 함수
    private void UpdateSpRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    // 배고픔을 처리하는 함수
    private void UpdateHungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다.");
    }

    // 목마름을 처리하는 함수
    private void UpdateThirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다.");
    }

    // UI 게이지 업데이트 함수
    private void UpdateGauges()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    // 체력을 증가시키는 함수
    public void IncreaseHP(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }
        if (currentHp + _count < hp)
            currentHp += _count;
        else currentHp = hp;
    }

    // 체력을 감소시키는 함수
    public void DecreaseHP(int _count)
    {
        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("HP가 0이 되었습니다.");
    }

    // 방어력을 증가시키는 함수
    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else currentDp = dp;
    }

    // 방어력을 감소시키는 함수
    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("방어력이 0이 되었습니다.");
    }

    // 배고픔을 증가시키는 함수
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else currentHungry = hungry;
    }

    // 배고픔을 감소시키는 함수
    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    // 목마름을 증가시키는 함수
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else currentThirsty = thirsty;
    }

    // 목마름을 감소시키는 함수
    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    // 스태미나를 감소시키는 함수
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }

    // 현재 스태미나 값을 반환하는 함수
    public int GetCurrentSP()
    {
        return currentSp;
    }
}
