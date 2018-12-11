using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Server
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
        public void ReadMessage()
        {

            while (true)
            {

                if (startIndex <= 4)
                {
                    return;
                }

                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= count)
                {
                    string msg = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出来的数据：    " + msg);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }

    }
}
