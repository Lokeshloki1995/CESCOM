using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;
namespace IIITS.DTLMS.BL
{
    public class clsIndent
    {
       
        string strFormCode = "clsIndent";
        public string sDtcFailId { get; set; }
        public string sIndentId { get; set; }
        public string sFailDate { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sStoreName { get; set; }
        public string sIndentDescription { get; set; }
        public string sWOSlno { get; set; }
        public string sCrBy { get; set; }
        public string sAlertFlg { get; set; }
        public string sWoNo { get; set; }
        public string sTasktype { get; set; }
        public string sOfficeCode { get; set; }
        public string sRequstedCapacity { get; set; }
        public string sFailureId { get; set; }

        public string sDTCCode { get; set; }

        public string sCrOn { get; set; }
        public string sDTCName { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }

        public string sApprovalDesc { get; set; }
        public string sCustomername { get; set; }
        public string sCustomerNumber { get; set; }
        public string sNumberofInstalment { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        public DataTable LoadAlreadyIndent(clsIndent objIndent)
        {
            oledbCommand = new OleDbCommand();
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(DF_ID) DF_ID ,WO_NO,DF_DTC_CODE,'YES' AS STATUS,TI_ID,WO_SLNO,TI_INDENT_NO FROM TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE,TBLWORKORDER, ";
                strQry += " TBLINDENT WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0";
                strQry += "  AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND DF_STATUS_FLAG=:Tasktype";
                strQry += " AND DF_LOC_CODE LIKE :OfficeCode||'%' ";
                oledbCommand.Parameters.AddWithValue("Tasktype", objIndent.sTasktype);
                oledbCommand.Parameters.AddWithValue("OfficeCode", objIndent.sOfficeCode);
                dtIndentDetails = ObjCon.getDataTable(strQry, oledbCommand);

                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " LoadIndentGrid");
                return dtIndentDetails;
            }
            finally
            {
                
            }
        }

        public DataTable LoadAllIndent(clsIndent objIndent)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(DF_ID) DF_ID,WO_NO,DF_DTC_CODE,'YES' AS STATUS,TI_ID,WO_SLNO,TI_INDENT_NO FROM TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE,";
                strQry += " TBLWORKORDER, TBLINDENT WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_ID =  WO_DF_ID ";
                strQry += " AND WO_SLNO = TI_WO_SLNO AND DF_STATUS_FLAG=:Tasktype ";
                strQry += " AND DF_LOC_CODE LIKE :OfficeCode||'%'";
                strQry += " UNION ALL ";
                strQry += " SELECT DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(DF_ID) DF_ID,WO_NO,DF_DTC_CODE,'NO' AS STATUS,0 AS TI_ID,WO_SLNO,'' AS TI_INDENT_NO FROM TBLDTCMAST,TBLTCMASTER, ";
                strQry += " TBLDTCFAILURE , TBLWORKORDER WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 ";
                strQry += " AND DF_ID =  WO_DF_ID AND  DF_STATUS_FLAG=:Tasktype1 AND WO_SLNO NOT IN (SELECT TI_WO_SLNO FROM TBLINDENT)";
                strQry += " AND DF_LOC_CODE LIKE :OfficeCode1||'%' ";

