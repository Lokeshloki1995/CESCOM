using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.Globalization;
using System.Configuration;
using System.IO;


namespace IIITS.DTLMS.BL
{
    public class clsDeCommissioning
    {

        string strFormCode = "clsDeCommissioning";
        public string sDecommId { get; set; }
        public string sFailureId { get; set; }
        public string sFailureDate { get; set; }
        public string sTRReading { get; set; }
        public string sRINo { get; set; }
        public string sRIDate { get; set; }
        public string sRVNo { get; set; }
        public string sRVDate { get; set; }
        public string sRemarks { get; set; }
        public string sStoreId { get; set; }
        public string sInvoiceId { get; set; }
        public string sCrby { get; set; }
        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }
        public string sDTCCode { get; set; }
        public string sOilQuantity { get; set; }
        public string sDecommDate { get; set; }
        public string sManualRINo { get; set; }
        public string sCommDate { get; set; }
        public string sWorkOrderNo { get; set; }
        public string sWorkOrderDate { get; set; }


        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }

        public string sDtrWarrentyTime { get; set; }
        public string sWarrentyPeriod { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        public DataTable LoadAllDecommission(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = "SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID) DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,IN_NO,";
                strQry += " TR_ID,'YES' AS STATUS FROM TBLDTCMAST,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT, TBLDTCINVOICE,TBLTCREPLACE";
                strQry += " WHERE DT_CODE = DF_DTC_CODE  AND DF_ID = WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_STATUS_FLAG=:sTaskType";
                strQry += " AND TR_IN_NO=IN_NO AND DF_LOC_CODE LIKE :sOfficeCode||'%' ";
                strQry += " UNION ALL ";
                strQry += " SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID) DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,";
                strQry += " IN_NO,0 AS TR_ID,'NO' AS STATUS FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE";
                strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE  AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_REPLACE_FLAG = 0";
                strQry += " AND IN_NO NOT IN (SELECT TR_IN_NO FROM TBLTCREPLACE) AND DF_STATUS_FLAG=:sTaskType1";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode1||'%' ";
                oledbCommand.Parameters.AddWithValue("sTaskType", objDecomm.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objDecomm.sOfficeCode);
                oledbCommand.Parameters.AddWithValue("sTaskType1", objDecomm.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode1", objDecomm.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllDecommission");
                return dt;
            }            

        }

        public DataTable LoadCreateDecommission(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry += " SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID) DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,";
                strQry += " IN_NO,0 AS TR_ID,'NO' AS STATUS FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE";
                strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE  AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_REPLACE_FLAG = 0";
                strQry += " AND IN_NO NOT IN (SELECT TR_IN_NO FROM TBLTCREPLACE) AND DF_STATUS_FLAG=:sTaskType";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%' ";
                oledbCommand.Parameters.AddWithValue("sTaskType", objDecomm.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objDecomm.sOfficeCode);

                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllDecommission");
                return dt;
            }            

        }

        public DataTable LoadAlreadyDecomm(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                
                strQry = "SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID)DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,IN_NO,";
                strQry += " TR_ID,'YES' AS STATUS FROM TBLDTCMAST,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT, TBLDTCINVOICE,TBLTCREPLACE";
                strQry += " WHERE DT_CODE = DF_DTC_CODE  AND DF_ID = WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_STATUS_FLAG=:sTaskType AND TR_IN_NO=IN_NO";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%' AND DF_REPLACE_FLAG='0'";
                oledbCommand.Parameters.AddWithValue("sTaskType", objDecomm.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objDecomm.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyDecomm");
                return dt;
            }            

        }

        public void updateRecord(string DtcCode)
        {
            string strQry = string.Empty;
            string strTemp;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = "SELECT DF_ID FROM TBLDTCFAILURE WHERE DF_DTC_CODE=:DtcCode AND DF_REPLACE_FLAG='0'";
                oledbCommand.Parameters.AddWithValue("DtcCode", DtcCode);
                string df_id = ObjCon.get_value(strQry, oledbCommand);


                oledbCommand = new OleDbCommand();
                strTemp = "SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_DF_ID=:df_id";
                oledbCommand.Parameters.AddWithValue("df_id", df_id);
                string sReplaceTCCode = ObjCon.get_value(strTemp, oledbCommand);

                oledbCommand = new OleDbCommand();
                strQry = "SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN 1 ELSE 0 END FROM TBLTCMASTER WHERE TC_CODE=:sReplaceTCCode";
                oledbCommand.Parameters.AddWithValue("sReplaceTCCode", sReplaceTCCode);
                string sval = ObjCon.get_value(strQry, oledbCommand);

                if (sval == "0")
                {
                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT to_char(add_months(TM_MAPPING_DATE,Warenty_month),'yyyy-mm-dd')Warenty_date FROM (SELECT rsd_guarrenty_type,";
                    strQry += " (rsd_warenty_period * 12)Warenty_month,TC_CODE,TM_MAPPING_DATE FROM TBLTRANSDTCMAPPING,TBLREPAIRSENTDETAILS,";
                    strQry += " TBLTCMASTER WHERE  TC_CODE=RSD_TC_CODE AND RSD_TC_CODE=TM_TC_ID AND TC_CODE=:sReplaceTCCode2 AND TM_LIVE_FLAG=1)";
                    oledbCommand.Parameters.AddWithValue("sReplaceTCCode2", sReplaceTCCode);
                    sDtrWarrentyTime = ObjCon.get_value(strQry, oledbCommand);

                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT RSD_WARENTY_PERIOD FROM TBLTRANSDTCMAPPING,TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE AND ";
                    strQry += "RSD_TC_CODE =TM_TC_ID AND TC_CODE=:sReplaceTCCode1 AND TM_LIVE_FLAG=1";
                    oledbCommand.Parameters.AddWithValue("sReplaceTCCode1", sReplaceTCCode);
                    sWarrentyPeriod = ObjCon.get_value(strQry, oledbCommand);
                }
                if (sval == "0")
                {
                    oledbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLTCMASTER SET TC_WARANTY_PERIOD=TO_DATE(:sDtrWarrentyTime,'yyyy-MM-dd'),TC_WARRENTY=:sWarrentyPeriod WHERE ";
                    strQry += " TC_CODE=:sReplaceTCCode";
                    oledbCommand.Parameters.AddWithValue("sDtrWarrentyTime", sDtrWarrentyTime);
                    oledbCommand.Parameters.AddWithValue("sWarrentyPeriod", sWarrentyPeriod);
                    oledbCommand.Parameters.AddWithValue("sReplaceTCCode", sReplaceTCCode);

                    ObjCon.Execute(strQry, oledbCommand);
                }
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyDecomm");
            }            
        }

        public string[] SaveReplaceDetails(clsDeCommissioning objReplace)
        {
            //Added by sandeep Because of Begin and Commit 15-11-2022
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);         
            string[] Arr = new string[2];
            try
            {
                

                string strQry = string.Empty;
                string strTemp = string.Empty;
                string strQry1 = string.Empty;
                oledbCommand = new OleDbCommand();
                //Check Failure ID is exists or not
                oledbCommand.Parameters.AddWithValue("sFailureId", objReplace.sFailureId);
                OleDbDataReader dr = ObjCon.Fetch("SELECT DF_ID FROM TBLDTCFAILURE WHERE DF_ID=:sFailureId", oledbCommand);
                if (!dr.Read())
                {
                    Arr[0] = "Enter Valid Failure ID";
                    Arr[1] = "2";
                    dr.Close();
                    return Arr;
                }
                dr.Close();

                if (objReplace.sDecommId == "")
                {
                    oledbCommand = new OleDbCommand();

                    //To get Failure TC ID and DTC Code
                    strTemp = " SELECT CAST(DF_EQUIPMENT_ID AS VARCHAR2(20)) || '~' || CAST(DF_DTC_CODE AS VARCHAR2(20)) FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,";
                    strTemp += " TBLDTCINVOICE WHERE DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=:sInvoiceId";
                    oledbCommand.Parameters.AddWithValue("sInvoiceId", objReplace.sInvoiceId);
                    string sTCCode = ObjCon.get_value(strTemp, oledbCommand);

                    oledbCommand = new OleDbCommand();
                    //To get Alloted New TC ID
                    oledbCommand = new OleDbCommand();
                    strTemp = "SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_DF_ID=:sFailureId";
                    oledbCommand.Parameters.AddWithValue("sFailureId", objReplace.sFailureId);
                    string sReplaceTCCode = ObjCon.get_value(strTemp, oledbCommand);
                    
                    //Workflow / Approval
                    #region Workflow


                    strQry = "INSERT INTO TBLTCREPLACE(TR_ID,TR_RDG,TR_RI_NO,TR_RI_DATE,TR_DESC,TR_IN_NO,TR_STORE_SLNO,TR_CRBY,TR_OIL_QUNTY,TR_DECOMM_DATE,TR_MANUAL_RINO,TR_COMM_DATE)";
                    strQry += "VALUES('{0}','" + objReplace.sTRReading + "',";
                    strQry += " (SELECT RINUMBER('" + objReplace.sInvoiceId + "') FROM DUAL),TO_DATE('" + objReplace.sRIDate + "','dd/MM/yyyy'),'" + objReplace.sRemarks + "',";
                    strQry += " '" + objReplace.sInvoiceId + "','" + objReplace.sStoreId + "',";
                    strQry += " '" + objReplace.sCrby + "','" + objReplace.sOilQuantity + "',TO_DATE('" + objReplace.sDecommDate + "','dd/MM/yyyy'),'" + objReplace.sManualRINo.ToUpper() + "',TO_DATE('" + objReplace.sCommDate + "','dd/MM/yyyy'))";

                    strQry = strQry.Replace("'", "''");
                    
                    strQry1 = "UPDATE TBLTCMASTER SET TC_UPDATED_EVENT='REPLACE',TC_UPDATED_EVENT_ID='{0}',";
                    strQry1 += " TC_CURRENT_LOCATION=2,TC_LOCATION_ID='" + objReplace.sOfficeCode + "' WHERE ";
                    strQry1 += " TC_CODE=(select TD_TC_NO from TBLTCDRAWN where TD_INV_NO='" + objReplace.sInvoiceId + "')";
                    strQry1 = strQry1.Replace("'", "''");

                    string strQry2 = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG= '0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objReplace.sCrby + "',";
                    strQry2 += " TM_UNMAP_REASON='FROM FAILURE ENTRY' WHERE TM_TC_ID='" + sTCCode.Split('~').GetValue(0).ToString() + "'";
                    strQry2 += " AND TM_LIVE_FLAG='1' AND TM_DTC_ID='" + objReplace.sDTCCode + "'";
                    
                    strQry2 = strQry2.Replace("'", "''");

                    string strQry3 = "UPDATE TBLDTCMAST SET DT_TC_ID='" + sReplaceTCCode + "' WHERE DT_CODE='" + objReplace.sDTCCode + "'";

                    strQry3 = strQry3.Replace("'", "''");

                    string strQry4 = "INSERT INTO TBLTRANSDTCMAPPING(TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON)";
                    strQry4 += " VALUES('{1}', SYSDATE ,'" + sReplaceTCCode + "',";
                    strQry4 += " '" + objReplace.sDTCCode + "','1','" + objReplace.sCrby + "',SYSDATE)";

                    strQry4 = strQry4.Replace("'", "''");



                    string sParam = "SELECT NVL(MAX(TR_ID),0)+1 FROM TBLTCREPLACE";

                    string sParam1 = "SELECT NVL(MAX(TM_ID),0)+1 FROM TBLTRANSDTCMAPPING";

                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objReplace.sFormName;
                    objApproval.sOfficeCode = objReplace.sOfficeCode;
                    objApproval.sClientIp = objReplace.sClientIP;
                    objApproval.sCrby = objReplace.sCrby;
                    objApproval.sWFObjectId = objReplace.sWFOId;
                    objApproval.sDataReferenceId = objReplace.sDTCCode;
                    objApproval.sWFAutoId = objReplace.sWFAutoId;

                    objApproval.sQryValues = strQry + ";" + strQry1 + ";" + strQry2 + ";" + strQry3 + ";" + strQry4;
                    objApproval.sParameterValues = sParam + ";" + sParam1;
                    objApproval.sMainTable = "TBLTCREPLACE";

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("sFailureId1", objReplace.sFailureId);
                    string sWorkOrderNo = ObjCon.get_value("SELECT WO_NO_DECOM  FROM TBLDTCFAILURE,TBLWORKORDER  WHERE DF_ID = WO_DF_ID AND DF_ID =:sFailureId1 ", oledbCommand);

                    objApproval.sDescription = "Decommisioning for DTC Code " + objReplace.sDTCCode + " with WorkOrder Number " + sWorkOrderNo + "";

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("sInvoiceId", objReplace.sInvoiceId);
                    objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=:sInvoiceId",oledbCommand);

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "TR_ID,TR_RDG,TR_RI_NO,TR_RI_DATE,TR_DESC,TR_IN_NO,TR_STORE_SLNO,TR_CRBY,TR_OIL_QUNTY,TR_DECOMM_DATE,OFFICECODE,TR_MANUAL_RINO,TR_COMM_DATE";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objReplace.sTRReading + "," + objReplace.sRINo + "," + objReplace.sRIDate + "," + objReplace.sRemarks.Replace(",", "") + ",";
                    objApproval.sColumnValues += "" + objReplace.sInvoiceId + "," + objReplace.sStoreId + ",";
                    objApproval.sColumnValues += "" + objReplace.sCrby + "," + objReplace.sOilQuantity + "," + objReplace.sDecommDate + "," + objReplace.sOfficeCode + "," + objReplace.sManualRINo + "," + objReplace.sCommDate + "";


                    objApproval.sTableNames = "TBLTCREPLACE";

                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }


                    objconn.BeginTrans();
                    string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

                    if (!Directory.Exists(sFolderPath))
                    {
                        Directory.CreateDirectory(sFolderPath);

                    }
                    string sFileName = "Begintrans";
                    string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , Begin"  + Environment.NewLine);

                    if (objReplace.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData1(objApproval, objconn);
                        objReplace.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {
                        File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , Else Action Type" + Environment.NewLine);

                        objApproval.SaveWorkFlowData1(objApproval, objconn);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow1(objconn);
                        objApproval.SaveWorkflowObjects1(objApproval, objconn);
                    }
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , Before Commit" + Environment.NewLine);


                    objconn.CommitTrans();
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , After Commit " + Environment.NewLine + "###############################" + Environment.NewLine);

                    #endregion
                    Arr[0] = "Decommissioning Done Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    oledbCommand = new OleDbCommand();

                    strQry = "UPDATE TBLTCREPLACE SET TR_RDG=:sTRReading,TR_RI_NO=:sRINo,TR_RI_DATE=TO_DATE(:sRIDate,'dd/MM/yyyy'),";
                    strQry += " TR_DESC=:sRemarks,TR_STORE_SLNO=:sStoreId,TR_RV_NO=:sRVNo,";
                    strQry += " TR_RV_DATE=TO_DATE(:sRVDate,'dd/MM/yyyy'),TR_OIL_QUNTY=:sOilQuantity,TR_COMM_DATE=:sCommDate' WHERE TR_ID=:sDecommId";
                    oledbCommand.Parameters.AddWithValue("sTRReading", objReplace.sTRReading);
                    oledbCommand.Parameters.AddWithValue("sRINo", objReplace.sRINo);
                    oledbCommand.Parameters.AddWithValue("sRIDate", objReplace.sRIDate);
                    oledbCommand.Parameters.AddWithValue("sRemarks", objReplace.sRemarks);
                    oledbCommand.Parameters.AddWithValue("sInvoiceId", objReplace.sInvoiceId);
                    oledbCommand.Parameters.AddWithValue("sStoreId", objReplace.sStoreId);

                    oledbCommand.Parameters.AddWithValue("sRVNo", objReplace.sRVNo);
                    oledbCommand.Parameters.AddWithValue("sRVDate", objReplace.sRVDate);
                    oledbCommand.Parameters.AddWithValue("sOilQuantity", objReplace.sOilQuantity);
                    oledbCommand.Parameters.AddWithValue("sCommDate", objReplace.sCommDate);
                    oledbCommand.Parameters.AddWithValue("sDecommId", objReplace.sDecommId);
                    ObjCon.Execute(strQry,oledbCommand);


                    Arr[0] = "Decommission Details Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }


            }

            catch (Exception ex)
            {
                objconn.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveReplaceDetails");
                // return Arr;
                throw ex;
            }            

        }


        public clsDeCommissioning GetDecommDetails(clsDeCommissioning objDecomm)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();          
                strQry = "SELECT TR_RDG,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD/MM/YYYY') TR_RI_DATE,TR_DESC,TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MM/YYYY') TR_RV_DATE,";
                strQry += " TR_STORE_SLNO,TR_OIL_QUNTY,TO_CHAR(TR_DECOMM_DATE,'DD/MM/YYYY') TR_DECOMM_DATE,TR_MANUAL_RINO from TBLTCREPLACE WHERE TR_ID=:sDecommId";
                oledbCommand.Parameters.AddWithValue("sDecommId", objDecomm.sDecommId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objDecomm.sRINo = dt.Rows[0]["TR_RI_NO"].ToString();
                    objDecomm.sRIDate = dt.Rows[0]["TR_RI_DATE"].ToString();
                    objDecomm.sRemarks = dt.Rows[0]["TR_DESC"].ToString();
                    objDecomm.sStoreId = dt.Rows[0]["TR_STORE_SLNO"].ToString();
                    objDecomm.sRVNo = dt.Rows[0]["TR_RV_NO"].ToString();
                    objDecomm.sRVDate = dt.Rows[0]["TR_RV_DATE"].ToString();
                    objDecomm.sOilQuantity = dt.Rows[0]["TR_OIL_QUNTY"].ToString();
                    objDecomm.sTRReading = dt.Rows[0]["TR_RDG"].ToString();
                    objDecomm.sDecommDate = dt.Rows[0]["TR_DECOMM_DATE"].ToString();
                    objDecomm.sManualRINo = dt.Rows[0]["TR_MANUAL_RINO"].ToString();
                }

                return objDecomm;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDecommDetails");
                return objDecomm;
            }            
        }

        public string GetInvoiceNo(string sFailureId)
        {
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();          
                strQry = "SELECT IN_NO FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE DF_ID=WO_DF_ID ";
                strQry += " AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND DF_ID=:sFailureId";
                oledbCommand.Parameters.AddWithValue("sFailureId", sFailureId);
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoiceNo");
                return "";
            }            
        }

        public string GenerateRINo(string sOfficeCode)
        {
            try
            {
                
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sRINo = ObjCon.get_value("SELECT NVL(MAX(TR_RI_NO),0)+1  FROM TBLTCREPLACE WHERE TR_RI_NO LIKE :sOfficeCode||'%' ",oledbCommand);
                if (sRINo.Length == 1)
                {

                    //4 digit Section Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy");
                    }

                    sRINo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //4 digit Section Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sRINo.Substring(4, 2))
                        {
                            return sRINo;
                        }
                        else
                        {
                            sRINo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sRINo;
                    }


                }

                return sRINo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateRINo");
                return "";
            }            
        }



        #region NewDTC

        public DataTable LoadCreateNewDTCDecommission(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                if (objDecomm.sOfficeCode.Length > 3)
                {
                    objDecomm.sOfficeCode = objDecomm.sOfficeCode.Substring(0, 3);
                }
                oledbCommand = new OleDbCommand();
              
                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,TI_ID,TI_INDENT_NO,'NO' AS STATUS,IN_INV_NO,";
                strQry += " TO_CHAR(IN_DATE,'DD-MON-YYYY') IN_DATE,(SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_INV_NO=IN_NO) TD_TC_NO,IN_NO ";
                strQry += "  FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND ";
                strQry += " WO_DF_ID IS NULL AND WO_REQUEST_LOC LIKE :sOfficeCode||'%' AND TI_ID=IN_TI_NO AND IN_NO NOT IN ";
                strQry += " (SELECT TR_IN_NO FROM TBLTCREPLACE)";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objDecomm.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCreateNewDTCDecommission");
                return dt;
            }            

        }


        #endregion

        #region WorkFlow XML

        public clsDeCommissioning GetDecommDetailsFromXML(clsDeCommissioning objDecomm)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if(objDecomm.sWFDataId!="" && objDecomm.sWFDataId!=null)
                {
                    dt = objApproval.GetDatatableFromXML(objDecomm.sWFDataId);
                }

                if (dt.Rows.Count > 0)
                {
                    objDecomm.sRINo = dt.Rows[0]["TR_RI_NO"].ToString();
                    objDecomm.sRIDate = dt.Rows[0]["TR_RI_DATE"].ToString();
                    objDecomm.sRemarks = dt.Rows[0]["TR_DESC"].ToString().Replace("ç", ",");
                    objDecomm.sStoreId = dt.Rows[0]["TR_STORE_SLNO"].ToString();
                    //objDecomm.sRVNo = dt.Rows[0]["TR_RV_NO"].ToString();
                    //objDecomm.sRVDate = dt.Rows[0]["TR_RV_DATE"].ToString();
                    objDecomm.sOilQuantity = dt.Rows[0]["TR_OIL_QUNTY"].ToString();
                    objDecomm.sTRReading = dt.Rows[0]["TR_RDG"].ToString();
                    objDecomm.sDecommDate = dt.Rows[0]["TR_DECOMM_DATE"].ToString();
                    objDecomm.sCrby = dt.Rows[0]["TR_CRBY"].ToString();
                    objDecomm.sOfficeCode = dt.Rows[0]["OFFICECODE"].ToString();
                    objDecomm.sCommDate = dt.Rows[0]["TR_COMM_DATE"].ToString();
                    if (dt.Columns.Contains("TR_MANUAL_RINO"))
                    {
                        objDecomm.sManualRINo = Convert.ToString(dt.Rows[0]["TR_MANUAL_RINO"]);
                    }
                }
                return objDecomm;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDecommDetailsFromXML");
                return objDecomm;
            }
        }

        #endregion
    }
}
