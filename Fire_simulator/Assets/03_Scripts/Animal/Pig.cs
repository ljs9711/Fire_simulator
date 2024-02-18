using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName; // 동물의 이름
    [SerializeField]
    private int hp; // 동물의 체력

    [SerializeField]
    private float walkSpeed; // 걷기 스피드
    private Vector3 direction; // 이동 방향

    // 상태 변수
    private bool isAction; // 행동 중 여부
    private bool isWalking; // 걷기 여부

    [SerializeField]
    private float walkTime; // 걷기 시간
    [SerializeField]
    private float waitTime; // 애니메이션 대기 시간
    private float currentTime; // 현재 시간

    // 필요한 컴포넌트
    [SerializeField]
    private Animator anim; // 애니메이터
    [SerializeField]
    private Rigidbody rigid; // 리지드바디
    [SerializeField]
    private BoxCollider boxCol; // 박스 콜라이더

    void Start()
    {
        currentTime = waitTime; // 시작 시 대기 시간 설정
        isAction = true; // 초기 상태는 행동 중
    }

    void Update()
    {
        Move(); // 이동 함수 호출
        Rotation(); // 회전 함수 호출
        ElapseTime(); // 시간 경과 함수 호출
    }

    // 이동 함수
    private void Move()
    {
        if (isWalking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    // 회전 함수
    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    // 시간 경과 함수
    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime; // 현재 시간 감소
            if (currentTime <= 0)
            {
                Reset_Anim(); // 애니메이션 초기화 함수 호출
            }
        }
    }

    // 애니메이션 초기화 함수
    private void Reset_Anim()
    {
        isWalking = false; // 걷기 상태 종료
        isAction = true; // 행동 상태로 변경
        anim.SetBool("Walking", isWalking); // 애니메이터에 걷기 상태 전달
        direction.Set(0f, Random.Range(0f, 360f), 0f); // 랜덤 방향 설정
        RandomAction(); // 랜덤 행동 함수 호출
    }

    // 랜덤 행동 함수
    private void RandomAction()
    {
        isAction = true; // 행동 상태로 변경

        int _random = Random.Range(0, 4); // 4개의 애니메이션 중 랜덤 선택

        if (_random == 0)
            Wait(); // 대기 애니메이션
        else if (_random == 1)
            Eat(); // 먹기 애니메이션
        else if (_random == 2)
            Peek(); // 살펴보기 애니메이션
        else if (_random == 3)
            tryWalk(); // 걷기 시도
    }

    // 대기 애니메이션 함수
    private void Wait()
    {
        currentTime = waitTime; // 대기 시간 설정
    }

    // 먹기 애니메이션 함수
    private void Eat()
    {
        currentTime = waitTime; // 대기 시간 설정
        anim.SetTrigger("Eat"); // 먹기 애니메이션 실행
    }

    // 살펴보기 애니메이션 함수
    private void Peek()
    {
        currentTime = waitTime; // 대기 시간 설정
        anim.SetTrigger("Peek"); // 살펴보기 애니메이션 실행
    }

    // 걷기 시도 함수
    private void tryWalk()
    {
        isWalking = true; // 걷기 상태로 변경
        anim.SetBool("Walking", isWalking); // 애니메이터에 걷기 상태 전달
        currentTime = walkTime; // 걷기 시간 설정
    }
}
