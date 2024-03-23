using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class GameFace : MonoBehaviour
{

    private ClientManager clientManager;
    private RequestManager requestManager;

    //���ҳ����ж�Ӧ���ֵ���Ϸ��Ʒface
    private static GameFace face;
    public static GameFace Face
    {
        get
        {
            if(face == null)
            {
                face = GameObject.Find("GameFace").GetComponent<GameFace>();
            }
            return face;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);

        clientManager.OnInit();
        requestManager.OnInit();
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
    }

    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void HandleResponse(MainPack pack)
    {
        //�����յ�����Ϣ
        requestManager.HandleResponse(pack);
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }
}
