using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsDTrAllocation
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsDTrAllocation";

        public string sCrby { get; set; }
        public string sDTrCode { get; set; }
        public string sDTCCode { get; set; }
        public string sOfficeCode { get; set; }
        public string sStoreId { get; set; }
        public string sUserName { get; set; }
        public string sStoreName { get; set; }
        public string sNewDTCCode { get; set; }

        public string sFirstDTrCode { get; set; }
        public string sSecondDTrCode { get; set; }
        public string sFirstDTCCode { get; set; }
        public string sSecondDTCCode { get; set; }


        public string sCapacity { get; set; }
        public string sMakeName { get; set; }
        public string sTcSlNo { get; set; }
        OleDbCommand oledbCommand;
        public class example : clsDTrAllocation
        {
            public string sTrail { get; set; }

        }


        public DataTable LoadDTrDetails(string sDTrCode)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;
                example objExam = new example();

                strQry = "SELECT TC_CODE,TC_SLNO,TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY,CASE TC_CURRENT_LOCATION WHEN 1 THEN 'STORE' WHEN 2 THEN 'FIELD' WHEN 2 THEN 'REPAIRER'";
                strQry += "  END || ' ; ' || OFF_NAME AS CURRENT_LOCATION,";
                strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_TC_ID=TC_CODE) DTC_NAME,(SELECT DT_CODE FROM TBLDTCMAST WHERE DT_TC_ID=TC_CODE) DTC_CODE";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,VIEW_ALL_OFFICES  WHERE  TC_LOCATION_ID=OFF_CODE";
                strQry += " AND TC_MAKE_ID= TM_ID AND TC_CODE=:sDTrCode";
                oledbCommand.Parameters.AddWithValue("sDTrCode", sDTrCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDTrDetails");
                return dt;
            }
            finally
            {

            }
        }

        public string[] DTrStoreAllocation(clsDTrAllocation objAllocation)
        {
            string[] Arr = new string[2];
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                string sDesc = string.Empty;
                string sDTrId = string.Empty;
                oledbCommand.Parameters.AddWithValue("sDTrCode", objAllocation.sDTrCode);
                sDTrId = ObjCon.get_value("SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=:sDTrCode", oledbCommand);

                ObjCon.BeginTrans();

                if (objAllocation.sDTCCode != "")
                {

                    strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG= '0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objAllocation.sCrby + "',";
                    strQry += " TM_UNMAP_REASON= 'UNMAP BY DTR Allocation' WHERE TM_TC_ID='" + objAllocation.sDTrCode + "'";
                    strQry += " AND TM_LIVE_FLAG='1' AND TM_DTC_ID  ='" + objAllocation.sDTCCode + "'";
                    ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='0' WHERE DT_CODE='" + objAllocation.sDTCCode + "'";
                    ObjCon.Execute(strQry);

                    sDesc = "DTR DEALLOCATED FROM DTC CODE " + objAllocation.sDTCCode + " BY " + objAllocation.sUserName + " AND MOVED TO STORE " + objAllocation.sStoreName;
                }
                else
                {
                    sDesc = "DTR MOVED TO STORE " + objAllocation.sStoreName + " BY " + objAllocation.sUserName;
                }

                strQry = "UPDATE TBLTCMASTER SET TC_UPDATED_EVENT='FROM DTR ALLOCATION',";
                strQry += " TC_CURRENT_LOCATION=1,TC_LOCATION_ID='" + objAllocation.sOfficeCode + "' WHERE ";
                strQry += " TC_CODE='" + objAllocation.sDTrCode + "'";
                ObjCon.Execute(strQry);

                long sMaxNo = ObjCon.Get_max_no("DRT_ID", "TBLDTRTRANSACTION");

                strQry = "INSERT INTO TBLDTRTRANSACTION (DRT_ID,DRT_DTR_CODE,DRT_LOC_ID,DRT_LOC_TYPE,DRT_TRANS_DATE, ";
                strQry += " DRT_ACT_REFNO,DRT_ACT_REFTYPE,DRT_DESC,DRT_DTR_STATUS ) VALUES ('" + sMaxNo + "','" + objAllocation.sDTrCode + "',";
                strQry += " '" + objAllocation.sStoreId + "','1',SYSDATE,'" + sDTrId + "','9','" + sDesc + "','1')";
                ObjCon.Execute(strQry);

                ObjCon.CommitTrans();

                Arr[0] = "Allocated Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "StoreAllocation");
                return Arr;

            }
            finally
            {

            }
        }


        public string[] DTrFieldAllocation(clsDTrAllocation objAllocation)
        {
            string[] Arr = new string[2];
            try
            {

                string strQry = string.Empty;
                string sDesc = string.Empty;
                string sDTCOffCode = string.Empty;
                string sResult = string.Empty;

                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sNewDTCCode", objAllocation.sNewDTCCode.ToUpper());
                sDTCOffCode = ObjCon.get_value("SELECT DT_OM_SLNO FROM TBLDTCMAST WHERE UPPER(DT_CODE)=:sNewDTCCode", oledbCommand);

                if (sDTCOffCode == "")
                {
                    Arr[0] = "Please enter Valid DTC Code";
                    Arr[1] = "2";
                    return Arr;
                }

                oledbCommand = new OleDbCommand();
                strQry = "SELECT TM_ID FROM TBLTRANSDTCMAPPING WHERE UPPER(TM_DTC_ID)=:sNewDTCCode1 AND TM_LIVE_FLAG='1'";
                oledbCommand.Parameters.AddWithValue("sNewDTCCode1", objAllocation.sNewDTCCode.ToUpper());
                sResult = ObjCon.get_value(strQry, oledbCommand);
                if (sResult != "")
                {
                    Arr[0] = "Already DTR Exists for Selected DTC Code " + objAllocation.sNewDTCCode;
                    Arr[1] = "2";
                    return Arr;

                }

                ObjCon.BeginTrans();

                strQry = "UPDATE TBLTCMASTER SET TC_UPDATED_EVENT='FROM DTR ALLOCATION',";
                strQry += " TC_CURRENT_LOCATION=2,TC_LOCATION_ID='" + sDTCOffCode + "' WHERE ";
                strQry += " TC_CODE='" + objAllocation.sDTrCode + "'";
                ObjCon.Execute(strQry);


                //Map the New TC for DTC
                strQry = "INSERT INTO TBLTRANSDTCMAPPING(TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON)";
                strQry += " VALUES('" + ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING") + "', SYSDATE,'" + objAllocation.sDTrCode + "',";
                strQry += " '" + objAllocation.sNewDTCCode + "','1','" + objAllocation.sCrby + "',SYSDATE)";
                ObjCon.Execute(strQry);


                // Update in DTC Table
                strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objAllocation.sDTrCode + "' WHERE DT_CODE='" + objAllocation.sNewDTCCode + "'";
                ObjCon.Execute(strQry);


                if (objAllocation.sDTCCode != "")
                {
                    strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='0' WHERE DT_CODE='" + objAllocation.sDTCCode + "'";
                    ObjCon.Execute(strQry);
                }


                ObjCon.CommitTrans();

                Arr[0] = "Allocated Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DTrFieldAllocation");
                return Arr;

            }
            finally
            {

            }
        }


        public object GetDTrDetails(clsDTrAllocation objAllocation)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = " SELECT TC_CODE,TC_SLNO,TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                strQry += " (SELECT DT_CODE FROM TBLDTCMAST WHERE DT_TC_ID=TC_CODE) DTC_CODE";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,VIEW_ALL_OFFICES  WHERE  TC_LOCATION_ID=OFF_CODE";
                strQry += "  AND TC_MAKE_ID= TM_ID AND TC_CODE=:sFirstDTrCode";
                oledbCommand.Parameters.AddWithValue("sFirstDTrCode", objAllocation.sFirstDTrCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);

                if (dt.Rows.Count > 0)
                {
                    objAllocation.sFirstDTCCode = dt.Rows[0]["DTC_CODE"].ToString();
                    objAllocation.sFirstDTrCode = dt.Rows[0]["TC_CODE"].ToString();
                    objAllocation.sTcSlNo = dt.Rows[0]["TC_SLNO"].ToString();
                    objAllocation.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objAllocation.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();

                }
                return objAllocation;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getDTrDetails");
                return dt;
            }
            finally
            {

            }
        }

        public string[] DTrAllocation(clsDTrAllocation objAllocation)
        {
            string[] Arr = new string[2];
            try
            {

                DataTable dt = new DataTable();
                string strQry = string.Empty;

                ObjCon.BeginTrans();

                // Updating or Interchanging the TC(DT_TC_ID) with DTC(DT_CODE)

                strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objAllocation.sSecondDTrCode + "' WHERE DT_CODE='" + objAllocation.sFirstDTCCode + "'";
                ObjCon.Execute(strQry);

                strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objAllocation.sFirstDTrCode + "' WHERE DT_CODE='" + objAllocation.sSecondDTCCode + "'";
                ObjCon.Execute(strQry);

                // Updating LiveFlag to '0' to the table TBLTRANSDTCMAPPING

                strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0' WHERE TM_TC_ID='" + objAllocation.sFirstDTrCode + "' AND";
                strQry += " TM_DTC_ID='" + objAllocation.sFirstDTCCode + "' AND TM_LIVE_FLAG='1'";
                ObjCon.Execute(strQry);

                strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0' WHERE TM_TC_ID='" + objAllocation.sSecondDTrCode + "' AND";
                strQry += " TM_DTC_ID='" + objAllocation.sSecondDTCCode + "' AND TM_LIVE_FLAG='1'";
                ObjCon.Execute(strQry);

                /// Insert to DTR Transaction Table

                string sDTrId = string.Empty;
                string sDesc = string.Empty;
                string sDTCLocCode = string.Empty;

                //oledbCommand = new OleDbCommand();
                ///First DTR Code
                //oledbCommand.Parameters.AddWithValue("sFirstDTrCode", objAllocation.sFirstDTrCode);
                //sDTrId = ObjCon.get_value("SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=:sFirstDTrCode", oledbCommand);
                sDTrId = ObjCon.get_value("SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE='" + objAllocation.sFirstDTrCode + "'");
                sDesc = "DTR REALLOCATED FROM DTC CODE " + objAllocation.sFirstDTCCode + " TO " + objAllocation.sSecondDTCCode + " BY " + objAllocation.sUserName + "";

                //oledbCommand = new OleDbCommand();
                //oledbCommand.Parameters.AddWithValue("sFirstDTCCode", sFirstDTCCode);
                //sDTCLocCode = ObjCon.get_value("SELECT DT_OM_SLNO FROM TBLDTCMAST WHERE DT_CODE =:sFirstDTCCode", oledbCommand);

                sDTCLocCode = ObjCon.get_value("SELECT DT_OM_SLNO FROM TBLDTCMAST WHERE DT_CODE ='" + sFirstDTCCode + "'");
                long sMaxNo = ObjCon.Get_max_no("DRT_ID", "TBLDTRTRANSACTION");

                strQry = "INSERT INTO TBLDTRTRANSACTION (DRT_ID,DRT_DTR_CODE,DRT_LOC_ID,DRT_LOC_TYPE,DRT_TRANS_DATE, ";
                strQry += " DRT_ACT_REFNO,DRT_ACT_REFTYPE,DRT_DESC,DRT_DTR_STATUS ) VALUES ('" + sMaxNo + "','" + objAllocation.sFirstDTrCode + "',";
                strQry += " '" + sDTCLocCode + "','2',SYSDATE,'" + sDTrId + "','9','" + sDesc + "','1')";
                ObjCon.Execute(strQry);


                ///Second DTR Code
                /// oledbCommand = new OleDbCommand();
                //oledbCommand.Parameters.AddWithValue("sSecondDTrCode", objAllocation.sSecondDTrCode);
                //sDTrId = ObjCon.get_value("SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=:sSecondDTrCode", oledbCommand);
                sDTrId = ObjCon.get_value("SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE='"+ objAllocation.sSecondDTrCode + "'");

                sDesc = "DTR REALLOCATED FROM DTC CODE " + objAllocation.sSecondDTCCode + " TO " + objAllocation.sFirstDTCCode + " BY " + objAllocation.sUserName + "";
                sDTCLocCode = ObjCon.get_value("SELECT DT_OM_SLNO FROM TBLDTCMAST WHERE DT_CODE = '" + sSecondDTCCode + "'");

                sMaxNo = ObjCon.Get_max_no("DRT_ID", "TBLDTRTRANSACTION");

                strQry = "INSERT INTO TBLDTRTRANSACTION (DRT_ID,DRT_DTR_CODE,DRT_LOC_ID,DRT_LOC_TYPE,DRT_TRANS_DATE, ";
                strQry += " DRT_ACT_REFNO,DRT_ACT_REFTYPE,DRT_DESC,DRT_DTR_STATUS ) VALUES ('" + sMaxNo + "','" + objAllocation.sSecondDTrCode + "',";
                strQry += " '" + sDTCLocCode + "','2',SYSDATE,'" + sDTrId + "','9','" + sDesc + "','1')";
                ObjCon.Execute(strQry);


                // Inserting New Row with LiveFlag as '1' to the table TBLTRANSDTCMAPPING

                sMaxNo = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON)";
                strQry += " VALUES ('" + sMaxNo + "',sysdate,'" + objAllocation.sFirstDTrCode + "','" + objAllocation.sSecondDTCCode + "','1','" + objAllocation.sCrby + "',sysdate)";
                ObjCon.Execute(strQry);

                sMaxNo = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON)";
                strQry += " VALUES ('" + sMaxNo + "',sysdate,'" + objAllocation.sSecondDTrCode + "','" + objAllocation.sFirstDTCCode + "','1','" + objAllocation.sCrby + "',sysdate)";
                ObjCon.Execute(strQry);

                ObjCon.CommitTrans();

                Arr[0] = "DTr Allocated Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DTrAllocation");
                return Arr;

            }
            finally
            {

            }

        }
    }
}
