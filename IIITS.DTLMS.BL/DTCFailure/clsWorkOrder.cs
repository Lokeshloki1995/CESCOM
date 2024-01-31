using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.BL
{
   public class clsWorkOrder
    {
       string strFormCode = "clsWorkOrder";
       CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string sWOId { get; set; }
        public string sFailureId { get; set; }
        public string sFailureDate { get; set; }
        public string sLocationCode { get; set; }
        public string sAccCode { get; set; }
        public string sCrBy { get; set; }
        public string sCommWoNo { get; set; }
        public string sCommDate { get; set; }
        public string sCommAmmount { get; set; }
        public string sDeWoNo { get; set; }
        public string sDeCommDate { get; set; }
        public string sDeCommAmmount { get; set; }
       
        public string sDecomAccCode { get; set; }
        public string sIssuedBy { get; set; }
        public string sCapacity { get; set; }
        public string sNewCapacity { get; set; }
        public string sEnhanceAccCode { get; set; }
        public string sGoodEnhanceAccCode { get; set; }
        public string sEnhancedCapacity { get; set; }

        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }

        public string sDTCName { get; set; }
        public string sTCSlno { get; set; }
        public string sTCCode { get; set; }
        public string sRequestLoc { get; set; }
        public string sDTCCode { get; set; }
        public string sDTCId { get; set; }
        public string sTCId { get; set; }

        public string sWOFilePath { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFObjectId { get; set; }

        public string sApprovalDesc { get; set; }
        public int sDtcScheme { get; set; }
        public string sDivCode { get; set; }
        public string sBudgetAccCode { get; set; }
        public string sBudgetAmount { get; set; }
        public string sFinancialYear { get; set; }
        public string sBMID { get; set; }
        public string  Budget { get; set; }
        public string sRoleId { get; set; }
        public string  sBudgetDivCode { get; set; }
        public string sCustomerName { get; set; }
        public string sCustomerNumber { get; set; }
        public string sNumberInsulator { get; set; }
        OleDbCommand oledbCommand;

        public string[] SaveUpdateWorkOrder(clsWorkOrder objWorkOrder)
        {
          
            string strQry = string.Empty;
            string[] Arr = new string[2];
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);
            try
            {

                oledbCommand = new OleDbCommand();
                if (objWorkOrder.sTaskType != "3")
                {
                    oledbCommand.Parameters.AddWithValue("FailureId", objWorkOrder.sFailureId);
                    //Check Failure ID is exists or not
                    OleDbDataReader dr = ObjCon.Fetch("SELECT DF_ID FROM TBLDTCFAILURE WHERE DF_ID=:FailureId", oledbCommand);
                    if (!dr.Read())
                    {
                        Arr[0] = "Enter Valid Failure ID";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();
                }


                if (objWorkOrder.sWOId == "")
                {
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("CommWoNo", objWorkOrder.sCommWoNo);
                    oledbCommand.Parameters.AddWithValue("LocationCode", objWorkOrder.sLocationCode);
                    oledbCommand.Parameters.AddWithValue("dfId", objWorkOrder.sFailureId);
                    OleDbDataReader dr = ObjCon.Fetch("SELECT WO_NO FROM TBLWORKORDER WHERE WO_NO=:CommWoNo AND WO_OFF_CODE=:LocationCode AND WO_DF_ID <> :dfId ", oledbCommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Work Order No. Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("DeWoNo", objWorkOrder.sDeWoNo);
                    oledbCommand.Parameters.AddWithValue("LocationCode1", objWorkOrder.sLocationCode);
                    oledbCommand.Parameters.AddWithValue("dfId", objWorkOrder.sFailureId);


                    dr = ObjCon.Fetch("SELECT WO_NO_DECOM FROM TBLWORKORDER WHERE WO_NO_DECOM=:DeWoNo AND WO_OFF_CODE=:LocationCode1 AND WO_DF_ID <> :dfId  ", oledbCommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Decommmissioning Work Order No. Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();
                    
                    // check in the dtc failure table  
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("CommWoNo", objWorkOrder.sCommWoNo);
                    oledbCommand.Parameters.AddWithValue("LocationCode", objWorkOrder.sLocationCode);
                    strQry = "SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_COMM_WONO= '" + objWorkOrder.sCommWoNo + "' AND substr(DF_LOC_CODE , 0 , 2 ) = '"+ objWorkOrder.sLocationCode + "' and  DF_ID <> '"+ objWorkOrder.sFailureId+ "'";
                     dr = ObjCon.Fetch(strQry);
                    if (dr.Read())
                    {
                        string dtcCode = dr.GetValue(0).ToString();
                        Arr[0] = "Work Order No. Already given for  "+ dtcCode + "  DTC";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("DeWoNo", objWorkOrder.sDeWoNo);
                    oledbCommand.Parameters.AddWithValue("LocationCode1", objWorkOrder.sLocationCode);
                    strQry = "SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_COMM_WONO= '" + objWorkOrder.sDeWoNo + "' AND substr(DF_LOC_CODE , 0 , 2 ) = '" + objWorkOrder.sLocationCode + "' and  DF_ID <> '" + objWorkOrder.sFailureId + "'";
                    dr = ObjCon.Fetch(strQry);
                    if (dr.Read())
                    {
                        string dtcCode = dr.GetValue(0).ToString();
                        Arr[0] = "Work Order No. Already given for  " + dtcCode + "  DTC";
                        Arr[1] = "2";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();

                    // end 

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("FailureId1", objWorkOrder.sFailureId);
                    string sDTCLocationCode = ObjCon.get_value("SELECT DF_LOC_CODE  FROM TBLDTCFAILURE WHERE DF_ID = :FailureId1", oledbCommand);

                    ObjCon.BeginTrans();
                    // DF_STATUS_FLAG--->1 Failure Entry ;  DF_STATUS_FLAG--->2 Enhancement Entry  
                    objWorkOrder.sWOId = Convert.ToString(ObjCon.Get_max_no("WO_SLNO", "TBLWORKORDER"));
                    strQry = "Insert into TBLWORKORDER (WO_SLNO,WO_NO,WO_DF_ID,WO_DATE,WO_AMT,WO_NO_DECOM,WO_DATE_DECOM,WO_AMT_DECOM,";
                    strQry += " WO_ACC_CODE,WO_OFF_CODE,WO_CRBY,WO_CRON,WO_ACCCODE_DECOM,WO_ISSUED_BY,WO_DTC_CAP,WO_NEW_CAP,WO_REQUEST_LOC) VALUES";
                    strQry += "('" + objWorkOrder.sWOId + "','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sFailureId + "',";
                    strQry+=" TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "', '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                    strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',SYSDATE,";
                    strQry+= " '"+ objWorkOrder.sDecomAccCode +"','"+ objWorkOrder.sIssuedBy  +"','"+ objWorkOrder.sCapacity  +"','"+ objWorkOrder.sNewCapacity  +"','"+ objWorkOrder.sRequestLoc +"') ";

                    //ObjCon.Execute(strQry);


                    //SaveWOFilePath(objWorkOrder);

                    //************************ WORK FLOW *************************************



                    //SendSMStoSectionStoreOfficer();

                    ObjCon.CommitTrans();

                    if (objWorkOrder.sTaskType == "2" || objWorkOrder.sTaskType == "5")
                    {
                        if (objWorkOrder.sRoleId=="7" && objWorkOrder.sActionType!="M")
                        {

                            string BM_OBAMOUNT = ObjCon.get_value("SELECT BM_OB_AMNT  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objWorkOrder.sBudgetAccCode + "' AND BM_FIN_YEAR = '" + objWorkOrder.sFinancialYear + "'", oledbCommand);

                            string BM_id = ObjCon.get_value("SELECT BM_ID  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objWorkOrder.sBudgetAccCode + "'", oledbCommand);
                            double availableamt = Convert.ToDouble(BM_OBAMOUNT) - Convert.ToDouble(objWorkOrder.sCommAmmount);

                            string BtId = Convert.ToString(ObjCon.Get_max_no("BT_ID", "TBLCOMMISSION_BUDGETTRANS"));
                            strQry = "INSERT INTO TBLCOMMISSION_BUDGETTRANS (BT_ID,BT_ACC_CODE,BT_BM_AMNT,BT_AVL_AMNT,BT_CRON,BT_DEBIT_AMNT,BT_FIN_YEAR,BT_DIV_CODE,BT_WONO,BT_BM_ID) VALUES('" + BtId + "','" + objWorkOrder.sBudgetAccCode + "',";
                            strQry += " '" + BM_OBAMOUNT + "','" + availableamt + "',SYSDATE,'" + objWorkOrder.sCommAmmount + "','" + objWorkOrder.sFinancialYear + "','" + objWorkOrder.sOfficeCode + "','" + objWorkOrder.sCommWoNo.ToUpper() + "'," + BM_id + ")";
                            ObjCon.Execute(strQry);

                            strQry = "UPDATE TBLBUDGETMAST SET BM_OB_AMNT = '" + availableamt + "' where BM_ACC_CODE='" + objWorkOrder.sBudgetAccCode + "' AND BM_FIN_YEAR = '" + objWorkOrder.sFinancialYear + "'";
                            ObjCon.Execute(strQry);
                        }
                    }          

                    #region Workflow


                    strQry = "Insert into TBLWORKORDER (WO_SLNO,WO_NO,WO_DF_ID,WO_DATE,WO_AMT,WO_NO_DECOM,WO_DATE_DECOM,WO_AMT_DECOM,";
                    strQry += " WO_ACC_CODE,WO_OFF_CODE,WO_CRBY,WO_CRON,WO_ACCCODE_DECOM,WO_ISSUED_BY,WO_DTC_CAP,WO_NEW_CAP,WO_REQUEST_LOC,WO_AUTO_NO) VALUES";
                    strQry += "('{0}','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sFailureId + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "', '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                    strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',SYSDATE,";
                    strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                    strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}') ";

                    strQry = strQry.Replace("'", "''");

                    string sParam = "SELECT NVL(MAX(WO_SLNO),0)+1 FROM TBLWORKORDER";
                    //     string sParam1 = "SELECT WONUMBER('" + objWorkOrder.sOfficeCode + "') FROM DUAL";
                    string sParam1 = "SELECT WONUMBERLATEST('" + sDTCLocationCode + "') FROM DUAL";

                    sParam1 = sParam1.Replace("'", "''");

                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objWorkOrder.sFormName;
                    //objApproval.sRecordId = objWorkOrder.sWOId;
                    objApproval.sOfficeCode = objWorkOrder.sOfficeCode;
                    objApproval.sClientIp = objWorkOrder.sClientIP;
                    objApproval.sCrby = objWorkOrder.sCrBy;
                    objApproval.sWFObjectId = objWorkOrder.sWFOId;
                    objApproval.sWFAutoId = objWorkOrder.sWFAutoId;

                    objApproval.sQryValues = strQry;
                    objApproval.sParameterValues = sParam + ";" + sParam1;
                    objApproval.sMainTable = "TBLWORKORDER";

                    objApproval.sDataReferenceId = objWorkOrder.sFailureId;

                    //string sDtcCode = ObjCon.get_value("SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_ID='" + objWorkOrder.sFailureId + "'");
                    if (objWorkOrder.sTaskType == "1" || objWorkOrder.sTaskType == "4")
                    {
                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("FailureId1", objWorkOrder.sFailureId);
                       // oledbCommand.Parameters.AddWithValue("LocationCode1", objWorkOrder.sLocationCode);

                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE WHERE DF_ID=:FailureId1", oledbCommand);
                        objApproval.sDescription = "Work Order For Failure Entry of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " +objWorkOrder.sDTCName + " with WO No "+ objWorkOrder.sCommWoNo +"";

                    }
                    else if (objWorkOrder.sTaskType == "2")
                    {

                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("FailureId2", objWorkOrder.sFailureId);

                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE WHERE DF_ID=:FailureId2", oledbCommand);
                        objApproval.sDescription = "Work Order For Capacity Enhancement of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " + objWorkOrder.sDTCName + " with WO No " + objWorkOrder.sCommWoNo + "";
                    }
                    else if (objWorkOrder.sTaskType == "5")
                    {

                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("FailureId2", objWorkOrder.sFailureId);

                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE WHERE DF_ID=:FailureId2", oledbCommand);
                        objApproval.sDescription = "Work Order For Capacity Reduction of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " + objWorkOrder.sDTCName + " with WO No " + objWorkOrder.sCommWoNo + "";
                    }
                    else if (objWorkOrder.sTaskType == "3")
                    {
                        objApproval.sRefOfficeCode = objWorkOrder.sRequestLoc;
                        objApproval.sDescription = "Work Order For New DTC Commission with WO NO : " + objWorkOrder.sCommWoNo;
                    }


                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "WO_SLNO,WO_NO,WO_DF_ID,WO_DATE,WO_AMT,WO_NO_DECOM,WO_DATE_DECOM,WO_AMT_DECOM,";
                    objApproval.sColumnNames += "WO_ACC_CODE,WO_OFF_CODE,WO_CRBY,WO_CRON,WO_ACCCODE_DECOM,WO_ISSUED_BY,WO_DTC_CAP,WO_NEW_CAP,WO_REQUEST_LOC,WO_DTC_SCHEME";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objWorkOrder.sCommWoNo.ToUpper() + "," + objWorkOrder.sFailureId + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sCommDate + "," + objWorkOrder.sCommAmmount + ", " + objWorkOrder.sDeWoNo.ToUpper() + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sDeCommDate + "," + objWorkOrder.sDeCommAmmount + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sAccCode + "," + objWorkOrder.sLocationCode + "," + objWorkOrder.sCrBy + ",SYSDATE,";
                    objApproval.sColumnValues += "" + objWorkOrder.sDecomAccCode + "," + objWorkOrder.sIssuedBy + "," + objWorkOrder.sCapacity + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sNewCapacity + "," + objWorkOrder.sRequestLoc + ","+ objWorkOrder.sDtcScheme +"";


                    objApproval.sTableNames = "TBLWORKORDER";


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }


                    if (objWorkOrder.sActionType == "M")
                    {
                        // objApproval.SaveWorkFlowData(objApproval);
                        objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;
                        objWorkOrder.sApprovalDesc = objApproval.sDescription;
                    }
                    else
                    {
                        objconn.BeginTrans();
                        objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;  
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlowlatest(objconn);
                        objApproval.SaveWorkflowObjectslatest(objApproval, objconn);
                        objWorkOrder.sWFObjectId = objApproval.sWFObjectId; 
                    }
         

                    #endregion

                    UpdateWorkOrderinFailure(objWorkOrder);

                    Arr[0] = "Work Order Created Successfully";
                    Arr[1] = "0";
                    return Arr;

                }

                else
                {
                    ObjCon.BeginTrans();
                    oledbCommand = new OleDbCommand();

                    oledbCommand.Parameters.AddWithValue("sCommWoNo1", objWorkOrder.sCommWoNo);
                    oledbCommand.Parameters.AddWithValue("sWOId1", objWorkOrder.sWOId);
                    oledbCommand.Parameters.AddWithValue("sLocationCode2", objWorkOrder.sLocationCode);
                    OleDbDataReader dr = ObjCon.Fetch("SELECT WO_NO FROM TBLWORKORDER WHERE WO_NO=:sCommWoNo1 AND WO_SLNO<> :sWOId1 AND WO_OFF_CODE=:sLocationCode2", oledbCommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Work Order No. Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();
                    oledbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLWORKORDER SET WO_NO =:sCommWoNo12,";
                    strQry += " WO_DF_ID= :sFailureId12,";
                    strQry += " WO_OFF_CODE= :sLocationCode12,";
                    strQry += " WO_ACC_CODE= :sAccCode12,WO_DATE= TO_DATE(:sCommDate12,'dd/MM/yyyy'),WO_AMT= :sCommAmmount12,WO_NO_DECOM= :sDeWoNo12,";
                    strQry += " WO_DATE_DECOM= TO_DATE(:sDeCommDate12,'dd/MM/yyyy'),WO_AMT_DECOM=:sDeCommAmmount12,";
                    strQry += " WO_ACCCODE_DECOM=:sDecomAccCode12,WO_ISSUED_BY=:sIssuedBy12,";
                    strQry += "  WO_DTC_CAP=:sCapacity12,WO_NEW_CAP=:sNewCapacity12 WHERE WO_SLNO=:sWOId12";

                    oledbCommand.Parameters.AddWithValue("sCommWoNo12", objWorkOrder.sCommWoNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sFailureId12", objWorkOrder.sFailureId);
                    oledbCommand.Parameters.AddWithValue("sLocationCode12", objWorkOrder.sLocationCode);
                    oledbCommand.Parameters.AddWithValue("sAccCode12", objWorkOrder.sAccCode);
                    oledbCommand.Parameters.AddWithValue("sCommDate12", objWorkOrder.sCommDate);
                    oledbCommand.Parameters.AddWithValue("sCommAmmount12", objWorkOrder.sCommAmmount);
                    oledbCommand.Parameters.AddWithValue("sDeWoNo12", objWorkOrder.sDeWoNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sDeCommDate12", objWorkOrder.sDeCommDate);
                    oledbCommand.Parameters.AddWithValue("sDeCommAmmount12", objWorkOrder.sDeCommAmmount);
                    oledbCommand.Parameters.AddWithValue("sDecomAccCode12", objWorkOrder.sDecomAccCode);
                    oledbCommand.Parameters.AddWithValue("sIssuedBy12", objWorkOrder.sIssuedBy);
                    oledbCommand.Parameters.AddWithValue("sCapacity12", objWorkOrder.sCapacity);
                    oledbCommand.Parameters.AddWithValue("sNewCapacity12", objWorkOrder.sNewCapacity);
                    oledbCommand.Parameters.AddWithValue("sWOId12", objWorkOrder.sWOId);

                    ObjCon.Execute(strQry, oledbCommand);

                    //SaveWOFilePath(objWorkOrder);

                    ObjCon.CommitTrans();
                    Arr[0] = "Work Order Details Updated Successfully";
                    Arr[1] = "1";
                    return Arr;

                }

            }

            catch (Exception ex)
            {
                // ObjCon.RollBack();
                objconn.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateWorkOrder");
                // return Arr;
                throw ex;
            }
            finally
            {
                
            }
        }

        /// <summary>
        /// Author : Pradeep
        /// Date : 18-03-2020
        /// This method is used to update the work  order in the main table since 
        /// we wont get the work order pending for approval  hence i am updating in the failure 
        /// table 
        /// </summary>
        /// <param name="objWorkorder"></param>
        public void UpdateWorkOrderinFailure(clsWorkOrder objWorkorder)
        {
            string strQry = string.Empty;
            try
            {
                strQry = " UPDATE TBLDTCFAILURE set DF_COMM_WONO= '"+objWorkorder.sCommWoNo+ "' , DF_DECOM_WONO =  '" + objWorkorder.sDeWoNo+"'   WHERE DF_ID = "+objWorkorder.sFailureId+"  ";
                ObjCon.Execute(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateWorkOrderinFailure");
            }
        }

        public bool SaveWOFilePath(clsWorkOrder objWO)
        {
            try
            {
                
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string sFTPMainFolder = string.Empty;
                string sFolderName = "WORKORDER";
                
                string sWOFileName = string.Empty;
                string sDirectory = string.Empty;

                //  Photo Save DTLMSDocs

                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPMAINLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                sFTPLink= Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"].ToUpper());
                sFTPMainFolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"].ToUpper());
                sFTPUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"].ToUpper());
                sFTPPassword= Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"].ToUpper());  

                 clsSFTP objFtp = new clsSFTP(sFTPLink, sFTPUserName, sFTPPassword);
                bool Isuploaded;
                sDirectory = objWO.sWOFilePath;

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(sFTPMainFolder + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(sFTPMainFolder);
                }

                 IsExists = objFtp.FtpDirectoryExists(sFTPMainFolder + "/"+ sFolderName + " / ");
                if (IsExists == false)
                {

                    objFtp.createDirectory(sFolderName);
                }

                sWOFileName = Path.GetFileName(objWO.sWOFilePath);

                if (sWOFileName != "")
                {

                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists( sFolderName + "/" + objWO.sWOId + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(sFolderName + "/" + objWO.sWOId );
                        }

                        Isuploaded = objFtp.upload(sFolderName + "/" +objWO.sWOId + "/" ,  sWOFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objWO.sWOFilePath = sFolderName + "/" + objWO.sWOId + "/" + sWOFileName;

                        }
                    }
                }

                string strQry = string.Empty;
                oledbCommand = new OleDbCommand();
                strQry = "UPDATE TBLWORKORDER SET WO_FILE_PATH=:sWOFilePath WHERE WO_SLNO=:sWOId";
                oledbCommand.Parameters.AddWithValue("sWOFilePath", objWO.sWOFilePath);
                oledbCommand.Parameters.AddWithValue("sWOId", objWO.sWOId);
                ObjCon.Execute(strQry, oledbCommand);
                return true;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveWOFilePath");
                return false;
            }
            finally
            {
                
            }
        }

       
        public DataTable LoadAlreadyWorkOrder(clsWorkOrder objWorkOrder)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
               //Type=1----> Failure ;  Type=2-----------> Enhancement

                strQry = "SELECT  DT_CODE, DT_NAME, TO_CHAR(TC_CODE) TC_CODE, TO_CHAR(DF_ID) DF_ID ,WO_NO,'YES' AS STATUS FROM TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE ,";
                strQry += " TBLWORKORDER WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_ID =  WO_DF_ID AND";
                strQry += " DF_STATUS_FLAG=:sTaskType ";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%' ";
                oledbCommand.Parameters.AddWithValue("sTaskType", objWorkOrder.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objWorkOrder.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                //AND DF_APPROVE_STATUS='1'
                return dt;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAlreadyWorkOrder");
                return dt;

            }
            finally
            {
                
            }
        }



        public DataTable LoadAllWorkOrder(clsWorkOrder objWorkOrder)
        {
            oledbCommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                
                //Type=1----> Failure ;  Type=2-----------> Enhancement
                //strQry = "SELECT  DT_CODE, DT_NAME, TC_SLNO, DF_ID, '' AS WO_NO   FROM TBLDTCMAST,TBLTCMASTER,TBLTRANSMAKES,";
                //strQry += " TBLDTCFAILURE WHERE DT_TC_ID = TC_CODE AND TM_ID = TC_MAKE_ID AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='"+ sType  +"' AND ";
                //strQry += " DF_ID NOT IN (SELECT WO_DF_ID FROM  TBLWORKORDER  )";

                strQry = "SELECT  DT_CODE, DT_NAME, TO_CHAR(TC_CODE) TC_CODE, TO_CHAR(DF_ID) DF_ID ,WO_NO,'YES' AS STATUS FROM TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE ,";
                strQry += " TBLWORKORDER WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_ID =  WO_DF_ID AND ";
                strQry += " DF_STATUS_FLAG=:sTaskType ";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%'  ";
                strQry += " UNION ALL";
                strQry += " SELECT  DT_CODE, DT_NAME, TO_CHAR(TC_CODE) TC_CODE, TO_CHAR(DF_ID) DF_ID, '' AS WO_NO,'NO' AS STATUS   FROM TBLDTCMAST,TBLTCMASTER,";
                strQry += " TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG=:sTaskType1  AND ";
                strQry += " DF_ID NOT IN (SELECT WO_DF_ID FROM  TBLWORKORDER WHERE WO_DF_ID IS NOT NULL)";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode1||'%' ";

                oledbCommand.Parameters.AddWithValue("sTaskType", objWorkOrder.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objWorkOrder.sOfficeCode);
                oledbCommand.Parameters.AddWithValue("sTaskType1", objWorkOrder.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode1", objWorkOrder.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllWorkOrder");
                return dt;

            }
            finally
            {
                
            }
        }


        public object GetWorkOrderDetails(clsWorkOrder objWorkOrder)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();
             
               strQry = "SELECT  WO_SLNO,WO_NO,WO_DF_ID,TO_CHAR(WO_DATE,'dd/MM/yyyy')WO_DATE,TO_CHAR(WO_AMT)WO_AMT,WO_NO_DECOM,WO_ACC_CODE,WO_ACCCODE_DECOM,";
               strQry += " WO_OFF_CODE,WO_CRBY,TO_CHAR(WO_CRON,'dd/MM/yyyy')WO_CRON,TO_CHAR(WO_DATE_DECOM,'dd/MM/yyyy')WO_DATE_DECOM,TO_CHAR(WO_AMT_DECOM)WO_AMT_DECOM,WO_ISSUED_BY,  ";
               strQry += " TO_CHAR(DF_DATE,'DD/MM/YYYY')DF_FAILED_DATE,WO_NEW_CAP,WO_REQUEST_LOC,DF_ENHANCE_CAPACITY ";
               strQry += " FROM TBLWORKORDER,TBLDTCFAILURE WHERE DF_ID=WO_DF_ID AND DF_ID=:sFailureId ";
               oledbCommand.Parameters.AddWithValue("sFailureId", objWorkOrder.sFailureId);
               
               dtWODetails = ObjCon.getDataTable(strQry,oledbCommand);

               if (dtWODetails.Rows.Count > 0)
               {

                   objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["WO_SLNO"]).Trim();
                   objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_CODE"]).Trim();
                   objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["WO_CRBY"]).Trim();
                   objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO"]).Trim();
                   objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE"]).Trim();
                   objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT"]).Trim();
                   objWorkOrder.sDeWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO_DECOM"]).Trim();
                   objWorkOrder.sDeCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_DECOM"]).Trim();
                   objWorkOrder.sDeCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_DECOM"]).Trim();
                   objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["WO_ISSUED_BY"]).Trim();
                   objWorkOrder.sDecomAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACCCODE_DECOM"]).Trim();
                   objWorkOrder.sFailureId = Convert.ToString(dtWODetails.Rows[0]["WO_DF_ID"]).Trim();
                   objWorkOrder.sFailureDate = Convert.ToString(dtWODetails.Rows[0]["DF_FAILED_DATE"]).Trim();
                   objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["WO_NEW_CAP"]).Trim();
                   objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["WO_REQUEST_LOC"]).Trim();
                   objWorkOrder.sEnhancedCapacity = Convert.ToString(dtWODetails.Rows[0]["DF_ENHANCE_CAPACITY"]).Trim();
                }
                return objWorkOrder;
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetWorkOrderDetails");
                return objWorkOrder;

            }
            finally
            {
                
            }

        }

        public bool ValidateUpdate(string strFailureId,string sWOSlno,string sType)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                DataTable dt = new DataTable();
                OleDbDataReader dr;
                string strQry = string.Empty;
                if (sType != "3")
                {
                    strQry = "select TI_WO_SLNO from TBLWORKORDER,TBLINDENT,TBLDTCFAILURE WHERE WO_SLNO=TI_WO_SLNO AND ";
                    strQry += " DF_ID=WO_DF_ID AND WO_DF_ID=:FailureId";
                    oledbCommand.Parameters.AddWithValue("FailureId", strFailureId);
                }
                else
                {
                    strQry = "select TI_WO_SLNO from TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND ";
                    strQry += "  WO_SLNO=:WOSlno";
                    oledbCommand.Parameters.AddWithValue("WOSlno", sWOSlno);
                }
                dr = ObjCon.Fetch(strQry, oledbCommand);
                dt.Load(dr);

                if (dt.Rows.Count > 0)
                {
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateUpdate");
                return false;
            }
            finally
            {
                
            }
        }

        public clsWorkOrder GetWOBasicDetails(clsWorkOrder objWO)
        {
            oledbCommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                
                strQry = "SELECT DF_ID,to_char(DF_DATE,'dd/MM/yyyy') DF_FAILED_DATE,DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,DF_NUMBER_OF_INSTALLATIONS,WO_SLNO,WO_NO,to_char(WO_DATE,'dd/MM/yyyy') WO_DATE,WO_NEW_CAP, ";
                strQry += " DT_NAME,TC_CODE,(SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CRBY=US_ID) US_FULL_NAME,DT_CODE,TC_ID,DT_ID,DF_LOC_CODE  FROM TBLDTCFAILURE,TBLWORKORDER,TBLDTCMAST,TBLTCMASTER";
                strQry += " WHERE WO_DF_ID=DF_ID AND DF_EQUIPMENT_ID=TC_CODE AND  DF_DTC_CODE=DT_CODE AND WO_SLNO=:sWOId";
                oledbCommand.Parameters.AddWithValue("WOId", objWO.sWOId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objWO.sCommWoNo = dt.Rows[0]["WO_NO"].ToString();
                    objWO.sCommDate = dt.Rows[0]["WO_DATE"].ToString();
                    objWO.sDTCName = dt.Rows[0]["DT_NAME"].ToString();
                    objWO.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objWO.sCrBy = dt.Rows[0]["US_FULL_NAME"].ToString();
                    objWO.sFailureId = dt.Rows[0]["DF_ID"].ToString();
                    objWO.sFailureDate = dt.Rows[0]["DF_FAILED_DATE"].ToString();
                    objWO.sNewCapacity = dt.Rows[0]["WO_NEW_CAP"].ToString();
                    objWO.sDTCCode = dt.Rows[0]["DT_CODE"].ToString();
                    objWO.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objWO.sDTCId = dt.Rows[0]["DT_ID"].ToString();
                    objWO.sLocationCode = dt.Rows[0]["DF_LOC_CODE"].ToString();
                    objWO.sCustomerName= dt.Rows[0]["DF_CUSTOMER_NAME"].ToString();
                    objWO.sCustomerNumber = dt.Rows[0]["DF_CUSTOMER_MOBILE"].ToString();
                    objWO.sNumberInsulator = dt.Rows[0]["DF_NUMBER_OF_INSTALLATIONS"].ToString();
                }

                return objWO;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetWOBasicDetails");
                return objWO;
            }
            finally
            {
                
            }
        }


        public clsWorkOrder GetCommDecommAccCode(clsWorkOrder objWO)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                // strQry = "SELECT MD_COMM_ACCCODE,MD_DECOMM_ACCCODE,MD_ENHANCE_ACCCODE FROM TBLMASTERDATA WHERE MD_TYPE='C' AND MD_NAME=:Capacity";
               // Updated by sandeep bcz of comm_acccode has been updated on 05-09-2022
                strQry = "SELECT MD_COMM_ACCCODE,MD_DECOMM_ACCCODE,MD_ENHANCE_ACCCODE,MD_UPDATED_ENHANCE_ACCCODE FROM TBLMASTERDATA WHERE MD_TYPE='C' AND MD_NAME=:Capacity";
                oledbCommand.Parameters.AddWithValue("Capacity", objWO.sCapacity);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objWO.sAccCode = Convert.ToString(dt.Rows[0]["MD_COMM_ACCCODE"]);
                    objWO.sDecomAccCode = Convert.ToString(dt.Rows[0]["MD_DECOMM_ACCCODE"]);
                    objWO.sEnhanceAccCode = Convert.ToString(dt.Rows[0]["MD_ENHANCE_ACCCODE"]);
                    objWO.sGoodEnhanceAccCode = Convert.ToString(dt.Rows[0]["MD_UPDATED_ENHANCE_ACCCODE"]);
                }
                return objWO;
               
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCommDecommAccCode");
                return objWO;
            }
            finally
            {
                
            }
        }

        public clsWorkOrder GetDTCAccCode(clsWorkOrder objWO)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT SCHM_ACCCODE FROM TBLDTCSCHEME WHERE SCHM_ID=:DtcScheme";
                oledbCommand.Parameters.AddWithValue("DtcScheme", objWO.sDtcScheme);
                objWO.sAccCode = ObjCon.get_value(strQry, oledbCommand);
                
                return objWO;
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCAccCode");
                return objWO;
            }
            finally
            {
                
            }
        }

        #region NewDTC
     
        public DataTable LoadNewDTCWO(clsWorkOrder objWorkOrder)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE FROM TBLWORKORDER WHERE WO_DF_ID IS NULL ";
                strQry += " AND WO_OFF_CODE LIKE :OfficeCode||'%' AND WO_REPLACE_FLG='0'";
                oledbCommand.Parameters.AddWithValue("OfficeCode", objWorkOrder.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadNewDTCWO");
                return dt;

            }
            finally
            {
                
            }
        }

        public object GetWODetailsForNewDTC(clsWorkOrder objWorkOrder)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();

                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,WO_AMT,WO_ACC_CODE,WO_ISSUED_BY,WO_NEW_CAP,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CRBY=US_ID) US_FULL_NAME,WO_REQUEST_LOC FROM TBLWORKORDER WHERE ";
                strQry += " WO_SLNO=:WOId ";
                oledbCommand.Parameters.AddWithValue("WOId", objWorkOrder.sWOId);
                dtWODetails = ObjCon.getDataTable(strQry, oledbCommand);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = dtWODetails.Rows[0]["WO_SLNO"].ToString();
                    objWorkOrder.sAccCode = dtWODetails.Rows[0]["WO_ACC_CODE"].ToString();
                    objWorkOrder.sCommWoNo = dtWODetails.Rows[0]["WO_NO"].ToString();
                    objWorkOrder.sCommDate = dtWODetails.Rows[0]["WO_DATE"].ToString();
                    objWorkOrder.sCommAmmount = dtWODetails.Rows[0]["WO_AMT"].ToString();                 
                    objWorkOrder.sIssuedBy = dtWODetails.Rows[0]["WO_ISSUED_BY"].ToString();                   
                    objWorkOrder.sNewCapacity = dtWODetails.Rows[0]["WO_NEW_CAP"].ToString();
                    objWorkOrder.sCrBy = dtWODetails.Rows[0]["US_FULL_NAME"].ToString();
                    objWorkOrder.sRequestLoc = dtWODetails.Rows[0]["WO_REQUEST_LOC"].ToString();
                }
                return objWorkOrder;
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetWODetailsForNewDTC");
                return objWorkOrder;

            }
            finally
            {
                
            }

        }

        #endregion

        public void SendSMStoSectionOfficer(string sFailureId, string sDTCCode,string sWONo,string sDTCName)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbCommand.Parameters.AddWithValue("sFailureId", sFailureId);
                string sOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE WHERE DF_ID=:sFailureId", oledbCommand);
                oledbCommand = new OleDbCommand();
                strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN (4) AND US_OFFICE_CODE=:OfficeCode";
                oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objComm = new clsCommunication();

                    objComm.sSMSkey = "SMStoWorkOrder";
                    objComm = objComm.GetsmsTempalte(objComm);
                    string sSMSText = String.Format(objComm.sSMSTemplate, sDTCCode, sWONo, sDTCName);
                    //objCommunication.sendSMS(sSMSText, sMobileNo, sFullName);
                    objComm.DumpSms(sMobileNo, sSMSText, objComm.sSMSTemplateID);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetMobileNoofSectionStoreOfficer");
                //return ex.Message;

            }
            finally
            {
                
            }
        }


        #region WorkFlow XML

        public clsWorkOrder GetWODetailsFromXML(clsWorkOrder objWorkOrder)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtWODetails = new DataTable();
                if(objWorkOrder.sWFDataId!="" && objWorkOrder.sWFDataId!=null)
                {
                    dtWODetails = objApproval.GetDatatableFromXML(objWorkOrder.sWFDataId);
                }

                if (dtWODetails.Rows.Count > 0)
                {
                   // objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["WO_SLNO"]).Trim();
                    objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_CODE"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["WO_CRBY"]).Trim();
                    objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO"]).Trim();
                    objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE"]).Trim();
                    objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT"]).Trim();
                    objWorkOrder.sDeWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO_DECOM"]).Trim();
                    objWorkOrder.sDeCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_DECOM"]).Trim();
                    objWorkOrder.sDeCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_DECOM"]).Trim();
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["WO_ISSUED_BY"]).Trim();
                    objWorkOrder.sDecomAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACCCODE_DECOM"]).Trim();
                    objWorkOrder.sFailureId = Convert.ToString(dtWODetails.Rows[0]["WO_DF_ID"]).Trim();
                    //objWorkOrder.sFailureDate = Convert.ToString(dtWODetails.Rows[0]["DF_FAILED_DATE"]).Trim();
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["WO_NEW_CAP"]).Trim();
                    objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["WO_REQUEST_LOC"]).Trim();
                    if (dtWODetails.Columns.Contains("WO_DTC_SCHEME"))
                    {
                        objWorkOrder.sDtcScheme = Convert.ToInt32(dtWODetails.Rows[0]["WO_DTC_SCHEME"]);
                    }
                    else
                    {
                        objWorkOrder.sDtcScheme = 0 ;
                    }
                }
                return objWorkOrder;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetWODetailsFromXML");
                return objWorkOrder;
            }
        }

        #endregion


        public ArrayList getCreatedByUserName(string sDataId, string sOffCode)
        {
           
            ArrayList strQrylist = new ArrayList();
            string sWoid = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("DataId", sDataId);
                oledbCommand.Parameters.AddWithValue("OffCode", sOffCode);
                sWoid = ObjCon.get_value("SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='11' AND WO_DATA_ID=:DataId AND WO_OFFICE_CODE LIKE :OffCode||'%'", oledbCommand);
                //dt = ObjCon.getDataTable("SELECT (SELECT US_FULL_NAME  FROM TBLUSER WHERE US_ID=WO_CR_BY) FROM TBLWORKFLOWOBJECTS WHERE  WO_INITIAL_ID ='" + sWoid + "' ORDER BY WO_ID");
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("OffCode1", sOffCode);
                dt = ObjCon.getDataTable("SELECT US_FULL_NAME,(CASE WHEN US_ROLE_ID='7' THEN 1 WHEN US_ROLE_ID ='2' THEN 2 WHEN US_ROLE_ID ='6' THEN 3 ELSE 4 END )SLEVEL  FROM TBLUSER WHERE US_ROLE_ID IN(7,2,6,3) AND US_OFFICE_CODE LIKE :OffCode1||'%' AND US_MMS_ID IS NULL AND US_STATUS='A' ORDER BY SLEVEL", oledbCommand);
                for (int i = 0; i < 4; i++)
                {
                    if (dt.Rows.Count > i)
                    {
                        if (dt.Rows[i][0].ToString() != "" || dt.Rows[i][0].ToString() != null)
                            strQrylist.Add(dt.Rows[i][0].ToString());
                    }
                    else
                        strQrylist.Add("");

                }
                return strQrylist;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getCreatedByUserName");
                return strQrylist;
            }
            finally
            {
                
            }


        }

        public string getWoDataId(string sFailureId)
        {
            oledbCommand = new OleDbCommand();
            string sQry = string.Empty;
            string sWoDataId = string.Empty;
            sQry = "select max(wo_wfo_id) from TBLWORKFLOWOBJECTS where WO_DATA_ID=:FailureId AND WO_BO_ID='11' order by wo_id desc";
            oledbCommand.Parameters.AddWithValue("FailureId", sFailureId);
            sWoDataId = ObjCon.get_value(sQry, oledbCommand);
            return sWoDataId;


        }
        public string getdivcode(clsWorkOrder objWorkOrder)
        {
            oledbCommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CODE ='" + objWorkOrder.sOfficeCode + "'";
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string getBudgetAccountCode()
        {
            oledbCommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT  MD_UPDATED_ENHANCE_ACCCODE  FROM TBLMASTERDATA WHERE MD_TYPE='C' AND MD_NAME='63'";
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string getAvailableBudget(clsWorkOrder objWorkOrder)
        {
            string BudgetAmount = string.Empty;
            string strQry = string.Empty;
            string BudgetOpeningBalance = string.Empty;
            try
            {
                ERPService.ERPServiceClient obj = new ERPService.ERPServiceClient();
                string FormName = "Estimation_Form_dtlms";
                DateTime Date = DateTime.Now;
                string sDate = Date.ToString("dd/MM/yyyy");
                string sPath = "C:\\ERRORLOG\\ERRORLOGBudget" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "ErrorLog.txt";
                File.AppendAllText(sPath, "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + Environment.NewLine + "BEFORE BudgetAmount" + Environment.NewLine + "DivCode=" + objWorkOrder.sDivCode + Environment.NewLine + " AccCode = " + objWorkOrder.sBudgetAccCode + Environment.NewLine + " Date=" + sDate + Environment.NewLine + " Formname = " + FormName + Environment.NewLine);
                string SanctionedAmount = obj.FetchBudgetAmountForAccountCode("FMS", objWorkOrder.sDivCode, objWorkOrder.sBudgetAccCode, sDate, FormName);

                File.AppendAllText(sPath, "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + Environment.NewLine + "AFTER BudgetAmount = " + SanctionedAmount + Environment.NewLine + "###########################################################" + Environment.NewLine);

                objWorkOrder.sBudgetAmount = Convert.ToString(Convert.ToDouble(SanctionedAmount) * 100000);

                string BM_AMOUNT = ObjCon.get_value("SELECT BM_AMOUNT  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objWorkOrder.sBudgetAccCode + "'AND BM_FIN_YEAR = '" + objWorkOrder.sFinancialYear + "'", oledbCommand);
                if (BM_AMOUNT == "")
                {

                    objWorkOrder.sBMID = Convert.ToString(ObjCon.Get_max_no("BM_ID", "TBLBUDGETMAST"));
                    strQry = "INSERT INTO TBLBUDGETMAST (BM_ID,BM_NO,BM_DATE,BM_AMOUNT,BM_ACC_CODE,BM_DIV_CODE,BM_CRON,BM_FIN_YEAR,BM_OB_AMNT,BM_OB_DATE) VALUES('" + objWorkOrder.sBMID + "','" + objWorkOrder.sBudgetAccCode + "',";
                    strQry += " SYSDATE,'" + objWorkOrder.sBudgetAmount + "','" + objWorkOrder.sBudgetAccCode + "','" + objWorkOrder.sDivCode + "',SYSDATE,'" + objWorkOrder.sFinancialYear + "','" + objWorkOrder.sBudgetAmount + "',SYSDATE)";
                    ObjCon.Execute(strQry);
                }
                else
                {
                    string BM_OB_AMNT = ObjCon.get_value("SELECT BM_OB_AMNT  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objWorkOrder.sBudgetAccCode + "'", oledbCommand);
                    string OB_AMT = ObjCon.get_value("SELECT SUM(BT_DEBIT_AMNT)  FROM TBLCOMMISSION_BUDGETTRANS ", oledbCommand);
                    if (OB_AMT != "")
                    {
                         BudgetOpeningBalance = Convert.ToString(Convert.ToDouble(objWorkOrder.sBudgetAmount) - Convert.ToDouble(OB_AMT));
                    }
                    if(BM_OB_AMNT=="")
                    {
                        BM_OB_AMNT = "0";
                    }
   
                  
                    if (Convert.ToDouble(BM_OB_AMNT) <= 0)
                    {
                        strQry = "UPDATE TBLBUDGETMAST SET BM_AMOUNT ='" + objWorkOrder.sBudgetAmount + "',BM_OB_AMNT='" + objWorkOrder.sBudgetAmount + "' WHERE BM_ACC_CODE='" + objWorkOrder.sBudgetAccCode + "'";
                        ObjCon.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLBUDGETMAST SET BM_AMOUNT ='" + objWorkOrder.sBudgetAmount + "',BM_OB_AMNT ='" + BudgetOpeningBalance + "'  WHERE BM_ACC_CODE='" + objWorkOrder.sBudgetAccCode + "'";
                        ObjCon.Execute(strQry);
                    }

                }


                return objWorkOrder.sBudgetAmount;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getAvailableBudget");
                return BudgetAmount;
            }

        }
        public DataTable ViewBudgetstatusgrid(clsWorkOrder objbudget)
        {
            oledbCommand = new OleDbCommand();

            DataTable DtBudgetstatus = new DataTable();
            try
            {

                string strQry = string.Empty;
                string stracccode = string.Empty;

                oledbCommand.Parameters.AddWithValue("accode", objbudget.sBudgetAccCode);
                oledbCommand.Parameters.AddWithValue("divcode", objbudget.sBudgetDivCode);
                oledbCommand.Parameters.AddWithValue("finyr", objbudget.sFinancialYear);
                strQry = "SELECT BT_ID,BT_ACC_CODE,BT_PONO  ,TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE,BT_BM_AMNT,BT_DEBIT_AMNT,BT_AVL_AMNT,BT_CREDIT_AMNT,BT_CRON,BT_FIN_YEAR,BT_DIV_CODE from TBLBUDGETTRANS LEFT JOIN TBLOILSENTMASTER ON BT_PONO=OSD_PO_NO";
                strQry += " WHERE BT_ACC_CODE='" + objbudget.sBudgetAccCode + "' and BT_DIV_CODE='" + objbudget.sBudgetDivCode + "' and BT_FIN_YEAR='" + objbudget.sFinancialYear + "' ORDER BY BT_ID";
                DtBudgetstatus = ObjCon.getDataTable(strQry, oledbCommand);
                return DtBudgetstatus;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtBudgetstatus;
            }
        }

        public string ViewBudgetstatusaval(clsWorkOrder objbudget)
        {
            oledbCommand = new OleDbCommand();
            string Budgetstatusavl = string.Empty;
            try
            {

                string strQry = string.Empty;
                string stracccode = string.Empty;

                stracccode = ObjCon.get_value("SELECT MD_UPDATED_ENHANCE_ACCCODE FROM TBLMASTERDATA WHERE MD_TYPE = 'C'", oledbCommand);

                strQry = "SELECT BT_AVL_AMNT from TBLCOMMISSION_BUDGETTRANS LEFT JOIN TBLWORKORDER ON BT_WONO = WO_NO";
                strQry += " WHERE BT_ACC_CODE='" + objbudget.sBudgetAccCode.Substring(0,6) + "' and BT_DIV_CODE='" + objbudget.sBudgetDivCode + "' and BT_FIN_YEAR='" + objbudget.sFinancialYear + "' ORDER BY BT_ID DESC";
                Budgetstatusavl = ObjCon.get_value(strQry, oledbCommand);
                return Budgetstatusavl;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Budgetstatusavl;
            }
        }
    }
}
