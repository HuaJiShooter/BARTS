using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocal;
using SocketGame.Servers;

namespace SocketGame.Controller
{
    class UserController:BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }


        public MainPack Link(Server server, Client client, MainPack pack)
        {
            if (client.Link(pack))
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            return pack;
        }

        /*
        public MainPack Login(Server server, Client client, MainPack pack)
        {

        }         
         */
    }
}
