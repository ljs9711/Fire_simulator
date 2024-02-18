using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField]
    private int hp; // ������ ü��

    [SerializeField]
    private float destroyTime; // ���� ���� �ð�

    [SerializeField]
    private CapsuleCollider col; // �ݶ��̴�

    // �ʿ��� ���� ������Ʈ
    [SerializeField]
    private GameObject go_wood; // �Ϲ� ����
    [SerializeField]
    private GameObject go_debris; // �μ��� ����
    [SerializeField]
    private GameObject go_effect_prefabs; // ä�� ����Ʈ
    [SerializeField]
    private GameObject go_wood_item_prefab; // ���� ������

    // �ı� �� ������ ����
    [SerializeField]
    private int count;

    // �ʿ��� ���� �̸�
    [SerializeField]
    private string strike_Sound; // ä�� ����
    [SerializeField]
    private string destroy_Sound; // �ı� ����

    // ä�� �Լ�
    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound); // ä�� ���� ���

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity); // ä�� ����Ʈ ����
        Destroy(clone, destroyTime); // ����Ʈ ����

        hp--; // ���� ü�� ����
        if (hp <= 0)
            Destruction(); // ü���� 0 ������ �� �ı� �Լ� ȣ��
    }

    // �ı� �Լ�
    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound); // �ı� ���� ���

        col.enabled = false; // �ݶ��̴� ��Ȱ��ȭ

        // ������ ����
        for (int i = 0; i < count; i++)
        {
            Instantiate(go_wood_item_prefab, go_wood.transform.position, Quaternion.identity);
        }

        Destroy(go_wood); // �Ϲ� ���� ����

        go_debris.SetActive(true); // �μ��� ���� Ȱ��ȭ
        Destroy(go_debris, destroyTime); // �μ��� ���� ����
    }
}
