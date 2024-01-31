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


namespace IIITS.DTLMS.BL
{
    public class clsTcMaster
    {
        string strFormCode = "clsTcMaster";
        string sWarranty = string.Empty;
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sTcId { get; set; }
        public string sTcMakeId { get; set; }
        public string sTcSlNo { get; set; }
        public string sTcCapacity { get; set; }
        public string sTcLifeSpan { get; set; }
        public string sManufacDate { get; set; }
        public string sPurchaseDate { get; set; }
        public string sPoNo { get; set; }
        public string sPrice { get; set; }
        public string sSupplierId { get; set; }
        public string sWarrentyPeriod { get; set; }
        public string sLastServiceDate { get; set; }
        public string sQuantity { get; set; }
        public string sPoId { get; set; }

        public string sTcCode { get; set; }
        public string sTcLiveFlag { get; set; }
        public int sStatus { get; set; }
        public string sTcStatus { get; set; }
        public string sCurrentLocation { get; set; }
        public string sLocationId { get; set; }
        public string sLastRepairerId { get; set; }
        public string sUpdatedEvent { get; set; }
        public string sUpdateEventId { get; set; }
        public string sCrBy { get; set; }
        public string sOfficeCode { get; set; }
        public string sStoreId { get; set; }

        public string sRating { get; set; }
        public string sStarRate { get; set; }
        public string sOilCapacity { get; set; }
        public string sWeight { get; set; }

        public string srepairOffCode { get; set; }
        public string sDtcCodes { get; set; }



        public string sColumnNames { get; set; }
        public string sColumnValues { get; set; }
        public string sTableNames { get; set; }

        public string sQryValues { get; set; }
        public string sDescription { get; set; }
        public string sParameterValues { get; set; }

        public string sWFDataId { get; set; }
        public string sXmlData { get; set; }

        public string sBOId { get; set; }

        public string sFormName { get; set; }

        public string sClientIP { get; set; }
        public string validaingFailurEntry { get; set; }
        public string Tankcapacity { get; set; }


        OleDbCommand oledbcommand;
        public string[] SaveUpdateTransformerDetails(clsTcMaster objTcMaster)
        {

            OleDbDataReader dr;
            string[] Arr = new string[2];
            string strQry = string.Empty;

            try
            {
                oledbcommand = new OleDbCommand();
                if (objTcMaster.sPurchaseDate != null && objTcMaster.sPurchaseDate != "")
                {
                    DateTime dPurchaseDate = DateTime.ParseExact(objTcMaster.sPurchaseDate, "dd/MM/yyyy", null);

                    if (objTcMaster.sWarrentyPeriod != "")
                    {
                        sWarranty = Convert.ToString(dPurchaseDate.AddYears(Convert.ToInt32(objTcMaster.sWarrentyPeriod)));
                        sWarranty = Convert.ToDateTime(sWarranty).ToString("dd/MM/yyyy");
                    }
                }

                //Get Store Id
                sStoreId = GetStoreId(objTcMaster.sOfficeCode);

                //CHECK Supplier ID exists or not
                //dr = ObjCon.Fetch("SELECT TS_ID FROM TBLTRANSSUPPLIER WHERE TS_ID='" + objTcMaster.sSupplierId + "'  AND TS_STATUS='A'");
                //if (!dr.Read())
                //{
                //    Arr[0] = "Enter Valid Supplier ID";
                //    Arr[1] = "2";
                //    dr.Close();
                //    return Arr;

                //}
                //dr.Close();


                if (objTcMaster.sTcId == "")
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TcCode", objTcMaster.sTcCode);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE=:TcCode", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer Code Already Exist";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TcSlNo", objTcMaster.sTcSlNo);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_SLNO=:TcSlNo", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer SlNo Already Exist";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;

                    }
                    dr.Close();

                    if (objTcMaster.sOfficeCode.Length >= 2)
                    {
                        objTcMaster.sOfficeCode = objTcMaster.sOfficeCode.Substring(0, 2);
                    }

                    objTcMaster.sTcId = Convert.ToString(ObjCon.Get_max_no("TC_ID", "TBLTCMASTER"));

                    strQry = "Insert into TBLTCMASTER (TC_ID,TC_CODE,TC_SLNO,TC_MAKE_ID,TC_CAPACITY,TC_MANF_DATE,TC_PURCHASE_DATE,TC_LIFE_SPAN, ";
                    strQry += " TC_SUPPLIER_ID,TC_PO_NO,TC_PRICE,TC_WARANTY_PERIOD,TC_LAST_SERVICE_DATE,TC_CURRENT_LOCATION, ";
                    strQry += "  TC_CRBY,TC_WARRENTY,TC_STORE_ID,TC_LOCATION_ID,TC_RATING,TC_STAR_RATE,TC_OIL_CAPACITY,TC_WEIGHT,TC_TANK_CAPACITY) VALUES ('" + objTcMaster.sTcId + "','" + objTcMaster.sTcCode + "','" + objTcMaster.sTcSlNo + "', ";
                    strQry += " '" + objTcMaster.sTcMakeId + "','" + objTcMaster.sTcCapacity + "',TO_DATE('" + objTcMaster.sManufacDate + "','DD/MM/YYYY'), ";
                    strQry += " TO_DATE('" + objTcMaster.sPurchaseDate + "','DD/MM/YYYY'), '" + objTcMaster.sTcLifeSpan + "'  , ";
                    strQry += " '" + objTcMaster.sSupplierId + "','" + objTcMaster.sPoNo + "','" + objTcMaster.sPrice + "', ";
                    strQry += " TO_DATE('" + sWarranty + "','DD/MM/YYYY'),TO_DATE('" + objTcMaster.sLastServiceDate + "','DD/MM/YYYY'), ";
                    strQry += " '" + objTcMaster.sCurrentLocation + "','" + objTcMaster.sCrBy + "','" + objTcMaster.sWarrentyPeriod + "',";
                    strQry += " '" + sStoreId + "','" + objTcMaster.sOfficeCode + "','" + objTcMaster.sRating + "','" + objTcMaster.sStarRate + "',";
                    strQry += " '" + objTcMaster.sOilCapacity + "','" + objTcMaster.sWeight + "','"+ objTcMaster.Tankcapacity + "') ";

