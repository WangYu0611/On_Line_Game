using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
    class ConnHelper
    {
        public const string ConnectionString = "datasource=127.0.0.1;port=3306;database=game01;user=root;password=root";

        public static MySqlConnection Connect()
        {
            MySqlConnection Conn = new MySqlConnection(ConnectionString);

            try
            {

                Conn.Open();
                return Conn;
            }
            catch (Exception e)
            {

                Console.WriteLine("连接数据库异常：  " + e);
                return null;
            }



        }




        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
            else
            {
                Console.WriteLine("MysqlConnection不能为空！");
            }

        }


    }
}
