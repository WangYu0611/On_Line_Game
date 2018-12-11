using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP_Server
{
    class Program
    {
        static byte[] dataBuffer = new byte[2048];
        static Socket ServerSocket;
        static Message msg = new Message();

        static void Main(string[] args)
        {
            StartServerAsync();
            Console.ReadKey();
        }

        /// <summary>
        /// 原理
        /// </summary>
        void StartServerSync()
        {
            Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8888);
            ServerSocket.Bind(iPEndPoint);
            ServerSocket.Listen(10);
            Socket clientSocket = ServerSocket.Accept();


            string msg = "客户端你好！";
            byte[] msgByte = new byte[2048];
            msgByte = System.Text.Encoding.UTF8.GetBytes(msg);

            clientSocket.Send(msgByte);


            byte[] dataBuffer = new byte[2048];
            int ByteCount = clientSocket.Receive(dataBuffer);
            string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, ByteCount);


            Console.WriteLine(msgReceive);


            clientSocket.Close();
            ServerSocket.Close();
        }


        /// <summary>
        /// 异步接收，同开新线程
        /// </summary>
        static void StartServerAsync()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8888);
            ServerSocket.Bind(iPEndPoint);
            ServerSocket.Listen(10);

            ServerSocket.BeginAccept(AcceptCallBack, ServerSocket);


        }

        /// <summary>
        /// 接收客户端消息回调
        /// </summary>
        /// <param name="ar"></param>
        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                msg.AddCount(count);


                // string msg1 = Encoding.UTF8.GetString(dataBuffer, 0, count);
                msg.ReadMessage();



                // Console.WriteLine("消息长度为：  " + count + "    *接收到来自客户端的数据：  " + msg);
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);

            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                if (clientSocket == null)
                {
                    clientSocket.Close();
                }
            }




        }



        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSoeket = ar.AsyncState as Socket;
            Socket clientSocket = serverSoeket.EndAccept(ar);


            string msg1 = "客户端你好！";
            byte[] msgByte = new byte[2048];
            msgByte = System.Text.Encoding.UTF8.GetBytes(msg1);
            clientSocket.Send(msgByte);

            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            ServerSocket.BeginAccept(AcceptCallBack, ServerSocket);
        }
    }
}
