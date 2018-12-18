using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;



namespace GameServer.Controller
{
    class UserController : BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }



        private UserDAO userDAO = new UserDAO();


        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerfyUser(client.MySqlConn, strs[0], strs[1]);
            if (user == null)
            {
                Console.WriteLine("账号：" + strs[0] + "    数据库返回失败");
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Console.WriteLine("账号：" + strs[0] + "    数据库返回成功");
                return ((int)ReturnCode.Success).ToString();
            }




        }


    }
}
