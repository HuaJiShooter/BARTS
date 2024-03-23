using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Link;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                Debug.Log("���ӳɹ�");
                break;
            case ReturnCode.Failed:
                Debug.LogWarning("����ʧ��");
                break;
        }
    }

    //����ע������
    public void SendRequest(int playerID)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        LinkPack linkPack = new LinkPack();
        linkPack.PlayerID = playerID;
        pack.Linkpack = linkPack;
        base.SendRequest(pack);
    }
}
