using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.IO;
using System.Configuration;


namespace IIITS.DTLMS.BL.OilFlow
{

    public class clsOilTest
    {

        string strFormCode = "clsOilTest";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        //Tc Details
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }

        public string sMakeName { get; set; }
        public string sManfDate { get; set; }
        public string sCapacity { get; set; }
        public string sActionType { get; set; }
        public string sWarrantyPeriod { get; set; }
        public string sStatusFlag { get; set; }
        public string sSupplierName { get; set; }
        public string sGuarantyType { get; set; }
        public string sInspectionId { get; set; }
        public string sMakeId { get; set; }
        public string sStoreId { get; set; }
        public string sParam { get; set; }
        public string sDivisionId { get; set; }
        public string sSupplierId { get; set; }
        public string sRepairerId { get; set; }
        public string sEnhancementId { get; set; }
        public string sWOId { get; set; }
        public string sOilRecordid { get; set; }
        public string sTcId { get; set; }
        public string sRefString { get; set; }
        public string sItemCode { get; set; }
        public string sRoleId { get; set; }

        //To send to Repairer/Supplier
        public string sSupRepId { get; set; }
        public string sIssueDate { get; set; }
        public string sPurchaseOrderNo { get; set; }
        public string sPurchaseDate { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sManualInvoiceNo { get; set; }
        public string sCrby { get; set; }
        public string sType { get; set; }

        public string sRepairDetailsId { get; set; }
        public string sRepairMasterId { get; set; }
        public string sQty { get; set; }
        public string sNthTime { get; set; }

        //Testing Activity       
        public string sPass { get; set; }
        public string sFail { get; set; }
        public bool sIsOldPo { get; set; }

        public string sTestingDone { get; set; }
        public string sTestedBy { get; set; }
        public string sTestedOn { get; set; }
        public string sTestLocation { get; set; }
        public string sInspRemarks { get; set; }
        public string sTestResult { get; set; }
        public string sTestInspectionId { get; set; }
        public string sOilQty { get; set; }
        public string sDescription { get; set; }
        public string sBOId { get; set; }
        public string sAinAsdid { get; set; }
        public string sAINTestLocation { get; set; }
        public string sAinInspResult { get; set; }
        public string sAinRemarks { get; set; }
        public string sAinCrby { get; set; }
        public string sAinCron { get; set; }
        public string sAsdId { get; set; }
        public string sFailureId { get; set; }
        public string sAsdquantity { get; set; }
        public string sCron { get; set; }
        public DataTable dtTestDone { get; set; }

        //Deliver or Recieve DTR
        public string sDeliverDate { get; set; }
        public string sDeliverChallenNo { get; set; }
        public string sVerifiedby { get; set; }
        public string sOfficeCode { get; set; }
        public string sRVNo { get; set; }
        public string sRVDate { get; set; }
        public string sOMNo { get; set; }
        public string sOMDate { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sWFObjectId { get; set; }

        //Document
        public string sFileName { get; set; }
        public string sStatus { get; set; }
        public string sOldPONo { get; set; }
        public string sPORemarks { get; set; }

        public string sMMSAutoInvNo { get; set; }
        public string sMMSAutoRVNo { get; set; }
        public string sInsQty { get; set; }
        public string sInspectedQty { get; set; }
        public double sPendingQty { get; set; }
        public string sAgencyName { get; set; }
        public string sAgencyValue { get; set; }
        public string sOilTestFileName { get; set; }

        public string sOilFilePath { get; set; }
        public string PercentageValue { get; set; }
        public string budgetaccCode { get; set; }
        public string budgetDivcode { get; set; }
        public string budgetFinyear { get; set; }

        #region Fault TC Search and Send to Repair
        OleDbCommand oleDbCommand;


        public string GenerateAutoRVNumber(string sOfficeLoc)
        {
            try
            {

                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;

                if (sOfficeLoc.Length > 2)
                {
                    sOfficeLoc = sOfficeLoc.Substring(0, 2);
                }
                oleDbCommand = new OleDbCommand();
                oleDbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeLoc);
                string sRVAutoNumber = ObjCon.get_value("SELECT NVL(MAX(RV_NO),0)+1 FROM VIEWRVNUMBER WHERE LOCCODE =:sOfficeCode ", oleDbCommand);

                if (sRVAutoNumber.Length == 1)
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

                    sRVAutoNumber = sOfficeLoc + sFinancialYear + "00001";
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
                        if (sFinancialYear == sRVAutoNumber.Substring(2, 4))
                        {
                            return sRVAutoNumber;
                        }
                        else
                        {
                            sRVAutoNumber = sOfficeLoc + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sRVAutoNumber;
                    }

                }
                return sRVAutoNumber;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
                return "";
            }

        }

        public string[] getItemQnty(clsOilTest objRepair)
        {
            string[] arrResult = new string[2];
            //stirng[] result = new string[2];
            try
            {
                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();

                arrResult = objWCF.get_ItemQty(objRepair.sItemCode, objRepair.sOfficeCode, objRepair.sQty);
                return arrResult;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getItemQnty");
                return arrResult;
            }
        }

        public clsOilTest GetOilDetails(clsOilTest objoiltest)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                oleDbCommand = new OleDbCommand();
                strQry = "Select OSD_ID, OSD_PO_NO, OSD_OFFICE_CODE, OSD_STATUS from TBLOILSENTMASTER where OSD_PO_NO ='" + sPurchaseOrderNo + "'";

