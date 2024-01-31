using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsDTCCommision
    {
        string strFormCode = "clsDTCCommision";
        public long lGetMaxMap { get; set; }
        public string lDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sOMSectionName { get; set; }
        public string iConnectedKW { get; set; }
        public string iConnectedHP { get; set; }
        public string sInternalCode { get; set; }
        public string sPlatformType { get; set; }
        public string sConnectionDate { get; set; }
        public string sInspectionDate { get; set; }
        public string sServiceDate { get; set; }
        public string sCommisionDate { get; set; }
        public string sFeederChangeDate { get; set; }
        public string iKWHReading { get; set; }
        public string sTCMakeName { get; set; }
        public string sTCCapacity { get; set; }
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }
        public string sOldTcCode { get; set; }
        public string sCrBy { get; set; }
        public string sHtlinelength { get; set; }
        public string sLtlinelength { get; set; }
        public string sArresters { get; set; }
        public string sGrounding { get; set; }
        public string sHTProtect { get; set; }
        public string sLTProtect { get; set; }
        public string sDTCMeters { get; set; }
        public string sBreakertype { get; set; }
        public string sLatitude { get; set; }
        public string sLongitude { get; set; }
        public string sProjecttype { get; set; }
        public string sLoadtype { get; set; }
        public string sDepreciation { get; set; }

        public string sWOslno { get; set; }
        public string sOfficeCode { get; set; }
        public string sDTCPath { get; set; }
        public string sDTCImagePath { get; set; }
        public string sDTrImagePath { get; set; }
        public string sDTrNamePlate { get; set; }
        public string sFeedercode { get; set; }





        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }


        public string sColumnNames { get; set; }
        public string sColumnValues { get; set; }
        public string sTableNames { get; set; }

        public string sQryValues { get; set; }
        public string sDescription { get; set; }
        public string sParameterValues { get; set; }

        public string sWFDataId { get; set; }
        public string sXmlData { get; set; }

        public string sBOId { get; set; }
        public string MeterStatus { get; set; }

        public string Ctratio { get; set; }
        public string Modem { get; set; }
        public string Ltstatus { get; set; }
        public string Wiring { get; set; }
        public string RoleId { get; set; }
        public string Metermake { get; set; }
        public string Meterslno { get; set; }
        public string Manufactureyear { get; set; }
        public string Remarks { get; set; }

        public string Meterrecording { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbcommand;
        /// <summary>
        ///  this method used to save the dtc details
        /// </summary>
        /// <param name="objDtcMaster"></param>
        /// <returns></returns>
        public string[] SaveUpdateDtcDetails(clsDTCCommision objDtcMaster)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATHDEV"]) + DateTime.Now.ToString("yyyyMM");
            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);

            }
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";

            string[] Arr = new string[2];

            try
            {

                OleDbDataReader dr;
                string strQry = string.Empty;
                oledbcommand = new OleDbCommand();

                if (objDtcMaster.sOfficeCode.Length >= 4)
                {
                    objDtcMaster.sOfficeCode = objDtcMaster.sOfficeCode.Substring(0, 3);
                }

                oledbcommand.Parameters.AddWithValue("FeederCode", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                oledbcommand.Parameters.AddWithValue("OfficeCode", objDtcMaster.sOfficeCode);
                strQry = "select * from TBLFEEDERMAST,TBLFEEDEROFFCODE WHERE FD_FEEDER_CODE= :FeederCode";
                strQry += " AND  FD_FEEDER_ID=FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE :OfficeCode||'%'";
                dr = ObjCon.Fetch(strQry, oledbcommand);
                if (!dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Code Does Not Match With The Feeder Code";
                    Arr[1] = "2";
                    return Arr;

                }
                dr.Close();
                if (objDtcMaster.lDtcId == "" || objDtcMaster.lDtcId == null)
                {
                    dr = ObjCon.Fetch("select DT_CODE from TBLDTCMAST where DT_CODE='" + objDtcMaster.sDtcCode + "'");
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "DTC Code Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();


                    dr = ObjCon.Fetch("select * from TBLOMSECMAST where OM_CODE='" + objDtcMaster.sOMSectionName + "'");
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid O&M Section Code ";
                        Arr[1] = "2";
                        return Arr;

                    }
                    dr.Close();
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE='" + objDtcMaster.sTcCode + "'");
                    if (!dr.Read())
                    {

                        dr.Close();
                        Arr[0] = "Enter Valid TC Code ";
                        Arr[1] = "2";
                        return Arr;

                    }
                    dr.Close();

                    ObjCon.BeginTrans();
                    objDtcMaster.lDtcId = Convert.ToString(ObjCon.Get_max_no("DT_ID", "TBLDTCMAST"));

                    string Qry = "SELECT FD_FEEDER_ID FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE='"
                        + objDtcMaster.sDtcCode.ToString().Substring(0, 4) + "'";
                    string strFeederSlno = ObjCon.get_value(Qry);

                    strQry = "Insert into TBLDTCMAST (DT_ID,DT_CODE,DT_NAME,DT_OM_SLNO,DT_TOTAL_CON_KW,";
                    strQry += " DT_TOTAL_CON_HP,DT_KWH_READING,DT_INTERNAL_CODE,DT_TC_ID,DT_CON_DATE,";
                    strQry += " DT_LAST_SERVICE_DATE,DT_TRANS_COMMISION_DATE,DT_FDRCHANGE_DATE,DT_FDRSLNO,";
                    strQry += " DT_CRBY,DT_CRON,DT_WO_ID,DT_PROJECTTYPE) VALUES ('" + objDtcMaster.lDtcId + "',";
                    strQry += " '" + objDtcMaster.sDtcCode + "','" + objDtcMaster.sDtcName + "','"
                        + objDtcMaster.sOMSectionName + "','" + objDtcMaster.iConnectedKW + "',";
                    strQry += "'" + objDtcMaster.iConnectedHP + "','" + objDtcMaster.iKWHReading + "',";
                    strQry += " '" + objDtcMaster.sInternalCode + "','" + objDtcMaster.sTcCode
                        + "',TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),";
                    strQry += " TO_DATE('" + objDtcMaster.sServiceDate + "','dd/MM/yyyy'),";
                    strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),TO_DATE('"
                        + objDtcMaster.sFeederChangeDate + "','dd/MM/yyyy'), ";
                    strQry += " '" + objDtcMaster.sDtcCode.ToString().Substring(0, 4) + "','"
                        + objDtcMaster.sCrBy + "',SYSDATE,'" + objDtcMaster.sWOslno + "','"
                        + objDtcMaster.sProjecttype + "' )";
                    ObjCon.Execute(strQry);

                    objDtcMaster.lGetMaxMap = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                    strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_TC_ID,TM_DTC_ID,";
                    strQry += "TM_MAPPING_DATE,TM_CRBY)VALUES('" + objDtcMaster.lGetMaxMap + "',";
                    strQry += " '" + objDtcMaster.sTcCode.ToUpper() + "','" + objDtcMaster.sDtcCode + "',";
                    strQry += " SYSDATE,'" + objDtcMaster.sCrBy + "')";
                    ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLTCMASTER set TC_UPDATED_EVENT='DTC MASTER ENTRY', ";
                    strQry += " TC_UPDATED_EVENT_ID='" + lGetMaxMap + "', TC_CURRENT_LOCATION=2, TC_LOCATION_ID='"
                        + objDtcMaster.sOMSectionName + "' where TC_CODE='" + objDtcMaster.sTcCode.ToUpper() + "'";
                    ObjCon.Execute(strQry);


                    strQry = "UPDATE TBLWORKORDER SET WO_REPLACE_FLG='1' WHERE WO_SLNO='" + objDtcMaster.sWOslno + "'";
                    ObjCon.Execute(strQry);
                    if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                    {
                        DateTime fromdate = Convert.ToDateTime(objDtcMaster.sCommisionDate);
                        objDtcMaster.sCommisionDate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        objDtcMaster.sCommisionDate = "";
                    }
                    if (objDtcMaster.sTCCapacity == null || objDtcMaster.sTCCapacity == "")
                    {
                        objDtcMaster.sTCCapacity = "0";
                    }

                    TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    string result = objWCF.SendDTCDetailstoTrms(objDtcMaster.sDtcName, objDtcMaster.sDtcCode,
                        objDtcMaster.sDtcCode.ToString().Substring(0, 4), objDtcMaster.sLongitude,
                        objDtcMaster.sLatitude, objDtcMaster.sOMSectionName,
                        objDtcMaster.sTCCapacity, objDtcMaster.sCommisionDate, "", "", "", "", "");

                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objDtcMaster.sFormName;
                    objApproval.sRecordId = objDtcMaster.lDtcId;
                    objApproval.sOfficeCode = objDtcMaster.sOfficeCode;
                    objApproval.sClientIp = objDtcMaster.sClientIP;
                    objApproval.sCrby = objDtcMaster.sCrBy;
                    objApproval.sWFObjectId = objDtcMaster.sWFOId;

                    objApproval.sDescription = "Commissioning of DTC " + objDtcMaster.sDtcCode;
                    objApproval.SaveWorkflowObjects(objApproval);
                    ObjCon.CommitTrans();

                    //  objcon.Con.Close();
                    Arr[0] = "DTC Details Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("dtCode1", objDtcMaster.sDtcCode);
                    oledbcommand.Parameters.AddWithValue("dtId1", objDtcMaster.lDtcId);
                    dr = ObjCon.Fetch("select * from TBLDTCMAST where DT_CODE= :dtCode1 AND DT_ID<>:dtId1", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "DTC With This Id  Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("omCode1", objDtcMaster.sOMSectionName);
                    dr = ObjCon.Fetch("select * from TBLOMSECMAST where OM_CODE=:omCode1", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid OM Section Code";
                        Arr[1] = "2";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tcCode1", objDtcMaster.sTcCode);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE=:tcCode1", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid TC Code ";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("DfDtcCode", objDtcMaster.sDtcCode);
                    string Qry = "SELECT * FROM TBLDTCFAILURE WHERE DF_DTC_CODE= :DfDtcCode and DF_REPLACE_FLAG=0";
                    dr = ObjCon.Fetch(Qry, oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Selected DTC Cannot be updated, due to Declared as Failure/Enhancement";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TmDtcCode", objDtcMaster.sDtcCode);
                    OleDbDataReader drMapCondition;
                    drMapCondition = ObjCon.Fetch("SELECT COUNT(*) FROM TBLTRANSDTCMAPPING  WHERE TM_DTC_ID= :TmDtcCode", oledbcommand);
                    if (drMapCondition.Read())
                    {

                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("feedercode2", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                        string strFeederSlno = ObjCon.get_value("SELECT FD_FEEDER_ID FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE= :feedercode2", oledbcommand);
                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("TmTcCode2", objDtcMaster.sTcCode);
                        string strCount = ObjCon.get_value("select count(*) from TBLTRANSDTCMAPPING where TM_TC_ID= :TmTcCode2 and TM_LIVE_FLAG=1", oledbcommand);
                        if (Convert.ToInt32(strCount) <= 1)
                        {
                            ObjCon.BeginTrans();

                            strQry = "UPDATE TBLDTCMAST SET DT_CODE='" + objDtcMaster.sDtcCode + "',DT_NAME ='" + objDtcMaster.sDtcName + "',";
                            strQry += "DT_OM_SLNO='" + objDtcMaster.sOMSectionName + "',DT_TC_ID='" + objDtcMaster.sTcCode + "',DT_INTERNAL_CODE='" + objDtcMaster.sInternalCode + "',";
                            strQry += "DT_KWH_READING='" + objDtcMaster.iKWHReading + "',DT_TOTAL_CON_HP='" + objDtcMaster.iConnectedHP + "',DT_TOTAL_CON_KW='" + objDtcMaster.iConnectedKW + "',";
                            strQry += "DT_LAST_SERVICE_DATE=TO_DATE('" + objDtcMaster.sServiceDate + "','DD/MM/YYYY'),DT_TRANS_COMMISION_DATE=TO_DATE('" + objDtcMaster.sCommisionDate + "','DD/MM/YYYY'),";
                            strQry += "DT_FDRCHANGE_DATE=TO_DATE('" + objDtcMaster.sFeederChangeDate + "','DD/MM/YYYY') ,DT_FDRSLNO='" + objDtcMaster.sDtcCode.ToString().Substring(0, 4) + "', ";
                            strQry += "DT_CON_DATE=TO_DATE('" + objDtcMaster.sConnectionDate + "','DD/MM/YYYY'),DT_PROJECTTYPE='" + objDtcMaster.sProjecttype
                                + "' , DT_UPDATED_ON = SYSDATE , DT_UPDATED_BY = '" + objDtcMaster.sCrBy + "', DT_DTCMETERS='" + objDtcMaster.sDTCMeters + "',";
                            strQry += "DT_METER_STATUS='" + objDtcMaster.MeterStatus + "',DT_CT_RATIO='" + objDtcMaster.Ctratio + "',DT_WIRING='" + objDtcMaster.Wiring
                                + "',DT_MODEM='" + objDtcMaster.Modem + "',DT_LTSTATUS='" + objDtcMaster.Ltstatus
                                + "',DT_METER_SLNO='" + objDtcMaster.Meterslno + "',DT_METER_MAKE='" + objDtcMaster.Metermake
                                + "',DT_METER_RECORDING='" + objDtcMaster.Meterrecording + "',DT_METER_REMARKS='" + objDtcMaster.Remarks
                                + "',DT_MTR_MANUFACTURE_YR='" + objDtcMaster.Manufactureyear + "' WHERE DT_ID='" + objDtcMaster.lDtcId + "'";


                            ObjCon.Execute(strQry);


                            strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_TC_ID='" + objDtcMaster.sTcCode.ToUpper() + "',TM_CRBY='" + objDtcMaster.sCrBy + "'";
                            strQry += "where TM_DTC_ID='" + objDtcMaster.sDtcCode + "'";
                            ObjCon.Execute(strQry);

                            ObjCon.Execute("UPDATE TBLTCMASTER set TC_CURRENT_LOCATION=2, TC_LOCATION_ID='" + objDtcMaster.sOMSectionName + "' where TC_CODE='" + objDtcMaster.sTcCode + "'");

                            if (objDtcMaster.sTcCode != objDtcMaster.sOldTcCode && objDtcMaster.sOldTcCode != "")
                            {

                                ObjCon.Execute("UPDATE TBLTCMASTER set TC_CURRENT_LOCATION=1 where TC_CODE='" + objDtcMaster.sOldTcCode + "'");
                            }

                            ObjCon.CommitTrans();

                            string sCommisionDate = string.Empty;
                            if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                            {
                                File.AppendAllText(sPath, " TRM Before Comm date " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " Comm Date : " + objDtcMaster.sCommisionDate + Environment.NewLine + " ##############################################################" + Environment.NewLine);
                                DateTime fromdate = DateTime.ParseExact(objDtcMaster.sCommisionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                sCommisionDate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");

                                File.AppendAllText(sPath, " TRM After Comm date " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " Comm Date : " + sCommisionDate + Environment.NewLine + " ##############################################################" + Environment.NewLine);

                            }
                            else
                            {
                                sCommisionDate = "";
                            }
                            if (objDtcMaster.sTCCapacity == null || objDtcMaster.sTCCapacity == "")
                            {
                                objDtcMaster.sTCCapacity = "0";
                            }
                            TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                            File.AppendAllText(sPath, " TRM Before DtcUpdate " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " dtcname : " + objDtcMaster.sDtcName + Environment.NewLine + "Dtccode=" + objDtcMaster.sDtcCode + Environment.NewLine + "Dtccode2=" + objDtcMaster.sDtcCode.ToString().Substring(0, 4) + Environment.NewLine + "sectionname=" + objDtcMaster.sOMSectionName + Environment.NewLine + "capacity=" + objDtcMaster.sTCCapacity + Environment.NewLine + "Commissiondate=" + sCommisionDate + Environment.NewLine + "---------------------------------" + Environment.NewLine);
                            string result = objWCF.SendupdateDTCDetailstoTrms(objDtcMaster.sDtcName, objDtcMaster.sDtcCode,
                                objDtcMaster.sDtcCode.ToString().Substring(0, 4), "0", "0", objDtcMaster.sOMSectionName,
                                objDtcMaster.sTCCapacity, sCommisionDate, objDtcMaster.sDTCMeters, objDtcMaster.MeterStatus,
                        objDtcMaster.Ctratio, objDtcMaster.Wiring, objDtcMaster.Modem, objDtcMaster.Meterslno, objDtcMaster.Metermake,
                        objDtcMaster.Meterrecording, objDtcMaster.Manufactureyear);

                            File.AppendAllText(sPath, " TRM Before DtcUpdate " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " result : " + result + Environment.NewLine + " ##############################################################" + Environment.NewLine);

                            Arr[0] = "DTC Details Updated Successfully";
                            Arr[1] = "1";
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "DTC Cannot be updated as it is not in work, due to Failure";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    drMapCondition.Close();
                }
                return Arr;

            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
                return Arr;
            }
        }

        public string[] SaveUpdateDtcSpecification(clsDTCCommision objDtcMaster)
        {
            oledbcommand = new OleDbCommand();

            string[] Arr = new string[2];
            try
            {

                string strQry = string.Empty;

                strQry = "UPDATE TBLDTCMAST SET ";
                strQry += "DT_BREAKER_TYPE = '" + objDtcMaster.sBreakertype + "', DT_DTCMETERS= '" + objDtcMaster.sDTCMeters + "', DT_HT_PROTECT = '" + objDtcMaster.sHTProtect + "', ";
                strQry += "DT_LT_PROTECT = '" + objDtcMaster.sLTProtect + "', DT_GROUNDING = '" + objDtcMaster.sGrounding + "', DT_ARRESTERS = '" + objDtcMaster.sArresters + "', ";
                strQry += "DT_LT_LINE = '" + objDtcMaster.sLtlinelength + "', DT_HT_LINE = '" + objDtcMaster.sHtlinelength + "', DT_PLATFORM='" + objDtcMaster.sPlatformType + "', ";
                strQry += "DT_LOADTYPE = '" + objDtcMaster.sLoadtype + "', DT_LONGITUDE = '" + objDtcMaster.sLongitude + "', ";
                strQry += "DT_LATITUDE = '" + objDtcMaster.sLatitude + "',DT_DEPRECIATION='" + objDtcMaster.sDepreciation + "'  WHERE DT_ID='" + objDtcMaster.lDtcId + "'";

                ObjCon.Execute(strQry);
                if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                {
                    DateTime fromdate = Convert.ToDateTime(objDtcMaster.sCommisionDate);
                    objDtcMaster.sCommisionDate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
                }
                else
                {
                    objDtcMaster.sCommisionDate = "";
                }
                if (objDtcMaster.sTCCapacity == null || objDtcMaster.sTCCapacity == "")
                {
                    objDtcMaster.sTCCapacity = "0";
                }

                if (objDtcMaster.sDtcCode != null)
                {
                    TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    string result = objWCF.SendupdateDTCDetailstoTrms(objDtcMaster.sDtcName, objDtcMaster.sDtcCode,
                        objDtcMaster.sDtcCode.ToString().Substring(0, 4), "0", "0", objDtcMaster.sOMSectionName,
                        objDtcMaster.sTCCapacity, objDtcMaster.sCommisionDate, "0", "0", "0", "0", "0", "0", "0", "0", "0");
                }

                Arr[0] = "DTC Details Saved/Updated Successfully";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                return Arr;
            }
            finally
            {

            }
        }


        public object GetDTCDetails(clsDTCCommision objDtcMaster)
        {

            try
            {
                oledbcommand = new OleDbCommand();
                DataTable dtDcDetails = new DataTable();
                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("DiId", objDtcMaster.lDtcId);
                strQry = " SELECT DT_ID,DT_CODE,DT_NAME,DT_TC_ID,DT_OM_SLNO,TO_CHAR(DT_TOTAL_CON_KW) DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP) DT_TOTAL_CON_HP,TO_CHAR(DT_KWH_READING) DT_KWH_READING,DT_PLATFORM,";
                strQry += " DT_INTERNAL_CODE,DT_TC_ID,to_char(DT_CON_DATE,'dd/MM/yyyy')DT_CON_DATE,to_char(DT_LAST_INSP_DATE,'dd/MM/yyyy')DT_LAST_INSP_DATE,";
                strQry += " to_char(DT_LAST_SERVICE_DATE,'dd/MM/yyyy')DT_LAST_SERVICE_DATE,to_char(DT_TRANS_COMMISION_DATE,'dd/MM/yyyy')DT_TRANS_COMMISION_DATE,";
                strQry += " to_char(DT_FDRCHANGE_DATE,'dd/MM/yyyy')DT_FDRCHANGE_DATE,to_char(DT_CON_DATE,'dd/MM/yyyy') DT_CON_DATE, NVL(DT_BREAKER_TYPE,0) DT_BREAKER_TYPE,  ";
                strQry += "  NVL(DT_DTCMETERS,0) DT_DTCMETERS,  NVL(DT_HT_PROTECT,0) DT_HT_PROTECT, NVL(DT_LT_PROTECT,0) DT_LT_PROTECT, NVL( DT_GROUNDING,0) DT_GROUNDING, ";
                strQry += " NVL(DT_ARRESTERS,0) DT_ARRESTERS,DT_LT_LINE, DT_HT_LINE, NVL(DT_LOADTYPE,0) DT_LOADTYPE, NVL(DT_PROJECTTYPE,0) DT_PROJECTTYPE, DT_LONGITUDE, DT_LATITUDE,DT_DEPRECIATION,FD_FEEDER_NAME,";
                strQry += " FD_FEEDER_CODE ||'-'|| FD_FEEDER_NAME AS FEEDERCODE ,DT_METER_STATUS,DT_CT_RATIO,DT_WIRING, DT_MODEM,DT_LTSTATUS,DT_METER_SLNO,DT_METER_MAKE,DT_METER_REMARKS,DT_METER_RECORDING,DT_MTR_MANUFACTURE_YR  FROM ";
                strQry += " TBLDTCMAST,TBLFEEDERMAST  WHERE DT_ID= :DiId AND  DT_FDRSLNO=FD_FEEDER_CODE";

                dtDcDetails = ObjCon.getDataTable(strQry, oledbcommand);


                if (dtDcDetails.Rows.Count > 0)
                {
                    objDtcMaster.lDtcId = Convert.ToString(dtDcDetails.Rows[0]["DT_ID"]);
                    objDtcMaster.sDtcCode = Convert.ToString(dtDcDetails.Rows[0]["DT_CODE"]);
                    objDtcMaster.sDtcName = Convert.ToString(dtDcDetails.Rows[0]["DT_NAME"]);
                    objDtcMaster.sOMSectionName = Convert.ToString(dtDcDetails.Rows[0]["DT_OM_SLNO"]);
                    objDtcMaster.iConnectedKW = Convert.ToString(dtDcDetails.Rows[0]["DT_TOTAL_CON_KW"]);
                    objDtcMaster.iConnectedHP = Convert.ToString(dtDcDetails.Rows[0]["DT_TOTAL_CON_HP"]);
                    objDtcMaster.iKWHReading = Convert.ToString(dtDcDetails.Rows[0]["DT_KWH_READING"]);
                    objDtcMaster.sPlatformType = Convert.ToString(dtDcDetails.Rows[0]["DT_PLATFORM"]);
                    objDtcMaster.sTcCode = Convert.ToString(dtDcDetails.Rows[0]["DT_TC_ID"]);
                    objDtcMaster.sConnectionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_CON_DATE"]);
                    objDtcMaster.sInspectionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_LAST_INSP_DATE"]);
                    objDtcMaster.sServiceDate = Convert.ToString(dtDcDetails.Rows[0]["DT_LAST_SERVICE_DATE"]);
                    objDtcMaster.sCommisionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_TRANS_COMMISION_DATE"]);
                    objDtcMaster.sFeederChangeDate = Convert.ToString(dtDcDetails.Rows[0]["DT_FDRCHANGE_DATE"]);
                    objDtcMaster.sInternalCode = Convert.ToString(dtDcDetails.Rows[0]["DT_INTERNAL_CODE"]);
                    objDtcMaster.sBreakertype = Convert.ToString(dtDcDetails.Rows[0]["DT_BREAKER_TYPE"]);

                    objDtcMaster.sHTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_PROTECT"]);
                    objDtcMaster.sLTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_PROTECT"]);
                    objDtcMaster.sGrounding = Convert.ToString(dtDcDetails.Rows[0]["DT_GROUNDING"]);
                    objDtcMaster.sArresters = Convert.ToString(dtDcDetails.Rows[0]["DT_ARRESTERS"]);
                    objDtcMaster.sLtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_LINE"]);
                    objDtcMaster.sHtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_LINE"]);
                    objDtcMaster.sProjecttype = Convert.ToString(dtDcDetails.Rows[0]["DT_PROJECTTYPE"]);
                    objDtcMaster.sLoadtype = Convert.ToString(dtDcDetails.Rows[0]["DT_LOADTYPE"]);
                    objDtcMaster.sLongitude = Convert.ToString(dtDcDetails.Rows[0]["DT_LONGITUDE"]);
                    objDtcMaster.sLatitude = Convert.ToString(dtDcDetails.Rows[0]["DT_LATITUDE"]);
                    objDtcMaster.sDepreciation = Convert.ToString(dtDcDetails.Rows[0]["DT_DEPRECIATION"]);
                    objDtcMaster.sFeedercode = Convert.ToString(dtDcDetails.Rows[0]["FEEDERCODE"]);

                    objDtcMaster.sDTCMeters = Convert.ToString(dtDcDetails.Rows[0]["DT_DTCMETERS"]);
                    objDtcMaster.MeterStatus = Convert.ToString(dtDcDetails.Rows[0]["DT_METER_STATUS"]);
                    objDtcMaster.Ctratio = Convert.ToString(dtDcDetails.Rows[0]["DT_CT_RATIO"]);
                    objDtcMaster.Wiring = Convert.ToString(dtDcDetails.Rows[0]["DT_WIRING"]);
                    objDtcMaster.Modem = Convert.ToString(dtDcDetails.Rows[0]["DT_MODEM"]);
                    objDtcMaster.Ltstatus = Convert.ToString(dtDcDetails.Rows[0]["DT_LTSTATUS"]);

                    objDtcMaster.Meterslno = Convert.ToString(dtDcDetails.Rows[0]["DT_METER_SLNO"]);
                    objDtcMaster.Metermake = Convert.ToString(dtDcDetails.Rows[0]["DT_METER_MAKE"]);
                    objDtcMaster.Meterrecording = Convert.ToString(dtDcDetails.Rows[0]["DT_METER_RECORDING"]);
                    objDtcMaster.Remarks = Convert.ToString(dtDcDetails.Rows[0]["DT_METER_REMARKS"]);
                    objDtcMaster.Manufactureyear = Convert.ToString(dtDcDetails.Rows[0]["DT_MTR_MANUFACTURE_YR"]);


                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TcCode", objDtcMaster.sTcCode);
                    strQry = "SELECT TC_SLNO ||  '~' ||  TM_NAME || '~' || TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES  WHERE TC_MAKE_ID= TM_ID and TC_CODE= :TcCode";

                    string sResult = ObjCon.get_value(strQry, oledbcommand);

                    if (sResult != "")
                    {
                        objDtcMaster.sTcSlno = sResult.Split('~').GetValue(0).ToString();
                        objDtcMaster.sTCMakeName = sResult.Split('~').GetValue(1).ToString();
                        objDtcMaster.sTCCapacity = sResult.Split('~').GetValue(2).ToString();
                    }
                }

                //getting DTC photo 
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("DteDtcCode3", objDtcMaster.sDtcCode);
                //   strQry = "SELECT EP_DTLMSDTC_PATH  FROM TBLENUMERATIONPHOTOS , TBLDTCENUMERATION WHERE DTE_ED_ID=  EP_ED_ID AND DTE_DTCCODE = :DteDtcCode3";
                strQry = "SELECT EP_DTLMSDTC_PATH  FROM TBLENUMERATIONPHOTOS , TBLDTCENUMERATION WHERE DTE_ED_ID=  EP_ED_ID AND DTE_DTCCODE = :DteDtcCode3 ORDER BY EP_ED_ID desc";
                DataTable dt = ObjCon.getDataTable(strQry, oledbcommand);
                if (dt.Rows.Count > 0)
                {
                    objDtcMaster.sDTCImagePath = Convert.ToString(dt.Rows[0]["EP_DTLMSDTC_PATH"]);
                }

                // getting DTR photo
                oledbcommand = new OleDbCommand();
                if (objDtcMaster.sTcCode == "0")
                {
                    oledbcommand.Parameters.AddWithValue("DteTcCode4", objDtcMaster.sDtcCode);
                    strQry = "SELECT EP_DTC_PATH   FROM TBLENUMERATIONPHOTOS , TBLDTCENUMERATION WHERE DTE_ED_ID=  EP_ED_ID AND DTE_DTCCODE  = :DteTcCode4 ";
                    dt = ObjCon.getDataTable(strQry, oledbcommand);
                    if (dt.Rows.Count > 0)
                    {
                        objDtcMaster.sDTCPath = Convert.ToString(dt.Rows[0]["EP_DTC_PATH"]);
                    }
                }
                else
                {
                    oledbcommand.Parameters.AddWithValue("DteTcCode3", objDtcMaster.sTcCode);
                    //strQry = "SELECT EP_SSPLATE_PATH ,EP_NAMEPLATE_PATH   FROM TBLENUMERATIONPHOTOS , TBLDTCENUMERATION WHERE DTE_ED_ID=  EP_ED_ID AND DTE_TC_CODE  = :DteTcCode3";
                    strQry = "SELECT EP_SSPLATE_PATH ,EP_NAMEPLATE_PATH   FROM TBLENUMERATIONPHOTOS , TBLDTCENUMERATION WHERE DTE_ED_ID=  EP_ED_ID AND DTE_TC_CODE  = :DteTcCode3  ORDER BY DTE_ID desc ";
                    dt = ObjCon.getDataTable(strQry, oledbcommand);
                    if (dt.Rows.Count > 0)
                    {
                        objDtcMaster.sDTrImagePath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                        objDtcMaster.sDTrNamePlate = Convert.ToString(dt.Rows[0]["EP_NAMEPLATE_PATH"]);
                    }
                }
                return objDtcMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcDetails");
                return objDtcMaster;
            }
            finally
            {

            }

        }

        /// <summary>
        /// To get TC Details Used in DTCMaster Form
        /// </summary>
        /// <param name="objTCMaster"></param>
        /// <returns></returns>
        public clsDtcMaster GetTCDetails(clsDtcMaster objDTCMaster)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string sQry = string.Empty;
                DataTable dt = new DataTable();
                oledbcommand.Parameters.AddWithValue("TcSlNo", objDTCMaster.sTcSlno);
                sQry = "SELECT TC_SLNO,TC_CODE,TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES ";
                sQry += " WHERE TC_MAKE_ID= TM_ID and TC_SLNO= :TcSlNo";
                dt = ObjCon.getDataTable(sQry, oledbcommand);
                if (dt.Rows.Count > 0)
                {
                    objDTCMaster.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objDTCMaster.sTCMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objDTCMaster.sTCCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objDTCMaster.sTcCode = dt.Rows[0]["TC_CODE"].ToString();

                }
                return objDTCMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTCDetails");
                return objDTCMaster;
            }
            finally
            {

            }
        }


        public clsDTCCommision GetImagePath(clsDTCCommision objDTCComm)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbcommand.Parameters.AddWithValue("DteDtcCode4", objDTCComm.sDtcCode);
                strQry = "SELECT EP_DTLMSDTC_PATH,EP_SSPLATE_PATH FROM TBLDTCENUMERATION,TBLENUMERATIONPHOTOS,TBLENUMERATIONDETAILS  ";
                strQry += " WHERE DTE_DTCCODE= :DteDtcCode4 AND DTE_ED_ID=EP_ED_ID AND ED_ID=DTE_ED_ID AND ED_STATUS_FLAG<>'5'";
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                if (dt.Rows.Count > 0)
                {
                    objDTCComm.sDTCImagePath = Convert.ToString(dt.Rows[0]["EP_DTLMSDTC_PATH"]);
                    objDTCComm.sDTrImagePath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                }
                return objDTCComm;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetImagePath");
                return objDTCComm;
            }
            finally
            {

            }
        }


        public bool CheckSelfExecutionSchemeType(clsDTCCommision objDTcComm)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                bool bResult = false;
                if (objDTcComm.sProjecttype == "9" || objDTcComm.sProjecttype == "10")
                {
                    oledbcommand.Parameters.AddWithValue("DtCode5", objDTcComm.sDtcCode);
                    strQry = "SELECT DT_TRANS_COMMISION_DATE FROM TBLDTCMAST WHERE SYSDATE-DT_TRANS_COMMISION_DATE>365 AND DT_CODE= :DtCode5";
                    string sResult = ObjCon.get_value(strQry, oledbcommand);
                    if (sResult == "")
                    {
                        bResult = true;
                    }

                }
                return bResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckSelfExecutionSchemeTypeFailureEntry");
                return false;
            }
            finally
            {

            }
        }

        public string SaveXmlData(clsDTCCommision objDTcComm)
        {
            oledbcommand = new OleDbCommand();
            string sTcXmlData = string.Empty;

            try
            {

                string strQry = string.Empty;
                string strTemp = string.Empty;

                string sPrimaryKey = "{0}";


                objDTcComm.sColumnNames = "DT_CODE,DT_NAME,DT_OM_SLNO,DT_TC_ID,DT_INTERNAL_CODE,";
                objDTcComm.sColumnNames += "DT_KWH_READING,DT_TOTAL_CON_HP,DT_TOTAL_CON_KW,";
                objDTcComm.sColumnNames += "DT_LAST_SERVICE_DATE,DT_TRANS_COMMISION_DATE,";
                objDTcComm.sColumnNames += "DT_FDRCHANGE_DATE,DT_FDRSLNO,DT_CON_DATE,DT_PROJECTTYPE";

                objDTcComm.sColumnValues = "" + objDTcComm.sDtcCode + "," + objDTcComm.sDtcName + ",";
                objDTcComm.sColumnValues += "" + objDTcComm.sOMSectionName + "," + objDTcComm.sTcCode + ",";
                objDTcComm.sColumnValues += "" + objDTcComm.sInternalCode + "," + objDTcComm.iKWHReading + "," + objDTcComm.iConnectedHP + ",," + objDTcComm.iConnectedKW + "";
                objDTcComm.sColumnValues += "," + objDTcComm.sServiceDate + "," + objDTcComm.sCommisionDate + "," + objDTcComm.sFeederChangeDate + "," + objDTcComm.sDtcCode.ToString().Substring(0, 4) + ",";
                objDTcComm.sColumnValues += "" + objDTcComm.sConnectionDate + "," + objDTcComm.sProjecttype + "";

                objDTcComm.sTableNames = "TBLDTCMAST";

                sTcXmlData = CreateXml(objDTcComm.sColumnNames, objDTcComm.sColumnValues, objDTcComm.sTableNames);
                return sTcXmlData;
            }

            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveXmlData");
                return sTcXmlData;
            }

        }

        //save data in tblefodata and workflowobjects
        public bool SaveWorkFlowData(clsDTCCommision objDTcComm)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                ObjCon.BeginTrans();
                string strQry = string.Empty;
                objDTcComm.sWFDataId = Convert.ToString(ObjCon.Get_max_no("WFO_ID", "TBLWFODATA"));
                strQry = "INSERT INTO TBLWFODATA (WFO_ID,WFO_QUERY_VALUES,WFO_PARAMETER,WFO_DATA,WFO_CR_BY) VALUES (";
                strQry += " '" + objDTcComm.sWFDataId + "','" + objDTcComm.sQryValues + "','" + objDTcComm.sParameterValues + "','" + sXmlData + "',";
                strQry += " '" + objDTcComm.sCrBy + "')";
                ObjCon.Execute(strQry);

                if (objDTcComm.sFormName != null && objDTcComm.sFormName != "")
                {
                    //To get Business Object Id
                    oledbcommand.Parameters.AddWithValue("FormName1", objDTcComm.sFormName.Trim().ToUpper());
                    objDTcComm.sBOId = ObjCon.get_value("SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)=:FormName1", oledbcommand);
                }

                WorkFlowObjects(objDTcComm);

                string sWFlowId = Convert.ToString(ObjCon.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,WO_CLIENT_IP,";
                strQry += " WO_CR_BY,WO_APPROVED_BY,WO_APPROVE_STATUS,WO_RECORD_BY,WO_DESCRIPTION,WO_USER_COMMENT,WO_WFO_ID,WO_INITIAL_ID,WO_REF_OFFCODE)";
                strQry += " VALUES ('" + sWFlowId + "','" + objDTcComm.sBOId + "','" + objDTcComm.lDtcId + "',0,";
                strQry += " '0','" + objDTcComm.sOfficeCode + "','" + objDTcComm.sClientIP + "','" + objDTcComm.sCrBy + "',";
                strQry += " '" + objDTcComm.sCrBy + "','1','WEB','" + objDTcComm.sDescription + "','" + objDTcComm.sDescription + "','" + objDTcComm.sWFDataId + "','" + sWFlowId + "','" + objDTcComm.sOfficeCode + "')";
                ObjCon.Execute(strQry);
                ObjCon.CommitTrans();

                return true;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveWorkFlowData");
                return false;
            }
            finally
            {

            }
        }

        //To Get CLient ip
        public void WorkFlowObjects(clsDTCCommision objDTcComm)
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



                objDTcComm.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
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
        }

        public string AutoGenerateDTCCode(clsDTCCommision objDtccommission)
        {
            string final = string.Empty;
            try
            {
                // string sLastDtcCode = "AG";
                string sLastDtcCode;
                string strQry = string.Empty;

                char last, lastupdated;
                char first, firstupdated;

                strQry = "select max(DT_CODE) from (SELECT DT_CODE as DT_CODE FROM TBLDTCMAST WHERE DT_FDRSLNO = '" + objDtccommission.sFeedercode + "' union all ";
                strQry += " SELECT FBDS_NEW_DTC_CODE as DT_CODE  FROM TBLFEEDER_BFCN_DETAILS_SO WHERE FBDS_NEW_FEEDER_CODE = '" + objDtccommission.sFeedercode + "'  and  FBDS_STATUS in (0,1)  union all ";
                strQry += " SELECT DTE_DTCCODE as DT_CODE   FROM TBLDTCENUMERATION  ,TBLENUMERATIONDETAILS WHERE ED_ID = DTE_ED_ID ";
                strQry += " and ED_STATUS_FLAG <>  5 and ED_FEEDERCODE = '" + objDtccommission.sFeedercode + "' and ED_LOCTYPE = 2  UNION ALL SELECT TE_DTC_CODE  as DT_CODE FROM TBLTEMPENUMERATION WHERE TE_FD_CODE='" + objDtccommission.sFeedercode + "'  ORDER BY DT_CODE desc)";

                sLastDtcCode = ObjCon.get_value(strQry);
                // if greater than 4 then 
                if (sLastDtcCode.Length > 4)
                {

                    string s = sLastDtcCode.Substring(4);

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(s);

                    if ((asciiBytes[0] >= 97 && asciiBytes[0] <= 122) || (asciiBytes[1] >= 97 && asciiBytes[1] <= 122))
                    {
                        final = "Invalid DTC Code Generated " + sLastDtcCode + ",Please contact support team";
                        return final;
                    }



                    if (asciiBytes[0] >= 48 && asciiBytes[0] <= 57)
                    {
                        int temp = Convert.ToInt32(s);
                        temp = temp + 1;


                        if (temp.ToString().Length == 1)
                        {
                            string strTemp = "0" + Convert.ToString(temp);
                            final = sLastDtcCode.Substring(0, 4) + strTemp;
                            return final;
                        }

                        if (temp > 99)
                        {
                            final = "AA";
                        }
                        else
                        {
                            final = Convert.ToString(temp);
                        }

                    }
                    else
                    {

                        s.ToUpper();
                        char[] arr = s.ToCharArray();
                        //first = firstupdated = 'Z';
                        //last = lastupdated = 'Z';
                        first = firstupdated = arr[0];
                        last = lastupdated = arr[1];

                        lastupdated++;
                        if (lastupdated.Equals('['))
                        {
                            lastupdated = 'A';
                            firstupdated++;
                            if (firstupdated.Equals('['))
                            {
                                firstupdated = 'A';
                            }
                        }
                        //else
                        //{
                        //    firstupdated++;
                        //}
                        //if (firstupdated.Equals('['))
                        //{
                        //    firstupdated = 'A';
                        //}

                        final = Convert.ToString(firstupdated) + Convert.ToString(lastupdated);
                    }
                }
                else //  if there are not dtc code in the feeder 
                {
                    final = objDtccommission.sFeedercode + "01";
                    return final;
                }
                final = sLastDtcCode.Substring(0, 4) + final;
                return final;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AutoGenerateDTCCode");
                return final;
            }
        }




    }
}
