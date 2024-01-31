using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.Diagnostics;
namespace IIITS.DTLMS.BL
{
    public class clsDtcDetailsApprove
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string OfficeCode { get; set; }
        public string Status { get; set; }

        OleDbCommand oledbCommand;
        /// <summary>
        /// this method used to load the Dtc Details for LT Approve
        /// </summary>
        /// <param name="objDtcdetails"></param>
        /// <returns></returns>
        public DataTable LoadApprovePendingDtcDetails(clsDtcDetailsApprove objDtcdetails)
        {
            oledbCommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {
                string Qry = "SELECT DT_ID,DT_CODE,DT_NAME,DT_CRON,DT_TC_ID,TO_CHAR(TC_CAPACITY)TC_CAPACITY, ";
                Qry += " (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_ID = DT_DTCMETERS AND MD_TYPE='MTR') as METERS_AVAILABLE, ";
                Qry += " (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_ID = DT_METER_STATUS AND MD_TYPE='MTRS') ";
                Qry += " as METERS_STATUS,(SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_ID = DT_CT_RATIO ";
                Qry += " AND MD_TYPE='CTR')CT_RATIO,(SELECT MD_NAME FROM TBLMASTERDATA WHERE ";
                Qry += " MD_ID = DT_WIRING AND MD_TYPE='WIR')WIRING,(SELECT MD_NAME FROM TBLMASTERDATA ";
                Qry += " WHERE MD_ID = DT_MODEM AND MD_TYPE='MODEM')MODEM, ";
                Qry += " (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_ID = DT_METER_MAKE  AND MD_TYPE='MAKE')METER_MAKE, ";
                Qry += " (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_ID = DT_METER_RECORDING  AND MD_TYPE='MRECORDING')METER_RECORDING, ";
                Qry += " DT_DTCMETERS,DT_METER_STATUS, DT_CT_RATIO,DT_WIRING, DT_MODEM,DT_LTSTATUS, ";
                Qry += " DT_METER_MAKE,DT_METER_SLNO,DT_METER_RECORDING,DT_METER_REMARKS,DT_MTR_MANUFACTURE_YR, ";
                Qry += " (CASE WHEN DT_LTSTATUS='1' then 'APPROVED' ELSE 'PENDING'END)  APPROVE_STATUS ";
                Qry += " FROM TBLDTCMAST inner join TBLTCMASTER on DT_TC_ID = TC_CODE  ";
                Qry += " where DT_LTSTATUS IN(" + objDtcdetails.Status + ")  AND DT_OM_SLNO LIKE:OfficeCode||'%'";

                oledbCommand.Parameters.AddWithValue("OfficeCode", objDtcdetails.OfficeCode);

                dtDetails = ObjCon.getDataTable(Qry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;

            }
        }

    }
}




