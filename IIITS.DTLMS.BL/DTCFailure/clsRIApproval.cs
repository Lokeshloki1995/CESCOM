using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{

    public class clsRIApproval
    {

        string strFormCode = "clsRIApproval";
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
        public string sDecommDate { get; set; }
        public string sManualRVACKNo { get; set; }

        public string sTasktype { get; set; }
        public string sOfficeCode { get; set; }
        public string sWorkOrderDate { get; set; }
        public string sComWorkOrder { get; set; }
        public string sDecomWorkOrder { get; set; }
        public string sManInvNumber { get; set; }
        public string sInvDate { get; set; }



        //RI
        public string sTCCode { get; set; }
        public string sTcSlno { get; set; }
        public string sTcMake { get; set; }
        public string sDate { get; set; }
        public string sOilQuantity { get; set; }
        public string sDTCCode { get; set; }
        public string sDTCId { get; set; }
        public string sFailureTCId { get; set; }
        public string sNewTCId { get; set; }
        public string sNewTCCode { get; set; }
        public string sTCId { get; set; }
        public string sOilQuantitySK { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sWFObjectId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }

        //CR
        public string sCommentByStoreKeeper { get; set; }
        public string sCommentByStoreOfficer { get; set; }
        public string sStoreKeeperName { get; set; }
        public string sStoreOfficerName { get; set; }
        public string sApprovedDate { get; set; }
        public string sInventoryQty { get; set; }
        public string sDecommInventoryQty { get; set; }
        public string sCrDate { get; set; }


        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        public DataTable LoadAlreadyRI(clsRIApproval objRIApp)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "SELECT DF_ID,DT_NAME,TR_ID,DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,IN_NO,TR_ID,'YES' AS STATUS,DT_TC_ID FROM TBLDTCMAST,TBLDTCFAILURE,";
                strQry += " TBLWORKORDER,TBLINDENT, TBLDTCINVOICE,TBLTCREPLACE";
                strQry += " WHERE DT_CODE = DF_DTC_CODE  AND DF_ID = WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND ";
                strQry += " DF_STATUS_FLAG=:sTasktype AND TR_IN_NO=IN_NO AND TR_APPROVE_FLAG=1";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%'";
                oledbCommand.Parameters.AddWithValue("sTasktype", objRIApp.sTasktype);
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objRIApp.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDecommforApproval");
                return dt;
            }
            finally
            {

            }
        }

        public string[] UpdateReplaceDetails(clsRIApproval objReplace)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            try
            {

                oledbCommand = new OleDbCommand();
                strQry = " SELECT DF_EQUIPMENT_ID  FROM TBLDTCFAILURE WHERE DF_ID=:sFailureId";
                oledbCommand.Parameters.AddWithValue("sFailureId", objReplace.sFailureId);
                string sTCCode = ObjCon.get_value(strQry, oledbCommand);



                //Workflow / Approval
                #region Workflow

                strQry = "UPDATE TBLTCREPLACE SET TR_APPROVE_REMARKS='" + objReplace.sRemarks + "' , TR_APPROVE_FLAG=1,TR_OIL_QTY_BYSK='" + objReplace.sOilQuantitySK + "', ";
                strQry += " TR_RV_NO='{0}',TR_RV_DATE=TO_DATE('" + objReplace.sRVDate + "','dd/MM/yyyy'),";
                strQry += " TR_APPROVED_DATE=SYSDATE,TR_APPROVED_BY='" + objReplace.sCrby + "',TR_MANUAL_ACKRV_NO='" + objReplace.sManualRVACKNo + "' WHERE TR_ID='" + objReplace.sDecommId + "'";

                strQry = strQry.Replace("'", "''");

                //string strQry1 = "UPDATE TBLDTCFAILURE SET DF_REPLACE_FLAG=1,DF_REP_DATE=SYSDATE WHERE DF_ID='" + objReplace.sFailureId + "' ";
                //strQry1 += " AND DF_REPLACE_FLAG=0";

                //strQry1 = strQry1.Replace("'", "''");

                string strQry2 = string.Empty;
                if (objReplace.sTasktype == "1" || objReplace.sTasktype == "4")
                {
                    //Update TC Status in TC Master Table
                    strQry2 = "UPDATE TBLTCMASTER SET TC_STATUS=3 ,TC_CURRENT_LOCATION=1,TC_UPDATED_EVENT='FAILURE ENTRY',";
                    strQry2 += "TC_UPDATED_EVENT_ID='" + objReplace.sFailureId + "', TC_LOCATION_ID='" + objReplace.sOfficeCode + "'";
                    strQry2 += " WHERE TC_CODE= '" + sTCCode + "'";

                    strQry2 = strQry2.Replace("'", "''");
                }
                else
                {
                    //Update TC Status in TC Master Table
                    strQry2 = "UPDATE TBLTCMASTER SET TC_STATUS=1 ,TC_CURRENT_LOCATION=1,TC_UPDATED_EVENT='ENHANCEMENT ENTRY',";
                    strQry2 += "TC_UPDATED_EVENT_ID='" + objReplace.sFailureId + "',TC_LOCATION_ID='" + objReplace.sOfficeCode + "' ";
                    strQry2 += " WHERE TC_CODE= '" + sTCCode + "'";

                    strQry2 = strQry2.Replace("'", "''");
                }

                string sParam = "SELECT ACKNUMBER(" + objReplace.sDecommId + ") FROM DUAL";

                //
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objReplace.sFormName;
                objApproval.sRecordId = objReplace.sDecommId;
                objApproval.sOfficeCode = objReplace.sOfficeCode;
                objApproval.sClientIp = objReplace.sClientIP;
                objApproval.sCrby = objReplace.sCrby;
                objApproval.sWFObjectId = objReplace.sWFObjectId;
                objApproval.sDataReferenceId = objReplace.sDTCCode;
                objApproval.sWFAutoId = objReplace.sWFAutoId;

                objApproval.sQryValues = strQry + ";" + strQry2;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLTCREPLACE";

                objApproval.sDescription = "RI Approval For DTr Code " + objReplace.sTCCode;

                objApproval.sRefOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE WHERE DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=TR_IN_NO AND TR_ID='" + objReplace.sDecommId + "'");

                objApproval.sColumnNames = "TR_ID,TR_OIL_QUNTY,TR_APPROVE_REMARKS,TR_APPROVE_FLAG,TR_OIL_QTY_BYSK,TR_RV_NO,TR_RV_DATE,TR_APPROVED_DATE,TR_APPROVED_BY,TR_MANUAL_ACKRV_NO,TR_COMM_DATE";

                objApproval.sColumnValues = "" + objReplace.sDecommId + "," + objReplace.sOilQuantity + "," + objReplace.sRemarks + ",1," + objReplace.sOilQuantitySK + "," + objReplace.sRVNo + ",";
                objApproval.sColumnValues += "" + objReplace.sRVDate + ",SYSDATE," + objReplace.sCrby + "," + objReplace.sManualRVACKNo + "";

                objApproval.sTableNames = "TBLTCREPLACE";


                //Check for Duplicate Approval
                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                if (objReplace.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objReplace.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objReplace.sWFDataId = objApproval.sWFDataId;
                    //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }

                #endregion


                Arr[0] = "RI Approved Successfully";
                Arr[1] = "1";
                return Arr;

            }
            catch (Exception ex)
            {
                //ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateReplaceDetails");
                return Arr;
            }
            finally
            {

            }
        }

        public object GetFailureTCDetails(clsRIApproval objRIAprrove)
        {
            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbCommand = new OleDbCommand();
                strQry = "SELECT TC_ID,TC_SLNO,TC_CODE,WO_NO_DECOM,TO_CHAR(TC_CAPACITY) TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)  TM_NAME,DF_DTC_CODE, ";
                strQry += " TO_CHAR(DF_DATE,'DD/MM/YYYY') AS DF_DATE FROM TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER WHERE DF_EQUIPMENT_ID=TC_CODE AND DF_ID=WO_DF_ID AND ";
                strQry += " DF_ID =:sFailureId";
                oledbCommand.Parameters.AddWithValue("sFailureId", objRIAprrove.sFailureId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRIAprrove.sDecomWorkOrder = dt.Rows[0]["WO_NO_DECOM"].ToString();
                    objRIAprrove.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objRIAprrove.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objRIAprrove.sTcMake = dt.Rows[0]["TM_NAME"].ToString();
                    objRIAprrove.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objRIAprrove.sDTCCode = dt.Rows[0]["DF_DTC_CODE"].ToString();
                    objRIAprrove.sFailureDate = dt.Rows[0]["DF_DATE"].ToString();
                    //objDecomm.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objRIAprrove.sOilQuantity = ObjCon.get_value("SELECT TR_OIL_QUNTY FROM TBLTCREPLACE WHERE TR_ID='" + objRIAprrove.sDecommId + "'");

                }
                return objRIAprrove;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailureTCDetails");
                return objRIAprrove;
            }
            finally
            {

            }
        }

        #region Completion Report

        #endregion


        public clsRIApproval GetDTCTCDetailsFromFailure(clsRIApproval objRI)
        {
            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbCommand = new OleDbCommand();
                strQry = "SELECT DF_DTC_CODE,DF_EQUIPMENT_ID,(SELECT DT_ID FROM TBLDTCMAST WHERE DF_DTC_CODE=DT_CODE) DT_ID,";
                strQry += " (SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=DF_EQUIPMENT_ID) TC_ID,TD_TC_NO,(SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=TD_TC_NO) TD_TC_ID";
                strQry += " FROM TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND DF_ID=:sFailureId";
                oledbCommand.Parameters.AddWithValue("sFailureId", objRI.sFailureId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRI.sDTCCode = Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]);
                    objRI.sTCCode = Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]);
                    objRI.sFailureTCId = Convert.ToString(dt.Rows[0]["TC_ID"]);
                    objRI.sDTCId = Convert.ToString(dt.Rows[0]["DT_ID"]);
                    objRI.sNewTCCode = Convert.ToString(dt.Rows[0]["TD_TC_NO"]);
                    objRI.sNewTCId = Convert.ToString(dt.Rows[0]["TD_TC_ID"]);

                }
                return objRI;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCTCDetailsFromFailure");
                return objRI;
            }
            finally
            {

            }
        }

        public clsRIApproval GetRIDetails(clsRIApproval objRIApproval)
        {
            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbCommand = new OleDbCommand();
                strQry = "SELECT TR_OIL_QUNTY,TR_APPROVE_REMARKS,TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MM/YYYY') TR_RV_DATE,TR_MANUAL_ACKRV_NO,";
                strQry += " TR_OIL_QTY_BYSK,TO_CHAR(TR_DECOMM_DATE,'DD/MM/YYYY') TR_DECOMM_DATE FROM TBLTCREPLACE WHERE TR_ID=:sDecommId";
                oledbCommand.Parameters.AddWithValue("sDecommId", objRIApproval.sDecommId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                    objRIApproval.sRemarks = Convert.ToString(dt.Rows[0]["TR_APPROVE_REMARKS"]).Replace("ç", ",");
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["TR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["TR_RV_DATE"]);
                    objRIApproval.sOilQuantitySK = Convert.ToString(dt.Rows[0]["TR_OIL_QTY_BYSK"]);
                    objRIApproval.sDecommDate = Convert.ToString(dt.Rows[0]["TR_DECOMM_DATE"]);
                    objRIApproval.sManualRVACKNo = Convert.ToString(dt.Rows[0]["TR_MANUAL_ACKRV_NO"]);
                }

                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRIDetails");
                return objRIApproval;
            }
            finally
            {

            }
        }

        public void SendSMStoSectionOfficer(string sDTrNo, string sDecommId, string sFailureId)
        {
            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sFailureId", sFailureId);
                string sOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE WHERE DF_ID=:sFailureId", oledbCommand);



                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sDecommId", sDecommId);
                string sResult = ObjCon.get_value("SELECT SM_NAME || '~' || TR_RI_NO FROM TBLTCREPLACE,TBLSTOREMAST WHERE TR_STORE_SLNO=SM_ID AND TR_ID=:sDecommId", oledbCommand);

                oledbCommand = new OleDbCommand();

                strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN (4) AND US_OFFICE_CODE=:sOfficeCode";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objComm = new clsCommunication();

                    objComm.sSMSkey = "SMStoRI";
                    objComm = objComm.GetsmsTempalte(objComm);
                    string sSMSText = String.Format(objComm.sSMSTemplate,
                        sResult.Split('~').GetValue(1).ToString(), sDTrNo, sResult.Split('~').GetValue(0).ToString());
                    //objCommunication.sendSMS(sSMSText, sMobileNo, sFullName);
                    objComm.DumpSms(sMobileNo, sSMSText, objComm.sSMSTemplateID);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetMobileNoofSectionStoreOfficer");
                //return ex.Message;

            }
            finally
            {

            }
        }


        public string GenerateAckNo(string sOfficeCode)
        {
            try
            {

                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sAckNo = ObjCon.get_value("SELECT  NVL(MAX(TR_RV_NO),0)+1  FROM TBLTCREPLACE WHERE TR_RV_NO LIKE :sOfficeCode||'%'", oledbCommand);
                if (sAckNo.Length == 1)
                {

                    //2 digit Division Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy");
                    }

                    sAckNo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //2 digit Division Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sAckNo.Substring(2, 4))
                        {
                            return sAckNo;
                        }
                        else
                        {
                            sAckNo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sAckNo;
                    }


                }

                return sAckNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateAckNo");
                return "";
            }
            finally
            {

            }
        }

        #region WorkFlow XML

        public clsRIApproval GetRIDetailsFromXML(clsRIApproval objRIApproval)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if(objRIApproval.sWFDataId!="" && objRIApproval.sWFDataId!=null)
                {
                    dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                }
               
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                    objRIApproval.sRemarks = Convert.ToString(dt.Rows[0]["TR_APPROVE_REMARKS"]);
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["TR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["TR_RV_DATE"]);
                    objRIApproval.sOilQuantitySK = Convert.ToString(dt.Rows[0]["TR_OIL_QTY_BYSK"]);
                    objRIApproval.sCrby = Convert.ToString(dt.Rows[0]["TR_APPROVED_BY"]);
                    objRIApproval.sManualRVACKNo = Convert.ToString(dt.Rows[0]["TR_MANUAL_ACKRV_NO"]);
                }
                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRIDetailsFromXML");
                return objRIApproval;
            }
        }

        #endregion






    }
}
