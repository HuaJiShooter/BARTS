using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocal;
using SocketGame.Servers;
using System.Reflection;

namespace SocketGame.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controlDict = new Dictionary<RequestCode, BaseController>();

        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;

            UserController userController = new UserController();
            controlDict.Add(userController.GetRequestCode,userController);
        }

        //处理pack
        public void HandleRequest(MainPack pack,Client client)
        {
            if(controlDict.TryGetValue(pack.Requestcode, out BaseController controller))
            {
                //通过ActionCode获取controller中的方法
                string metname = pack.Actioncode.ToString();
                MethodInfo method = controller.GetType().GetMethod(metname);
                if(method == null)
                {
                    Console.WriteLine("没有找到对应的处理方法");
                    return;
                }
                //执行方法，obj为方法的参数，controller为方法对象，
                object[] obj = new object[] { server, client, pack };
                object ret = method.Invoke(controller,obj);
                if (ret != null)//ret有值则返回给客户端,ret是指return
                {
                    //将object类的ret转化为MainPack,然后进行发送
                    client.Send(ret as MainPack);
                }
            }
            else
            {
                Console.WriteLine("没有找到对应的Controller处理方法");
            }
        }
    }
}
