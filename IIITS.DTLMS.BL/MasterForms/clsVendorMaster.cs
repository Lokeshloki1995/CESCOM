using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsVendorMaster
    {
        string StrQry = string.Empty;
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string sVendorName { get; set; }
        public string sVendorNumber { get; set; }
        public string sVendorId { get; set; }


        OleDbCommand oledbcommand;
        public string sCrBy { get; set; }

        public string save_Vendor_Details(clsVendorMaster objVendor)
        {
            
            try
            {
                
                StrQry = "INSERT INTO TBLVENDORMASTER VALUES ((SELECT nvl(max(VM_ID),0)+1 FROM TBLVENDORMASTER),'"+ objVendor.sVendorName+ "','"+objVendor.sVendorNumber+"','')";
                ObjCon.Execute(StrQry);
                return "1";
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "0";
            }
            finally
            {
                
            }
        }

        public string update_Vendor_Details(clsVendorMaster objVendor)
        {
            try
            {

                StrQry = "UPDATE TBLVENDORMASTER SET VM_NAME=UPPER('" + objVendor.sVendorName + "'),VM_MOBILE_NUM='" + objVendor.sVendorNumber + "', ";
                StrQry += "  VM_UPDATED_ON = SYSDATE , VM_UPDATED_BY = '"+objVendor.sCrBy+"' ";
                StrQry += " WHERE VM_ID='"+ objVendor.sVendorId + "'";
                ObjCon.Execute(StrQry);
                return "1";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "0";
            }
            finally
            {
                
            }
        }

        public DataTable GetVendorDetails(string sVendorId)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtVendorDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("VmId", sVendorId);
                StrQry = "SELECT * FROM TBLVENDORMASTER WHERE VM_ID=:VmId";
                dtVendorDetails = ObjCon.getDataTable(StrQry, oledbcommand);
                return dtVendorDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtVendorDetails;
            }
            finally
            {
                
            }
        }

        public DataTable LoadVendorDetails(string sVendorName)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtVendorDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("VmName", sVendorName);
                StrQry = "SELECT * FROM TBLVENDORMASTER WHERE VM_NAME LIKE UPPER(:VmName||'%')";
                dtVendorDetails = ObjCon.getDataTable(StrQry, oledbcommand);
                return dtVendorDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtVendorDetails;
            }
            finally
            {
                
            }
        }

        public DataTable LoadVendorDetails()
        {
            DataTable dtVendorDetails = new DataTable();
            try
            {
                
                StrQry = "SELECT VM_ID,VM_NAME,VM_MOBILE_NUM FROM TBLVENDORMASTER ";
                dtVendorDetails = ObjCon.getDataTable(StrQry);
                return dtVendorDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtVendorDetails;
            }
            finally
            {
                
            }
        }
    }
}
