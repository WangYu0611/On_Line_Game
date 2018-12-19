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

        public string Reginster(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];

            bool res = userDAO.GetUserByUsername(client.MySqlConn, username);
            if (res)
            {
                Console.WriteLine("用户名重复，添加新用户失败");
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                int num = userDAO.AddUser(client.MySqlConn, username, password);
                Console.WriteLine("增加" + num + "个新用户    账号： " + username + "    密码：" + password);
                return ((int)ReturnCode.Success).ToString();
            }

        }
    }
}
