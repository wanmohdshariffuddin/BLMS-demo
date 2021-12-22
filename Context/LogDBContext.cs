using BLMS.Connection;
using BLMS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLMS.Context
{
    public class LogDBContext
    {
        readonly ConnectionSQL connectSQLLog = new ConnectionSQL();
        readonly LogDBContext LogDbContext = new LogDBContext();

        #region AUDIT
        #region GRIDVIEW
        public IEnumerable<AuditLog> GetAuditLog()
        {
            var AuditLogList = new List<AuditLog>();

            Models.Connection connection = connectSQLLog.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
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

        #region ADD AUDIT
        public void AddAuditLog(AuditLog auditlist)
        {
            Models.Connection connection = connectSQLLog.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuditLogAdd", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Command", auditlist.Command);
                cmd.Parameters.AddWithValue("SPName", auditlist.SPName);
                cmd.Parameters.AddWithValue("ScreenPath", auditlist.ScreenPath);
                cmd.Parameters.AddWithValue("OldValue", auditlist.OldValue);
                cmd.Parameters.AddWithValue("NewValue", auditlist.NewValue);
                cmd.Parameters.AddWithValue("CreatedBy", auditlist.CreatedBy);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
        #endregion

        #region ERROR
        #region GRIDVIEW
        public IEnumerable<ErrorLog> GetErrorLog()
        {
            var ErrorLogList = new List<ErrorLog>();

            Models.Connection connection = connectSQLLog.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
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

        #region ADD ERROR
        public void AddErrorLog(string path, string method, Int32 lineNumber, string msg, string UserName)
        {
            Models.Connection connection = connectSQLLog.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spErrorLogAdd", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PageName", path);
                cmd.Parameters.AddWithValue("ErrorMessage", msg);
                cmd.Parameters.AddWithValue("Method", method);
                cmd.Parameters.AddWithValue("LineNumber", lineNumber);
                cmd.Parameters.AddWithValue("CreatedBy", UserName);
                
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
        #endregion
    }
}
