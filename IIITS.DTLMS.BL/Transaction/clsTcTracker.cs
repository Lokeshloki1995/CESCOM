using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
namespace IIITS.DTLMS.BL
{
    public class clsTcTracker
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsTcTracker";
        public string sTCCode { get; set; }
        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public string sTCSlno { get; set; }
        public string sCapacity { get; set; }
        public string sMake { get; set; }
        public string sFailureType { get; set; }
        public string sTaskType { get; set; }
        public DataTable dTracker { get; set; }
        public string sRepairCount { get; set; }
        OleDbCommand oledbCommand;
        //public clsTcTracker GetTcTrackstatus(clsTcTracker objTracker)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;

        //        //  code for plate lost DTR .
        //        oledbCommand = new OleDbCommand();
        //        strQry = "SELECT LDP_OLD_DTR || ',' || LDP_NEW_DTR DTRCODES  FROM TBLDTRPLATELOST WHERE  LDP_OLD_DTR = :sTCCode OR LDP_NEW_DTR =:sTCCode1";
        //        oledbCommand.Parameters.AddWithValue("sTCCode", objTracker.sTCCode);
        //        oledbCommand.Parameters.AddWithValue("sTCCode1", objTracker.sTCCode);
        //        dt = ObjCon.getDataTable(strQry, oledbCommand);

        //        if (dt.Rows.Count > 0)
        //        {
        //            objTracker.sTCCode = Convert.ToString(dt.Rows[0]["DTRCODES"]);
        //        }


        //        strQry = " SELECT TC_CODE, TC_SLNO, TO_CHAR(TC_CAPACITY) TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID) ";
        //        strQry += " MAKE FROM TBLTCMASTER WHERE TC_LIVE_FLAG = 1 AND UPPER(TC_CODE) IN (" + objTracker.sTCCode.ToUpper() + ")";
        //        dt = ObjCon.getDataTable(strQry);
        //        if (dt.Rows.Count > 0)
        //        {
        //            objTracker.sTCSlno = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
        //            objTracker.sCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
        //            objTracker.sMake = Convert.ToString(dt.Rows[0]["MAKE"]);
        //        }

        //        strQry = "SELECT COUNT(*) FROM TBLDTRTRANSACTION WHERE DRT_DTR_CODE IN ("+ objTracker.sTCCode +") AND DRT_ACT_REFTYPE='5'";
        //        objTracker.sRepairCount = ObjCon.get_value(strQry);


        //        // LOC_TYPE  1--> Store  2---> Field  3----> Repairer

        //        //DRT_ACT_REFTYPE   1---> Against Purchase Order  2---> After RI  3-->Failure  4--->Sent To Repairer 
        //        //                  5----> Recieve from Repairer  6----> Scarp Entry   7---> Scrap Disposal    8------> Inspection
        //                       //   9-----> DTR Allocation from DTR Allocation Form  

        //        strQry = "SELECT TO_CHAR(DRT_TRANS_DATE,'DD-MON-YYYY HH:MI AM') AS TRANSDATE, UPPER(DRT_DTR_CODE) DRT_DTR_CODE, ";
        //        strQry += " CASE WHEN DRT_LOC_TYPE = 2 THEN (SELECT 'SECTION : ' || OM_NAME   FROM TBLOMSECMAST WHERE OM_CODE = DRT_LOC_ID) ";
        //        strQry += " WHEN  DRT_LOC_TYPE = 1 THEN (SELECT 'DIVISION : ' || (SELECT DIV_NAME FROM TBLDIVISION WHERE SM_OFF_CODE=DIV_CODE) || '; ' || 'STORE NAME : ' || SM_NAME FROM TBLSTOREMAST WHERE SM_ID = DRT_LOC_ID)  ";
        //        strQry += " WHEN  DRT_LOC_TYPE = 3 THEN (SELECT 'DIVISION : ' || (SELECT DIV_NAME FROM TBLDIVISION WHERE TR_OFFICECODE=DIV_CODE) || '; ' ||'REPAIRER NAME : ' || TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = DRT_LOC_ID)  ";
        //        strQry += " WHEN  DRT_LOC_TYPE = 4 THEN (SELECT 'SUPPLIER NAME : ' || TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID = DRT_LOC_ID)";
        //        strQry += " ELSE '' END AS LOCATION, DRT_DESC AS STATUS,DRT_ACT_REFTYPE,DRT_ACT_REFNO,DRT_DTR_STATUS FROM TBLDTRTRANSACTION WHERE DRT_DTR_CODE IN( " + objTracker.sTCCode + ")";

