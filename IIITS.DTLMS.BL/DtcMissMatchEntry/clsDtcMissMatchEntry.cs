using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsDtcMissMatchEntry
    {
        public string sDtcCode { get; set; }
        public string sDtrCode { get; set; }
        public string sNewDTCCode { get; set; }
        public string sOfficeCode { get; set; }
        public string sLocType { get; set; }
        public string sNewDtrCode { get; set; }
        public string sNewOfficeCode { get; set; }
        public string sNewLocType { get; set; }
        public string sRemarks { get; set; }
        public string sCrBy { get; set; }
        public string sOldDtrCode { get; set; }
        public string sStoreId { get; set; }
        public string sDtrStatus { get; set; }

        public string sDTRCommissionDate { get; set; }


        string validationstatus = ConfigurationSettings.AppSettings["VaildationOfDtcDtr"].ToString();
        string validateCRforDTR = ConfigurationSettings.AppSettings["ValidateCRCompletion"].ToString();

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsDtcMissMatchEntry";
        public DataTable LoadDtcDetails(clsDtcMissMatchEntry objDtcMissEntry)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT DT_CODE,DT_NAME,DT_OM_SLNO,DT_TC_ID,TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_CURRENT_LOCATION FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND DT_CODE='" + objDtcMissEntry.sDtcCode + "'";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcDetails");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadDtrDetails(clsDtcMissMatchEntry objDtcMissEntry)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                

                strQry = "SELECT TC_CODE, TO_CHAR(TC_SLNO)TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_LOCATION_ID,TC_CURRENT_LOCATION, ";
                strQry += "CASE TC_CURRENT_LOCATION WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' END AS CURRENTLOCATION, ";
                strQry += " (SELECT  MD_NAME FROM TBLMASTERDATA WHERE TC_STATUS = MD_ID AND MD_TYPE = 'TCT') AS STATUS  ,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)OFFNAME";
                strQry += " FROM TBLTCMASTER WHERE TC_LIVE_FLAG = 1 AND  TC_CODE='" + objDtcMissEntry.sDtrCode + "'";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcDetails");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadDtrDetails1(clsDtcMissMatchEntry objDtcMissEntry)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                //strQry = "SELECT DF_REPLACE_FLAG FROM TBLDTCFAILURE WHERE DF_EQUIPMENT_ID='" + objDtcMissEntry.sDtrCode + "'";
                //string replace_status = ObjCon.get_value(strQry);

                //if (replace_status == "0")
                //{
                //    strQry = "SELECT * FROM TBLDTCFAILURE WHERE DF_EQUIPMENT_ID='" + objDtcMissEntry.sDtrCode + "'";
                //    dt = ObjCon.getDataTable(strQry);
                //    return dt;
                //}

                strQry = "SELECT TD_DF_ID FROM TBLTCDRAWN WHERE TD_TC_NO='" + objDtcMissEntry.sDtrCode + "'";
                string DFId = ObjCon.get_value(strQry);

                if (DFId == null || DFId == "")
                {
                    strQry = "SELECT TC_CODE, TO_CHAR(TC_SLNO)TC_SLNO,DT_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,'' AS DF_REPLACE_FLAG,TC_UPDATED_EVENT,";
                    strQry += "TC_LOCATION_ID, CASE TC_CURRENT_LOCATION WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' END AS ";
                    strQry += "TC_CURRENT_LOCATION, (SELECT  MD_NAME FROM TBLMASTERDATA WHERE TC_STATUS = MD_ID AND  MD_TYPE = 'TCT' ) AS STATUS  ";
                    strQry += " ,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)";
                    strQry += "OFFNAME FROM TBLTCMASTER,TBLDTCMAST WHERE TC_LIVE_FLAG = 1 AND TC_CODE=DT_TC_ID AND TC_CODE='" + objDtcMissEntry.sDtrCode + "'";
                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        strQry = "SELECT TC_CODE, TO_CHAR(TC_SLNO)TC_SLNO,'' AS DT_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,'' AS DF_REPLACE_FLAG,TC_UPDATED_EVENT,";
                        strQry += "TC_LOCATION_ID, CASE TC_CURRENT_LOCATION WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER'  END AS ";
                        strQry += "TC_CURRENT_LOCATION, (SELECT  MD_NAME FROM TBLMASTERDATA WHERE TC_STATUS = MD_ID AND MD_TYPE = 'TCT' )  AS STATUS ";
                        strQry += " ,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)";
                        strQry += "OFFNAME FROM TBLTCMASTER WHERE TC_LIVE_FLAG = 1 AND   TC_CODE='" + objDtcMissEntry.sDtrCode + "'";
                        dt = ObjCon.getDataTable(strQry);
                        return dt;
                    }
                }
                else
                {

                    strQry = "SELECT TO_CHAR(TC_SLNO)TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_LOCATION_ID,DT_CODE,TC_UPDATED_EVENT,DF_REPLACE_FLAG,CASE TC_CURRENT_LOCATION WHEN  ";
                    strQry += " 1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' END AS TC_CURRENT_LOCATION, (SELECT  MD_NAME FROM TBLMASTERDATA WHERE TC_STATUS = MD_ID AND MD_TYPE = 'TCT' )  AS STATUS ";
                    strQry += " ,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  FROM TBLDTCMAST,TBLTCMASTER,VIEW_ALL_OFFICES,TBLDTCFAILURE ";
                    strQry += " WHERE TC_CODE=DT_TC_ID AND TC_CODE='" + objDtcMissEntry.sDtrCode + "' AND DT_CODE=DF_DTC_CODE  and TC_LOCATION_ID=OFF_CODE AND TC_LIVE_FLAG = 1 ";
                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        strQry = "SELECT TC_CODE, TO_CHAR(TC_SLNO)TC_SLNO,'' as DT_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REPLACE_FLAG,TC_UPDATED_EVENT,TC_LOCATION_ID, ";
                        strQry += "CASE TC_CURRENT_LOCATION WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' END AS TC_CURRENT_LOCATION, ";
                        strQry += " (SELECT  MD_NAME FROM TBLMASTERDATA WHERE TC_STATUS = MD_ID AND MD_TYPE = 'TCT' )  AS STATUS,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)OFFNAME";
                        strQry += " FROM TBLTCMASTER,TBLTCDRAWN,TBLDTCFAILURE WHERE TC_CODE=TD_TC_NO AND DF_ID=TD_DF_ID AND TC_CODE='" + objDtcMissEntry.sDtrCode + "' AND TC_LIVE_FLAG = 1 ";
                        dt1 = ObjCon.getDataTable(strQry);
                        return dt1;
                    }


                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcDetails");
                return dt;
            }
            finally
            {
                
            }
        }

        public string[] SendTOStore(clsDtcMissMatchEntry objsend)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            string Desc = string.Empty;
            try
            {
                
                #region Validation of DTR codes 


                strQry = "SELECT TC_CAPACITY,TC_STATUS,TC_CURRENT_LOCATION,TC_LOCATION_ID,TC_STORE_ID FROM TBLTCMASTER WHERE TC_CODE='" + objsend.sDtrCode + "'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    string tcstatus = dt.Rows[0]["TC_STATUS"].ToString();
                    string tcCurentLocation = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    string tcLocationId = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    string storeId = dt.Rows[0]["TC_STORE_ID"].ToString();


                    // check whether tc is in interstore transfer 
                    if (tcCurentLocation == "4")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTR is in INTERSTORE transfer so cant allocate  ";
                        return Arr;
                    }
                    //tc shpuld be in the scrap 
                    if (tcstatus == "6" || tcstatus == "7")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTR has been sent to scrap";
                        return Arr;
                    }
                    //DTR shouldnt be failed 
                    strQry = "SELECT * from TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = '" + objsend.sDtrCode + "'  AND DF_REPLACE_FLAG = 0";
                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        if (validateCRforDTR == "TRUE")
                        {
                            strQry = "SELECT WO_NO FROM TBLDTCFAILURE,TBLWORKORDER WHERE DF_ID = WO_DF_ID and DF_EQUIPMENT_ID = '" + objsend.sDtrCode + "'";
                            string sWoNo = ObjCon.get_value(strQry);
                            Arr[0] = "2";
                            Arr[1] = "DTR CODE / CESC PLATE / UNIQUE ID has been failed and needs to complete the cycle  with workorder number " + sWoNo + " so cant allocate";
                            return Arr;
                        }
                            if (tcLocationId.Length == 4)
                            {
                                strQry = "SELECT WO_NO FROM TBLDTCFAILURE,TBLWORKORDER WHERE DF_ID = WO_DF_ID and DF_EQUIPMENT_ID = '" + objsend.sDtrCode + "'";
                                string sWoNo = ObjCon.get_value(strQry);
                                Arr[0] = "2";
                                Arr[1] = "DTR CODE / CESC PLATE / UNIQUE ID has been failed and needs to complete the cycle  with workorder number " + sWoNo + " so cant allocate";
                                return Arr;
                            }
                        
                    }
                    if (!(validationstatus == "FALSE"))
                    {
                        //DTR should  or moved from backend 
                        strQry = "SELECT DM_REMARKS  FROM TBLDTRMISMATCHENTRY WHERE DM_DTR_CODE ='" + objsend.sDtrCode + "' ";
                        dt = ObjCon.getDataTable(strQry);
                        if (dt.Rows.Count > 0)
                        {
                            string remarks = dt.Rows[0]["DM_REMARKS"].ToString();
                            Arr[0] = "2";
                            Arr[1] = " DTR has been moved from  Backend .. Remarks as follows " + remarks + "";
                            return Arr;
                        }
                    }

                    //DTR should be sent to repair centre 
                    strQry = "SELECT RSM_PO_NO  FROM TBLREPAIRSENTMASTER ,TBLREPAIRSENTDETAILS WHERE RSM_ID = RSD_RSM_ID AND RSD_DELIVARY_DATE IS NULL  AND RSD_TC_CODE ='" + objsend.sDtrCode + "' ";
                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        string pono = dt.Rows[0]["RSM_PO_NO"].ToString();
                        Arr[0] = "2";
                        Arr[1] = " DTR has been sent to repair centre  with Purchase  order number  " + pono + "";
                        return Arr;
                    }

                    //if the DTR is invoiced and not completed the CR then it cannot be moved .
                    strQry = "SELECT *  FROM TBLDTCFAILURE ,TBLTCDRAWN ,TBLTCMASTER WHERE DF_ID = TD_DF_ID AND  TD_TC_NO = TC_CODE AND TC_CURRENT_LOCATION = 2 AND  DF_REPLACE_FLAG = 0 AND TC_CODE = '" + objsend.sDtrCode + "' ";
                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        string dtcCode = Convert.ToString( dt.Rows[0]["DF_DTC_CODE"]);
                        Arr[0] = "2";
                        Arr[1] = "DTR has been invoice to " + dtcCode + " and needs to complete the CR ";
                        return Arr;
                    }



                    if (!(validationstatus == "FALSE"))
                    {
                        //DTR should not be allocated from backend 
                        strQry = "SELECT DME_REMARKS FROM TBLDTCMISMATCHENTRY WHERE DME_NEW_DTR_CODE  = '" + objsend.sDtrCode + "' ";
                        dt = ObjCon.getDataTable(strQry);
                        if (dt.Rows.Count > 0)
                        {
                            string remarks = dt.Rows[0]["DME_REMARKS"].ToString();
                            Arr[0] = "2";
                            Arr[1] = " DTR has been mapped from  Backend .. Remarks as follows " + remarks + "";
                            return Arr;
                        }
                    }
                }

                #endregion

                strQry = "SELECT SM_ID FROM TBLSTOREMAST WHERE SM_CODE='" + objsend.sStoreId + "'";
                string Store_id = ObjCon.get_value(strQry);

                long DTRSL_NO = ObjCon.Get_max_no("DM_SL_NO", "TBLDTRMISMATCHENTRY");

                strQry = " SELECT DT_CODE,DT_OM_SLNO,TC_CURRENT_LOCATION,TC_LOCATION_ID FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND TC_CODE='" + objsend.sDtrCode + "'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {

                    string tc_id = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    strQry = " SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE = '" + tc_id + "' ";
                    string From_Office_Name = ObjCon.get_value(strQry);
                    strQry = " SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE = '" + objsend.sStoreId + "' ";
                    string To_Office_Name = ObjCon.get_value(strQry);

                    strQry = "SELECT SM_ID FROM TBLSTOREMAST WHERE SM_CODE='" + objsend.sStoreId + "'";
                    string StoreId = ObjCon.get_value(strQry);

                    strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID='"+ StoreId + "'";
                    string sMmsStoreId = ObjCon.get_value(strQry);

                    if (tc_id.Length == 2)
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " STORE TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }
                    else
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " FIELD TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }

                    ObjCon.BeginTrans();
                    strQry = " INSERT INTO TBLDTRMISMATCHENTRY (DM_SL_NO,DM_ENTRY_DATE,DM_DTC_CODE,DM_DTR_CODE,DM_DTRCODE_OLD_LOCTYPE,DM_DTRCODE_OLD_REFCODE,DM_DTRCODE_NEW_LOCTYPE,DM_DTRCODE_NEW_REFCODE,DM_REMARKS,DM_CREATED_BY)";
                    strQry += " VALUES ('" + DTRSL_NO + "',SYSDATE,'" + dt.Rows[0]["DT_CODE"].ToString() + "','" + objsend.sDtrCode + "','" + dt.Rows[0]["TC_CURRENT_LOCATION"] + "',";
                    strQry += " '" + dt.Rows[0]["DT_OM_SLNO"].ToString() + "','1','" + objsend.sStoreId + "','" + objsend.sRemarks + "','" + objsend.sCrBy + "')";
                    ObjCon.Execute(strQry);


                    strQry = " UPDATE TBLDTCMAST SET DT_TC_ID='0' WHERE DT_CODE='" + dt.Rows[0]["DT_CODE"].ToString() + "'";
                    ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objsend.sCrBy + "',TM_UNMAP_REASON='" + objsend + "' WHERE TM_DTC_ID='" + dt.Rows[0]["DT_CODE"].ToString() + "' AND TM_LIVE_FLAG=1";
                    ObjCon.Execute(strQry);

                    strQry = "SELECT DF_ID FROM TBLDTCFAILURE WHERE DF_DTC_CODE='" + dt.Rows[0]["DT_CODE"].ToString() + "'";
                    string val = ObjCon.get_value(strQry);

                    long SL_NO = ObjCon.Get_max_no("UNA_SL_NO", "TBLUNALLOCATEDDTCS");

                    if (val == null || val == "")
                    {
                        strQry = "INSERT INTO TBLUNALLOCATEDDTCS (UNA_SL_NO,UNA_MISMATCHENTRY_SLNO,UNA_DTCCODE,UNA_IS_DTC_FAILURE,UNA_ENTRY_FROM) VALUES (";
                        strQry += " '" + SL_NO + "','" + DTRSL_NO + "','" + dt.Rows[0]["DT_CODE"].ToString() + "','NO','TBLDTRMISMATCHENTRY')";

                        ObjCon.Execute(strQry);

                    }
                    else
                    {
                        strQry = "INSERT INTO TBLUNALLOCATEDDTCS (UNA_SL_NO,UNA_MISMATCHENTRY_SLNO,UNA_DTCCODE,UNA_IS_DTC_FAILURE,UNA_FAILURE_ID,UNA_ENTRY_FROM) VALUES (";
                        strQry += " '" + SL_NO + "','" + DTRSL_NO + "','" + dt.Rows[0]["DT_CODE"].ToString() + "','YES','" + val + "','TBLDTRMISMATCHENTRY')";
                        ObjCon.Execute(strQry);
                    }

                    strQry = " UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION='1',TC_LOCATION_ID='" + objsend.sStoreId + "',TC_STATUS='" + objsend.sDtrStatus + "',TC_STORE_ID='" + StoreId + "' WHERE TC_CODE='" + objsend.sDtrCode + "'";
                    ObjCon.Execute(strQry);

                    strQry = " UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION='1',TC_LOCATION_ID='" + objsend.sStoreId + "',TC_STATUS='" + objsend.sDtrStatus + "',TC_STORE_ID='" + sMmsStoreId + "' WHERE TC_CODE='" + objsend.sDtrCode + "'";
                    DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                    objWCF.SaveTcDetails(strQry);

                    strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT max(DRT_ID)+1 FROM TBLDTRTRANSACTION),'" + objsend.sDtrCode + "',";
                    strQry += "'" + Store_id + "','1',SYSDATE,'','2','" + Desc + "','" + objsend.sDtrStatus + "',SYSDATE,'0','','')";
                    ObjCon.Execute(strQry);

                }
                else
                {
                    strQry = "SELECT TC_CAPACITY,TC_STATUS,TC_CURRENT_LOCATION,TC_LOCATION_ID FROM TBLTCMASTER WHERE TC_CODE='" + objsend.sDtrCode + "'";
                    dt = ObjCon.getDataTable(strQry);

                    string tc_id = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    strQry = " SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE = '" + tc_id + "' ";
                    string From_Office_Name = ObjCon.get_value(strQry);
                    strQry = " SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE = '" + objsend.sStoreId + "' ";
                    string To_Office_Name = ObjCon.get_value(strQry);

                    strQry = "SELECT SM_ID FROM TBLSTOREMAST WHERE SM_CODE='" + objsend.sStoreId + "'";
                    string StoreId = ObjCon.get_value(strQry);

                    strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID='" + StoreId + "'";
                    string sMmsStoreId = ObjCon.get_value(strQry);

                    if (tc_id.Length == 2)
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " STORE TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }
                    else
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " FIELD TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }

                    ObjCon.BeginTrans();
                    strQry = " INSERT INTO TBLDTRMISMATCHENTRY (DM_SL_NO,DM_ENTRY_DATE,DM_DTC_CODE,DM_DTR_CODE,DM_DTRCODE_OLD_LOCTYPE,DM_DTRCODE_OLD_REFCODE,DM_DTRCODE_NEW_LOCTYPE,DM_DTRCODE_NEW_REFCODE,DM_REMARKS,DM_CREATED_BY)";
                    strQry += " VALUES ('" + DTRSL_NO + "',SYSDATE,'','" + objsend.sDtrCode + "','" + dt.Rows[0]["TC_CURRENT_LOCATION"] + "',";
                    strQry += " '" + dt.Rows[0]["TC_LOCATION_ID"].ToString() + "','1','" + objsend.sStoreId + "','" + objsend.sRemarks + "','" + objsend.sCrBy + "')";
                    ObjCon.Execute(strQry);

                    strQry = " UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION='1',TC_LOCATION_ID='" + objsend.sStoreId + "',TC_STATUS='" + objsend.sDtrStatus + "',TC_STORE_ID='" + StoreId + "' WHERE TC_CODE='" + objsend.sDtrCode + "'";
                    ObjCon.Execute(strQry);
                    strQry = " UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION='1',TC_LOCATION_ID='" + objsend.sStoreId + "',TC_STATUS='" + objsend.sDtrStatus + "',TC_STORE_ID='" + sMmsStoreId + "' WHERE TC_CODE='" + objsend.sDtrCode + "'";
                    DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                    objWCF.SaveTcDetails(strQry);

                    strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT max(DRT_ID)+1 FROM TBLDTRTRANSACTION),'" + objsend.sDtrCode + "',";
                    strQry += "'" + Store_id + "','1',SYSDATE,'','2','" + Desc + "','" + objsend.sDtrStatus + "',SYSDATE,'0','','')";
                    ObjCon.Execute(strQry);
                }


                ObjCon.CommitTrans();
                Arr[0] = "1";
                Arr[1] = "TC SEND TO STORE SUCCESFULLY";
                return Arr;

            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "swapDetails");
                Arr[0] = "2";
                Arr[1] = "Error Occured";
                return Arr;
            }
            finally
            {
                
            }
        }

        public string[] swapDetails(clsDtcMissMatchEntry objswap)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            string strQry1 = string.Empty;
            DataTable dt = new DataTable();
            DataTable dtDtrDetails = new DataTable();
            DataTable dtDtcDetails = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                
                long sDTC_SL_NO = ObjCon.Get_max_no("DME_SL_NO", "TBLDTCMISMATCHENTRY");
                #region Validation of  DTC and DTR codes

                if (!(validationstatus == "FALSE"))
                {

                    //check whether the DTC or DTR has been mapped previsously in backend
                    strQry1 = "SELECT DME_DTC_CODE,DME_NEW_DTR_CODE,DME_REMARKS  FROM TBLDTCMISMATCHENTRY WHERE DME_DTC_CODE ";
                    strQry1 += " = '" + objswap.sDtcCode + "' OR DME_NEW_DTR_CODE = '" + objswap.sNewDtrCode + "'";
                    dt = ObjCon.getDataTable(strQry1);
                    if (dt.Rows.Count > 0)
                    {
                        string remarks = dt.Rows[0]["DME_REMARKS"].ToString();
                        Arr[0] = "2";
                        Arr[1] = "DTC or DTR has been allocated using Backend .. Remarks as follows " + remarks + "";
                        return Arr;
                    }
                }

                strQry = "SELECT * FROM TBLTCMASTER WHERE TC_CODE = '" + objswap.sDtrCode + "'";
                dt = ObjCon.getDataTable(strQry);
                if(dt.Rows.Count == 0)
                {
                    Arr[0] = "2";
                    Arr[1] = "DTR Code doesnt Exists";
                    return Arr;
                }

                strQry = "SELECT * FROM TBLDTCMAST WHERE DT_CODE ='" + objswap.sDtcCode + "'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count == 0)
                {
                    Arr[0] = "2";
                    Arr[1] = "DTC Code doesnt Exists";
                    return Arr;
                }

                // validate if DTC has not  completed the cycle
                strQry1 = "SELECT * from TBLDTCFAILURE WHERE DF_DTC_CODE = '" + objswap.sDtcCode + "'  AND DF_REPLACE_FLAG = 0";
                dt = ObjCon.getDataTable(strQry1);
                if (dt.Rows.Count > 0)
                {
                    strQry1 = "SELECT WO_NO FROM TBLDTCFAILURE,TBLWORKORDER WHERE DF_ID = WO_DF_ID and DF_DTC_CODE = '" + objswap.sDtcCode + "'";
                    string sWoNo = ObjCon.get_value(strQry1);
                    Arr[0] = "2";
                    Arr[1] = "DTC has been failed and needs to complete the cycle  with workorder number " + sWoNo + " so cant allocate";
                    return Arr;
                }


                //validate if DTR has failed and not completed the cycle 
                strQry1 = "SELECT * from TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = '" + objswap.sNewDtrCode + "'  AND DF_REPLACE_FLAG = 0";
                dt = ObjCon.getDataTable(strQry1);
                if (dt.Rows.Count > 0)
                {
                    strQry1 = "SELECT WO_NO FROM TBLDTCFAILURE,TBLWORKORDER WHERE DF_ID = WO_DF_ID and DF_EQUIPMENT_ID = '" + objswap.sNewDtrCode + "'";
                    string sWoNo = ObjCon.get_value(strQry1);
                    Arr[0] = "2";
                    Arr[1] = "DTR CODE / CESC PLATE / UNIQUE ID has been failed and needs to complete the cycle  with workorder number " + sWoNo + " so can't allocate";
                    return Arr;
                }

                //check if the TC is in Store or  Field or repairer with good condition
                strQry1 = "SELECT TC_STATUS ,TC_CURRENT_LOCATION ,TC_LOCATION_ID,TC_STORE_ID  FROM TBLTCMASTER WHERE TC_CODE = '" + objswap.sNewDtrCode + "'";
                dt = ObjCon.getDataTable(strQry1);
                if (dt.Rows.Count > 0)
                {
                    string tcstatus = dt.Rows[0]["TC_STATUS"].ToString();
                    string tcCurentLocation = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    string tcLocationId = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    string storeId = dt.Rows[0]["TC_STORE_ID"].ToString();

                    // if DTR has been failed (maybe if the status is empty )
                    if (tcstatus == "3")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTR has been failed";
                        return Arr;
                    }

                    //if tc is in inter store transfer 
                    if (tcstatus == "4")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTR has been scraped";
                        return Arr;
                    }

                    //if the DTR is in interstore transaction 
                    if (tcCurentLocation == "4")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTR is in INTERSTORE transfer so cant allocate  ";
                        return Arr;
                    }

                    //if any of the field has null value 
                    if (tcCurentLocation.Length == 0 || tcLocationId.Length == 0 || storeId.Length == 0)
                    {
                        Arr[0] = "2";
                        Arr[1] = "Store Id  Location Id / Current Location  is empty please contact backend team ";
                        return Arr;
                    }
                    //if location code and current location doesnt match 
                    if ((tcCurentLocation == "1" && tcLocationId.Length == 4) || (tcCurentLocation == "2" && tcLocationId.Length == 2))
                    {
                        Arr[0] = "2";
                        Arr[1] = "There may be some problem with current location and location id please contact backend team ";
                        return Arr;
                    }
                    // if tc has been send to repaircentre  
                    if (tcCurentLocation == "3")
                    {
                        strQry1 = "SELECT RSM_PO_NO FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER  WHERE RSD_DELIVARY_DATE IS ";
                        strQry1 += "NULL and RSD_RSM_ID = RSM_ID AND RSD_TC_CODE = '" + objswap.sNewDtrCode + "'";
                        string sPoNo = ObjCon.get_value(strQry1);
                        Arr[0] = "2";
                        Arr[1] = "DTR CODE / CESC PLATE / UNIQUE ID has been sent to repair centre with PO NO " + sPoNo + "";
                        return Arr;
                    }
                }

                #endregion

                //get the store id of the mapping dtr (if dtr is in other store)
                strQry = "SELECT SM_ID FROM TBLSTOREMAST WHERE SM_CODE =SUBSTR(" + objswap.sOfficeCode + ",0,2) ";
                objswap.sStoreId = ObjCon.get_value(strQry);


                #region NewDtc Notthere
                if (objswap.sNewDTCCode == null || objswap.sNewDTCCode == "")
                {


                    strQry1 = "SELECT DT_CODE,DT_NAME,DT_OM_SLNO,DT_TC_ID,TC_SLNO,TC_CAPACITY,TC_CURRENT_LOCATION FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND TC_CODE='" + objswap.sNewDtrCode + "'";
                    dt = ObjCon.getDataTable(strQry1);
                    if (dt.Rows.Count > 0)
                    {
                        long UNA_SL_NO = ObjCon.Get_max_no("UNA_SL_NO", "TBLUNALLOCATEDDTCS");
                        string sDTC_Code = dt.Rows[0]["DT_CODE"].ToString();

                        strQry = "select DF_ID FROM TBLDTCFAILURE WHERE DF_DTC_CODE='" + sDTC_Code + "'";
                        string sDfId = ObjCon.get_value(strQry);

                        ObjCon.BeginTrans();
                        if (sDfId == "")
                        {
                            strQry = "INSERT INTO TBLUNALLOCATEDDTCS (UNA_SL_NO,UNA_MISMATCHENTRY_SLNO,UNA_DTCCODE,UNA_ENTRY_FROM,UNA_IS_DTC_FAILURE)";
                            strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                            strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','NO')";
                            ObjCon.Execute(strQry);
                        }
                        else
                        {
                            strQry = "INSERT INTO TBLUNALLOCATEDDTCS (UNA_SL_NO,UNA_MISMATCHENTRY_SLNO,UNA_DTCCODE,UNA_ENTRY_FROM,UNA_IS_DTC_FAILURE,UNA_FAILURE_ID)";
                            strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                            strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','YES','" + sDfId + "')";
                            ObjCon.Execute(strQry);
                        }


                        strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF Dtc CODE BY DTCMISMATCHENTRY' WHERE TM_DTC_ID='" + sDTC_Code + "' AND TM_LIVE_FLAG=1";
                        ObjCon.Execute(strQry);

                        strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='0' WHERE DT_CODE='" + sDTC_Code + "'";
                        ObjCon.Execute(strQry);
                        ObjCon.CommitTrans();
                    }
                    ObjCon.BeginTrans();
                    strQry = "INSERT INTO TBLDTCMISMATCHENTRY (DME_SL_NO,DME_DTC_CODE,DME_EXISTING_DTR_CODE,DME_NEW_DTR_CODE,DME_NEWDTRCODE_OLDLOC_TYPE, ";
                    strQry += " DME_NEWDTRCODE_OLDLOC_REFNO,DME_ENTRY_DATE,DME_REMARKS,DME_CREATED_BY ) VALUES ('" + sDTC_SL_NO + "','" + objswap.sDtcCode + "',";
                    strQry += " '" + objswap.sDtrCode + "','" + objswap.sNewDtrCode + "','" + objswap.sLocType + "','" + objswap.sOfficeCode + "',SYSDATE,'" + objswap.sRemarks + "','" + objswap.sCrBy + "')";
                    ObjCon.Execute(strQry);

                    long sMaxNo = ObjCon.Get_max_no("UAD_SL_NO", "TBLUNALLOCATEDDTRS");

                    strQry = "INSERT INTO TBLUNALLOCATEDDTRS (UAD_SL_NO,UAD_SL_MISMATCHENTRY_SLNO,UAD_DTRCODE,UAD_ENTRY_FROM)";
                    strQry += " VALUES ('" + sMaxNo + "','" + sDTC_SL_NO + "',";
                    strQry += " '" + objswap.sDtrCode + "','DTCMISMATCHENTRY')";
                    ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objswap.sNewDtrCode + "' WHERE DT_CODE='" + objswap.sDtcCode + "'";
                    ObjCon.Execute(strQry);

                    strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + objswap.sNewDtrCode + "'";
                    string cap = ObjCon.get_value(strQry);

                    TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                    string result = objWCF1.SendCapacitytoTrms(cap, objswap.sDtcCode);

                    strQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + objswap.sOfficeCode + "',TC_CURRENT_LOCATION='2' ,TC_STATUS = 2,TC_STORE_ID= '"+objswap.sStoreId+"' WHERE TC_CODE='" + objswap.sNewDtrCode + "'";
                    ObjCon.Execute(strQry);

                    DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                    objWCF.SaveTcDetails(strQry);

                    //strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + objswap.sDtcCode + "'";
                    //string cap = ObjCon.get_value(strQry);

                    //TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                    //string result = objWCF1.SendCapacitytoTrms(cap, objswap.sDtcCode);


                    strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_DTC_ID='" + objswap.sDtcCode + "' AND TM_LIVE_FLAG=1";
                    ObjCon.Execute(strQry);

                    long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                    strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON) VALUES ('" + sMaxmapping + "',";
                    strQry += " to_date('"+objswap.sDTRCommissionDate+"','dd/MM/yyyy'),'" + objswap.sNewDtrCode + "','" + objswap.sDtcCode + "','1','" + objswap.sCrBy + "',SYSDATE)";
                    ObjCon.Execute(strQry);

                    strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_TC_ID='" + objswap.sDtrCode + "' AND TM_LIVE_FLAG=1";
                    ObjCon.Execute(strQry);

                    strQry = "INSERT INTO TBLDTCTRANSACTION VALUES((SELECT max(DCT_ID)+1 FROM TBLDTCTRANSACTION),'" + objswap.sDtcCode + "',";
                    strQry += "'" + objswap.sNewDtrCode + "','1',to_date('" + objswap.sDTRCommissionDate + "','dd/MM/yyyy'),'','1','NEW DTC COMMISSIONED (FROM BACKEND)',SYSDATE,'0','','')";
                    ObjCon.Execute(strQry);

                    strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT max(DRT_ID)+1 FROM TBLDTRTRANSACTION),'" + objswap.sNewDtrCode + "',";
                    strQry += "'" + objswap.sOfficeCode + "','2',to_date('" + objswap.sDTRCommissionDate + "','dd/MM/yyyy'),'','2','COMMISSIONED TO DTCCODE :" + objswap.sDtcCode + " (FROM BACKEND)','1',SYSDATE,'0','','')";
                    ObjCon.Execute(strQry);

                    Arr[0] = "1";
                    Arr[1] = "Allocate Succesfully";

                    ObjCon.CommitTrans();
                    return Arr;

                }
                #endregion
                #region NewDTc Given
                else
                {
                    strQry = "SELECT DT_CODE,DT_NAME,DT_OM_SLNO,DT_TC_ID,TC_SLNO,TC_CAPACITY,TC_CURRENT_LOCATION FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND TC_CODE='" + objswap.sNewDtrCode + "'";
                    dtDtcDetails = ObjCon.getDataTable(strQry);
                    if (dtDtcDetails.Rows.Count > 0)
                    {
                        strQry = "INSERT INTO TBLDTCMISMATCHENTRY (DME_SL_NO,DME_DTC_CODE,DME_EXISTING_DTR_CODE,DME_NEW_DTR_CODE,DME_NEWDTRCODE_OLDLOC_TYPE, ";
                        strQry += " DME_NEWDTRCODE_OLDLOC_REFNO,DME_ENTRY_DATE,DME_REMARKS,DME_CREATED_BY,DME_NEW_DTC_CODE,DME_EXIST_DTRCODE_NEWLOCTYPE,DME_EXIST_DTRCODE_NEWLOC_REFNO ) VALUES ('" + sDTC_SL_NO + "','" + objswap.sDtcCode + "',";
                        strQry += " '" + objswap.sDtrCode + "','" + objswap.sNewDtrCode + "','" + objswap.sLocType + "','" + objswap.sOfficeCode + "',SYSDATE,'" + objswap.sRemarks + "','" + objswap.sCrBy + "','" + objswap.sNewDTCCode + "',";
                        strQry += " '" + dtDtcDetails.Rows[0]["TC_CURRENT_LOCATION"] + "','" + dtDtcDetails.Rows[0]["DT_OM_SLNO"] + "' )";
                        ObjCon.Execute(strQry);
                    }
                    else
                    {
                        strQry = "SELECT TC_SLNO,TC_CAPACITY,TC_CURRENT_LOCATION FROM TBLTCMASTER WHERE  TC_CODE='" + objswap.sNewDtrCode + "'";
                        dt = ObjCon.getDataTable(strQry);
                    }

                    //strQry = "SELECT DT_CODE,DT_NAME,DT_OM_SLNO,DT_TC_ID,TC_SLNO,TC_CAPACITY,TC_CURRENT_LOCATION FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND TC_CODE='"+ objswap.sNewDtrCode +"'";
                    //dt = ObjCon.getDataTable(strQry);
                    strQry = "SELECT * FROM TBLDTCMISMATCHENTRY WHERE DME_DTC_CODE='" + objswap.sDtcCode + "' AND  DME_NEW_DTR_CODE='" + objswap.sNewDtrCode + "'";
                    strQry += " AND DME_NEW_DTC_CODE='" + objswap.sNewDTCCode + "' AND DME_EXISTING_DTR_CODE='" + objswap.sDtrCode + "' AND DME_SL_NO='" + sDTC_SL_NO + "'";
                    dt1 = ObjCon.getDataTable(strQry);

                    strQry = "SELECT DT_TC_ID FROM TBLDTCMAST WHERE DT_CODE='" + objswap.sNewDTCCode + "'";
                    string res = ObjCon.get_value(strQry);

                    if ((dt1.Rows.Count > 0 && res == objswap.sNewDtrCode) || (dt1.Rows.Count > 0 && res == objswap.sDtrCode))
                    {
                        strQry = "SELECT DT_TC_ID FROM TBLDTCMAST WHERE DT_CODE='" + objswap.sNewDTCCode + "'";
                        string sNewDTC_TC = ObjCon.get_value(strQry);

                        strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objswap.sNewDtrCode + "' WHERE DT_CODE='" + objswap.sDtcCode + "'";
                        ObjCon.Execute(strQry);

                        strQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + objswap.sOfficeCode + "',TC_CURRENT_LOCATION='2' WHERE TC_CODE='" + objswap.sNewDtrCode + "'";
                        ObjCon.Execute(strQry);

                        DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        objWCF.SaveTcDetails(strQry);

                        strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objswap.sDtrCode + "' WHERE DT_CODE='" + objswap.sNewDTCCode + "'";
                        ObjCon.Execute(strQry);

                        strQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + objswap.sOfficeCode + "',TC_CURRENT_LOCATION='2',TC_STATUS = 2, WHERE TC_CODE='" + objswap.sDtrCode + "'";
                        ObjCon.Execute(strQry);

                        //DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        objWCF.SaveTcDetails(strQry);

                        strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + objswap.sDtcCode + "'";
                        string cap = ObjCon.get_value(strQry);

                        TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                        string result = objWCF1.SendCapacitytoTrms(cap, objswap.sDtcCode);


                        strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_TC_ID='" + sNewDTC_TC + "' AND TM_LIVE_FLAG=1 ";
                        ObjCon.Execute(strQry);

                        strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_DTC_ID='" + objswap.sDtcCode + "' AND TM_LIVE_FLAG=1";
                        ObjCon.Execute(strQry);

                        long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                        strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON) VALUES ('" + sMaxmapping + "',";
                        strQry += "SYSDATE,'" + objswap.sNewDtrCode + "','" + objswap.sDtcCode + "','1','" + objswap.sCrBy + "',SYSDATE)";
                        ObjCon.Execute(strQry);

                        strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_TC_ID='" + objswap.sDtrCode + "' AND TM_LIVE_FLAG=1";
                        ObjCon.Execute(strQry);

                        long sMaxDtcmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                        strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON) VALUES ('" + sMaxDtcmapping + "',";
                        strQry += "SYSDATE,'" + objswap.sDtrCode + "','" + objswap.sNewDTCCode + "','1','" + objswap.sCrBy + "',SYSDATE)";
                        ObjCon.Execute(strQry);

                        strQry = "INSERT INTO TBLDTCTRANSACTION VALUES((SELECT max(DCT_ID)+1 FROM TBLDTCTRANSACTION),'" + objswap.sDtcCode + "',";
                        strQry += "'" + objswap.sNewDtrCode + "','1',SYSDATE,'','1','NEW DTC COMMISSIONED (FROM BACKEND)',SYSDATE,'0','','')";
                        ObjCon.Execute(strQry);

                        strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT max(DRT_ID)+1 FROM TBLDTRTRANSACTION),'" + objswap.sNewDtrCode + "',";
                        strQry += "'" + objswap.sOfficeCode + "','2',SYSDATE,'','2','COMMISSIONED TO DTCCODE :" + objswap.sDtcCode + " (FROM BACKEND)','1',SYSDATE,'0','','')";
                        ObjCon.Execute(strQry);

                        Arr[0] = "1";
                        Arr[1] = "Allocate Succesfully";
                        return Arr;
                    }
                    else
                    {

                        if (dtDtcDetails.Rows.Count > 0)
                        {
                            long UNA_SL_NO = ObjCon.Get_max_no("UNA_SL_NO", "TBLUNALLOCATEDDTCS");
                            string sDTC_Code = dtDtcDetails.Rows[0]["DT_CODE"].ToString();

                            strQry = "select DF_ID FROM TBLDTCFAILURE WHERE DF_DTC_CODE='" + sDTC_Code + "'";
                            string sDfId = ObjCon.get_value(strQry);

                            if (sDfId == "")
                            {
                                strQry = "INSERT INTO TBLUNALLOCATEDDTCS (UNA_SL_NO,UNA_MISMATCHENTRY_SLNO,UNA_DTCCODE,UNA_ENTRY_FROM,UNA_IS_DTC_FAILURE)";
                                strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                                strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','NO')";
                                ObjCon.Execute(strQry);
                            }
                            else
                            {
                                strQry = "INSERT INTO TBLUNALLOCATEDDTCS (UNA_SL_NO,UNA_MISMATCHENTRY_SLNO,UNA_DTCCODE,UNA_ENTRY_FROM,UNA_IS_DTC_FAILURE,UNA_FAILURE_ID)";
                                strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                                strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','YES','" + sDfId + "')";
                                ObjCon.Execute(strQry);
                            }



                            strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF Dtc CODE BY DTCMISMATCHENTRY' WHERE TM_DTC_ID='" + sDTC_Code + "' AND TM_LIVE_FLAG=1";
                            ObjCon.Execute(strQry);

                            strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='0' WHERE DT_CODE='" + sDTC_Code + "'";
                            ObjCon.Execute(strQry);
                        }

                        strQry = "SELECT DT_TC_ID FROM TBLDTCMAST WHERE DT_CODE='" + objswap.sNewDTCCode + "'";
                        string sNewDTC_TC = ObjCon.get_value(strQry);

                        long sMaxNo = ObjCon.Get_max_no("UAD_SL_NO", "TBLUNALLOCATEDDTRS");

                        strQry = "INSERT INTO TBLUNALLOCATEDDTRS (UAD_SL_NO,UAD_SL_MISMATCHENTRY_SLNO,UAD_DTRCODE,UAD_ENTRY_FROM)";
                        strQry += " VALUES ('" + sMaxNo + "','" + sDTC_SL_NO + "',";
                        strQry += " '" + sNewDTC_TC + "','DTCMISMATCHENTRY')";
                        ObjCon.Execute(strQry);





                        strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objswap.sNewDtrCode + "' WHERE DT_CODE='" + objswap.sDtcCode + "'";
                        ObjCon.Execute(strQry);

                        strQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + objswap.sOfficeCode + "',TC_CURRENT_LOCATION='2',TC_STATUS = 2,TC_STORE_ID = '" + objswap.sStoreId + "' WHERE TC_CODE='" + objswap.sNewDtrCode + "'";
                        ObjCon.Execute(strQry);

                        DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        objWCF.SaveTcDetails(strQry);

                        strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objswap.sDtrCode + "' WHERE DT_CODE='" + objswap.sNewDTCCode + "'";
                        ObjCon.Execute(strQry);

                        strQry = "UPDATE TBLTCMASTER SET TC_STATUS = 2, TC_LOCATION_ID=(SELECT DT_OM_SLNO FROM TBLDTCMAST WHERE  DT_CODE = '" + objswap.sNewDTCCode + "'),TC_CURRENT_LOCATION='2',TC_STORE_ID = (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_CODE = (SELECT SUBSTR(DT_OM_SLNO,0,2) FROM TBLDTCMAST WHERE DT_CODE = '" + objswap.sNewDTCCode + "')) WHERE TC_CODE='" + objswap.sDtrCode + "'";
                        ObjCon.Execute(strQry);

                        //DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        objWCF.SaveTcDetails(strQry);


                        strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + objswap.sNewDtrCode + "'";
                        string cap = ObjCon.get_value(strQry);

                        TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                        string result = objWCF1.SendCapacitytoTrms(cap, objswap.sDtcCode);


                        strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_TC_ID='" + sNewDTC_TC + "' AND TM_LIVE_FLAG=1 ";
                        ObjCon.Execute(strQry);

                        strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_DTC_ID='" + objswap.sDtcCode + "' AND TM_LIVE_FLAG=1";
                        ObjCon.Execute(strQry);

                        long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                        strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON) VALUES ('" + sMaxmapping + "',";
                        strQry += "SYSDATE,'" + objswap.sNewDtrCode + "','" + objswap.sDtcCode + "','1','" + objswap.sCrBy + "',SYSDATE)";
                        ObjCon.Execute(strQry);

                        strQry = " UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objswap.sCrBy + "',TM_UNMAP_REASON='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE TM_TC_ID='" + objswap.sDtrCode + "' AND TM_LIVE_FLAG=1";
                        ObjCon.Execute(strQry);

                        long sMaxDtcmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                        strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON) VALUES ('" + sMaxDtcmapping + "',";
                        strQry += "SYSDATE,'" + objswap.sDtrCode + "','" + objswap.sNewDTCCode + "','1','" + objswap.sCrBy + "',SYSDATE)";
                        ObjCon.Execute(strQry);

                        strQry = "INSERT INTO TBLDTCTRANSACTION VALUES((SELECT max(DCT_ID)+1 FROM TBLDTCTRANSACTION),'" + objswap.sDtcCode + "',";
                        strQry += "'" + objswap.sNewDtrCode + "','1',SYSDATE,'','1','NEW DTC COMMISSIONED (FROM BACKEND)',SYSDATE,'0','','')";
                        ObjCon.Execute(strQry);

                        strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT max(DRT_ID)+1 FROM TBLDTRTRANSACTION),'" + objswap.sNewDtrCode + "',";
                        strQry += "'" + objswap.sOfficeCode + "','2',SYSDATE,'','2','COMMISSIONED TO DTCCODE :" + objswap.sDtcCode + " (FROM BACKEND)','1',SYSDATE,'0','','')";
                        ObjCon.Execute(strQry);

                        Arr[0] = "1";
                        Arr[1] = "Allocate Succesfully";
                        return Arr;
                    }
                }
            }
            #endregion

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "swapDetails");
                Arr[0] = "2";
                Arr[1] = "Error Occured";
                return Arr;
            }
            finally
            {
                
            }
        }

        public DataTable LoadUnAllocateDetails(string OffCode)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT DT_CODE,DT_NAME,0 AS TC_CODE,TO_CHAR(DM_ENTRY_DATE,'dd-mon-YYYY')DM_ENTRY_DATE,DT_OM_SLNO,(SELECT substr(OFF_NAME,instr(OFF_NAME,':')+1)SECTION FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DT_OM_SLNO,0,4))SECTION FROM TBLUNALLOCATEDDTCS,TBLDTRMISMATCHENTRY,TBLDTCMAST ";
                strQry += " WHERE DT_CODE=UNA_DTCCODE AND DM_SL_NO=UNA_MISMATCHENTRY_SLNO AND DT_OM_SLNO LIKE '" + OffCode + "%'";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUnAllocateDetails");
                return dt;
            }
            finally
            {
                
            }
            
        }

        public DataTable LoadUnAllocateDTRDetails(string OffCode)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT 0 AS DT_CODE,DME_EXISTING_DTR_CODE,TO_CHAR(DME_ENTRY_DATE,'dd-mon-YYYY')DME_ENTRY_DATE,TC_LOCATION_ID,(SELECT substr(OFF_NAME,instr(OFF_NAME,':')+1)SECTION FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(TC_LOCATION_ID,0,4))SECTION FROM TBLUNALLOCATEDDTRS,TBLDTCMISMATCHENTRY, ";
                strQry += " TBLTCMASTER WHERE DME_SL_NO=UAD_SL_MISMATCHENTRY_SLNO AND DME_EXISTING_DTR_CODE=TC_CODE AND TC_LOCATION_ID LIKE '" + OffCode + "%'";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUnAllocateDTRDetails");
                return dt;
            }
            finally
            {
                
            }
            
        }

        public DataTable GetUnmapDetails(clsDtcMissMatchEntry objDtcMissMatch)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT DT_CODE,DT_NAME,DT_OM_SLNO,0 as DT_TC_ID FROM TBLUNALLOCATEDDTCS,TBLDTRMISMATCHENTRY,TBLDTCMAST WHERE DT_CODE=UNA_DTCCODE AND DM_SL_NO=UNA_MISMATCHENTRY_SLNO AND UNA_REALLOCATED_DTR_SLNO IS NULL AND DT_CODE='" + objDtcMissMatch.sDtcCode + "'";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetUnmapDetails");
                return dt;
            }
            finally
            {
                
            }
            
        }

        public DataTable GetUnmapDTRDetails(clsDtcMissMatchEntry objDtcMissMatch)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "SELECT * FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND TC_CODE='" + objDtcMissMatch.sDtrCode + "'";
                dt1 = ObjCon.getDataTable(strQry);

                if (dt1.Rows.Count > 0)
                {
                    return dt1;
                }
                else
                {
                    strQry = "SELECT 0 AS DTCODE,DME_EXISTING_DTR_CODE,TO_CHAR(DME_ENTRY_DATE,'dd-mon-YYYY')DME_ENTRY_DATE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,CASE WHEN ";
                    strQry += " TC_CURRENT_LOCATION='1' THEN 'STORE' WHEN TC_CURRENT_LOCATION='2' THEN 'FIELD' END TC_CURRENT_LOCATION FROM TBLUNALLOCATEDDTRS,TBLDTCMISMATCHENTRY,";
                    strQry += " TBLTCMASTER WHERE DME_SL_NO=UAD_SL_MISMATCHENTRY_SLNO AND DME_EXISTING_DTR_CODE=TC_CODE AND DME_EXISTING_DTR_CODE='" + objDtcMissMatch.sDtrCode + "'";
                    dt = ObjCon.getDataTable(strQry);

                    if (dt.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        strQry = "SELECT 0 as DTCODE,TC_CODE AS DME_EXISTING_DTR_CODE,'' as DME_ENTRY_DATE,TC_CAPACITY,CASE WHEN  TC_CURRENT_LOCATION='1' THEN 'STORE' WHEN ";
                        strQry += " TC_CURRENT_LOCATION='2' THEN 'FIELD' END TC_CURRENT_LOCATION FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_CODE='" + objDtcMissMatch.sDtrCode + "'";
                        dt = ObjCon.getDataTable(strQry);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetUnmapDetails");
                return dt;
            }
            finally
            {
                
            }
            
        }


        public string[] AllocateUnMappDTC(clsDtcMissMatchEntry objMissMatch)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                
                ObjCon.BeginTrans();
                strQry = "UPDATE TBLUNALLOCATEDDTCS SET UNA_REALLOCATION_DATE=SYSDATE,UNA_REALLOCATION_BY='" + objMissMatch.sCrBy + "',UNA_REALLOCATED_DTR_SLNO=";
                strQry += "'" + objMissMatch.sDtrCode + "',UNA_REMARKS='" + objMissMatch.sRemarks + "' WHERE UNA_DTCCODE='" + objMissMatch.sDtcCode + "'";
                ObjCon.Execute(strQry);

                strQry = "SELECT UAD_SL_NO FROM TBLUNALLOCATEDDTRS WHERE UAD_DTRCODE='" + objMissMatch.sDtrCode + "'";
                string res = ObjCon.get_value(strQry);
                if (res == null || res == "")
                {

                }
                else
                {
                    strQry = "UPDATE TBLUNALLOCATEDDTRS SET UAD_REALLOCATION_DATE=SYSDATE,UAD_DTCCODE='" + objMissMatch.sDtcCode + "',UAD_REALLOCATION_BY='" + objMissMatch.sCrBy + "',";
                    strQry += "UAD_REALLOCATED_LOC_TYPE='2',UAD_REALLOCATED_LOC_REFNO='" + objMissMatch.sOfficeCode + "',UAD_REMARKS='" + objMissMatch.sRemarks + "' WHERE UAD_SL_NO='" + res + "'";
                    ObjCon.Execute(strQry);
                }

                strQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + objMissMatch.sDtrCode + "' WHERE DT_CODE='" + objMissMatch.sDtcCode + "'";
                ObjCon.Execute(strQry);

                strQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + objMissMatch.sOfficeCode + "',TC_CURRENT_LOCATION='2' WHERE TC_CODE='" + objMissMatch.sDtrCode + "'";
                ObjCon.Execute(strQry);

                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                objWCF.SaveTcDetails(strQry);

                long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,TM_LIVE_FLAG,TM_CRBY,TM_CRON) VALUES ('" + sMaxmapping + "',";
                strQry += "SYSDATE,'" + objMissMatch.sDtrCode + "','" + objMissMatch.sDtcCode + "','1','" + objMissMatch.sCrBy + "',SYSDATE)";
                ObjCon.Execute(strQry);

                strQry = "INSERT INTO TBLDTCTRANSACTION VALUES((SELECT max(DCT_ID)+1 FROM TBLDTCTRANSACTION),'" + objMissMatch.sDtcCode + "',";
                strQry += "'" + objMissMatch.sDtrCode + "','1',SYSDATE,'','1','NEW DTC COMMISSIONED (FROM BACKEND)',SYSDATE,'0','','')";
                ObjCon.Execute(strQry);

                strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT max(DRT_ID)+1 FROM TBLDTRTRANSACTION),'" + objMissMatch.sDtcCode + "',";
                strQry += "'" + objMissMatch.sOfficeCode + "','2',SYSDATE,'','2','COMMISSIONED TO DTCCODE :" + objMissMatch.sDtrCode + " (FROM BACKEND)','1',SYSDATE,'0','','')";
                ObjCon.Execute(strQry);

              //  strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + objMissMatch.sDtcCode + "'";

                strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + objMissMatch.sDtrCode + "'";
                string cap = ObjCon.get_value(strQry);
                cap = cap == "" ? "0" : cap;

                    TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                    string result = objWCF1.SendCapacitytoTrms(cap, objMissMatch.sDtcCode);


                Arr[0] = "1";
                ObjCon.CommitTrans();
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AllocateUnMappDTC");
                Arr[0] = "2";
                return Arr;
            }
            finally
            {
                
            }
            
        }
    }
}
