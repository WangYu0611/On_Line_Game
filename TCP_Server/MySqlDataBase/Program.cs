﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MySqlDataBase
{
    class Program
    {
        static void Main(string[] args)
        {

            string SQLcon = "Database=test001;DataSource=127.0.0.1;port=3306;User Id=root;pwd=root;";
            MySqlConnection SQL = new MySqlConnection(SQLcon);


            try
            {

                SQL.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }



            //查询
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM user WHERE Id = 1", SQL);//"SELECT * FROM users WHERE username="
            MySqlDataReader reader = cmd1.ExecuteReader();
            Console.WriteLine(reader.HasRows);

            if (reader.HasRows)
            {
                Console.WriteLine("开启成功，读取到数据");
                reader.Read();
                string userName1 = reader.GetString("UseName");
                string passWord1 = reader.GetString("Password");
                Console.WriteLine(userName1 + ":" + passWord1);
            }
            else
            {
                Console.WriteLine("没有读取到");
            }

            reader.Close();

            //插入
            string useName2 = "caiyinhua";
            string passWord2 = "123456";
            MySqlCommand cmd2 = new MySqlCommand("insert into user set UseName='" + useName2 + "'" + ",Password='" + passWord2 + "' ", SQL);

            cmd2.ExecuteNonQuery();


            SQL.Close();

            Console.ReadKey();

        }
    }
}