        //        if (objTracker.sFromDate.Length > 0)
        //        {
        //            DateTime dFromDate = DateTime.ParseExact(objTracker.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(DRT_TRANS_DATE,'YYYYMMDD') >=  '" + dFromDate.ToString("yyyyMMdd") + "'  ";                  
        //        }
        //        if (objTracker.sToDate.Length > 0)
        //        {
        //            DateTime dToDate = DateTime.ParseExact(objTracker.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(DRT_TRANS_DATE,'YYYYMMDD') <=  '" + dToDate.ToString("yyyyMMdd") + "' ";
        //        }

        //        //Failure
        //        if (objTracker.sTaskType != null && objTracker.sTaskType == "3")
        //        {
        //            strQry += " AND DRT_ACT_REFTYPE = '3' ";
        //        }
        //        // Commissioning
        //        else if (objTracker.sTaskType != null && objTracker.sTaskType == "1")
        //        {
        //            strQry += "  AND DRT_ACT_REFTYPE = '1' ";
        //        }
        //        // Dispatch Tc to repairer
        //        else if (objTracker.sTaskType != null && objTracker.sTaskType == "4")
        //        {
        //            strQry += "  AND DRT_ACT_REFTYPE = '4' ";
        //        }
        //        // Receive DTR from Repairer
        //        else if (objTracker.sTaskType != null && objTracker.sTaskType == "5")
        //        {
        //            strQry += "  AND DRT_ACT_REFTYPE = '5' ";
        //        }
        //        // De-Commissioning
        //        else if (objTracker.sTaskType != null && objTracker.sTaskType == "2")
        //        {
        //            strQry += " AND DRT_ACT_REFTYPE = '2' ";
        //        }

        //        strQry += "ORDER BY DRT_ID DESC";
        //        dt = ObjCon.getDataTable(strQry);
        //        objTracker.dTracker = dt;
        //        return objTracker;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTcTrackstatus");
        //        return objTracker;

        //    }
        //    finally
        //    {

        //    }
        //}

        public clsTcTracker GetTcTrackstatus(clsTcTracker objTracker)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //  code for plate lost DTR .
                oledbCommand = new OleDbCommand();
                strQry = "SELECT LDP_OLD_DTR || ',' || LDP_NEW_DTR DTRCODES  FROM TBLDTRPLATELOST WHERE  LDP_OLD_DTR = :sTCCode OR LDP_NEW_DTR =:sTCCode1";
                oledbCommand.Parameters.AddWithValue("sTCCode", objTracker.sTCCode);
                oledbCommand.Parameters.AddWithValue("sTCCode1", objTracker.sTCCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);

                if (dt.Rows.Count > 0)
                {
                    objTracker.sTCCode = Convert.ToString(dt.Rows[0]["DTRCODES"]);
                }


