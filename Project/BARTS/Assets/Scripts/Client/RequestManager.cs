using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class RequestManager : BaseManager
{
    //���û���Ĺ��캯��
    public RequestManager(GameFace face) : base(face) { }

    //ͨ��actioncode�ҵ���Ӧ�Ĵ�����
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(BaseRequest request)
    {
        requestDict.Add(request.GetActionCode,request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestDict.Remove(action);
    }

    public void HandleResponse(MainPack pack)
    {
        if (requestDict.TryGetValue(pack.Actioncode,out BaseRequest request))
        {
            request.OnResponse(pack);
        }
        else
        {
            Debug.LogWarning("�Ҳ�����Ӧ�Ĵ�����");
        }
    }
    
}