                    ObjCon.Execute(strQry);




                    Arr[0] = "Transformer Details Saved Successfully";
                    Arr[1] = "0";
                    return Arr;

                }
                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tccode1", objTcMaster.sTcCode);
                    oledbcommand.Parameters.AddWithValue("tcid1", objTcMaster.sTcId);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE=:tccode1 and TC_ID<>:tcid1", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer Code Already Exist";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tcslno1", objTcMaster.sTcSlNo);
                    oledbcommand.Parameters.AddWithValue("tcId1", objTcMaster.sTcId);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where  TC_SLNO=:tcslno1 and TC_ID<>:tcId1", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Transformer SlNo Already Exist";
                        Arr[1] = "2";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    #region
                    /*
                     * done the changes on 28-03-2023
                     * Removed TC_CAPACITY='" + objTcMaster.sTcCapacity + "' becouse of unnecessary updating 
                     */
                    #endregion
                    strQry = "UPDATE TBLTCMASTER SET TC_MAKE_ID='" + objTcMaster.sTcMakeId + "',";
                    strQry += " TC_MANF_DATE= TO_DATE('" + objTcMaster.sManufacDate + "','DD/MM/YYYY'),TC_PURCHASE_DATE= TO_DATE('" + objTcMaster.sPurchaseDate + "','DD/MM/YYYY'), ";
                    strQry += " TC_LIFE_SPAN='"+ objTcMaster.sTcLifeSpan +"', TC_SUPPLIER_ID='" + objTcMaster.sSupplierId + "',TC_PO_NO='" + objTcMaster.sPoNo + "', ";
                    strQry += " TC_PRICE='" + objTcMaster.sPrice + "',TC_WARANTY_PERIOD= TO_DATE('" + sWarranty + "','DD/MM/YYYY'),TC_WARRENTY='"+ objTcMaster.sWarrentyPeriod  +"',";
                    if (sLastServiceDate.Length > 0)
                    {
                        strQry += "TC_LAST_SERVICE_DATE=TO_DATE('" + objTcMaster.sLastServiceDate + "','DD/MM/YYYY'),";
                    }

                    strQry += "TC_CURRENT_LOCATION='" + objTcMaster.sCurrentLocation + "',TC_STORE_ID='" + sStoreId + "',";
                    strQry += " TC_SLNO='" + objTcMaster.sTcSlNo + "',TC_RATING='" + objTcMaster.sRating + "',TC_STAR_RATE='" + objTcMaster.sStarRate + "',";
                    strQry += " TC_UPDATED_ON = SYSDATE , TC_UPDATED_BY = '"+objTcMaster.sCrBy+"' , ";
                    strQry += " TC_OIL_CAPACITY='" + objTcMaster.sOilCapacity + "',TC_WEIGHT='" + objTcMaster.sWeight + "',TC_TANK_CAPACITY='"
                        + objTcMaster.Tankcapacity + "' WHERE TC_ID= '" + objTcMaster.sTcId + "'";

                    ObjCon.Execute(strQry);
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("SmId9", sStoreId);
                    strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID = :SmId9";
                    string sMMsStoreId = ObjCon.get_value(strQry, oledbcommand);

                    if (sMMsStoreId == "" || sMMsStoreId == null)
                    {
                        sMMsStoreId = "0";
                    }

                    if (objTcMaster.sTcLifeSpan == null || objTcMaster.sTcLifeSpan == "")
                    {
                        objTcMaster.sTcLifeSpan = "0";
                    }
                    if (objTcMaster.sPrice == null || objTcMaster.sPrice == "")
                    {
                        objTcMaster.sPrice = "0";
                    }
                    if (objTcMaster.sWarrentyPeriod == null || objTcMaster.sWarrentyPeriod == "")
                    {
                        objTcMaster.sWarrentyPeriod = "0";
                    }
                    if (objTcMaster.sRating == null || objTcMaster.sRating == "")
                    {
                        objTcMaster.sRating = "0";
                    }
                    if (objTcMaster.sStarRate == null || objTcMaster.sStarRate == "")
                    {
                        objTcMaster.sStarRate = "0";
                    }
                    if (objTcMaster.sSupplierId == null || objTcMaster.sSupplierId == "")
                    {
                        objTcMaster.sSupplierId = "0";
                    }
                    #region
                    /*
                     * done the changes on 28-03-2023
                     * Removed TC_CAPACITY='" + objTcMaster.sTcCapacity + "' becouse of unnecessary updating 
                     */
                    #endregion
                    strQry = "UPDATE TBLTCMASTER SET TC_MAKE_ID='" + objTcMaster.sTcMakeId + "',";
                    strQry += "   TC_SUPPLIER_ID='" + objTcMaster.sSupplierId + "',TC_PO_NO='" + objTcMaster.sPoNo + "', ";
                    strQry += " TC_PRICE='" + objTcMaster.sPrice + "'  , ";


