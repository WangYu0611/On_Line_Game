using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
    class ResultDAO
    {
        public Result GetResultByUseId(MySqlConnection mySqlConnection, int userId)
        {

            MySqlDataReader reader = null;
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand("select*from result where userid=@userid", mySqlConnection);
                mySqlCommand.Parameters.AddWithValue("userid", userId);

                reader = mySqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");

                    Result result = new Result(id, userId, totalCount, winCount);
                    return result;
                }
                else
                {
                    Result result = new Result(-1, userId, 0, 0);
                    return result;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("在GetResultByUseId的时候出现异常：" + e);

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
