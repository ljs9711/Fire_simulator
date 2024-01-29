using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //ȹ�� ������ �ִ�Ÿ�

    private bool pickupActivated = false; //ȹ�� �����ϸ� true

    private RaycastHit hitInfo; //�浹ü ���� ����

    //������ ���̾�� �����ϵ��� ���̾� ����
    [SerializeField]
    private LayerMask layerMask;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText;



    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickup();
        }
    }

    private void CanPickup()
    {
        if(pickupActivated )
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ���߽��ϴ�");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    //������ üũ
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }

    //������ ȹ�� �ؽ�Ʈ ���ܳ�
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ�� " + "<coler=yellow>" + "(E)" + "</color>";
    }

    //������ ȹ�� �ؽ�Ʈ �����
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