                    strQry += "TC_CURRENT_LOCATION='" + objTcMaster.sCurrentLocation + "',TC_STORE_ID='" + sMMsStoreId + "',";
                    strQry += " TC_SLNO='" + objTcMaster.sTcSlNo + "',TC_RATING='" + objTcMaster.sRating + "',TC_STAR_RATE='" + objTcMaster.sStarRate + "',";
                    strQry += " TC_OIL_CAPACITY='" + objTcMaster.sOilCapacity + "',TC_WEIGHT='" + objTcMaster.sWeight + "' WHERE TC_CODE= '" + objTcMaster.sTcCode + "'";

                    DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                    objWCF.SaveTcDetails(strQry);

                    Arr[0] = "Transformer Details Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateTransformerDetails");
                return Arr;

            }
            finally
            {

            }
        }

        public DataTable GetTCDetailsForSearch(clsTcMaster objtcmaster)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("LocationId", objtcmaster.sOfficeCode);
                //strQry = "SELECT (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) AS TC_MAKE_ID,TC_ID,TO_CHAR(TC_CODE) TC_CODE,";
                //strQry += " TC_SLNO,TC_MAKE_ID,TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,TO_CHAR(TC_LIFE_SPAN) AS TC_LIFE_SPAN FROM TBLTCMASTER ";
                //strQry += "  WHERE TC_LOCATION_ID LIKE :LocationId||'%'";

                strQry = " SELECT TM_NAME,TC_ID,TO_CHAR(TC_CODE) TC_CODE, TC_SLNO,TC_MAKE_ID,TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,TO_CHAR(TC_LIFE_SPAN) AS TC_LIFE_SPAN ";
                strQry += " FROM TBLTCMASTER FULL JOIN TBLTRANSMAKES ON TM_ID=TC_MAKE_ID WHERE TC_LOCATION_ID LIKE :LocationId||'%' ";

                if(objtcmaster.sTcCode != null)
                {
                    oledbcommand.Parameters.AddWithValue("tcCode", objtcmaster.sTcCode);
                    strQry += " AND TC_CODE=:tcCode";

                }
                if (objtcmaster.sTcMakeId != null)
                {
                    oledbcommand.Parameters.AddWithValue("TcMake", objtcmaster.sTcMakeId);
                    strQry += " AND TM_NAME=:TcMake";
                }

                //if (objtcmaster.sTcCapacity != null)
                //{
                //    oledbcommand.Parameters.AddWithValue("TcCapacity", objtcmaster.sTcCapacity);
                //    strQry += " AND TC_CAPACITY=:TcCapacity";
                //}
                if (objtcmaster.sTcSlNo != null)
                {
                    oledbcommand.Parameters.AddWithValue("TcSlNo", objtcmaster.sTcSlNo.ToUpper());
                    strQry += " AND Upper(TC_SLNO)=:TcSlNo";
                }

                strQry += "  ORDER BY TC_ID DESC";
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateTransformerDetails");
                return dt;
            }
        }

        public string GetDTRCount(clsTcMaster objTcMaster)
        {
            string strQry;
            string sDtrCount;
            sDtrCount   = strQry = string.Empty;
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("sOfficeCode", objTcMaster.sOfficeCode);
                strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_LOCATION_ID LIKE  :sOfficeCode || '%'  ";

                sDtrCount = ObjCon.get_value(strQry, oledbcommand);
                return sDtrCount;

            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateTransformerDetails");
                return sDtrCount;
            }
        }

        public string[] SaveTCDetails(string[] sTcDetails, clsTcMaster  objTcMaster)
        {

            string[] Arr = new string[2];
            string strQry = string.Empty;
            bool bResult = false;
            OleDbDataReader dr;
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("TsId", objTcMaster.sSupplierId);
                OleDbDataReader drChk = ObjCon.Fetch("select TS_ID from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND  TS_ID=:TsId", oledbcommand);
                if (!drChk.Read())
                {
                    Arr[0] = "Enter a Valid Supplier ID";
                    Arr[1] = "2";
                    drChk.Close();
                    return Arr;
                }
                drChk.Close();
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("PoNo",  objTcMaster.sPoNo );
                drChk = ObjCon.Fetch("SELECT * FROM TBLPOMASTER WHERE PO_NO=:PoNo", oledbcommand);
                if (!drChk.Read())
                {
                    Arr[0] = "Enter a Valid Purchase Order Number";
                    Arr[1] = "2";
                    drChk.Close();
                    return Arr;
                }
                drChk.Close();

                oledbcommand = new OleDbCommand();

                //Get Store Id
                sStoreId = GetStoreId(objTcMaster.sOfficeCode);
                string[] strDetailVal = sTcDetails.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    oledbcommand.Parameters.AddWithValue("TcCode", strDetailVal[i].Split('~').GetValue(0).ToString());
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE=:TcCode", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer Code " + strDetailVal[i].Split('~').GetValue(0).ToString() + "  Already Exist";
                        Arr[1] = "2";

                        return Arr;
                        break;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TcSlNo", strDetailVal[i].Split('~').GetValue(1).ToString());
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_SLNO=:TcSlNo", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer SlNo  " + strDetailVal[i].Split('~').GetValue(1).ToString() + "  Already Exist";
                        Arr[1] = "2";

                        return Arr;
                        break;

                    }
                    dr.Close();
                }
                ObjCon.BeginTrans();
                //Insert to TBLTCRECIEPT Table
                long sRecieptID = ObjCon.Get_max_no("TCR_ID", "TBLTCRECIEPT");
                strQry = "INSERT INTO TBLTCRECIEPT (TCR_ID,TCR_PO_NO,TCR_PURCHASE_DATE,TCR_QUANTITY,TCR_SUPPLIER_ID,TCR_CRBY) VALUES (";
                strQry += " '" + sRecieptID + "','" + objTcMaster.sPoId + "',TO_DATE('" + objTcMaster.sPurchaseDate + "','DD/MM/YYYY'),'"+ objTcMaster.sQuantity  +"',";
                strQry += " '"+ objTcMaster.sSupplierId  +"','"+ objTcMaster.sCrBy +"')";
                ObjCon.Execute(strQry);

                DateTime dPurchaseDate = DateTime.ParseExact(objTcMaster.sPurchaseDate,"dd/MM/yyyy",null);

                //string[] strDetailVal = sTcDetails.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tccode1", strDetailVal[i].Split('~').GetValue(0).ToString());
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE=:tccode1", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer Code " + strDetailVal[i].Split('~').GetValue(0).ToString() + "  Already Exist";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tcslno1", strDetailVal[i].Split('~').GetValue(1).ToString());
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_SLNO=:tcslno1", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Transformer SlNo  " + strDetailVal[i].Split('~').GetValue(1).ToString() + "  Already Exist";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;

                    }
                    dr.Close();
                    objTcMaster.sTcId = ObjCon.Get_max_no("TC_ID", "TBLTCMASTER").ToString();


                    if (objTcMaster.sOfficeCode.Length >= 2)
                    {
                        objTcMaster.sOfficeCode = objTcMaster.sOfficeCode.Substring(0, 2);
                    }

                    string sWarranty = Convert.ToString(dPurchaseDate.AddYears(Convert.ToInt32(strDetailVal[i].Split('~').GetValue(6).ToString())));
                    sWarranty = Convert.ToDateTime(sWarranty).ToString("dd/MM/yyyy");

                    //GENERATED ALWAYS AS (ADD_MONTHS(ENTERDATE,IDS))

                    strQry = "INSERT INTO TBLTCMASTER (TC_ID,TC_CODE,TC_SLNO,TC_MAKE_ID,TC_CAPACITY,TC_MANF_DATE,TC_PURCHASE_DATE,TC_LIFE_SPAN, ";
                    strQry += " TC_SUPPLIER_ID,TC_PO_NO,TC_WARANTY_PERIOD,TC_WARRENTY,TC_CURRENT_LOCATION, ";
                    strQry += " TC_CRBY,TC_STORE_ID,TC_LOCATION_ID,TC_TCR_ID,TC_OIL_CAPACITY,TC_WEIGHT) VALUES ('" + objTcMaster.sTcId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString() + "','" + strDetailVal[i].Split('~').GetValue(1).ToString() + "', ";
                    strQry += " '" + strDetailVal[i].Split('~').GetValue(2).ToString() + "','" + strDetailVal[i].Split('~').GetValue(3).ToString() + "',TO_DATE('" + strDetailVal[i].Split('~').GetValue(4).ToString() + "','DD/MM/YYYY'), ";
                    strQry += " TO_DATE('" + objTcMaster.sPurchaseDate + "','DD/MM/YYYY'), '" + strDetailVal[i].Split('~').GetValue(5).ToString() + "'  , ";
                    strQry += " '" + objTcMaster.sSupplierId + "','" + objTcMaster.sPoNo + "', ";
                    strQry += " TO_DATE('" + sWarranty + "','DD/MM/YYYY'), ";
                    strQry += " '" + strDetailVal[i].Split('~').GetValue(6).ToString() + "','1','" + objTcMaster.sCrBy + "','"+ sStoreId  +"',";
                    strQry += " '" + objTcMaster.sOfficeCode + "','" + sRecieptID + "','" + strDetailVal[i].Split('~').GetValue(7).ToString() + "','" + strDetailVal[i].Split('~').GetValue(8).ToString() + "') ";
                    ObjCon.Execute(strQry);


                    bResult = true;

                }
                ObjCon.CommitTrans();

                if (bResult == true)
                {
                    Arr[0] = "Transformers Details Saved Successfully";
                    Arr[1] = "0";
                }
                else
                {
                    Arr[0] = "No Transformer Exists to Save";
                    Arr[1] = "2";
                }
                return Arr;

            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveTCDetails");
                return Arr;
            }
            finally
            {

            }
        }

        public DataTable LoadTcMaster(clsTcMaster objTc)
        {

            string strQry = string.Empty;

            DataTable dtTcDetails = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("LocationId", objTc.sOfficeCode);
                //strQry = "SELECT (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) AS TC_MAKE_ID,TC_ID,TO_CHAR(TC_CODE) TC_CODE,";
                //strQry += " TC_SLNO,TC_MAKE_ID,TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,TO_CHAR(TC_LIFE_SPAN) AS TC_LIFE_SPAN FROM TBLTCMASTER ";
                //strQry += "  WHERE TC_LOCATION_ID LIKE :LocationId||'%'";

                strQry = " SELECT TM_NAME,TC_ID,TO_CHAR(TC_CODE) TC_CODE, TC_SLNO,TC_MAKE_ID,TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,TO_CHAR(TC_LIFE_SPAN) AS TC_LIFE_SPAN ";
                strQry += " FROM TBLTCMASTER FULL JOIN TBLTRANSMAKES ON TM_ID=TC_MAKE_ID WHERE TC_LOCATION_ID LIKE :LocationId||'%' ";

                    if (objTc.sTcMakeId != null)
                {
                    oledbcommand.Parameters.AddWithValue("TcMakeId", objTc.sTcMakeId);
                    strQry += " AND TC_MAKE_ID=:TcMakeId";
                }
                if (objTc.sTcCapacity != null)
                {
                    oledbcommand.Parameters.AddWithValue("TcCapacity", objTc.sTcCapacity);
                    strQry += " AND TC_CAPACITY=:TcCapacity";
                }
                strQry += " AND ROWNUM < 1000 ORDER BY TC_ID DESC";
                dtTcDetails = ObjCon.getDataTable(strQry, oledbcommand);

                return dtTcDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcMaster");
                return dtTcDetails;
            }
            finally
            {

            }
        }


        public clsTcMaster GetTCDetails(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                DataTable dtStoreDetails = new DataTable();

                oledbcommand.Parameters.AddWithValue("Tcid", objTcMaster.sTcId);
                strQry = "SELECT TC_ID,TC_CODE,TC_SLNO,TC_MAKE_ID,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(TC_MANF_DATE,'dd/MM/yyyy')TC_MANF_DATE,";
                strQry += " TO_CHAR(TC_PURCHASE_DATE,'dd/MM/yyyy')TC_PURCHASE_DATE,TO_CHAR(TC_LIFE_SPAN) AS TC_LIFE_SPAN,TC_SUPPLIER_ID,TC_PO_NO,TC_PRICE,";
                strQry += " TO_CHAR(TC_WARANTY_PERIOD,'dd/MM/yyyy')TC_WARANTY_PERIOD,TC_WARRENTY,TO_CHAR(TC_LAST_SERVICE_DATE,'dd/MM/yyyy')TC_LAST_SERVICE_DATE,";
                strQry += "  (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_ID = TC_STATUS AND  MD_TYPE = 'TCT' ) AS  TC_STATUS,TC_CURRENT_LOCATION,TC_LAST_REPAIRER_ID,TC_RATING,TC_STAR_RATE,TC_OIL_CAPACITY,  ";
                strQry += "TC_WEIGHT,TC_TANK_CAPACITY FROM TBLTCMASTER WHERE TC_ID =:Tcid";
                dtStoreDetails = ObjCon.getDataTable(strQry, oledbcommand);

                if (dtStoreDetails.Rows.Count > 0)
                {

                    objTcMaster.sTcId = dtStoreDetails.Rows[0]["TC_ID"].ToString();
                    objTcMaster.sTcCode = dtStoreDetails.Rows[0]["TC_CODE"].ToString();
                    objTcMaster.sTcSlNo = dtStoreDetails.Rows[0]["TC_SLNO"].ToString();
                    objTcMaster.sTcMakeId = dtStoreDetails.Rows[0]["TC_MAKE_ID"].ToString();

                    objTcMaster.sTcCapacity = dtStoreDetails.Rows[0]["TC_CAPACITY"].ToString();
                    objTcMaster.sTcLifeSpan = dtStoreDetails.Rows[0]["TC_LIFE_SPAN"].ToString();
                    objTcMaster.sManufacDate = dtStoreDetails.Rows[0]["TC_MANF_DATE"].ToString();
                    objTcMaster.sPurchaseDate = dtStoreDetails.Rows[0]["TC_PURCHASE_DATE"].ToString();

                    objTcMaster.sPoNo = dtStoreDetails.Rows[0]["TC_PO_NO"].ToString();
                    objTcMaster.sPrice = dtStoreDetails.Rows[0]["TC_PRICE"].ToString();
                    objTcMaster.sSupplierId = dtStoreDetails.Rows[0]["TC_SUPPLIER_ID"].ToString();
                    objTcMaster.sWarrentyPeriod = dtStoreDetails.Rows[0]["TC_WARRENTY"].ToString();

                    objTcMaster.sLastServiceDate = dtStoreDetails.Rows[0]["TC_LAST_SERVICE_DATE"].ToString();

                    objTcMaster.sCurrentLocation = dtStoreDetails.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    objTcMaster.sLastRepairerId =dtStoreDetails.Rows[0]["TC_LAST_REPAIRER_ID"].ToString();

                    objTcMaster.sRating = Convert.ToString(dtStoreDetails.Rows[0]["TC_RATING"]);
                    objTcMaster.sStarRate = Convert.ToString(dtStoreDetails.Rows[0]["TC_STAR_RATE"]);
                    objTcMaster.sOilCapacity = Convert.ToString(dtStoreDetails.Rows[0]["TC_OIL_CAPACITY"]);
                    objTcMaster.sWeight = Convert.ToString(dtStoreDetails.Rows[0]["TC_WEIGHT"]);
                    objTcMaster.sTcStatus = Convert.ToString(dtStoreDetails.Rows[0]["TC_STATUS"]);
                    objTcMaster.Tankcapacity = Convert.ToString(dtStoreDetails.Rows[0]["TC_TANK_CAPACITY"]);



                }
                // added by santhosh on  07-12-2022 -- start
                string WO_RECORD_ID = string.Empty;
                string strqry1 = string.Empty;
                strqry1 = "SELECT DT_CODE from TBLDTCMAST WHERE  DT_TC_ID ='" + objTcMaster.sTcCode + "'";
                string DTCCode = ObjCon.get_value(strqry1);

                if (objTcMaster.sTcCode != "" || objTcMaster.sTcCode != null)
                {
                    strqry1 = "SELECT TC_STATUS from TBLTCMASTER where TC_CODE = '" + objTcMaster.sTcCode + "'";
                    string TC_STATUS = ObjCon.get_value(strqry1);

                    if (TC_STATUS == "3")
                    {
                        objTcMaster.validaingFailurEntry = "Desable";
                    }
                    else
                    {
                        strqry1 = "SELECT WO_RECORD_ID from TBLWORKFLOWOBJECTS where WO_DATA_ID = '" + DTCCode + "' and WO_BO_ID = '9' and WO_RECORD_ID< 0";
                        WO_RECORD_ID = ObjCon.get_value(strqry1);

                        if (WO_RECORD_ID != "")
                        {
                            objTcMaster.validaingFailurEntry = "Desable";
                        }
                        else
                        {
                            objTcMaster.validaingFailurEntry = "Enable";
                        }

                    }

                }
                //--- End
                return objTcMaster;
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTCDetails");
                return objTcMaster;

            }
            finally
            {

            }

        }

        public string GetTransformerCount(string sOfficeCode)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                if (sOfficeCode.Length > 1)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }
                oledbcommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID IN (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_OFF_CODE=:OfficeCode)";
                return ObjCon.get_value(strQry, oledbcommand);
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

        public string GetStoreId(string sOfficeCode)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                if (sOfficeCode.Length > 1)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }
                oledbcommand.Parameters.AddWithValue("smoffcode", sOfficeCode);
                strQry = "SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_OFF_CODE=:smoffcode";
                return ObjCon.get_value(strQry, oledbcommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }

        }

        public bool CheckTransformerCodeExist(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                oledbcommand.Parameters.AddWithValue("tccode", objTcMaster.sTcCode);
                OleDbDataReader dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE=:tccode", oledbcommand);
                if (dr.Read())
                {
                    dr.Close();
                    return true;

                }
                dr.Close();
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetStoreId");
                return false;
            }
            finally
            {

            }
        }

        public object GetPoDetails(clsTcMaster objTc)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;

            DataTable dtTcDetails = new DataTable();
            try
            {

                oledbcommand.Parameters.AddWithValue("pono", objTc.sPoNo.ToUpper());
                strQry = "Select PO_ID,TO_CHAR(PO_DATE,'DD/MM/yyyy') PO_DATE,SUM(PB_QUANTITY) AS PB_QUANTITY,PO_SUPPLIER_ID,PO_NO ";
                strQry += " FROM TBLPOMASTER,TBLPOOBJECTS WHERE PB_PO_ID=PO_ID AND UPPER(PO_NO)=:pono GROUP BY PO_ID,PO_DATE,PO_SUPPLIER_ID,PO_NO ";
                dtTcDetails = ObjCon.getDataTable(strQry, oledbcommand);

                if (dtTcDetails.Rows.Count > 0)
                {
                    objTc.sPoId = dtTcDetails.Rows[0]["PO_ID"].ToString();
                    objTc.sPoNo = dtTcDetails.Rows[0]["PO_NO"].ToString();
                    objTc.sPurchaseDate = dtTcDetails.Rows[0]["PO_DATE"].ToString();
                    objTc.sQuantity = dtTcDetails.Rows[0]["PB_QUANTITY"].ToString();
                    objTc.sSupplierId = dtTcDetails.Rows[0]["PO_SUPPLIER_ID"].ToString();
                }
                return objTc;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetPoDetails");
                return dtTcDetails;
            }
            finally
            {

            }
        }

        public DataTable LoadTcGrid(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dtTcDetails = new DataTable();
            try
            {

                // strQry = "SELECT PO_ID,PO_NO,CAPACITY,REQ_QNTY,(NVL(REQ_QNTY,0) - NVL(SENT_QNT,0)) AS PENDINGCOUNT  FROM ";
                // strQry += " (SELECT PO_ID,PO_NO,PB_QUANTITY AS REQ_QNTY,TO_CHAR(PB_CAPACITY) AS CAPACITY FROM TBLPOMASTER, TBLPOOBJECTS WHERE PO_ID = PB_PO_ID GROUP BY PO_ID,PO_NO,PB_CAPACITY,PB_QUANTITY)A,";
                // strQry += " (SELECT TCR_PO_NO,TO_CHAR(PB_CAPACITY) PB_CAPACITY,SUM(TCR_QUANTITY) AS SENT_QNT FROM TBLPOMASTER,TBLTCRECIEPT,TBLPOOBJECTS,";
                // strQry += " TBLTCMASTER WHERE  PO_ID = PB_PO_ID AND TCR_PO_NO=PO_ID AND PB_PO_ID=TCR_PO_NO AND TC_TCR_ID = TCR_ID ";
                //strQry+= " AND PB_CAPACITY = TC_CAPACITY GROUP BY TCR_PO_NO,PB_CAPACITY)B WHERE A.PO_ID= B.TCR_PO_NO(+) ";
                //strQry += " AND A.CAPACITY = B.PB_CAPACITY(+) AND   (NVL(REQ_QNTY,0) - NVL(SENT_QNT,0))<>0 AND A.PO_ID='" + objTcMaster.sPoId + "'";

                oledbcommand.Parameters.AddWithValue("Pid", objTcMaster.sPoId);
                strQry = "  SELECT PO_ID,PO_NO,CAPACITY,REQ_QNTY,(NVL(REQ_QNTY,0) - NVL(SENT_QNT,0)) AS PENDINGCOUNT,MAKE  FROM   ";
                strQry += " (SELECT PO_ID,PO_NO,PB_QUANTITY AS REQ_QNTY,TO_CHAR(PB_CAPACITY) AS CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=PB_MAKE) AS MAKE FROM TBLPOMASTER, TBLPOOBJECTS WHERE ";
                strQry += " PO_ID = PB_PO_ID GROUP BY  PO_ID,PO_NO,PB_CAPACITY,PB_QUANTITY,PB_MAKE)A, (SELECT TCR_PO_NO,TO_CHAR(PB_CAPACITY) PB_CAPACITY,";
                strQry += " count(PB_CAPACITY) AS SENT_QNT FROM  TBLPOMASTER,TBLTCRECIEPT,TBLPOOBJECTS, TBLTCMASTER WHERE  PO_ID = PB_PO_ID AND ";
                strQry += " TCR_PO_NO=PO_ID AND PB_PO_ID=TCR_PO_NO AND TC_TCR_ID = TCR_ID AND PB_CAPACITY = TC_CAPACITY GROUP BY TCR_PO_NO,PB_CAPACITY)B ";
                strQry += " WHERE A.PO_ID= B.TCR_PO_NO(+) AND A.PO_ID=:Pid";
                strQry += " AND A.CAPACITY = B.PB_CAPACITY(+)";
                dtTcDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtTcDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcGrid");
                return dtTcDetails;
            }
            finally
            {

            }
        }


        public DataTable GetRepairerDetails(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {

                oledbcommand.Parameters.AddWithValue("offcd",  objTcMaster.srepairOffCode );
                String strQry = "SELECT TR_NAME,TR_ADDRESS,TR_MOBILE_NO,TR_EMAIL";
                strQry += " FROM TBLTRANSREPAIRER ";
                strQry += " WHERE TR_OFFICECODE Like :offcd||'%'";
                dtDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRepairerDetails");
                return dtDetails;
            }
            finally
            {

            }

        }

        public DataTable GetStoreDetails(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtStoreDetails = new DataTable();
            try
            {

                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("smoffcd", objTcMaster.srepairOffCode);
                strQry = "SELECT SM_NAME,SM_STORE_INCHARGE,SM_MOBILENO,SM_ADDRESS FROM TBLSTOREMAST WHERE SM_OFF_CODE like :smoffcd||'%'";
                dtStoreDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreDetails");
                return dtStoreDetails;

            }
            finally
            {

            }
        }

        public DataTable GetDtcDetails(clsTcMaster objtcMaster)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDtcDetails = new DataTable();
            try
            {

                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("dtcode", objtcMaster.sDtcCodes);
                strQry = " SELECT DT_CODE,DT_NAME,to_char(DT_LAST_SERVICE_DATE,'dd/MM/yyyy')DT_LAST_SERVICE_DATE,to_char(DT_LAST_INSP_DATE,'dd/MM/yyyy')DT_LAST_INSP_DATE from";
                strQry += " TBLDTCMAST WHERE DT_CODE like :dtcode||'%'";
                dtDtcDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtDtcDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDtcDetails");
                return dtDtcDetails;
            }
            finally
            {

            }
        }

        public DataTable GetFieldDetails(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("locationid", objTcMaster.srepairOffCode);
                oledbcommand.Parameters.AddWithValue("tccode", objTcMaster.sTcCode);
                String strQry = "select DT_CODE,DT_NAME from TBLDTCMAST,TBLTCMASTER,TBLTRANSDTCMAPPING where TM_DTC_ID=DT_CODE  and TM_TC_ID=TC_CODE ";
                strQry += " and TM_LIVE_FLAG=1 and TC_LOCATION_ID Like :locationid||'%' and TC_CODE=:tccode";
                dtDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFieldDetails");
                return dtDetails;
            }
            finally
            {

            }

        }

        public string SaveXmlData(clsTcMaster objTcMaster)
        {
            string sTcXmlData = string.Empty;

            try
            {

                string strQry = string.Empty;
                string strTemp = string.Empty;

                string sPrimaryKey = "{0}";

                objTcMaster.sColumnNames = "TC_ID,TC_CODE,TC_SLNO,TC_MAKE_ID,TC_CAPACITY,TC_MANF_DATE,TC_PURCHASE_DATE,";
                objTcMaster.sColumnNames += "TC_LIFE_SPAN,TC_SUPPLIER_ID,TC_PO_NO,TC_PRICE,TC_WARANTY_PERIOD,TC_LAST_SERVICE_DATE,";
                objTcMaster.sColumnNames += "TC_CURRENT_LOCATION,TC_CRBY,TC_WARRENTY,TC_STORE_ID,TC_LOCATION_ID,TC_RATING,TC_STAR_RATE,TC_OIL_CAPACITY,TC_WEIGHT";
                objTcMaster.sColumnValues = "" + objTcMaster.sTcId + "," + objTcMaster.sTcCode + "," + objTcMaster.sTcSlNo + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sTcMakeId + "," + objTcMaster.sTcCapacity + "," + objTcMaster.sManufacDate + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sPurchaseDate + "," + objTcMaster.sTcLifeSpan + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sSupplierId + "," + objTcMaster.sPoNo + "," + objTcMaster.sPrice + ",";
                objTcMaster.sColumnValues += "" + sWarranty + "," + objTcMaster.sLastServiceDate + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sCurrentLocation + "," + objTcMaster.sCrBy + "," + objTcMaster.sWarrentyPeriod + ",";
                objTcMaster.sColumnValues += "" + sStoreId + "," + objTcMaster.sOfficeCode + "," + objTcMaster.sRating + "," + objTcMaster.sStarRate + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sOilCapacity + "," + objTcMaster.sWeight + "";

                objTcMaster.sTableNames = "TBLTCMASTER";

                sTcXmlData = CreateXml(objTcMaster.sColumnNames, objTcMaster.sColumnValues, objTcMaster.sTableNames);
                return sTcXmlData;
            }

            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveXmlData");
                return sTcXmlData;
            }
            finally
            {

            }

        }

        //save data in tblefodata and workflowobjects
        public bool SaveWorkFlowData(clsTcMaster objTcMaster)
        {
            try
            {

                string strQry = string.Empty;
                objTcMaster.sWFDataId = Convert.ToString(ObjCon.Get_max_no("WFO_ID", "TBLWFODATA"));
                strQry = "INSERT INTO TBLWFODATA (WFO_ID,WFO_QUERY_VALUES,WFO_PARAMETER,WFO_DATA,WFO_CR_BY) VALUES (";
                strQry += " '" + objTcMaster.sWFDataId + "','" + objTcMaster.sQryValues + "','" + objTcMaster.sParameterValues + "','" + sXmlData + "',";
                strQry += " '" + objTcMaster.sCrBy + "')";
                ObjCon.Execute(strQry);

                if (objTcMaster.sFormName != null && objTcMaster.sFormName != "")
                {
                    //To get Business Object Id
                    objTcMaster.sBOId = ObjCon.get_value("SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)='" + objTcMaster.sFormName.Trim().ToUpper() + "'");
                }

                WorkFlowObjects(objTcMaster);

                string sWFlowId = Convert.ToString(ObjCon.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,WO_CLIENT_IP,";
                strQry += " WO_CR_BY,WO_APPROVED_BY,WO_APPROVE_STATUS,WO_RECORD_BY,WO_DESCRIPTION,WO_USER_COMMENT,WO_WFO_ID,WO_INITIAL_ID,WO_REF_OFFCODE)";
                strQry += " VALUES ('" + sWFlowId + "','" + objTcMaster.sBOId + "','" + objTcMaster.sTcId + "',0,";
                strQry += " '0','" + objTcMaster.sOfficeCode + "','" + objTcMaster.sClientIP + "','" + objTcMaster.sCrBy + "',";
                strQry += " '" + objTcMaster.sCrBy + "','1','WEB','" + objTcMaster.sDescription + "','" + objTcMaster.sDescription + "','" + objTcMaster.sWFDataId + "','" + sWFlowId + "','" + objTcMaster.sOfficeCode + "')";
                ObjCon.Execute(strQry);


                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveWorkFlowData");
                return false;
            }
            finally
            {

            }
        }

        //To Get CLient ip
        public void WorkFlowObjects(clsTcMaster objTcMaster)
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }



                objTcMaster.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
            }
            finally
            {

            }
        }

        //creating xml data for Wfo_data insert
        public string CreateXml(string strColumns, string strParameters, string strTableName)
        {
            try
            {
                DataTable dtXmlContent = new DataTable();

                DataTable dtnew = new DataTable();

                DataSet ds;
                if (strTableName.Contains(";"))
                {
                    ds = new DataSet(strTableName.Split(';').GetValue(0).ToString());
                }
                else
                {
                    ds = new DataSet(strTableName);
                }

                string[] strArrColumns = strColumns.Split(';');
                string[] strArrParameters = strParameters.Split(';');
                string[] strTableNames = strTableName.Split(';');

                int k = 0;
                //DataRow dRow = dt.NewRow();
                for (int i = 0; i < strArrColumns.Length; i++)
                {
                    DataTable dt = new DataTable();
                    DataRow dRow = dt.NewRow();
                    string[] strdtColumns = strArrColumns[i].Split(',');
                    string[] strdtParametres = strArrParameters[i].Split(',');
                    dt.TableName = strTableNames[i];
                    //DataRow dRow1 = dtnew.NewRow();
                    for (int j = 0; j < strdtColumns.Length; j++)
                    {
                        dt.Columns.Add(strdtColumns[j]);
                        if (k < strdtParametres.Length)
                        {
                            string strColumnName = strdtParametres[k];
                            dRow[dt.Columns[j]] = strdtParametres[k];
                            if (dt.Rows.Count == 0)
                            {
                                dt.Rows.Add(dRow);
                            }
                            dt.AcceptChanges();
                            //i--;
                        }
                        k++;

                    }

                    k = 0;

                    ds.Merge(dt);
                    dt.Clear();

                }
                return ds.GetXml();
                //dt.TableName = "Failure and Invoice";
                //////////////////////////////////////////////
                //dt.TableName = "TBLDTCFAILURE";

            }

            catch (Exception ex)
            {
                string strfailure = string.Empty;
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CreateXml");
                return strfailure;
                //return ds;
            }
            finally
            {

            }
        }

        public int CheckRICompletionOfDTR(clsTcMaster objTcMaster)
        {
            oledbcommand = new OleDbCommand();
            String strQry = string.Empty;
            int flag = 0;
            DataTable dtTcDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("Tccode", objTcMaster.sTcCode);
                strQry = "SELECT * from TBLTCMASTER WHERE TC_CODE = :Tccode AND TC_CURRENT_LOCATION = '1' AND TC_STATUS = '3'";
                dtTcDetails = ObjCon.getDataTable(strQry, oledbcommand);
                if (dtTcDetails.Rows.Count > 0)
                {
                    flag = 1;
                    return flag;
                }
                return flag;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CreateXml");
                return flag;
            }
        }
    }
}



