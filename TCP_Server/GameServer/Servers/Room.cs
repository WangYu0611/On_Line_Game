using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Servers
{
    class Room
    {
        enum RoomState
        {
            WaittingJion,
            WaittingBattle,
            Battle,
            End
        }



        private List<Client> clientRoom = new List<Client>();
        private RoomState roomState = RoomState.WaittingJion;
        private Server server;

        public Room(Server server)
        {
            this.server = server;
        }

        public bool IsWaitingJoin()
        {
            return roomState == RoomState.WaittingJion;
        }

        public void AddClientPlayer(Client client)
        {
            clientRoom.Add(client);
            client.room = this;
            if (clientRoom.Count >= 2)
            {
                roomState = RoomState.WaittingBattle;
            }
        }

        public string GetHostData()
        {
            return clientRoom[0].GetUserData();
        }


        public void Close(Client client)
        {
            if (client == clientRoom[0])
            {
                server.RemoveRoom(this);
            }
            else
            {
                clientRoom.Remove(client);
            }
        }

        public int GetRoomID()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserID();
            }
            return -1;
        }


        public string GetAllPlayerData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in clientRoom)
            {
                sb.Append(item.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();

        }

    }
}
