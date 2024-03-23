using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class RequestManager : BaseManager
{
    //调用基类的构造函数
    public RequestManager(GameFace face) : base(face) { }

    //通过actioncode找到对应的处理方法
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
            Debug.LogWarning("找不到对应的处理方法");
        }
    }
    
}
