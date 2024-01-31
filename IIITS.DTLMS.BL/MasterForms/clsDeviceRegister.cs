using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsDeviceRegister
    {
        string strFormCode = "clsUser";
        public string sUserId { get; set; }
        public string sFullName { get; set; }
        public string sMuId { get; set; }
        public string sDeviceId { get; set; }
        public string sRequestedBy { get; set; }
        public string sApprovalStatus { get; set; }
        public string sApprovedBy { get; set; }
        public string sCrOn { get; set; }

        public string sMobileNumber { get; set; }
        public string officeCode { get; set; }


        /// <summary>
        /// //
        /// </summary>

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        OleDbCommand oledbcommand;

        public DataTable LoadDeviceGrid(clsDeviceRegister objdevice)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtUserDetails = new DataTable();

            string strQry = string.Empty;
            try
            {
                
                strQry = "select US_ID, (SELECT RO_NAME  FROM TBLROLES WHERE RO_ID = US_ROLE_ID )RO_NAME , US_OFFICE_CODE, MR_REQUEST_BY,MR_ID,MR_DEVICE_ID,US_FULL_NAME,US_MOBILE_NO, DECODE(MR_APPROVE_STATUS,'1','APPROVED','PENDING')MR_APPROVE_STATUS,TO_CHAR(MR_CRON,'DD-MON-YYYY')MR_CRON ";
                strQry += " from TBLMOBILEREGISTER,tbluser  where ";


                if (objdevice.sDeviceId != null && objdevice.sDeviceId != "")
                {
                    oledbcommand.Parameters.AddWithValue("DeviceId", objdevice.sDeviceId.ToUpper());
                    strQry += " UPPER(MR_DEVICE_ID) like :DeviceId||'%' and";
                }
                if (objdevice.sFullName != null && objdevice.sFullName != "")
                {
                    oledbcommand.Parameters.AddWithValue("DeviceName", objdevice.sFullName.ToUpper());
                    strQry += " UPPER(US_FULL_NAME) like :DeviceName||'%' and";
                }
                if (objdevice.sMobileNumber != null && objdevice.sMobileNumber != "")
                {
                    oledbcommand.Parameters.AddWithValue("PhoneNumber", objdevice.sMobileNumber);
                    strQry += " US_MOBILE_NO = :PhoneNumber and";
                }
                if (objdevice.officeCode != null && objdevice.officeCode != "")
                {
                    oledbcommand.Parameters.AddWithValue("OfficeCode", objdevice.officeCode);
                    strQry += " US_OFFICE_CODE = :OfficeCode and";
                }

                strQry += " US_ID=MR_REQUEST_BY  ORDER BY MR_ID desc ";
                dtUserDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtUserDetails;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadDeviceGrid");
                return dtUserDetails;
            }
            finally
            {
                
            }
        }

        public bool UpdateDeviceStatus(clsDeviceRegister objdevice)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                
                string strQry = string.Empty;
                DataTable dtUserDetails = new DataTable();
                oledbcommand.Parameters.AddWithValue("UsId", objdevice.sRequestedBy);
                strQry = "SELECT US_EFFECT_FROM,US_STATUS,US_FULL_NAME,US_MOBILE_NO FROM TBLUSER where  US_ID= :UsId and US_EFFECT_FROM > sysdate and US_STATUS= 'D' ";
                dtUserDetails = ObjCon.getDataTable(strQry, oledbcommand);
                string strSMS = string.Empty;
                string strPhone = string.Empty;
                string sUserName = string.Empty;
                if (dtUserDetails.Rows.Count > 0)
                {
                     strPhone = dtUserDetails.Rows[0]["US_MOBILE_NO"].ToString();
                     sUserName = dtUserDetails.Rows[0]["US_FULL_NAME"].ToString();
                     strSMS = "User Disabled";
                }
                else
                {
                    // strSMS = "User Enabled";
                 
                    strQry = "UPDATE TBLMOBILEREGISTER SET MR_APPROVE_STATUS= '" + objdevice.sApprovalStatus + "',MR_APPROVED_BY= '"+ objdevice.sApprovedBy + "'";
                    strQry += " ,MR_APPROVED_ON= SYSDATE where MR_ID = '"+ objdevice.sMuId + "'";
                    //oledbcommand.Parameters.AddWithValue("sApprovedBy", objdevice.sApprovedBy);
                    //oledbcommand.Parameters.AddWithValue("sMuId", objdevice.sMuId);
                    //oledbcommand.Parameters.AddWithValue("sApprovalStatus", objdevice.sApprovalStatus);
                    ObjCon.Execute(strQry);

                }

                clsCommunication objCommunication = new clsCommunication();

                //objCommunication.sendSMS(strSMS, strPhone, sUserName);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "UpdateDeviceStatus");
                return false;

            }
            finally
            {
                
            }
        }

        public string  DeleteDeviceStatus(clsDeviceRegister objDevice)
        {
            string status = string.Empty;
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT * FROM TBLMOBILEREGISTER WHERE MR_DEVICE_ID LIKE '@%' and  MR_ID = " + objDevice.sMuId + " ";
                string temp = ObjCon.get_value(strQry);
                if (temp != "")
                {
                    return status = "DEVICE HAS BEEN ALREADY DELETED";
                }
                strQry = " UPDATE TBLMOBILEREGISTER set MR_DEVICE_ID = '@'||MR_DEVICE_ID , MR_UPDATED_ON =  SYSDATE  ,  MR_UPDATED_BY  =  '"+objDevice.sApprovedBy + "'  WHERE MR_ID = " + objDevice.sMuId+" ";
                ObjCon.Execute(strQry);
                return status = "SUCCESS";

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateDeviceStatus");
                return status = "FAILURE";
            }


        }
    }
}
