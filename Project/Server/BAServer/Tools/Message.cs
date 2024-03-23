using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocal;
using Google.Protobuf;

namespace SocketGame.Tools
{
    class Message
    {
        //用来存储消息主体
        private byte[] buffer = new byte[1024];

        //当前buffer存到的位数
        private int startindex;

        //一些获取的方法
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

        //获得剩余空间
        public int Remsize
        {
            get
            {
                return buffer.Length - startindex;
            }
        }

        public void ReadBuffer(int len,Action<MainPack> HandleRequest)
        {
            startindex += len;
            if (startindex <= 4) return;
            int count = BitConverter.ToInt32(Buffer, 0);//解析包头获取信息长度
            while (true)
            {
                if (startindex >= (count + 4))
                {
                    //消息本体

                    MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);

                    //回调处理消息的函数
                    HandleRequest(pack);

                    Array.Copy(buffer, count + 4, buffer, 0, startindex - count - 4);
                    startindex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
            return;
        }

        //服务端返回的数据封装
        public static byte[] PackData(MainPack pack)
        {
            //消息转化成数组,构造包体
            byte[] data = pack.ToByteArray();
            //构造包头
            byte[] head = BitConverter.GetBytes(data.Length);
            //包头包体连接并转化为byte数组
            return head.Concat(data).ToArray();
         }
    }
}
