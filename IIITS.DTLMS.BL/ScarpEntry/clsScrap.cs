using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
namespace IIITS.DTLMS.BL
{
    public class clsScrap
    {
        string strFormCode = "clsScrap";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);


        public string sMakeId { get; set; }
        public string sStoreId { get; set; }
        public string sSupplierId { get; set; }
        public string sTcId { get; set; }

        public string sScrapId { get; set; }
        public string sScrapDetailsId { get; set; }
        public string sIssueDate { get; set; }
        public string sWorkOrderNo { get; set; }
        public string sWorkOrderDate { get; set; }
        public int sDTrCount { get; set; }
        public string sCrby { get; set; }
        public string sSendTo { get; set; }

        //Tc Details
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }
        public string sMakeName { get; set; }
        public string sManfDate { get; set; }
        public string sCapacity { get; set; }
        public string sPurchaseDate { get; set; }
        public string sWarrantyPeriod { get; set; }
        public string sSupplierName { get; set; }
        public string sRemarks { get; set; }

        public string sOMNo { get; set; }
        public string sAmount { get; set; }
        public string sDisposeDesc { get; set; }
        public string sStatus { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }

        public string sOfficeCode { get; set; }

        public string sTestResult { get; set; }
        OleDbCommand oleDbCommand;
        public DataTable LoadTCForScrap(clsScrap objScrap)
        {
            DataTable dt = new DataTable();
             oleDbCommand = new OleDbCommand();
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST WHERE TC_MAKE_ID=TM_ID AND SM_ID=TC_STORE_ID";
                strQry += " AND TC_STATUS=7 ";
                strQry += " AND TC_LOCATION_ID LIKE :OfficeCode || '%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objScrap.sOfficeCode);
                if (objScrap.sTcId != null)
                {
                    strQry += " AND TC_ID IN (" + objScrap.sTcId + ")";
                }
                else
                {
                    strQry += " AND TC_CURRENT_LOCATION=1 AND TO_CHAR(TC_CODE) NOT IN (SELECT WO_DATA_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='30' AND WO_APPROVE_STATUS='0') ";
                }
                if (objScrap.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY=:Capacity ";
                    oleDbCommand.Parameters.AddWithValue("Capacity", objScrap.sCapacity);
                }
                if (objScrap.sMakeId != null)
                {
                    strQry += " AND TC_MAKE_ID=:MakeID";
                    oleDbCommand.Parameters.AddWithValue("MakeID", objScrap.sMakeId);
                }
                if (objScrap.sStoreId != null)
                {
                    strQry += " AND SM_ID=:SMID";
                    oleDbCommand.Parameters.AddWithValue("SMID", objScrap.sStoreId);
                }

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " LoadFaultTC");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadFaultTCForScrap(clsScrap objScrap)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT TC_ID,TO_CHAR(TC_CODE)TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TC_STATUS,";
                strQry += " (SELECT IND_DOC FROM TBLINSPECTIONDETAILS WHERE IND_ID =(SELECT max(ind_id) FROM TBLINSPECTIONDETAILS WHERE IND_RSD_ID IN(SELECT max(RSD_ID) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE RSD_TC_CODE=TC_CODE AND RSD_DELIVARY_DATE IS NOT NULL )))IND_DOC,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST WHERE TC_MAKE_ID=TM_ID AND SM_ID=TC_STORE_ID";
                strQry += " AND TC_STATUS=6 ";
                strQry += " AND TC_LOCATION_ID LIKE :OfficeCode || '%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objScrap.sOfficeCode);
                if (objScrap.sTcId != null)
                {
                    strQry += " AND TC_ID IN (" + objScrap.sTcId + ")";
                }
                else
                {
                    strQry += " AND TC_CURRENT_LOCATION=1 AND TO_CHAR(TC_CODE) NOT IN (SELECT WO_DATA_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='30' AND WO_APPROVE_STATUS='0') ";
                }
                if (objScrap.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY=:Capacity ";
                    oleDbCommand.Parameters.AddWithValue("Capacity", objScrap.sCapacity);
                }
                if (objScrap.sMakeId != null)
                {
                    strQry += " AND TC_MAKE_ID=:MakeID";
                    oleDbCommand.Parameters.AddWithValue("MakeID", objScrap.sMakeId);
                }
                if (objScrap.sStoreId != null)
                {
                    strQry += " AND SM_ID=:SMID";
                    oleDbCommand.Parameters.AddWithValue("SMID", objScrap.sStoreId);
                }

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " LoadFaultTC");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadScrapTc(clsScrap objScrap)
        {
            DataTable dt = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                // TC_STATUS--------> 4 Scrap
                string strQry = string.Empty;
                strQry = "SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,SO_ID,ST_ID,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST,TBLSCRAPOBJECT,TBLSCRAPTC WHERE TC_MAKE_ID=TM_ID AND SM_ID=TC_STORE_ID";
                strQry += " AND TC_STATUS=4  AND SO_TC_CODE = TC_CODE AND ST_ID=SO_ST_ID ";
                strQry += " AND TC_LOCATION_ID LIKE :OfficeCode || '%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objScrap.sOfficeCode);
                if (objScrap.sTcId != null)
                {
                    strQry += " AND TC_ID IN (" + objScrap.sTcId + ")";
                }
                else
                {
                    strQry += " AND TC_CURRENT_LOCATION=1";
                }
                if (objScrap.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY=:Capacity ";
                    oleDbCommand.Parameters.AddWithValue("Capacity", objScrap.sCapacity);
                }
                if (objScrap.sMakeId != null)
                {
                    strQry += " AND TC_MAKE_ID=:MakeID";
                    oleDbCommand.Parameters.AddWithValue("MakeID", objScrap.sMakeId);
                }
                if (objScrap.sStoreId != null)
                {
                    strQry += " AND SM_ID=:SMID";
                    oleDbCommand.Parameters.AddWithValue("SMID", objScrap.sStoreId);
                }

                if (objScrap.sWorkOrderNo != null)
                {
                    strQry += " AND ST_OM_NO LIKE :WONO ||'%'";
                    oleDbCommand.Parameters.AddWithValue("WONO", objScrap.sWorkOrderNo);
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " LoadScrapTc");
                return dt;
            }
            finally
            {
                
            }
        }


        public string[] SaveScrapEntry(string[] sTcCodes, clsScrap objScrap)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
           
            bool bResult = false;
            try
            {
                oleDbCommand = new OleDbCommand();
                if (objScrap.sScrapId == "")
                {
                    ObjCon.BeginTrans();

                    string[] strDetailVal = sTcCodes.ToArray();

                    if (objScrap.sOfficeCode.Length > 2)
                    {
                        objScrap.sOfficeCode = objScrap.sOfficeCode.Substring(0, 2);
                    }

                    if (objScrap.sDTrCount > 0)
                    {
                        objScrap.sScrapId = ObjCon.Get_max_no("ST_ID", "TBLSCRAPTC").ToString();
                        strQry = "INSERT INTO TBLSCRAPTC(ST_ID,ST_OM_NO,ST_OM_DATE,ST_REMARKS,ST_CRBY,ST_CRON,ST_QTY,ST_DIV_CODE) VALUES";
                        strQry += " (:sScrapId,:sWorkOrderNo,TO_DATE(:sWorkOrderDate,'DD/MM/YYYY'),";
                        strQry += " :sRemarks,:sCrby,SYSDATE,:sDTrCount,:sOfficeCode)";

                        oleDbCommand.Parameters.AddWithValue("sScrapId", objScrap.sScrapId);
                        oleDbCommand.Parameters.AddWithValue("sWorkOrderNo", objScrap.sWorkOrderNo);
                        oleDbCommand.Parameters.AddWithValue("sWorkOrderDate", objScrap.sWorkOrderDate);
                        oleDbCommand.Parameters.AddWithValue("sRemarks", objScrap.sRemarks);
                        oleDbCommand.Parameters.AddWithValue("sCrby", objScrap.sCrby);
                        oleDbCommand.Parameters.AddWithValue("sDTrCount", objScrap.sDTrCount);
                        oleDbCommand.Parameters.AddWithValue("sOfficeCode", objScrap.sOfficeCode);

                        ObjCon.Execute(strQry, oleDbCommand);
                    }


                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        //dr = ObjCon.Fetch("select * from TBLSCRAPTC where  ST_TC_CODE='" + strDetailVal[i] + "'");
                        //if (dr.Read())
                        //{
                        //    Arr[0] = "TC Code Already Exists";
                        //    Arr[1] = "2";
                        //    return Arr;
                        //}
                        //dr.Close();

                        objScrap.sScrapDetailsId = ObjCon.Get_max_no("SO_ID", "TBLSCRAPOBJECT").ToString();
                        strQry = "INSERT INTO TBLSCRAPOBJECT(SO_ID,SO_ST_ID,SO_TC_CODE,SO_CRBY,SO_CRON) VALUES";
                        strQry += " ('" + objScrap.sScrapDetailsId + "','" + objScrap.sScrapId + "','" + strDetailVal[i] + "','" + objScrap.sCrby + "',SYSDATE)";
                        ObjCon.Execute(strQry);

                        //Update Scrap TC Status in TC Master
                        strQry = "UPDATE TBLTCMASTER SET TC_STATUS='4' where TC_CODE='" + strDetailVal[i] + "'";
                        ObjCon.Execute(strQry);

                        bResult = true;

                    }
                    ObjCon.CommitTrans();

                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        strQry = "SELECT CASE WHEN (TC_STATUS=1) THEN ITM_BRAND_NEW WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP')) THEN ITM_FAULTY_AGP WHEN ";
                        strQry += "(TC_STATUS=3 AND (WARRENTY_TYPE='WGP' OR WARRENTY_TYPE='WRGP')) THEN ITM_FAULTY_WGP WHEN (TC_STATUS=2) THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND ";
                        strQry += "(WARRENTY_TYPE='AGP') AND TC_MAKE_ID='98') THEN ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN ";
                        strQry += "ITM_REPAIRE_GOOD_ABB_MAKE WHEN TC_STATUS='4' THEN ITM_SCRAPE  END ITEM_CODE FROM (SELECT TC_CAPACITY,TC_STATUS,";
                        strQry += "ITM_FAULTY_AGP,ITM_FAULTY_WGP,ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE,ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,";
                        strQry += "TC_MAKE_ID,(CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT RSD_GUARRENTY_TYPE FROM TBLREPAIRSENTDETAILS WHERE ";
                        strQry += "RSD_TC_CODE =TC_CODE AND RSD_GUARRENTY_TYPE IS NOT NULL) WHEN DF_GUARANTY_TYPE IS NOT NULL THEN ";
                        strQry += "DF_GUARANTY_TYPE ELSE '' END)WARRENTY_TYPE FROM TBLTCMASTER,TBLITEMPRICEMASTER,TBLDTCFAILURE WHERE ";
                        strQry += "TC_CAPACITY =ITM_CAPACITY AND TC_CODE=DF_EQUIPMENT_ID AND DF_REPLACE_FLAG=0 AND TC_CODE='" + strDetailVal[i] + "')";
                        string sDtrItemCode = ObjCon.get_value(strQry);
                        string tc_code = strDetailVal[i];

                        DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                        objWcf.saveItem_code(sDtrItemCode, tc_code);
                        bResult = true;
                    }

                    if (bResult == true)
                    {
                        Arr[0] = "Scrap Details Saved Successfully";
                        Arr[1] = "0";
                    }
                    else
                    {
                        Arr[0] = "No Transformer Exists to do Scrap Entry";
                        Arr[1] = "2";
                    }
                    return Arr;

                }
                else
                {

                    //string[] strDetailVal = sTcCodes.ToArray();
                    //for (int i = 0; i < strDetailVal.Length; i++)
                    //{
                    //    strQry = "UPDATE TBLSCRAPTC SET ST_REMARKS='" + objScrap.sRemarks + "'  where ST_TC_CODE='" + strDetailVal[i] + "'";
                    //    ObjCon.Execute(strQry);
                    //    Arr[0] = "Scrap Details Updated Successfully";
                    //    Arr[1] = "1";
                    //    return Arr;
                    //}
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveScrapEntry");
                return Arr;
            }
            finally
            {
                
            }
        }
        public string[] SaveScrapDispose(string[] sTcCode, clsScrap objScrap)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            bool bResult = false;
            try
            {
                
                string[] strDetailVal = sTcCode.ToArray();

                ObjCon.BeginTrans();

                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    strQry = "UPDATE TBLSCRAPOBJECT SET SO_AMOUNT='" + objScrap.sAmount + "',SO_SEND_TO='" + sSendTo + "',";
                    strQry += " SO_DISPOSAL_BY='" + objScrap.sCrby + "',SO_DISPOSAL_DATE=SYSDATE,SO_DISPOSAL_DESC='" + objScrap.sDisposeDesc + "', ";
                    strQry += " SO_INV_NO='" + objScrap.sInvoiceNo + "',SO_INV_DATE=TO_DATE('" + objScrap.sInvoiceDate + "','DD/MM/YYYY')";
                    strQry += " WHERE SO_TC_CODE='" + strDetailVal[i] + "'";

                    ObjCon.Execute(strQry);

                    //Update TC Status in TC MaSTER    5--> dISPOSED
                    strQry = "UPDATE TBLTCMASTER SET TC_STATUS='5' WHERE TC_CODE='" + strDetailVal[i] + "'";
                    ObjCon.Execute(strQry);

                    bResult = true;

                }
                ObjCon.CommitTrans();
                if (bResult == true)
                {
                    Arr[0] = "Scrap Dispose Details Saved Successfully";
                    Arr[1] = "0";
                }
                else
                {
                    Arr[0] = "No Transformer Exists to Dispose";
                    Arr[1] = "2";
                }

                return Arr;

            }


            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveScrapDispose");
                return Arr;
            }
            finally
            {
                
            }
        }
        public clsDTrRepairActivity GetFaultTCDetails(clsDTrRepairActivity objScrap)
        {
            DataTable dt = new DataTable();
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD_MON-YYYY') TC_WARANTY_PERIOD,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID ";
                strQry += " AND TC_STATUS=3 AND TC_CURRENT_LOCATION<>3 AND TC_CODE = :TCCODE";
                oleDbCommand.Parameters.AddWithValue("TCCODE", objScrap.sTcCode);

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objScrap.sTcId = dt.Rows[0]["TC_ID"].ToString();
                    objScrap.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objScrap.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objScrap.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objScrap.sManfDate = dt.Rows[0]["TC_MANF_DATE"].ToString();
                    objScrap.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objScrap.sPurchaseDate = dt.Rows[0]["TC_PURCHASE_DATE"].ToString();
                    objScrap.sWarrantyPeriod = dt.Rows[0]["TC_WARANTY_PERIOD"].ToString();
                    objScrap.sSupplierName = dt.Rows[0]["TS_NAME"].ToString();
                }
                return objScrap;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultTCDetails");
                return objScrap;
            }
            finally
            {
                
            }
        }

        public clsScrap GetScrapMasterDetails(clsScrap objScrap)
        {
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                strQry = "SELECT ST_OM_NO,TO_CHAR(ST_OM_DATE,'DD/MM/YYYY') ST_OM_DATE,ST_REMARKS,SO_AMOUNT,SO_DISPOSAL_DESC,SO_SEND_TO,ST_QTY FROM TBLSCRAPTC,TBLSCRAPOBJECT ";
                strQry += " WHERE ST_ID=SO_ST_ID AND SO_ID='" + objScrap.sScrapDetailsId + "'";
                oleDbCommand.Parameters.AddWithValue("SOID", objScrap.sScrapDetailsId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count > 0)
                {
                    objScrap.sWorkOrderNo = Convert.ToString(dt.Rows[0]["ST_OM_NO"]);
                    objScrap.sWorkOrderDate = Convert.ToString(dt.Rows[0]["ST_OM_DATE"]);
                    objScrap.sRemarks = Convert.ToString(dt.Rows[0]["ST_REMARKS"]);

                    //objScrap.sOMNo = Convert.ToString(dt.Rows[0]["SO_OM_NO"]);
                    objScrap.sAmount = Convert.ToString(dt.Rows[0]["SO_AMOUNT"]);
                    objScrap.sDisposeDesc = Convert.ToString(dt.Rows[0]["SO_DISPOSAL_DESC"]);
                    objScrap.sSendTo = Convert.ToString(dt.Rows[0]["SO_SEND_TO"]);
                    objScrap.sDTrCount = Convert.ToInt32(dt.Rows[0]["ST_QTY"]);
                }
                return objScrap;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetScrapMasterDetails");
                return objScrap;
            }
            finally
            {
                
            }
        }

        public DataTable LoadScrapGrid(clsScrap objScrap)
        {
            DataTable dt = new DataTable();
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT TC_ID,TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,SO_ID,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST,TBLSCRAPOBJECT,TBLSCRAPTC WHERE TC_MAKE_ID=TM_ID AND SM_ID=TC_STORE_ID";
                strQry += "  AND SO_TC_CODE = TC_CODE AND ST_ID=SO_ST_ID AND SO_ID='" + objScrap.sScrapDetailsId + "'";
                oleDbCommand.Parameters.AddWithValue("", objScrap.sScrapDetailsId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadScrapGrid");
                return dt;
            }
            finally
            {
                
            }
        }

        //Decalre TC As Scrap

        public string[] DeclareTcScrap(string[] sTcCode, clsScrap objScrap)
        {
            string[] Arr = new string[2];
            bool bResult = false;
            try
            {
                
                string[] strDetailVal = sTcCode.ToArray();

                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    string strQry = string.Empty;
                    string sTestRes = string.Empty;
                    if (strDetailVal[i] != null)
                        sTestRes = strDetailVal[i].Split('~').GetValue(1).ToString();
                    if (sTestRes == "1")
                    {
                        //Update TC Status in TC MaSTER    7--> Declared as Scrap
                        strQry = "UPDATE TBLTCMASTER SET TC_STATUS='7' WHERE TC_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "'";
                        ObjCon.Execute(strQry);
                        bResult = true;
                    }
                    else
                        if (sTestRes == "2")
                    {
                        strQry = "UPDATE TBLTCMASTER SET TC_STATUS='3' WHERE TC_ID='" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "'";
                        ObjCon.Execute(strQry);
                        bResult = true;
                    }
                }

                if (bResult == true)
                {
                    Arr[0] = "Testing Done Successfully";
                    Arr[1] = "0";
                }
                else
                {
                    Arr[0] = "No Transformer Exists to Test";
                    Arr[1] = "2";
                }

                return Arr;

            }


            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DeclareTcScrap");
                return Arr;
            }
            finally
            {
                
            }
        }

        // Test Done For Scrap

        public DataTable LoadAlreadyDone(clsScrap objScrap)
        {
            DataTable dt = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT TC_ID,TO_CHAR(TC_CODE)TC_CODE,TC_SLNO,TM_NAME, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') TC_MANF_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TC_STATUS,";
                strQry += " (SELECT IND_DOC FROM TBLINSPECTIONDETAILS WHERE IND_RSD_ID= (SELECT MAX(RSD_ID) FROM TBLREPAIRSENTDETAILS WHERE RSD_TC_CODE=TC_CODE AND RSD_DELIVARY_DATE IS NOT NULL))IND_DOC,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(TC_WARANTY_PERIOD,'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TC_SUPPLIER_ID=TS_ID) TS_NAME ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST WHERE TC_MAKE_ID=TM_ID AND SM_ID=TC_STORE_ID";
                strQry += " AND TC_STATUS=7 ";
                strQry += " AND TC_LOCATION_ID LIKE '" + objScrap.sOfficeCode + "%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objScrap.sOfficeCode);
                if (objScrap.sTcId != null)
                {
                    strQry += " AND TC_ID IN (" + objScrap.sTcId + ")";
                }
                else
                {
                    strQry += " AND TC_CURRENT_LOCATION=1 AND TO_CHAR(TC_CODE) NOT IN (SELECT WO_DATA_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='30' AND WO_APPROVE_STATUS='0') ";
                }
                if (objScrap.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY=:Capacity ";
                    oleDbCommand.Parameters.AddWithValue("Capacity", objScrap.sCapacity);
                }
                if (objScrap.sMakeId != null)
                {
                    strQry += " AND TC_MAKE_ID=:MakeID";
                    oleDbCommand.Parameters.AddWithValue("MakeID", objScrap.sMakeId);
                }
                if (objScrap.sStoreId != null)
                {
                    strQry += " AND SM_ID=:StoreID";
                    oleDbCommand.Parameters.AddWithValue("StoreID", objScrap.sStoreId);
                }
                dt = ObjCon.getDataTable(strQry, oleDbCommand);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyDone");
                return dt;
            }
            finally
            {
                
            }
        }
    }
}