                oledbCommand.Parameters.AddWithValue("Tasktype", objIndent.sTasktype);
                oledbCommand.Parameters.AddWithValue("OfficeCode", objIndent.sOfficeCode);
                oledbCommand.Parameters.AddWithValue("Tasktype1", objIndent.sTasktype);
                oledbCommand.Parameters.AddWithValue("OfficeCode1", objIndent.sOfficeCode);
                dt = ObjCon.getDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllIndent");
                return dt;
            }
            finally
            {
                
            }
        }

        public string[] SaveUpdateIndentDetails(clsIndent objIndent)
        {
            string[] Arr = new string[2];
            OleDbDataReader dr;
            string strQry = string.Empty;
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);

            try
            {
                oledbCommand = new OleDbCommand();
                //Check Work Order no exists or not
                oledbCommand.Parameters.AddWithValue("WoNo", objIndent.sWoNo.ToUpper());
                dr = ObjCon.Fetch("SELECT WO_NO FROM TBLWORKORDER WHERE UPPER(WO_NO)=:WoNo", oledbCommand);
                if (!dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Enter Valid Work Order No";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();

                if (objIndent.sIndentId =="")
                {
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("IndentNo", objIndent.sIndentNo.ToUpper());
                    dr = ObjCon.Fetch("select * from TBLINDENT where  UPPER(TI_INDENT_NO)=:IndentNo", oledbCommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Indent No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                    objIndent.sIndentId = Convert.ToString(ObjCon.Get_max_no("TI_ID", "TBLINDENT"));

                    //ObjCon.BeginTrans();

                    //strQry = "INSERT into TBLINDENT (TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_STORE_ID,TI_DESC,TI_WO_SLNO,TI_CRBY,TI_CRON,TI_ALERT_FLAG) ";
                    //strQry += "values ('" + objIndent.sIndentId + "','" + objIndent.sIndentNo.ToUpper() + "',TO_DATE('" + objIndent.sIndentDate+ "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription+ "'";
                    //strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',SYSDATE,'" + objIndent.sAlertFlg + "')";
                    ////ObjCon.Execute(strQry);

                    //ObjCon.CommitTrans();

                    //Workflow / Approval
                    #region Workflow

                    strQry = "INSERT into TBLINDENT (TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_STORE_ID,TI_DESC,TI_WO_SLNO,TI_CRBY,TI_CRON,TI_ALERT_FLAG) ";
                    strQry += "values ('{0}',(SELECT INDENTNUMBER('" + objIndent.sWOSlno + "','" + objIndent.sFailureId + "') FROM DUAL),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                    strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',SYSDATE,'" + objIndent.sAlertFlg + "')";

                    strQry = strQry.Replace("'", "''");

                    string sParam = "SELECT NVL(MAX(TI_ID),0)+1 FROM TBLINDENT";

                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objIndent.sFormName;
                    //objApproval.sRecordId = objIndent.sIndentId;
                    objApproval.sOfficeCode = objIndent.sOfficeCode;
                    objApproval.sClientIp = objIndent.sClientIP;
                    objApproval.sCrby = objIndent.sCrBy;
                    objApproval.sWFObjectId = objIndent.sWFOId;
                    objApproval.sWFAutoId = objIndent.sWFAutoId;

                    objApproval.sQryValues = strQry ;
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLINDENT";
                    objApproval.sDataReferenceId = objIndent.sWOSlno;
                    oledbCommand = new OleDbCommand();
                    if (objIndent.sTasktype != "3")
                    {
                        
                        oledbCommand.Parameters.AddWithValue("WOSlno", objIndent.sWOSlno);

                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE,TBLWORKORDER WHERE DF_ID=WO_DF_ID AND WO_SLNO=:WOSlno", oledbCommand);
                        objApproval.sDescription = "Indent pertaining to DTC Code - " + objIndent.sDTCCode + ", DTC Name - " + objIndent.sDTCName + " for Work Order No " + objIndent.sWoNo;
                    }
                    else
                    {
                        oledbCommand.Parameters.AddWithValue("WOSlno1", objIndent.sWOSlno);
                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT WO_REQUEST_LOC FROM TBLWORKORDER WHERE WO_SLNO=:WOSlno1",oledbCommand);
                        objApproval.sDescription = "Indent pertaining to Work Order No " + objIndent.sWoNo;
                    }

                    string sPrimaryKey = "{0}";

                    objApproval.sColumnNames = "TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_STORE_ID,TI_DESC,TI_WO_SLNO,TI_CRBY,TI_CRON,TI_ALERT_FLAG";


                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objIndent.sIndentNo.ToUpper() + "," + objIndent.sIndentDate + ",";
                    objApproval.sColumnValues += "" + objIndent.sStoreName + "," + objIndent.sIndentDescription + ",";
                    objApproval.sColumnValues += "" + objIndent.sWOSlno + "," + objIndent.sCrBy + ",SYSDATE," + objIndent.sAlertFlg + "";

                    objApproval.sTableNames = "TBLINDENT";


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objIndent.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                        objIndent.sWFDataId = objApproval.sWFDataId;

                    }
                    else
                    {
                        objconn.BeginTrans();
                        objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlowlatest(objconn);
                        objApproval.SaveWorkflowObjectslatest(objApproval, objconn);
                    }

                    #endregion

                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    ObjCon.BeginTrans();
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("IndentNo1", objIndent.sIndentNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("IndentId", objIndent.sIndentId);
                    dr = ObjCon.Fetch("select * from TBLINDENT where  UPPER(TI_INDENT_NO)=:IndentNo1 and TI_ID<>:IndentId", oledbCommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Indent No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                    strQry = "UPDATE TBLINDENT SET TI_INDENT_NO='" + objIndent.sIndentNo + "',TI_INDENT_DATE=TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),";
                    strQry += " TI_DESC='" + objIndent.sIndentDescription + "',TI_STORE_ID='" + objIndent.sStoreName + "',TI_ALERT_FLAG='" + objIndent.sAlertFlg + "' ";
                    strQry+= " where TI_ID='" + objIndent.sIndentId + "'";
                    ObjCon.Execute(strQry);

                    ObjCon.CommitTrans();
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
               // ObjCon.RollBack();
                objconn.RollBack();

                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateIndentDetails");
                // return Arr;
                throw ex;

            }
            finally
            {
                
            }
        }


        
        public object GetIndentDetails(clsIndent objIndent)
        {

            try
            {
                oledbCommand = new OleDbCommand();
                
                DataTable dtIndentDetails = new DataTable();
                string strQry = string.Empty;

                strQry = "SELECT TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy') TI_INDENT_DATE,";
                strQry += " (SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) CAPACITY,TO_CHAR(TI_CRON,'dd/MM/yyyy') TI_CRON,";
                strQry += " TI_STORE_ID,TI_DESC,(SELECT US_FULL_NAME FROM TBLUSER WHERE TI_CRBY=US_ID) US_FULL_NAME  FROM TBLINDENT WHERE  TI_ID=:IndentId ";
                oledbCommand.Parameters.AddWithValue("IndentId", objIndent.sIndentId);
               // oledbCommand.Parameters.AddWithValue("IndentId", objIndent.sIndentId);
                dtIndentDetails = ObjCon.getDataTable(strQry, oledbCommand);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_NO"]);
                    objIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_DATE"]);
                    objIndent.sStoreName = Convert.ToString(dtIndentDetails.Rows[0]["TI_STORE_ID"]);
                    objIndent.sIndentDescription = Convert.ToString(dtIndentDetails.Rows[0]["TI_DESC"]).Replace("ç", ",");
                    objIndent.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["TI_ID"]);
                    objIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["US_FULL_NAME"]);
                    objIndent.sRequstedCapacity = Convert.ToString(dtIndentDetails.Rows[0]["CAPACITY"]);
                    objIndent.sCrOn = Convert.ToString(dtIndentDetails.Rows[0]["TI_CRON"]);
                }
                    
                   
                return objIndent;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetIndentDetails");
                return objIndent;

            }
            finally
            {
                
            }
        }
        public bool ValidateUpdate(string sIndentId)
        {
            try
            {

                oledbCommand = new OleDbCommand();
                DataTable dt = new DataTable();
                OleDbDataReader dr;
                oledbCommand.Parameters.AddWithValue("IndentId", sIndentId);
                dr = ObjCon.Fetch("select IN_INV_NO from TBLINDENT,TBLDTCINVOICE WHERE IN_TI_NO=TI_ID AND TI_ID=:IndentId", oledbCommand);
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
                return false ;

            }
            finally
            {
                
            }
        }

        public string GetTransformerCount(string sOfficeCode,string sWOslno)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                if (sOfficeCode.Length > 1)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }
                //strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID IN (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_OFF_CODE='" + sOfficeCode + "') ";
                //strQry += " AND  TC_CAPACITY IN (SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO='" + sWOslno + "') AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1";


                strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_LOCATION_ID=:OfficeCode ";
                strQry += " AND  TC_CAPACITY IN (SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO=:WOslno) AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1";
                oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                oledbCommand.Parameters.AddWithValue("WOslno", sWOslno);
                return ObjCon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTransformerCount");
                return "";
            }
            finally
            {
                
            }
        }


        public string GenerateIndentNo(string sOfficeCode)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                string sIndentNo = ObjCon.get_value("SELECT  NVL(MAX(TI_INDENT_NO),0)+1 FROM TBLINDENT WHERE TI_INDENT_NO LIKE:OfficeCode||'%' ", oledbCommand);
                if (sIndentNo.Length == 1)
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

                    sIndentNo = sOfficeCode + sFinancialYear + "00001";
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
                        if (sFinancialYear == sIndentNo.Substring(4, 2))
                        {
                            return sIndentNo;
                        }
                        else
                        {
                            sIndentNo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sIndentNo;
                    }

                   
                }

                return sIndentNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GenerateIndentNo");
                return "";
            }
            finally
            {
                
            }
        }

        #region NewDTC
        public DataTable LoadAllNewDTCIndent(clsIndent objIndent)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                
                string strQry = string.Empty;
                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,'YES' AS STATUS ";
                strQry += " FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND WO_DF_ID IS NULL AND WO_REQUEST_LOC LIKE :OfficeCode||'%'";
                strQry += " UNION ALL ";
                strQry += " SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE,0 AS TI_ID,'' AS TI_INDENT_NO,'' AS TI_INDENT_DATE,'NO' AS STATUS FROM TBLWORKORDER ";
                strQry += " WHERE WO_REPLACE_FLG='0'  AND WO_DF_ID IS NULL AND WO_REQUEST_LOC LIKE :OfficeCode1||'%'";
                strQry += " AND  WO_SLNO NOT IN (SELECT TI_WO_SLNO FROM TBLINDENT)";
                oledbCommand.Parameters.AddWithValue("OfficeCode", objIndent.sOfficeCode);
                oledbCommand.Parameters.AddWithValue("OfficeCode1", objIndent.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAlreadyNewDTCIndent");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadAlreadyNewDTCIndent(clsIndent objIndent)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = " SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,'YES' AS STATUS ";
                strQry += " FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND WO_DF_ID IS NULL ";
                strQry += "  AND WO_OFF_CODE LIKE :OfficeCode||'%'";
                oledbCommand.Parameters.AddWithValue("OfficeCode", objIndent.sOfficeCode.Substring(0, 2));
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAlreadyNewDTCIndent");
                return dt;
            }
            finally
            {
                
            }
        }
        #endregion


        #region WorkFlow XML

        public clsIndent GetIndentDetailsFromXML(clsIndent objIndent)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtIndentDetails = new DataTable();
                if(objIndent.sWFDataId!="" && objIndent.sWFDataId!=null)
                {
                    dtIndentDetails = objApproval.GetDatatableFromXML(objIndent.sWFDataId);
                }

                if (dtIndentDetails.Rows.Count > 0)
                {
                    objIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_NO"]);
                    objIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_DATE"]);
                    objIndent.sStoreName = Convert.ToString(dtIndentDetails.Rows[0]["TI_STORE_ID"]);
                    objIndent.sIndentDescription = Convert.ToString(dtIndentDetails.Rows[0]["TI_DESC"]).Replace("ç", ",");
                    objIndent.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["TI_ID"]);
                    objIndent.sWOSlno = Convert.ToString(dtIndentDetails.Rows[0]["TI_WO_SLNO"]);
                    objIndent.sCrOn = ObjCon.get_value("SELECT TO_CHAR("+ dtIndentDetails.Rows[0]["TI_CRON"] +",'DD/MM/YYYY') FROM DUAL");
                    //objIndent.sCrOn =Convert.ToDateTime(Convert.ToString(dtIndentDetails.Rows[0]["TI_CRON"])).ToString("dd/MM/yyyy");
                    objIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["TI_CRBY"]);
                    //objIndent.sCrBy = ObjCon.get_value("SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID='"+ sCrby  +"'");
                    //objIndent.sRequstedCapacity = ObjCon.get_value("SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO='" + objIndent.sWOSlno + "'"); 
                }
                return objIndent;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetIndentDetailsFromXML");
                return objIndent;
            }
        }

        #endregion


        public bool CheckIndentCreation3DaysExceeds(string sIndentCreatedDate)
        {
            try
            {
                
                string strQry = string.Empty;
                string sResult = string.Empty;
                if (sIndentCreatedDate == "")
                {
                    return false;
                }
                DateTime dIndentDate = DateTime.ParseExact(sIndentCreatedDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                strQry = "SELECT ((TO_CHAR(sysdate,'YYYYMMDD') - '" + dIndentDate.ToString("yyyyMMdd") + "')) FROM DUAL";
                sResult = ObjCon.get_value(strQry);
                if (Convert.ToInt32(sResult) > 3)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message + " " + sIndentCreatedDate, strFormCode, "CheckIndentCreation3DaysExceeds");
                return false;
            }
            finally
            {
                
            }
        }
    }
}
