using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsDTCTracker
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsTcTracker";
        public string sDTCCode { get; set; }
        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public string sDTCName { get; set; }
        public string sDTRCode { get; set; }
        public string sConnectedLoad { get; set; }
        public DataTable dTracker { get; set; }
        public string sTaskType { get; set; }

        OleDbCommand oledbCommand;
        public object GetDTCTrackstatus(clsDTCTracker objTracker)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oledbCommand = new OleDbCommand();
                strQry = "SELECT '''' ||  FBD_OLD_DTC_CODE ||''''|| ',' || '''' || FBD_NEW_DTC_CODE || '''' AS DTCCODE FROM TBLFEEDER_BIFURCATION_DETAILS WHERE FBD_OLD_DTC_CODE = :sDTCCode2 OR FBD_NEW_DTC_CODE = :sDTCCode3 ";
                oledbCommand.Parameters.AddWithValue("sDTCCode2", objTracker.sDTCCode.ToUpper());
                oledbCommand.Parameters.AddWithValue("sDTCCode3", objTracker.sDTCCode.ToUpper());

                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if(dt.Rows.Count > 0)
                {
                    objTracker.sDTCCode = Convert.ToString(dt.Rows[0]["DTCCODE"]);
                }
                else
                {
                    objTracker.sDTCCode = "'" + objTracker.sDTCCode + "'";
                }

                oledbCommand = new OleDbCommand();
               
                strQry = " SELECT DT_CODE, DT_NAME, DT_TC_ID, DT_TOTAL_CON_KW FROM TBLDTCMAST WHERE DT_CODE in (" + objTracker.sDTCCode + ")";
               // oledbCommand.Parameters.AddWithValue("sDTCCode", objTracker.sDTCCode);
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objTracker.sConnectedLoad = Convert.ToString(dt.Rows[0]["DT_TOTAL_CON_KW"]);
                    objTracker.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                    objTracker.sDTRCode = Convert.ToString(dt.Rows[0]["DT_TC_ID"]);
                }

                // LOC_TYPE  1--> Store  2---> Field  3----> Repairer

                //DRT_ACT_REFTYPE   1---> New DTC Entry  2---> Failure  3-->After RI   4--> FeederBifurcation.

                oledbCommand = new OleDbCommand();
                strQry = "SELECT TO_CHAR(DCT_TRANS_DATE,'DD-MON-YYYY HH:MI AM') AS TRANSDATE, UPPER(DCT_DTR_CODE) DCT_DTR_CODE, DCT_DESC AS STATUS,DCT_ACT_REFTYPE,";
                strQry += " DCT_DTR_STATUS,DCT_ACT_REFNO,UPPER(DCT_DTC_CODE) DCT_DTC_CODE FROM TBLDTCTRANSACTION ";
                strQry += "  WHERE UPPER(DCT_DTC_CODE) in ("+ objTracker.sDTCCode + ")";

               // oledbCommand.Parameters.AddWithValue("sDTCCode1", objTracker.sDTCCode.ToUpper());

                if (objTracker.sFromDate.Length > 0)
                {

                    DateTime dFromDate = DateTime.ParseExact(objTracker.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(DCT_TRANS_DATE,'YYYYMMDD') >= '"+ dFromDate.ToString("yyyyMMdd") + "' ";
                //    oledbCommand.Parameters.AddWithValue("dFromDate", dFromDate.ToString("yyyyMMdd"));

                }
                if (objTracker.sToDate.Length > 0)
                {
                    DateTime dToDate = DateTime.ParseExact(objTracker.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(DCT_TRANS_DATE,'YYYYMMDD') <= '"+ dToDate.ToString("yyyyMMdd") + "' ";
                   // oledbCommand.Parameters.AddWithValue("dToDate", dToDate.ToString("yyyyMMdd"));
                }

                //Failure
                if (objTracker.sTaskType != null )
                {
                    strQry += " AND DCT_ACT_REFTYPE = "+ objTracker.sTaskType + " ";
                    //oledbCommand.Parameters.AddWithValue("sTaskType", objTracker.sTaskType);
                }

                strQry += " ORDER BY DCT_ID DESC";
                dt = ObjCon.getDataTable(strQry);
                objTracker.dTracker = dt;
                return objTracker;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetDTCTrackstatus");
                return objTracker;

            }
            finally
            {
                
            }
        }


        public string GetDTCIdFromCode(string sDTCCode)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT DT_ID FROM TBLDTCMAST WHERE DT_CODE=:sDTCCode";
                oledbCommand.Parameters.AddWithValue("sDTCCode", sDTCCode);
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCIdFromCode");
                return ex.Message;

            }
            finally
            {
                
            }
        }
    }
}
