using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Servers
{
    class Message
    {

        private byte[] data = new byte[2048];
        private int startIndex = 0;



        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }

        public void AddCount(int count)
        {
            startIndex += count;
        }


        /// <summary>
        /// 解析数据(粘包问题)
        /// </summary>
        public void ReadMessage(int newDataAmount, Action<RequestCode, ActionCode, string> ProcessDataCallBack)
        {
            startIndex += newDataAmount;
            while (true)
            {

                if (startIndex <= 4)
                {
                    return;
                }

                int count = BitConverter.ToInt32(data, 0);

                if ((startIndex - 4) >= count)
                {

                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);

                    string msg = Encoding.UTF8.GetString(data, 12, count - 8);
                    Console.WriteLine("解析出来的数据：" + msg);
                    Console.WriteLine(count + "," + startIndex);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);

                    ProcessDataCallBack(requestCode, actionCode, msg);
                }
                else
                {
                    break;
                }
            }
        }



        public static byte[] PackData(ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newByte = dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>();//.Concat(dataBytes);
            return newByte.Concat(dataBytes).ToArray<byte>();

        }




    }
}
