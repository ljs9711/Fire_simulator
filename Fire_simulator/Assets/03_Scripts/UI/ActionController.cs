using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // ȹ�� ������ �ִ� �Ÿ�

    private bool pickupActivated = false; // ȹ�� ���� ����

    private RaycastHit hitInfo; // �浹ü ���� ����

    // ������ ���̾�� �����ϵ��� ���̾� ����
    [SerializeField]
    private LayerMask layerMask;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText; // ��ȣ�ۿ� �׼� �ؽ�Ʈ
    [SerializeField]
    private Inventory theInventory; // �κ��丮

    void Update()
    {
        CheckItem(); // ������ üũ
        TryAction(); // �ൿ �õ�
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E Ű �Է� ��
        {
            CheckItem(); // ������ üũ
            CanPickup(); // ������ ȹ�� �õ�
        }
    }

    private void CanPickup()
    {
        if (pickupActivated) // ȹ�� ������ ���¶��
        {
            if (hitInfo.transform != null && hitInfo.transform.CompareTag("Item")) // �浹ü�� ������ �±׸� ������ �ִٸ�
            {
                Item item = hitInfo.transform.GetComponent<ItemPickup>().item; // ȹ���� ������ ��������
                Debug.Log(item.itemName + "��(��) ȹ���߽��ϴ�");
                theInventory.AcquireItem(item); // ������ �κ��丮�� �߰�
                Destroy(hitInfo.transform.gameObject); // ������ ���� ������Ʈ �ı�
                InfoDisappear(); // ȹ�� �ؽ�Ʈ �����
            }
        }
    }

    // ������ üũ
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask)) // �÷��̾� ��ġ���� �������� ����ĳ��Ʈ �߻�
        {
            if (hitInfo.transform.CompareTag("Item")) // �浹ü�� ������ �±׸� ������ �ִٸ�
            {
                ItemInfoAppear(); // ������ ȹ�� �ؽ�Ʈ ���
            }
        }
        else
        {
            InfoDisappear(); // ������ ȹ�� �ؽ�Ʈ �����
        }
    }

    // ������ ȹ�� �ؽ�Ʈ ���
    private void ItemInfoAppear()
    {
        pickupActivated = true; // ȹ�� ���� ���·� ����
        actionText.gameObject.SetActive(true); // �׼� �ؽ�Ʈ Ȱ��ȭ
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>"; // �ؽ�Ʈ ����
    }

    // ������ ȹ�� �ؽ�Ʈ �����
    private void InfoDisappear()
    {
        pickupActivated = false; // ȹ�� �Ұ��� ���·� ����
        actionText.gameObject.SetActive(false); // �׼� �ؽ�Ʈ ��Ȱ��ȭ
    }
}
