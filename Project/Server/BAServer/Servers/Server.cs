using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SocketGame.Controller;
using SocketGameProtocal;

namespace SocketGame.Servers
{
    class Server
    {
        private Socket socket;

        //用来存储和管理客户端列表
        private List<Client> clientList = new List<Client>();

        private ControllerManager controllerManager;

        public Server(int port)
        {
            Console.WriteLine("正在初始化服务器");

            controllerManager = new ControllerManager(this);
            //绑定socket并开始监听
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(0);

            Console.WriteLine("初始化服务器完毕，正在监听连接请求");

            //开始监听连接请求
            StartAccept();
        }

        void StartAccept()
        {
            Console.WriteLine("监听连接中...");
            //回调函数
            socket.BeginAccept(AcceptCallback, null);
        }

        //异步响应函数
        void AcceptCallback(IAsyncResult iar)
        {
            Console.WriteLine("正在建立新连接");

            Socket client = socket.EndAccept(iar);
            clientList.Add(new Client(client, this));

            Console.WriteLine("新链接已建立");

            //AcceptCallback 函数再次调用 StartAccept 函数，以继续监听新的连接请求。
            StartAccept();
        }


        public void HandleRequest(MainPack pack,Client client)
        {
            controllerManager.HandleRequest(pack, client);
        }

    }
}
