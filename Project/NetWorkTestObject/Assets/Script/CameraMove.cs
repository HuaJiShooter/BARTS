using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class CameraMove : MonoBehaviour
{
    //��ȡ��������ƶ�
    Transform CameraTransform;
    //��Ϊѡ�еĶ���
    GameObject SelectedObject = null;

    private Vector3 moveInput;
    [SerializeField] private float panSpeed;
    [SerializeField] private float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //����ƶ�
        CameraMoveFunction();



        //���ƽ�ɫ
        //�����ȡ���ٿؽ�ɫ
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool res = Physics.Raycast(ray, out hit);

            if (res == true)
            {
                //����Ϊѡ�еĽ�ɫ
                SelectedObject = hit.collider.gameObject;
                Debug.Log("Selected Object is��"+SelectedObject.name);
            }
        }

        //�Ҽ���ȡĿ��
        else if(Input.GetMouseButtonDown(1)){
            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit1;
            bool res = Physics.Raycast(ray1, out hit1);
            if (res == true && SelectedObject != null)
            {
                if (SelectedObject.GetComponent<NavMeshAgent>() != null)
                {
                    //����Ҽ��˽�ɫ
                    if(hit1.collider.gameObject.GetComponent<NavMeshAgent>() != null)
                    {
                        ControlCharactor(hit1.collider.gameObject);
                    }
                    //����Ҽ��˵���
                    else
                    {
                        ControlCharactor(hit1.point);
                    }
                }
                else
                {
                    Debug.Log("Selected type error");
                }
            }
            else
            {
                Debug.Log("No Object Selected");
            }
        }
    }

    void CameraMoveFunction()
    {
        //����ƶ��߼�
        Vector3 pos = transform.position;
        moveInput.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x > Screen.width * 0.95f && mousePos.x < Screen.width)
            moveInput.x = 1;
        if (mousePos.x < Screen.width * 0.1f && mousePos.x > 0)
            moveInput.x =-1;
        if (mousePos.y > Screen.height * 0.95 && mousePos.y < Screen.height)
            moveInput.z = 1;
        if (mousePos.y < Screen.height * 0.1 && mousePos.y > 0)
            moveInput.z = -1;

        pos.x += moveInput.normalized.x * panSpeed * Time.deltaTime;
        pos.y += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        pos.z += moveInput.normalized.z * panSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -100, 100);
        pos.y = Mathf.Clamp(pos.y, 2, 6);
        pos.z = Mathf.Clamp(pos.z, -100, 100);

        transform.position = pos;
    }

    void ControlCharactor(GameObject TargetObject)
    {
        //֪ͨ��ɫ�ƶ����
        SelectedObject.SendMessage("MoveToPlace", TargetObject);
    }
    void ControlCharactor(Vector3 TargetPlace)
    {
        SelectedObject.SendMessage("MoveToPlace", TargetPlace);
    }

}
