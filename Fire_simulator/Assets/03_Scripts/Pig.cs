using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName; //������ �̸�
    [SerializeField]
    private int hp; //������ ü��

    [SerializeField]
    private float walkSpeed; //�ȱ� ���ǵ�
    private Vector3 direction;

    //���º���
    private bool isAction; //�ൿ�� ����
    private bool isWalking; //�ȱ⿩��

    [SerializeField]
    private float walkTime; //�ȱ� �ð�
    [SerializeField]
    private float waitTime; //�ִϸ��̼� ���ð�
    private float currentTime;


    //�ʿ��� ������Ʈ
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
        direction.Set(0f, Random.Range(0f, 360f), 0f); //���� ����
        RandomAction();
    }


    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 4); // 4���� �ִϸ��̼� 4�� ���Ծȵ�

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
        Debug.Log("���");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Ǯ���");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("�θ���");
    }
    private void tryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        
        Debug.Log("�ȱ�");
    }

}
