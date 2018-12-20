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
        RoomState roomState = RoomState.WaittingJion;





    }
}
