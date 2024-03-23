using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SocketGame.Tools;
using SocketGameProtocal;

namespace SocketGame.Servers
{
    class Client
    {
        private Socket socket;
        private Message message;
        private Server server;


        public Client(Socket socket,Server server)
        {

            message = new Message();

            this.server = server;
            this.socket = socket;

            //开始消息接收
            StartRecive();
        }

        void StartRecive()
        {
            socket.BeginReceive(message.Buffer,message.StartIndex,message.Remsize,SocketFlags.None,ReceiveCallback,null);
        }

        void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                //在这里解析消息
                Console.WriteLine("ReceiveCallback接收到消息");
                int len = socket.EndReceive(iar);
                if (len == 0)
                {
                    return;
                }

                message.ReadBuffer(len,HandleRequest);
                StartRecive();
            }
            catch(Exception e)
            {
                Console.WriteLine("ReceiveCallback报错："+ e.Message + e.Source + e.StackTrace + e.HelpLink);
            }
        }

        public void Send(MainPack pack)
        {
            socket.Send(Message.PackData(pack));
        }

        private void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack,this);
        }

        public bool Link(MainPack pack)
        {
            try
            {
                int ClientID = pack.Linkpack.PlayerID;
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("获取ID出错:" + e.Message);
                return false;
            }
        }

    }
}
