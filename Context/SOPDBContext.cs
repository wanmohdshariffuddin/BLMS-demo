using BLMS.Connection;
using BLMS.Models.Admin;
using BLMS.Models.SOP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLMS.Context
{
    public class SOPDBContext
    {
        readonly ConnectionSQL connectSQLSOP = new ConnectionSQL();
        readonly LogDBContext LogDbContext = new LogDBContext();

        #region AUTHORITY LINK
        #region GRIDVIEW
        public IEnumerable<Authority> AuthorityGetAll()
        {
            var authorityList = new List<Authority>();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuthorityLinkGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var authority = new Authority();

                    authority.AuthorityID = Convert.ToInt32(dr["AuthorityID"].ToString());
                    authority.AuthorityName = dr["AuthorityName"].ToString();
                    authority.AuthorityLink = dr["AuthorityLink"].ToString();

                    authorityList.Add(authority);
                }

                conn.Close();
            }

            return authorityList;
        }
        #endregion

        #region CREATE
        public void AddAuthority(Authority authority, string UserName)
        {
            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuthorityLinkAdd", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("AuthorityName", authority.AuthorityName);
                cmd.Parameters.AddWithValue("AuthorityLink", authority.AuthorityLink);
                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region EDIT
        public void EditAuthority(Authority authority, string UserName)
        {
            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuthorityLinkEdit", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("AuthorityID", authority.AuthorityID);
                cmd.Parameters.AddWithValue("AuthorityName", authority.AuthorityName);
                cmd.Parameters.AddWithValue("AuthorityLink", authority.AuthorityLink);
                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region DELETE
        public void DeleteAuthority(int? id)
        {
            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuthorityLinkDelete", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("AuthorityID", id);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region GET AUTHORITY BY ID
        public Authority GetAuthorityByID(int? id)
        {
            var authority = new Authority();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuthorityLinkGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("AuthorityID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    authority.AuthorityID = Convert.ToInt32(dr["AuthorityID"].ToString());
                    authority.AuthorityName = dr["AuthorityName"].ToString();
                    authority.AuthorityLink = dr["AuthorityLink"].ToString();
                    authority.OldAuthorityName = dr["AuthorityName"].ToString();
                    authority.OldAuthorityLink = dr["AuthorityLink"].ToString();
                }

                conn.Close();
            }

            return authority;
        }
        #endregion

        #region CHECK EXISTING AUTHORITY
        public Authority CheckAuthorityByName(string AuthorityName)
        {
            var authority = new Authority();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spAuthorityLinkCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("AuthorityName", AuthorityName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    authority.ExistData = Convert.ToInt32(dr["ExistData"].ToString());
                }

                conn.Close();
            }

            return authority;
        }
        #endregion
        #endregion

        #region SOP & GUIDELINE
        #region GRIDVIEW
        public IEnumerable<SOP> SOPGetAll()
        {
            var sopList = new List<SOP>();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spSOPGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var sop = new SOP();

                    sop.SOPID = Convert.ToInt32(dr["ID"].ToString());
                    sop.SOPName = dr["SOPName"].ToString();
                    sop.RefNo = dr["RefNo"].ToString();
                    sop.RevNo = dr["RevNo"].ToString();
                    sop.EffectiveDT = string.Format("{0:d/M/yyyy}", dr["EffectiveDT"]);
                    sop.HasFile = Convert.ToBoolean(dr["HasFile"].ToString());

                    sopList.Add(sop);
                }

                conn.Close();
            }

            return sopList;
        }
        #endregion

        #region GET SOP BY ID
        public SOP GetSOPByID(int? id)
        {
            var sop = new SOP();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spSOPGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("ID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    sop.SOPID = Convert.ToInt32(dr["ID"].ToString());
                    sop.OldSOPName = dr["SOPName"].ToString();
                    sop.SOPName = dr["SOPName"].ToString();
                    sop.RefNo = dr["RefNo"].ToString();
                    sop.RevNo = dr["RevNo"].ToString();
                    sop.EffectiveDT = string.Format("{0:d/M/yyyy}", dr["EffectiveDT"]);
                    sop.HasFile = Convert.ToBoolean(dr["HasFile"].ToString());

                    sop.SOPFileName = dr["SOPFileName"].ToString();
                    sop.OldSOPFileName = dr["SOPFileName"].ToString();
                }

                conn.Close();
            }

            return sop;
        }
        #endregion

        #region CREATE
        public void CreateSOP(SOP sop, string UserName)
        {
            AuditLog auditLog = new AuditLog();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spSOPAdd", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("SOPName", sop.SOPName);
                cmd.Parameters.AddWithValue("RefNo", sop.RefNo);
                cmd.Parameters.AddWithValue("RevNo", sop.RevNo);
                cmd.Parameters.AddWithValue("EffectiveDT", sop.EffectiveDT);
                cmd.Parameters.AddWithValue("SOPFileName", sop.SOPFileName);
                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();

                #region AUDIT
                auditLog.Command = "CREATE";
                auditLog.ScreenPath = "SOP";
                auditLog.CreatedBy = UserName;
                auditLog.SPName = cmd.CommandText.ToString();
                auditLog.OldValue = "-";
                auditLog.NewValue = "'SOP Name': " + sop.SOPName;

                LogDbContext.AddAuditLog(auditLog);
                #endregion

                conn.Close();
            }
        }
        #endregion

        #region EDIT
        public void EditSOP(SOP sop, string UserName)
        {
            AuditLog auditLog = new AuditLog();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spSOPEdit", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("ID", sop.SOPID);
                cmd.Parameters.AddWithValue("SOPName", sop.SOPName);
                cmd.Parameters.AddWithValue("RefNo", sop.RefNo);
                cmd.Parameters.AddWithValue("RevNo", sop.RevNo);
                cmd.Parameters.AddWithValue("EffectiveDT", sop.EffectiveDT);
                cmd.Parameters.AddWithValue("SOPFileName", sop.SOPFileName);
                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();

                #region AUDIT
                auditLog.Command = "EDIT";
                auditLog.ScreenPath = "SOP";
                auditLog.CreatedBy = UserName;
                auditLog.SPName = cmd.CommandText.ToString();
                auditLog.OldValue = "-";
                auditLog.NewValue = "'SOP Name': " + sop.SOPName;

                LogDbContext.AddAuditLog(auditLog);
                #endregion

                conn.Close();
            }
        }
        #endregion

        #region CHECK DUPLICATE SOP NAME
        public SOP CheckSOPByName(string SOPName)
        {
            var sop = new SOP();

            Models.Connection connection = connectSQLSOP.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spSOPCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("SOPName", SOPName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    sop.ExistData = Convert.ToInt32(dr["ExistData"].ToString());
                }

                conn.Close();
            }

            return sop;
        }
        #endregion
        #endregion
    }
}
