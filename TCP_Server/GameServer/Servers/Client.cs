using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;


namespace GameServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        private User user;
        private Result result;
        private Room _room;
        public Room room
        {
            set { _room = value; }
        }


        private MySqlConnection mySqlConn;

        public MySqlConnection MySqlConn
        {
            get { return mySqlConn; }
        }
        public Client()
        {

        }

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mySqlConn = ConnHelper.Connect();
        }

        public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }

        public string GetUserData()
        {
            return user.Id + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
        }


        public void StartClient()
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                return;
            }
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || !clientSocket.Connected)
                {
                    return;
                }

                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }

                //TODO 处理接收到的数据
                msg.ReadMessage(count, OnProcessMessage);

                StartClient();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }



        }


        public void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandlerRequest(requestCode, actionCode, data, this, this.server);
        }

        public void Send(ActionCode actionCode, string data)
        {
            byte[] bytes = Message.PackData(actionCode, data);
            clientSocket.Send(bytes);



        }

        public int GetUserID()
        {
            return user.Id;
        }

        private void Close()
        {
            ConnHelper.CloseConnection(mySqlConn);
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
            if (_room != null)
            {
                _room.Close(this);
            }

        }

    }
}
