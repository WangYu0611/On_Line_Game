using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    class Message
    {
        public static byte[] GetBytes(string msg)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(msg);
            int dataLength = dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            byte[] SendDataBytes = lengthBytes.Concat(dataBytes).ToArray();
            return SendDataBytes;
        }

    }
}
