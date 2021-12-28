using BLMS.Models.License;
using BLMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Context
{
    public class LicenseDBContext
    {
        #region LICENSE SITE
        #region GRIDVIEW
        public IEnumerable<LicenseSite> LicenseSiteGetAll()
        {
            var counter = 1;

            var licenseSiteList = new List<LicenseSite>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseSite = new LicenseSite();

                    licenseSite.IndexNo = counter;

                    licenseSite.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseSite.LicenseName = dr["LicenseName"].ToString();
                    licenseSite.CategoryName = dr["CategoryName"].ToString();
                    licenseSite.UnitName = dr["UnitName"].ToString();
                    licenseSite.IssuedDT = dr["IssuedDT"].ToString();
                    licenseSite.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseSite.PIC1Name = dr["PIC1Name"].ToString();
                    licenseSite.PIC2Name = dr["PIC2Name"].ToString();
                    licenseSite.PIC3Name = dr["PIC3Name"].ToString();
                    licenseSite.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseSite.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseSite.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseSite.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseSiteList.Add(licenseSite);

                    counter++;
                }

                conn.Close();
            }

            return licenseSiteList;
        }
        #endregion

        #region REGISTER
        public void RegisterLicenseSite(LicenseSite licenseSite, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteRegister", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("CategoryID", licenseSite.CategoryID);
                cmd.Parameters.AddWithValue("DivID", licenseSite.DivID);
                cmd.Parameters.AddWithValue("UnitID", licenseSite.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", licenseSite.LicenseName);
                cmd.Parameters.AddWithValue("RegistrationNo", licenseSite.RegistrationNo);
                cmd.Parameters.AddWithValue("SerialNo", licenseSite.SerialNo);
                cmd.Parameters.AddWithValue("IssuedDT", licenseSite.IssuedDT);
                cmd.Parameters.AddWithValue("ExpiredDT", licenseSite.ExpiredDT);
                cmd.Parameters.AddWithValue("PIC1Name", licenseSite.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", licenseSite.PIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", licenseSite.PIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", licenseSite.Remarks);

                //License File
                cmd.Parameters.AddWithValue("LicenseFileName", licenseSite.LicenseFileName);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region RENEWAL
        public void RenewalLicenseSite(RenewalLicenseSiteViewModel licenseSite, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteRenewal", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("CategoryID", licenseSite.RenewalLicense.CategoryID);
                cmd.Parameters.AddWithValue("DivID", licenseSite.RenewalLicense.DivID);
                cmd.Parameters.AddWithValue("UnitID", licenseSite.RenewalLicense.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", licenseSite.RenewalLicense.LicenseName);
                cmd.Parameters.AddWithValue("RegistrationNo", licenseSite.RenewalLicense.NewRegistrationNo);
                cmd.Parameters.AddWithValue("SerialNo", licenseSite.RenewalLicense.NewSerialNo);
                cmd.Parameters.AddWithValue("IssuedDT", licenseSite.RenewalLicense.NewIssuedDT);
                cmd.Parameters.AddWithValue("ExpiredDT", licenseSite.RenewalLicense.NewExpiredDT);
                cmd.Parameters.AddWithValue("PIC1Name", licenseSite.RenewalLicense.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", licenseSite.RenewalLicense.PIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", licenseSite.RenewalLicense.PIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", licenseSite.RenewalLicense.NewRemarks);

                //License File
                cmd.Parameters.AddWithValue("LicenseFileName", licenseSite.RenewalLicense.NewLicenseFileName);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region GET LICENSE BY ID
        //License By ID
        public LicenseSite GetLicenseSiteByID(int? id)
        {
            var licenseSite = new LicenseSite();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseSite.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseSite.OldLicenseName = dr["LicenseName"].ToString();
                    licenseSite.LicenseName = dr["LicenseName"].ToString();
                    licenseSite.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                    licenseSite.CategoryName = dr["CategoryName"].ToString();
                    licenseSite.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseSite.SerialNo = dr["SerialNo"].ToString();
                    licenseSite.DivID = Convert.ToInt32(dr["DivID"].ToString());
                    licenseSite.DivName = dr["DivName"].ToString();
                    licenseSite.UnitID = Convert.ToInt32(dr["UnitID"].ToString());
                    licenseSite.UnitName = dr["UnitName"].ToString();
                    licenseSite.IssuedDT = dr["IssuedDT"].ToString();
                    licenseSite.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseSite.PIC1StaffNo = dr["PIC1StaffNo"].ToString();
                    licenseSite.PIC1Name = dr["PIC1Name"].ToString();
                    licenseSite.PIC1Email = dr["PIC1Email"].ToString();
                    licenseSite.PIC2StaffNo = dr["PIC2StaffNo"].ToString();
                    licenseSite.PIC2Name = dr["PIC2Name"].ToString();
                    licenseSite.PIC2Email = dr["PIC2Email"].ToString();
                    licenseSite.PIC3StaffNo = dr["PIC3StaffNo"].ToString();
                    licenseSite.PIC3Name = dr["PIC3Name"].ToString();
                    licenseSite.PIC3Email = dr["PIC3Email"].ToString();
                    licenseSite.Remarks = dr["Remarks"].ToString();
                    licenseSite.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseSite.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseSite.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseSite.LicenseFileName = dr["LicenseFileName"].ToString();

                    licenseSite.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseSite.UserNameStaffNo = dr["CreatedBY"].ToString();
                }

                conn.Close();
            }

            return licenseSite;
        }
        #endregion

        #region HISTORY GRIDVIEW
        public IEnumerable<LicenseSite> LicenseSiteGetLog(string LicenseName)
        {
            var counter = 1;

            var licenseSiteList = new List<LicenseSite>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteGetLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("LicenseName", LicenseName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseSite = new LicenseSite();

                    licenseSite.IndexNo = counter;

                    licenseSite.HistoryLicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseSite.HistoryLicenseName = dr["LicenseName"].ToString();
                    licenseSite.HistoryRegistrationNo = dr["RegistrationNo"].ToString();
                    licenseSite.HistorySerialNo = dr["SerialNo"].ToString();
                    licenseSite.HistoryIssuedDT = dr["IssuedDT"].ToString();
                    licenseSite.HistoryExpiredDT = dr["ExpiredDT"].ToString();
                    licenseSite.HistoryPIC1Name = dr["PIC1Name"].ToString();
                    licenseSite.HistoryPIC2Name = dr["PIC2Name"].ToString();
                    licenseSite.HistoryPIC3Name = dr["PIC3Name"].ToString();
                    licenseSite.HistoryisRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseSite.HistoryisApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseSite.HistoryisRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseSite.HistoryisRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseSite.HistoryisRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseSite.HistoryRenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseSite.HistoryhasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseSiteList.Add(licenseSite);

                    counter++;
                }

                conn.Close();
            }

            return licenseSiteList;
        }
        #endregion

        #region CHECK DUPLICATE LICENSE SITE NAME
        public LicenseSite CheckLicenseSiteByName(string LicenseName)
        {
            var licenseSite = new LicenseSite();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseName", LicenseName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseSite.ExistData = Convert.ToInt32(dr["ExistData"].ToString());
                }

                conn.Close();
            }

            return licenseSite;
        }
        #endregion

        #region CHECK DUPLICATE LICENSE SITE FILE NAME
        public LicenseSite CheckLicenseSiteFileByName(string LicenseFileName)
        {
            var licenseSite = new LicenseSite();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteFileCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseFileName", LicenseFileName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseSite.ExistData = Convert.ToInt32(dr["ExistData"].ToString());
                }

                conn.Close();
            }

            return licenseSite;
        }
        #endregion

        #region EDIT LICENSE SITE
        //License Request
        public void EditLicenseSite(LicenseSite licenseSite, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseSiteEdit", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", licenseSite.LicenseID);
                cmd.Parameters.AddWithValue("CategoryID", licenseSite.CategoryID);
                cmd.Parameters.AddWithValue("DivID", licenseSite.DivID);
                cmd.Parameters.AddWithValue("UnitID", licenseSite.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", licenseSite.LicenseName);
                cmd.Parameters.AddWithValue("RegistrationNo", licenseSite.RegistrationNo);
                cmd.Parameters.AddWithValue("SerialNo", licenseSite.SerialNo);
                cmd.Parameters.AddWithValue("IssuedDT", licenseSite.IssuedDT);
                cmd.Parameters.AddWithValue("ExpiredDT", licenseSite.ExpiredDT);
                cmd.Parameters.AddWithValue("PIC1Name", licenseSite.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2Name", licenseSite.PIC2Name);
                cmd.Parameters.AddWithValue("PIC3Name", licenseSite.PIC3Name);
                cmd.Parameters.AddWithValue("LicenseFileName", licenseSite.LicenseFileName);
                cmd.Parameters.AddWithValue("Remarks", licenseSite.Remarks);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
        #endregion

        #region LICENSE HQ
        #region GRIDVIEW
        public IEnumerable<LicenseHQ> LicenseHQGetAll()
        {
            var licenseHQList = new List<LicenseHQ>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                var counter = 1;

                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseHQ = new LicenseHQ();

                    licenseHQ.indexNo = counter;

                    licenseHQ.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseHQ.LicenseName = dr["LicenseName"].ToString();
                    licenseHQ.CategoryName = dr["CategoryName"].ToString();
                    licenseHQ.UnitName = dr["UnitName"].ToString();
                    licenseHQ.IssuedDT = dr["IssuedDT"].ToString();
                    licenseHQ.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseHQ.PIC1Name = dr["PIC1Name"].ToString();
                    licenseHQ.PIC2Name = dr["PIC2Name"].ToString();
                    licenseHQ.PIC3Name = dr["PIC3Name"].ToString();
                    licenseHQ.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseHQ.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseHQ.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseHQ.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseHQ.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseHQ.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseHQ.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseHQList.Add(licenseHQ);

                    counter++;
                }

                conn.Close();
            }

            return licenseHQList;
        }
        #endregion

        #region REQUEST
        //License Request
        public void RequestLicenseHQ(LicenseHQ licenseHQ, string UserName)
        {

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQRequest", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("CategoryID", licenseHQ.CategoryID);
                cmd.Parameters.AddWithValue("DivID", licenseHQ.DivID);
                cmd.Parameters.AddWithValue("UnitID", licenseHQ.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", licenseHQ.LicenseName);
                cmd.Parameters.AddWithValue("PIC1Name", licenseHQ.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", licenseHQ.PIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", licenseHQ.PIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", licenseHQ.Remarks);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region GET LICENSE BY ID
        //Get License HQ By ID
        public LicenseHQ GetLicenseHQByID(int? id)
        {
            var licenseHQ = new LicenseHQ();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseHQ.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseHQ.LicenseName = dr["LicenseName"].ToString();
                    licenseHQ.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                    licenseHQ.CategoryName = dr["CategoryName"].ToString();
                    licenseHQ.DivID = Convert.ToInt32(dr["DivID"].ToString());
                    licenseHQ.DivName = dr["DivName"].ToString();
                    licenseHQ.UnitID = Convert.ToInt32(dr["UnitID"].ToString());
                    licenseHQ.UnitName = dr["UnitName"].ToString();

                    licenseHQ.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseHQ.SerialNo = dr["SerialNo"].ToString();

                    licenseHQ.IssuedDT = dr["IssuedDT"].ToString();
                    licenseHQ.ExpiredDT = dr["ExpiredDT"].ToString();

                    licenseHQ.PIC1StaffNo = dr["PIC1StaffNo"].ToString();
                    licenseHQ.PIC1Name = dr["PIC1Name"].ToString();
                    licenseHQ.PIC1Email = dr["PIC1Email"].ToString();

                    licenseHQ.PIC2StaffNo = dr["PIC2StaffNo"].ToString();
                    licenseHQ.PIC2Name = dr["PIC2Name"].ToString();
                    licenseHQ.PIC2Email = dr["PIC2Email"].ToString();
                    licenseHQ.PIC3StaffNo = dr["PIC3StaffNo"].ToString();
                    licenseHQ.PIC3Name = dr["PIC3Name"].ToString();
                    licenseHQ.PIC3Email = dr["PIC3Email"].ToString();
                    licenseHQ.Remarks = dr["Remarks"].ToString();

                    licenseHQ.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseHQ.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseHQ.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseHQ.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseHQ.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseHQ.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseHQ.LicenseFileName = dr["LicenseFileName"].ToString();

                    licenseHQ.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseHQ.RejectionRemarks = dr["RejectRemarks"].ToString();
                }

                conn.Close();
            }

            return licenseHQ;
        }
        #endregion

        #region HISTORY GRIDVIEW
        public IEnumerable<LicenseHQ> LicenseHQGetLog(string LicenseName)
        {
            var counter = 1;

            var licenseHQList = new List<LicenseHQ>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQGetLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("LicenseName", LicenseName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseHQ = new LicenseHQ();

                    licenseHQ.indexNo = counter;

                    licenseHQ.HistoryLicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseHQ.HistoryLicenseName = dr["LicenseName"].ToString();
                    licenseHQ.HistoryRegistrationNo = dr["RegistrationNo"].ToString();
                    licenseHQ.HistorySerialNo = dr["SerialNo"].ToString();
                    licenseHQ.HistoryIssuedDT = dr["IssuedDT"].ToString();
                    licenseHQ.HistoryExpiredDT = dr["ExpiredDT"].ToString();
                    licenseHQ.HistoryPIC1Name = dr["PIC1Name"].ToString();
                    licenseHQ.HistoryPIC2Name = dr["PIC2Name"].ToString();
                    licenseHQ.HistoryPIC3Name = dr["PIC3Name"].ToString();
                    licenseHQ.HistoryisRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseHQ.HistoryisApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseHQ.HistoryisRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseHQ.HistoryisRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseHQ.HistoryisRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseHQ.HistoryRenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseHQ.HistoryhasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseHQList.Add(licenseHQ);

                    counter++;
                }

                conn.Close();
            }

            return licenseHQList;
        }
        #endregion

        #region CHECK DUPLICATE LICENSE HQ
        //Check Duplication in Request new license
        public LicenseHQ CheckLicenseByName(string LicenseName)
        {
            var licenseHQ = new LicenseHQ();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseName", LicenseName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseHQ.ExistData = Convert.ToInt32(dr["ExistData"].ToString());
                }

                conn.Close();
            }

            return licenseHQ;
        }
        #endregion
        #endregion

        #region LICENSE ADMIN
        #region GRIDVIEW
        public IEnumerable<LicenseAdmin> LicenseAdminGetAll()
        {
            var counter = 1;

            var licenseAdminList = new List<LicenseAdmin>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseAdminGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseAdmin = new LicenseAdmin();

                    licenseAdmin.IndexNo = counter;

                    licenseAdmin.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseAdmin.LicenseName = dr["LicenseName"].ToString();
                    licenseAdmin.CategoryName = dr["CategoryName"].ToString();
                    licenseAdmin.UnitName = dr["UnitName"].ToString();
                    licenseAdmin.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseAdmin.IssuedDT = dr["IssuedDT"].ToString();
                    licenseAdmin.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseAdmin.PIC1Name = dr["PIC1Name"].ToString();
                    licenseAdmin.PIC2Name = dr["PIC2Name"].ToString();
                    licenseAdmin.PIC3Name = dr["PIC3Name"].ToString();
                    licenseAdmin.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseAdmin.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseAdmin.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseAdmin.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseAdmin.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseAdmin.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseAdmin.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseAdmin.UserType = dr["UserType"].ToString();

                    licenseAdminList.Add(licenseAdmin);

                    counter++;
                }

                conn.Close();
            }

            return licenseAdminList;
        }
        #endregion

        #region EDIT LICENSE HQ (REQUEST/APPROVE)
        //License Request
        public void EditLicenseHQRequest(LicenseAdmin LicenseAdmin, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQReqEdit", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", LicenseAdmin.LicenseID);
                cmd.Parameters.AddWithValue("CategoryID", LicenseAdmin.CategoryID);
                cmd.Parameters.AddWithValue("DivID", LicenseAdmin.DivID);
                cmd.Parameters.AddWithValue("UnitID", LicenseAdmin.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", LicenseAdmin.LicenseName);
                cmd.Parameters.AddWithValue("PIC1Name", LicenseAdmin.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", LicenseAdmin.PIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", LicenseAdmin.PIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", LicenseAdmin.Remarks);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region EDIT LICENSE HQ (REGISTER/RENEW)
        //License Request
        public void EditLicenseHQRegister(LicenseAdmin LicenseAdmin, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQRegEdit", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", LicenseAdmin.LicenseID);
                cmd.Parameters.AddWithValue("CategoryID", LicenseAdmin.CategoryID);
                cmd.Parameters.AddWithValue("DivID", LicenseAdmin.DivID);
                cmd.Parameters.AddWithValue("UnitID", LicenseAdmin.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", LicenseAdmin.LicenseName);
                cmd.Parameters.AddWithValue("PIC1Name", LicenseAdmin.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", LicenseAdmin.PIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", LicenseAdmin.PIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", LicenseAdmin.Remarks);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region GET LICENSE BY ID
        //Get License HQ By ID
        public LicenseAdmin GetLicenseAdminByID(int? id)
        {
            var licenseAdmin = new LicenseAdmin();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseAdminGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseAdmin.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseAdmin.OldLicenseName = dr["LicenseName"].ToString();

                    licenseAdmin.LicenseName = dr["LicenseName"].ToString();
                    licenseAdmin.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                    licenseAdmin.CategoryName = dr["CategoryName"].ToString();
                    licenseAdmin.DivID = Convert.ToInt32(dr["DivID"].ToString());
                    licenseAdmin.DivName = dr["DivName"].ToString();
                    licenseAdmin.UnitID = Convert.ToInt32(dr["UnitID"].ToString());
                    licenseAdmin.UnitName = dr["UnitName"].ToString();

                    licenseAdmin.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseAdmin.SerialNo = dr["SerialNo"].ToString();

                    licenseAdmin.IssuedDT = dr["IssuedDT"].ToString();
                    licenseAdmin.ExpiredDT = dr["ExpiredDT"].ToString();

                    licenseAdmin.PIC1StaffNo = dr["PIC1StaffNo"].ToString();
                    licenseAdmin.PIC1Name = dr["PIC1Name"].ToString();
                    licenseAdmin.PIC1Email = dr["PIC1Email"].ToString();
                    licenseAdmin.PIC2StaffNo = dr["PIC2StaffNo"].ToString();
                    licenseAdmin.PIC2Name = dr["PIC2Name"].ToString();
                    licenseAdmin.PIC2Email = dr["PIC2Email"].ToString();
                    licenseAdmin.PIC3StaffNo = dr["PIC3StaffNo"].ToString();
                    licenseAdmin.PIC3Name = dr["PIC3Name"].ToString();
                    licenseAdmin.PIC3Email = dr["PIC3Email"].ToString();

                    licenseAdmin.UserType = dr["UserType"].ToString();

                    licenseAdmin.Remarks = dr["Remarks"].ToString();

                    licenseAdmin.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseAdmin.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseAdmin.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseAdmin.isRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseAdmin.isRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseAdmin.hasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseAdmin.LicenseFileName = dr["LicenseFileName"].ToString();

                    licenseAdmin.RenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseAdmin.RejectionRemarks = dr["RejectRemarks"].ToString();
                }

                conn.Close();
            }

            return licenseAdmin;
        }
        #endregion

        #region CHECK DUPLICATE LICENSE HQ NAME
        public LicenseAdmin CheckLicenseHQByName(string LicenseName)
        {
            var licenseAdmin = new LicenseAdmin();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseName", LicenseName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseAdmin.ExistData = Convert.ToInt32(dr["ExistData"].ToString());
                }

                conn.Close();
            }

            return licenseAdmin;
        }
        #endregion

        #region REGISTER LICENSE HQ
        public void RegisterLicenseHQ(LicenseAdmin licenseAdmin, string Issued, string Expired, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQRegister", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", licenseAdmin.LicenseID);
                cmd.Parameters.AddWithValue("CategoryID", licenseAdmin.CategoryID);
                cmd.Parameters.AddWithValue("DivID", licenseAdmin.DivID);
                cmd.Parameters.AddWithValue("UnitID", licenseAdmin.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", licenseAdmin.LicenseName);
                cmd.Parameters.AddWithValue("RegistrationNo", licenseAdmin.RegistrationNo);
                cmd.Parameters.AddWithValue("SerialNo", licenseAdmin.SerialNo);
                cmd.Parameters.AddWithValue("IssuedDT", Issued);
                cmd.Parameters.AddWithValue("ExpiredDT", Expired);
                cmd.Parameters.AddWithValue("PIC1Name", licenseAdmin.PIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", licenseAdmin.PIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", licenseAdmin.PIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", licenseAdmin.Remarks);

                //License File
                cmd.Parameters.AddWithValue("LicenseFileName", licenseAdmin.LicenseFileName);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region HISTORY GRIDVIEW
        public IEnumerable<LicenseAdmin> LicenseAdminGetLog(string LicenseName)
        {
            var counter = 1;

            var licenseAdminList = new List<LicenseAdmin>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseAdminGetLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("LicenseName", LicenseName);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseAdmin = new LicenseAdmin();

                    licenseAdmin.IndexNo = counter;

                    licenseAdmin.HistoryLicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseAdmin.HistoryLicenseName = dr["LicenseName"].ToString();
                    licenseAdmin.HistoryRegistrationNo = dr["RegistrationNo"].ToString();
                    licenseAdmin.HistorySerialNo = dr["SerialNo"].ToString();
                    licenseAdmin.HistoryIssuedDT = dr["IssuedDT"].ToString();
                    licenseAdmin.HistoryExpiredDT = dr["ExpiredDT"].ToString();
                    licenseAdmin.HistoryPIC1Name = dr["PIC1Name"].ToString();
                    licenseAdmin.HistoryPIC2Name = dr["PIC2Name"].ToString();
                    licenseAdmin.HistoryPIC3Name = dr["PIC3Name"].ToString();
                    licenseAdmin.HistoryisRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseAdmin.HistoryisApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseAdmin.HistoryisRejected = Convert.ToBoolean(dr["isRejected"].ToString());
                    licenseAdmin.HistoryisRegistered = Convert.ToBoolean(dr["isRegistered"].ToString());
                    licenseAdmin.HistoryisRenewed = Convert.ToBoolean(dr["isRenewed"].ToString());

                    licenseAdmin.HistoryRenewReminderDT = Convert.ToDateTime(dr["RenewReminderDT"].ToString());

                    licenseAdmin.HistoryhasFile = Convert.ToBoolean(dr["hasFile"].ToString());

                    licenseAdminList.Add(licenseAdmin);

                    counter++;
                }

                conn.Close();
            }

            return licenseAdminList;
        }
        #endregion

        #region RENEWAL LICENSE HQ
        public void RenewalLicenseHQ(RenewalLicenseHQViewModel licenseAdmin, string Issued, string Expired, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQRenewal", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("CategoryID", licenseAdmin.RenewalLicense.CategoryID);
                cmd.Parameters.AddWithValue("DivID", licenseAdmin.RenewalLicense.DivID);
                cmd.Parameters.AddWithValue("UnitID", licenseAdmin.RenewalLicense.UnitID);
                cmd.Parameters.AddWithValue("LicenseName", licenseAdmin.RenewalLicense.LicenseName);
                cmd.Parameters.AddWithValue("RegistrationNo", licenseAdmin.RenewalLicense.NewRegistrationNo);
                cmd.Parameters.AddWithValue("SerialNo", licenseAdmin.RenewalLicense.NewSerialNo);
                cmd.Parameters.AddWithValue("IssuedDT", Issued);
                cmd.Parameters.AddWithValue("ExpiredDT", Expired);
                cmd.Parameters.AddWithValue("PIC1Name", licenseAdmin.RenewalLicense.NewPIC1Name);
                cmd.Parameters.AddWithValue("PIC2StaffNo", licenseAdmin.RenewalLicense.NewPIC2StaffNo);
                cmd.Parameters.AddWithValue("PIC3StaffNo", licenseAdmin.RenewalLicense.NewPIC3StaffNo);
                cmd.Parameters.AddWithValue("Remarks", licenseAdmin.RenewalLicense.NewRemarks);

                //License File
                cmd.Parameters.AddWithValue("LicenseFileName", licenseAdmin.RenewalLicense.NewLicenseFileName);

                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
        #endregion

        #region LICENSE APPROVAL
        #region GRIDVIEW
        public IEnumerable<LicenseApproval> LicenseApprovalGetAll()
        {
            var counter = 1;

            var licenseApprovalList = new List<LicenseApproval>();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseApprovalGetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var licenseApproval = new LicenseApproval();

                    licenseApproval.indexNo = counter;

                    licenseApproval.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseApproval.LicenseName = dr["LicenseName"].ToString();
                    licenseApproval.CategoryName = dr["CategoryName"].ToString();
                    licenseApproval.UnitName = dr["UnitName"].ToString();
                    licenseApproval.IssuedDT = dr["IssuedDT"].ToString();
                    licenseApproval.ExpiredDT = dr["ExpiredDT"].ToString();
                    licenseApproval.PIC1Name = dr["PIC1Name"].ToString();
                    licenseApproval.PIC2Name = dr["PIC2Name"].ToString();
                    licenseApproval.PIC3Name = dr["PIC3Name"].ToString();
                    licenseApproval.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseApproval.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseApproval.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());

                    licenseApprovalList.Add(licenseApproval);

                    counter++;
                }

                conn.Close();
            }

            return licenseApprovalList;
        }
        #endregion

        #region GET LICENSE BY ID
        //Get License HQ By ID
        public LicenseApproval GetLicenseApprovalByID(int? id)
        {
            var licenseApproval = new LicenseApproval();

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseApprovalGetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    licenseApproval.LicenseID = Convert.ToInt32(dr["LicenseID"].ToString());
                    licenseApproval.LicenseName = dr["LicenseName"].ToString();
                    licenseApproval.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                    licenseApproval.CategoryName = dr["CategoryName"].ToString();
                    licenseApproval.DivID = Convert.ToInt32(dr["DivID"].ToString());
                    licenseApproval.DivName = dr["DivName"].ToString();
                    licenseApproval.UnitID = Convert.ToInt32(dr["UnitID"].ToString());
                    licenseApproval.UnitName = dr["UnitName"].ToString();

                    licenseApproval.RegistrationNo = dr["RegistrationNo"].ToString();
                    licenseApproval.SerialNo = dr["SerialNo"].ToString();

                    licenseApproval.PIC1StaffNo = dr["PIC1StaffNo"].ToString();
                    licenseApproval.PIC1Name = dr["PIC1Name"].ToString();
                    licenseApproval.PIC1Email = dr["PIC1Email"].ToString();

                    licenseApproval.PIC2StaffNo = dr["PIC2StaffNo"].ToString();
                    licenseApproval.PIC2Name = dr["PIC2Name"].ToString();
                    licenseApproval.PIC2Email = dr["PIC2Email"].ToString();
                    licenseApproval.PIC3StaffNo = dr["PIC3StaffNo"].ToString();
                    licenseApproval.PIC3Name = dr["PIC3Name"].ToString();
                    licenseApproval.PIC3Email = dr["PIC3Email"].ToString();
                    licenseApproval.Remarks = dr["Remarks"].ToString();

                    licenseApproval.isRequested = Convert.ToBoolean(dr["isRequested"].ToString());
                    licenseApproval.isApproved = Convert.ToBoolean(dr["isApproved"].ToString());
                    licenseApproval.isRejected = Convert.ToBoolean(dr["isRejected"].ToString());

                    licenseApproval.RejectionRemarks = dr["RejectRemarks"].ToString();
                }

                conn.Close();
            }

            return licenseApproval;
        }
        #endregion

        #region CONFIRM APPROVE
        public void ApproveLicense(int Id, string UserName)
        {

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQApprove", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", Id);
                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region CONFIRM REJECT
        public void RejectLicense(int Id, string Remarks, string UserName)
        {

            using (SqlConnection conn = new SqlConnection(Startup.connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLicenseHQReject", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("LicenseID", Id);
                cmd.Parameters.AddWithValue("Remarks", Remarks);
                cmd.Parameters.AddWithValue("UserName", UserName);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
        #endregion
    }
}
