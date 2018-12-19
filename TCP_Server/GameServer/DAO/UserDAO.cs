using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        public User VerfyUser(MySqlConnection mySql, string username, string password)
        {

            MySqlDataReader reader = null;
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand("select*from user where username=@username and password=@password", mySql);
                mySqlCommand.Parameters.AddWithValue("username", username);
                mySqlCommand.Parameters.AddWithValue("password", password);
                reader = mySqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    int id = reader.GetInt32("id");

                    User user = new User(id, username, password);
                    return user;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("在Verify的时候出现异常：" + e);

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;



        }

        public bool GetUserByUsername(MySqlConnection mySql, string username)
        {

            MySqlDataReader reader = null;

            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand("select*from user where username=@username ", mySql);
                mySqlCommand.Parameters.AddWithValue("username", username);
                reader = mySqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername的时候出现异常：" + e);

            }

            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return false;

        }


        public int AddUser(MySqlConnection mySql, string username, string password)
        {
            int num = 0;
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand("insert into user set username=@username, password=@password ", mySql);
                mySqlCommand.Parameters.AddWithValue("username", username);
                mySqlCommand.Parameters.AddWithValue("password", password);
                num = mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser的时候出现异常：" + e);
            }


            return num;
        }
    }
}
