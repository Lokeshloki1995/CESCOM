using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.BL
{

    public class clsDTrRepairActivity
    {

        string strFormCode = "clsDTrRepairActivity";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        //Tc Details
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }
        public string sMakeName { get; set; }
        public string sManfDate { get; set; }
        public string sCapacity { get; set; }

        public string sWarrantyPeriod { get; set; }
        public string sSupplierName { get; set; }
        public string sGuarantyType { get; set; }
        public string sMakeId { get; set; }
        public string sStoreId { get; set; }
        public string sSupplierId { get; set; }
        public string sRepairerId { get; set; }
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
        public string slochtlt { get; set; }

        public string sAgencyname { get; set; }
        public string sAgencyName { get; set; }

        public string sPercentage { get; set; }
        public string sOilType { get; set; }
        public string sAmount { get; set; }
        public string sDivCode { get; set; }
        public string sBudgetAccCode { get; set; }
        public string sBudgetAmount { get; set; }
        public string sBMID { get; set; }
        public string sActionType { get; set; }
        public string sID { get; set; }
        public string sWFOId { get; set; }
        public string sWFAutoId { get; set; }
        public string sStarRating { get; set; }
        public string sDivision { get; set; }
        public string sQuantityLtr { get; set; }
        public string sFinancialYear { get; set; }
        public string sInvoicedQty { get; set; }
        public string RepairDetailsId { get; set; }
        public List<string> sListOfRepairDetailsId = new List<string>();
        public string sTcQuantity { get; set; }
        public string sTotalOilQty { get; set; }
        public string sSentOilQty { get; set; }
        public string sPendingOilQty { get; set; }
        public string sRepairerInvoiceNO { get; set; }
        public string Repairsentoil { get; set; }
        public string sOilQtyInKltr { get; set; }
        public string UploadedpathHt { get; set; }
        public string Pototaloilqty { get; set; }
        public string POPendingOilQTY { get; set; }
        public string StoreAlradyIssued { get; set; }
        public string RepairerAlreadyIssued { get; set; }
        public string OilSupplyingBy { get; set; }
        public string RepairerSupplyingOilQTY { get; set; }
        public string RSMOilType { get; set; }
        public string TotalRemaingPendingOilQty { get; set; }
        public string TotalRPoilQtyInKltr { get; set; }
        public Int32 Ins_result_pass { get; set; }

        #region Fault TC Search and Send to Repair
        OleDbCommand oleDbCommand;
        /// <summary>
        /// Load Fault TC
        /// </summary>
        /// <param name="objTcRepair"></param>
        /// <returns></returns>
        public DataTable LoadFaultTC(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                #region nithin given
                strQry = "SELECT  CASE WHEN DTR_CODE IS NOT NULL  THEN 'Already Sent' ELSE 'PENDING' END AS STATUS,IT_ID,IT_CODE,IT_NAME,TC_ID, TC_CODE, TC_SLNO, TM_NAME,";
                strQry += " TC_MANF_DATE,TC_CAPACITY,NVL(RCOUNT,0)RCOUNT,TS_NAME,TC_GUARANTY_TYPE,TC_WARANTY_PERIOD,TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE, ";
                strQry += " (select MD_NAME from TBLMASTERDATA where MD_TYPE='SRT' and MD_ID=TC_STAR_RATE) TC_STAR_RATE ";
                //newly added
                strQry += " ,(select OC_OIL_QUANTITY from TBLOILCAL where OC_STAR_RATING = (case TC_STAR_RATE when 3 then 1when 4 then 1 when 5 then 1 when 6 then 0 end) and OC_CAPACITY = TC_CAPACITY) Amount";
                //
                strQry += " FROM (SELECT IT_ID,IT_CODE,IT_NAME,TC_ID, TC_CODE,TC_PURCHASE_DATE,TC_SLNO, ";
                strQry += " TM_NAME, TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY') TC_MANF_DATE, TO_CHAR(TC_CAPACITY) TC_CAPACITY,RCOUNT,TS_NAME,";
                strQry += " TO_CHAR(TC_WARANTY_PERIOD, 'DD-MON-YYYY') TC_WARANTY_PERIOD,CASE WHEN TC_WARANTY_PERIOD IS NOT NULL THEN (SELECT CASE ";
                strQry += " WHEN SYSDATE < TC_WARANTY_PERIOD THEN RSD_GUARRENTY_TYPE ELSE 'AGP' END AS TC_WARENTY_PERIOD FROM DUAL) WHEN ";
                strQry += " TC_WARANTY_PERIOD IS  null THEN (SELECT DF_GUARANTY_TYPE FROM (SELECT DF_GUARANTY_TYPE,DF_EQUIPMENT_ID,DENSE_RANK() ";
                strQry += " OVER (PARTITION BY DF_EQUIPMENT_ID ORDER BY DF_ID DESC) AS RANKED_DF_ID  FROM TBLDTCFAILURE)A WHERE DF_EQUIPMENT_ID=TC_CODE ";
                strQry += " GROUP BY DF_GUARANTY_TYPE,DF_EQUIPMENT_ID HAVING MIN(RANKED_DF_ID) IN (1) ) END AS TC_GUARANTY_TYPE ,TC_STAR_RATE FROM  ";
                strQry += " (SELECT RSD_GUARRENTY_TYPE,RCOUNT,A.RSD_TC_CODE,RSD_WARENTY_PERIOD FROM (SELECT SUM(CASE WHEN RSD_DELIVARY_DATE IS NOT ";
                strQry += " NULL THEN 1 ELSE 0 END) AS RCOUNT,MAX(RSD_ID) RSD_ID,RSD_TC_CODE FROM TBLREPAIRSENTDETAILS  GROUP BY RSD_TC_CODE )A ";
                strQry += " INNER JOIN  TBLREPAIRSENTDETAILS ON TBLREPAIRSENTDETAILS.RSD_ID=A.RSD_ID AND A.RSD_TC_CODE=TBLREPAIRSENTDETAILS.RSD_TC_CODE)A ";
                strQry += " RIGHT JOIN (SELECT * FROM (SELECT * FROM TBLTCMASTER, TBLTRANSMAKES, TBLSTOREMAST ,TBLMMSITEMMASTER WHERE TC_MAKE_ID=TM_ID AND SM_ID=TC_STORE_ID ";
                strQry += " AND TC_STATUS=3 AND TC_ITEM_ID = IT_ID AND TC_LOCATION_ID LIKE :OfficeCode ||'%' AND TC_CURRENT_LOCATION=1  ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTcRepair.sOfficeCode);
                if (objTcRepair.sTcId != null)
                {
                    strQry += " AND TC_ID IN (:TCID)";
                    oleDbCommand.Parameters.AddWithValue("TCID", objTcRepair.sTcId);
                }

                if (objTcRepair.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY=:Capacity ";
                    oleDbCommand.Parameters.AddWithValue("Capacity", objTcRepair.sCapacity);
                }
                if (objTcRepair.sTcSlno != null)
                {
                    strQry += "AND TC_SLNO LIKE '%'|| :SerialNo || '%'";
                    oleDbCommand.Parameters.AddWithValue("SerialNo", objTcRepair.sTcSlno.Trim());
                }
                if (objTcRepair.sMakeId != null)
                {
                    strQry += " AND TC_MAKE_ID=:MakeID ";
                    oleDbCommand.Parameters.AddWithValue("MakeID", objTcRepair.sMakeId);
                }
                if (objTcRepair.sStoreId != null)
                {
                    strQry += " AND SM_ID=:StoreID";
                    oleDbCommand.Parameters.AddWithValue("StoreID", objTcRepair.sStoreId);
                }
                if (objTcRepair.sStarRating != null)
                {
                    strQry += " AND TC_STAR_RATE=:StarRating";
                    oleDbCommand.Parameters.AddWithValue("StarRating", objTcRepair.sStarRating);
                }
                strQry += " )A LEFT JOIN  TBLTRANSSUPPLIER ON ";
                strQry += " TS_ID = TC_SUPPLIER_ID)A ON RSD_TC_CODE=TC_CODE)A LEFT JOIN ";
                strQry += " (SELECT * FROM (SELECT regexp_substr(WO_DATA_ID, '[^, ]+', 1, level) AS DTR_CODE FROM(select TO_CHAR(listagg(WO_DATA_ID, ', ') ";
                strQry += " within group (order by WO_DATA_ID)) as WO_DATA_ID from TBLWORKFLOWOBJECTS WHERE WO_BO_ID='30' AND WO_APPROVE_STATUS='0' ";
                strQry += " AND WO_DATA_ID IS NOT NULL) connect by regexp_substr(WO_DATA_ID, '[^, ]+', 1, level) is not null)A )B ON TC_CODE=DTR_CODE  ";
                if (objTcRepair.sGuarantyType != null)
                {
                    if (objTcRepair.sGuarantyType == "AGP")
                        strQry += " WHERE TC_GUARANTY_TYPE='AGP' ";
                    else if (objTcRepair.sGuarantyType == "WGP")
                        strQry += " WHERE TC_GUARANTY_TYPE='WGP' ";
                    else
                        strQry += " WHERE TC_GUARANTY_TYPE='WRGP' ";
                }
                #endregion
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        public string[] SaveRepairerOilInvoice(clsDTrRepairActivity objoilinvoice)
        {
            string[] Arr = new string[2];
            bool res = false;
            string[] result = new string[2];
            string RepairerName = string.Empty;
            try
            {

                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;


                strQry = "SELECT ROI_INVOICE_NO FROM TBL_REPAIREROIL_INVOICE WHERE ROI_INVOICE_NO=:Invoiceno ";
                oledbCommand.Parameters.AddWithValue("Invoiceno", objoilinvoice.sInvoiceNo);
                string Exist = ObjCon.get_value(strQry, oledbCommand);
                if ((Exist ?? "").Length > 0)
                {
                    Arr[1] = "2";
                    return Arr;
                }

                oledbCommand.Parameters.AddWithValue("TSID", objoilinvoice.sRepairerId);
                strQry = "SELECT   (SELECT TR_NAME FROM TBLTRANSREPAIRER where TR_ID = A.TR_TR_ID ) || '-' || TQ_NAME || 'TALUK'  || '-' || TR_COMM_ADDRESS   As TR_NAME   FROM  ";
                strQry += " TBLTRANSREPAIRER  A  left join TBLTALQ  on A.TR_LOC_CODE = TQ_SLNO   WHERE TR_ID = '" + objoilinvoice.sRepairerId + "'  ";
                RepairerName = ObjCon.get_value(strQry, oledbCommand);
                ObjCon.BeginTrans();

                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                objoilinvoice.sMMSAutoInvNo = objWCF.GetRepairerAutoINvoiceNum(sOfficeCode);




                if (objoilinvoice.sSentOilQty == "" || objoilinvoice.sSentOilQty == null)
                {
                    objoilinvoice.sSentOilQty = "0";
                }
                double TotalPendingOilQty = Convert.ToDouble(sPendingOilQty) - (Convert.ToDouble(objoilinvoice.sInvoicedQty));
                string Maxid = Convert.ToString(ObjCon.Get_max_no("ROI_ID", "TBL_REPAIREROIL_INVOICE"));
                string Repairsentid = ObjCon.get_value("SELECT RSD_RSM_ID FROM TBLREPAIRSENTDETAILS WHERE RSD_ID='" + objoilinvoice.RepairDetailsId + "'");

                strQry = "INSERT INTO TBL_REPAIREROIL_INVOICE (ROI_ID,ROI_RSM_ID,ROI_TC_QUANTITY,ROI_INVOICE_NO,ROI_MMS_INVOICE_NO,ROI_TOTAL_OILQTY,ROI_INVOICED_OILQTY,ROI_PENDINGQTY,";
                strQry += " ROI_CRON,ROI_CR_BY,ROI_APPROVE_STATUS,ROI_OFFICECODE,ROI_INVOICE_DATE,ROI_INVOICE_QTY_KLTR,ROI_OILSENT_BY,ROI_ITEM_ID) VALUES (";
                strQry += " '" + Maxid + "','" + Repairsentid + "','" + objoilinvoice.sTcQuantity + "','" + objoilinvoice.sInvoiceNo + "','" + objoilinvoice.sMMSAutoInvNo + "','" + objoilinvoice.sTotalOilQty + "',";
                strQry += "'" + objoilinvoice.sInvoicedQty + "','" + TotalPendingOilQty + "',SYSDATE,'" + objoilinvoice.sCrby + "',0,'"
                    + objoilinvoice.sOfficeCode + "',TO_DATE('" + objoilinvoice.sInvoiceDate + "','dd/MM/yyyy'),'"
                    + objoilinvoice.sOilQtyInKltr + "',1,'" + objoilinvoice.sItemCode + "')";
                ObjCon.Execute(strQry);


                strQry = "UPDATE  TBL_REPAIREROIL_INVOICE SET ROI_APPROVE_STATUS = '1' WHERE ROI_ID='" + Maxid + "'";

                strQry = strQry.Replace("'", "''");



                DataTable dt = new DataTable("RepairerOilDetails");
                dt.Columns.Add("RSM_PO_NO", typeof(string));
                dt.Columns.Add("RSD_ITEMID", typeof(string));
                dt.Columns.Add("RSM_PO_DATE", typeof(string));
                dt.Columns.Add("RSM_ITEMCODE", typeof(string));
                dt.Columns.Add("RSM_INV_NO", typeof(string));
                dt.Columns.Add("RSD_INV_NO", typeof(string));
                dt.Columns.Add("RSD_INV_DATE", typeof(string));
                dt.Columns.Add("RSD_DIV_CODE", typeof(string));
                dt.Columns.Add("RSD_INVOICED_QNTY", typeof(string));
                dt.Columns.Add("RSD_INVOICE_QNTY_KLTR", typeof(string));
                dt.Columns.Add("RSD_TOTAL_OILQTY", typeof(string));
                dt.Columns.Add("RSD_PENDING_OILQTY", typeof(string));
                dt.Columns.Add("RSD_MANUAL_INV_NO", typeof(string));
                dt.Columns.Add("RSD_TYPE", typeof(string));
                dt.Columns.Add("RSM_RSD_MMS_AUTO_INV_NUM", typeof(string));
                dt.Columns.Add("RSM_REPAIRER_NAME", typeof(string));
                dt.Columns.Add("RSM_REMARKS", typeof(string));


                DataRow drow = dt.NewRow();

                drow["RSM_PO_NO"] = objoilinvoice.sPurchaseOrderNo.ToUpper();
                drow["RSM_INV_NO"] = objoilinvoice.sRepairerInvoiceNO;
                drow["RSD_INV_NO"] = objoilinvoice.sInvoiceNo;
                DateTime invoice_date = DateTime.ParseExact(objoilinvoice.sInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                drow["RSD_INV_DATE"] = invoice_date.ToString("yyyy-MM-dd");
                drow["RSD_DIV_CODE"] = objoilinvoice.sOfficeCode;
                drow["RSD_INVOICED_QNTY"] = objoilinvoice.sInvoicedQty;
                drow["RSD_INVOICE_QNTY_KLTR"] = objoilinvoice.sOilQtyInKltr;
                drow["RSD_MANUAL_INV_NO"] = objoilinvoice.sManualInvoiceNo;
                drow["RSD_TOTAL_OILQTY"] = objoilinvoice.sTotalOilQty;
                drow["RSD_PENDING_OILQTY"] = TotalPendingOilQty;
                drow["RSD_TYPE"] = "INVOICE";
                drow["RSM_REMARKS"] = "SEND FROM STORE TO REPAIR";
                drow["RSM_RSD_MMS_AUTO_INV_NUM"] = objoilinvoice.sMMSAutoInvNo;
                drow["RSD_ITEMID"] = objoilinvoice.sItemCode;
                drow["RSM_REPAIRER_NAME"] = RepairerName;
                dt.Rows.Add(drow);
                clsApproval objWCF1 = new clsApproval();
                DateTime dtHost = Convert.ToDateTime(ConfigurationSettings.AppSettings["dHost"].ToString());

                if (invoice_date < dtHost)
                {
                    res = true;
                }
                else
                {

                    result = objWCF.saveRepairer_Oilinvoice(dt);

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
                    Arr[0] = "Something Went Wrong";
                    Arr[1] = "0";
                    return Arr;
                }

                //Workflow / Approval
                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = objoilinvoice.sFormName;
                objApproval.sOfficeCode = objoilinvoice.sOfficeCode;
                objApproval.sClientIp = objoilinvoice.sClientIP;
                objApproval.sRecordId = Maxid;
                objApproval.sCrby = objoilinvoice.sCrby;
                objApproval.sWFObjectId = objoilinvoice.sWFOId;
                objApproval.sWFAutoId = objoilinvoice.sWFAutoId;
                objApproval.sBOId = "52";
                objApproval.sQryValues = strQry;
                //  objApproval.sParameterValues = sParam + ";" + sParam1;
                objApproval.sMainTable = "TBL_REPAIREROIL_INVOICE";

                objApproval.sDataReferenceId = objoilinvoice.sPurchaseOrderNo;

                objApproval.sRefOfficeCode = objoilinvoice.sOfficeCode;

                objApproval.sDescription = "Repair Oil Issue With PO No: " + objoilinvoice.sPurchaseOrderNo + " and Invoice No :" + objoilinvoice.sInvoiceNo + " ";

                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "ROI_ID,ROI_RSM_ID,ROI_TC_QUANTITY,ROI_INVOICE_NO,ROI_MMS_INVOICE_NO,ROI_TOTAL_OILQTY,ROI_INVOICED_OILQTY,ROI_PENDINGQTY,ROI_CRON,ROI_CR_BY,ROI_APPROVE_STATUS,ROI_OFFICECODE,RSD_ID,ROI_INVOICE_DATE,ROI_REPAIRERNAME,ROI_INVOICE_QTY_KLTR,REPAIRER_SENT_OIL,ROI_ITEM_ID";

                objApproval.sColumnValues = "" + sPrimaryKey + "," + Repairsentid + "," + objoilinvoice.sTcQuantity + ",";
                objApproval.sColumnValues += "" + objoilinvoice.sInvoiceNo + "," + objoilinvoice.sMMSAutoInvNo + "," + objoilinvoice.sTotalOilQty + "," + objoilinvoice.sInvoicedQty + ",";
                objApproval.sColumnValues += "" + TotalPendingOilQty + ",," + objoilinvoice.sCrby + ",0," + objoilinvoice.sOfficeCode + "," + objoilinvoice.RepairDetailsId + "," + objoilinvoice.sInvoiceDate + "," + RepairerName + "," + objoilinvoice.sOilQtyInKltr + "," + objoilinvoice.Repairsentoil + "," + objoilinvoice.sItemCode + "";
                objApproval.sTableNames = "TBL_REPAIREROIL_INVOICE";

                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }
                objApproval.SaveWorkFlowData(objApproval);
                objApproval.SaveWorkflowObjects(objApproval);

                Arr[0] = "Details Saved Successfully";
                Arr[1] = "1";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Arr[0] = "Something Went Wrong";
                Arr[1] = "0";
                return Arr;
            }
        }
        /// <summary>
        /// Generate Auto RV Number
        /// </summary>
        /// <param name="sOfficeLoc"></param>
        /// <returns></returns>
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }

        }

        public clsDTrRepairActivity GetFaultTCDetails(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = " SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD, ";
                strQry += " (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME , ";
                strQry += " (CASE WHEN TO_CHAR(TC_WARANTY_PERIOD,'YYYYMMDD')<TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'AGP' ";
                strQry += " WHEN TO_CHAR(TC_WARANTY_PERIOD,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'WGP' END )TC_GUARANTY_TYPE ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID  ";
                strQry += " AND TC_CURRENT_LOCATION<>3 AND TC_CODE = :DTRCode ";
                oleDbCommand.Parameters.AddWithValue("DTRCode", objTcRepair.sTcCode);
                if (objTcRepair.sRefString != null)
                {
                    strQry += " AND TC_STATUS=4 ";
                }
                else
                {
                    strQry += " AND TC_STATUS=7 ";
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objTcRepair.sTcId = dt.Rows[0]["TC_ID"].ToString();
                    objTcRepair.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objTcRepair.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objTcRepair.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objTcRepair.sManfDate = dt.Rows[0]["TC_MANF_DATE"].ToString();
                    objTcRepair.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objTcRepair.sPurchaseDate = dt.Rows[0]["TC_PURCHASE_DATE"].ToString();
                    objTcRepair.sWarrantyPeriod = dt.Rows[0]["TC_WARANTY_PERIOD"].ToString();
                    objTcRepair.sSupplierName = dt.Rows[0]["TS_NAME"].ToString();
                    objTcRepair.sGuarantyType = dt.Rows[0]["TC_GUARANTY_TYPE"].ToString();

                }
                return objTcRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objTcRepair;
            }
            finally
            {

            }
        }

        public clsDTrRepairActivity AddFaultTCDetails(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = " SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD, ";
                strQry += " (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME , ";
                strQry += " (CASE WHEN TO_CHAR(TC_WARANTY_PERIOD,'YYYYMMDD')<TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'AGP' ";
                strQry += " WHEN TO_CHAR(TC_WARANTY_PERIOD,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'WGP' END )TC_GUARANTY_TYPE";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID  ";
                strQry += " AND TC_CURRENT_LOCATION<>3 AND TC_CODE = :DTRCode";
                oleDbCommand.Parameters.AddWithValue("DTRCode", objTcRepair.sTcCode);
                if (objTcRepair.sRefString != null)
                {
                    strQry += " AND TC_STATUS=4 ";
                }
                else
                {
                    strQry += " AND TC_STATUS=3 ";
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objTcRepair.sTcId = dt.Rows[0]["TC_ID"].ToString();
                    objTcRepair.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objTcRepair.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objTcRepair.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objTcRepair.sManfDate = dt.Rows[0]["TC_MANF_DATE"].ToString();
                    objTcRepair.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objTcRepair.sPurchaseDate = dt.Rows[0]["TC_PURCHASE_DATE"].ToString();
                    objTcRepair.sWarrantyPeriod = dt.Rows[0]["TC_WARANTY_PERIOD"].ToString();
                    objTcRepair.sSupplierName = dt.Rows[0]["TS_NAME"].ToString();
                    objTcRepair.sGuarantyType = dt.Rows[0]["TC_GUARANTY_TYPE"].ToString();
                }
                return objTcRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objTcRepair;
            }
            finally
            {

            }
        }
        /// <summary>
        /// get Item Qnty
        /// </summary>
        /// <param name="objRepair"></param>
        /// <returns></returns>
        public string[] getItemQnty(clsDTrRepairActivity objRepair)
        {
            string[] arrResult = new string[2];
            try
            {
                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                arrResult = objWCF.get_ItemQty(objRepair.sItemCode, objRepair.sOfficeCode, objRepair.sQty);
                return arrResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return arrResult;
            }
        }
        /// <summary>
        /// Load Repair Sent DTR
        /// </summary>
        /// <param name="sRepairMasterId"></param>
        /// <param name="sNthTime"></param>
        /// <returns></returns>
        public DataTable LoadRepairSentDTR(string sRepairMasterId, string sNthTime)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD, ";
                strQry += " (select MD_NAME from TBLMASTERDATA where MD_TYPE='SRT' and MD_ID=TC_STAR_RATE) TC_STAR_RATE ";
                strQry += " ,(select OC_OIL_QUANTITY from TBLOILCAL where OC_STAR_RATING = (case TC_STAR_RATE when 3 then 1 when 4 then 1 ";
                strQry += " when 5 then 1 when 6 then 0 end) and OC_CAPACITY = TC_CAPACITY) Amount , ";
                strQry += " (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME, ";
                strQry += " (SELECT IT_CODE from TBLMMSITEMMASTER WHERE IT_ID = TC_ITEM_ID) IT_CODE, ";
                strQry += "(SELECT IT_NAME from TBLMMSITEMMASTER WHERE IT_ID = TC_ITEM_ID) IT_NAME, ";
                strQry += " (SELECT IT_ID from TBLMMSITEMMASTER WHERE IT_ID = TC_ITEM_ID) IT_ID, ";
                strQry += " (CASE WHEN TO_CHAR(TC_WARANTY_PERIOD,'YYYYMMDD')<TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'AGP' ";
                strQry += " WHEN TO_CHAR(TC_WARANTY_PERIOD,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'WGP' END )TC_GUARANTY_TYPE ";
                strQry += " FROM TBLREPAIRSENTDETAILS,TBLTCMASTER,TBLTRANSMAKES,TBLREPAIRSENTMASTER WHERE RSD_TC_CODE=TC_CODE AND  TC_MAKE_ID=TM_ID AND ";
                strQry += " RSD_RSM_ID=RSM_ID AND RSM_ID=:RSMID AND RSD_NTH_TIME = :COUNT";
                oleDbCommand.Parameters.AddWithValue("RSMID", sRepairMasterId);
                oleDbCommand.Parameters.AddWithValue("COUNT", sNthTime);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Check with Or without Oil
        /// </summary>
        /// <returns></returns>
        public string CheckwithOrwithoutOil()
        {
            string Result = string.Empty;
            Result = "SELECT RSM_OIL_TYPE FROM TBLREPAIRSENTMASTER";
            return Result;
        }
        /// <summary>
        /// Save Repair Issue Details
        /// </summary>
        /// <param name="sTcCodes"></param>
        /// <param name="objTcRepair"></param>
        /// <returns></returns>
        public string[] SaveRepairIssueDetails(string[] sTcCodes, clsDTrRepairActivity objTcRepair)
        {
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);

            string[] Arr = new string[2];
            string strQry = string.Empty;
            bool bResult = false;
            bool res = false;
            string sDTrCode = string.Empty;
            string TC_CODE = string.Empty;
            string ITEM_CODES = string.Empty;
            string sDTrCodes = string.Empty;
            string[] statusArray = new string[2];
            string SupplierName = string.Empty;
            int soldQnty = 0;

            DateTime invoice_date = new DateTime();
            DateTime dtHost = Convert.ToDateTime(ConfigurationManager.AppSettings["dHost"].ToString());
            try
            {
                oleDbCommand = new OleDbCommand();
                strQry = " SELECT * FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER ";
                strQry += " WHERE RSD_RSM_ID = RSM_ID AND RSD_MAN_INV_NO = :INVOICENO AND  RSM_DIV_CODE = :DivisionCode ";
                oleDbCommand.Parameters.AddWithValue("INVOICENO", objTcRepair.sManualInvoiceNo);
                oleDbCommand.Parameters.AddWithValue("DivisionCode", objTcRepair.sOfficeCode);
                DataTable dtmanualInvNo = new DataTable();
                dtmanualInvNo = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dtmanualInvNo.Rows.Count > 0)
                {
                    Arr[0] = "Invoice Number already exists";
                    Arr[1] = "2";
                    return Arr;
                }
                string[] strDtrDetailVal = sTcCodes.ToArray();
                for (int i = 0; i < strDtrDetailVal.Length; i++)
                {
                    oleDbCommand = new OleDbCommand();
                    strQry = " select RSD_TC_CODE,RSM_PO_NO  from TBLREPAIRSENTDETAILS inner join TBLREPAIRSENTMASTER on RSD_RSM_ID=RSM_ID ";
                    strQry += " where RSD_RV_NO is null and  RSD_RV_DATE is null and ";
                    strQry += " RSD_TC_CODE in ('" + strDtrDetailVal[i].Split('~').GetValue(0).ToString() + "') ";

                    DataTable dtCheckDtrPendInRep = new DataTable();
                    dtCheckDtrPendInRep = ObjCon.getDataTable(strQry, oleDbCommand);
                    if (dtCheckDtrPendInRep.Rows.Count > 0)
                    {
                        string TcCode = Convert.ToString(dtCheckDtrPendInRep.Rows[0]["RSD_TC_CODE"]);
                        string PoNo = Convert.ToString(dtCheckDtrPendInRep.Rows[0]["RSM_PO_NO"]);
                        Arr[0] = "DTr " + TcCode + "  already Invoiced To Repairer and PO No: " + PoNo + " ";
                        Arr[1] = "2";
                        return Arr;
                    }
                }


                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                string[] strDetailVal = sTcCodes.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    TC_CODE += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";

                }

                TC_CODE = TC_CODE.Remove(TC_CODE.Length - 1);


                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    strQry = " UPDATE TBLTCMASTER SET TC_ITEM_ID = '" + strDetailVal[i].Split('~').GetValue(2).ToString() + "' ";
                    strQry += " WHERE TC_CODE in ('" + strDetailVal[i].Split('~').GetValue(0).ToString() + "') ";
                    ObjCon.Execute(strQry);
                    objWCF.SaveTcDetails(strQry);

                    sDTrCodes += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";

                }

                if (objTcRepair.sType == "1")
                {
                    oleDbCommand = new OleDbCommand();
                    oleDbCommand.Parameters.AddWithValue("TSID", objTcRepair.sSupRepId);
                    SupplierName = ObjCon.get_value("SELECT TS_NAME  FROM TBLTRANSSUPPLIER WHERE TS_ID = :TSID", oleDbCommand);

                }
                else
                {
                    oleDbCommand = new OleDbCommand();
                    oleDbCommand.Parameters.AddWithValue("TSID", objTcRepair.sSupRepId);
                    strQry = "SELECT   (SELECT TR_NAME FROM TBLTRANSREPAIRER where TR_ID = A.TR_TR_ID ) || '-' || TQ_NAME || 'TALUK'  || '-' || TR_COMM_ADDRESS   As TR_NAME   FROM  ";
                    strQry += " TBLTRANSREPAIRER  A  left join TBLTALQ  on A.TR_LOC_CODE = TQ_SLNO   WHERE TR_ID = :TSID  ";
                    SupplierName = ObjCon.get_value(strQry, oleDbCommand);
                }
                strQry = "SELECT  TO_CHAR(CASE WHEN (TC_STATUS=1 AND WARRENTY_TYPE='1') THEN ITM_BRAND_NEW  WHEN TC_STATUS=3 THEN ITM_FAULTY_AGP WHEN (TC_STATUS=3 AND ";
                strQry += " (WARRENTY_TYPE='AGP')) THEN ITM_FAULTY_AGP WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='WGP' OR  WARRENTY_TYPE ='WRGP')) THEN ";
                strQry += " ITM_FAULTY_WGP WHEN (TC_STATUS=2 AND WARRENTY_TYPE='2') THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND  (WARRENTY_TYPE='AGP') AND ";
                strQry += " TC_MAKE_ID ='98') THEN ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN  ITM_REPAIRE_GOOD_ABB_MAKE WHEN ";
                strQry += " TC_STATUS ='4' THEN ITM_SCRAPE WHEN WARRENTY_TYPE=0 THEN 0 END) ITEM_CODE,TC_ID,TC_CODE FROM (SELECT CASE WHEN SYSDATE < ";
                strQry += " TC_WARANTY_PERIOD THEN (SELECT RSD_GUARRENTY_TYPE FROM (SELECT RSD_GUARRENTY_TYPE,RSD_TC_CODE,DENSE_RANK()  OVER (PARTITION BY ";
                strQry += " RSD_TC_CODE ORDER BY RSD_ID DESC) AS RANKED_RSD_ID  FROM TBLREPAIRSENTDETAILS WHERE  RSD_GUARRENTY_TYPE IS NOT NULL ";
                strQry += " )A where  TC_CODE=RSD_TC_CODE  GROUP BY RSD_GUARRENTY_TYPE,RSD_TC_CODE HAVING MIN(RANKED_RSD_ID) IN (1)) WHEN DF_GUARANTY_TYPE IS NOT ";
                strQry += " NULL AND TC_WARANTY_PERIOD IS NULL THEN DF_GUARANTY_TYPE WHEN TC_STATUS='1'  THEN '1' WHEN TC_STATUS='2' THEN '2' WHEN ";
                strQry += " TC_STATUS ='3' THEN '3' ELSE '0'  END AS WARRENTY_TYPE,TC_ID,TC_CODE, TC_CAPACITY,TC_STATUS,ITM_FAULTY_AGP,ITM_FAULTY_WGP,";
                strQry += " ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE, ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,TC_MAKE_ID FROM (SELECT * FROM ";
                strQry += " TBLTCMASTER LEFT JOIN TBLITEMPRICEMASTER ON TC_CAPACITY =ITM_CAPACITY)A LEFT JOIN (SELECT DF_GUARANTY_TYPE,DF_EQUIPMENT_ID FROM ";
                strQry += "(SELECT DF_GUARANTY_TYPE,DF_EQUIPMENT_ID,DENSE_RANK()  OVER (PARTITION BY DF_EQUIPMENT_ID ORDER BY DF_ID DESC) AS RANKED_DF_ID  ";
                strQry += "FROM TBLDTCFAILURE)A  GROUP BY DF_GUARANTY_TYPE,DF_EQUIPMENT_ID HAVING MIN(RANKED_DF_ID) IN (1))B ON A.TC_CODE=B.DF_EQUIPMENT_ID ";
                strQry += "WHERE  TC_CODE IN (" + TC_CODE + "))";

                strQry = "SELECT  to_char(TC_ITEM_ID) ITEM_CODE, TC_ID ,to_char(TC_CODE) TC_CODE FROM TBLTCMASTER  WHERE  TC_CODE IN (" + TC_CODE + ")";

                DataTable dtItem_code = ObjCon.getDataTable(strQry);

                DataTable dr = ObjCon.getDataTable("SELECT RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE UPPER(RSM_PO_NO)='" + objTcRepair.sPurchaseOrderNo.ToUpper() + "' AND RSM_DIV_CODE= " + objTcRepair.sOfficeCode + "");
                DataTable dt1 = new DataTable();
                if (objTcRepair.sIsOldPo == true)
                {
                    oleDbCommand = new OleDbCommand();
                    oleDbCommand.Parameters.AddWithValue("PONONUM", objTcRepair.sPurchaseOrderNo.ToUpper());
                    oleDbCommand.Parameters.AddWithValue("DivisionCode23", objTcRepair.sOfficeCode);
                    strQry = "SELECT RSM_ID,RSM_PO_QNTY,RSM_REMARKS  FROM TBLREPAIRSENTMASTER WHERE RSM_PO_NO =:PONONUM AND RSM_DIV_CODE =:DivisionCode23";

                    dt1 = ObjCon.getDataTable(strQry, oleDbCommand);
                    if (dt1.Rows.Count > 0)
                    {
                        sRepairMasterId = dt1.Rows[0]["RSM_ID"].ToString();
                    }
                    // so that for the same PO he cannot send untill the record is approved for the previous one for the same po 
                    strQry = "SELECT * FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID = '" + sRepairMasterId + "' AND WO_BO_ID = '30' AND WO_APPROVE_STATUS = '0' ";
                    strQry += " AND WO_NEXT_ROLE = '2'";
                    if (!(ObjCon.getDataTable(strQry).Rows.Count == 0))
                    {
                        Arr[0] = "Please Approve the previously sent record  for the same PO Number in Store Officer Inbox";
                        Arr[1] = "2";
                        return Arr;
                    }

                }
                strQry = " SELECT COALESCE(MAX(RSD_NTH_TIME)+1,1) from TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID  = '" + sRepairMasterId + "'";
                string nthtime = ObjCon.get_value(strQry);
                objTcRepair.sMMSAutoInvNo = objWCF.GetRepairerAutoINvoiceNum(sOfficeCode);

                ObjCon.BeginTrans();


                if (objTcRepair.sOfficeCode.Length > 2)
                {
                    objTcRepair.sOfficeCode = objTcRepair.sOfficeCode.Substring(0, 2);
                }


                //for new po there shouldnt be old po number
                if (dr.Rows.Count > 0 && objTcRepair.sIsOldPo == false)
                {
                    Arr[0] = "Purchase Order Number " + objTcRepair.sPurchaseOrderNo.ToUpper() + " Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }
                string soilstatus = string.Empty;
                if (objTcRepair.sType == "2")
                {
                    string strQry1 = "select TR_OIL_TYPE from TBLTRANSREPAIRER where TR_ID='" + objTcRepair.sSupRepId + "'";
                    soilstatus = ObjCon.get_value(strQry1);
                    if (soilstatus != "1")
                    {
                        soilstatus = "0";
                    }
                    if (objTcRepair.sGuarantyType.ToUpper() == "WRGP")
                    {
                        soilstatus = "1";
                    }

                }


                //add to master only if its new po 
                if (objTcRepair.sIsOldPo == false)
                {
                    // TR_TYPE :  2----> Repairer   1----> Supplier
                    sRepairMasterId = Convert.ToString(ObjCon.Get_max_no("RSM_ID", "TBLREPAIRSENTMASTER"));
                    if (sTcCodes.Length > 0)
                    {
                        strQry = "INSERT INTO TBLREPAIRSENTMASTER (RSM_ID,RSM_ISSUE_DATE,RSM_PO_NO,RSM_PO_DATE,RSM_INV_NO,RSM_INV_DATE,RSM_GUARANTY_TYPE,RSM_SUPREP_TYPE,";
                        strQry += " RSM_SUPREP_ID,RSM_DIV_CODE,RSM_CRBY,RSM_PO_QNTY,RSM_MANUAL_INV_NO,RSM_OLD_PO_NO,RSM_REMARKS,RSM_HTLT_DIV_CODE,RSM_OIL_TYPE ) VALUES (";
                        strQry += " '" + sRepairMasterId + "',TO_DATE('" + objTcRepair.sIssueDate + "','DD/MM/YYYY'),'" + objTcRepair.sPurchaseOrderNo.ToUpper() + "',";
                        strQry += " TO_DATE('" + objTcRepair.sPurchaseDate + "','DD/MM/YYYY'),'" + objTcRepair.sInvoiceNo + "',TO_DATE('" + objTcRepair.sInvoiceDate + "','DD/MM/YYYY'),";
                        strQry += " '" + objTcRepair.sGuarantyType + "','" + objTcRepair.sType + "','" + objTcRepair.sSupRepId + "',";
                        strQry += " '" + objTcRepair.sOfficeCode + "','" + objTcRepair.sCrby + "','" + objTcRepair.sQty + "','" + objTcRepair.sManualInvoiceNo + "', ";
                        strQry += " '" + objTcRepair.sOldPONo + "','" + objTcRepair.sPORemarks + "','" + objTcRepair.slochtlt + "','" + soilstatus + "')";
                        ObjCon.Execute(strQry);
                    }
                }
                else
                {

                    if (dt1.Rows.Count > 0)
                    {
                        sRepairMasterId = dt1.Rows[0]["RSM_ID"].ToString();
                        soldQnty = Convert.ToInt32(dt1.Rows[0]["RSM_PO_QNTY"].ToString());
                        objTcRepair.sQty = Convert.ToString(soldQnty + Convert.ToInt32(objTcRepair.sQty));
                        //objTcRepair.sPORemarks = dt1.Rows[0]["RSM_REMARKS"].ToString()+";"+objTcRepair.sPORemarks;
                    }
                    strQry = "UPDATE TBLREPAIRSENTMASTER SET RSM_PO_QNTY = '" + objTcRepair.sQty + "' WHERE RSM_ID = '" + sRepairMasterId + "'";
                    ObjCon.Execute(strQry);
                }



                string str = string.Empty;
                string[] result = new string[2];
                for (int i = 0; i < strDetailVal.Length; i++)
                {

                    string sRepairMasterDetailsId = ObjCon.Get_max_no("RSD_ID", "TBLREPAIRSENTDETAILS").ToString();
                    strQry = " INSERT INTO TBLREPAIRSENTDETAILS (RSD_ID,RSD_RSM_ID,RSD_TC_CODE,RSD_CRBY,RSD_INV_NO,RSD_INV_DATE,RSD_MAN_INV_NO,RSD_NTH_TIME,RSD_OIL_QTY,RSD_ISSUE_DATE,RSD_REMARKS,RSD_MMS_AUTO_INV_NO,RSD_OIL_QUANTITY,RSD_OIL_APPROVE_STATUS) ";
                    strQry += " VALUES ('" + sRepairMasterDetailsId + "','" + sRepairMasterId + "',";
                    strQry += " '" + strDetailVal[i].Split('~').GetValue(0).ToString() + "','" + objTcRepair.sCrby + "','" + objTcRepair.sManualInvoiceNo + "',";
                    strQry += "TO_DATE('" + objTcRepair.sInvoiceDate + "','DD/MM/YYYY'),'" + objTcRepair.sManualInvoiceNo + "','" + nthtime + "','" + objTcRepair.sOilQty + "',TO_DATE('" + objTcRepair.sIssueDate + "','DD/MM/YYYY'),'" + objTcRepair.sPORemarks + "','" + objTcRepair.sMMSAutoInvNo + "','" + strDetailVal[i].Split('~').GetValue(3).ToString() + "','0')";
                    ObjCon.Execute(strQry);
                    str += strQry + ";";

                    strQry = "UPDATE TBLTCMASTER SET TC_ITEM_ID='" + strDetailVal[i].Split('~').GetValue(2).ToString() + "'  ,TC_CURRENT_LOCATION=3,TC_UPDATED_EVENT='REPAIRER ISSUE',TC_UPDATED_EVENT_ID='" + sRepairMasterDetailsId + "',";
                    strQry += "TC_LAST_REPAIRER_ID='" + objTcRepair.sSupRepId + "' where TC_CODE='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                    //ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLTCMASTER SET TC_ITEM_ID='" + strDetailVal[i].Split('~').GetValue(2).ToString() + "'   ,TC_UPDATED_EVENT_ID='" + sRepairMasterDetailsId + "',";
                    strQry += "TC_LAST_REPAIRER_ID='" + objTcRepair.sSupRepId + "' where TC_CODE='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                    ObjCon.Execute(strQry);
                    str += strQry + ";";
                    sDTrCode += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";
                    bResult = true;

                }
                DataTable dt = new DataTable("Repair_Masrter");
                dt.Columns.Add("RSM_ID", typeof(string));
                dt.Columns.Add("RSM_ISSUE_DATE", typeof(string));
                dt.Columns.Add("RSM_PO_NO", typeof(string));
                dt.Columns.Add("RSM_PO_DATE", typeof(string));
                dt.Columns.Add("RSM_INV_NO", typeof(string));
                dt.Columns.Add("RSM_INV_DATE", typeof(string));
                dt.Columns.Add("RSM_GUARANTY_TYPE", typeof(string));
                dt.Columns.Add("RSM_DIV_CODE", typeof(string));
                dt.Columns.Add("RSM_PO_QNTY", typeof(string));
                dt.Columns.Add("RSM_MANUAL_INV_NO", typeof(string));
                dt.Columns.Add("RSM_TYPE", typeof(string));
                dt.Columns.Add("RSM_REMARKS", typeof(string));
                dt.Columns.Add("RSM_TC_CODE", typeof(string));
                dt.Columns.Add("RSM_REPAIRER_NAME", typeof(string));
                dt.Columns.Add("OIL", typeof(string));
                dt.Columns.Add("RSM_RSD_MMS_AUTO_INV_NUM", typeof(string));
                //dt.Columns.Add("RSM_INV_RSD_ID", typeof(string));

                DataRow drow = dt.NewRow();
                drow["RSM_ID"] = sRepairMasterId;
                DateTime issue_date = DateTime.ParseExact(objTcRepair.sIssueDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                invoice_date = issue_date;
                drow["RSM_ISSUE_DATE"] = issue_date.ToString("yyyy-MM-dd");
                drow["RSM_PO_NO"] = objTcRepair.sPurchaseOrderNo.ToUpper();
                issue_date = DateTime.ParseExact(objTcRepair.sPurchaseDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                drow["RSM_PO_DATE"] = issue_date.ToString("yyyy-MM-dd");
                drow["RSM_INV_NO"] = objTcRepair.sInvoiceNo;
                issue_date = DateTime.ParseExact(objTcRepair.sInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                drow["RSM_INV_DATE"] = issue_date.ToString("yyyy-MM-dd");
                drow["RSM_GUARANTY_TYPE"] = objTcRepair.sGuarantyType;
                drow["RSM_DIV_CODE"] = objTcRepair.sOfficeCode;
                drow["RSM_PO_QNTY"] = objTcRepair.sQty;
                drow["RSM_MANUAL_INV_NO"] = objTcRepair.sManualInvoiceNo;
                drow["RSM_TYPE"] = "INVOICE";
                drow["RSM_REMARKS"] = "SEND FROM STORE TO REPAIR";
                drow["RSM_TC_CODE"] = TC_CODE;
                drow["RSM_REPAIRER_NAME"] = SupplierName;
                drow["OIL"] = objTcRepair.sOilQty;
                drow["RSM_RSD_MMS_AUTO_INV_NUM"] = objTcRepair.sMMSAutoInvNo;
                dt.Rows.Add(drow);
                clsApproval objWCF1 = new clsApproval();
                if (invoice_date < dtHost)
                {
                    res = true;
                }
                else
                {
                    result = objWCF.saveRepairer_invoice(dt, dtItem_code);

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


                if (bResult == true)
                {

                    #region WorkFlow

                    clsApproval objApproval = new clsApproval();

                    if (!sDTrCode.StartsWith(","))
                    {
                        sDTrCode = "," + sDTrCode;
                    }
                    if (!sDTrCode.EndsWith(","))
                    {
                        sDTrCode = sDTrCode + ",";
                    }

                    sDTrCode = sDTrCode.Substring(1, sDTrCode.Length - 1);
                    sDTrCode = sDTrCode.Substring(0, sDTrCode.Length - 1);

                    strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=3,TC_UPDATED_EVENT='REPAIRER ISSUE',";
                    strQry += "TC_LAST_REPAIRER_ID='" + objTcRepair.sSupRepId + "' WHERE TC_CODE IN (" + sDTrCode + ")";

                    strQry = strQry.Replace("'", "''");


                    objApproval.sFormName = objTcRepair.sFormName;
                    objApproval.sRecordId = sRepairMasterId;
                    objApproval.sOfficeCode = objTcRepair.sOfficeCode;
                    objApproval.sClientIp = objTcRepair.sClientIP;
                    objApproval.sCrby = objTcRepair.sCrby;
                    objApproval.sWFObjectId = objTcRepair.sWFObjectId;
                    objApproval.sDataReferenceId = sDTrCode;
                    objApproval.sRefOfficeCode = objTcRepair.sOfficeCode;

                    objApproval.sQryValues = strQry;

                    if (objTcRepair.sIsOldPo == true)
                    {
                        objApproval.sIsOldPo = true;
                    }
                    else
                    {
                        objApproval.sIsOldPo = false;
                    }
                    objApproval.sMainTable = "TBLREPAIRSENTMASTER";

                    objApproval.sDescription = "Faulty Transformer issue to Supplier / Repairer with Invoice NO " + objTcRepair.sInvoiceNo;

                    objconn.BeginTrans();
                    objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                    objApproval.SaveWorkflowObjectslatest(objApproval, objconn);

                    #endregion

                    #region WCF

                    if (res == false && objTcRepair.sIsOldPo == false)
                    {
                        strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID='" + sRepairMasterId + "' AND WO_BO_ID='30'";
                        ObjCon.Execute(strQry);

                        strQry = "DELETE FROM TBLREPAIRSENTMASTER WHERE RSM_ID='" + sRepairMasterId + "'";
                        ObjCon.Execute(strQry);

                        strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID in ( SELECT DRT_ID  FROM TBLDTRTRANSACTION ,TBLREPAIRSENTDETAILS  WHERE DRT_DTR_CODE in (" + TC_CODE + ") and DRT_ACT_REFTYPE = 4 and DRT_ACT_REFNO = RSD_ID and RSD_DELIVARY_DATE is null )";
                        ObjCon.Execute(strQry);

                        strQry = "DELETE FROM TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID='" + sRepairMasterId + "'";
                        ObjCon.Execute(strQry);
                        if (result[1] == "0")
                        {
                            Arr[0] = "Item Quantity Out Of Stock";

                            Arr[1] = "2";
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "Something Went Wrong";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    if (res == false && objTcRepair.sIsOldPo == true)
                    {


                        strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID='" + sRepairMasterId + "' AND WO_BO_ID='30' AND WO_DATA_ID = '" + TC_CODE + "'";
                        ObjCon.Execute(strQry);

                        strQry = "UPDATE  TBLREPAIRSENTMASTER SET RSM_PO_QNTY = '" + soldQnty + "' WHERE RSM_ID='" + sRepairMasterId + "'";
                        ObjCon.Execute(strQry);

                        strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID in ( SELECT DRT_ID  FROM TBLDTRTRANSACTION ,TBLREPAIRSENTDETAILS  WHERE DRT_DTR_CODE in (" + TC_CODE + ") and DRT_ACT_REFTYPE = 4 and DRT_ACT_REFNO = RSD_ID  and RSD_DELIVARY_DATE is null)";
                        ObjCon.Execute(strQry);

                        strQry = "DELETE FROM TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID='" + sRepairMasterId + "' AND RSD_NTH_TIME = '" + nthtime + "' AND RSD_TC_CODE in (" + TC_CODE + ")";
                        ObjCon.Execute(strQry);


                        if (result[1] == "0")
                        {
                            Arr[0] = "Item Quantity Out Of Stock";
                            Arr[1] = "2";
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "Something Went Wrong";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }

                    #endregion

                    Arr[0] = "Transformers Issued Sucessfully to Repairer/Supplier";
                    Arr[1] = "0";
                }
                else
                {
                    Arr[0] = "No Transformer Exists to Issue for Repairer/Supplier";
                    Arr[1] = "2";
                }

                return Arr;

            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                if (objconn != null)
                {
                    objconn.RollBack();
                }
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);

                strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID in ( SELECT DRT_ID  FROM TBLDTRTRANSACTION ,TBLREPAIRSENTDETAILS  WHERE DRT_DTR_CODE in (" + TC_CODE + ") and DRT_ACT_REFTYPE = 4 and DRT_ACT_REFNO = RSD_ID and RSD_DELIVARY_DATE is null )";
                ObjCon.Execute(strQry);


                if (objTcRepair.sIsOldPo == false)
                {
                    strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID='" + sRepairMasterId + "' AND WO_BO_ID='30'";
                    ObjCon.Execute(strQry);
                    strQry = "DELETE FROM TBLREPAIRSENTMASTER WHERE RSM_ID='" + sRepairMasterId + "'";
                    ObjCon.Execute(strQry);
                    strQry = "DELETE FROM TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID='" + sRepairMasterId + "'";
                    ObjCon.Execute(strQry);
                }
                else
                {
                    strQry = "DELETE FROM TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID='" + sRepairMasterId + "' AND RSD_TC_CODE in (" + TC_CODE + ")";
                    ObjCon.Execute(strQry);
                }


                Arr[0] = "Something Went Wrong Contact Support team for more details";
                Arr[1] = "2";
                return Arr;
            }

        }
        #endregion



        #region Testing Activity
        /// <summary>
        /// Load Test Or DeliverPending DTR
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public DataTable LoadTestOrDeliverPendingDTR(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                string oiltype = string.Empty;

                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    oiltype = GetOilTypeByPoNo(objTestPending.sPurchaseOrderNo);
                }
                strQry = " SELECT RSM_PO_NO,TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE, RSD_ID, TC_CODE,TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) TM_NAME, ";
                strQry += " TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME,RSM_OLD_PO_NO,RSM_REMARKS,RSM_OIL_TYPE  ";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER WHERE  RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND TC_CURRENT_LOCATION='3' ";
                strQry += " AND RSD_DELIVARY_DATE IS NULL AND RSD_ID NOT IN (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT IN(1,3,4,5,6,7))";
                strQry += " AND RSM_HTLT_DIV_CODE LIKE '" + objTestPending.sOfficeCode + "%'";



                if (objTestPending.sRoleId == "2")
                {
                    strQry += " AND RSM_STO_STATUS_FLAG ='1' ";
                }
                if (objTestPending.sRoleId == "10")
                {
                    strQry += " AND RSM_STO_STATUS_FLAG is null  ";
                }
                if (objTestPending.sRepairDetailsId != null)
                {
                    strQry += " AND RSD_ID IN (" + objTestPending.sRepairDetailsId + ") ";
                }
                if (objTestPending.sRepairerId != null)
                {
                    strQry += " AND RSM_SUPREP_ID=:SupplierID AND RSM_SUPREP_TYPE='2'";
                    oleDbCommand.Parameters.AddWithValue("SupplierID", objTestPending.sRepairerId.ToString().ToUpper());
                }

                if (objTestPending.sSupplierId != null)
                {
                    strQry += " AND RSM_SUPREP_ID=:SupplierID1 AND RSM_SUPREP_TYPE='1'";
                    oleDbCommand.Parameters.AddWithValue("SupplierID1", objTestPending.sSupplierId.ToString().ToUpper());
                }
                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)='" + objTestPending.sPurchaseOrderNo.ToString().ToUpper() + "'";
                }
                if (objTestPending.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY='" + objTestPending.sCapacity + "'";
                }
                if (objTestPending.sMakeId != null)
                {
                    strQry += " AND TC_MAKE_ID='" + Convert.ToString(objTestPending.sMakeId) + "'";
                }

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
        }
        /// <summary>
        /// Save Testing TCDetails
        /// </summary>
        /// <param name="strRepairDetailsIds"></param>
        /// <param name="objpending"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string[] SaveTestingTCDetails(string[] strRepairDetailsIds, clsDTrRepairActivity objpending, DataTable dt)
        {
            string[] Arr = new string[2];
            string sFilePath = string.Empty;
            string strQry = string.Empty;
            try
            {
                if (objpending.RSMOilType == "0" && objpending.RepairerSupplyingOilQTY != null)
                {
                    string RSM_ID = ObjCon.get_value("SELECT RSM_ID FROM TBLREPAIRSENTMASTER WHERE RSM_PO_NO='" + objpending.sPurchaseOrderNo + "'");

                    string MaXROI_ID = ObjCon.get_value(" SELECT NVL(MAX(ROI_ID),0)+1 as ROI_ID from TBL_REPAIREROIL_INVOICE");

                    strQry = "INSERT INTO TBL_REPAIREROIL_INVOICE (ROI_ID,ROI_RSM_ID,ROI_TOTAL_OILQTY,ROI_INVOICED_OILQTY,ROI_PENDINGQTY,";
                    strQry += " ROI_CRON,ROI_CR_BY,ROI_OFFICECODE,ROI_INVOICE_QTY_KLTR,ROI_OILSENT_BY) VALUES (";
                    strQry += " '" + MaXROI_ID + "','" + RSM_ID + "','" + objpending.Pototaloilqty + "',";
                    strQry += "'" + objpending.RepairerSupplyingOilQTY + "','" + objpending.TotalRemaingPendingOilQty + "',SYSDATE,'" + objpending.sCrby + "','" + objpending.sOfficeCode + "' ";
                    strQry += " ,'" + objpending.TotalRPoilQtyInKltr + "',2)";

                    ObjCon.Execute(strQry);
                }
                string[] strDetailVal = strRepairDetailsIds.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {

                        string IND_ID = ObjCon.get_value("select IND_ID from TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT in(1,3,4,5,6,7) AND IND_RSD_ID = '"
                                           + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "' FETCH FIRST 1 ROWS ONLY");
                        if ((IND_ID ?? "").Length > 0)
                        {
                            Arr[0] = "Already Testing Done";
                            Arr[1] = "0";
                            return Arr;
                        }
                    }
                }

                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        strQry = string.Empty;

                        int? Ins_result = (strQry == "" ? null : (int?)Convert.ToInt32(strQry));

                        if (ConfigurationManager.AppSettings["INS_RESULTWITHPASS"].Contains(strDetailVal[i].Split('~').GetValue(1).ToString()))
                        {
                            Ins_result = 0;
                        }

                        string sInspectionId = Convert.ToString(ObjCon.Get_max_no("IND_ID", "TBLINSPECTIONDETAILS"));
                        strQry = "INSERT INTO TBLINSPECTIONDETAILS (IND_ID,IND_RSD_ID,IND_INSP_BY,IND_INSP_DATE,IND_TEST_LOCATION, ";
                        strQry += " IND_INSP_RESULT,IND_REMARKS,IND_CRBY,IND_EE_APPROVE_STATUS)";
                        strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "', ";
                        strQry += " '" + objpending.sTestedBy + "',TO_DATE('" + objpending.sTestedOn + "','DD/MM/YYYY'),";
                        strQry += " '" + objpending.sTestLocation + "','" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "', ";
                        strQry += " '" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objpending.sCrby + "', ";
                        strQry += " '" + Ins_result + "')";
                        ObjCon.Execute(strQry);

                        Arr[0] = "Testing Done Successfully";
                        Arr[1] = "0";
                    }
                }
                return Arr;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Arr;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Load Tested DTR
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public clsDTrRepairActivity LoadTestedDTR(clsDTrRepairActivity objTestPending)
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
                strQry += " IND_INSP_BY,TO_CHAR(IND_INSP_DATE,'DD-MON-YYYY') IND_INSP_DATE,IND_TEST_LOCATION,IND_INSP_RESULT,IND_REMARKS,RSM_REMARKS";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLTCMASTER,TBLINSPECTIONDETAILS WHERE  RSM_ID = RSD_RSM_ID AND ";
                strQry += " RSD_TC_CODE = TC_CODE AND IND_INSP_RESULT in(0,1,3,4,5,6,7) AND IND_RSD_ID=RSD_ID AND TC_CODE=:TCCode ";
                strQry += " AND IND_ID=:InspectionID";
                oleDbCommand.Parameters.AddWithValue("TCCode", objTestPending.sTcCode);
                oleDbCommand.Parameters.AddWithValue("InspectionID", objTestPending.sTestInspectionId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objTestPending.dtTestDone = dt;
                    objTestPending.sInspRemarks = Convert.ToString(dt.Rows[0]["IND_REMARKS"]);
                    objTestPending.sTestedBy = Convert.ToString(dt.Rows[0]["IND_INSP_BY"]);
                    objTestPending.sTestedOn = Convert.ToString(dt.Rows[0]["IND_INSP_DATE"]);
                    objTestPending.sTestLocation = Convert.ToString(dt.Rows[0]["IND_TEST_LOCATION"]);
                    objTestPending.sTestResult = Convert.ToString(dt.Rows[0]["IND_INSP_RESULT"]);
                }
                return objTestPending;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objTestPending;
            }
            finally
            {

            }
        }
        #endregion

        #region Deliver DTR / Recieve DTR
        /// <summary>
        /// Load Testing Passed Details
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public DataTable LoadTestingPassedDetails(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                string RSM_ID = ObjCon.get_value("SELECT RSM_ID FROM TBLREPAIRSENTMASTER WHERE RSM_PO_NO='" + objTestPending.sPurchaseOrderNo.ToString().ToUpper() + "'");
                strQry = " SELECT RSM_PO_NO, RSM_ID, TO_CHAR(RSM_PO_DATE,'DD-MON-YY') AS PODATE, TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YY') AS ISSUEDATE,";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME,";
                strQry += " RSM_PO_QNTY PO_QUANTITY, SUM(CASE WHEN RSD_DELIVARY_DATE IS NULL AND (IND_EE_APPROVE_STATUS  IS NULL  OR IND_EE_APPROVE_STATUS = 1) THEN 1 ELSE 0 END) PENDING_QNTY,";
                strQry += " SUM(CASE WHEN RSD_DELIVARY_DATE IS NOT NULL THEN 1 ELSE 0 END) DELIVERED_QNTY, ";
                strQry += "(SELECT SUM(ROI_INVOICED_OILQTY) FROM TBL_REPAIREROIL_INVOICE where ROI_RSM_ID='" + RSM_ID + "' AND ROI_OILSENT_BY='2') AS REPAIR_SENT_OIL ";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS WHERE IND_RSD_ID =RSD_ID AND RSM_ID = RSD_RSM_ID AND RSD_ID IN ";
                strQry += " (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT IN(1,3,4,5,6,7)) AND IND_INSP_RESULT <> 0 AND RSM_DIV_CODE LIKE :OfficeCode ||'%'";

                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);
                if (objTestPending.sRepairerId != null)
                {
                    strQry += " AND RSM_SUPREP_ID=:SupplierID AND RSM_SUPREP_TYPE='2'";
                    oleDbCommand.Parameters.AddWithValue("SupplierID", objTestPending.sRepairerId.ToString().ToUpper());
                }

                if (objTestPending.sSupplierId != null)
                {
                    strQry += " AND RSM_SUPREP_ID=:SupplierID1 AND RSM_SUPREP_TYPE='1'";
                    oleDbCommand.Parameters.AddWithValue("SupplierID1", objTestPending.sSupplierId.ToString().ToUpper());
                }
                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }

                strQry += " GROUP BY RSM_PO_NO, RSM_PO_QNTY, TO_CHAR(RSM_PO_DATE,'DD-MON-YY'),";
                strQry += " TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YY'),RSM_SUPREP_TYPE,RSM_SUPREP_ID,RSM_ID";
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Load EE Tested DTR
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public clsDTrRepairActivity LoadEETestedDTR(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = " SELECT RSM_PO_NO,TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE, RSD_ID, ";
                strQry += " TC_CODE,TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) TM_NAME, ";
                strQry += " TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE, ";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR ";
                strQry += " WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN  RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM ";
                strQry += " TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME,  IND_INSP_BY, ";
                strQry += " TO_CHAR(IND_INSP_DATE,'DD-MON-YYYY') IND_INSP_DATE,IND_TEST_LOCATION, ";
                strQry += " (CASE WHEN IND_INSP_RESULT='5' THEN 'ALL TEST OK EXPT CU LOSS' WHEN ";
                strQry += " IND_INSP_RESULT ='6' THEN  'ALL TEST OK EXPT IRON LOSS' WHEN IND_INSP_RESULT='7' ";
                strQry += " THEN 'ALL TEST OK EXPT CU & IRON LOSS' END) INS_RES_HT, IND_REMARKS,IND_DOC as HT_uplodede_doc, ";
                strQry += " IND_EE_INSPECTION_RESULT, IND_EE_OMNO , TO_CHAR(IND_EE_OM_DATE,'DD-MON-YYYY') IND_EE_OM_DATE, ";
                strQry += " IND_EE_REMARKS,RSM_REMARKS FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLTCMASTER,TBLINSPECTIONDETAILS ";
                strQry += " WHERE  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND IND_INSP_RESULT in(0,1,3,4,5,6,7) ";
                strQry += " AND IND_RSD_ID=RSD_ID AND TC_CODE=:TCCode AND IND_ID=:InspectionID ";                
                oleDbCommand.Parameters.AddWithValue("TCCode", objTestPending.sTcCode);
                oleDbCommand.Parameters.AddWithValue("InspectionID", objTestPending.sTestInspectionId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objTestPending.dtTestDone = dt;
                    objTestPending.sInspRemarks = Convert.ToString(dt.Rows[0]["IND_REMARKS"]);
                    objTestPending.sTestedBy = Convert.ToString(dt.Rows[0]["IND_INSP_BY"]);
                    objTestPending.sTestedOn = Convert.ToString(dt.Rows[0]["IND_INSP_DATE"]);
                    objTestPending.sTestLocation = Convert.ToString(dt.Rows[0]["IND_TEST_LOCATION"]);
                    objTestPending.sTestResult = Convert.ToString(dt.Rows[0]["IND_EE_INSPECTION_RESULT"]);
                }
                return objTestPending;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objTestPending;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Load Pending For Testing Details
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public DataTable LoadPendingForTestingDetails(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                //strQry = "SELECT RSM_PO_NO,  TO_CHAR(RSM_PO_DATE,'DD-MON-YY') AS PODATE, TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YY') AS ISSUEDATE,";
                //strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME  FROM TBLTRANSREPAIRER  WHERE TR_ID =RSM_SUPREP_ID) WHEN ";
                //strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME,";
                //strQry += " RSM_PO_QNTY PO_QUANTITY, SUM(CASE WHEN RSD_DELIVARY_DATE IS NULL THEN 1 ELSE 0 END) PENDING_QNTY,";
                //strQry += " SUM(CASE WHEN RSD_DELIVARY_DATE IS NOT NULL THEN 1 ELSE 0 END) DELIVERED_QNTY";
                //strQry += " ,SUM(CASE WHEN IND_EE_APPROVE_STATUS= 0 AND IND_INSP_RESULT IN (5,6,7) THEN 1 ELSE 0 END)PENDING_QTY_EE ";
                //strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS WHERE  IND_RSD_ID = RSD_ID AND RSM_ID = RSD_RSM_ID ";

                //if (objTestPending.sRoleId == "2")
                //{
                //    strQry += " AND RSM_STO_STATUS_FLAG ='1'  ";
                //}
                //if (objTestPending.sRoleId == "10")
                //{
                //    strQry += " AND RSM_STO_STATUS_FLAG is null  ";
                //}
                //strQry += " AND RSD_ID NOT IN  (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT IN (1,3)) AND RSM_DIV_CODE LIKE :OfficeCode ||'%'";

                //oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);

                //if (objTestPending.sRepairerId != null)
                //{
                //    strQry += " AND RSM_SUPREP_ID=:SupplierID AND RSM_SUPREP_TYPE='2'";
                //    oleDbCommand.Parameters.AddWithValue("SupplierID", objTestPending.sRepairerId.ToString().ToUpper());
                //}

                //if (objTestPending.sSupplierId != null)
                //{
                //    strQry += " AND RSM_SUPREP_ID=:SupplierID1 AND RSM_SUPREP_TYPE='1'";
                //    oleDbCommand.Parameters.AddWithValue("SupplierID1", objTestPending.sSupplierId.ToString().ToUpper());
                //}
                //if (objTestPending.sPurchaseOrderNo.Trim() != "")
                //{
                //    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                //    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                //}

                //strQry += " GROUP BY RSM_PO_NO, RSM_PO_QNTY, TO_CHAR(RSM_PO_DATE,'DD-MON-YY'),";
                //strQry += " TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YY'),RSM_SUPREP_TYPE,RSM_SUPREP_ID";



                strQry = " select A.RSM_PO_NO,A.RSM_DIV_CODE,PODATE,ISSUEDATE,SUP_REPNAME,PO_QUANTITY,";
                strQry += " coalesce(PENDING_QTY_EE, 0) PENDING_QTY_EE,coalesce(PENDING_QNTY, 0) PENDING_QNTY,coalesce(DELIVERED_QNTY,0) DELIVERED_QNTY ";
                strQry += " from (SELECT DISTINCT RSM_PO_NO, RSM_DIV_CODE, TO_CHAR(RSM_PO_DATE, 'DD-MON-YY') AS PODATE,";
                strQry += " TO_CHAR(RSM_ISSUE_DATE, 'DD-MON-YY') AS ISSUEDATE,(CASE WHEN RSM_SUPREP_TYPE = '2' ";
                strQry += " THEN(SELECT TR_NAME  FROM TBLTRANSREPAIRER  WHERE TR_ID = RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE = '1' THEN(SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE ";
                strQry += " TS_ID = RSM_SUPREP_ID) END) SUP_REPNAME, RSM_PO_QNTY PO_QUANTITY ";
                strQry += " FROM TBLREPAIRSENTMASTER inner join  TBLREPAIRSENTDETAILS on RSM_ID = RSD_RSM_ID ";
                strQry += " left join  TBLINSPECTIONDETAILS on IND_RSD_ID = RSD_ID ";
                strQry += " WHERE RSM_DIV_CODE LIKE :OfficeCode ||'%' ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);
                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }

                if (objTestPending.sRoleId == "2")
                {
                    strQry += " AND RSM_STO_STATUS_FLAG ='1'  ";
                }
                if (objTestPending.sRoleId == "10")
                {
                    strQry += " AND RSM_STO_STATUS_FLAG is null  ";
                }

                if (objTestPending.sRepairerId != null)
                {
                    strQry += " AND RSM_SUPREP_ID=:SupplierID AND RSM_SUPREP_TYPE='2'";
                    oleDbCommand.Parameters.AddWithValue("SupplierID", objTestPending.sRepairerId.ToString().ToUpper());
                }

                if (objTestPending.sSupplierId != null)
                {
                    strQry += " AND RSM_SUPREP_ID=:SupplierID1 AND RSM_SUPREP_TYPE='1'";
                    oleDbCommand.Parameters.AddWithValue("SupplierID1", objTestPending.sSupplierId.ToString().ToUpper());
                }
                strQry += " and RSD_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND RSD_MMS_AUTO_RV_NO IS NULL ";
                strQry += " AND RSD_DELIVARY_DATE IS NULL )A ";
                strQry += " LEFT JOIN ";
                //-- PENDING IN EE
                strQry += " (SELECT COUNT(IND_ID) PENDING_QTY_EE, RSM_DIV_CODE, RSM_PO_NO FROM   TBLREPAIRSENTMASTER INNER JOIN ";
                strQry += " TBLREPAIRSENTDETAILS ON RSM_ID = RSD_RSM_ID INNER JOIN TBLINSPECTIONDETAILS ON RSD_ID = IND_RSD_ID ";
                strQry += " WHERE  IND_EE_APPROVE_STATUS = 0 AND IND_INSP_RESULT IN(5, 6, 7)  AND RSM_DIV_CODE LIKE :OfficeCode ||'%'  ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);
                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }
                strQry += " AND  RSD_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_DELIVARY_DATE IS NULL GROUP BY  ";
                strQry += " RSM_DIV_CODE, RSM_PO_NO)B ON A.RSM_DIV_CODE = B.RSM_DIV_CODE AND A.RSM_PO_NO = B.RSM_PO_NO ";
                strQry += " LEFT JOIN ";
                //--PEND IN HT
                strQry += " (select  count(RSD_ID) PENDING_QNTY, RSM_DIV_CODE, RSM_PO_NO  FROM TBLREPAIRSENTMASTER  inner join  ";
                strQry += " TBLREPAIRSENTDETAILS on  RSM_ID = RSD_RSM_ID inner join TBLTCMASTER on RSD_TC_CODE = TC_CODE ";
                strQry += " WHERE     TC_CURRENT_LOCATION = '3'  AND RSD_DELIVARY_DATE IS NULL AND RSD_ID NOT IN ";
                strQry += " (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT IN(1, 3, 4, 5, 6, 7))  ";
                strQry += " and RSD_RV_NO is null and RSD_RV_DATE is null and RSD_MMS_AUTO_RV_NO is null AND RSM_DIV_CODE LIKE :OfficeCode ||'%'   ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);
                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }
                strQry += " GROUP BY  RSM_DIV_CODE, RSM_PO_NO )C ON  A.RSM_DIV_CODE = C.RSM_DIV_CODE AND A.RSM_PO_NO = C.RSM_PO_NO ";

                strQry += " left join ";
                // rv completed 
                strQry += " (select  count(RSD_ID) DELIVERED_QNTY, RSM_DIV_CODE, RSM_PO_NO  from TBLREPAIRSENTMASTER inner join ";
                strQry += " TBLREPAIRSENTDETAILS on RSM_ID = RSD_RSM_ID ";
                strQry += " where RSD_RV_NO is not null and RSD_RV_DATE is not null and  RSD_DELIVARY_DATE is not null  ";
                strQry += " and RSD_MMS_AUTO_RV_NO is not null AND RSM_DIV_CODE LIKE :OfficeCode ||'%' ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);
                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }
                strQry += " GROUP BY  RSM_DIV_CODE, RSM_PO_NO  )D ON A.RSM_DIV_CODE = D.RSM_DIV_CODE AND A.RSM_PO_NO = D.RSM_PO_NO ";




                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Load Pending For Recieve
        /// </summary>
        /// <param name="sRepairMasterId"></param>
        /// <returns></returns>
        public DataTable LoadPendingForRecieve(string sRepairMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = " SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN '1' ELSE '' END STATUS, CASE WHEN SYSDATE < TC_WARANTY_PERIOD ";
                strQry += "THEN (SELECT RSD_GUARRENTY_TYPE FROM (SELECT RSD_GUARRENTY_TYPE,RSD_TC_CODE,DENSE_RANK() OVER (PARTITION BY RSD_TC_CODE ";
                strQry += " ORDER BY RSD_ID ) AS RANKED_RSD_ID  FROM TBLREPAIRSENTDETAILS)A WHERE RSD_TC_CODE=TC_CODE AND RSD_GUARRENTY_TYPE IS NOT NULL GROUP BY RSD_GUARRENTY_TYPE,RSD_TC_CODE HAVING MIN(RANKED_RSD_ID) IN (1)) ";
                strQry += " ELSE '' END WARRENTY_TYPE ,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY')TC_WARANTY_PERIOD,TC_WARRENTY, ";
                strQry += "RSM_PO_NO,TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE, RSD_ID, TO_CHAR(TC_CODE)TC_CODE,TC_SLNO, ";
                strQry += "(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) MAKE, (SELECT IND_DOC FROM TBLINSPECTIONDETAILS ";
                strQry += " WHERE RSD_TC_CODE=TC_CODE and rsd_id=IND_RSD_ID AND IND_INSP_RESULT IN(1,3,5,6,7) AND (IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1))IND_DOC,";
                strQry += " (SELECT CASE IND_EE_INSPECTION_RESULT WHEN 1 THEN 'REPAIR GOOD' WHEN 2 THEN 'FAULTY' WHEN 3 THEN 'NOT REPAIRABLE' END AS ITEM_TYPE ";
                strQry += " FROM TBLINSPECTIONDETAILS WHERE RSD_TC_CODE = TC_CODE and rsd_id = IND_RSD_ID AND IND_INSP_RESULT IN(1, 3, 5, 6, 7) ";
                strQry += " AND(IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1)) ITEM_TYPE,(SELECT IND_EE_REMARKS FROM TBLINSPECTIONDETAILS ";
                strQry += " WHERE RSD_TC_CODE = TC_CODE and rsd_id = IND_RSD_ID AND IND_INSP_RESULT IN(1, 3, 5, 6, 7) ";
                strQry += " AND(IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1))REMARKS_EE,(SELECT IND_EE_OMNO ";
                strQry += " FROM TBLINSPECTIONDETAILS WHERE RSD_TC_CODE = TC_CODE and rsd_id = IND_RSD_ID AND IND_INSP_RESULT IN(1, 3, 5, 6, 7) ";
                strQry += " AND(IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1))OM_No,(SELECT TO_CHAR(IND_EE_OM_DATE, 'DD/MM/YYYY') ";
                strQry += " FROM TBLINSPECTIONDETAILS WHERE RSD_TC_CODE = TC_CODE and rsd_id = IND_RSD_ID AND IND_INSP_RESULT IN(1, 3, 5, 6, 7) ";
                strQry += " AND(IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1))OM_DATE,(SELECT IND_EE_OM_UPLOADED_PATH ";
                strQry += " FROM TBLINSPECTIONDETAILS WHERE RSD_TC_CODE = TC_CODE and rsd_id = IND_RSD_ID ";
                strQry += " AND IND_INSP_RESULT IN(1, 3, 5, 6, 7) AND(IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1))OM_Doc, ";
                strQry += " TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,(CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME ";
                strQry += "  FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM ";
                strQry += "  TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME, (SELECT  CASE IND_INSP_RESULT WHEN 1 THEN 'PASS'";
                strQry += " WHEN 3 THEN 'NOT REPAIRABLE' WHEN 4 THEN 'FAULTY FOR RECLASSIFICATION' WHEN 5 THEN 'ALL TEST OK EXPT CU LOSS' WHEN 6 THEN 'ALL TEST OK EXPT IRON LOSS' ";
                strQry += " WHEN 7 THEN 'ALL TEST OK EXPT CU & IRON LOSS' END AS STATE  FROM TBLINSPECTIONDETAILS ";
                strQry += " WHERE IND_RSD_ID=RSD_ID AND IND_INSP_RESULT IN(1,3,4,5,6,7) )STATE , (SELECT IND_REMARKS FROM TBLINSPECTIONDETAILS ";
                strQry += " WHERE IND_RSD_ID=RSD_ID AND IND_INSP_RESULT IN(1,3,4,5,6,7) )REMARKS";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE RSM_ID = :RSMID AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE";
                strQry += " AND RSD_DELIVARY_DATE IS NULL AND RSD_ID  IN ";
                strQry += " (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT IN(1,3,4,5,6,7) AND (IND_EE_APPROVE_STATUS IS NULL OR IND_EE_APPROVE_STATUS = 1))";
                oleDbCommand.Parameters.AddWithValue("RSMID", sRepairMasterId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadPendingForRecieve");
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Load Recieved Transformers
        /// </summary>
        /// <param name="sRepairMasterId"></param>
        /// <returns></returns>
        public DataTable LoadRecievedTransformers(string sRepairMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = " SELECT RSM_PO_NO, TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE, RSD_ID, TC_CODE,TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) MAKE, ";
                strQry += " TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME, ";
                strQry += " CASE IND_INSP_RESULT WHEN 1 THEN 'PASS' WHEN 3 THEN 'NOT REPAIRABLE' WHEN 5 THEN 'ALL TEST OK EXPT CU LOSS' ";
                strQry += " WHEN 6 THEN 'ALL TEST OK EXPT IRON LOSS' WHEN 7 THEN 'ALL TEST OK EXPT CU & IRON LOSS' END AS STATE,IND_INSP_RESULT ";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS, TBLTCMASTER WHERE RSM_ID = :RSMID AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE";
                strQry += " AND RSD_DELIVARY_DATE IS NOT NULL AND RSD_ID=IND_RSD_ID";
                oleDbCommand.Parameters.AddWithValue("RSMID", sRepairMasterId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Save Deliver TC Details
        /// </summary>
        /// <param name="sRepairDetailsId"></param>
        /// <param name="objDeliverpending"></param>
        /// <returns></returns>
        public string[] SaveDeliverTCDetails(string[] sRepairDetailsId, clsDTrRepairActivity objDeliverpending)
        {
            string[] Arr = new string[2];
            bool result;
            string ArrRsd_id = string.Empty;
            string ArrRsd_id1 = string.Empty;
            string strQry = string.Empty;

            DateTime dtRepairDateTemp = new DateTime();
            DateTime dtHost = Convert.ToDateTime(ConfigurationManager.AppSettings["dHost"].ToString());
            try
            {
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT * FROM TBLREPAIRSENTMASTER ,TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID = RSM_ID and RSD_RV_NO =:RVNO ";
                strQry += " AND RSM_DIV_CODE = (SELECT SM_CODE FROM TBLSTOREMAST WHERE SM_ID = :StoreID ) ";
                oleDbCommand.Parameters.AddWithValue("RVNO", objDeliverpending.sRVNo);
                oleDbCommand.Parameters.AddWithValue("StoreID", objDeliverpending.sStoreId);
                DataTable dtobjrvno = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dtobjrvno.Rows.Count > 0)
                {
                    Arr[0] = "Oops... RV Number already exists";
                    Arr[1] = "0";
                    return Arr;
                }
                if (objDeliverpending.sOMNo != "" && objDeliverpending.sOMNo != null)
                {
                    oleDbCommand = new OleDbCommand();
                    strQry = "SELECT * FROM TBLREPAIRSENTMASTER ,TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID = RSM_ID and RSD_OM_NO =:OMNO";
                    strQry += " AND RSM_DIV_CODE = (SELECT SM_CODE FROM TBLSTOREMAST WHERE SM_ID = :StoreID) ";
                    oleDbCommand.Parameters.AddWithValue("OMNO", objDeliverpending.sOMNo);
                    oleDbCommand.Parameters.AddWithValue("StoreID", objDeliverpending.sStoreId);
                    DataTable dtobjomno = ObjCon.getDataTable(strQry, oleDbCommand);
                    if (dtobjomno.Rows.Count > 0)
                    {
                        Arr[0] = "Oops... OM Number already exists";
                        Arr[1] = "0";
                        return Arr;
                    }
                }
                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();

                string[] strDetailVal = sRepairDetailsId.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        ArrRsd_id1 += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";
                        strQry = "UPDATE TBLTCMASTER SET TC_ITEM_ID = '" + strDetailVal[i].Split('~').GetValue(5).ToString() + "' WHERE TC_CODE = '" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                        ObjCon.Execute(strQry);
                        objWCF.SaveTcDetails(strQry);
                    }
                }
                ArrRsd_id1 = ArrRsd_id1.Remove(ArrRsd_id1.Length - 1);

                strQry = "SELECT TO_CHAR(LISTAGG(RSD_TC_CODE,',')WITHIN GROUP (ORDER BY RSD_TC_CODE)) AS TC_CODE FROM TBLREPAIRSENTDETAILS WHERE RSD_ID IN (" + ArrRsd_id1 + ") ";
                string STC_CODE = ObjCon.get_value(strQry);


                strQry = "SELECT TO_CHAR(ITEM_CODE)ITEM_CODE,TC_ID,TC_CODE FROM (SELECT CASE WHEN (TC_STATUS=1 AND (WARRENTY_TYPE='1' OR WARRENTY_TYPE =''))";
                strQry += "THEN ITM_BRAND_NEW WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP' OR WARRENTY_TYPE ='')) THEN ITM_FAULTY_AGP WHEN (TC_STATUS=3 AND";
                strQry += "(WARRENTY_TYPE='WGP' OR WARRENTY_TYPE='WRGP' OR WARRENTY_TYPE ='')) THEN ITM_FAULTY_WGP WHEN (TC_STATUS=2 AND (WARRENTY_TYPE='2' ";
                strQry += " OR WARRENTY_TYPE ='')) THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP' OR WARRENTY_TYPE ='') AND TC_MAKE_ID='98') THEN";
                strQry += " ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN ITM_REPAIRE_GOOD_ABB_MAKE WHEN TC_STATUS='4' THEN ";
                strQry += "ITM_SCRAPE WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='0' OR WARRENTY_TYPE ='')) THEN ITM_SCRAPE WHEN TC_STATUS='2' THEN ";
                strQry += "ITM_REPAIR_GOOD END ITEM_CODE,TC_ID,TC_CODE FROM (SELECT DISTINCT TC_ID,TC_CODE,";
                strQry += "TC_CAPACITY,TC_STATUS,ITM_FAULTY_AGP,ITM_FAULTY_WGP,ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE,ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,TC_MAKE_ID,";
                strQry += "(SELECT  DF_GUARANTY_TYPE FROM (SELECT DF_GUARANTY_TYPE,DF_EQUIPMENT_ID,DENSE_RANK() OVER (PARTITION BY DF_EQUIPMENT_ID ";
                strQry += "ORDER BY DF_ID DESC) AS RANKED_DF_ID  FROM TBLDTCFAILURE WHERE DF_EQUIPMENT_ID IN (" + STC_CODE + ") AND DF_GUARANTY_TYPE IS NOT NULL ) ";
                strQry += "WHERE RANKED_DF_ID = 1 AND DF_EQUIPMENT_ID= TC_CODE ) WARRENTY_TYPE FROM TBLTCMASTER,TBLITEMPRICEMASTER,TBLDTCFAILURE WHERE ";
                strQry += "TC_CAPACITY =ITM_CAPACITY(+) AND TC_CODE=DF_EQUIPMENT_ID(+)  AND TC_CODE IN (" + STC_CODE + "))) ORDER BY TC_CODE ";

                strQry = "SELECT  to_char(TC_ITEM_ID) ITEM_CODE, TC_ID ,to_char(TC_CODE) TC_CODE FROM TBLTCMASTER  WHERE  TC_CODE IN (" + STC_CODE + ")";
                DataTable dtRV_Item_Code = ObjCon.getDataTable(strQry);

                objDeliverpending.sMMSAutoRVNo = objWCF.GetRepairerAutoRVNum(objDeliverpending.sRVNo.Substring(0, 2));

                ObjCon.BeginTrans();

                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        ArrRsd_id += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";
                        strQry = "SELECT DISTINCT STATUS FROM (SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT MAX(RSD_WARENTY_PERIOD)  RSD_WARENTY_PERIOD ";
                        strQry += " FROM TBLREPAIRSENTDETAILS WHERE RSD_WARENTY_PERIOD IS NOT NULL AND ";
                        strQry += "RSD_TC_CODE ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "') ELSE '' END STATUS FROM TBLTCMASTER,";
                        strQry += "TBLREPAIRSENTDETAILS WHERE TC_CODE=RSD_TC_CODE AND TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "')";
                        string res = ObjCon.get_value(strQry);
                        if (res == "" || res == null)
                        {
                            if (objDeliverpending.sOMNo != "" && objDeliverpending.sOMNo != null && objDeliverpending.sOMDate != "" && objDeliverpending.sOMDate != null)

                            {
                                strQry = "Update TBLREPAIRSENTDETAILS SET ";
                                strQry += "RSD_DELIVARY_DATE=to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
                                strQry += "RSD_DELIVER_VER_BY='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
                                strQry += "RSD_DELIVER_LOCATION='" + objDeliverpending.sStoreId + "',";
                                strQry += "RSD_DELIVER_CHALLEN_NO='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
                                strQry += " RSD_RV_NO='" + objDeliverpending.sRVNo + "',RSD_RV_DATE=to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY')";
                                strQry += ",RSD_WARENTY_PERIOD='" + strDetailVal[i].Split('~').GetValue(3).ToString() + "',RSD_GUARRENTY_TYPE='" + strDetailVal[i].Split('~').GetValue(4).ToString() + "'";
                                strQry += ", RSD_OM_NO='" + objDeliverpending.sOMNo + "',RSD_OM_DATE=to_date('" + objDeliverpending.sOMDate + "','DD/MM/YYYY')";
                                strQry += " , RSD_MMS_AUTO_RV_NO = '" + objDeliverpending.sMMSAutoRVNo + "' where RSD_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                                ObjCon.Execute(strQry);


                            }
                            else
                            {
                                strQry = "UPDATE TBLREPAIRSENTDETAILS SET ";
                                strQry += "RSD_DELIVARY_DATE=to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
                                strQry += "RSD_DELIVER_VER_BY='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
                                strQry += "RSD_DELIVER_LOCATION='" + objDeliverpending.sStoreId + "',";
                                strQry += "RSD_DELIVER_CHALLEN_NO='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
                                strQry += " RSD_RV_NO='" + objDeliverpending.sRVNo + "',RSD_RV_DATE=to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY') ,  RSD_MMS_AUTO_RV_NO = '" + objDeliverpending.sMMSAutoRVNo + "'";
                                strQry += " where RSD_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                                ObjCon.Execute(strQry);

                            }


                        }
                        else
                        {
                            if (objDeliverpending.sOMNo != "" && objDeliverpending.sOMNo != null && objDeliverpending.sOMDate != "" && objDeliverpending.sOMDate != null)
                            {

                                strQry = "UPDATE TBLREPAIRSENTDETAILS SET ";
                                strQry += "RSD_DELIVARY_DATE=to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
                                strQry += "RSD_DELIVER_VER_BY='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
                                strQry += "RSD_DELIVER_LOCATION='" + objDeliverpending.sStoreId + "',";
                                strQry += "RSD_DELIVER_CHALLEN_NO='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
                                strQry += " RSD_RV_NO='" + objDeliverpending.sRVNo + "',RSD_RV_DATE=to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY')";
                                strQry += ", RSD_OM_NO='" + objDeliverpending.sOMNo + "',RSD_OM_DATE=to_date('" + objDeliverpending.sOMDate + "','DD/MM/YYYY') ,  RSD_MMS_AUTO_RV_NO = '" + objDeliverpending.sMMSAutoRVNo + "'";
                                strQry += " where RSD_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                                ObjCon.Execute(strQry);
                            }
                            else
                            {
                                strQry = "UPDATE TBLREPAIRSENTDETAILS SET ";
                                strQry += "RSD_DELIVARY_DATE=to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
                                strQry += "RSD_DELIVER_VER_BY='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
                                strQry += "RSD_DELIVER_LOCATION='" + objDeliverpending.sStoreId + "',";
                                strQry += "RSD_DELIVER_CHALLEN_NO='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
                                strQry += " RSD_RV_NO='" + objDeliverpending.sRVNo + "',RSD_RV_DATE=to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY') ,  RSD_MMS_AUTO_RV_NO = '" + objDeliverpending.sMMSAutoRVNo + "'";
                                strQry += " where RSD_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                                ObjCon.Execute(strQry);

                            }


                        }

                        string rsd_id = ArrRsd_id.Remove(ArrRsd_id.Length - 1);
                        strQry = "SELECT DISTINCT TO_CHAR(RSM_ISSUE_DATE)RSM_ISSUE_DATE FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS WHERE RSM_ID=RSD_RSM_ID AND RSD_ID IN (" + rsd_id + ")";
                        string RepairDate = ObjCon.get_value(strQry);

                      



                        DateTime dtRepairDate = Convert.ToDateTime(RepairDate);
                        dtRepairDateTemp = dtRepairDate;

                        if (strDetailVal[i].Split('~').GetValue(2).ToString() == "1")
                        {
                            string TcStatusflag = string.Empty;
                            strQry = "SELECT (CASE WHEN IND_EE_INSPECTION_RESULT= 1 THEN '2' WHEN IND_EE_INSPECTION_RESULT= 2 THEN '3' WHEN IND_EE_INSPECTION_RESULT= 3 THEN '9' else '2' end)STATUS from TBLINSPECTIONDETAILS where IND_RSD_ID in(" + strDetailVal[i].Split('~').GetValue(0).ToString() + ") ORDER BY 1 desc fetch first 1 rows only";
                            TcStatusflag = ObjCon.get_value(strQry);

                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_STORE_ID='" + objDeliverpending.sStoreId + "', ";
                            strQry += " TC_STATUS='"+ TcStatusflag + "',TC_UPDATED_EVENT='DELIVER TC',TC_UPDATED_EVENT_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                            strQry += " WHERE TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                            ObjCon.Execute(strQry);



                            strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID='" + objDeliverpending.sStoreId + "'";
                            string MMS_StoreID = ObjCon.get_value(strQry);

                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_STORE_ID='" + MMS_StoreID + "', ";
                            strQry += " TC_STATUS='"+ TcStatusflag + "',TC_UPDATED_EVENT='DELIVER TC',TC_UPDATED_EVENT_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                            strQry += " WHERE TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";

                            if (dtRepairDate < dtHost)
                            {
                                result = true;
                            }
                            else
                            {
                                result = objWCF.SaveTcDetails(strQry);
                            }
                            if (result == false)
                            {
                                ObjCon.RollBack();
                                Arr[0] = "Something Went wrong Please contact Admin";
                                Arr[1] = "0";
                                return Arr;
                            }

                        }
                        // Changing TC_STATUS 6 to 9 Because We have changed inspection result scrap to not repairable 
                        // changes are done by sandeep on 12-10-2023.
                        else if (strDetailVal[i].Split('~').GetValue(2).ToString() == "3")
                        {
                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_STORE_ID='" + objDeliverpending.sStoreId + "', ";
                            strQry += " TC_STATUS=9,TC_UPDATED_EVENT='DELIVER TC',TC_UPDATED_EVENT_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                            strQry += " WHERE TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                            ObjCon.Execute(strQry);

                            strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID='" + objDeliverpending.sStoreId + "'";
                            string MMS_StoreID = ObjCon.get_value(strQry);
                            //string MMS_StoreID = objWCF.get_StoreID(objDeliverpending.sStoreId);

                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_STORE_ID='" + MMS_StoreID + "', ";
                            strQry += " TC_STATUS=9,TC_UPDATED_EVENT='DELIVER TC',TC_UPDATED_EVENT_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                            strQry += " WHERE TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";

                            if (dtRepairDate < dtHost)
                            {
                                result = true;
                            }
                            else
                            {
                                result = objWCF.SaveTcDetails(strQry);
                            }

                            if (result == false)
                            {
                                ObjCon.RollBack();
                                Arr[0] = "Something Went wrong Please contact Admin";
                                Arr[1] = "0";
                                return Arr;
                            }
                        }
                        //if its "none"
                        // Replaced NONE to FAULTY FOR RECLASSIFICATION
                        // In TC_STATUS we are changing  6 to 3 while sending query to execute in MMS WCF.
                        else if (strDetailVal[i].Split('~').GetValue(2).ToString() == "4")
                        {
                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_STORE_ID='" + objDeliverpending.sStoreId + "', ";
                            strQry += " TC_STATUS=3,TC_UPDATED_EVENT='DELIVER TC',TC_UPDATED_EVENT_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                            strQry += " WHERE TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                            ObjCon.Execute(strQry);

                            strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID='" + objDeliverpending.sStoreId + "'";
                            string MMS_StoreID = ObjCon.get_value(strQry);


                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_STORE_ID='" + MMS_StoreID + "', ";
                            strQry += " TC_STATUS=3,TC_UPDATED_EVENT='DELIVER TC',TC_UPDATED_EVENT_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                            strQry += " WHERE TC_CODE='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";

                            if (dtRepairDate < dtHost)
                            {
                                result = true;
                            }
                            else
                            {
                                result = objWCF.SaveTcDetails(strQry);
                            }

                            if (result == false)
                            {
                                ObjCon.RollBack();
                                Arr[0] = "Something Went wrong Please contact Admin";
                                Arr[1] = "0";
                                return Arr;
                            }

                        }
                        Arr[0] = "Repaired Transformer Successfully Recieved in Store";
                        Arr[1] = "0";
                    }

                }

                strQry = "SELECT DISTINCT RSM_PO_NO FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS WHERE RSM_ID=RSD_RSM_ID AND RSD_ID IN (" + ArrRsd_id1 + ")";
                string sPO_NO = ObjCon.get_value(strQry);
                strQry = "SELECT DISTINCT RSD_MMS_AUTO_INV_NO FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS WHERE RSM_ID=RSD_RSM_ID AND RSD_ID IN (" + ArrRsd_id1 + ")";
                DataTable DtAutoinv = ObjCon.getDataTable(strQry);

                List<string> myIds = new List<string>();
                foreach (DataRow dr in DtAutoinv.Rows)
                {
                    myIds.Add(Convert.ToString(dr["RSD_MMS_AUTO_INV_NO"]));
                }
                string MMS_Auto_Invno = string.Join(",", myIds);

                result = objWCF.RVItem_code(dtRV_Item_Code, sPO_NO, objDeliverpending.sRVNo, objDeliverpending.sRVDate,
                    objDeliverpending.sOMNo, objDeliverpending.sOMDate, STC_CODE, objDeliverpending.sMMSAutoRVNo,
                    objDeliverpending.sDeliverChallenNo, objDeliverpending.sDeliverDate, MMS_Auto_Invno);



                if (result == true)
                {
                    ObjCon.CommitTrans();
                }
                else
                {
                    ObjCon.RollBack();
                    Arr[0] = "Bad Response MMS";
                    Arr[1] = "0";
                }


                return Arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Arr;
            }
            finally
            {

            }
        }
        #endregion

        public DataTable LoadTestOrDeliverPendingDTRForRIC(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;


                string RSM_ID = ObjCon.get_value("SELECT RSM_ID FROM TBLREPAIRSENTMASTER WHERE RSM_PO_NO='" + objTestPending.sPurchaseOrderNo + "'");
                string ROI_RSM_ID = ObjCon.get_value("select * from TBL_REPAIREROIL_INVOICE where ROI_RSM_ID = '" + RSM_ID + "'");

                if (ROI_RSM_ID == "")
                {
                    strQry = " SELECT RSM_PO_NO,(SELECT Count(RSD_RSM_ID) FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER WHERE RSM_ID = RSD_RSM_ID AND RSM_PO_NO='" + objTestPending.sPurchaseOrderNo + "' ) TC_QUANTITY,";
                    strQry += " (SELECT SUM(RSD_OIL_QUANTITY) FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER WHERE RSM_ID = RSD_RSM_ID AND RSM_PO_NO='" + objTestPending.sPurchaseOrderNo + "')TOTAL_OIL_AMOUNT,";
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE,RSM_INV_NO,TO_CHAR(RSM_PO_DATE,'DD-MM-YYYY')RSM_PO_DATE, RSD_ID, TC_CODE,TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) TM_NAME, ";
                    strQry += " TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,";
                    strQry += " (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN ";
                    strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME,RSM_OLD_PO_NO,RSM_REMARKS,RSM_SUPREP_ID,RSD_OIL_QUANTITY,RSD_SENT_OIL AS SENT_OIL,";
                    strQry += " 0 as REPAIRER_SENT_OIL,RSD_PENDING_OIL AS ROI_PENDINGQTY ";
                    strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER WHERE  RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE ";
                    strQry += " AND RSM_DIV_CODE LIKE :OfficeCode ||'%' AND RSD_OIL_APPROVE_STATUS=0";
                }

                else
                {
                    strQry = "SELECT RSM_PO_NO,(SELECT Count(RSD_RSM_ID) FROM TBLREPAIRSENTDETAILS, TBLREPAIRSENTMASTER WHERE RSM_ID = RSD_RSM_ID AND RSM_PO_NO = '" + objTestPending.sPurchaseOrderNo + "') TC_QUANTITY,";
                    strQry += "(SELECT SUM(RSD_OIL_QUANTITY) FROM TBLREPAIRSENTDETAILS, TBLREPAIRSENTMASTER WHERE RSM_ID = RSD_RSM_ID AND RSM_PO_NO = '" + objTestPending.sPurchaseOrderNo + "')TOTAL_OIL_AMOUNT,";
                    strQry += "TO_CHAR(RSM_ISSUE_DATE, 'DD-MON-YYYY') RSM_ISSUE_DATE,TO_CHAR(RSM_PO_DATE,'DD-MM-YYYY')RSM_PO_DATE,RSM_INV_NO, RSD_ID, TC_CODE,";
                    strQry += "(CASE WHEN RSM_SUPREP_TYPE = '2' THEN(SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID = RSM_SUPREP_ID) WHEN RSM_SUPREP_TYPE = '1' ";
                    strQry += "THEN(SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID = RSM_SUPREP_ID) END ) SUP_REPNAME,RSM_SUPREP_ID,RSD_OIL_QUANTITY,(SELECT SUM(ROI_INVOICED_OILQTY) ";
                    strQry += "FROM TBL_REPAIREROIL_INVOICE where ROI_RSM_ID ='" + RSM_ID + "' AND  ROI_OILSENT_BY='1') AS SENT_OIL,(SELECT SUM(ROI_INVOICED_OILQTY) FROM TBL_REPAIREROIL_INVOICE where ROI_RSM_ID ='" + RSM_ID + "' ";
                    strQry += " AND ROI_OILSENT_BY='2') AS REPAIRER_SENT_OIL, ROI_PENDINGQTY  FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER,";
                    strQry += "TBL_REPAIREROIL_INVOICE WHERE RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE ";
                    strQry += "AND RSM_DIV_CODE LIKE :OfficeCode ||'%' AND RSD_OIL_APPROVE_STATUS = 0";

                }


                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);

                if (objTestPending.sRoleId == "2")
                {
                    strQry += " AND RSM_STO_STATUS_FLAG ='1' ";
                }

                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }
                if (RSM_ID != "" && ROI_RSM_ID != "")
                {
                    strQry += " AND ROI_RSM_ID=:RSM_ID  ORDER BY ROI_ID desc ";
                    oleDbCommand.Parameters.AddWithValue("RSM_ID", RSM_ID);
                }
                strQry += " fetch first 1 rows only";
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }

        public clsDTrRepairActivity GetRepairSentDetails(clsDTrRepairActivity objRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                oleDbCommand.Parameters.AddWithValue("RSMID", objRepair.sRepairMasterId);
                string strQry = string.Empty;
                strQry = "SELECT RSM_ID,TO_CHAR(RSM_ISSUE_DATE,'DD/MM/YYYY') RSM_ISSUE_DATE,RSM_PO_NO,TO_CHAR(RSM_PO_DATE,'DD/MM/YYYY') RSM_PO_DATE,RSM_INV_NO,";
                strQry += " TO_CHAR(RSM_INV_DATE,'DD/MM/YYYY') RSM_INV_DATE,RSM_GUARANTY_TYPE,RSM_SUPREP_TYPE,RSM_SUPREP_ID,RSM_MANUAL_INV_NO,RSM_OLD_PO_NO,RSM_REMARKS,RSM_HTLT_DIV_CODE FROM TBLREPAIRSENTMASTER WHERE RSM_ID=:RSMID";

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRepair.sIssueDate = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
                    objRepair.sPurchaseOrderNo = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                    objRepair.sPurchaseDate = Convert.ToString(dt.Rows[0]["RSM_PO_DATE"]);
                    objRepair.sInvoiceNo = Convert.ToString(dt.Rows[0]["RSM_INV_NO"]);
                    objRepair.sInvoiceDate = Convert.ToString(dt.Rows[0]["RSM_INV_DATE"]);
                    objRepair.sGuarantyType = Convert.ToString(dt.Rows[0]["RSM_GUARANTY_TYPE"]);
                    objRepair.sType = Convert.ToString(dt.Rows[0]["RSM_SUPREP_TYPE"]);
                    objRepair.sSupRepId = Convert.ToString(dt.Rows[0]["RSM_SUPREP_ID"]);
                    objRepair.sOldPONo = Convert.ToString(dt.Rows[0]["RSM_OLD_PO_NO"]);
                    objRepair.sPORemarks = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);
                    objRepair.slochtlt = Convert.ToString(dt.Rows[0]["RSM_HTLT_DIV_CODE"]);

                    objRepair.sManualInvoiceNo = Convert.ToString(dt.Rows[0]["RSM_MANUAL_INV_NO"]);
                }
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT RSD_REMARKS,RSD_OIL_QTY,TO_CHAR(RSD_INV_DATE,'DD/MM/YYYY') RSD_INV_DATE,TO_CHAR(RSD_ISSUE_DATE,'DD/MM/YYYY') RSD_ISSUE_DATE , RSD_INV_NO ";
                strQry += " ,RSD_NTH_TIME,RSD_MAN_INV_NO FROM TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID =:sRepairMasterId AND RSD_NTH_TIME <> 1  ORDER BY RSD_ID DESC";
                oleDbCommand.Parameters.AddWithValue("sRepairMasterId", objRepair.sRepairMasterId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRepair.sIsOldPo = true;
                    objRepair.sNthTime = dt.Rows[0]["RSD_NTH_TIME"].ToString();
                    objRepair.sIssueDate = Convert.ToString(dt.Rows[0]["RSD_ISSUE_DATE"]);
                    objRepair.sInvoiceNo = Convert.ToString(dt.Rows[0]["RSD_INV_NO"]);
                    objRepair.sInvoiceDate = Convert.ToString(dt.Rows[0]["RSD_INV_DATE"]);
                    objRepair.sManualInvoiceNo = Convert.ToString(dt.Rows[0]["RSD_MAN_INV_NO"]);
                    objRepair.sPORemarks = Convert.ToString(dt.Rows[0]["RSD_REMARKS"]);
                }
                else
                {
                    oleDbCommand = new OleDbCommand();
                    strQry = "SELECT * FROM TBLREPAIRSENTDETAILS WHERE RSD_RSM_ID =:sRepairMasterId1 ORDER BY RSD_ID DESC";
                    oleDbCommand.Parameters.AddWithValue("sRepairMasterId1", objRepair.sRepairMasterId);
                    dt = ObjCon.getDataTable(strQry, oleDbCommand);
                    objRepair.sOilQty = dt.Rows[0]["RSD_OIL_QTY"].ToString();
                    objRepair.sIsOldPo = false;
                    objRepair.sNthTime = "1";
                }
                objRepair.sOilQty = dt.Rows[0]["RSD_OIL_QTY"].ToString();
                return objRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objRepair;
            }
            finally
            {

            }
        }

        public clsDTrRepairActivity GetRepairRecieveDetails(clsDTrRepairActivity objRepair)
        {
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT TO_CHAR(RSD_DELIVARY_DATE,'DD/MM/YYYY') RSD_DELIVARY_DATE ,RSD_DELIVER_VER_BY,";
                strQry += " RSD_DELIVER_LOCATION,RSD_DELIVER_CHALLEN_NO,RSD_RV_NO,TO_CHAR(RSD_RV_DATE,'DD/MM/YYYY') RSD_RV_DATE ,RSD_OM_NO,TO_CHAR(RSD_OM_DATE,'DD/MM/YYYY') RSD_OM_DATE FROM ";
                strQry += " TBLREPAIRSENTDETAILS WHERE RSD_ID=:RSDID";
                oleDbCommand.Parameters.AddWithValue("RSDID", objRepair.sRepairDetailsId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRepair.sDeliverDate = Convert.ToString(dt.Rows[0]["RSD_DELIVARY_DATE"]);
                    objRepair.sVerifiedby = Convert.ToString(dt.Rows[0]["RSD_DELIVER_VER_BY"]);
                    objRepair.sDeliverChallenNo = Convert.ToString(dt.Rows[0]["RSD_DELIVER_CHALLEN_NO"]);
                    objRepair.sRVNo = Convert.ToString(dt.Rows[0]["RSD_RV_NO"]);
                    objRepair.sRVDate = Convert.ToString(dt.Rows[0]["RSD_RV_DATE"]);
                    if (dt.Rows[0]["RSD_OM_NO"] != "" && dt.Rows[0]["RSD_OM_NO"] != null)
                    {
                        objRepair.sOMNo = Convert.ToString(dt.Rows[0]["RSD_OM_NO"]);
                    }
                    if (dt.Rows[0]["RSD_OM_DATE"] != "" && dt.Rows[0]["RSD_OM_DATE"] != null)
                    {
                        objRepair.sOMDate = Convert.ToString(dt.Rows[0]["RSD_OM_DATE"]);
                    }


                }

                return objRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objRepair;
            }
            finally
            {

            }
        }

        public string GetRepairDetailsId(string sRepairMasterId)
        {
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT RSD_RSM_ID FROM TBLREPAIRSENTDETAILS WHERE RSD_ID=:RSDID";
                oleDbCommand.Parameters.AddWithValue("RSDID", sRepairMasterId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;

            }
        }

        public string GetOilTypeByPoNo(string sPono)
        {
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT RSM_OIL_TYPE FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS WHERE RSM_ID = RSD_RSM_ID AND UPPER(RSM_PO_NO)=:PONO";
                oleDbCommand.Parameters.AddWithValue("PONO", sPono);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;

            }
        }


        public string GetOilCalculation(string starrating, string capacity)
        {
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT OC_OIL_QUANTITY FROM TBLOILCAL where OC_CAPACITY='" + capacity + "' AND OC_STAR_RATING='" + starrating + "'";
                // oleDbCommand.Parameters.AddWithValue("RSDID", sRepairMasterId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;

            }
        }
        public DataTable GetAllImages()
        {
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT IND_DOC FROM TBLINSPECTIONDETAILS WHERE IND_DOC IS NOT NULL";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;

            }
        }

        #region Convert Image to Byte Array
        public static byte[] ConvertImg(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }
        #endregion

        public DataTable GetRepairPoDetails(clsDTrRepairActivity objRepair)
        {
            DataTable dtPoDetails = new DataTable();

            string strQry = string.Empty;
            try
            {
                oleDbCommand = new OleDbCommand();

                strQry = "SELECT TC_CODE,TC_SLNO,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TM_NAME,";
                strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY,to_char(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE,RSM_GUARANTY_TYPE,CASE WHEN ";
                strQry += "RSD_DELIVARY_DATE IS NULL THEN 'Repair Pending' WHEN RSD_DELIVARY_DATE IS NOT NULL THEN 'Repair Completed' END ";
                strQry += "STATUS FROM TBLREPAIRSENTMASTER INNER JOIN TBLREPAIRSENTDETAILS ON RSM_ID=RSD_RSM_ID INNER JOIN TBLTCMASTER ON ";
                strQry += "TC_CODE=RSD_TC_CODE AND rsm_div_code=:DivisionCode AND RSM_PO_NO=:PONO";
                oleDbCommand.Parameters.AddWithValue("DivisionCode", objRepair.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("PONO", objRepair.sPurchaseOrderNo);
                dtPoDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtPoDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetAllImages");
                return dtPoDetails;
            }
        }

        public void UpdateGatePassMMS(clsInvoice objInvoice, clsDTrRepairActivity objRepair)
        {
            string strQry = string.Empty;
            string mmsStoreId = string.Empty;
            try
            {
                oleDbCommand = new OleDbCommand();
                DataTable dtRepairPo = new DataTable();
                dtRepairPo.TableName = "GatePass";
                dtRepairPo.Columns.Add("RSM_PO_NO", typeof(string));//1
                dtRepairPo.Columns.Add("RSM_MANUAL_INV_NO", typeof(string));//2
                dtRepairPo.Columns.Add("RSM_DIV_CODE", typeof(string));//3
                dtRepairPo.Columns.Add("VEHICLE_NUMBER", typeof(string));//4
                dtRepairPo.Columns.Add("RECEIPT_NAME", typeof(string));//5
                dtRepairPo.Columns.Add("CHALLEN_NUMBER", typeof(string));//6

                oleDbCommand.Parameters.AddWithValue("OfficeCode", objRepair.sOfficeCode);
                mmsStoreId = ObjCon.get_value("SELECT SM_MMS_STORE_ID  FROM TBLSTOREMAST WHERE SM_CODE = :OfficeCode", oleDbCommand);

                DataRow drow = dtRepairPo.NewRow();
                drow["RSM_PO_NO"] = objRepair.sPurchaseOrderNo;
                drow["RSM_MANUAL_INV_NO"] = objRepair.sManualInvoiceNo;
                drow["RSM_DIV_CODE"] = mmsStoreId;
                drow["VEHICLE_NUMBER"] = objInvoice.sVehicleNumber;
                drow["RECEIPT_NAME"] = objInvoice.sReceiptientName;
                drow["CHALLEN_NUMBER"] = objInvoice.sChallenNo;

                dtRepairPo.Rows.Add(drow);

                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                objWCF.UpdateRepairGatePass(dtRepairPo);

                strQry = "SELECT * FROM TBLREPAIRSENTMASTER WHERE RSM_PO_NO = '" + objRepair.sPurchaseOrderNo + "' AND RSM_DIV_CODE =  '' AND RSM_MANUAL_INV_NO = ''";

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        public string getTotalItemQnty(clsDTrRepairActivity objRepair)
        {
            string Totalqty = string.Empty;
            try
            {
                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();


                Totalqty = objWCF.get_TotalItemQty(objRepair.sItemCode, objRepair.sOfficeCode);
                return Totalqty;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Totalqty;
            }
        }

        public string[] SaveUpdateAgentdetails(clsDTrRepairActivity objRepair)
        {

            string strQry = string.Empty;
            string[] Arr = new string[2];
            DateTime invoice_date = new DateTime();
            string SupplierName = string.Empty;
            string TC_CODE = string.Empty;
            bool res = false;
            DateTime dtHost = Convert.ToDateTime(ConfigurationSettings.AppSettings["dHost"].ToString());

            try
            {
                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                oledbCommand = new OleDbCommand();
                objRepair.sMMSAutoInvNo = objWCF.GetOilAutoInvoiceNum(sOfficeCode);
                double Temp = Convert.ToDouble(objRepair.sOilQty) * Convert.ToDouble(objRepair.sPercentage) / 100;
                string dr = ObjCon.get_value("SELECT OSD_PO_NO FROM TBLOILSENTMASTER WHERE OSD_PO_NO='" + objRepair.sPurchaseOrderNo + "' AND OSD_OFFICE_CODE= " + objRepair.sOfficeCode + " and OSD_INVOICE_NO='" + objRepair.sInvoiceNo + "'");
                if (dr == "")
                {
                    ObjCon.BeginTrans();
                    objRepair.sID = Convert.ToString(ObjCon.Get_max_no("OSD_ID", "TBLOILSENTMASTER"));
                    strQry = "INSERT INTO TBLOILSENTMASTER (OSD_ID,OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,OSD_INVOICE_DATE,OSD_QUANTITY,OSD_CRON,OSD_CRBY,OSD_OFFICE_CODE,OSD_STATUS,OSD_ITEM_CODE,OSD_AGENCY,OSD_MMS_AUTO_INV_NO,OSD_PERCENTAGE_VALUE,OSD_AMOUNT,OSD_ACC_CODE,OSD_OILQUANTITY_LTR) VALUES('" + objRepair.sID + "','" + objRepair.sPurchaseOrderNo + "',";
                    strQry += " TO_DATE('" + objRepair.sPurchaseDate + "','dd/MM/yyyy'),'" + objRepair.sInvoiceNo + "',TO_DATE('" + objRepair.sInvoiceDate + "','dd/MM/yyyy'),'" + objRepair.sOilQty + "',SYSDATE,'" + objRepair.sCrby + "','" + objRepair.sOfficeCode + "','0','" + objRepair.sItemCode + "','" + objRepair.sAgencyname + "',";
                    strQry += "'" + objRepair.sMMSAutoInvNo + "','" + Temp + "','" + objRepair.sAmount + "','" + objRepair.sBudgetAccCode + "','" + objRepair.sQuantityLtr + "')";
                    ObjCon.Execute(strQry);

                    string BM_OBAMOUNT = ObjCon.get_value("SELECT BM_OB_AMNT  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objRepair.sBudgetAccCode + "'", oleDbCommand);

                    string BM_id = ObjCon.get_value("SELECT BM_ID  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objRepair.sBudgetAccCode + "'", oleDbCommand);
                    double availableamt = Convert.ToDouble(BM_OBAMOUNT) - Convert.ToDouble(objRepair.sAmount);

                    string BtId = Convert.ToString(ObjCon.Get_max_no("BT_ID", "TBLBUDGETTRANS"));
                    strQry = "INSERT INTO TBLBUDGETTRANS (BT_ID,BT_ACC_CODE,BT_BM_AMNT,BT_AVL_AMNT,BT_CRON,BT_DEBIT_AMNT,BT_FIN_YEAR,BT_DIV_CODE,BT_PONO,BT_BM_ID) VALUES('" + BtId + "','" + objRepair.sBudgetAccCode + "',";
                    strQry += " '" + BM_OBAMOUNT + "','" + availableamt + "',SYSDATE,'" + objRepair.sAmount + "','" + objRepair.sFinancialYear + "','" + objRepair.sOfficeCode + "','" + objRepair.sPurchaseOrderNo + "'," + BM_id + ")";
                    ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLBUDGETMAST SET BM_OB_AMNT = '" + availableamt + "' where BM_ACC_CODE='" + objRepair.sBudgetAccCode + "'";
                    ObjCon.Execute(strQry);


                    strQry = "UPDATE TBLOILSENTMASTER SET OSD_BT_ID = '" + BtId + "' where OSD_ID='" + objRepair.sID + "'";
                    ObjCon.Execute(strQry);
                }
                else
                {
                    strQry = "UPDATE TBLOILSENTMASTER SET OSD_QUANTITY = '" + objRepair.sOilQty + "', OSD_AGENCY='" + objRepair.sAgencyname + "',OSD_AMOUNT='" + objRepair.sAmount + "'  WHERE OSD_PO_NO = '" + sPurchaseOrderNo + "'";
                    ObjCon.Execute(strQry);

                }
                //ObjCon.CommitTrans();

                DataTable OilDetails = new DataTable("OilDetails");
                OilDetails.Columns.Add("OSD_ID", typeof(string));
                OilDetails.Columns.Add("OSD_PO_NO", typeof(string));
                OilDetails.Columns.Add("OSD_PO_DATE", typeof(string));
                OilDetails.Columns.Add("OSD_INVOICE_NO", typeof(string));
                OilDetails.Columns.Add("OSD_INVOICE_DATE", typeof(string));
                OilDetails.Columns.Add("OSD_QUANTITY", typeof(string));
                OilDetails.Columns.Add("OSD_CRON", typeof(string));
                OilDetails.Columns.Add("OSD_CRBY", typeof(string));
                OilDetails.Columns.Add("OSD_OFFICE_CODE", typeof(string));
                OilDetails.Columns.Add("OSD_STATUS", typeof(string));
                OilDetails.Columns.Add("OSD_ITEM_CODE", typeof(string));
                OilDetails.Columns.Add("OSD_TYPE", typeof(string));
                OilDetails.Columns.Add("OSD_REMARKS", typeof(string));
                OilDetails.Columns.Add("OSD_AGENCY", typeof(string));
                OilDetails.Columns.Add("OSD_MMS_AUTO_INV_NO", typeof(string));
                OilDetails.Columns.Add("OSD_AGENCY_NAME", typeof(string));
                OilDetails.Columns.Add("OSD_AMOUNT", typeof(string));
                DataRow drow = OilDetails.NewRow();
                drow["OSD_ID"] = sID;
                DateTime issue_date = DateTime.ParseExact(objRepair.sPurchaseDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                invoice_date = issue_date;
                drow["OSD_PO_NO"] = sPurchaseOrderNo.ToUpper();
                issue_date = DateTime.ParseExact(objRepair.sPurchaseDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                drow["OSD_PO_DATE"] = issue_date.ToString("yyyy-MM-dd");
                drow["OSD_INVOICE_NO"] = objRepair.sInvoiceNo;
                issue_date = DateTime.ParseExact(objRepair.sInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                drow["OSD_INVOICE_DATE"] = issue_date.ToString("yyyy-MM-dd");
                drow["OSD_OFFICE_CODE"] = objRepair.sOfficeCode;
                drow["OSD_AGENCY"] = objRepair.sAgencyname;
                drow["OSD_MMS_AUTO_INV_NO"] = objRepair.sMMSAutoInvNo;
                drow["OSD_QUANTITY"] = objRepair.sOilQty;
                drow["OSD_TYPE"] = "INVOICE";
                drow["OSD_REMARKS"] = "CREATED OIL PO DETAILS";
                drow["OSD_ITEM_CODE"] = objRepair.sItemCode;
                drow["OSD_AGENCY_NAME"] = objRepair.sAgencyName;
                drow["OSD_AMOUNT"] = objRepair.sAmount;
                OilDetails.Rows.Add(drow);
                string[] result = new string[2];

                if (invoice_date < dtHost)
                {
                    res = true;
                }
                else
                {
                    result = objWCF.saveOil_invoice(OilDetails);

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
                if (res == true)
                {
                    strQry = "UPDATE  TBLOILSENTMASTER SET OSD_STATUS_FLAG = '1' WHERE OSD_PO_NO = '" + sPurchaseOrderNo + "'";

                    strQry = strQry.Replace("'", "''");

                    string sParam = "SELECT NVL(MAX(OSD_ID),0)+1 FROM TBLOILSENTMASTER";
                    //
                    string sParam1 = "SELECT WONUMBERLATEST('" + sOfficeCode + "') FROM DUAL";

                    sParam1 = sParam1.Replace("'", "''");

                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();

                    objApproval.sFormName = objRepair.sFormName;
                    objApproval.sOfficeCode = objRepair.sOfficeCode;
                    objApproval.sClientIp = objRepair.sClientIP;
                    objApproval.sCrby = objRepair.sCrby;
                    objApproval.sWFObjectId = objRepair.sWFOId;
                    objApproval.sWFAutoId = objRepair.sWFAutoId;
                    objApproval.sagency = objRepair.sAgencyname;

                    objApproval.sQryValues = strQry;
                    objApproval.sParameterValues = sParam + ";" + sParam1;
                    objApproval.sMainTable = "TBLOILSENTMASTER";

                    objApproval.sDataReferenceId = objRepair.sPurchaseOrderNo;

                    objApproval.sRefOfficeCode = objRepair.sOfficeCode;

                    objApproval.sDescription = "Oil Issue To Agency With PO No: " + objRepair.sPurchaseOrderNo + " and Invoice No :" + objRepair.sInvoiceNo + " ";

                    string sPrimaryKey = "{0}";

                    objApproval.sColumnNames = "OSD_ID,OSD_PO_NO,OSD_PO_DATE,OSD_INVOICE_NO,OSD_INVOICE_DATE,OSD_QUANTITY,OSD_CRON,OSD_CRBY,OSD_OFFICE_CODE,OSD_STATUS,OSD_ITEM_CODE,OSD_AGENCY,OSD_AMOUNT,OSD_OILQUANTITY_LTR";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objRepair.sPurchaseOrderNo + "," + objRepair.sPurchaseDate + ",";
                    objApproval.sColumnValues += "" + objRepair.sInvoiceNo + "," + objRepair.sInvoiceDate + "," + objRepair.sOilQty + ",SYSDATE," + objRepair.sCrby + "," + objRepair.sOfficeCode + ",0," + objRepair.sItemCode + "," + objApproval.sagency + "," + objRepair.sAmount + "," + objRepair.sQuantityLtr + "";
                    objApproval.sTableNames = "TBLOILSENTMASTER";

                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objRepair.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objRepair.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }


                    Arr[0] = "Details Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    Arr[0] = "Something Went Wrong";
                    Arr[1] = "1";
                    return Arr;
                }


                //  return Arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Arr;
            }
        }

        public clsDTrRepairActivity GetAgencyDetailsFromXML(clsDTrRepairActivity objRepair)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if (objRepair.sWFDataId != "" && objRepair.sWFDataId != null)
                {
                    dt = objApproval.GetDatatableFromXML(objRepair.sWFDataId);
                }

                if (dt.Rows.Count > 0)
                {
                    objRepair.sID = Convert.ToString(dt.Rows[0]["OSD_ID"]).Trim();
                    objRepair.sPurchaseOrderNo = Convert.ToString(dt.Rows[0]["OSD_PO_NO"]).Trim();
                    objRepair.sPurchaseDate = Convert.ToString(dt.Rows[0]["OSD_PO_DATE"]).Trim();
                    objRepair.sInvoiceNo = Convert.ToString(dt.Rows[0]["OSD_INVOICE_NO"]);
                    objRepair.sInvoiceDate = Convert.ToString(dt.Rows[0]["OSD_INVOICE_DATE"]);

                    objRepair.sOilQty = Convert.ToString(dt.Rows[0]["OSD_QUANTITY"]).Trim();
                    //  objRepair.scron = Convert.ToString(dt.Rows[0]["OSD_CRON"]).Trim();
                    objRepair.sCrby = Convert.ToString(dt.Rows[0]["OSD_CRBY"]).Trim();
                    objRepair.sDivision = Convert.ToString(dt.Rows[0]["OSD_OFFICE_CODE"]).Trim();
                    objRepair.sStatus = Convert.ToString(dt.Rows[0]["OSD_STATUS"]).Trim();
                    objRepair.sAgencyname = Convert.ToString(dt.Rows[0]["OSD_AGENCY"]).Trim();
                    objRepair.sAmount = Convert.ToString(dt.Rows[0]["OSD_AMOUNT"]).Trim();
                    objRepair.sQuantityLtr = Convert.ToString(dt.Rows[0]["OSD_OILQUANTITY_LTR"]).Trim();
                }
                return objRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objRepair;
            }
        }

        public DataTable GetRepairerOilDetailsFromXML(clsDTrRepairActivity objRepairOilInvoice)
        {
            oledbCommand = new OleDbCommand();
            DataTable dtRepairerOilInvoiced = new DataTable();
            DataTable DtForGrid = new DataTable("RepairerOilDetails");

            try
            {
                clsApproval objApproval = new clsApproval();
                if (objRepairOilInvoice.sWFDataId != "" && objRepairOilInvoice.sWFDataId != null)
                {
                    dtRepairerOilInvoiced = objApproval.GetDatatableFromXML(objRepairOilInvoice.sWFDataId);
                }

                if (dtRepairerOilInvoiced.Rows.Count > 0)
                {
                    DtForGrid.Columns.Add("RSD_ID", typeof(string));
                    DtForGrid.Columns.Add("RSM_PO_NO", typeof(string));
                    DtForGrid.Columns.Add("TC_QUANTITY", typeof(string));
                    DtForGrid.Columns.Add("RSM_INV_NO", typeof(string));
                    DtForGrid.Columns.Add("TOTAL_OIL_AMOUNT", typeof(string));
                    DtForGrid.Columns.Add("SENT_OIL", typeof(string));
                    DtForGrid.Columns.Add("ROI_PENDINGQTY", typeof(string));
                    DtForGrid.Columns.Add("TC_CODE", typeof(string));
                    DtForGrid.Columns.Add("RSM_ISSUE_DATE", typeof(string));
                    DtForGrid.Columns.Add("RSD_OIL_QUANTITY", typeof(string));
                    DtForGrid.Columns.Add("SUP_REPNAME", typeof(string));
                    DtForGrid.Columns.Add("ROI_INVOICED_OILQTY", typeof(string));
                    DtForGrid.Columns.Add("ROI_INVOICED_DATE", typeof(string));
                    DtForGrid.Columns.Add("ROI_INVOICE_QTY_KLTR", typeof(string));
                    DtForGrid.Columns.Add("REPAIRER_SENT_OIL", typeof(string));
                    DtForGrid.Columns.Add("ROI_ITEM_ID", typeof(string));

                    DataRow drow = DtForGrid.NewRow();
                    drow["RSD_ID"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["RSD_ID"]).Trim();
                    drow["RSM_PO_NO"] = objRepairOilInvoice.sPurchaseOrderNo;
                    drow["TC_QUANTITY"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_TC_QUANTITY"]).Trim();
                    drow["RSM_INV_NO"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_INVOICE_NO"]).Trim();
                    drow["TOTAL_OIL_AMOUNT"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_TOTAL_OILQTY"]).Trim();
                    drow["SENT_OIL"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_INVOICED_OILQTY"]).Trim();
                    drow["ROI_PENDINGQTY"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_PENDINGQTY"]).Trim();
                    drow["RSM_ISSUE_DATE"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_CRON"]).Trim();
                    drow["RSD_OIL_QUANTITY"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_TOTAL_OILQTY"]).Trim();
                    drow["ROI_INVOICED_OILQTY"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_INVOICED_OILQTY"]).Trim();
                    drow["ROI_INVOICED_DATE"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_INVOICE_DATE"]).Trim();
                    drow["SUP_REPNAME"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_REPAIRERNAME"]).Trim();
                    drow["ROI_INVOICE_QTY_KLTR"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_INVOICE_QTY_KLTR"]).Trim();
                    drow["REPAIRER_SENT_OIL"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["REPAIRER_SENT_OIL"]).Trim();
                    drow["ROI_ITEM_ID"] = Convert.ToString(dtRepairerOilInvoiced.Rows[0]["ROI_ITEM_ID"]).Trim();

                    DtForGrid.Rows.Add(drow);

                }
                return DtForGrid;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return DtForGrid;
            }
        }
        public string[] LoadAlreadyPurchaseOrder(clsDTrRepairActivity objRepair)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            string existpo = string.Empty;
            string strQry = string.Empty;
            string[] Arr = new string[3];
            try
            {

                strQry = "select * from (select XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//OSD_PO_NO/text()').getStringVal() as OSD_PO_NO  FROM TBLWORKFLOWOBJECTS,TBLWFODATA  WHERE WO_WFO_ID = wfo_id AND  WO_BO_ID IN (48) AND WO_RECORD_ID  < 0 and  WO_APPROVE_STATUS=0 and WO_OFFICE_CODE ='" + objRepair.sOfficeCode + "') A where A.OSD_PO_NO =:sPono";
                oledbCommand.Parameters.AddWithValue("sPono", objRepair.sPurchaseOrderNo);
                //oledbCommand.Parameters.AddWithValue("sLocationCode", objRepair.sOfficeCode);
                existpo = ObjCon.get_value(strQry, oledbCommand);
                if (existpo.Length > 0)
                {
                    Arr[0] = "Purchase Order Number Already Exist";
                    Arr[1] = "2";
                    return Arr;
                }
                if (objRepair.sPurchaseOrderNo != "")
                {
                    strQry = "SELECT \"OSD_PO_NO\" FROM \"TBLOILSENTMASTER\" WHERE \"OSD_PO_NO\"='" + objRepair.sPurchaseOrderNo + "' AND \"OSD_OFFICE_CODE\" ='" + objRepair.sOfficeCode + "'";
                    existpo = ObjCon.get_value(strQry, oledbCommand);

                    if (existpo.Length > 0)
                    {
                        Arr[0] = "Purchase Order Number Already Exist";
                        Arr[1] = "2";
                        return Arr;
                    }
                }
                if (Arr[1] == null)
                {
                    Arr[1] = "1";
                }


                return Arr;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Arr;

            }
        }


        public string getdivcode(clsDTrRepairActivity objRepair)
        {
            oleDbCommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CODE ='" + objRepair.sOfficeCode + "'";
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string getAvailableBudget(clsDTrRepairActivity objRepair)
        {
            string BudgetAmount = string.Empty;
            string strQry = string.Empty;
            try
            {
                ERPService.ERPServiceClient obj = new ERPService.ERPServiceClient();
                string FormName = "Estimation_Form_dtlms";
                DateTime Date = DateTime.Now;
                string sDate = Date.ToString("dd/MM/yyyy");
                string sPath = "C:\\ERRORLOG\\ERRORLOGBudget" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "ErrorLog.txt";
                File.AppendAllText(sPath, "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + Environment.NewLine + "BEFORE BudgetAmount" + Environment.NewLine + "DivCode=" + objRepair.sDivCode + Environment.NewLine + " AccCode = " + objRepair.sBudgetAccCode + Environment.NewLine + " Date=" + sDate + Environment.NewLine + " Formname = " + FormName + Environment.NewLine);
                string SanctionedAmount = obj.FetchBudgetAmountForAccountCode("FMS", objRepair.sDivCode, objRepair.sBudgetAccCode, sDate, FormName);

                File.AppendAllText(sPath, "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + Environment.NewLine + "AFTER BudgetAmount = " + SanctionedAmount + Environment.NewLine + "###########################################################" + Environment.NewLine);

                objRepair.sBudgetAmount = Convert.ToString(Convert.ToDouble(SanctionedAmount) * 100000);

                File.AppendAllText(sPath, "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + Environment.NewLine + "AFTER Multiply SanctionedAmount = " + objRepair.sBudgetAmount + Environment.NewLine + "###########################################################" + Environment.NewLine);

                string BM_AMOUNT = ObjCon.get_value("SELECT BM_AMOUNT  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objRepair.sBudgetAccCode + "' ", oleDbCommand);
                if (BM_AMOUNT == "")
                {
                    objRepair.sBMID = Convert.ToString(ObjCon.Get_max_no("BM_ID", "TBLBUDGETMAST"));
                    strQry = "INSERT INTO TBLBUDGETMAST (BM_ID,BM_NO,BM_DATE,BM_AMOUNT,BM_ACC_CODE,BM_DIV_CODE,BM_CRON,BM_FIN_YEAR,BM_OB_AMNT,BM_OB_DATE) VALUES('" + objRepair.sBMID + "','" + objRepair.sBudgetAccCode + "',";
                    strQry += " SYSDATE,'" + objRepair.sBudgetAmount + "','" + objRepair.sBudgetAccCode + "','" + objRepair.sDivCode + "',SYSDATE,'" + objRepair.sFinancialYear + "','" + objRepair.sBudgetAmount + "',SYSDATE)";
                    ObjCon.Execute(strQry);
                }
                else
                {
                    string BM_OB_AMNT = ObjCon.get_value("SELECT BM_OB_AMNT  FROM TBLBUDGETMAST WHERE BM_ACC_CODE = '" + objRepair.sBudgetAccCode + "'", oleDbCommand);
                    string OB_AMT = ObjCon.get_value("SELECT SUM(BT_DEBIT_AMNT)  FROM TBLBUDGETTRANS ", oleDbCommand);
                    if (OB_AMT == "")
                    {
                        OB_AMT = "0";
                    }
                    string BudgetOpeningBalance = Convert.ToString(Convert.ToDouble(objRepair.sBudgetAmount) - Convert.ToDouble(OB_AMT));
                    File.AppendAllText(sPath, "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + Environment.NewLine + "AFTER BudgetOpeningBalance = " + BudgetOpeningBalance + Environment.NewLine + "###########################################################" + Environment.NewLine);

                    if (Convert.ToDouble(BM_OB_AMNT) <= 0)
                    {
                        strQry = "UPDATE TBLBUDGETMAST SET BM_AMOUNT ='" + objRepair.sBudgetAmount + "',BM_OB_AMNT='" + objRepair.sBudgetAmount + "' WHERE BM_ACC_CODE='" + objRepair.sBudgetAccCode + "'";
                        ObjCon.Execute(strQry);
                    }
                    else
                    //if (BudgetAmount != BM_AMOUNT)
                    {
                        strQry = "UPDATE TBLBUDGETMAST SET BM_AMOUNT ='" + objRepair.sBudgetAmount + "',BM_OB_AMNT ='" + BudgetOpeningBalance + "'  WHERE BM_ACC_CODE='" + objRepair.sBudgetAccCode + "'";
                        ObjCon.Execute(strQry);
                    }

                }
                return objRepair.sBudgetAmount;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BudgetAmount;
            }
        }
        public string getBudgetAccountCode()
        {
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT  MD_NAME from TBLMASTERDATA where MD_TYPE = 'FMSACHEAD'  ORDER BY MD_ID";
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return "";
            }
        }
        public DataTable LoadInvoicedOilDetails(clsDTrRepairActivity objinvoiceoildetails)
        {
            oleDbCommand = new OleDbCommand();
            DataTable dtInvoicedDetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT RSM_PO_NO,ROI_INVOICE_NO,TO_CHAR(ROI_INVOICE_DATE,'dd-MM-yyyy')ROI_INVOICE_DATE,ROI_MMS_INVOICE_NO,ROI_TOTAL_OILQTY,ROI_INVOICED_OILQTY ";
                strQry += " FROM TBLREPAIRSENTMASTER INNER JOIN TBL_REPAIREROIL_INVOICE ON RSM_ID=ROI_RSM_ID WHERE RSM_PO_NO like'%'";
                strQry += "AND ROI_OFFICECODE ='" + objinvoiceoildetails.sOfficeCode + "'  AND ROI_OILSENT_BY='1'  ORDER BY ROI_ID desc";
                dtInvoicedDetails = ObjCon.getDataTable(strQry);
                return dtInvoicedDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return dtInvoicedDetails;
            }
        }


        /// <summary>
        /// This function used to get the oil quantity details. 
        /// </summary>
        /// <param name="sPurchaseOrderNo"></param>
        /// <returns></returns>
        public DataTable LoadOilDetailsOnPONO(clsDTrRepairActivity objOilDetailsOnPONO)
        {
            DataTable dtLoadOilDetailsOnPONO = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();

                string strQry = string.Empty;
                string oiltype = string.Empty;

                if (objOilDetailsOnPONO.sPurchaseOrderNo.Trim() != "")
                {
                    oiltype = GetOilTypeByPoNo(objOilDetailsOnPONO.sPurchaseOrderNo);
                }

                if (oiltype == "0")
                {
                    string RSM_ID = ObjCon.get_value("SELECT RSM_ID FROM TBLREPAIRSENTMASTER WHERE RSM_PO_NO='" + objOilDetailsOnPONO.sPurchaseOrderNo + "'");
                    string ROI_RSM_ID = ObjCon.get_value("select * from TBL_REPAIREROIL_INVOICE where ROI_RSM_ID = '" + RSM_ID + "'");

                    if (ROI_RSM_ID == "")
                    {
                        strQry = "SELECT RSM_PO_NO, SUM(RSD_OIL_QUANTITY)TOTAL_OIL_AMOUNT, 0 AS STORE_SENT_OIL,0 AS REPAIRER_SENT_OIL, ";
                        strQry += " CASE WHEN RSD_PENDING_OIL = '' THEN RSD_PENDING_OIL ELSE SUM(RSD_OIL_QUANTITY)END  AS ROI_PENDINGQTY FROM ";
                        strQry += " TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER WHERE  RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE ";
                        strQry += " AND RSM_PO_NO = '" + objOilDetailsOnPONO.sPurchaseOrderNo + "' GROUP BY RSM_PO_NO, RSD_SENT_OIL, RSD_PENDING_OIL";
                    }
                    else
                    {

                        strQry = " SELECT RSM_PO_NO,(SELECT SUM(RSD_OIL_QUANTITY) FROM TBLREPAIRSENTDETAILS, TBLREPAIRSENTMASTER WHERE RSM_ID = RSD_RSM_ID AND ";
                        strQry += " RSM_PO_NO = '" + objOilDetailsOnPONO.sPurchaseOrderNo + "')TOTAL_OIL_AMOUNT,";
                        strQry += " (SELECT SUM(ROI_INVOICED_OILQTY) FROM TBL_REPAIREROIL_INVOICE where ROI_RSM_ID = '" + RSM_ID + "' AND ROI_OILSENT_BY = '1') AS STORE_SENT_OIL,";
                        strQry += " (SELECT SUM(ROI_INVOICED_OILQTY) FROM TBL_REPAIREROIL_INVOICE where ROI_RSM_ID = '" + RSM_ID + "' AND ROI_OILSENT_BY = '2') AS REPAIRER_SENT_OIL,  ";
                        strQry += " ROI_PENDINGQTY FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBL_REPAIREROIL_INVOICE WHERE RSM_ID = RSD_RSM_ID ";


                        if (sPurchaseOrderNo.Trim() != "")
                        {
                            strQry += " AND UPPER(RSM_PO_NO)='" + Convert.ToString(objOilDetailsOnPONO.sPurchaseOrderNo).ToString().ToUpper() + "' ";
                        }
                        if (RSM_ID != "" && ROI_RSM_ID != "")
                        {
                            strQry += " AND ROI_RSM_ID=" + Convert.ToInt32(RSM_ID) + "  ORDER BY ROI_ID DESC";
                        }
                        strQry += " FETCH FIRST 1 ROWS ONLY";
                    }

                    dtLoadOilDetailsOnPONO = ObjCon.getDataTable(strQry);

                }
                return dtLoadOilDetailsOnPONO;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtLoadOilDetailsOnPONO;
            }
        }
        // <summary>
        /// This method used to upaded the uploaded document path of ht inspected records.
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateHT_Uploadedpath(clsDTrRepairActivity obj)
        {
            try
            {
                string Qry = string.Empty;
                string ind_id = ObjCon.get_value("SELECT IND_ID FROM TBLINSPECTIONDETAILS  WHERE IND_RSD_ID='" + obj.RepairDetailsId
                     + "' ORDER BY IND_ID DESC FETCH FIRST 1 ROWS ONLY");

                Qry = "Update TBLINSPECTIONDETAILS set IND_DOC = '" + obj.UploadedpathHt + "' where IND_ID='" + ind_id + "'";
                ObjCon.Execute(Qry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        // <summary>
        /// This method used to upaded the uploaded document path of ht inspected records.
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateEE_Uploadedpath(clsDTrRepairActivity obj)
        {
            try
            {
                string Qry = string.Empty;
                string ind_id = ObjCon.get_value("SELECT IND_ID FROM TBLINSPECTIONDETAILS  WHERE IND_RSD_ID='" + obj.RepairDetailsId
                     + "' ORDER BY IND_ID DESC FETCH FIRST 1 ROWS ONLY");

                Qry = "Update TBLINSPECTIONDETAILS set IND_EE_OM_UPLOADED_PATH = '" + obj.UploadedpathHt + "' where IND_ID='" + ind_id + "'";
                ObjCon.Execute(Qry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        /// <summary>
        /// This method used to fetch the pending records of repairer inspection for EE Div 
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public DataTable LoadPendingForEETestingDetails(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                //strQry = "SELECT RSM_ID,RSM_PO_NO, TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY') AS PODATE, TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') AS ISSUEDATE, ";
                //strQry += " (CASE WHEN RSM_SUPREP_TYPE = '2' THEN(SELECT TR_NAME  FROM TBLTRANSREPAIRER  WHERE TR_ID = RSM_SUPREP_ID) WHEN ";
                //strQry += " RSM_SUPREP_TYPE = '1' THEN(SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID = RSM_SUPREP_ID) END ) SUP_REPNAME, ";
                //strQry += " RSM_PO_QNTY PO_QUANTITY, SUM(CASE WHEN RSD_DELIVARY_DATE IS NULL AND IND_EE_APPROVE_STATUS = 0 THEN 1 ELSE 0 END) ";
                //strQry += " PENDING_QNTY, SUM(CASE WHEN RSD_DELIVARY_DATE IS NOT NULL AND  IND_EE_APPROVE_STATUS = 1 ";
                //strQry += " AND IND_EE_INSPECTION_RESULT IS NOT NULL THEN 1 ELSE 0 END) ";
                //strQry += " EE_INS_QTY , SUM(CASE WHEN IND_INSP_RESULT IN (1,3,5,6,7) THEN 1 ELSE 0 END) INSPECTED_QNTY_HT ";
                //strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS WHERE IND_RSD_ID = RSD_ID ";
                //strQry += " AND RSM_ID = RSD_RSM_ID  AND RSD_ID NOT IN  (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE ";
                //strQry += " IND_INSP_RESULT IN(1, 3)) AND RSM_DIV_CODE LIKE :OfficeCode ||'%' AND IND_EE_APPROVE_STATUS  = 0 ";

                //oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);

                //if (objTestPending.sPurchaseOrderNo.Trim() != "")
                //{
                //    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                //    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                //}

                //strQry += " GROUP BY RSM_ID,RSM_PO_NO, RSM_PO_QNTY, TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY'),";
                //strQry += " TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY'),RSM_SUPREP_TYPE,RSM_SUPREP_ID";

                strQry = "  SELECT * FROM     (SELECT RSM_ID,RSM_PO_NO, TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY') AS PODATE, TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') AS ISSUEDATE, ";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE = '2' THEN(SELECT TR_NAME  FROM TBLTRANSREPAIRER  WHERE TR_ID = RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE = '1' THEN(SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID = RSM_SUPREP_ID) END ) SUP_REPNAME, ";
                strQry += " RSM_PO_QNTY PO_QUANTITY, SUM(CASE WHEN RSD_DELIVARY_DATE IS NULL AND IND_EE_APPROVE_STATUS = 0 THEN 1 ELSE 0 END) ";
                strQry += " PENDING_QNTY, SUM(CASE WHEN RSD_DELIVARY_DATE IS NOT NULL AND  IND_EE_APPROVE_STATUS = 1 ";
                strQry += " AND IND_EE_INSPECTION_RESULT IS NOT NULL THEN 1 ELSE 0 END) ";
                strQry += " EE_INS_QTY , SUM(CASE WHEN IND_INSP_RESULT IN (1,3,4,5,6,7) THEN 1 ELSE 0 END) INSPECTED_QNTY_HT ";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS WHERE IND_RSD_ID = RSD_ID ";
                strQry += " AND RSM_ID = RSD_RSM_ID  AND RSD_ID IN  (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE ";
                strQry += " IND_INSP_RESULT IN(1,3,4,5,6,7)) AND RSM_DIV_CODE LIKE :OfficeCode ||'%'  ";

                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);

                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }

                strQry += " GROUP BY RSM_ID,RSM_PO_NO, RSM_PO_QNTY, TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY'),";
                strQry += " TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY'),RSM_SUPREP_TYPE,RSM_SUPREP_ID)A WHERE PENDING_QNTY <> 0 ";

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        public DataTable LoadEETestingPassDetails(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT RSM_ID,RSM_PO_NO, TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY') AS PODATE, TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') AS ISSUEDATE, ";
                strQry += " (CASE WHEN RSM_SUPREP_TYPE = '2' THEN(SELECT TR_NAME  FROM TBLTRANSREPAIRER  WHERE TR_ID = RSM_SUPREP_ID) WHEN ";
                strQry += " RSM_SUPREP_TYPE = '1' THEN(SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID = RSM_SUPREP_ID) END ) SUP_REPNAME, ";
                strQry += " RSM_PO_QNTY PO_QUANTITY, SUM(CASE WHEN IND_EE_APPROVE_STATUS = 1 ";
                strQry += " AND IND_EE_INSPECTION_RESULT IS NOT NULL THEN 1 ELSE 0 END) ";
                strQry += " EE_INS_QTY , SUM(CASE WHEN IND_INSP_RESULT IN (1,3,5,6,7) THEN 1 ELSE 0 END) INSPECTED_QNTY_HT ";
                strQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS WHERE IND_RSD_ID = RSD_ID ";
                strQry += " AND RSM_ID = RSD_RSM_ID  AND RSD_ID NOT IN  (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS WHERE ";
                strQry += " IND_INSP_RESULT IN(1, 3)) AND RSM_DIV_CODE LIKE :OfficeCode ||'%' AND IND_EE_APPROVE_STATUS  = 1 ";

                oleDbCommand.Parameters.AddWithValue("OfficeCode", objTestPending.sOfficeCode);

                if (objTestPending.sPurchaseOrderNo.Trim() != "")
                {
                    strQry += " AND UPPER(RSM_PO_NO)=:PONO";
                    oleDbCommand.Parameters.AddWithValue("PONO", objTestPending.sPurchaseOrderNo.ToString().ToUpper());
                }

                strQry += " GROUP BY RSM_ID,RSM_PO_NO, RSM_PO_QNTY, TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY'),";
                strQry += " TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY'),RSM_SUPREP_TYPE,RSM_SUPREP_ID";

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Get RepairMaster Detaisl
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public DataTable GetRepairMasterDetaisl(clsDTrRepairActivity Obj)
        {
            DataTable Dt = new DataTable();
            string SelQry = string.Empty;
            oleDbCommand = new OleDbCommand();
            try
            {
                SelQry = " SELECT RSM_PO_NO,TO_CHAR(RSM_ISSUE_DATE,'DD-MON-YYYY') RSM_ISSUE_DATE, RSD_ID, TC_CODE,TC_SLNO, ";
                SelQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID =  TC_MAKE_ID) TM_NAME,  TO_CHAR(TC_CAPACITY) AS CAPACITY, ";
                SelQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE, (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM ";
                SelQry += " TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN  RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME ";
                SelQry += " FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END ) SUP_REPNAME,RSM_OLD_PO_NO,RSM_REMARKS,RSM_OIL_TYPE, ";
                SelQry += " (CASE WHEN IND_INSP_RESULT='5' THEN 'ALL TEST OK EXPT CU LOSS' WHEN  IND_INSP_RESULT='6' THEN ";
                SelQry += " 'ALL TEST OK EXPT IRON LOSS' WHEN IND_INSP_RESULT='7' THEN 'ALL TEST OK EXPT CU & IRON LOSS' END) INS_RES_HT,";
                SelQry += " IND_REMARKS,IND_DOC as HT_uplodede_doc,IND_TEST_LOCATION,TO_CHAR(IND_INSP_DATE,'DD-MON-YYYY') AS IND_INSP_DATE, IND_INSP_BY";
                SelQry += " FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER,TBLINSPECTIONDETAILS ";
                SelQry += " WHERE IND_RSD_ID = RSD_ID AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND TC_CURRENT_LOCATION='3' ";
                SelQry += " AND RSD_DELIVARY_DATE IS NULL AND RSD_ID  IN (SELECT IND_RSD_ID FROM TBLINSPECTIONDETAILS ";
                SelQry += " WHERE IND_INSP_RESULT IN(5,6,7)) AND RSM_DIV_CODE LIKE '" + Obj.sOfficeCode + "%' ";
                SelQry += " AND RSM_ID IN (" + Obj.RepairDetailsId + ") AND IND_EE_APPROVE_STATUS=0 AND UPPER(RSM_PO_NO)='" + Obj.sPurchaseOrderNo + "' ";

                Dt = ObjCon.getDataTable(SelQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            return Dt;
        }
        /// <summary>
        /// Save EE Testing Details
        /// </summary>
        /// <param name="strRepairDetailsIds"></param>
        /// <param name="objpending"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string[] SaveEETestingDetails(string[] strRepairDetailsIds, clsDTrRepairActivity objpending, DataTable dt)
        {
            string[] Arr = new string[2];

            try
            {
                string[] strDetailVal = strRepairDetailsIds.ToArray();

                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    string strQry = string.Empty;

                    string RSM_ID = string.Empty;
                    string itemType = string.Empty;
                    string EERemarks = string.Empty;
                    string OMNUM = string.Empty;
                    string OMDate = string.Empty;
                    string IND_ID = string.Empty;

                    if (strDetailVal[i] != null)
                    {
                        RSM_ID = strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper().Trim();
                        itemType = strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper().Trim();
                        EERemarks = strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper().Trim();
                        OMNUM = strDetailVal[i].Split('~').GetValue(3).ToString().ToUpper().Trim();
                        OMDate = strDetailVal[i].Split('~').GetValue(4).ToString().ToUpper().Trim();

                        IND_ID = ObjCon.get_value("select IND_ID from TBLINSPECTIONDETAILS WHERE IND_INSP_RESULT in(5,6,7) " +
                            " AND IND_EE_APPROVE_STATUS = 0 AND IND_RSD_ID ='" + RSM_ID + "' FETCH FIRST 1 ROWS ONLY");

                        if ((IND_ID ?? "").Length > 0)
                        {
                            strQry = " UPDATE TBLINSPECTIONDETAILS SET IND_EE_INSPECTION_RESULT = " + itemType + ", ";
                            strQry += " IND_EE_OMNO = '" + OMNUM + "', IND_EE_OM_DATE = TO_DATE('" + OMDate + "','dd/MM/yyyy'), IND_EE_REMARKS = '" + EERemarks + "', ";
                            strQry += " IND_EE_APPROVE_STATUS = 1 WHERE IND_ID = " + IND_ID + " AND IND_RSD_ID = " + RSM_ID + " ";
                            ObjCon.Execute(strQry);

                            Arr[0] = "Testing Done Successfully";
                            Arr[1] = "0";
                        }
                    }
                }

                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Arr;
            }
            finally
            {

            }
        }
    }
}
