using BLMS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Context
{
    public class LogDBContext
    {
        readonly string connectionstring = "Data Source= 10.49.45.40; Database=BLMS; User ID = Appsa; Password=Opuswebsql2018; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //readonly string connectionstring = "Data Source = 10.249.1.125; Database=BLMSDev;User ID = Appsa; Password=Opuswebsql2017;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region AUDIT
        #region GRIDVIEW
        public IEnumerable<AuditLog> GetAuditLog()
        {
            var AuditLogList = new List<AuditLog>();


            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuditLogGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var auditList = new AuditLog();

                    auditList.Command = dr["Command"].ToString();
                    auditList.SPName = dr["SPName"].ToString();
                    auditList.ScreenPath = dr["ScreenPath"].ToString();
                    auditList.OldValue = dr["OldValue"].ToString();
                    auditList.NewValue = dr["NewValue"].ToString();
                    auditList.CreatedBy = dr["CreatedBy"].ToString();
                    auditList.CreatedDt = dr["CreatedDt"].ToString();

                    AuditLogList.Add(auditList);
                }

                conn.Close();
            }

            return AuditLogList;
        }
        #endregion
        #endregion

        #region ERROR
        #region GRIDVIEW
        public IEnumerable<ErrorLog> GetErrorLog()
        {
            var ErrorLogList = new List<ErrorLog>();


            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spErrorLogGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var errorList = new ErrorLog();

                    errorList.PageName = dr["PageName"].ToString();
                    errorList.ErrorMessage = dr["ErrorMessage"].ToString();
                    errorList.Method = dr["Method"].ToString();
                    errorList.LineNumber = dr["LineNumber"].ToString();
                    errorList.CreatedBy = dr["CreatedBy"].ToString();
                    errorList.CreatedDt = dr["CreatedDt"].ToString();

                    ErrorLogList.Add(errorList);
                }

                conn.Close();
            }

            return ErrorLogList;
        }
        #endregion
        #endregion
    }
}
