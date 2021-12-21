using BLMS.Models.License;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLMS.Context
{
    public class AllUserDBContext
    {
        readonly string connectionstring = "Data Source= 10.49.45.40; Database=BLMS; User ID = Appsa; Password=Opuswebsql2018; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //readonly string connectionstring = "Data Source = 10.249.1.125; Database=BLMSDev;User ID = Appsa; Password=Opuswebsql2017;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region GRIDVIEW
        public IEnumerable<LicenseAllUser> LicenseAllUserGetAll()
        {
            var licenseAllUserList = new List<LicenseAllUser>();

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("splicenseAllUserGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseAllUser = new LicenseAllUser();

                    licenseAllUser.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseAllUser.LicenseName = dr["LicenseName"].ToString();
                    licenseAllUser.CategoryName = dr["CategoryName"].ToString();
                    licenseAllUser.UnitName = dr["UnitName"].ToString();
                    licenseAllUser.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseAllUser.IssuedDT = dr["IssuedDT"].ToString();
                    licenseAllUser.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseAllUser.PIC1Name = dr["PIC1Name"].ToString();
                    licenseAllUser.PIC2Name = dr["PIC2Name"].ToString();
                    licenseAllUser.PIC3Name = dr["PIC3Name"].ToString();
                    licenseAllUser.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseAllUser.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseAllUser.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseAllUser.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseAllUser.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseAllUser.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseAllUser.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseAllUser.UserType = dr["UserType"].ToString();

                    licenseAllUserList.Add(licenseAllUser);
                }

                conn.Close();
            }

            return licenseAllUserList;
        }
        #endregion

        #region GET LICENSE BY ID
        public LicenseAllUser GetLicenseAllUserByID(int? id)
        {
            var licenseAllUser = new LicenseAllUser();

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseAllUserGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseAllUser.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseAllUser.LicenseName = dr["LicenseName"].ToString();
                    licenseAllUser.CategoryName = dr["CategoryName"].ToString();
                    licenseAllUser.DivName = dr["DivName"].ToString();
                    licenseAllUser.UnitName = dr["UnitName"].ToString();
                    licenseAllUser.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseAllUser.IssuedDT = dr["IssuedDT"].ToString();
                    licenseAllUser.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseAllUser.PIC1Name = dr["PIC1Name"].ToString();
                    licenseAllUser.PIC2Name = dr["PIC2Name"].ToString();
                    licenseAllUser.PIC3Name = dr["PIC3Name"].ToString();
                    licenseAllUser.UserType = dr["UserType"].ToString();

                    licenseAllUser.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseAllUser.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseAllUser.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseAllUser.LicenseFileName = dr["LicenseFileName"].ToString();

                    licenseAllUser.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());
                }

                conn.Close();
            }

            return licenseAllUser;
        }
        #endregion
    }
}
