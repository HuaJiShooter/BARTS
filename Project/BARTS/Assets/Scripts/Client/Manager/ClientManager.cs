using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System;
using SocketGameProtocal;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;

    public ClientManager(GameFace face) : base(face) { }

    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        //对socket进行初始化
        InitSocket();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        //这样销毁不会导致内存溢出？
        message = null;
        //销毁时关闭socket
        CloseSocket();
    }

    /// <summary>
    /// 初始化socket
    /// </summary>
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("127.0.0.1", 25656);
            //连接成功
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }

    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    private void CloseSocket()
    {
        if (socket.Connected&&socket!=null)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer,message.StartIndex,message.Remsize,SocketFlags.None,ReceiveCallback,null);
    }

    private void ReceiveCallback(IAsyncResult iar)
    {
        try
        {
            if (socket == null || socket.Connected == false) return;
            int len = socket.EndReceive(iar);
            if (len == 0)
            {
                CloseSocket();
                return;
            }

            message.ReadBuffer(len,HandleResponse);
            StartReceive();
        }
        catch
        {

        }
    }

    public void HandleResponse(MainPack pack)
    {
        face.HandleResponse(pack);
    }

    public void Send(MainPack pack)
    {
        socket.Send(Message.PackData(pack));
    }
}
