using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField]
    private int hp; // 나무의 체력

    [SerializeField]
    private float destroyTime; // 파편 제거 시간

    [SerializeField]
    private CapsuleCollider col; // 콜라이더

    // 필요한 게임 오브젝트
    [SerializeField]
    private GameObject go_wood; // 일반 나무
    [SerializeField]
    private GameObject go_debris; // 부서진 나무
    [SerializeField]
    private GameObject go_effect_prefabs; // 채집 이펙트
    [SerializeField]
    private GameObject go_wood_item_prefab; // 나무 아이템

    // 파괴 시 아이템 갯수
    [SerializeField]
    private int count;

    // 필요한 사운드 이름
    [SerializeField]
    private string strike_Sound; // 채집 사운드
    [SerializeField]
    private string destroy_Sound; // 파괴 사운드

    // 채집 함수
    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound); // 채집 사운드 재생

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity); // 채집 이펙트 생성
        Destroy(clone, destroyTime); // 이펙트 제거

        hp--; // 나무 체력 감소
        if (hp <= 0)
            Destruction(); // 체력이 0 이하일 때 파괴 함수 호출
    }

    // 파괴 함수
    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound); // 파괴 사운드 재생

        col.enabled = false; // 콜라이더 비활성화

        // 아이템 생성
        for (int i = 0; i < count; i++)
        {
            Instantiate(go_wood_item_prefab, go_wood.transform.position, Quaternion.identity);
        }

        Destroy(go_wood); // 일반 나무 제거

        go_debris.SetActive(true); // 부서진 나무 활성화
        Destroy(go_debris, destroyTime); // 부서진 나무 제거
    }
}
