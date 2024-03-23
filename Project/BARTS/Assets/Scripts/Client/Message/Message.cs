using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using SocketGameProtocal;
using Google.Protobuf;

public class Message
{
    //�����洢��Ϣ����
    private byte[] buffer = new byte[1024];

    //��ǰbuffer�浽��λ��
    private int startindex;

    //һЩ��ȡ�ķ���
    public byte[] Buffer
    {
        get
        {
            return buffer;
        }
    }

    public int StartIndex
    {
        get
        {
            return startindex;
        }
    }

    //���ʣ��ռ�
    public int Remsize
    {
        get
        {
            return buffer.Length - startindex;
        }
    }

    public void ReadBuffer(int len, Action<MainPack> HandleResponse)
    {
        startindex += len;
        if (startindex <= 4) return;
        int count = BitConverter.ToInt32(Buffer, 0);//������ͷ��ȡ��Ϣ����
        while (true)
        {
            if (startindex >= (count + 4))
            {
                //��Ϣ����
                MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);
                HandleResponse(pack);
                Array.Copy(buffer, count + 4, buffer, 0, startindex - count - 4);
                startindex -= (count + 4);
            }
            else
            {
                break;
            }
        }
    }

    //����˷��ص����ݷ�װ
    public static byte[] PackData(MainPack pack)
    {
        //��Ϣת��������,�������
        byte[] data = pack.ToByteArray();
        //�����ͷ
        byte[] head = BitConverter.GetBytes(data.Length);
        //��ͷ�������Ӳ�ת��Ϊbyte����
        return head.Concat(data).ToArray();
    }
}
