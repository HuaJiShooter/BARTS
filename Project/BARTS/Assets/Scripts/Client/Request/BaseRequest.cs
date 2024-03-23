using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;
using System.Xml.Serialization;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    protected GameFace face;
    public ActionCode GetActionCode
    {
        get
        {
            return actionCode;
        }
    }

    //统一所有的request进行管理
    //将所有的Request放进RequestManager中管理
    public virtual void Awake()
    {
        face = GameFace.Face;
        
    }

    public virtual void Start()
    {
        face.AddRequest(this);
    }

    public virtual void OnDestroy()
    {
        face.RemoveRequest(actionCode);
    }

    public virtual void OnResponse(MainPack pack)
    {

    }

    public virtual void SendRequest(MainPack pack)
    {
        face.Send(pack);
    }
}
