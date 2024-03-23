using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGame.Servers;

namespace SocketGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服务端开始启动");
            Server server = new Server(25656);
            Console.Read();
        }


    }
}