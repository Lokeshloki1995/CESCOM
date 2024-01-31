using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.Diagnostics;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsFailureEntry
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sDtcServicedate { get; set; }
        public string sDtcLoadKw { get; set; }
        public string sDtcLoadHp { get; set; }
        public string sCommissionDate { get; set; }
        public string sDtcCapacity { get; set; }
        public string sDtcLocation { get; set; }
        public string sDtcTcSlno { get; set; }
        public string sDtcTcMake { get; set; }
        public string sFailureDate { get; set; }
        public string sFailureReasure { get; set; }
        public string sDtcReadings { get; set; }
        public string sDtcTcCode { get; set; }
        public string sFailureId { get; set; }
        public string sCrby { get; set; }
        public string sOfficeCode { get; set; }
        public string sDtrSaveCommissionDate { get; set; }
        public string sDTrCommissionDate { get; set; }
        public string sDTrEnumerationDate { get; set; }
        public string sManfDate { get; set; }
        public string sTCId { get; set; }
        public string sSubDivName { get; set; }
        public string sLastRepairedBy { get; set; }
        public string sLastRepairedDate { get; set; }
        public string sGuarantyType { get; set; }
        public string sGuarantySource { get; set; }

        public string sEnhancedCapacity { get; set; }
        public string sFailtype { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; } 
        public string sFailureType { get; set; }
        public string sHTBusing { get; set; }
        public string sLTBusing { get; set; }
        public string sHTBusingRod { get; set; }
        public string sLTBusingRod { get; set; }
        public string sOilLevel { get; set; }
        public string sOilQuantity { get; set; }
        public string sTankCondition { get; set; }
        public string sWheel { get; set; }
        public string sExplosionValve { get; set; }
        public string sBreather { get; set; }
        public string sDrainValve { get; set; }
        public string sOilCapacity { get; set; }
        public string sFirstGuarantyType = string.Empty;

        public string sTcInvoiceDate { get; set; }
        public string sDTRStarRating { get; set; }
        public string sCustomerName { get; set; }
        public string sNumberOfInstalments { get; set; }
        public string sCustomerNumber { get; set; }
        public string sTankcapacity { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        OleDbCommand oledbCommand;
        /// <summary>
        /// This function calls while click on save button in failure form
        /// </summary>
        /// <param name="objFailureDetails"></param>
        /// <returns></returns>
        public string[] SaveFailureDetails(clsFailureEntry objFailureDetails)
        {
            string[] Arr = new string[2];
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);
            try
            {
                oledbCommand = new OleDbCommand();
                OleDbDataReader dr;
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //Check DTC Code exists or not
                string qry = "SELECT DT_CODE FROM TBLDTCMAST WHERE DT_CODE=:DtcCode";
                oledbCommand.Parameters.AddWithValue("DtcCode", objFailureDetails.sDtcCode);
                dr = ObjCon.Fetch(qry, oledbCommand);
                if (!dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Enter Valid DTC Code";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                oledbCommand = new OleDbCommand();
                string qrys = "SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_DTC_CODE=:DtcCodes AND DF_REPLACE_FLAG=0";
                oledbCommand.Parameters.AddWithValue("DtcCodes", objFailureDetails.sDtcCode);
                dr = ObjCon.Fetch(qrys, oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Already Declared Failure or Enhancement for Selected DTC Code " + objFailureDetails.sDtcCode;
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();

                qrys = "select SM_ID from TBLSTOREMAST where SM_OFF_CODE='" + objFailureDetails.sOfficeCode.Substring(0, 2) + "' ";
                String storeId = ObjCon.get_value(qrys);


                if (objFailureDetails.sFailureId == "0" || objFailureDetails.sFailureId == "")
                {
                    //Workflow / Approval
                    #region Workflow

                    if (objFailureDetails.sFailtype == "1")
                    {
                        strQry = "INSERT INTO TBLDTCFAILURE(DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_CRBY, ";
                        strQry += " DF_CRON,DF_DATE,DF_REASON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,";
                        strQry += " DF_FAILURE_TYPE,DF_HT_BUSING,DF_LT_BUSING,DF_HT_BUSING_ROD, ";
                        strQry += " DF_LT_BUSING_ROD,DF_BREATHER,DF_OIL_LEVEL,DF_DRAIN_VALVE,";
                        strQry += " DF_OIL_QNTY,DF_TANK_CONDITION,DF_EXPLOSION,DF_GUARANTY_TYPE, ";
                        strQry += " DF_GUARANTY_TYPE_SOURCE,DF_DTR_COMMISSION_DATE,DF_DTR_STAR_RATING, ";
                        strQry += " DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,DF_NUMBER_OF_INSTALLATIONS)";
                        strQry += "VALUES('{0}','" + objFailureDetails.sDtcCode + "','" + objFailureDetails.sDtcTcCode 
                            + "','" + objFailureDetails.sCrby + "',";
                        strQry += " SYSDATE,TO_DATE('" + objFailureDetails.sFailureDate + "','dd/MM/yyyy HH24:mi:ss'),'" 
                            + objFailureDetails.sFailureReasure + "',";
                        strQry += " '" + objFailureDetails.sDtcReadings + "','" + objFailureDetails.sFailtype + "','" 
                            + sOfficeCode + "','" + objFailureDetails.sFailureType + "','" + objFailureDetails.sHTBusing + "',";
                        strQry += " '" + objFailureDetails.sLTBusing + "','" + objFailureDetails.sHTBusingRod + "','" 
                            + objFailureDetails.sLTBusingRod + "','" + objFailureDetails.sBreather + "',";
                        strQry += " '" + objFailureDetails.sOilLevel + "','" + objFailureDetails.sDrainValve + "','" 
                            + objFailureDetails.sOilQuantity + "','" + objFailureDetails.sTankCondition + "',";
                        strQry += " '" + objFailureDetails.sExplosionValve + "','" + objFailureDetails.sGuarantyType 
                            + "','" + objFailureDetails.sGuarantySource + "',TO_DATE('" + objFailureDetails.sDTrCommissionDate
                            + "','dd/MM/yyyy'), '" + objFailureDetails.sDTRStarRating + "'";
                        strQry += ",'" + objFailureDetails.sCustomerName + "'," + objFailureDetails.sCustomerNumber 
                            + "," + objFailureDetails.sNumberOfInstalments + ")";
                    }
                    else
                    {
                        strQry = "INSERT INTO TBLDTCFAILURE(DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_CRBY,DF_CRON, ";
                        strQry += " DF_DATE,DF_REASON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,";
                        strQry += " DF_FAILURE_TYPE,DF_HT_BUSING,DF_LT_BUSING,DF_HT_BUSING_ROD,DF_LT_BUSING_ROD, ";
                        strQry += " DF_BREATHER,DF_OIL_LEVEL,DF_DRAIN_VALVE,";
                        strQry += " DF_OIL_QNTY,DF_TANK_CONDITION,DF_EXPLOSION,DF_ENHANCE_CAPACITY,DF_GUARANTY_TYPE, ";
                        strQry += " DF_GUARANTY_TYPE_SOURCE,DF_DTR_COMMISSION_DATE, DF_DTR_STAR_RATING, ";
                        strQry += " DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,DF_NUMBER_OF_INSTALLATIONS)";
                        strQry += "VALUES('{0}','" + objFailureDetails.sDtcCode + "','" + objFailureDetails.sDtcTcCode 
                            + "','" + objFailureDetails.sCrby + "',";
                        strQry += " SYSDATE,TO_DATE('" + objFailureDetails.sFailureDate + "','dd/MM/yyyy HH24:mi:ss'),'" 
                            + objFailureDetails.sFailureReasure + "',";
                        strQry += " '" + objFailureDetails.sDtcReadings + "','" + objFailureDetails.sFailtype + "','" 
                            + sOfficeCode + "','" + objFailureDetails.sFailureType + "','" + objFailureDetails.sHTBusing + "',";
                        strQry += " '" + objFailureDetails.sLTBusing + "','" + objFailureDetails.sHTBusingRod + "','" 
                            + objFailureDetails.sLTBusingRod + "','" + objFailureDetails.sBreather + "',";
                        strQry += " '" + objFailureDetails.sOilLevel + "','" + objFailureDetails.sDrainValve + "','" 
                            + objFailureDetails.sOilQuantity + "','" + objFailureDetails.sTankCondition + "',";
                        strQry += " '" + objFailureDetails.sExplosionValve + "','" + objFailureDetails.sEnhancedCapacity 
                            + "','" + objFailureDetails.sGuarantyType + "','" + objFailureDetails.sGuarantySource 
                            + "',TO_DATE('" + objFailureDetails.sDTrCommissionDate + "','dd/MM/yyyy'), '" 
                            + objFailureDetails.sDTRStarRating + "'";
                        strQry += " ,'" + objFailureDetails.sCustomerName + "'," + objFailureDetails.sCustomerNumber 
                            + "," + objFailureDetails.sNumberOfInstalments + ")";
                    }

                    strQry = strQry.Replace("'", "''");
                    
                    string strQry1 = " UPDATE TBLTCMASTER SET TC_STATUS=3 ,TC_UPDATED_EVENT='FAILURE ENTRY',";
                    strQry1 += " TC_UPDATED_EVENT_ID='" + objFailureDetails.sDtcId + "',";
                    strQry1 += " TC_LOCATION_ID ='" + sOfficeCode + "',TC_STORE_ID='" + storeId + "' , ";
                    strQry1 += " TC_STAR_RATE = '" + objFailureDetails.sDTRStarRating + "' ";
                    strQry1 += "WHERE TC_CODE='" + objFailureDetails.sDtcTcCode + "' ";

                    strQry1 = strQry1.Replace("'", "''");

                    string sParam = "SELECT NVL(MAX(DF_ID),0)+1 FROM TBLDTCFAILURE";

                    string Qry = " UPDATE TBLTCMASTER SET TC_TANK_CAPACITY='" + objFailureDetails.sTankcapacity + "', ";
                    Qry += "TC_OIL_CAPACITY='" + objFailureDetails.sOilQuantity 
                        + "'WHERE TC_CODE='" + objFailureDetails.sDtcTcCode + "'  ";
                    ObjCon.Execute(Qry);
                    string Qry1 = " UPDATE TBLDTCMAST SET DT_LONGITUDE='" + objFailureDetails.Longitude + "', ";
                    Qry1 += "DT_LATITUDE='" + objFailureDetails.Latitude + "'WHERE DT_CODE='" 
                        + objFailureDetails.sDtcCode + "'  ";
                    ObjCon.Execute(Qry1);

                    clsApproval objApproval = new clsApproval();

                    if (objFailureDetails.sActionType == null)
                    {

                        bool bResult = objApproval.CheckAlreadyExistEntry(objFailureDetails.sDtcCode, "9");
                        if (bResult == true)
                        {
                            Arr[0] = "Failure Declare Already done for DTC Code " 
                                + objFailureDetails.sDtcCode + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                        bResult = objApproval.CheckAlreadyExistEntry(objFailureDetails.sDtcCode, "10");
                        if (bResult == true)
                        {
                            Arr[0] = "Capacity Enhancement Already done for DTC Code " 
                                + objFailureDetails.sDtcCode + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }

                    objApproval.sFormName = objFailureDetails.sFormName;
                    objApproval.sOfficeCode = objFailureDetails.sOfficeCode;
                    objApproval.sClientIp = objFailureDetails.sClientIP;
                    objApproval.sCrby = objFailureDetails.sCrby;
                    objApproval.sQryValues = strQry + ";" + strQry1;
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLDTCFAILURE";
                    objApproval.sDataReferenceId = objFailureDetails.sDtcCode;

                    if (objFailureDetails.sFailtype == "1")
                    {
                        objApproval.sDescription = "Failure Entry For DTC Code " + objFailureDetails.sDtcCode;
                    }
                    else
                    {
                        objApproval.sDescription = "Failure Entry with Enhancement For DTC Code " 
                            + objFailureDetails.sDtcCode;
                    }
                    objApproval.sRefOfficeCode = objFailureDetails.sOfficeCode;

                    string sPrimaryKey = "{0}";

                    if (objFailureDetails.sFailtype == "1")
                    {
                        objApproval.sColumnNames = "DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_CRBY,DF_CRON,DF_DATE,";
                        objApproval.sColumnNames += "DF_REASON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,";
                        objApproval.sColumnNames += "DF_FAILURE_TYPE,DF_HT_BUSING,DF_LT_BUSING,DF_HT_BUSING_ROD,";
                        objApproval.sColumnNames += "DF_LT_BUSING_ROD,DF_BREATHER,DF_OIL_LEVEL,DF_DRAIN_VALVE,";
                        objApproval.sColumnNames += "DF_OIL_QNTY,DF_TANK_CONDITION,DF_EXPLOSION,GUARENTEE,";
                        objApproval.sColumnNames += "DF_GUARANTY_TYPE_SOURCE,DTR_COMISSION_DATE,DF_DTR_STAR_RATING,";
                        objApproval.sColumnNames += "DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,DF_NUMBER_OF_INSTALLATIONS";
                        objApproval.sColumnValues = "" + sPrimaryKey + "," + objFailureDetails.sDtcCode + "," 
                            + objFailureDetails.sDtcTcCode + "," + objFailureDetails.sCrby + ",SYSDATE,";
                        objApproval.sColumnValues += "" + objFailureDetails.sFailureDate + "," 
                            + objFailureDetails.sFailureReasure.Replace(",", "") + "," + objFailureDetails.sDtcReadings 
                            + "," + objFailureDetails.sFailtype + "," + sOfficeCode + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sFailureType + "," + objFailureDetails.sHTBusing + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sLTBusing + "," + objFailureDetails.sHTBusingRod 
                            + "," + objFailureDetails.sLTBusingRod + "," + objFailureDetails.sBreather + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sOilLevel + "," + objFailureDetails.sDrainValve 
                            + "," + objFailureDetails.sOilQuantity + "," + objFailureDetails.sTankCondition + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sExplosionValve.Trim() + "," 
                            + objFailureDetails.sGuarantyType + "," + objFailureDetails.sGuarantySource + "," 
                            + objFailureDetails.sDTrCommissionDate + " ," + objFailureDetails.sDTRStarRating + "";
                        objApproval.sColumnValues += "," + objFailureDetails.sCustomerName + "," 
                            + objFailureDetails.sCustomerNumber + "," + objFailureDetails.sNumberOfInstalments + "";
                        objApproval.sTableNames = "TBLDTCFAILURE";
                    }
                    else
                    {

                        objApproval.sColumnNames = "DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_CRBY,DF_CRON,";
                        objApproval.sColumnNames += "DF_DATE,DF_REASON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,";
                        objApproval.sColumnNames += "DF_FAILURE_TYPE,DF_HT_BUSING,DF_LT_BUSING,DF_HT_BUSING_ROD,";
                        objApproval.sColumnNames += "DF_LT_BUSING_ROD,DF_BREATHER,DF_OIL_LEVEL,DF_DRAIN_VALVE,";
                        objApproval.sColumnNames += "DF_OIL_QNTY,DF_TANK_CONDITION,DF_EXPLOSION,DF_ENHANCE_CAPACITY,";
                        objApproval.sColumnNames += "GUARENTEE,DF_GUARANTY_TYPE_SOURCE,DTR_COMISSION_DATE,";
                        objApproval.sColumnNames += "DF_DTR_STAR_RATING,DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,";
                        objApproval.sColumnNames += "DF_NUMBER_OF_INSTALLATIONS";
                        objApproval.sColumnValues = "" + sPrimaryKey + "," + objFailureDetails.sDtcCode + "," 
                            + objFailureDetails.sDtcTcCode + "," + objFailureDetails.sCrby + ",SYSDATE,";
                        objApproval.sColumnValues += "" + objFailureDetails.sFailureDate + "," 
                            + objFailureDetails.sFailureReasure.Replace(",", "ç") + "," + objFailureDetails.sDtcReadings 
                            + "," + objFailureDetails.sFailtype + "," + sOfficeCode + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sFailureType + "," 
                            + objFailureDetails.sHTBusing + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sLTBusing + "," + objFailureDetails.sHTBusingRod 
                            + "," + objFailureDetails.sLTBusingRod + "," + objFailureDetails.sBreather + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sOilLevel + "," + objFailureDetails.sDrainValve 
                            + "," + objFailureDetails.sOilQuantity + "," + objFailureDetails.sTankCondition + ",";
                        objApproval.sColumnValues += "" + objFailureDetails.sExplosionValve.Trim() + ","
                            + objFailureDetails.sEnhancedCapacity + "," + objFailureDetails.sGuarantyType +
                            "," + objFailureDetails.sGuarantySource + "," + objFailureDetails.sDTrCommissionDate +
                            " ," + objFailureDetails.sDTRStarRating + "";
                        objApproval.sColumnValues += "," + objFailureDetails.sCustomerName + ","
                            + objFailureDetails.sCustomerNumber + "," + objFailureDetails.sNumberOfInstalments + "";
                        objApproval.sTableNames = "TBLDTCFAILURE";
                    }


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objFailureDetails.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                        objFailureDetails.sWFDataId = objApproval.sWFDataId;

                    }
                    else
                    {
                        objconn.BeginTrans();
                        objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlowlatest(objconn);
                        objApproval.SaveWorkflowObjectslatest(objApproval, objconn);
                    }

                    #endregion

                    Arr[0] = "DTC Failure Declared Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    string qryfailureid = "select WO_DF_ID,DF_ID from TBLWORKORDER,TBLDTCFAILURE ";
                    qryfailureid += "  WHERE WO_DF_ID=DF_ID AND DF_ID=:FailureId";
                    oledbCommand.Parameters.AddWithValue("FailureId", objFailureDetails.sFailureId);
                    dr = ObjCon.Fetch(qryfailureid, oledbCommand);
                    dt.Load(dr);
                    if (dt.Rows.Count > 0)
                    {

                        dr.Close();
                        Arr[0] = "WorkOrder done So cannot update";
                        Arr[1] = "2";
                        return Arr;

                    }

                    strQry = "UPDATE TBLDTCFAILURE SET DF_DATE=TO_DATE(:sFailureDate1,'dd/MM/yyyy'), ";
                    strQry += " DF_REASON=:sFailureReasure1,DF_KWH_READING=:sDtcReadings1,";
                    strQry += " DF_FAILURE_TYPE=:sFailureType1,DF_HT_BUSING=:sHTBusing1,DF_LT_BUSING=:sLTBusing1,";
                    strQry += " DF_HT_BUSING_ROD=:sHTBusingRod1,DF_LT_BUSING_ROD=:sLTBusingRod1,DF_BREATHER=:sBreather1,";
                    strQry += " DF_OIL_LEVEL=:sOilLevel1,DF_DRAIN_VALVE=:sDrainValve,";
                    strQry += " DF_OIL_QNTY=:sOilQuantity1,DF_TANK_CONDITION=:sTankCondition1,DF_WHEEL=:sWheel1,";
                    strQry += " DF_EXPLOSION=:sExplosionValve1,DF_OIL_CAPACITY=:sOilCapacity1";
                    strQry += " WHERE DF_ID=:sFailureId12 AND DF_EQUIPMENT_ID=:sDtcTcCode12";
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("sFailureDate1", objFailureDetails.sFailureDate);
                    oledbCommand.Parameters.AddWithValue("sFailureReasure1", objFailureDetails.sFailureReasure);
                    oledbCommand.Parameters.AddWithValue("sDtcReadings1", objFailureDetails.sDtcReadings);
                    oledbCommand.Parameters.AddWithValue("sFailureType1", objFailureDetails.sFailureType);
                    oledbCommand.Parameters.AddWithValue("sHTBusing1", objFailureDetails.sHTBusing);
                    oledbCommand.Parameters.AddWithValue("sLTBusing1", objFailureDetails.sLTBusing);
                    oledbCommand.Parameters.AddWithValue("sHTBusingRod1", objFailureDetails.sHTBusingRod);
                    oledbCommand.Parameters.AddWithValue("sLTBusingRod1", objFailureDetails.sLTBusingRod);
                    oledbCommand.Parameters.AddWithValue("sBreather1", objFailureDetails.sBreather);

                    oledbCommand.Parameters.AddWithValue("sDrainValve", objFailureDetails.sDrainValve);
                    oledbCommand.Parameters.AddWithValue("sOilLevel1", objFailureDetails.sOilLevel);
                    oledbCommand.Parameters.AddWithValue("sOilQuantity1", objFailureDetails.sOilQuantity);
                    oledbCommand.Parameters.AddWithValue("sTankCondition1", objFailureDetails.sTankCondition);
                    oledbCommand.Parameters.AddWithValue("sWheel1", objFailureDetails.sWheel);
                    oledbCommand.Parameters.AddWithValue("sExplosionValve1", objFailureDetails.sExplosionValve);
                    oledbCommand.Parameters.AddWithValue("sOilCapacity1", objFailureDetails.sOilCapacity);
                    oledbCommand.Parameters.AddWithValue("sFailureId12", objFailureDetails.sFailureId);
                    oledbCommand.Parameters.AddWithValue("sDtcTcCode12", objFailureDetails.sDtcTcCode);

                    ObjCon.Execute(strQry, oledbCommand);

                    strQry = " UPDATE TBLTCMASTER SET TC_STATUS=3 ,TC_UPDATED_EVENT='FAILURE ENTRY',";
                    strQry += " TC_UPDATED_EVENT_ID=:sDtcId13, TC_LOCATION_ID = (SELECT MAX(DT_OM_SLNO) ";
                    strQry += " FROM TBLDTCMAST WHERE DT_CODE =:sDtcCode13) ";
                    strQry += "WHERE TC_CODE=:sDtcTcCode13 ";

                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("sDtcId13", objFailureDetails.sDtcId);
                    oledbCommand.Parameters.AddWithValue("sDtcCode13", objFailureDetails.sDtcCode);
                    oledbCommand.Parameters.AddWithValue("sDtcTcCode13", objFailureDetails.sDtcTcCode);

                    ObjCon.Execute(strQry, oledbCommand);

                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;

                }
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        /// This will update the dtr commission date
        /// </summary>
        /// <param name="objFailureDetails"></param>
        public void UpdateDtrCommDate(clsFailureEntry objFailureDetails)
        {
            try
            {
                string strQry;
                strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_MAPPING_DATE=TO_DATE(:sDtrSaveCommissionDate13,'YYYY-MM-DD') ";
                strQry += " WHERE TM_DTC_ID=:sDtcCode13 AND TM_TC_ID=:sDtcTcCode13 AND TM_LIVE_FLAG='1'";
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sDtrSaveCommissionDate13", objFailureDetails.sDtrSaveCommissionDate);
                oledbCommand.Parameters.AddWithValue("sDtcCode13", objFailureDetails.sDtcCode);
                oledbCommand.Parameters.AddWithValue("sDtcTcCode13", objFailureDetails.sDtcTcCode);
                ObjCon.Execute(strQry, oledbCommand);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// This Method used to search the failure details
        /// </summary>
        /// <param name="objFailureDetails"></param>
        /// <returns></returns>
        public clsFailureEntry SearchFailureDetails(clsFailureEntry objFailureDetails)
        {
            DataTable dtDetails = new DataTable();
            OleDbDataReader dr = null;
            oledbCommand = new OleDbCommand();
            try
            {
                objFailureDetails.sFailureId = "0";
                if (objFailureDetails.sFailureId != "0")
                {


                    string strQry = "SELECT DF_ID,DT_ID, DF_DTC_CODE,DT_NAME, ";
                    strQry += " TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP,";
                    strQry += " TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    strQry += "(SELECT DISTINCT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO)AS TC_LOCATION_ID ,";
                    strQry += " TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,TC_CODE,";
                    strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(DF_DATE,'DD/MM/YYYY') DF_FAILED_DATE, ";
                    strQry += " DF_REASON,TO_CHAR(DF_KWH_READING)DF_KWH_READING,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE DF_CRBY=US_ID) US_FULL_NAME,DF_LOC_CODE,TC_ID, ";
                    strQry += " DF_FAILURE_TYPE,DF_HT_BUSING,DF_LT_BUSING,DF_HT_BUSING_ROD,DF_LT_BUSING_ROD,DF_BREATHER,";
                    strQry += " DF_OIL_LEVEL,DF_DRAIN_VALVE,DF_OIL_QNTY,DF_TANK_CONDITION,DF_WHEEL,DF_EXPLOSION,";
                    strQry += " DF_OIL_CAPACITY,TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY') TC_MANF_DATE,DF_ENHANCE_CAPACITY,TC_OIL_CAPACITY ";
                    strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_DTC_CODE=DT_CODE ";
                    strQry += " AND DF_DTC_CODE=DT_CODE AND DF_EQUIPMENT_ID=TC_CODE ";
                    strQry += " AND DF_ID=:FailureId";

                    oledbCommand.Parameters.AddWithValue("FailureId", objFailureDetails.sFailureId);
                    dr = ObjCon.Fetch(strQry, oledbCommand);
                    dtDetails.Load(dr);

                    if (dtDetails.Rows.Count > 0)
                    {
                        objFailureDetails.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objFailureDetails.sDtcCode = dtDetails.Rows[0]["DF_DTC_CODE"].ToString();
                        objFailureDetails.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objFailureDetails.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objFailureDetails.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objFailureDetails.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objFailureDetails.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objFailureDetails.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objFailureDetails.sEnhancedCapacity = dtDetails.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();
                        objFailureDetails.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objFailureDetails.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objFailureDetails.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objFailureDetails.sFailureDate = dtDetails.Rows[0]["DF_FAILED_DATE"].ToString();
                        objFailureDetails.sFailureReasure = dtDetails.Rows[0]["DF_REASON"].ToString();
                        objFailureDetails.sDtcReadings = dtDetails.Rows[0]["DF_KWH_READING"].ToString();
                        objFailureDetails.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objFailureDetails.sFailureId = dtDetails.Rows[0]["DF_ID"].ToString();
                        objFailureDetails.sCrby = dtDetails.Rows[0]["US_FULL_NAME"].ToString();
                        objFailureDetails.sOfficeCode = dtDetails.Rows[0]["DF_LOC_CODE"].ToString();
                        objFailureDetails.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objFailureDetails.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        GetDTRCommissionDate(objFailureDetails);

                        objFailureDetails.sFailureType = dtDetails.Rows[0]["DF_FAILURE_TYPE"].ToString();
                        objFailureDetails.sHTBusing = dtDetails.Rows[0]["DF_HT_BUSING"].ToString();
                        objFailureDetails.sLTBusing = dtDetails.Rows[0]["DF_LT_BUSING"].ToString();
                        objFailureDetails.sHTBusingRod = dtDetails.Rows[0]["DF_HT_BUSING_ROD"].ToString();
                        objFailureDetails.sLTBusingRod = dtDetails.Rows[0]["DF_LT_BUSING_ROD"].ToString();
                        objFailureDetails.sBreather = dtDetails.Rows[0]["DF_BREATHER"].ToString();
                        objFailureDetails.sOilLevel = dtDetails.Rows[0]["DF_OIL_LEVEL"].ToString();
                        objFailureDetails.sDrainValve = dtDetails.Rows[0]["DF_DRAIN_VALVE"].ToString();
                        objFailureDetails.sOilQuantity = dtDetails.Rows[0]["DF_OIL_QNTY"].ToString();
                        objFailureDetails.sTankCondition = dtDetails.Rows[0]["DF_TANK_CONDITION"].ToString();
                        objFailureDetails.sWheel = dtDetails.Rows[0]["DF_WHEEL"].ToString();
                        objFailureDetails.sExplosionValve = dtDetails.Rows[0]["DF_EXPLOSION"].ToString();
                      //  objFailureDetails.sOilQuantity = dtDetails.Rows[0]["DF_OIL_CAPACITY"].ToString();
                        objFailureDetails.sOilQuantity = dtDetails.Rows[0]["TC_OIL_CAPACITY"].ToString();
                        objFailureDetails.sTankcapacity = dtDetails.Rows[0]["TC_TANK_CAPACITY"].ToString();

                        objFailureDetails.sEnhancedCapacity = Convert.ToString(dtDetails.Rows[0]["DF_ENHANCE_CAPACITY"]).Trim();


                        GetLastRepairedDetails(objFailureDetails);

                    }

                    return objFailureDetails;
                }

                else
                {
                    String strQry = "SELECT  DT_ID,DT_NAME,DT_CODE,";
                    strQry += "TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE, ";
                    strQry += " TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP, ";
                    strQry += " TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO ) AS TC_LOCATION_ID ,";
                    strQry += " TC_ID,TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID, ";
                    strQry += " TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                    strQry += " TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY') TC_MANF_DATE,TC_STAR_RATE,DT_LATITUDE,DT_LONGITUDE,TC_OIL_CAPACITY,TC_TANK_CAPACITY FROM TBLDTCMAST, ";
                    strQry += " TBLTCMASTER WHERE  DT_TC_ID=TC_CODE AND DT_CODE=:DtcCode";
                    strQry += " AND DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0 )";
                    oledbCommand.Parameters.AddWithValue("DtcCode", objFailureDetails.sDtcCode);
                    dr = ObjCon.Fetch(strQry, oledbCommand);
                    dtDetails.Load(dr);


                    if (dtDetails.Rows.Count > 0)
                    {
                        objFailureDetails.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objFailureDetails.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objFailureDetails.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objFailureDetails.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objFailureDetails.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objFailureDetails.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objFailureDetails.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objFailureDetails.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objFailureDetails.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objFailureDetails.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objFailureDetails.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objFailureDetails.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objFailureDetails.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objFailureDetails.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objFailureDetails.sDTRStarRating = Convert.ToString(dtDetails.Rows[0]["TC_STAR_RATE"]);
                        objFailureDetails.Latitude = dtDetails.Rows[0]["DT_LATITUDE"].ToString();
                        objFailureDetails.Longitude = dtDetails.Rows[0]["DT_LONGITUDE"].ToString();
                        objFailureDetails.sOilCapacity = dtDetails.Rows[0]["TC_OIL_CAPACITY"].ToString();
                        objFailureDetails.sTankcapacity = dtDetails.Rows[0]["TC_TANK_CAPACITY"].ToString();

                        if (objFailureDetails.sEnhancedCapacity == null || objFailureDetails.sEnhancedCapacity == "")
                        {
                            objFailureDetails.sEnhancedCapacity = objFailureDetails.sDtcCapacity;
                        }

                        GetLastRepairedDetails(objFailureDetails);
                        GetDTRCommissionDate(objFailureDetails);
                        GetEnumerationDate(objFailureDetails);

                    }

                    return objFailureDetails;
                }


            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objFailureDetails;
            }
        }
        /// <summary>
        /// This Method used to get the dtc failure details
        /// </summary>
        /// <param name="objFailure"></param>
        /// <returns></returns>
        public DataTable LoadAllDTCFailure(clsFailureEntry objFailure)
        {
            oledbCommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {
                string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO,";
                strQry += "  (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID) TM_NAME, ";
                strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,DF_ID, 'YES' AS STATUS,DT_PROJECTTYPE ";
                strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE AND ";
                strQry += "  DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG IN ('1','4')  ";
                strQry += " AND DF_LOC_CODE LIKE:sOfficeCode||'%'";
                strQry += " UNION ALL";
                strQry += " SELECT DT_ID, TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, ";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID) TM_NAME, ";
                strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY, 0 AS DF_ID,'NO' AS STATUS,DT_PROJECTTYPE ";
                strQry += " FROM TBLDTCMAST,TBLTCMASTER WHERE DT_TC_ID = TC_CODE AND ";
                strQry += " DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0 ) ";
                strQry += " AND DT_OM_SLNO LIKE:OfficeCode||'%' AND ROWNUM < 10000 ORDER BY STATUS ";

                oledbCommand.Parameters.AddWithValue("sOfficeCode", objFailure.sOfficeCode);
                oledbCommand.Parameters.AddWithValue("OfficeCode", objFailure.sOfficeCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtDetails;

            }
        }

        /// <summary>
        /// This method used to get the already failured records
        /// </summary>
        /// <param name="objFailure"></param>
        /// <returns></returns>
        public DataTable LoadAlreadyFailure(clsFailureEntry objFailure)
        {
            oledbCommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {
                string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DF_ID,DT_NAME,TC_SLNO,";
                strQry += " TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY,'YES' AS STATUS,DT_PROJECTTYPE ";
                strQry += " FROM TBLDTCMAST,TBLTCMASTER,";
                strQry += "TBLTRANSMAKES,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE AND ";
                strQry += "TM_ID = TC_MAKE_ID AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND ";
                strQry += " DF_STATUS_FLAG IN ('1','4') AND DF_LOC_CODE LIKE:OfficeCode||'%' ORDER BY DF_ID DESC";
                oledbCommand.Parameters.AddWithValue("OfficeCode", objFailure.sOfficeCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtDetails;

            }
        }
        
        /// <summary>
        /// This method used to get the failure details
        /// </summary>
        /// <param name="objFailureDetails"></param>
        /// <returns></returns>
        public object GetFailureDetails(clsFailureEntry objFailureDetails)
        {
            oledbCommand = new OleDbCommand();

            DataTable dtDetails = new DataTable();
            OleDbDataReader dr = null;

            try
            {
                if (objFailureDetails.sFailureId != "0")
                {
                    string strQry = "SELECT DF_ID,DT_ID, DF_DTC_CODE, DF_GUARANTY_TYPE, DT_NAME,";
                    strQry += " TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP,";
                    strQry += " TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    strQry += "(SELECT DISTINCT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO  ";
                    strQry += " )AS TC_LOCATION_ID,(SELECT SD_SUBDIV_NAME  FROM TBLSUBDIVMAST WHERE ";
                    strQry += " SUBSTR(DT_OM_SLNO,0,3) =SD_SUBDIV_CODE  ) AS DIVISION ,TC_SLNO, ";
                    strQry += "(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,TC_CODE,";
                    strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(DF_DATE,'DD/MM/YYYY') DF_FAILED_DATE, ";
                    strQry += " DF_REASON,TO_CHAR(DF_KWH_READING)DF_KWH_READING,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE DF_CRBY=US_ID) US_FULL_NAME,DF_LOC_CODE,";
                    strQry += " TC_ID,DF_FAILURE_TYPE,DF_HT_BUSING,DF_LT_BUSING,DF_HT_BUSING_ROD,DF_LT_BUSING_ROD, ";
                    strQry += " DF_BREATHER,DF_OIL_LEVEL,DF_DRAIN_VALVE,DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,";
                    strQry += " DF_NUMBER_OF_INSTALLATIONS,DF_OIL_QNTY,DF_TANK_CONDITION,DF_WHEEL,DF_EXPLOSION,";
                    strQry += " DF_OIL_CAPACITY,TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY') TC_MANF_DATE,DF_ENHANCE_CAPACITY,";
                    strQry += " TO_CHAR( DF_DTR_COMMISSION_DATE,'dd/MM/yyyy')DF_DTR_COMMISSION_DATE , TC_STAR_RATE ";
                    strQry += " ,DT_LATITUDE,DT_LONGITUDE,TC_OIL_CAPACITY,TC_TANK_CAPACITY ";
                    strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_DTC_CODE=DT_CODE AND ";
                    strQry += " DF_DTC_CODE=DT_CODE AND DF_EQUIPMENT_ID=TC_CODE AND DF_ID=:FailureId";
                    oledbCommand.Parameters.AddWithValue("FailureId", objFailureDetails.sFailureId);
                    dr = ObjCon.Fetch(strQry, oledbCommand);
                    dtDetails.Load(dr);

                    if (dtDetails.Rows.Count > 0)
                    {
                        objFailureDetails.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objFailureDetails.sDtcCode = dtDetails.Rows[0]["DF_DTC_CODE"].ToString();
                        objFailureDetails.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objFailureDetails.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objFailureDetails.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objFailureDetails.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objFailureDetails.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objFailureDetails.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objFailureDetails.sEnhancedCapacity = dtDetails.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();
                        objFailureDetails.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objFailureDetails.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objFailureDetails.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objFailureDetails.sFailureDate = dtDetails.Rows[0]["DF_FAILED_DATE"].ToString();
                        objFailureDetails.sFailureReasure = dtDetails.Rows[0]["DF_REASON"].ToString();
                        objFailureDetails.sDtcReadings = dtDetails.Rows[0]["DF_KWH_READING"].ToString();
                        objFailureDetails.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objFailureDetails.sFailureId = dtDetails.Rows[0]["DF_ID"].ToString();
                        objFailureDetails.sCrby = dtDetails.Rows[0]["US_FULL_NAME"].ToString();
                        objFailureDetails.sOfficeCode = dtDetails.Rows[0]["DF_LOC_CODE"].ToString();
                        objFailureDetails.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objFailureDetails.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objFailureDetails.sDTrCommissionDate = dtDetails.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString();

                        objFailureDetails.sFailureType = dtDetails.Rows[0]["DF_FAILURE_TYPE"].ToString();
                        objFailureDetails.sHTBusing = dtDetails.Rows[0]["DF_HT_BUSING"].ToString();
                        objFailureDetails.sLTBusing = dtDetails.Rows[0]["DF_LT_BUSING"].ToString();
                        objFailureDetails.sHTBusingRod = dtDetails.Rows[0]["DF_HT_BUSING_ROD"].ToString();
                        objFailureDetails.sLTBusingRod = dtDetails.Rows[0]["DF_LT_BUSING_ROD"].ToString();
                        objFailureDetails.sBreather = dtDetails.Rows[0]["DF_BREATHER"].ToString();
                        objFailureDetails.sOilLevel = dtDetails.Rows[0]["DF_OIL_LEVEL"].ToString();
                        objFailureDetails.sDrainValve = dtDetails.Rows[0]["DF_DRAIN_VALVE"].ToString();
                        objFailureDetails.sOilQuantity = dtDetails.Rows[0]["DF_OIL_QNTY"].ToString();
                        objFailureDetails.sTankCondition = dtDetails.Rows[0]["DF_TANK_CONDITION"].ToString();
                        objFailureDetails.sWheel = dtDetails.Rows[0]["DF_WHEEL"].ToString();
                        objFailureDetails.sExplosionValve = dtDetails.Rows[0]["DF_EXPLOSION"].ToString();
                        objFailureDetails.sOilQuantity = dtDetails.Rows[0]["DF_OIL_CAPACITY"].ToString();
                        objFailureDetails.sEnhancedCapacity = Convert.ToString(dtDetails.Rows[0]["DF_ENHANCE_CAPACITY"]).Trim();
                        objFailureDetails.sSubDivName = Convert.ToString(dtDetails.Rows[0]["DIVISION"]).Trim();
                        objFailureDetails.sGuarantyType = Convert.ToString(dtDetails.Rows[0]["DF_GUARANTY_TYPE"]).Trim();

                        objFailureDetails.sCustomerName = dtDetails.Rows[0]["DF_CUSTOMER_NAME"].ToString();
                        objFailureDetails.sCustomerNumber = dtDetails.Rows[0]["DF_CUSTOMER_MOBILE"].ToString();
                        objFailureDetails.sNumberOfInstalments = dtDetails.Rows[0]["DF_NUMBER_OF_INSTALLATIONS"].ToString();

                        objFailureDetails.sDTRStarRating = Convert.ToString(dtDetails.Rows[0]["TC_STAR_RATE"]);
                        objFailureDetails.Latitude = dtDetails.Rows[0]["DT_LATITUDE"].ToString();
                        objFailureDetails.Longitude = dtDetails.Rows[0]["DT_LONGITUDE"].ToString();
                        objFailureDetails.sOilCapacity = dtDetails.Rows[0]["TC_OIL_CAPACITY"].ToString();
                        objFailureDetails.sTankcapacity = dtDetails.Rows[0]["TC_TANK_CAPACITY"].ToString();

                        GetLastRepairedDetails(objFailureDetails);

                    }

                    return objFailureDetails;
                }

                else
                {
                    String strQry = "SELECT  DT_ID,DT_NAME,DT_CODE,";
                    strQry += " TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP, ";
                    strQry += " TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO ) AS TC_LOCATION_ID ,";
                    strQry += " TC_ID,TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,";
                    strQry += " TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY') TC_MANF_DATE,";
                    strQry += "  TC_STAR_RATE,DT_LATITUDE,DT_LONGITUDE,TC_OIL_CAPACITY,TC_TANK_CAPACITY ";
                    strQry += " FROM TBLDTCMAST,TBLTCMASTER WHERE  DT_TC_ID=TC_CODE AND DT_ID=:DtcId";
                    oledbCommand.Parameters.AddWithValue("DtcId", objFailureDetails.sDtcId);
                    dr = ObjCon.Fetch(strQry, oledbCommand);
                    dtDetails.Load(dr);


                    if (dtDetails.Rows.Count > 0)
                    {
                        objFailureDetails.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objFailureDetails.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objFailureDetails.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objFailureDetails.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objFailureDetails.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objFailureDetails.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objFailureDetails.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objFailureDetails.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objFailureDetails.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objFailureDetails.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objFailureDetails.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objFailureDetails.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objFailureDetails.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objFailureDetails.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objFailureDetails.Latitude = dtDetails.Rows[0]["DT_LATITUDE"].ToString();
                        objFailureDetails.Longitude = dtDetails.Rows[0]["DT_LONGITUDE"].ToString();
                        objFailureDetails.sOilCapacity = dtDetails.Rows[0]["TC_OIL_CAPACITY"].ToString();
                        objFailureDetails.sTankcapacity = dtDetails.Rows[0]["TC_TANK_CAPACITY"].ToString();

                        objFailureDetails.sDTRStarRating = Convert.ToString(dtDetails.Rows[0]["TC_STAR_RATE"]);

                        if (objFailureDetails.sEnhancedCapacity == null || objFailureDetails.sEnhancedCapacity == "")
                        {
                            objFailureDetails.sEnhancedCapacity = objFailureDetails.sDtcCapacity;
                        }

                        GetLastRepairedDetails(objFailureDetails);
                        GetDTRCommissionDate(objFailureDetails);
                        GetEnumerationDate(objFailureDetails);

                    }

                    return objFailureDetails;
                }
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objFailureDetails;
            }
        }
        /// <summary>
        /// This method used get the enumeration date
        /// </summary>
        /// <param name="objFailureDetails"></param>
        /// <returns></returns>
        public clsFailureEntry GetEnumerationDate(clsFailureEntry objFailureDetails)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT TO_CHAR(ED_APPROVED_ON,'DD/MM/YYYY') ED_APPROVED_ON FROM TBLENUMERATIONDETAILS,";
                strQry += " TBLDTCENUMERATION WHERE ED_ID=DTE_ED_ID AND DTE_TC_CODE=:DtcTcCode AND DTE_DTCCODE=:DtcCode";
                oledbCommand.Parameters.AddWithValue("DtcTcCode", objFailureDetails.sDtcTcCode);
                oledbCommand.Parameters.AddWithValue("DtcCode", objFailureDetails.sDtcCode);
                string sResult = ObjCon.get_value(strQry, oledbCommand);
                objFailureDetails.sDTrEnumerationDate = sResult;
                return objFailureDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objFailureDetails;
            }
        }
        /// <summary>
        /// This method used to get the last repairer details
        /// </summary>
        /// <param name="objFailureDetails"></param>
        /// <returns></returns>
        public clsFailureEntry GetLastRepairedDetails(clsFailureEntry objFailureDetails)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                int sRepairerDate = 0;
                oledbCommand = new OleDbCommand();
                strQry = "SELECT * FROM ( SELECT TO_CHAR(RSD_RV_DATE,'dd-MM-yyyy') RSD_RV_DATE, ";
                strQry += " TO_CHAR(RSD_DELIVARY_DATE,'dd-MM-yyyy') RSD_DELIVARY_DATE,CASE RSM_SUPREP_TYPE WHEN 1 THEN ";
                strQry += " (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID=RSM_SUPREP_ID) ";
                strQry += " WHEN 2 THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID=RSM_SUPREP_ID) ";
                strQry += " ELSE '' END SUP_REPNAME,RSM_GUARANTY_TYPE FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS";
                strQry += "  WHERE RSM_ID=RSD_RSM_ID AND RSD_TC_CODE=:sDtcTcCode AND RSD_RV_DATE IS NOT NULL ";
                strQry += " ORDER BY RSM_ID DESC) WHERE ROWNUM=1";
                oledbCommand.Parameters.AddWithValue("sDtcTcCode", objFailureDetails.sDtcTcCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    DateTime WarantyCommDate = DateTime.ParseExact(Convert.ToString(dt.Rows[0]["RSD_RV_DATE"]).Replace('-', '/'), 
                        "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    sRepairerDate = WarantyCommDate.Year;
                    if (sRepairerDate < 2000)
                    {
                        objFailureDetails.sLastRepairedDate = Convert.ToString(dt.Rows[0]["RSD_DELIVARY_DATE"]);
                        objFailureDetails.sLastRepairedBy = Convert.ToString(dt.Rows[0]["SUP_REPNAME"]);
                    }
                    else
                    {
                        objFailureDetails.sLastRepairedDate = Convert.ToString(dt.Rows[0]["RSD_RV_DATE"]);
                        objFailureDetails.sLastRepairedBy = Convert.ToString(dt.Rows[0]["SUP_REPNAME"]);
                    }
                    // CODED BY RUDRA 
                    #region
                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT TD_INV_NO FROM TBLTCDRAWN WHERE TD_TC_NO=:sDtcTcCode order by TD_ID DESC";
                    oledbCommand.Parameters.AddWithValue("sDtcTcCode", objFailureDetails.sDtcTcCode);
                    string InvoicedNo = ObjCon.get_value(strQry, oledbCommand);
                    if (InvoicedNo != "")
                    {
                        oledbCommand = new OleDbCommand();
                        strQry = "select TO_CHAR(IN_DATE,'DD/MM/YYYY') IN_DATE from TBLDTCINVOICE where IN_NO=:InvoiceNo ";
                        oledbCommand.Parameters.AddWithValue("InvoiceNo", InvoicedNo);
                        objFailureDetails.sTcInvoiceDate = ObjCon.get_value(strQry, oledbCommand);
                    }
                    #endregion
                }

                return objFailureDetails;
                #region this has to be commented since we are not checking the guaranty type here
                //or take the quaranty type from tbldtcfailure 
                if (objFailureDetails.sFailureId == "0")
                {
                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT TC_WARANTY_PERIOD FROM TBLTCMASTER WHERE TC_CODE=:sDtcTcCodes";
                    oledbCommand.Parameters.AddWithValue("sDtcTcCodes", objFailureDetails.sDtcTcCode);
                    string val = ObjCon.get_value(strQry, oledbCommand);

                    if (val != "")
                    {
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN RSD_GUARRENTY_TYPE ";
                        strQry += " ELSE 'AGP' END FROM TBLTCMASTER,TBLREPAIRSENTDETAILS WHERE ";
                        strQry += " TC_CODE=RSD_TC_CODE AND TC_CODE=:sDtcTcCodesid AND ";
                        strQry += " RSD_GUARRENTY_TYPE IS NOT NULL AND RSD_WARENTY_PERIOD IS NOT NULL";
                        oledbCommand.Parameters.AddWithValue("sDtcTcCodesid", objFailureDetails.sDtcTcCode);
                        objFailureDetails.sGuarantyType = ObjCon.get_value(strQry, oledbCommand);

                        if (objFailureDetails.sGuarantyType == "" || objFailureDetails.sGuarantyType == null)
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = "SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN 'WGP' ";
                            strQry += "    ELSE 'AGP' END FROM TBLTCMASTER WHERE TC_CODE=:DtcTcCod";
                            oledbCommand.Parameters.AddWithValue("DtcTcCod", objFailureDetails.sDtcTcCode);
                            objFailureDetails.sGuarantyType = ObjCon.get_value(strQry, oledbCommand);
                        }
                    }
                    if (objFailureDetails.sGuarantyType == "" || objFailureDetails.sGuarantyType == null)
                    {
                        objFailureDetails.sGuarantyType = sFirstGuarantyType;
                    }
                }
                else
                {
                    // this concept is wrong since the guaranty type is being taken from all the tabls instead of TBLDTCFAILURE .
                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_DF_ID=:sFailureId order by TD_ID DESC";
                    oledbCommand.Parameters.AddWithValue("sFailureId", objFailureDetails.sFailureId);
                    string InvoicedTC = ObjCon.get_value(strQry, oledbCommand);

                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT TC_WARANTY_PERIOD FROM TBLTCMASTER WHERE TC_CODE=:InvoicedTC";
                    oledbCommand.Parameters.AddWithValue("InvoicedTC", InvoicedTC);
                    string val = ObjCon.get_value(strQry, oledbCommand);

                    if (val != "")
                    {
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN 'WRGP' ELSE 'AGP' ";
                        strQry += " END FROM TBLTCMASTER,TBLREPAIRSENTDETAILS WHERE ";
                        strQry += " TC_CODE=RSD_TC_CODE AND TC_CODE=:InvoicedTCs AND ";
                        strQry += " RSD_GUARRENTY_TYPE IS NOT NULL AND RSD_WARENTY_PERIOD IS NOT NULL";
                        oledbCommand.Parameters.AddWithValue("InvoicedTCs", InvoicedTC);
                        objFailureDetails.sGuarantyType = ObjCon.get_value(strQry, oledbCommand);

                        if (objFailureDetails.sGuarantyType == "" || objFailureDetails.sGuarantyType == null)
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = "SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN 'WGP' ";
                            strQry += " ELSE 'AGP' END FROM TBLTCMASTER WHERE TC_CODE=:InvoicedTCid";
                            oledbCommand.Parameters.AddWithValue("InvoicedTCid", InvoicedTC);
                            objFailureDetails.sGuarantyType = ObjCon.get_value(strQry, oledbCommand);
                        }
                    }
                    if (objFailureDetails.sGuarantyType == "" || objFailureDetails.sGuarantyType == null)
                    {
                        objFailureDetails.sGuarantyType = sFirstGuarantyType;
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);

                return objFailureDetails;
            }
        }
        /// <summary>
        /// This method used to get the existing failure id
        /// </summary>
        /// <param name="strFailureId"></param>
        /// <returns></returns>
        public bool ValidateUpdate(string strFailureId)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                OleDbDataReader dr;
                DataTable dt = new DataTable();
                string qry = "select WO_DF_ID,DF_ID from TBLWORKORDER,TBLDTCFAILURE ";
                qry += " WHERE WO_DF_ID=DF_ID AND DF_ID=:strFailureId";
                oledbCommand.Parameters.AddWithValue("strFailureId", strFailureId);
                dr = ObjCon.Fetch(qry, oledbCommand);
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        /// <summary>
        /// This method used to get dtr commission date
        /// </summary>
        /// <param name="objFailure"></param>
        /// <returns></returns>
        public clsFailureEntry GetDTRCommissionDate(clsFailureEntry objFailure)
        {
            DataTable dt;
            string sDtcCode = string.Empty;
            // code updated by Rudra 
            // Date  :  13-Feb-2020
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                string sResult = string.Empty;
             
                // ************* START **************** // 
                // if the dtr has been repair good than the last invoiced date has to be taken as the dtrcommission date 
                strQry = "SELECT DF_DTC_CODE FROM  TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = '" 
                    + objFailure.sDtcTcCode + "' AND DF_STATUS_FLAG= 2 ORDER BY DF_ID  desc ";
                dt = ObjCon.getDataTable(strQry);

                if (dt.Rows.Count > 0)
                {
                    sDtcCode = Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]);
                    strQry = "SELECT TO_CHAR(IN_DATE,'dd/MM/yyyy') IN_DATE  FROM TBLTCDRAWN , ";
                    strQry += " TBLDTCINVOICE , TBLDTCFAILURE   WHERE IN_NO  =TD_INV_NO  and  DF_ID = TD_DF_ID and";
                    strQry += " DF_DTC_CODE = '" + sDtcCode + "' and DF_STATUS_FLAG <> 2  ORDER BY DF_ID DESC ";

                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        sResult = Convert.ToString(dt.Rows[0]["IN_DATE"]);
                        objFailure.sDTrCommissionDate = sResult;

                        return objFailure;
                    }
                    else
                    {
                        strQry = "SELECT to_char(TM_MAPPING_DATE , 'dd/MM/yyyy') TM_MAPPING_DATE  FROM ";
                        strQry += " TBLTRANSDTCMAPPING WHERE TM_DTC_ID = '" + sDtcCode + "' and ";
                        strQry += " TM_TC_ID =  '" + objFailure.sDtcTcCode + "' and TM_UNMAP_REASON = 'FROM FAILURE ENTRY' ";
                        objFailure.sDTrCommissionDate = ObjCon.get_value(strQry);
                        if (!(objFailure.sDTrCommissionDate.Length == 0))
                        {
                            return objFailure;
                        }
                    }
                }
                // ************* END **************** // 

                strQry = " SELECT TO_CHAR(IN_DATE,'dd/MM/yyyy') IN_DATE FROM TBLDTCINVOICE,TBLTCDRAWN ";
                strQry += " WHERE TD_INV_NO = IN_NO AND TD_TC_NO = '" + objFailure.sDtcTcCode + "' ORDER BY TD_ID DESC ";
                sResult = ObjCon.get_value(strQry);

                if (sResult.Length == 0)
                {
                    strQry = "SELECT TO_CHAR(TM_MAPPING_DATE,'dd/MM/yyyy') TM_MAPPING_DATE FROM ";
                    strQry += " TBLTRANSDTCMAPPING WHERE TM_TC_ID= '" + objFailure.sDtcTcCode + "' ";
                    strQry += " AND TM_DTC_ID= '" + objFailure.sDtcCode + "' AND TM_LIVE_FLAG='1'";
                    sResult = ObjCon.get_value(strQry);
                }
                objFailure.sDTrCommissionDate = sResult;
                return objFailure;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objFailure;
            }
        }
        /// <summary>
        /// This method used to send sms to section officer
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <param name="sDTCCode"></param>
        public void SendSMStoSectionOfficer(string sOfficeCode, string sDTCCode)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                if (sOfficeCode.Length > 2)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }

                strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER ";
                strQry += " WHERE US_ROLE_ID IN (7) AND US_OFFICE_CODE=:sOfficeCode";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objComm = new clsCommunication();

                    objComm.sSMSkey = "SMStoFailure";
                    objComm = objComm.GetsmsTempalte(objComm);
                    string sSMSText = String.Format(objComm.sSMSTemplate, sDTCCode);
                    objComm.DumpSms(sMobileNo, sSMSText, objComm.sSMSTemplateID);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to send sms to sub division officer
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <param name="sDTCCode"></param>

        public void SendSMStoSDO(string sOfficeCode, string sDTCCode)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                if (sOfficeCode.Length > 3)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 3);
                }

                strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER ";
                strQry += " WHERE US_ROLE_ID IN (1) AND US_OFFICE_CODE=:sOfficeCode";
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objCommunication = new clsCommunication();

                    string sSMSText = String.Format(Convert.ToString
                        (ConfigurationManager.AppSettings["SMStoFailureCreate"]), sDTCCode);

                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        #region WorkFlow XML
        /// <summary>
        /// This method used to get the failure details from xml
        /// </summary>
        /// <param name="objFailure"></param>
        /// <returns></returns>
        public clsFailureEntry GetFailureDetailsFromXML(clsFailureEntry objFailure)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if (objFailure.sWFDataId != "" && objFailure.sWFDataId != null)
                {
                    dt = objApproval.GetDatatableFromXML(objFailure.sWFDataId);
                }

                if (dt.Rows.Count > 0)
                {
                    objFailure.sFailureDate = Convert.ToString(dt.Rows[0]["DF_DATE"]).Trim();
                    objFailure.sFailureReasure = Convert.ToString(dt.Rows[0]["DF_REASON"]).Trim().Replace("ç", ",");
                    objFailure.sDtcCode = Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]).Trim();
                    objFailure.sOfficeCode = Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]);
                    objFailure.sCrby = Convert.ToString(dt.Rows[0]["DF_CRBY"]);

                    objFailure.sFailureType = Convert.ToString(dt.Rows[0]["DF_FAILURE_TYPE"]).Trim();
                    objFailure.sHTBusing = Convert.ToString(dt.Rows[0]["DF_HT_BUSING"]).Trim();
                    objFailure.sLTBusing = Convert.ToString(dt.Rows[0]["DF_LT_BUSING"]).Trim();
                    objFailure.sHTBusingRod = Convert.ToString(dt.Rows[0]["DF_HT_BUSING_ROD"]).Trim();
                    objFailure.sLTBusingRod = Convert.ToString(dt.Rows[0]["DF_LT_BUSING_ROD"]).Trim();
                    objFailure.sBreather = Convert.ToString(dt.Rows[0]["DF_BREATHER"]).Trim();
                    objFailure.sOilLevel = Convert.ToString(dt.Rows[0]["DF_OIL_LEVEL"]).Trim();
                    objFailure.sDrainValve = Convert.ToString(dt.Rows[0]["DF_DRAIN_VALVE"]).Trim();
                    objFailure.sOilQuantity = Convert.ToString(dt.Rows[0]["DF_OIL_QNTY"]).Trim();
                    objFailure.sTankCondition = Convert.ToString(dt.Rows[0]["DF_TANK_CONDITION"]).Trim();
                    objFailure.sExplosionValve = Convert.ToString(dt.Rows[0]["DF_EXPLOSION"]).Trim();
                    objFailure.sDtcReadings = Convert.ToString(dt.Rows[0]["DF_KWH_READING"]);

                    if(dt.Columns.Contains("DF_DTR_STAR_RATING"))
                    {
                        objFailure.sDTRStarRating = Convert.ToString(dt.Rows[0]["DF_DTR_STAR_RATING"]);
                    }

                    if ((dt.Columns.Contains("DF_CUSTOMER_NAME") && dt.Columns.Contains("DF_CUSTOMER_MOBILE"))
                        && dt.Columns.Contains("DF_NUMBER_OF_INSTALLATIONS"))
                    {
                        objFailure.sCustomerName = Convert.ToString(dt.Rows[0]["DF_CUSTOMER_NAME"]);
                        objFailure.sCustomerNumber = Convert.ToString(dt.Rows[0]["DF_CUSTOMER_MOBILE"]);
                        objFailure.sNumberOfInstalments = Convert.ToString(dt.Rows[0]["DF_NUMBER_OF_INSTALLATIONS"]);
                    }
                    else
                    {
                        objFailure.sCustomerName = "";
                        objFailure.sCustomerNumber = "";
                        objFailure.sNumberOfInstalments = "";
                    }

                    if (dt.Columns.Contains("GUARENTEE"))
                    {
                        sFirstGuarantyType = Convert.ToString(dt.Rows[0]["GUARENTEE"]);
                    }
                    if (dt.Columns.Contains("DF_GUARANTY_TYPE_SOURCE"))
                    {
                        objFailure.sGuarantySource = Convert.ToString(dt.Rows[0]["DF_GUARANTY_TYPE_SOURCE"]);
                    }
                    else
                    {
                        objFailure.sGuarantySource = "";
                    }
                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                        objFailure.sEnhancedCapacity = Convert.ToString(dt.Rows[0]["DF_ENHANCE_CAPACITY"]);

                    objFailure.sFailureId = "0";
                    string qry = "SELECT DT_ID FROM TBLDTCMAST WHERE DT_CODE=:sDtcCode";
                    oledbCommand.Parameters.AddWithValue("sDtcCode", objFailure.sDtcCode);
                    objFailure.sDtcId = ObjCon.get_value(qry, oledbCommand);
                    GetFailureDetails(objFailure);
                }
                return objFailure;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name);
                return objFailure;
            }
        }

        #endregion
        /// <summary>
        /// It will get the wo id for estimation
        /// </summary>
        /// <param name="sOffCode"></param>
        /// <param name="sDtcCode"></param>
        /// <returns></returns>
        public string getWoIDforEstimation(string sOffCode, string sDtcCode)
        {
            oledbCommand = new OleDbCommand();
            string sWoID = string.Empty;
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_OFFICE_CODE=:sOffCode ";
                sQry += " AND WO_NEXT_ROLE=1 AND WO_DATA_ID=:sDtcCode ";
                oledbCommand.Parameters.AddWithValue("sOffCode", sOffCode);
                oledbCommand.Parameters.AddWithValue("sDtcCode", sDtcCode);
                sWoID = ObjCon.get_value(sQry, oledbCommand);
                return sWoID;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name);
                return sWoID;
            }
        }
        /// <summary>
        /// It will get the wfo id for estimation so
        /// </summary>
        /// <param name="sWOId"></param>
        /// <returns></returns>
        public string getWfoIDforEstimationSO(string sWOId)
        {
            oledbCommand = new OleDbCommand();
            string sWFOID = string.Empty;
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID=:sWOId ";
                oledbCommand.Parameters.AddWithValue("sWOId", sWOId);
                sWFOID = ObjCon.get_value(sQry, oledbCommand);
                return sWFOID;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return sWFOID;
            }
        }

    }
}
