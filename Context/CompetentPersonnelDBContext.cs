using BLMS.Connection;
using BLMS.Models.SOP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLMS.Context
{
    public class CompetentPersonnelDBContext
    {
        readonly ConnectionSQL connectSQLCompetent = new ConnectionSQL();
        readonly LogDBContext LogDbContext = new LogDBContext();

        #region GRIDVIEW
        public IEnumerable<Competent> CompetentPersonnelGetAll(string UserName)
        {
            var competentList = new List<Competent>();

            Models.Connection connection = connectSQLCompetent.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spCompetentPersonnelGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("UserName", UserName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var competent = new Competent();

                    competent.PersonnelId = Convert.ToInt32(dr["PersonnelId"].ToString());
                    competent.PersonnelName = dr["PersonnelName"].ToString();
                    competent.AppointedDT = string.Format("{0:d/M/yyyy}", dr["AppointedDT"]);
                    competent.ExpiredDT = string.Format("{0:d/M/yyyy}", dr["ExpiredDT"]);
                    competent.CertFrom = dr["CertFrom"].ToString();
                    competent.CertType = dr["CertType"].ToString();
                    competent.YearAward = dr["PersonnelName"].ToString();
                    competent.CertFileName = dr["CertFileName"].ToString();
                    competent.BusinessDiv = dr["BusinessDiv"].ToString();
                    competent.BusinessUnit = dr["BusinessUnit"].ToString();
                    competent.UserRole = dr["UserRole"].ToString();


                    competentList.Add(competent);
                }

                conn.Close();
            }

            return competentList;
        }
        #endregion

    }
}