                strQry = " SELECT TC_CODE, TC_SLNO, TO_CHAR(TC_CAPACITY) TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID) ";
                strQry += " MAKE FROM TBLTCMASTER WHERE TC_LIVE_FLAG = 1 AND UPPER(TC_CODE) IN (" + objTracker.sTCCode.ToUpper() + ")";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objTracker.sTCSlno = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
                    objTracker.sCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                    objTracker.sMake = Convert.ToString(dt.Rows[0]["MAKE"]);
                }

                strQry = "SELECT COUNT(*) FROM TBLDTRTRANSACTION WHERE DRT_DTR_CODE IN (" + objTracker.sTCCode + ") AND DRT_ACT_REFTYPE='5'";
                objTracker.sRepairCount = ObjCon.get_value(strQry);


                // LOC_TYPE  1--> Store  2---> Field  3----> Repairer

                //DRT_ACT_REFTYPE   1---> Against Purchase Order  2---> After RI  3-->Failure  4--->Sent To Repairer 
                //                  5----> Recieve from Repairer  6----> Scarp Entry   7---> Scrap Disposal    8------> Inspection
                //   9-----> DTR Allocation from DTR Allocation Form  

                strQry = "SELECT TO_CHAR(DRT_TRANS_DATE,'DD-MON-YYYY') AS TRANSDATE,TO_CHAR(DRT_ENTRYDATE,'DD-MON-YYYY') AS DRT_ENTRYDATE, UPPER(DRT_DTR_CODE) DRT_DTR_CODE, ";
                strQry += " CASE WHEN DRT_LOC_TYPE = 2 THEN (SELECT 'SECTION : ' || OM_NAME   FROM TBLOMSECMAST WHERE OM_CODE = DRT_LOC_ID) ";
                strQry += " WHEN  DRT_LOC_TYPE = 1 THEN (SELECT 'DIVISION : ' || (SELECT DIV_NAME FROM TBLDIVISION WHERE SM_OFF_CODE=DIV_CODE) || '; ' || 'STORE NAME : ' || SM_NAME FROM TBLSTOREMAST WHERE SM_ID = DRT_LOC_ID)  ";
                strQry += " WHEN  DRT_LOC_TYPE = 3 THEN (SELECT 'DIVISION : ' || (SELECT DIV_NAME FROM TBLDIVISION WHERE TR_OFFICECODE=DIV_CODE) || '; ' ||'REPAIRER NAME : ' || TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = DRT_LOC_ID)  ";
                strQry += " WHEN  DRT_LOC_TYPE = 4 THEN (SELECT 'SUPPLIER NAME : ' || TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID = DRT_LOC_ID)";
                strQry += " ELSE '' END AS LOCATION, DRT_DESC AS STATUS,DRT_ACT_REFTYPE,DRT_ACT_REFNO,DRT_DTR_STATUS FROM TBLDTRTRANSACTION WHERE DRT_DTR_CODE IN( " + objTracker.sTCCode + ")";

                if (objTracker.sFromDate.Length > 0)
                {
                    DateTime dFromDate = DateTime.ParseExact(objTracker.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(DRT_TRANS_DATE,'YYYYMMDD') >=  '" + dFromDate.ToString("yyyyMMdd") + "'  ";
                }
                if (objTracker.sToDate.Length > 0)
                {
                    DateTime dToDate = DateTime.ParseExact(objTracker.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(DRT_TRANS_DATE,'YYYYMMDD') <=  '" + dToDate.ToString("yyyyMMdd") + "' ";
                }

                //Failure
                if (objTracker.sTaskType != null && objTracker.sTaskType == "3")
                {
                    strQry += " AND DRT_ACT_REFTYPE = '3' ";
                }
                // Commissioning
                else if (objTracker.sTaskType != null && objTracker.sTaskType == "1")
                {
                    strQry += "  AND DRT_ACT_REFTYPE = '1' ";
                }
                // Dispatch Tc to repairer
                else if (objTracker.sTaskType != null && objTracker.sTaskType == "4")
                {
                    strQry += "  AND DRT_ACT_REFTYPE = '4' ";
                }
                // Receive DTR from Repairer
                else if (objTracker.sTaskType != null && objTracker.sTaskType == "5")
                {
                    strQry += "  AND DRT_ACT_REFTYPE = '5' ";
                }
                // De-Commissioning
                else if (objTracker.sTaskType != null && objTracker.sTaskType == "2")
                {
                    strQry += " AND DRT_ACT_REFTYPE = '2' ";
                }

                strQry += "ORDER BY DRT_ID DESC";
                dt = ObjCon.getDataTable(strQry);
                objTracker.dTracker = dt;
                return objTracker;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTcTrackstatus");
                return objTracker;

            }
            finally
            {

            }
        }


        public string  GetDTCId(string sMappingId)
        {
            try
            {
                string strQry = string.Empty;
                oledbCommand = new OleDbCommand();
                strQry = "SELECT DT_ID FROM TBLTRANSDTCMAPPING,TBLDTCMAST WHERE TM_ID=:sMappingId AND TM_DTC_ID=DT_CODE";
                oledbCommand.Parameters.AddWithValue("sMappingId", sMappingId);
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTcTrackstatus");
                return ex.Message;

            }
            finally
            {
                
            }
        }

        public string GetTCIdFromCode(string sTCCode)
        {
            try
            {
                string strQry = string.Empty;
                oledbCommand = new OleDbCommand();
                strQry = "SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=:sTCCode";
                oledbCommand.Parameters.AddWithValue("sTCCode", sTCCode);
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTCIdFromCode");
                return ex.Message;

            }
            finally
            {
                
            }
        }
      
    }

}
