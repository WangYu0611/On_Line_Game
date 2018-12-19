using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    class Server
    {

        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private ControllerManager controllerManager;

        public Server()
        {

        }

        public Server(string ip, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ip, port);
        }


        public void SetIpAndPort(string ip, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }


        public void StartServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(50);
            serverSocket.BeginAccept(AcceptCallBack, null);


        }


        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.StartClient();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }



        public void RemoveClient(Client client)
        {
            //防止同一时间内多个客户端删除
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);

        }


        public void HandlerRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client, Server server)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client, server);
        }

    }
}