                oleDbCommand.Parameters.AddWithValue("sPurchaseOrderNo", objoiltest.sPurchaseOrderNo);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objoiltest.sPurchaseOrderNo = dt.Rows[0]["OSD_PO_NO"].ToString();
                    objoiltest.sOfficeCode = dt.Rows[0]["OSD_OFFICE_CODE"].ToString();

                }


                return objoiltest;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetOilDetails");
                return objoiltest;
            }
        }

        //save oil test details to main table..
        public string[] SaveOilDetails(string[] strRepairDetailsIds, clsOilTest objoiltest)
        {
            string[] Arr = new string[2];
            string sFilePath = string.Empty;
            string strQry1 = string.Empty;

            try
            {


                string[] strDetailVal = strRepairDetailsIds.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        byte[] imageData = null;


                        string strQry = string.Empty;

                        string sInspectionId = Convert.ToString(ObjCon.Get_max_no("AIN_ID", "TBLAGENCYINSPECTIONDETAILS"));
                        strQry = "INSERT INTO TBLAGENCYINSPECTIONDETAILS (AIN_ID,AIN_OSD_ID,AIN_INS_BY,AIN_INS_DATE,AIN_TEST_LOCATION,AIN_INSP_RESULT,AIN_REMARKS,AIN_CR_BY,AIN_CRON,AIN_INVOICE_QTY,AIN_INSP_QTY,AIN_STATUS_FLAG,AIN_UPLOAD_PATH)";
                        strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "','" + objoiltest.sTestedBy + "',TO_DATE('" + objoiltest.sTestedOn + "','DD/MM/YYYY'),";
                        strQry += " '" + objoiltest.sTestLocation + "',1,'" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objoiltest.sCrby + "',SYSDATE,";
                        strQry += "'" + strDetailVal[i].Split('~').GetValue(5) + "' ,'" + strDetailVal[i].Split('~').GetValue(4) + "',0,'" + objoiltest.sOilTestFileName + "')";
                        ObjCon.Execute(strQry);

                        if (strQry != string.Empty)
                        {
                            //strQry1 = "UPDATE TBLOILSENTMASTER SET OSD_INSP_RESULT='"+ strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "' WHERE OSD_PO_NO= '" + objoiltest.sPurchaseOrderNo + "' and OSD_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "'";
                            strQry1 = "UPDATE TBLOILSENTMASTER SET OSD_INSP_RESULT= 1, OSD_STATUS=0, OSD_STATUS_FLAG=0 WHERE OSD_PO_NO= '" + objoiltest.sPurchaseOrderNo + "' and OSD_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "'";
                            ObjCon.Execute(strQry1);
                        }

                        Arr[0] = " Oil Testing Done Successfully";
                        Arr[1] = "0";
                        return Arr;

                    }

                }

                Arr[1] = "1";
                return Arr;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveTestingTCDetails");
                return Arr;
            }
            finally
            {

            }
        }

        public string[] SaveOilReviceDetails(string[] strRepairDetailsIds, clsOilTest objoiltest)
        {
            string[] Arr = new string[2];
            string strQry1 = string.Empty;
            DateTime invoice_date = new DateTime();
            string SupplierName = string.Empty;
            string TC_CODE = string.Empty;
            string spath = string.Empty;
            bool res = false;
            string lineNumber = string.Empty;
            DateTime dtHost = Convert.ToDateTime(ConfigurationSettings.AppSettings["dHost"].ToString());

            try
            {
                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                oledbCommand = new OleDbCommand();
                //objoiltest.sMMSAutoRVNo = objWCF.GetRepairerAutoRVNum(objoiltest.sRVNo.Substring(0, 2));
                objoiltest.sMMSAutoRVNo = objWCF.GetOilAutoRVNum(objoiltest.sRVNo.Substring(0, 2));
                spath = "C:\\RVOILINVOICE" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-OilmmsUto.txt";
                File.AppendAllText(spath, " MMSautono " + sMMSAutoRVNo + " generated ,  TIME : " + DateTime.Now + Environment.NewLine);
                string[] strDetailVal = strRepairDetailsIds.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {

                        ObjCon.BeginTrans();
                        string strQry = string.Empty;
                        objoiltest.sOilQty = strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper();
                        //strQry = "Update TBLOILSENTMASTER SET ";
                        //strQry += " OSD_RECIEVED_QTY ='" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper()+ "',";
                        //strQry += "OSD_DELIVER_CHALLEN_NO='" + objoiltest.sDeliverChallenNo.Trim().ToUpper() + "', ";
                        //strQry += "OSD_DELIVER_VER_BY='" + objoiltest.sVerifiedby.Trim().ToUpper() + "',";
                        //strQry += " OSD_RV_NO='" + objoiltest.sRVNo + "',OSD_RV_DATE=to_date('" + objoiltest.sRVDate + "','DD/MM/YYYY')"; 
                        //strQry += ", OSD_STATUS = '1', OSD_ITEM_CODE = '" + objoiltest.sItemCode + "',OSD_MMS_AUTO_RV_NO = '"+ objoiltest.sMMSAutoRVNo + "'  WHERE UPPER(OSD_PO_NO) = '" + objoiltest.sPurchaseOrderNo.ToUpper() + "' and OSD_INVOICE_NO='"+objoiltest.sInvoiceNo+"' ";

                        strQry = "Update TBLOILSENTMASTER SET ";
                        strQry += " OSD_STATUS = '1',OSD_STATUS_FLAG='1',OSD_PENDING_QTY='" + objoiltest.sPendingQty + "'  WHERE UPPER(OSD_PO_NO) = '" + objoiltest.sPurchaseOrderNo.ToUpper() + "' and OSD_INVOICE_NO='" + objoiltest.sInvoiceNo + "' ";
                        ObjCon.Execute(strQry);

                        strQry = "Update TBLAGENCYINSPECTIONDETAILS SET ";
                        strQry += " AIN_STATUS_FLAG = '1'  WHERE AIN_OSD_ID = '" + strDetailVal[i].Split('~').GetValue(3).ToString() + "' ";
                        ObjCon.Execute(strQry);

                        string sOSID = Convert.ToString(ObjCon.Get_max_no("OS_ID", "TBLOILSENTDETAILS"));
                        strQry = "INSERT INTO TBLOILSENTDETAILS (OS_ID,OS_PO_NO,OS_QUANTITY,OS_OFFICE_CODE,OS_DELIVER_CHALLEN_NO,OS_DELIVER_VER_BY,OS_RV_DATE,OS_RV_NO,OS_ITEM_CODE,OS_MMS_AUTO_RV_NO,OS_RECIEVED_QTY,OS_PENDING_QTY,OS_STATUS_FLAG,OS_DELIVER_DATE)";
                        strQry += " VALUES ('" + sOSID + "','" + objoiltest.sPurchaseOrderNo.ToUpper() + "','" + strDetailVal[i].Split('~').GetValue(1).ToString() + "','" + objoiltest.sOfficeCode + "','" + objoiltest.sDeliverChallenNo.Trim().ToUpper() + "','" + objoiltest.sVerifiedby.Trim().ToUpper() + "',TO_DATE('" + objoiltest.sRVDate + "','DD/MM/YYYY'),";
                        strQry += " '" + objoiltest.sRVNo + "','" + objoiltest.sItemCode + "','" + objoiltest.sMMSAutoRVNo + "','" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objoiltest.sPendingQty + "',1,TO_DATE('" + objoiltest.sDeliverDate + "','DD/MM/YYYY'))";
                        ObjCon.Execute(strQry);


                        Arr[0] = " Oil Recieved Successfully";
                        Arr[1] = "0";


                        File.AppendAllText(spath, " PO NUMBER " + sPurchaseOrderNo + " Oil Recieved Update  Done Successfully ,  TIME : " + DateTime.Now + Environment.NewLine);


                    }


                }
                DataTable OilDetails = new DataTable("OilDetails");
                OilDetails.Columns.Add("OSD_QUANTITY", typeof(string));
                OilDetails.Columns.Add("OSD_DELIVER_CHALLEN_NO", typeof(string));
                OilDetails.Columns.Add("OSD_DELIVER_VER_BY", typeof(string));
                OilDetails.Columns.Add("OSD_RV_NO", typeof(string));
                OilDetails.Columns.Add("OSD_RV_DATE", typeof(string));
                OilDetails.Columns.Add("OSD_ITEM_CODE", typeof(string));
                OilDetails.Columns.Add("OSD_PO_NO", typeof(string));
                OilDetails.Columns.Add("OSD_OFFICE_CODE", typeof(string));
                OilDetails.Columns.Add("OSD_TYPE", typeof(string));
                OilDetails.Columns.Add("OSD_REMARKS", typeof(string));
                OilDetails.Columns.Add("OSD_PO_DATE", typeof(string));
                OilDetails.Columns.Add("OSD_MMS_AUTO_RV_NO", typeof(string));
                OilDetails.Columns.Add("OSD_AGENCY_NAME", typeof(string));
                DataRow drow = OilDetails.NewRow();
                // drow["OSD_ID"] = sID;
                //File.AppendAllText(spath, " RV DATE " + objoiltest.sRVDate + " Before Parse Extract ,  TIME : " + DateTime.Now + Environment.NewLine);
                DateTime issue_date = DateTime.ParseExact(objoiltest.sRVDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                invoice_date = issue_date;
                //File.AppendAllText(spath, " RV DATE " + issue_date + " After Parse Extract ,  TIME : " + DateTime.Now + Environment.NewLine);

                drow["OSD_PO_NO"] = objoiltest.sPurchaseOrderNo.ToUpper();
                //issue_date = DateTime.ParseExact(objoiltest.sPurchaseDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                drow["OSD_RV_DATE"] = issue_date.ToString("yyyy-MM-dd");
                drow["OSD_DELIVER_CHALLEN_NO"] = objoiltest.sDeliverChallenNo;
                //File.AppendAllText(spath, " Purchase Date" + objoiltest.sPurchaseDate + " Before Parse Extract ,  TIME : " + DateTime.Now + Environment.NewLine);
                DateTime podate = DateTime.ParseExact(objoiltest.sPurchaseDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //File.AppendAllText(spath, " Purchase Date" + podate + " After Parse Extract ,  TIME : " + DateTime.Now + Environment.NewLine);
                drow["OSD_PO_DATE"] = podate.ToString("yyyy-MM-dd");
                drow["OSD_DELIVER_VER_BY"] = objoiltest.sVerifiedby;
                //drow["OSD_RV_NO"] = objoiltest.sRVNo;
                drow["OSD_OFFICE_CODE"] = objoiltest.sOfficeCode;
                drow["OSD_RV_NO"] = objoiltest.sMMSAutoRVNo;
                drow["OSD_QUANTITY"] = sOilQty;
                drow["OSD_TYPE"] = "RecieptVoucher";
                drow["OSD_REMARKS"] = "RECEIVED OIL";
                drow["OSD_ITEM_CODE"] = objoiltest.sItemCode;
                drow["OSD_AGENCY_NAME"] = objoiltest.sAgencyName;
                OilDetails.Rows.Add(drow);
                string[] result = new string[2];
                //File.AppendAllText(spath, "Po Number "+ sPurchaseOrderNo + " Result " + Arr[1] + " after received oil ,  TIME : " + DateTime.Now + Environment.NewLine);
                if (Arr[1] != "0")
                {
                    res = false;
                }
                else
                {

                    result = objWCF.saveRVOil_invoice(OilDetails);
                    //File.AppendAllText(spath, "Po Number " + sPurchaseOrderNo + " Result " + result[1] + " after service call result from MMS Service  ,  TIME : " + DateTime.Now + Environment.NewLine);
                    if (result[1] == "1")
                    {
                        res = true;
                    }
                    else
                    {
                        res = false;
                    }
                }


                if (res == true)
                {
                    ObjCon.CommitTrans();
                }

                else
                {
                    ObjCon.RollBack();
                }

                return Arr;
            }
            catch (Exception ex)
            {
                if (ex.StackTrace.Contains(":line"))
                {
                    lineNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                }

                File.AppendAllText(spath, " PO NUMBER " + sPurchaseOrderNo + " ; EXCEPTION MESSAGE : " + ex.Message + " ; STACK TRACE : " + ex.StackTrace + ";  TIME : " + DateTime.Now + Environment.NewLine + lineNumber);
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveTestingTCDetails");
                return Arr;
            }
            finally
            {

            }
        }

        //save the oiltest details through xml..
        public string[] SaveOiltestapprovalDetails(string[] strRepairDetailsIds, clsOilTest objoiltest)
        {

            oleDbCommand = new OleDbCommand();
            string[] Arr = new string[2];
            try
            {
                oleDbCommand = new OleDbCommand();
                OleDbDataReader dr;
                string strQry = string.Empty;
                string strQry1 = string.Empty;
                DataTable dt = new DataTable();
                if (objoiltest.sPurchaseOrderNo == objoiltest.sPurchaseOrderNo || objoiltest.sPurchaseOrderNo == "")
                {

                    oleDbCommand = new OleDbCommand();
                    objoiltest.sOilRecordid = ObjCon.Get_max_no("AIN_ID", "TBLAGENCYINSPECTIONDETAILS").ToString();

                    oleDbCommand = new OleDbCommand();
                    oleDbCommand.Parameters.AddWithValue("DtcCode2", objoiltest.sPurchaseOrderNo);

                    if (objoiltest.sInvoiceDate == "")
                    {
                        objoiltest.sInvoiceDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    //Workflow / Approval
                    #region WorkFlow

                    string[] strDetailVal = strRepairDetailsIds.ToArray();


                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        if (strDetailVal[i] != null)
                        {
                            byte[] imageData = null;

                            string sInspectionId = Convert.ToString(ObjCon.Get_max_no("AIN_ID", "TBLAGENCYINSPECTIONDETAILS"));
                            strQry = "INSERT INTO TBLAGENCYINSPECTIONDETAILS (AIN_ID,AIN_OSD_ID,AIN_INS_BY,AIN_INS_DATE,AIN_TEST_LOCATION,AIN_INSP_RESULT,AIN_REMARKS,AIN_CR_BY)";
                            strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "','" + objoiltest.sTestedBy + "',TO_DATE('" + objoiltest.sTestedOn + "','DD/MM/YYYY'),";
                            strQry += " '" + objoiltest.sTestLocation + "','" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "','" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objoiltest.sCrby + "')";
                            strQry = strQry.Replace("'", "''");
                            //ObjCon.Execute(strQry);


                            //strQry1 = "UPDATE TBLOILSENTMASTER SET OSD_STATUS='" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "' WHERE OSD_PO_NO= '" + objoiltest.sPurchaseOrderNo + "'";
                            strQry1 = "UPDATE TBLOILSENTMASTER SET OSD_STATUS='1' WHERE OSD_PO_NO= '" + objoiltest.sPurchaseOrderNo + "'";

                            strQry1 = strQry1.Replace("'", "''");
                            //ObjCon.Execute(strQry);

                            //Arr[0] = "Testing Done Successfully";
                            //Arr[1] = "0";
                        }
                    }


                    // TC_STATUS  = 11 means  Released Good

                    string sParam = "SELECT NVL(MAX(AIN_ID),0)+1 FROM TBLAGENCYINSPECTIONDETAILS";

                    clsApproval objApproval = new clsApproval();

                    if (objoiltest.sActionType == null)
                    {

                        bool bResult = objApproval.CheckAlreadyExistEntry(objoiltest.sPurchaseOrderNo, "10");
                        if (bResult == true)
                        {
                            Arr[0] = "Capacity Enhancement/Reduction Already done for DTC Code " + objoiltest.sPurchaseOrderNo + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                        bResult = objApproval.CheckAlreadyExistEntry(objoiltest.sPurchaseOrderNo, "9");
                        if (bResult == true)
                        {
                            Arr[0] = "Failure Declare Already done for DTC Code " + objoiltest.sPurchaseOrderNo + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                    }

                    if (objoiltest.sStatusFlag == "2")
                        objApproval.sFormName = objoiltest.sFormName;
                    else
                        objApproval.sFormName = "OilTesting";


                    objApproval.sRecordId = objoiltest.sOilRecordid;
                    objApproval.sOfficeCode = objoiltest.sOfficeCode;
                    objApproval.sClientIp = objoiltest.sClientIP;
                    objApproval.sCrby = objoiltest.sCrby;

                    objApproval.sQryValues = strQry + ";" + strQry1;
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLAGENCYINSPECTIONDETAILS";
                    objApproval.sRefOfficeCode = objoiltest.sOfficeCode;
                    objApproval.sDataReferenceId = objoiltest.sPurchaseOrderNo;
                    if (objoiltest.sStatusFlag == "2")
                        objApproval.sDescription = "Capacity Enhancement For DTC Code " + objoiltest.sPurchaseOrderNo;
                    else
                        objApproval.sDescription = "OIL Tested  " + objoiltest.sPurchaseOrderNo;

                    //string sPrimaryKey = "{0}";

                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        if (strDetailVal[i] != null)
                        {
                            byte[] imageData = null;

                            objApproval.sColumnNames = "AIN_ID,AIN_OSD_ID,AIN_INS_BY,AIN_INS_DATE,AIN_TEST_LOCATION,AIN_INSP_RESULT,AIN_REMARKS,AIN_CR_BY,AIN_CRON";

                            objApproval.sColumnValues = "" + sInspectionId + "," + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "," + objoiltest.sTestedBy + "," + objoiltest.sTestedOn + ",";
                            objApproval.sColumnValues += "" + objoiltest.sTestLocation + "," + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "," + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "," + objoiltest.sCrby + "," + sOfficeCode + ", SYSDATE ";

                            objApproval.sTableNames = "TBLAGENCYINSPECTIONDETAILS";
                        }
                    }


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objoiltest.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objoiltest.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion
                    if (objoiltest.sStatusFlag == "2")
                        Arr[0] = "Test Test Declared Successfully";
                    else
                        Arr[0] = "OIL Test Declared Successfully";

                    Arr[1] = "0";
                    return Arr;
                }
                else
                {

                    oleDbCommand = new OleDbCommand();
                    oleDbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLAGENCYINSPECTIONDETAILS SET AIN_CRON=TO_DATE(:sEnhancementDate1,'dd/MM/yyyy'), AIN_REMARKS=:sInspRemarks1,";
                    strQry += " AIN_INSP_RESULT=:sTestResult1 WHERE AIN_ID=:sPurchaseOrderNo1";
                    oleDbCommand.Parameters.AddWithValue("sCron1", objoiltest.sCron);
                    oleDbCommand.Parameters.AddWithValue("sInspRemarks1", objoiltest.sInspRemarks);
                    oleDbCommand.Parameters.AddWithValue("sTestResult1", objoiltest.sTestResult);
                    oleDbCommand.Parameters.AddWithValue("sPurchaseOrderNo1", objoiltest.sPurchaseOrderNo);
                    ObjCon.Execute(strQry, oleDbCommand);



                    Arr[0] = "DTC Enhancement Updated Successfully";
                    Arr[1] = "1";
                    return Arr;

                }

            }

            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveEnhancementDetails");
                return Arr;
            }

        }



        #endregion

        # region Testing Activity

        public DataTable LoadTestOrDeliverPendingDTR(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS='0'   and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0')  and OSD_PENDING_QTY <>'0'  and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                string Pono = ObjCon.get_value(strQry, oleDbCommand);

                if (Pono == "")
                {
                    if (objoiltest.sRoleId=="31")
                    {
                        strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0'  and OSD_INSP_RESULT ='0' and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "'  and OSD_OFFICE_CODE ='" + objoiltest.sOfficeCode + "' union all ";
                        strQry += "Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_PENDING_QTY=OS_PENDING_QTY   and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "'  and (OS_PENDING_QTY <>'0') and OSD_OFFICE_CODE ='"+objoiltest.sOfficeCode+"'";
                    }
                    else
                    {
                        strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0'  and OSD_INSP_RESULT ='0' and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' union all ";
                        strQry += "Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_PENDING_QTY=OS_PENDING_QTY   and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "'  and (OS_PENDING_QTY <>'0')";
                    }
                }
                else
                {

                    //strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0'  and OSD_INSP_RESULT ='0' and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' ";
                    //strQry = " Select OSD_ID, OSD_PO_NO, OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' AND OSD_OFFICE_CODE='"+ objoiltest.sOfficeCode + "'";
                    if (objoiltest.sRoleId == "31")
                    {
                        strQry = "Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_PENDING_QTY=OS_PENDING_QTY  and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "'  and (OS_PENDING_QTY <>'0') and OSD_OFFICE_CODE ='" + objoiltest.sOfficeCode + "'";
                    }
                    else
                    {
                        strQry = "Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_PENDING_QTY=OS_PENDING_QTY  and OSD_STATUS_FLAG='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "'  and (OS_PENDING_QTY <>'0')";
                    }
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return dt;
            }
            finally
            {

            }
        }
        public DataTable LoadTestDeliverPendingoil(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS='0'   and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0')  and OSD_PENDING_QTY <>'0'  and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                string Pono = ObjCon.get_value(strQry, oleDbCommand);
                if (Pono == "")
                {
                    strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,OSD_PERCENTAGE_VALUE FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0'  and OSD_INSP_RESULT ='0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' and  OSD_ID in (" + objoiltest.sRepairDetailsId + ") UNION ALL ";
                    strQry += " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,OSD_PERCENTAGE_VALUE FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on upper(OSD_PO_NO) = upper(OS_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_PENDING_QTY=OS_PENDING_QTY  and OS_PENDING_QTY <>'0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' and  OSD_ID in (" + objoiltest.sRepairDetailsId + ")";
                }
                else
                {
                    strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,OSD_PERCENTAGE_VALUE FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on upper(OSD_PO_NO) = upper(OS_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_PENDING_QTY=OS_PENDING_QTY  and OS_PENDING_QTY <>'0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' and  OSD_ID in (" + objoiltest.sRepairDetailsId + ")";
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return dt;
            }
            finally
            {

            }
        }

        public string LoadInspectedoil(clsOilTest objoiltest)
        {
            string inspectedqty = string.Empty;
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                //strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0'  and OSD_INSP_RESULT ='0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' and  OSD_ID in (" + objoiltest.sRepairDetailsId + ")";
                strQry = "select sum(AIN_INSP_QTY)from TBLAGENCYINSPECTIONDETAILS where AIN_OSD_ID = '" + objoiltest.sRepairDetailsId + "'";

                inspectedqty = ObjCon.get_value(strQry, oleDbCommand);
                return inspectedqty;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return inspectedqty;
            }
            finally
            {

            }
        }

        public DataTable LoadTestingdoneoil(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE  and OSD_STATUS='0'  and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0')  and OSD_PENDING_QTY <>'0'  and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                string Pono = ObjCon.get_value(strQry, oleDbCommand);
                if (Pono == "")
                {
                    strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR( OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,";
                    strQry += "CASE WHEN OSD_INSP_RESULT  = '1'  THEN 'Reclaimed Oil' ELSE 'Bad Oil' END AS STATUS,AIN_INSP_QTY,OSD_AGENCY,AIN_INS_BY,TO_CHAR(AIN_INS_DATE,'dd-mm-yyyy')as AIN_INS_DATE,OSD_PERCENTAGE_VALUE  FROM TBLAGENCYINSPECTIONDETAILS inner join TBLOILSENTMASTER on AIN_OSD_ID=OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' and OSD_INSP_RESULT='1' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' AND OSD_QUANTITY='" + objoiltest.sQty + "' union all  ";
                    strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR( OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,";
                    strQry += "CASE WHEN OSD_INSP_RESULT  = '1'  THEN 'Reclaimed Oil' ELSE 'Bad Oil' END AS STATUS,AIN_INSP_QTY,OSD_AGENCY,AIN_INS_BY,TO_CHAR(AIN_INS_DATE,'dd-mm-yyyy')as AIN_INS_DATE,OSD_PERCENTAGE_VALUE  FROM TBLAGENCYINSPECTIONDETAILS inner join TBLOILSENTMASTER on AIN_OSD_ID=OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and AIN_STATUS_FLAG='0'  AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' AND OSD_QUANTITY='" + objoiltest.sQty + "' ";
                }
                else
                {
                    strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR( OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,";
                    strQry += "CASE WHEN OSD_INSP_RESULT  = '1'  THEN 'Reclaimed Oil' ELSE 'Bad Oil' END AS STATUS,AIN_INSP_QTY,OSD_AGENCY,AIN_INS_BY,TO_CHAR(AIN_INS_DATE,'dd-mm-yyyy')as AIN_INS_DATE  FROM TBLAGENCYINSPECTIONDETAILS inner join TBLOILSENTMASTER on AIN_OSD_ID=OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and AIN_STATUS_FLAG='0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' AND OSD_QUANTITY='" + objoiltest.sQty + "' ";
                }


                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return dt;
            }
            finally
            {

            }
        }

        public DataTable LoadRecievedOil(clsOilTest objoiltest, string sPurchaseOrderNo)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = " Select OSD_ID, OSD_PO_NO,TO_CHAR( OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_OFFICE_CODE LIKE '" + objoiltest.sOfficeCode + "%' and OSD_STATUS = '1' AND OSD_INSP_RESULT='1' ";
                oleDbCommand.Parameters.AddWithValue("OSD_ID", sPurchaseOrderNo);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadPendingForRecieve");
                return dt;
            }
            finally
            {

            }
        }

        public DataTable GetWOBasicDetails(clsOilTest objoiltest)
        {
            oleDbCommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {

                strQry = " Select OSD_ID, OSD_PO_NO, OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,WO_ID as OSD_PO_NO,OSD_STATUS,WO_DATA_ID FROM TBLOILSENTMASTER,TBLDIVISION,TBLWORKFLOWOBJECTS where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' AND WO_ID='" + objoiltest.sWOId + "' AND WO_DATA_ID = OSD_PO_NO  ";
                oleDbCommand.Parameters.AddWithValue("WOId", objoiltest.sWOId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetWOBasicDetails");
                return dt;
            }
            finally
            {

            }
        }

        public DataTable LoadPendingApprovalInbox(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                //strQry = " Select OSD_ID, OSD_PO_NO, OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' ";
                strQry = " Select OSD_ID, OSD_PO_NO, OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' AND OSD_OFFICE_CODE='" + objoiltest.sOfficeCode + "'";

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return dt;
            }
            finally
            {

            }
        }

        public string[] SaveTestingTCDetails(string[] strRepairDetailsIds, clsOilTest objpending, DataTable dt)
        {
            string[] Arr = new string[2];
            string sFilePath = string.Empty;

            try
            {


                string[] strDetailVal = strRepairDetailsIds.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        byte[] imageData = null;


                        string strQry = string.Empty;

                        string sInspectionId = Convert.ToString(ObjCon.Get_max_no("AIN_ID", "TBLAGENCYINSPECTIONDETAILS"));
                        strQry = "INSERT INTO TBLAGENCYINSPECTIONDETAILS (AIN_ID,AIN_OSD_ID,AIN_INS_BY,AIN_INS_DATE,AIN_TEST_LOCATION,AIN_INSP_RESULT,AIN_REMARKS,AIN_CR_BY,AIN_PO_NO)";
                        strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "','" + objpending.sTestedBy + "',TO_DATE('" + objpending.sTestedOn + "','DD/MM/YYYY'),";
                        strQry += " '" + objpending.sTestLocation + "','" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "','" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objpending.sCrby + "','" + objpending.sPurchaseOrderNo + "')";

                        ObjCon.Execute(strQry);
                        string strQry1 = string.Empty;

                        strQry1 = "UPDATE TBLOILSENTMASTER SET OSD_STATUS='" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "' WHERE OSD_PO_NO= '" + objpending.sPurchaseOrderNo + "'";
                        ObjCon.Execute(strQry1);

                        Arr[0] = "Testing Done Successfully";
                        Arr[1] = "0";



                    }
                }


                return Arr;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveTestingTCDetails");
                return Arr;
            }
            finally
            {

            }
        }

        public clsOilTest LoadTestedDTR(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = " SELECT RSM_PO_NO,TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE, RSD_ID, TC_CODE,TC_SLNO,";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) TM_NAME, ";
                strQry += " TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME, ";
                strQry += " IND_INSP_BY,TO_CHAR(IND_INSP_DATE,'DD-MON-YYYY') IND_INSP_DATE,IND_TEST_LOCATION,IND_INSP_RESULT,IND_REMARKS";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLTCMASTER,TBLINSPECTIONDETAILS WHERE  RSM_ID = RSD_RSM_ID AND ";
                strQry += " RSD_TC_CODE = TC_CODE AND IND_INSP_RESULT = 1 AND IND_RSD_ID=RSD_ID AND TC_CODE=:TCCode ";
                strQry += " AND IND_ID=:InspectionID";
                oleDbCommand.Parameters.AddWithValue("TCCode", objoiltest.sTcCode);
                oleDbCommand.Parameters.AddWithValue("InspectionID", objoiltest.sTestInspectionId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objoiltest.dtTestDone = dt;
                    objoiltest.sInspRemarks = Convert.ToString(dt.Rows[0]["IND_REMARKS"]);
                    objoiltest.sTestedBy = Convert.ToString(dt.Rows[0]["IND_INSP_BY"]);
                    objoiltest.sTestedOn = Convert.ToString(dt.Rows[0]["IND_INSP_DATE"]);
                    objoiltest.sTestLocation = Convert.ToString(dt.Rows[0]["IND_TEST_LOCATION"]);
                    objoiltest.sTestResult = Convert.ToString(dt.Rows[0]["IND_INSP_RESULT"]);
                }
                return objoiltest;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return objoiltest;
            }
            finally
            {

            }
        }

        #endregion

        #region Deliver DTR / Recieve DTR


        public DataTable LoadTestingPassedDetails(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS='0' and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0')  and OSD_PENDING_QTY <>'0' and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                string Pono = ObjCon.get_value(strQry, oleDbCommand);
                if (Pono == "")
                {
                    //strQry = " Select OSD_ID, UPPER(OSD_PO_NO) as OSD_PO_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,AIN_INSP_QTY FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_INSP_RESULT = '1' AND OSD_STATUS='0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' union all ";
                    //strQry += " Select OSD_ID, UPPER(OSD_PO_NO) as OSD_PO_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,AIN_INSP_QTY FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE  and OSD_STATUS = '1' AND OSD_PO_NO =  '" + objoiltest.sPurchaseOrderNo + "' and AIN_STATUS_FLAG='0' ";
                    strQry = " Select OSD_ID, UPPER(OSD_PO_NO) as OSD_PO_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,AIN_INSP_QTY FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_INSP_RESULT = '1' AND AIN_STATUS_FLAG='0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' ";
                }
                else
                {
                    strQry = "Select OSD_ID, UPPER(OSD_PO_NO) as OSD_PO_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS,AIN_INSP_QTY FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE AND OSD_PO_NO =  '" + objoiltest.sPurchaseOrderNo + "' and AIN_STATUS_FLAG='0' ";
                }

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPassedDetails");
                return dt;
            }
            finally
            {

            }
        }

        public DataTable LoadPendingForTestingDetails(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS='0'  and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0')  and OSD_PENDING_QTY <>'0' and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                string Pono = ObjCon.get_value(strQry, oleDbCommand);
                if (Pono == "")
                {


                    strQry = " Select UPPER(OSD_PO_NO)AS OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS";
                    //strQry += "  (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=RA_OFFICECODE) RA_OFFICECODE1 ";
                    strQry += " FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' and OSD_INSP_RESULT='1' and OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%'union all  ";
                    strQry += "Select UPPER(OSD_PO_NO)AS OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '1'   and OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' and AIN_STATUS_FLAG='0'";
                }
                else
                {
                    strQry = "Select UPPER(OSD_PO_NO)AS OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE   and OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' and AIN_STATUS_FLAG='0' ORDER BY OSD_ID DESC";
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPassedDetails");
                return dt;
            }
            finally
            {

            }
        }
        public DataTable LoadPendingToRecieveDetails(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS='0'   and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0') and OSD_PENDING_QTY <>'0'  and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                string Pono = ObjCon.get_value(strQry, oleDbCommand);
                if (Pono == "")
                {
                    //strQry = " Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS";
                    //strQry += " FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' and OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                    if (objoiltest.sRoleId == "31")
                    {
                        strQry = " select * from ( Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS,'' as OS_PENDING_QTY ,OSD_ID FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' and OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%'  union all";
                        strQry += " Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS,OS_PENDING_QTY,OSD_ID FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS_FLAG='1' and OSD_PENDING_QTY=OS_PENDING_QTY   and (OS_PENDING_QTY <>'0') and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' )A ORDER BY OSD_ID DESC";
                    }
                    else
                    {
                        strQry = " select * from ( Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS,'' as OS_PENDING_QTY ,OSD_ID FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' and OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%'  union all";
                        strQry += " Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS,OS_PENDING_QTY,OSD_ID FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS_FLAG='1' and OSD_PENDING_QTY=OS_PENDING_QTY   and (OS_PENDING_QTY <>'0') and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' )A ORDER BY OSD_ID DESC";
                    }
                }
                else
                {
                    strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS,OS_PENDING_QTY FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS_FLAG='1'   and (OS_PENDING_QTY <>'0') and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPassedDetails");
                return dt;
            }
            finally
            {

            }
        }

        public string GetPono(clsOilTest objoiltest)
        {
            string Pono = string.Empty;
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;


                strQry = "Select OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,DIV_NAME as OSD_OFFICE_CODE,OSD_QUANTITY,OSD_STATUS FROM TBLOILSENTDETAILS inner JOIN TBLOILSENTMASTER on UPPER(OS_PO_NO) = UPPER(OSD_PO_NO),TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS='0'   and OSD_STATUS_FLAG='1' and  (OS_PENDING_QTY <>'0')  and OSD_PENDING_QTY <>'0'  and  OSD_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY OSD_ID DESC";
                Pono = ObjCon.get_value(strQry, oleDbCommand);

                return Pono;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPassedDetails");
                return Pono;
            }
            finally
            {

            }
        }

        public DataTable getdivisionname(clsOilTest objoiltest)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "select DIV_NAME from TBLDIVISION where DIV_CODE= '" + objoiltest.sOfficeCode + "'";

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPassedDetails");
                return dt;
            }
            finally
            {

            }
        }



        #endregion


        public clsOilTest GetFailureDetailsFromXML(clsOilTest objoiltest)
        {
            oleDbCommand = new OleDbCommand();
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if(objoiltest.sWFDataId!="" && objoiltest.sWFDataId!=null)
                {
                    dt = objApproval.GetDatatableFromXML(objoiltest.sWFDataId);
                }
              
                if (dt.Rows.Count > 0)
                {
                    objoiltest.dtTestDone = dt;
                    objoiltest.sAinAsdid = Convert.ToString(dt.Rows[0]["AIN_OSD_ID"]);
                    objoiltest.sAINTestLocation = Convert.ToString(dt.Rows[0]["AIN_TEST_LOCATION"]);
                    objoiltest.sAinInspResult = Convert.ToString(dt.Rows[0]["AIN_INSP_RESULT"]);
                    objoiltest.sAinRemarks = Convert.ToString(dt.Rows[0]["AIN_REMARKS"]);
                    objoiltest.sAinCrby = Convert.ToString(dt.Rows[0]["AIN_CR_BY"]);
                    objoiltest.sAinCron = Convert.ToString(dt.Rows[0]["AIN_CRON"]);

                    objoiltest.sFailureId = "0";
                    string qry = "SELECT OSD_ID FROM TBLOILSENTMASTER WHERE OSD_ID=:sAinAsdid";
                    oleDbCommand.Parameters.AddWithValue("sAinAsdid", objoiltest.sAinAsdid);
                    objoiltest.sAsdId = ObjCon.get_value(qry, oleDbCommand);
                    GetFailureDetails(objoiltest);



                }
                return objoiltest;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetailsFromXML");
                return objoiltest;
            }
            finally
            {

            }
        }
        public object GetFailureDetails(clsOilTest objoiltest)
        {
            oleDbCommand = new OleDbCommand();

            DataTable dtDetails = new DataTable();
            OleDbDataReader dr = null;

            try
            {
                String strQry = "SELECT  OSD_ID,OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,OSD_INVOICE_DATE,OSD_QUANTITY,OSD_CRON,OSD_CRBY,";
                strQry += "DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS from TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_ID=:AsdId ";
                oleDbCommand.Parameters.AddWithValue("AsdId", objoiltest.sAsdId);
                dr = ObjCon.Fetch(strQry, oleDbCommand);
                dtDetails.Load(dr);


                if (dtDetails.Rows.Count > 0)
                {
                    objoiltest.sAsdId = dtDetails.Rows[0]["OSD_ID"].ToString();
                    objoiltest.sPurchaseOrderNo = dtDetails.Rows[0]["OSD_PO_NO"].ToString();
                    objoiltest.sPurchaseDate = dtDetails.Rows[0]["OSD_PO_DATE"].ToString();
                    objoiltest.sInvoiceNo = dtDetails.Rows[0]["OSD_INVOICE_NO"].ToString();
                    objoiltest.sInvoiceDate = dtDetails.Rows[0]["OSD_INVOICE_DATE"].ToString();
                    objoiltest.sAsdquantity = dtDetails.Rows[0]["OSD_QUANTITY"].ToString();
                    objoiltest.sCron = dtDetails.Rows[0]["OSD_CRON"].ToString();
                    objoiltest.sCrby = dtDetails.Rows[0]["OSD_OFFICE_CODE"].ToString();

                }

                return objoiltest;



            }

            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, "clsFailureEntry", "GetFailureDetails");
                return objoiltest;
            }
            finally
            {

            }

        }

        public string LoadInspectoil(string sOsdid)
        {
            string inspectedqty = string.Empty;
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                //strQry = " Select OSD_ID, OSD_PO_NO,OSD_INVOICE_NO, TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE, OSD_QUANTITY, DIV_NAME as OSD_OFFICE_CODE,OSD_STATUS FROM TBLOILSENTMASTER,TBLDIVISION where OSD_OFFICE_CODE = DIV_CODE and OSD_STATUS = '0'  and OSD_INSP_RESULT ='0' AND OSD_PO_NO = '" + objoiltest.sPurchaseOrderNo + "' and  OSD_ID in (" + objoiltest.sRepairDetailsId + ")";
                strQry = "select sum(AIN_INSP_QTY)from TBLAGENCYINSPECTIONDETAILS where AIN_OSD_ID = '" + sOsdid + "'";

                inspectedqty = ObjCon.get_value(strQry, oleDbCommand);
                return inspectedqty;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return inspectedqty;
            }
            finally
            {

            }
        }


        public string Getagencyname(clsOilTest objoiltest)
        {
            string Agencyname = string.Empty;
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT  RA_NAME FROM TBLREPAIRERAGENCYMASTER where RA_STATUS = 'A'  AND RA_ID = '" + objoiltest.sAgencyValue + "'";

                Agencyname = ObjCon.get_value(strQry, oleDbCommand);
                return Agencyname;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestOrDeliverPendingDTR");
                return Agencyname;
            }
            finally
            {

            }
        }

        public string getdivcode(string sOfficeCode)
        {
            oleDbCommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CODE ='" + sOfficeCode + "'";
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }


        public DataTable ViewBudgetstatusgrid(clsOilTest objbudget)
        {
            oleDbCommand = new OleDbCommand();

            DataTable DtBudgetstatus = new DataTable();
            try
            {

                string strQry = string.Empty;
                string stracccode = string.Empty;

                //  oleDbCommand.Parameters.AddWithValue("budcode", objbudget.budgetaccCode);
                //stracccode = ObjCon.get_value("SELECT MD_NAME  from TBLMASTERDATA WHERE MD_ID=:budcode and MD_TYPE='FMSACHEAD'", oleDbCommand);


                oleDbCommand.Parameters.AddWithValue("accode", objbudget.budgetaccCode);
                oleDbCommand.Parameters.AddWithValue("divcode", objbudget.budgetDivcode);
                oleDbCommand.Parameters.AddWithValue("finyr", objbudget.budgetFinyear);
                strQry = "SELECT BT_ID,BT_ACC_CODE,BT_PONO  ,TO_CHAR(OSD_PO_DATE,'dd-mm-yyyy') as OSD_PO_DATE,BT_BM_AMNT,BT_DEBIT_AMNT,BT_AVL_AMNT,BT_CREDIT_AMNT,BT_CRON,BT_FIN_YEAR,BT_DIV_CODE from TBLBUDGETTRANS LEFT JOIN TBLOILSENTMASTER ON BT_PONO=OSD_PO_NO";
                strQry += " WHERE BT_ACC_CODE='" + objbudget.budgetaccCode + "' and BT_DIV_CODE='" + objbudget.budgetDivcode + "' and BT_FIN_YEAR='" + objbudget.budgetFinyear + "' ORDER BY BT_ID";
                DtBudgetstatus = ObjCon.getDataTable(strQry, oleDbCommand);
                return DtBudgetstatus;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtBudgetstatus;
            }
        }

        public string ViewBudgetstatusaval(clsOilTest objbudget)
        {
            oleDbCommand = new OleDbCommand();
            string Budgetstatusavl = string.Empty;
            try
            {

                string strQry = string.Empty;
                string stracccode = string.Empty;

                oleDbCommand.Parameters.AddWithValue("accode", objbudget.budgetaccCode);
                stracccode = ObjCon.get_value("SELECT MD_NAME  from TBLMASTERDATA WHERE MD_ID=:budcode and MD_TYPE='FMSACHEAD'", oleDbCommand);


                oleDbCommand.Parameters.AddWithValue("acccode", stracccode);
                oleDbCommand.Parameters.AddWithValue("divcode", objbudget.budgetDivcode);
                oleDbCommand.Parameters.AddWithValue("finyr", objbudget.budgetFinyear);
                strQry = "SELECT BT_AVL_AMNT from TBLBUDGETTRANS LEFT JOIN TBLOILSENTMASTER ON BT_PONO=OSD_PO_NO";
                strQry += " WHERE BT_ACC_CODE='" + objbudget.budgetaccCode + "' and BT_DIV_CODE='" + objbudget.budgetDivcode + "' and BT_FIN_YEAR='" + objbudget.budgetFinyear + "' ORDER BY BT_ID DESC";
                Budgetstatusavl = ObjCon.get_value(strQry, oleDbCommand);
                return Budgetstatusavl;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Budgetstatusavl;
            }
        }

        public string getAvailableBudget(clsOilTest objbudget)
        {


            string BudgetAmount = string.Empty;
            try
            {
                //DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();

                ERPService.ERPServiceClient obj = new ERPService.ERPServiceClient();
                string FormName = "Estimation_Form_dtlms";
                DateTime Date = DateTime.Now;
                string sDate = Date.ToString("dd/MM/yyyy");
                BudgetAmount = obj.FetchBudgetAmountForAccountCode("FMS", objbudget.budgetDivcode, objbudget.budgetaccCode, sDate, FormName);
                return BudgetAmount;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getAvailableBudget");
                return BudgetAmount;
            }
        }

    }
}
