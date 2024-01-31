using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
   public class clsEnhancement
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sServicedate { get; set; }
        public string sLoadKw { get; set; }
        public string sLoadHp { get; set; }
        public string sCommissionDate { get; set; }
        public string sCapacity { get; set; }
        public string sLocation { get; set; }
        public string sTcSlno { get; set; }
        public string sTcMake { get; set; }
        public string sEnhancementDate { get; set; }
        public string sReason { get; set; }
        public string sDtcReadings { get; set; }
        public string sEnhancementId { get; set; }
        public string sTcCode { get; set; }
        public string sCrby { get; set; }
        public string sOfficeCode { get; set; }
        public string sEnhancedCapacity { get; set; }
        public string sTCId { get; set; }
        public string sDtrcommdate { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sStatusFlag { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Totaloilquantity { get; set; }
        public string Tankcapacity { get; set; }

        string strFormCode = "clsEnhancement";
        OleDbCommand oledbCommand;
        public string[] SaveEnhancementDetails(clsEnhancement  objEnhancement)
        {
            oledbCommand = new OleDbCommand();    
            string[] Arr = new string[2];
            try
            {
                oledbCommand = new OleDbCommand();  
                OleDbDataReader dr;
                string strQry = string.Empty;
                string strQry1 = string.Empty;
                DataTable dt = new DataTable();
                if (objEnhancement.sEnhancementId == "0" || objEnhancement.sEnhancementId == "")
                {

                    oledbCommand.Parameters.AddWithValue("DtcCode", objEnhancement.sDtcCode);

                    dr = ObjCon.Fetch("SELECT DT_CODE FROM TBLDTCMAST WHERE DT_CODE=:DtcCode",oledbCommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid DTC Code";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();
                    oledbCommand = new OleDbCommand();  
                    //Check Already Failure Entry Done but Decomm Pending
                    oledbCommand.Parameters.AddWithValue("DtcCode1", objEnhancement.sDtcCode);
                    dr = ObjCon.Fetch("SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_DTC_CODE=:DtcCode1 AND DF_REPLACE_FLAG=0",oledbCommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Already Declared Failure or Enhancement or Reduction for Selected DTC Code " + objEnhancement.sDtcCode;
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                  

                    objEnhancement.sEnhancementId = ObjCon.Get_max_no("DF_ID", "TBLDTCFAILURE").ToString();

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("DtcCode2", objEnhancement.sDtcCode);
                    string sOfficeCode = ObjCon.get_value("SELECT MAX(DT_OM_SLNO) FROM TBLDTCMAST WHERE DT_CODE =:DtcCode2", oledbCommand);

                    if (objEnhancement.sEnhancementDate == "")
                    {
                        objEnhancement.sEnhancementDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    //Workflow / Approval
                    #region WorkFlow

                    strQry = "INSERT INTO TBLDTCFAILURE(DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_REASON,DF_DATE,DF_CRBY,DF_CRON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,DF_ENHANCE_CAPACITY,DF_DTR_COMMISSION_DATE,DF_FAILURE_TYPE)";
                    strQry += "VALUES('{0}','" + objEnhancement.sDtcCode + "','" + objEnhancement.sTcCode + "','" + objEnhancement.sReason.Replace(",","") + "',";
                    strQry += " TO_DATE('" + objEnhancement.sEnhancementDate + "','dd/MM/yyyy'),'" + objEnhancement.sCrby + "',SYSDATE,";
                    strQry += " '" + objEnhancement.sDtcReadings + "','" + objEnhancement.sStatusFlag + "','" + sOfficeCode + "','" + objEnhancement.sEnhancedCapacity + "',TO_DATE('" + objEnhancement.sDtrcommdate + "','dd/MM/yyyy'),10)";

                    strQry = strQry.Replace("'", "''");

                    
                    // TC_STATUS  = 11 means  Released Good 
                     strQry1 = " UPDATE TBLTCMASTER SET TC_STATUS= 11 ,TC_UPDATED_EVENT='FAILURE ENTRY',";
                    strQry1 += " TC_UPDATED_EVENT_ID='" + objEnhancement.sEnhancementId + "', TC_LOCATION_ID ='" + sOfficeCode + "' ";
                    strQry1 += "WHERE TC_CODE='" + objEnhancement.sTcCode + "' ";

                    strQry1 = strQry1.Replace("'", "''");

                    string sParam = "SELECT NVL(MAX(DF_ID),0)+1 FROM TBLDTCFAILURE";

                    string Qry = " UPDATE TBLTCMASTER SET TC_TANK_CAPACITY='" + objEnhancement.Tankcapacity + "', ";
                    Qry += "TC_OIL_CAPACITY='" + objEnhancement.Totaloilquantity + "'WHERE TC_CODE='" + objEnhancement.sTcCode + "'  ";
                    ObjCon.Execute(Qry);
                    string Qry1 = " UPDATE TBLDTCMAST SET DT_LONGITUDE='" + objEnhancement.Longitude + "', ";
                    Qry1 += "DT_LATITUDE='" + objEnhancement.Latitude + "' WHERE DT_CODE='" + objEnhancement.sDtcCode + "'  ";
                    ObjCon.Execute(Qry1);
                    clsApproval objApproval = new clsApproval();

                    if (objEnhancement.sActionType == null )
                    {

                        bool bResult = objApproval.CheckAlreadyExistEntry(objEnhancement.sDtcCode, "10");
                        if (bResult == true)
                        {
                            Arr[0] = "Capacity Enhancement/Reduction Already done for DTC Code " + objEnhancement.sDtcCode + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                        bResult = objApproval.CheckAlreadyExistEntry(objEnhancement.sDtcCode, "9");
                        if (bResult == true)
                        {
                            Arr[0] = "Failure Declare Already done for DTC Code " + objEnhancement.sDtcCode + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                    }

                    if (objEnhancement.sStatusFlag == "2")
                        objApproval.sFormName = objEnhancement.sFormName;
                    else
                        objApproval.sFormName = "Reduction Entry";


                    //objApproval.sRecordId = objEnhancement.sEnhancementId;
                    objApproval.sOfficeCode = objEnhancement.sOfficeCode;
                    objApproval.sClientIp = objEnhancement.sClientIP;
                    objApproval.sCrby = objEnhancement.sCrby;

                    objApproval.sQryValues = strQry + ";" + strQry1;
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLDTCFAILURE";
                    objApproval.sRefOfficeCode = objEnhancement.sOfficeCode;
                    objApproval.sDataReferenceId = objEnhancement.sDtcCode;
                    if(objEnhancement.sStatusFlag=="2")
                    objApproval.sDescription = "Capacity Enhancement For DTC Code " + objEnhancement.sDtcCode;
                    else
                       objApproval.sDescription = "Capacity Reduction For DTC Code " + objEnhancement.sDtcCode;

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_REASON,DF_DATE,DF_CRBY,DF_CRON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,DF_ENHANCE_CAPACITY,DTR_COMISSION_DATE";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objEnhancement.sDtcCode + "," + objEnhancement.sTcCode + "," + objEnhancement.sReason + ",";
                    objApproval.sColumnValues += "" + objEnhancement.sEnhancementDate + "," + objEnhancement.sCrby + ",SYSDATE," + objEnhancement.sDtcReadings + "," + objEnhancement.sStatusFlag + "," + sOfficeCode + "," + objEnhancement.sEnhancedCapacity + "," + objEnhancement.sDtrcommdate + "";
                  
                    objApproval.sTableNames = "TBLDTCFAILURE";


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objEnhancement.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objEnhancement.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion
                    if (objEnhancement.sStatusFlag == "2")
                        Arr[0] = "DTC Enhancement Declared Successfully";
                    else
                        Arr[0] = "DTC Reduction Declared Successfully";

                    Arr[1] = "0";
                    return Arr;
                }
                else
                {

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("DtcCode4", objEnhancement.sDtcCode);

                    dr = ObjCon.Fetch("SELECT DT_CODE FROM TBLDTCMAST WHERE DT_CODE=:DtcCode4", oledbCommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid DTC Code";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();


                    oledbCommand = new OleDbCommand();
                    strQry = "UPDATE TBLDTCFAILURE SET DF_DATE=TO_DATE(:sEnhancementDate1,'dd/MM/yyyy'), DF_REASON=:sReason1,";
                    strQry += " DF_KWH_READING=:sDtcReadings1 WHERE DF_ID=:sEnhancementId1";
                    oledbCommand.Parameters.AddWithValue("sEnhancementDate1", objEnhancement.sEnhancementDate);
                    oledbCommand.Parameters.AddWithValue("sReason1", objEnhancement.sReason);
                    oledbCommand.Parameters.AddWithValue("sDtcReadings1", objEnhancement.sDtcReadings);
                    oledbCommand.Parameters.AddWithValue("sEnhancementId1", objEnhancement.sEnhancementId);
                    ObjCon.Execute(strQry, oledbCommand);

                   

                    Arr[0] = "DTC Enhancement Updated Successfully";
                    Arr[1] = "1";
                    return Arr;

                }

            }

            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveEnhancementDetails");
                return Arr;
            }

        }

        public DataTable LoadAllDTCEnhancement(clsEnhancement objEnhancement)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
               
                
                string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                strQry += " TM_ID = TC_MAKE_ID) TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY,DF_ID, 'YES' AS STATUS,DT_PROJECTTYPE ";
                strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE  ";
                strQry += "  AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='2'";
                strQry += " AND DF_LOC_CODE LIKE :OfficeCode||'%'";
                strQry += " UNION ALL";
                strQry += " SELECT DT_ID, TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                strQry += " TM_ID = TC_MAKE_ID) TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY, 0 AS DF_ID,'NO' AS STATUS,DT_PROJECTTYPE  FROM TBLDTCMAST,TBLTCMASTER";
                strQry += " WHERE DT_TC_ID = TC_CODE ";
                strQry += " AND DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0 ) ";
                strQry += " AND DT_OM_SLNO LIKE :OfficeCode1||'%' ORDER BY STATUS ";

                oledbCommand.Parameters.AddWithValue("OfficeCode", objEnhancement.sOfficeCode);

                oledbCommand.Parameters.AddWithValue("OfficeCode1", objEnhancement.sOfficeCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllDTCEnhancement");
                return dtDetails;

            }            

        }

        public DataTable LoadAlreadyEnhanced(clsEnhancement objEnhance)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DF_ID,DT_NAME, TC_SLNO, TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                strQry += " DF_ID,'YES' AS STATUS,DT_PROJECTTYPE  FROM TBLDTCMAST,TBLTCMASTER,";
                strQry += "TBLTRANSMAKES,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE AND ";
                strQry += "TM_ID = TC_MAKE_ID AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='2'";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%'";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objEnhance.sOfficeCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAlreadyEnhanced");
                return dtDetails;

            }
            

        }
        public DataTable LoadAlreadyReduced(clsEnhancement objEnhance)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DF_ID,DT_NAME, TC_SLNO, TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                strQry += " DF_ID,'YES' AS STATUS,DT_PROJECTTYPE  FROM TBLDTCMAST,TBLTCMASTER,";
                strQry += "TBLTRANSMAKES,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE AND ";
                strQry += "TM_ID = TC_MAKE_ID AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='5'";
                strQry += " AND DF_LOC_CODE LIKE :sOfficeCode||'%'";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", objEnhance.sOfficeCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyEnhanced");
                return dtDetails;

            }


        }
        public DataTable LoadAllDTCReduced(clsEnhancement objEnhancement)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();


                string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                strQry += " TM_ID = TC_MAKE_ID) TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY,DF_ID, 'YES' AS STATUS,DT_PROJECTTYPE ";
                strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE  ";
                strQry += "  AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='5'";
                strQry += " AND DF_LOC_CODE LIKE :OfficeCode||'%'";
                strQry += " UNION ALL";
                strQry += " SELECT DT_ID, TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                strQry += " TM_ID = TC_MAKE_ID) TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY, 0 AS DF_ID,'NO' AS STATUS,DT_PROJECTTYPE  FROM TBLDTCMAST,TBLTCMASTER";
                strQry += " WHERE DT_TC_ID = TC_CODE ";
                strQry += " AND DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0 ) ";
                strQry += " AND DT_OM_SLNO LIKE :OfficeCode1||'%'";

                oledbCommand.Parameters.AddWithValue("OfficeCode", objEnhancement.sOfficeCode);

                oledbCommand.Parameters.AddWithValue("OfficeCode1", objEnhancement.sOfficeCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllDTCEnhancement");
                return dtDetails;

            }

        }

        public object GetEnhancementDetails(clsEnhancement objEnhancement)
        {

            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                if (objEnhancement.sEnhancementId  != "0")
                {
                    
                    string strQry = "SELECT DF_ID,DT_ID, DF_DTC_CODE,DT_NAME,TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    strQry += "(SELECT DISTINCT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO  ";
                    strQry += " )AS TC_LOCATION_ID ,TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,TC_CODE,TC_ID, ";
                    strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(DF_DATE,'DD/MM/YYYY') DF_DATE,DF_REASON,TO_CHAR(DF_KWH_READING)DF_KWH_READING,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE DF_CRBY=US_ID) US_FULL_NAME,DF_ENHANCE_CAPACITY,DF_LOC_CODE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD/MM/YYYY') DF_DTR_COMMISSION_DATE  ";
                    strQry += " ,DT_LATITUDE,DT_LONGITUDE,TC_OIL_CAPACITY,TC_TANK_CAPACITY FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_STATUS_FLAG  IN (2,5)  AND DF_DTC_CODE=DT_CODE AND DF_EQUIPMENT_ID=TC_CODE";
                    strQry += " AND DF_ID=:sEnhancementId";
                    oledbCommand.Parameters.AddWithValue("sEnhancementId", objEnhancement.sEnhancementId);
                    dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                    if (dtDetails.Rows.Count > 0)
                    {
                        objEnhancement.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objEnhancement.sDtcCode = dtDetails.Rows[0]["DF_DTC_CODE"].ToString();
                        objEnhancement.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objEnhancement.sServicedate  = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objEnhancement.sLoadKw  = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objEnhancement.sLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objEnhancement.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objEnhancement.sCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objEnhancement.sLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objEnhancement.sTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objEnhancement.sTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objEnhancement.sEnhancementDate = dtDetails.Rows[0]["DF_DATE"].ToString();
                        objEnhancement.sReason = dtDetails.Rows[0]["DF_REASON"].ToString();
                        objEnhancement.sDtcReadings = dtDetails.Rows[0]["DF_KWH_READING"].ToString();
                        objEnhancement.sTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objEnhancement.sEnhancementId = dtDetails.Rows[0]["DF_ID"].ToString();
                        objEnhancement.sCrby = dtDetails.Rows[0]["US_FULL_NAME"].ToString();
                        objEnhancement.sOfficeCode = dtDetails.Rows[0]["DF_LOC_CODE"].ToString();
                        objEnhancement.sEnhancedCapacity = dtDetails.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();
                        objEnhancement.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objEnhancement.sDtrcommdate = dtDetails.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString();
                        objEnhancement.Latitude = dtDetails.Rows[0]["DT_LATITUDE"].ToString();
                        objEnhancement.Longitude = dtDetails.Rows[0]["DT_LONGITUDE"].ToString();
                        objEnhancement.Totaloilquantity = dtDetails.Rows[0]["TC_OIL_CAPACITY"].ToString();
                        objEnhancement.Tankcapacity = dtDetails.Rows[0]["TC_TANK_CAPACITY"].ToString();
                    }

                    return objEnhancement;
                }

                else
                {
                    String strQry = "SELECT  DT_ID,DT_NAME,DT_CODE,TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO ) AS TC_LOCATION_ID ,TC_ID,";
                    strQry += " TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,DT_LATITUDE,DT_LONGITUDE,TC_OIL_CAPACITY,TC_TANK_CAPACITY FROM";
                    strQry += " TBLDTCMAST,TBLTCMASTER WHERE  DT_TC_ID=TC_CODE AND DT_ID=:sDtcId";
                    oledbCommand.Parameters.AddWithValue("sDtcId", objEnhancement.sDtcId);
                    dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                    if (dtDetails.Rows.Count > 0)
                    {
                        objEnhancement.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objEnhancement.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objEnhancement.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objEnhancement.sServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objEnhancement.sLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objEnhancement.sLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objEnhancement.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objEnhancement.sCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objEnhancement.sLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objEnhancement.sTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objEnhancement.sTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objEnhancement.sTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objEnhancement.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objEnhancement.Latitude = dtDetails.Rows[0]["DT_LATITUDE"].ToString();
                        objEnhancement.Longitude = dtDetails.Rows[0]["DT_LONGITUDE"].ToString();
                        objEnhancement.Totaloilquantity = dtDetails.Rows[0]["TC_OIL_CAPACITY"].ToString();
                        objEnhancement.Tankcapacity = dtDetails.Rows[0]["TC_TANK_CAPACITY"].ToString();


                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT TO_CHAR(TM_MAPPING_DATE,'DD/MM/YYYY')DTR_COMM_DATE FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID=:sDtcCode AND TM_TC_ID=:sTcCode AND TM_LIVE_FLAG=1";
                        oledbCommand.Parameters.AddWithValue("sDtcCode", objEnhancement.sDtcCode);
                        oledbCommand.Parameters.AddWithValue("sTcCode", objEnhancement.sTcCode);
                        string sdtrCommDate = ObjCon.get_value(strQry, oledbCommand);
                        objEnhancement.sDtrcommdate = sdtrCommDate;

                    }

                    return objEnhancement;
                }


            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetEnhancementDetails");
                return objEnhancement;
            }
            

        }

        public bool ValidateEnhancementUpdate(string sEnhanceId)
        {
            try
            {
                
                DataTable dt = new DataTable();
                oledbCommand = new OleDbCommand();
                string strQry = "select WO_DF_ID from TBLWORKORDER,TBLDTCFAILURE WHERE WO_DF_ID=DF_ID AND  DF_STATUS_FLAG='2'";
                strQry += " AND DF_ID=:sEnhanceId";
                oledbCommand.Parameters.AddWithValue("sEnhanceId", sEnhanceId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateEnhancementUpdate");
                return false;
            }            
        }

        #region WorkFlow XML

        public clsEnhancement GetEnhancementDetailsFromXML(clsEnhancement objEnhancement)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if(objEnhancement.sWFDataId!="" && objEnhancement.sWFDataId!=null)
                {
                    dt = objApproval.GetDatatableFromXML(objEnhancement.sWFDataId);
                }

                if (dt.Rows.Count > 0)
                {
                    objEnhancement.sEnhancementDate = Convert.ToString(dt.Rows[0]["DF_DATE"]).Trim();
                    objEnhancement.sReason = Convert.ToString(dt.Rows[0]["DF_REASON"]).Trim();
                    objEnhancement.sDtcCode = Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]).Trim();
                    objEnhancement.sOfficeCode = Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]);
                    objEnhancement.sCrby = Convert.ToString(dt.Rows[0]["DF_CRBY"]);
                    objEnhancement.sDtcReadings = Convert.ToString(dt.Rows[0]["DF_KWH_READING"]);
                    if (dt.Columns.Contains("DF_DTR_COMMISSION_DATE"))
                    {
                        objEnhancement.sDtrcommdate = Convert.ToString(dt.Rows[0]["DF_DTR_COMMISSION_DATE"]);
                    }

                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                    {
                        objEnhancement.sEnhancedCapacity = Convert.ToString(dt.Rows[0]["DF_ENHANCE_CAPACITY"]);
                    }                   

                    objEnhancement.sEnhancementId = "0";
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("sDtcCode", objEnhancement.sDtcCode);
                    objEnhancement.sDtcId = ObjCon.get_value("SELECT DT_ID FROM TBLDTCMAST WHERE DT_CODE=:sDtcCode",oledbCommand);
                    GetEnhancementDetails(objEnhancement);
                }
                return objEnhancement;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailureDetailsFromXML");
                return objEnhancement;
            }
        }

        #endregion

        public string getWfoIDforEstimationSO(string sWOId)
        {
            string sWFOID = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                string sQry = string.Empty;
                sQry = "SELECT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID=:sWOId ";
                oledbCommand.Parameters.AddWithValue("sWOId", sWOId);
                sWFOID = ObjCon.get_value(sQry, oledbCommand);
                return sWFOID;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getWfoIDforEstimationSO");
                return sWFOID;
            }            
        }

        public string getWoIDforEstimation(string sOffCode, string sDtcCode)
        {

            string sWoID = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                string sQry = string.Empty;
                sQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_OFFICE_CODE=:sOffCode AND WO_NEXT_ROLE=1 AND WO_DATA_ID=:sDtcCode ";
                oledbCommand.Parameters.AddWithValue("sOffCode", sOffCode);
                oledbCommand.Parameters.AddWithValue("sDtcCode", sDtcCode);
                sWoID = ObjCon.get_value(sQry, oledbCommand);
                return sWoID;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getWoIDforEstimation");
                return sWoID;
            }
            

        }

    }
}
