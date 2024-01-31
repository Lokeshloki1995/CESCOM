using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsInvoice
    {
       
        string strFormCode = "clsInvoice";
        public string sInvoiceSlNo { get; set; }
        public string sStoreId { get; set; }
        public string sDtcFailId { get; set; }
        public string sTcSlNo { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sInvoiceDescription { get; set; }
        public string sFailDate { get; set; }
        public string sAmount { get; set; }
              
        public string sCreatedBy { get; set; }
        public string sTcMake { get; set; }
        public string sTcCapacity { get; set; }
        public string sWOSlno { get; set; }
        public string sTcNewCapacity { get; set; }
        public string sIndentId { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentCrby { get; set; }
        public string sIndentDate { get; set; }
        public string sTcCode { get; set; }
        public string sDTCName { get; set; }
        public string sOldTcSlno { get; set; }
        public string sOldTcCode { get; set; }
        public string sTCId { get; set; }
        public string sDTCId { get; set; }
        public string sDTCCODE { get; set; }

        public string sManualInvoiceNo { get; set; }

        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }

        //Gate Pass
        public string sVehicleNumber { get; set; }
        public string sReceiptientName { get; set; }
        public string sChallenNo { get; set; }
        public string sGatePassId { get; set; }
        public string sDTCCode { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        public string[] SaveUpdateInvoiceDetails(clsInvoice objInvoice)
        {
            string[] Arr = new string[2];
            OleDbDataReader dr;
            string strQry = string.Empty;
            try
            {

                oledbCommand = new OleDbCommand();
                //Check Work Order no exists or not
                oledbCommand.Parameters.AddWithValue("IndentNo", objInvoice.sIndentNo.ToUpper());
                dr = ObjCon.Fetch("SELECT TI_INDENT_NO FROM TBLINDENT WHERE UPPER(TI_INDENT_NO)=:IndentNo", oledbCommand);
                if (!dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Enter Valid Indent No";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("TcCode", objInvoice.sTcCode);
                dr = ObjCon.Fetch("SELECT TC_CODE FROM TBLTCMASTER WHERE TC_CODE=:TcCode ", oledbCommand);
                if (!dr.Read())
                {
                    Arr[0] = "Enter Valid DTr Code";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();

                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("TcCode1", objInvoice.sTcCode);
                dr = ObjCon.Fetch("SELECT TC_CODE FROM TBLTCMASTER WHERE TC_CODE=:TcCode1 AND TC_STATUS IN ('1','2') AND TC_CURRENT_LOCATION='1'", oledbCommand);
                if (!dr.Read())
                {
                    Arr[0] = "Entered DTr Code not in Store or Not in good condition";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();

                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("TcCode2", objInvoice.sTcCode);
                oledbCommand.Parameters.AddWithValue("TcNewCapacity", objInvoice.sTcNewCapacity);
                dr = ObjCon.Fetch("SELECT TC_CODE FROM TBLTCMASTER WHERE TC_CODE=:TcCode2 AND TC_CAPACITY=:TcNewCapacity", oledbCommand);
                if (!dr.Read())
                {
                    Arr[0] = "Entered DTr Code Capacity not Matching with Requested Capacity";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();

                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sDTCCODE", objInvoice.sDTCCODE);
                oledbCommand.Parameters.AddWithValue("OldTcCode", objInvoice.sOldTcCode);
                dr = ObjCon.Fetch("SELECT TI_INDENT_NO FROM TBLINDENT,TBLWORKORDER,TBLDTCFAILURE,TBLDTCINVOICE WHERE DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND DF_DTC_CODE=:sDTCCODE AND DF_EQUIPMENT_ID=:OldTcCode", oledbCommand);
                if (dr.Read())
                {
                    Arr[0] = "Invoice Already done for this DTC";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                oledbCommand = new OleDbCommand();
                if (objInvoice.sInvoiceSlNo == "")
                {
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("InvoiceNo", objInvoice.sInvoiceNo.ToUpper());
                    dr = ObjCon.Fetch("select * from TBLDTCINVOICE where  UPPER(IN_INV_NO)=:InvoiceNo ", oledbCommand);
                    if (dr.Read())
                    {

                        Arr[0] = "Invoice No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                    objInvoice.sInvoiceSlNo = Convert.ToString(ObjCon.Get_max_no("IN_NO", "TBLDTCINVOICE"));

                    ObjCon.BeginTrans();
                    oledbCommand = new OleDbCommand();
                    strQry = "INSERT into TBLDTCINVOICE (IN_NO,IN_INV_NO,IN_DATE,IN_DESC,IN_AMT,IN_TI_NO,IN_CRBY,IN_MANUAL_INVNO) ";
                    strQry += "values (:sInvoiceSlNo12,:sInvoiceNo12,TO_DATE(:sInvoiceDate12,'dd/MM/yyyy'),";
                    strQry += " :sInvoiceDescription12,:sAmount12,:sIndentId12,";
                    strQry += ":sCreatedBy12,:sManualInvoiceNo12)";

                    oledbCommand.Parameters.AddWithValue("sInvoiceSlNo12", objInvoice.sInvoiceSlNo);
                    oledbCommand.Parameters.AddWithValue("sInvoiceNo12", objInvoice.sInvoiceNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sInvoiceDate12", objInvoice.sInvoiceDate);
                    oledbCommand.Parameters.AddWithValue("sInvoiceDescription12", objInvoice.sInvoiceDescription);
                    oledbCommand.Parameters.AddWithValue("sAmount12", objInvoice.sAmount);
                    oledbCommand.Parameters.AddWithValue("sIndentId12", objInvoice.sIndentId);
                    oledbCommand.Parameters.AddWithValue("sCreatedBy12", objInvoice.sCreatedBy);
                    oledbCommand.Parameters.AddWithValue("sManualInvoiceNo12", objInvoice.sManualInvoiceNo.ToUpper());
                    ObjCon.Execute(strQry, oledbCommand);

                    oledbCommand = new OleDbCommand();
                    //Insert to new TC Details to TBLTCDRAWN
                    string strMax = Convert.ToString(ObjCon.Get_max_no("TD_ID", "TBLTCDRAWN"));
                    strQry = "INSERT INTO TBLTCDRAWN (TD_ID,TD_DF_ID,TD_TC_NO,TD_INV_NO,TD_DESC,TD_CRON)";
                    strQry += " VALUES (:strMax,:sDtcFailId12,:sTcCode12, ";
                    strQry += " :sInvoiceSlNo123,:sInvoiceDescription123,SYSDATE)";

                    oledbCommand.Parameters.AddWithValue("strMax", strMax);
                    oledbCommand.Parameters.AddWithValue("sDtcFailId12", objInvoice.sDtcFailId);
                    oledbCommand.Parameters.AddWithValue("sTcCode12", objInvoice.sTcCode);
                    oledbCommand.Parameters.AddWithValue("sInvoiceSlNo123", objInvoice.sInvoiceSlNo);
                    oledbCommand.Parameters.AddWithValue("sInvoiceDescription123", objInvoice.sInvoiceDescription);
                   

                    ObjCon.Execute(strQry, oledbCommand);


                    oledbCommand = new OleDbCommand();
                    strQry = " SELECT DF_LOC_CODE  FROM TBLDTCFAILURE WHERE DF_ID=:DtcFailId";

                    oledbCommand.Parameters.AddWithValue("DtcFailId", objInvoice.sDtcFailId);
                    string sFailOfficeCode = ObjCon.get_value(strQry, oledbCommand);

                    //if (objInvoice.sOfficeCode.Length > 1)
                    //{
                    //    objInvoice.sOfficeCode = objInvoice.sOfficeCode.Substring(0, 2);
                    //}

                    //Update status to TCMaster Table
                    oledbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLTCMASTER SET TC_UPDATED_EVENT='Drawn',TC_UPDATED_EVENT_ID=:strMax12,";
                    strQry += " TC_CURRENT_LOCATION=2,TC_LOCATION_ID=:sFailOfficeCode12 WHERE TC_CODE=:sTcCode12";
                    oledbCommand.Parameters.AddWithValue("strMax12", strMax);
                    oledbCommand.Parameters.AddWithValue("sFailOfficeCode12", sFailOfficeCode);
                    oledbCommand.Parameters.AddWithValue("sTcCode12", objInvoice.sTcCode);
                    ObjCon.Execute(strQry, oledbCommand);

                    //ObjCon.Execute("DELETE TBLTCDRAWNBUFFER WHERE BU_FAIL_ID='" + strFailureId + "'");

                    ObjCon.CommitTrans();

                    #region WorkFlow
                   
                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objInvoice.sFormName;
                    objApproval.sRecordId = objInvoice.sInvoiceSlNo;
                    objApproval.sOfficeCode = objInvoice.sOfficeCode;
                    objApproval.sClientIp = objInvoice.sClientIP;
                    objApproval.sCrby = objInvoice.sCreatedBy;
                    objApproval.sWFObjectId = objInvoice.sWFOId;
                    objApproval.sDataReferenceId = objInvoice.sIndentId;
                    objApproval.sWFAutoId = objInvoice.sWFAutoId;

                    objApproval.sDescription = "Invoice Creation for Indent No " + objInvoice.sIndentNo;
                    oledbCommand = new OleDbCommand();
                    if (objInvoice.sTaskType != "3")
                    {

                        
                        oledbCommand.Parameters.AddWithValue("IndentId", objInvoice.sIndentId);
                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT WHERE DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=:IndentId",oledbCommand);
                    }
                    else
                    {
                        oledbCommand.Parameters.AddWithValue("IndentId1", objInvoice.sIndentId);
                        objApproval.sRefOfficeCode = ObjCon.get_value("SELECT WO_REQUEST_LOC FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND TI_ID=:IndentId1",oledbCommand);
                    }


                    bool bResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }
                    objApproval.SaveWorkflowObjects(objApproval);

                    #endregion

                    Arr[0] = "Invoice Created Successfully";
                    Arr[1] = "0";
                    return Arr;
    
                }
                else
                {
                    ObjCon.BeginTrans();
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("InvoiceNo", objInvoice.sInvoiceNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sInvoiceSlNo", objInvoice.sInvoiceSlNo);
                    dr = ObjCon.Fetch("select * from TBLDTCINVOICE where  UPPER(IN_INV_NO)=:InvoiceNo and IN_NO<>:sInvoiceSlNo",oledbCommand);
                    if (dr.Read())
                    {

                        Arr[0] = "Invoice No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();
                    oledbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLDTCINVOICE SET IN_INV_NO=:sInvoiceNo12,";
                    strQry+= " IN_DATE=to_date(:sInvoiceDate12,'dd/MM/yyyy'),IN_DESC=:sInvoiceDescription12,";
                    strQry+= " IN_AMT=:sAmount12 where IN_NO=:sInvoiceSlNo123";

                    oledbCommand.Parameters.AddWithValue("sInvoiceNo12", objInvoice.sInvoiceNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sInvoiceDate12", objInvoice.sInvoiceDate);
                    oledbCommand.Parameters.AddWithValue("sInvoiceDescription12", objInvoice.sInvoiceDescription);
                    oledbCommand.Parameters.AddWithValue("sInvoiceSlNo123", objInvoice.sInvoiceSlNo);
                    ObjCon.Execute(strQry, oledbCommand);

                    ObjCon.CommitTrans();
                    Arr[0] = "Invoice Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateInvoiceDetails");
                return Arr;


            }
        }


        public DataTable LoadAllInvoiceDetails(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT  DT_NAME,TO_CHAR(TC_CODE) TC_CODE ,TI_ID,TI_INDENT_NO,WO_NO,0 AS IN_NO ,'' AS IN_INV_NO ,'NO' AS STATUS  ";
                strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT ";
                strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0  ";
                strQry += " AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND DF_STATUS_FLAG = :sTaskType AND TI_ID NOT IN ";
                strQry += " (SELECT IN_TI_NO FROM TBLDTCINVOICE)   ";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%'";
                strQry += " UNION ALL ";
                strQry += " SELECT  DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TI_ID,TI_INDENT_NO,WO_NO,IN_NO,IN_INV_NO ,'YES' AS STATUS FROM  ";
                strQry += " TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE , TBLWORKORDER, TBLINDENT, TBLDTCINVOICE ";
                strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG =:sTaskType1 ";
                strQry += " AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO  ";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode1||'%'";
                oledbCommand.Parameters.AddWithValue("sTaskType", objInvoice.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objInvoice.sOfficeCode);
                oledbCommand.Parameters.AddWithValue("sTaskType1", objInvoice.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode1", objInvoice.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllInvoiceDetails");
                return dt;
            }
        }


        public DataTable LoadExistingInvoice(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = "SELECT  DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TI_ID ,TI_INDENT_NO , WO_NO, IN_NO, IN_INV_NO, 'YES' AS STATUS  FROM TBLDTCMAST,";
                strQry += " TBLTCMASTER, TBLDTCFAILURE , TBLWORKORDER, TBLINDENT, TBLDTCINVOICE";
                strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG =:sTaskType ";
                strQry += " AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO  ";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%'";

                oledbCommand.Parameters.AddWithValue("sTaskType", objInvoice.sTaskType);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objInvoice.sOfficeCode);
                dt = ObjCon.getDataTable(strQry,oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadExistingInvoice");
                return dt;
            }
        }

        public clsInvoice GetTCDetails(clsInvoice objInvoice)
        {
            try
            {
                //oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //strQry = "SELECT TC_CODE FROM TBLTCMASTER WHERE TC_LOCATION_ID LIKE :sOfficeCode||'%' AND TC_STATUS in (1,2) AND ";
                //strQry += " TC_CURRENT_LOCATION =1 AND TC_CAPACITY=:sTcCapacity AND TC_CODE=:sTcCode";
                //oledbCommand.Parameters.AddWithValue("sOfficeCode", objInvoice.sOfficeCode);
                //oledbCommand.Parameters.AddWithValue("sTcCapacity", objInvoice.sTcCapacity);
                //oledbCommand.Parameters.AddWithValue("sTcCode", objInvoice.sTcCode);
                //string res = ObjCon.get_value(strQry, oledbCommand);

                //if (res != "")
                //{
                    oledbCommand = new OleDbCommand();

                    strQry = "SELECT TC_SLNO,TC_CODE,TM_NAME,TM_ID,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES  ";
                    strQry += " WHERE TC_MAKE_ID= TM_ID and TC_CODE=:sTcCode";
                    oledbCommand.Parameters.AddWithValue("sTcCode", objInvoice.sTcCode);
                    dt = ObjCon.getDataTable(strQry, oledbCommand);
                    if (dt.Rows.Count > 0)
                    {
                        objInvoice.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                        objInvoice.sTcSlNo = dt.Rows[0]["TC_SLNO"].ToString();
                        objInvoice.sTcMake = dt.Rows[0]["TM_ID"].ToString() + "~" + dt.Rows[0]["TM_NAME"].ToString();
                        objInvoice.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    }

                //}
                //else
                //{
                //    objInvoice.sTcCode = "";
                //}
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTCDetails");
                return objInvoice;
            }
        }


        public object GetBasicDetails(clsInvoice objInvoice)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT DF_ID,DT_NAME,DT_CODE, TI_ID, TO_CHAR(TI_INDENT_DATE,'DD/MM/YYYY') AS INDENTDDATE ,TI_INDENT_NO, ";
                strQry += "(SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID = TI_CRBY) USERNAME, WO_DTC_CAP, WO_NEW_CAP ,WO_AMT,";
                strQry += "(SELECT TC_SLNO FROM TBLTCMASTER WHERE TC_CODE = DF_EQUIPMENT_ID) TC_SLNO, ";
                strQry += " DF_EQUIPMENT_ID AS TC_CODE,DT_ID,(SELECT TC_ID FROM TBLTCMASTER WHERE DF_EQUIPMENT_ID=TC_CODE) TC_ID, TO_CHAR(DF_DATE,'DD/MM/YYYY') AS DF_DATE ";
                strQry += " from TBLDTCMAST,TBLDTCFAILURE, TBLWORKORDER, TBLINDENT WHERE DF_DTC_CODE= DT_CODE  AND  ";
                strQry += "WO_DF_ID = DF_ID AND TI_WO_SLNO = WO_SLNO AND TI_ID =:sIndentId ";
                oledbCommand.Parameters.AddWithValue("sIndentId", objInvoice.sIndentId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objInvoice.sDTCName = dt.Rows[0]["DT_NAME"].ToString(); ;
                    objInvoice.sIndentDate = dt.Rows[0]["INDENTDDATE"].ToString(); 
                    objInvoice.sIndentCrby = dt.Rows[0]["USERNAME"].ToString(); 
                    objInvoice.sDtcFailId = dt.Rows[0]["DF_ID"].ToString(); 
                    objInvoice.sTcNewCapacity = dt.Rows[0]["WO_NEW_CAP"].ToString(); 
                    objInvoice.sTcCapacity = dt.Rows[0]["WO_DTC_CAP"].ToString(); 
                    objInvoice.sOldTcSlno = dt.Rows[0]["TC_SLNO"].ToString(); 
                    objInvoice.sIndentNo = dt.Rows[0]["TI_INDENT_NO"].ToString();
                    objInvoice.sOldTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objInvoice.sDTCId = dt.Rows[0]["DT_ID"].ToString();
                    objInvoice.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objInvoice.sAmount = dt.Rows[0]["WO_AMT"].ToString();
                    objInvoice.sFailDate = dt.Rows[0]["DF_DATE"].ToString();
                  
                    
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetIndentDetails");
                return objInvoice;
            }
        }

        public object GetInvoiceDetails(clsInvoice objInvoice)
        {

            try
            {
                oledbCommand = new OleDbCommand();
                
                DataTable dtInvoiceDetails = new DataTable();
                string strQry = string.Empty;

                strQry = " SELECT IN_NO,IN_INV_NO,IN_TI_NO,IN_AMT,TO_CHAR(IN_DATE,'dd/MM/yyyy')IN_DATE,IN_DESC,TC_SLNO,TM_NAME,TC_CODE,";
                strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,IN_MANUAL_INVNO FROM TBLDTCINVOICE,TBLTCMASTER,TBLTCDRAWN,TBLTRANSMAKES ";
                strQry += " WHERE TD_TC_NO= TC_CODE AND TC_MAKE_ID= TM_ID AND IN_NO=:sInvoiceSlNo AND IN_NO=TD_INV_NO";
                oledbCommand.Parameters.AddWithValue("sInvoiceSlNo", objInvoice.sInvoiceSlNo);
                dtInvoiceDetails = ObjCon.getDataTable(strQry, oledbCommand);
                if (dtInvoiceDetails.Rows.Count > 0)
                {
                    objInvoice.sInvoiceNo = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_INV_NO"]);
                    objInvoice.sInvoiceDate = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DATE"]);
                    objInvoice.sAmount = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_AMT"]);
                    objInvoice.sInvoiceDescription = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DESC"]);

                    objInvoice.sTcCode = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CODE"]);
                    objInvoice.sTcSlNo = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_SLNO"]);
                    objInvoice.sTcMake = Convert.ToString(dtInvoiceDetails.Rows[0]["TM_NAME"]);
                    objInvoice.sTcCapacity = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CAPACITY"]);
                    objInvoice.sManualInvoiceNo = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_MANUAL_INVNO"]);
                }

                   
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadInvoiceDetails");
                return objInvoice;

            }
        }
        public bool ValidateUpdate(string sInvoiceId)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                DataTable dt = new DataTable();
                OleDbDataReader dr;
                oledbCommand.Parameters.AddWithValue("sInvoiceId", sInvoiceId);
                dr = ObjCon.Fetch("select IN_TI_NO from TBLDTCINVOICE,TBLTCREPLACE,TBLINDENT WHERE TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND IN_NO=:sInvoiceId", oledbCommand);
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
        }


        public string GenerateInvoiceNo(string sOfficeCode)
        {
            try
            {
                
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;

                if (sOfficeCode.Length > 2)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sInvoiceNo = ObjCon.get_value("SELECT NVL(MAX(INV_NO),0)+1 FROM VIEWINVOICE WHERE LOCCODE =:sOfficeCode ", oledbCommand);

                if (sInvoiceNo.Length==1 )
                {

                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy"); 
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }

                    sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                        {
                            return sInvoiceNo;
                        }
                        else
                        {
                            sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sInvoiceNo;
                    }
                    


                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GenerateInvoiceNo");
                return "";
            }
        }



        public string Generate_MMS_InvoiceNo(string sOfficeCode)
        {
            try
            {
                DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                //string InvoiceNum = objWcf.GenerateInvoiceNo(sOfficeCode);
                //return InvoiceNum;
                return "";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
                return "";
            }
        }
        #region NewDTC

        public DataTable LoadAllNewDTCInvoice(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
              
                string strQry = string.Empty;
                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,";
                strQry += " 'YES' AS STATUS,IN_INV_NO, TO_CHAR(IN_DATE,'DD-MON-YYYY') IN_DATE,IN_NO  FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND ";
                strQry += " WO_DF_ID IS NULL AND WO_OFF_CODE LIKE :sOfficeCode||'%' AND TI_ID=IN_TI_NO ";
                strQry += " UNION ALL ";
                strQry += " SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,";
                strQry += " 'NO' AS STATUS,'' AS IN_INV_NO,'' AS IN_DATE,0 AS IN_NO   FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND WO_DF_ID IS NULL ";
                strQry += " AND WO_OFF_CODE LIKE :sOfficeCode1||'%' AND TI_ID NOT IN (SELECT IN_TI_NO FROM TBLDTCINVOICE)";

                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                oledbCommand.Parameters.AddWithValue("sOfficeCode1", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllNewDTCInvoice");
                return dt;
            }
        }

        public DataTable LoadAlreadyNewDTCInvoice(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,";
                strQry += " 'YES' AS STATUS,IN_INV_NO, TO_CHAR(IN_DATE,'DD-MON-YYYY') IN_DATE,IN_NO  FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND ";
                strQry += " WO_DF_ID IS NULL AND WO_OFF_CODE LIKE :sOfficeCode||'%' AND TI_ID=IN_TI_NO ";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objInvoice.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAlreadyNewDTCInvoice");
                return dt;
            }
        }
        #endregion


        public clsInvoice GetGatePassDetials(clsInvoice objInvoice)
        {
            try
            {
                
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbCommand = new OleDbCommand();
                strQry = "SELECT GP_VEHICLE_NO,GP_RECIEPIENT_NAME,GP_CHALLEN_NO FROM TBLGATEPASS WHERE GP_IN_NO=:sInvoiceNo";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objInvoice.sInvoiceNo);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objInvoice.sChallenNo = Convert.ToString(dt.Rows[0]["GP_CHALLEN_NO"]);
                    objInvoice.sVehicleNumber = Convert.ToString(dt.Rows[0]["GP_VEHICLE_NO"]);
                    objInvoice.sReceiptientName = Convert.ToString(dt.Rows[0]["GP_RECIEPIENT_NAME"]);
                }

                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetGatePassDetials");
                return objInvoice;
            }
        }




        public string[] SaveUpdateGatePassDetails(clsInvoice objInvoice)
        {
            string[] Arr = new string[2];
            OleDbDataReader dr;
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sChallenNo", objInvoice.sChallenNo.ToUpper());
                oledbCommand.Parameters.AddWithValue("sGatePassId", objInvoice.sGatePassId);
                dr = ObjCon.Fetch("select * from TBLGATEPASS where  UPPER(GP_CHALLEN_NO)=:sChallenNo and GP_ID=:sGatePassId",oledbCommand);
                if (dr.Read())
                {
                    oledbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLGATEPASS SET GP_VEHICLE_NO=:sVehicleNumber,";
                    strQry += "GP_RECIEPIENT_NAME=:sReceiptientName,GP_CHALLEN_NO=:sChallenNo where GP_ID=:sGatePassId";

                    oledbCommand.Parameters.AddWithValue("sVehicleNumber", objInvoice.sVehicleNumber);
                    oledbCommand.Parameters.AddWithValue("sReceiptientName", objInvoice.sReceiptientName.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sChallenNo", objInvoice.sChallenNo.ToUpper());
                    oledbCommand.Parameters.AddWithValue("sGatePassId", objInvoice.sGatePassId);

                    ObjCon.Execute(strQry, oledbCommand);
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
                else
                {
                    if (objInvoice.sGatePassId == "")
                    {
                        objInvoice.sGatePassId = Convert.ToString(ObjCon.Get_max_no("GP_ID", "TBLGATEPASS"));

                        oledbCommand = new OleDbCommand();
                        strQry = "insert into TBLGATEPASS (GP_ID,GP_IN_NO,GP_TC_CODE,GP_DT_CODE,GP_RECIEPIENT_NAME,GP_VEHICLE_NO,GP_CHALLEN_NO,GP_CRBY,GP_CRON) ";
                        strQry += "values (:sGatePassId,:sInvoiceNo13,:sTcCode13,:sDTCCode23,";
                        strQry += " :sReceiptientName23,:sVehicleNumber23,:sChallenNo23,";
                        strQry += ":sCreatedBy34,SYSDATE)";

                        oledbCommand.Parameters.AddWithValue("sGatePassId", objInvoice.sGatePassId);
                        oledbCommand.Parameters.AddWithValue("sInvoiceNo13", objInvoice.sInvoiceNo);
                        oledbCommand.Parameters.AddWithValue("sTcCode13", objInvoice.sTcCode);
                        oledbCommand.Parameters.AddWithValue("sDTCCode23", objInvoice.sDTCCode);

                        oledbCommand.Parameters.AddWithValue("sReceiptientName23", objInvoice.sReceiptientName.ToUpper());
                        oledbCommand.Parameters.AddWithValue("sVehicleNumber23", objInvoice.sVehicleNumber.ToUpper());
                        oledbCommand.Parameters.AddWithValue("sChallenNo23", objInvoice.sChallenNo.ToUpper());
                        oledbCommand.Parameters.AddWithValue("sCreatedBy34", objInvoice.sCreatedBy);
                        ObjCon.Execute(strQry, oledbCommand);

                        Arr[0] = "Saved Successfully";
                        Arr[1] = "0";
                        return Arr;


                    }
                }
                return Arr;
                //else
                //{
                //    strQry = "UPDATE TBLGATEPASS SET GP_VEHICLE_NO='" + objInvoice.sVehicleNumber + "',";
                //    strQry += "GP_RECIEPIENT_NAME='" + objInvoice.sReceiptientName.ToUpper() + "',GP_CHALLEN_NO='" + objInvoice.sChallenNo.ToUpper() + "' where GP_ID='" + objInvoice.sGatePassId + "'";
                //    ObjCon.Execute(strQry);
                //    Arr[0] = "Updated Successfully";
                //    Arr[1] = "1";
                //    return Arr;
                //}
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveFunction");
                return Arr;


            }
        }

        #region WorkFlow XML

        public clsInvoice GetInvoiceDetailsFromXML(clsInvoice objInvoice)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtInvoiceDetails = new DataTable();
                if(objInvoice.sWFDataId!="" && objInvoice.sWFDataId!=null)
                {
                    dtInvoiceDetails = objApproval.GetDatatableFromXML(objInvoice.sWFDataId);
                }
        
                if (dtInvoiceDetails.Rows.Count > 0)
                {
                    objInvoice.sInvoiceNo = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_INV_NO"]);
                    objInvoice.sInvoiceDate = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DATE"]);
                    objInvoice.sAmount = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_AMT"]);
                    objInvoice.sInvoiceDescription = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DESC"]);

                    objInvoice.sTcCode = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CODE"]);
                    objInvoice.sTcSlNo = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_SLNO"]);
                    objInvoice.sTcMake = Convert.ToString(dtInvoiceDetails.Rows[0]["TM_NAME"]);
                    objInvoice.sTcCapacity = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CAPACITY"]);
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoiceDetailsFromXML");
                return objInvoice;
            }
        }

        #endregion

    }
}
