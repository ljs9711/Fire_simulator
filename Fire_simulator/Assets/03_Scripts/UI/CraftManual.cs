using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; // ����ǰ �̸�
    public GameObject go_Prefab; // ���� ��ġ�� ������
    public GameObject go_PreviewPrefab; // �̸����� ������
}

public class CraftManual : MonoBehaviour
{
    // ���� ����
    private bool isActivated = false; // �޴� Ȱ��ȭ ����
    private bool isPreviewActivated = false; // �̸����� Ȱ��ȭ ����

    [SerializeField]
    private GameObject go_BaseUI; // �⺻ ���̽� UI

    [SerializeField]
    private Craft[] craft_fire; // ��ںҿ� �ǿ� ���� ����ǰ �迭
    private GameObject go_Preview; // �̸����� �������� ���� ����
    private GameObject go_Prefab; // ���� ��ġ�� ������

    [SerializeField]
    private Transform tf_Player; // �÷��̾� ��ġ

    // ����ĳ��Ʈ�� ����Ͽ� ������ ��ġ�� ���󰡰� ��
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    // ���� Ŭ�� �� ����Ǵ� �Լ�
    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false); // �⺻ ���̽� UI ��Ȱ��ȭ
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            Window(); // â ����

        if (isPreviewActivated)
            PreviewPositionUpdate(); // �̸����� ��ġ ������Ʈ

        if (Input.GetButtonDown("Fire1"))
            Build(); // �Ǽ�

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel(); // ���
    }

    private void Build()
    {
        if (isPreviewActivated)
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity); // ������ ��ġ
            Destroy(go_Preview); // �̸����� ����
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location; // �̸����� ��ġ ������Ʈ
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview); // �̸����� ����

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false); // �⺻ ���̽� UI ��Ȱ��ȭ
    }

    private void Window()
    {
        if (!isActivated)
            OpenWindow(); // â ����
        else
            CloseWindow(); // â �ݱ�
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true); // �⺻ ���̽� UI Ȱ��ȭ
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false); // �⺻ ���̽� UI ��Ȱ��ȭ
    }
}
