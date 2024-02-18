using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName; // ������ �̸�
    [SerializeField]
    private int hp; // ������ ü��

    [SerializeField]
    private float walkSpeed; // �ȱ� ���ǵ�
    private Vector3 direction; // �̵� ����

    // ���� ����
    private bool isAction; // �ൿ �� ����
    private bool isWalking; // �ȱ� ����

    [SerializeField]
    private float walkTime; // �ȱ� �ð�
    [SerializeField]
    private float waitTime; // �ִϸ��̼� ��� �ð�
    private float currentTime; // ���� �ð�

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Animator anim; // �ִϸ�����
    [SerializeField]
    private Rigidbody rigid; // ������ٵ�
    [SerializeField]
    private BoxCollider boxCol; // �ڽ� �ݶ��̴�

    void Start()
    {
        currentTime = waitTime; // ���� �� ��� �ð� ����
        isAction = true; // �ʱ� ���´� �ൿ ��
    }

    void Update()
    {
        Move(); // �̵� �Լ� ȣ��
        Rotation(); // ȸ�� �Լ� ȣ��
        ElapseTime(); // �ð� ��� �Լ� ȣ��
    }

    // �̵� �Լ�
    private void Move()
    {
        if (isWalking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    // ȸ�� �Լ�
    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    // �ð� ��� �Լ�
    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime; // ���� �ð� ����
            if (currentTime <= 0)
            {
                Reset_Anim(); // �ִϸ��̼� �ʱ�ȭ �Լ� ȣ��
            }
        }
    }

    // �ִϸ��̼� �ʱ�ȭ �Լ�
    private void Reset_Anim()
    {
        isWalking = false; // �ȱ� ���� ����
        isAction = true; // �ൿ ���·� ����
        anim.SetBool("Walking", isWalking); // �ִϸ����Ϳ� �ȱ� ���� ����
        direction.Set(0f, Random.Range(0f, 360f), 0f); // ���� ���� ����
        RandomAction(); // ���� �ൿ �Լ� ȣ��
    }

    // ���� �ൿ �Լ�
    private void RandomAction()
    {
        isAction = true; // �ൿ ���·� ����

        int _random = Random.Range(0, 4); // 4���� �ִϸ��̼� �� ���� ����

        if (_random == 0)
            Wait(); // ��� �ִϸ��̼�
        else if (_random == 1)
            Eat(); // �Ա� �ִϸ��̼�
        else if (_random == 2)
            Peek(); // ���캸�� �ִϸ��̼�
        else if (_random == 3)
            tryWalk(); // �ȱ� �õ�
    }

    // ��� �ִϸ��̼� �Լ�
    private void Wait()
    {
        currentTime = waitTime; // ��� �ð� ����
    }

    // �Ա� �ִϸ��̼� �Լ�
    private void Eat()
    {
        currentTime = waitTime; // ��� �ð� ����
        anim.SetTrigger("Eat"); // �Ա� �ִϸ��̼� ����
    }

    // ���캸�� �ִϸ��̼� �Լ�
    private void Peek()
    {
        currentTime = waitTime; // ��� �ð� ����
        anim.SetTrigger("Peek"); // ���캸�� �ִϸ��̼� ����
    }

    // �ȱ� �õ� �Լ�
    private void tryWalk()
    {
        isWalking = true; // �ȱ� ���·� ����
        anim.SetBool("Walking", isWalking); // �ִϸ����Ϳ� �ȱ� ���� ����
        currentTime = walkTime; // �ȱ� �ð� ����
    }
}
