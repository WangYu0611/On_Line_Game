using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace TCP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ClientSocket.Connect("127.0.0.1", 8888);



            byte[] data = new byte[2048];
            int count = ClientSocket.Receive(data);
            string msg = Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine(msg);

            //while (true)
            //{
            //    string s = Console.ReadLine();
            //    if (s == "esc")
            //    {
            //        ClientSocket.Close();
            //        return;
            //    }
            //    // Console.WriteLine(s);
            //    ClientSocket.Send(Encoding.UTF8.GetBytes(s));
            //}

            for (int i = 0; i < 100; i++)
            {
                ClientSocket.Send(Message.GetBytes(i.ToString() + "数值"));
            }


            Console.ReadKey();
            ClientSocket.Close();

        }
    }
}
