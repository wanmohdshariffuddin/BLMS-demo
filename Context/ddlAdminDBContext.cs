using BLMS.Connection;
using BLMS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLMS.Context
{
    public class ddlAdminDBContext
    {
        readonly ConnectionSQL connectSQLDDLAdmin = new ConnectionSQL();
        readonly LogDBContext LogDbContext = new LogDBContext();

        #region BUSINESS UNIT
        #region GET BUSINESS DIVISION
        public IEnumerable<BusinessUnit> ddlBusinessDiv()
        {
            var businessUnitList = new List<BusinessUnit>();

            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLBusinessDiv", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var businessUnit = new BusinessUnit();

                    businessUnit.DivID = Convert.ToInt32(dr["DivID"].ToString());
                    businessUnit.DivName = dr["DivName"].ToString();

                    businessUnitList.Add(businessUnit);
                }

                conn.Close();
            }

            return businessUnitList;
        }
        #endregion
        #endregion

        #region PIC
        #region GET STAFF NAME
        public IEnumerable<PIC> ddlStaffNamePIC()
        {
            var staffNamePICList = new List<PIC>();

            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLStaff", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var staffNamePIC = new PIC();

                    staffNamePIC.PICStaffNo = dr["empl_no"].ToString();
                    staffNamePIC.PICName = dr["staff_name"].ToString();

                    staffNamePICList.Add(staffNamePIC);
                }

                conn.Close();
            }

            return staffNamePICList;
        }
        #endregion

        #region GET USER TYPE
        public IEnumerable<PIC> ddlUserTypePIC()
        {
            var userTypePICList = new List<PIC>();

            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLPICUserType", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var userTypePIC = new PIC();

                    userTypePIC.UserTypeID = Convert.ToInt32(dr["UserTypeID"].ToString());
                    userTypePIC.UserType = dr["UserType"].ToString();

                    userTypePICList.Add(userTypePIC);
                }

                conn.Close();
            }

            return userTypePICList;
        }
        #endregion
        #endregion

        #region USER ROLE
        #region GET STAFF NAME
        public IEnumerable<UserRole> ddlStaffNameUserRole()
        {
            var staffNameUserRoleList = new List<UserRole>();

            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLStaff", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var staffNameUserRole = new UserRole();

                    staffNameUserRole.UserRoleStaffNo = dr["empl_no"].ToString();
                    staffNameUserRole.UserRoleName = dr["staff_name"].ToString();

                    staffNameUserRoleList.Add(staffNameUserRole);
                }

                conn.Close();
            }

            return staffNameUserRoleList;
        }
        #endregion

        #region GET USER ROLE
        public IEnumerable<UserRole> ddlRoleUserRole()
        {
            var roleUserRoleList = new List<UserRole>();

            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var roleUserRole = new UserRole();

                    roleUserRole.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    roleUserRole.Role = dr["Role"].ToString();

                    roleUserRoleList.Add(roleUserRole);
                }

                conn.Close();
            }

            return roleUserRoleList;
        }
        #endregion

        #region GET USER TYPE
        public IEnumerable<UserRole> ddlUserTypeUserRole()
        {
            var userTypeUserRoleList = new List<UserRole>();

            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLUserType", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var userTypeUserRole = new UserRole();

                    userTypeUserRole.UserTypeID = Convert.ToInt32(dr["UserTypeID"].ToString());
                    userTypeUserRole.UserType = dr["UserType"].ToString();

                    userTypeUserRoleList.Add(userTypeUserRole);
                }

                conn.Close();
            }

            return userTypeUserRoleList;
        }
        #endregion

        #region ddlUserTypeLinkedRole
        public DataSet ddlUserTypeLinkedRole(int RoleID)
        {
            Models.Connection connection = connectSQLDDLAdmin.GetConnection();

            using (SqlConnection conn = new SqlConnection(connection.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spDDLUserTypeLinkedRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RoleID", RoleID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        #endregion
        #endregion
    }
}
