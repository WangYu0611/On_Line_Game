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


    }
}
