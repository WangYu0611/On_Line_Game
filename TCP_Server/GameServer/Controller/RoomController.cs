using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class RoomController : BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }


        public string CreateRoom(string data, Client client, Server server)
        {
            server.CreateRoom(client);
            return ((int)ReturnCode.Success).ToString();
        }

        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in server.GetRoomList())
            {
                if (item.IsWaitingJoin())
                {
                    sb.Append(item.GetHostData() + "|");
                }
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }

            Console.WriteLine(sb);
            return sb.ToString();
        }

        public string JoinRoom(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            Room room = server.GetRoomByID(id);
            if (room == null)
            {
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if (room.IsWaitingJoin() == false)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                room.AddClientPlayer(client);
                string allPlayerData = room.GetAllPlayerData();//  "RETURNCODE-ID,USERNAME,TC,WC,|ID,USERNAME,TC,WC"
                return ((int)ReturnCode.Success).ToString() + "-" + allPlayerData;
            }




        }





    }
}
