using BLMS.Connection;
using BLMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Context
{
    public class LoginDBContext
    {
        readonly ConnectionSQL connectSQL = new ConnectionSQL();

        //GET USER DETAILS FOR AUTHENTICATION
        public User GetUserByEmail(string STAFF_EMAIL)
        {
            var user = new User();

            Models.Connection connection = connectSQL.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spUserGetbyEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("STAFF_EMAIL", STAFF_EMAIL);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    user.USER_ID = Convert.ToInt32(dr["USER_ID"].ToString());
                    user.STAFF_EMAIL = dr["STAFF_EMAIL"].ToString();
                    user.STAFF_NAME = dr["STAFF_NAME"].ToString();
                    user.STAFF_NO = dr["STAFF_NO"].ToString();
                    user.PASSWORD = dr["PASSWORD"].ToString();

                    user.ROLE = dr["ROLE"].ToString();
                    user.ACCESS_LEVEL = dr["USER_TYPE"].ToString();
                    user.WRITE_ACCESS = dr["WRITE_ACCESS"].ToString();
                }

                conn.Close();
            }

            return user;
        }
    }
}
