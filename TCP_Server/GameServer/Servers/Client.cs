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

namespace GameServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
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



        private void Close()
        {
            ConnHelper.CloseConnection(mySqlConn);
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);

        }

    }
}
