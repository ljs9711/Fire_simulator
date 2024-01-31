using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName; //동물의 이름
    [SerializeField]
    private int hp; //동물의 체력

    [SerializeField]
    private float walkSpeed; //걷기 스피드
    private Vector3 direction;

    //상태변수
    private bool isAction; //행동중 여부
    private bool isWalking; //걷기여부

    [SerializeField]
    private float walkTime; //걷기 시간
    [SerializeField]
    private float waitTime; //애니메이션 대기시간
    private float currentTime;


    //필요한 컴포넌트
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;


    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        Move();
        Rotation();
        ElapseTime();
        
    }




    private void Move()
    {
        if (isWalking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    private void Rotation()
    {
        if(isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void ElapseTime()
    {
        if(isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                Reset_Anim();
            }

        }
    }

    private void Reset_Anim()
    {
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f); //랜덤 방향
        RandomAction();
    }


    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 4); // 4개의 애니메이션 4는 포함안됨

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            tryWalk();
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
    private void tryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        
        Debug.Log("걷기");
    }

}
