using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsDtcMaster
    {
        string strFormCode = "clsDtcMaster";
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
        public string sProjectType { get; set; }

        public string sOfficeCode { get; set; }

        public string sFeederCode { get; set; }
        public string sDate { get; set; }


        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        OleDbCommand oledbcommand;
        public string[] SaveUpdateDtcDetails(clsDtcMaster objDtcMaster)
        {
            oledbcommand = new OleDbCommand();
            string[] Arr = new string[2];
            try
            {
                OleDbDataReader dr;
                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("TcSlNo", objDtcMaster.sTcSlno);
                dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_SLNO=:TcSlNo", oledbcommand);
                if (!dr.Read())
                {
                    Arr[0] = "Enter Valid TC SlNo ";
                    Arr[1] = "4";
                    return Arr;
                }
                dr.Close();


                oledbcommand = new OleDbCommand();
                if (objDtcMaster.lDtcId == "")
                {
                    oledbcommand.Parameters.AddWithValue("DtCode", objDtcMaster.sDtcCode);
                    dr = ObjCon.Fetch("select DT_CODE from TBLDTCMAST where DT_CODE= :DtCode", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "DTC Code Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("FeederCode", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                    dr = ObjCon.Fetch("select * from TBLFEEDERMAST where FD_FEEDER_CODE=:FeederCode", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Code Does Not Match With The Feeder Code";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("OmCode", objDtcMaster.sOMSectionName);
                    dr = ObjCon.Fetch("select * from TBLOMSECMAST where OM_CODE= :OmCode", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid O&m Sec ";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TcCode", objDtcMaster.sTcCode);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE= :TcCode", oledbcommand);
                    if (!dr.Read())
                    {

                        dr.Close();
                        Arr[0] = "Enter Valid TC SlNo ";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();

                    ObjCon.BeginTrans();
                    objDtcMaster.lDtcId = Convert.ToString(ObjCon.Get_max_no("DT_ID", "TBLDTCMAST"));
                    oledbcommand.Parameters.AddWithValue("feederCode1", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                    string strFeederSlno = ObjCon.get_value("SELECT FD_FEEDER_ID FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE=:feederCode1", oledbcommand);

                    strQry = "Insert into TBLDTCMAST (DT_ID,DT_CODE,DT_NAME,DT_OM_SLNO,DT_TOTAL_CON_KW,DT_TOTAL_CON_HP,DT_KWH_READING,";
                    strQry += " DT_PLATFORM,DT_INTERNAL_CODE,DT_TC_ID,DT_CON_DATE,DT_LAST_INSP_DATE,DT_LAST_SERVICE_DATE,DT_TRANS_COMMISION_DATE,";
                    strQry += " DT_FDRCHANGE_DATE,DT_FDRSLNO,DT_CRBY,DT_CRON, DT_BREAKER_TYPE, DT_DTCMETERS, DT_HT_PROTECT, DT_LT_PROTECT, DT_GROUNDING, ";
                    strQry += " DT_ARRESTERS, DT_LT_LINE, DT_HT_LINE) VALUES ('" + objDtcMaster.lDtcId + "','" + objDtcMaster.sDtcCode + "',";
                    strQry += " '" + objDtcMaster.sDtcName + "','" + objDtcMaster.sOMSectionName + "','" + objDtcMaster.iConnectedKW + "',";
                    strQry += "'" + objDtcMaster.iConnectedHP + "','" + objDtcMaster.iKWHReading + "','" + objDtcMaster.sPlatformType + "',";
                    strQry += " '" + objDtcMaster.sInternalCode + "','" + objDtcMaster.sTcCode + "',TO_DATE('" + objDtcMaster.sConnectionDate + "','dd/MM/yyyy'),";
                    strQry += " TO_DATE('" + objDtcMaster.sInspectionDate + "','dd/MM/yyyy'),TO_DATE('" + objDtcMaster.sServiceDate + "','dd/MM/yyyy'),";
                    strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),TO_DATE('" + objDtcMaster.sFeederChangeDate + "','dd/MM/yyyy'), ";
                    strQry += " '" + objDtcMaster.sDtcCode.ToString().Substring(0, 4) + "','" + objDtcMaster.sCrBy + "',SYSDATE ,  '" + objDtcMaster.sBreakertype + "' , '" + objDtcMaster.sDTCMeters + "', ";
                    strQry += " '" + objDtcMaster.sHTProtect + "','" + objDtcMaster.sHTProtect + "','" + objDtcMaster.sGrounding + "', '" + objDtcMaster.sArresters + "', ";
                    strQry += " '" + objDtcMaster.sLtlinelength + "','" + objDtcMaster.sHtlinelength + "')";
                    ObjCon.Execute(strQry);

                    objDtcMaster.lGetMaxMap = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                    strQry = "INSERT INTO TBLTRANSDTCMAPPING (TM_ID,TM_TC_ID,TM_DTC_ID,TM_MAPPING_DATE,TM_CRBY) ";
                    strQry += "VALUES('" + objDtcMaster.lGetMaxMap + "','" + objDtcMaster.sTcCode.ToUpper() + "','" + objDtcMaster.sDtcCode + "',";
                    strQry += " TO_DATE('" + objDtcMaster.sConnectionDate + "','dd/MM/yyyy'),'" + objDtcMaster.sCrBy + "')";
                    ObjCon.Execute(strQry);

                    strQry = "UPDATE TBLTCMASTER set TC_UPDATED_EVENT='DTC MASTER ENTRY',TC_UPDATED_EVENT_ID='" + lGetMaxMap + "', TC_CURRENT_LOCATION=2, ";
                    strQry += " TC_LOCATION_ID='" + objDtcMaster.sOMSectionName + "' where TC_CODE='" + objDtcMaster.sTcCode.ToUpper() + "'";

                    ObjCon.Execute(strQry);
                    if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                    {
                        DateTime fromdate = Convert.ToDateTime(objDtcMaster.sCommisionDate);
                        objDtcMaster.sCommisionDate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
                    }
                    if (objDtcMaster.sTCCapacity == null && objDtcMaster.sTCCapacity == "")
                    {
                        objDtcMaster.sTCCapacity = "0";
                    }
                    TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    string result = objWCF.SendDTCDetailstoTrms(objDtcMaster.sDtcName, objDtcMaster.sDtcCode,
                        objDtcMaster.sDtcCode.ToString().Substring(0, 4), "0", "0", objDtcMaster.sOMSectionName,
                        objDtcMaster.sTCCapacity, objDtcMaster.sCommisionDate, "", "", "", "", "");

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
                    oledbcommand.Parameters.AddWithValue("DtId1", objDtcMaster.lDtcId);
                    dr = ObjCon.Fetch("select * from TBLDTCMAST where DT_CODE= :dtCode1 AND DT_ID<> :DtId1", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "DTC With This Id  Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("Feedercode2", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                    dr = ObjCon.Fetch("select * from TBLFEEDERMAST where FD_FEEDER_CODE= :Feedercode2", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Code Does Not Match With The Feeder Code";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("omCode1", objDtcMaster.sOMSectionName);
                    dr = ObjCon.Fetch("select * from TBLOMSECMAST where OM_CODE= :omCode1", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid O&m Sec ";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tcCode1", objDtcMaster.sTcCode);
                    dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE= :tcCode1", oledbcommand);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid TC SlNo ";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TmDtcId2", objDtcMaster.sDtcCode);
                    OleDbDataReader drMapCondition = ObjCon.Fetch("SELECT COUNT(*) FROM TBLTRANSDTCMAPPING  WHERE TM_DTC_ID= :TmDtcId2", oledbcommand);
                    if (drMapCondition.Read())
                    {
                        oledbcommand.Parameters.AddWithValue("feedercode3", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                        string strFeederSlno = ObjCon.get_value("SELECT FD_FEEDER_ID FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE= :feedercode3", oledbcommand);
                        oledbcommand.Parameters.AddWithValue("TmtcId2", objDtcMaster.sTcCode);
                        string strCount = ObjCon.get_value("select count(*) from TBLTRANSDTCMAPPING where TM_TC_ID= :TmtcId2 and TM_LIVE_FLAG=1", oledbcommand);
                        if (Convert.ToInt32(strCount) <= 1)
                        {
                            ObjCon.BeginTrans();
                            strQry = "update TBLDTCMAST SET DT_CODE='" + objDtcMaster.sDtcCode + "',DT_NAME ='" + objDtcMaster.sDtcName + "',";
                            strQry += "DT_OM_SLNO='" + objDtcMaster.sOMSectionName + "',DT_TC_ID='" + objDtcMaster.sTcCode + "',DT_INTERNAL_CODE='" + objDtcMaster.sInternalCode + "',";
                            strQry += "DT_KWH_READING='" + objDtcMaster.iKWHReading + "',DT_PLATFORM='" + objDtcMaster.sPlatformType + "',DT_TOTAL_CON_HP='" + objDtcMaster.iConnectedHP + "',DT_TOTAL_CON_KW='" + objDtcMaster.iConnectedKW + "',";
                            strQry += "DT_LAST_INSP_DATE=TO_DATE('" + objDtcMaster.sInspectionDate + "','DD/MM/YYYY'),";
                            strQry += "DT_LAST_SERVICE_DATE=TO_DATE('" + objDtcMaster.sServiceDate + "','DD/MM/YYYY'),DT_TRANS_COMMISION_DATE=TO_DATE('" + objDtcMaster.sCommisionDate + "','DD/MM/YYYY'),";
                            strQry += "DT_FDRCHANGE_DATE=TO_DATE('" + objDtcMaster.sFeederChangeDate + "','DD/MM/YYYY') ,DT_FDRSLNO='" + objDtcMaster.sDtcCode.ToString().Substring(0, 4) + "', ";
                            strQry += "DT_BREAKER_TYPE = '" + objDtcMaster.sBreakertype + "', DT_DTCMETERS= '" + objDtcMaster.sDTCMeters + "', DT_HT_PROTECT = '" + objDtcMaster.sHTProtect + "', ";
                            strQry += "DT_LT_PROTECT = '" + objDtcMaster.sLTProtect + "', DT_GROUNDING = '" + objDtcMaster.sGrounding + "', DT_ARRESTERS = '" + objDtcMaster.sArresters + "', ";
                            strQry += "DT_LT_LINE = '" + objDtcMaster.sLtlinelength + "', DT_HT_LINE = '" + objDtcMaster.sHtlinelength + "',DT_CON_DATE=TO_DATE('" + objDtcMaster.sConnectionDate + "','DD/MM/YYYY') WHERE DT_ID='" + objDtcMaster.lDtcId + "'";


                            ObjCon.Execute(strQry);


                            strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_TC_ID='" + objDtcMaster.sTcCode.ToUpper() + "',TM_CRBY='" + objDtcMaster.sCrBy + "'";
                            if (objDtcMaster.sConnectionDate != string.Empty)
                            {
                                strQry += ",TM_MAPPING_DATE=TO_DATE('" + sConnectionDate + "','DD/MM/YYYY')";
                            }
                            strQry += "where TM_DTC_ID='" + objDtcMaster.sDtcCode + "'";
                            ObjCon.Execute(strQry);

                            ObjCon.Execute("update TBLTCMASTER set TC_CURRENT_LOCATION=2, TC_LOCATION_ID='" + objDtcMaster.sOMSectionName + "' where TC_CODE='" + objDtcMaster.sTcCode + "'");

                            if (objDtcMaster.sTcCode != objDtcMaster.sOldTcCode && objDtcMaster.sOldTcCode != "")
                            {

                                ObjCon.Execute("update TBLTCMASTER set TC_CURRENT_LOCATION=1 where TC_CODE='" + objDtcMaster.sOldTcCode + "'");
                            }
                            if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                            {
                                DateTime fromdate = Convert.ToDateTime(objDtcMaster.sCommisionDate);
                                objDtcMaster.sCommisionDate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
                            }
                            if (objDtcMaster.sTCCapacity == null && objDtcMaster.sTCCapacity == "")
                            {
                                objDtcMaster.sTCCapacity = "0";
                            }
                            TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                            string result = objWCF.SendupdateDTCDetailstoTrms(objDtcMaster.sDtcName, objDtcMaster.sDtcCode,
                                objDtcMaster.sDtcCode.ToString().Substring(0, 4), "0", "0", objDtcMaster.sOMSectionName,
                                objDtcMaster.sTCCapacity, objDtcMaster.sCommisionDate, "0", "0", "0", "0", "0", "0", "0", "0", "0");

                            ObjCon.CommitTrans();
                            Arr[0] = "DTC Details Updated Successfully";
                            Arr[1] = "1";
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "DTC Cannot be updated as it is not in work, due to failure";
                            Arr[1] = "4";
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

        public string CheckFeederCodeStatus(string selectedValue)
        {
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT case FBS_NEW_FEEDER_CODE when null then 'SUCCESS' else 'Feeder '||FBS_NEW_FEEDER_CODE|| ' pending  with '|| ( " +
                    " SELECT OM_NAME  FROM TBLOMSECMAST WHERE US_OFFICE_CODE = OM_CODE )  ||' Section Officer for Feeder Bifurcation, Please contact through '||US_MOBILE_NO ||' or " +
                    "  Contact Support Team'   end as STATUS    FROM TBLFEEDERBIFURCATION_SO , TBLUSER  WHERE US_ID = FBS_CR_BY and  FBS_STATUS in (0,1) and " +
                    " FBS_NEW_FEEDER_CODE = '" + selectedValue + "'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
                throw ex;
            }
        }

        public string GetStationCode(string sNewFeederCode)
        {
            try
            {
                string strQry = " SELECT FD_ST_ID  FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE = '" + sNewFeederCode + "'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStationCode");
                throw ex;
            }
        }

        public string GetFeederBifurcationStatus(clsDtcMaster obj)
        {
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT case  FBS_STATUS when 1 then 'APPROVED' when 2 then 'BIFURCATED' WHEN 0 THEN 'PENDING' ELSE 'PENDING' END AS FBS_STATUS" +
                    " FROM TBLFEEDERBIFURCATION_SO , TBLFEEDER_BFCN_DETAILS_SO WHERE FBS_ID = FBDS_FB_ID and  FBDS_OLD_DTC_CODE = (SELECT DT_CODE FROM TBLDTCMAST WHERE DT_ID = '" + lDtcId + "' ) and   FBS_STATUS in (0,1)  ";

                return ObjCon.get_value(strQry);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
                return "Exception";
            }

        }

        public DataTable GetFdrBfcnDetails(clsDtcMaster obj)
        {
            try
            {
                string StrQry = string.Empty;
                StrQry = "  SELECT FBS_OLD_FEEDER_CODE  ,  FBS_NEW_FEEDER_CODE ,  to_char(FBS_OM_DATE ,'dd-MON-yyyy')  as FBS_OM_DATE , FDO_OFFICE_CODE   from " +
                    " TBLFEEDERBIFURCATION_SO   left join  TBLFEEDERMAST  on FBS_OLD_FEEDER_CODE = FD_FEEDER_CODE left join " +
                    " TBLFEEDEROFFCODE  on  FDO_FEEDER_ID = FD_FEEDER_ID  WHERE FBS_ID = '" + obj.lDtcId + "' AND FDO_OFFICE_CODE='"+ obj.sOfficeCode.Substring(0,3) + "' ";
                return ObjCon.getDataTable(StrQry);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFdrBfcnDetails");
                return null;
            }
        }

        public DataTable GetFeederBfcnRecords(clsDtcMaster obj)
        {

            DataTable dtFeederDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT FBS_ID ,  FBS_OLD_FEEDER_CODE as OLD_FEEDER_CODE , FBS_NEW_FEEDER_CODE  as NEW_FEEDER_CODE , " +
                    "  case FBS_status WHEN 1 THEN 'APPROVED' WHEN 0 THEN 'PENDING' WHEN  2  THEN 'FEEDER BIFRUCATED' ELSE 'PENDING'  END  AS STATUS  " +
                    " , (SELECT count(*) FROM TBLFEEDER_BFCN_DETAILS_SO WHERE FBS_ID = FBDS_FB_ID GROUP BY  FBDS_FB_ID  ) as COUNT_DTC  , " +
                    " (SELECT US_FULL_NAME  FROM TBLUSER WHERE US_ID = FBS_CR_BY  ) as SECTION_OFFICER ," +
                    " to_char(FBS_CRON ,  'dd-MON-yyyy') as CREATED_ON ,  (SELECT US_FULL_NAME  FROM TBLUSER WHERE US_ID = FBS_APP_BY  ) as APPROVED_BY, " +
                    " to_char(FBS_APP_ON ,  'dd-MON-yyyy') as APPROVED_ON FROM TBLFEEDERBIFURCATION_SO WHERE FBS_SECTION_CODE like '" + obj.sOfficeCode + "%' ORDER BY FBS_ID desc  ";
                dtFeederDetails = ObjCon.getDataTable(strQry);

                return dtFeederDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
                return dtFeederDetails;
            }
        }

        public DataTable LoadDtcGrid(clsDtcMaster objDTC, string displayType)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDtcDetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("DtOmSlNo", objDTC.sOfficeCode);
                strQry = "SELECT DT_ID,TO_CHAR(DT_CODE) DT_CODE,DT_NAME,TO_CHAR(DT_TOTAL_CON_KW) DT_TOTAL_CON_KW,";
                strQry += " TO_CHAR(DT_TOTAL_CON_HP) DT_TOTAL_CON_HP,TO_CHAR(TC_CODE) TC_CODE, TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                strQry += " TO_CHAR(DT_LAST_SERVICE_DATE,'DD-MON-YYYY') DT_LAST_SERVICE_DATE,DT_PROJECTTYPE, ";
                strQry += " (SELECT DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DT_OM_SLNO ,0,2))DIVISION , ";
                strQry += " (SELECT SD_SUBDIV_NAME  FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE  = SUBSTR(DT_OM_SLNO ,0,3))SUBDIVISION ,";
                strQry += " (SELECT OM_NAME  FROM TBLOMSECMAST   WHERE OM_CODE   = DT_OM_SLNO)SECTION , ";
                strQry += " (SELECT DISTINCT FD_FEEDER_NAME FROM TBLFEEDERMAST WHERE TO_CHAR(DT_FDRSLNO)=FD_FEEDER_CODE AND ROWNUM=1) FEEDER_NAME";
                strQry += " FROM TBLDTCMAST LEFT JOIN TBLTCMASTER ON TC_CODE = DT_TC_ID ";
                strQry += "  WHERE DT_OM_SLNO LIKE :DtOmSlNo||'%'";
                if (objDTC.sFeederCode != null)
                {
                    oledbcommand.Parameters.AddWithValue("DtFDRSlNo", objDTC.sFeederCode);
                    strQry += " AND DT_FDRSLNO LIKE :DtFDRSlNo ||'%'";
                }
                if (objDTC.sProjectType != null)
                {
                    oledbcommand.Parameters.AddWithValue("ProjectType", objDTC.sProjectType);
                    strQry += " AND DT_PROJECTTYPE=:ProjectType";
                }
                if (displayType == "EXCEL")
                {
                    strQry += " ORDER BY DT_CODE DESC";
                }
                else
                {
                    strQry += " AND ROWNUM < 1000  ORDER BY DT_CODE DESC";
                }
                dtDtcDetails = ObjCon.getDataTable(strQry, oledbcommand);

                return dtDtcDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcGrid");
                return dtDtcDetails;
            }

        }

        public string GetDTCCount(clsDtcMaster objDTCMaster)
        {
            oledbcommand = new OleDbCommand();
            string dtcCount = string.Empty;

            try
            {
                DataTable dtDcDetails = new DataTable();
                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("DtOmSlNo", objDTCMaster.sOfficeCode);
                strQry = " SELECT COUNT(*) FROM TBLDTCMAST WHERE DT_OM_SLNO LIKE :DtOmSlNo ||'%'";
                dtcCount = ObjCon.get_value(strQry, oledbcommand);
                return dtcCount;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcGrid");
                return null;
            }


        }

        public DataTable GetDTCDetailsForSearch(clsDtcMaster objDtcMast)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();


            try
            {

                strQry = "SELECT DT_ID,TO_CHAR(DT_CODE) DT_CODE,DT_NAME,TO_CHAR(DT_TOTAL_CON_KW) DT_TOTAL_CON_KW,";
                strQry += " TO_CHAR(DT_TOTAL_CON_HP) DT_TOTAL_CON_HP,TO_CHAR(TC_CODE) TC_CODE, TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                strQry += " TO_CHAR(DT_LAST_SERVICE_DATE,'DD-MON-YYYY') DT_LAST_SERVICE_DATE,DT_PROJECTTYPE, ";
                strQry += " (SELECT DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DT_OM_SLNO ,0,2))DIVISION , ";
                strQry += " (SELECT SD_SUBDIV_NAME  FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE  = SUBSTR(DT_OM_SLNO ,0,3))SUBDIVISION ,";
                strQry += " (SELECT OM_NAME  FROM TBLOMSECMAST   WHERE OM_CODE   = DT_OM_SLNO)SECTION ,FD_FEEDER_NAME AS FEEDER_NAME  ";
                //   strQry += " (SELECT DISTINCT FD_FEEDER_NAME FROM TBLFEEDERMAST WHERE TO_CHAR(DT_FDRSLNO)=FD_FEEDER_CODE AND ROWNUM=1) FEEDER_NAME";
                strQry += " FROM TBLDTCMAST LEFT JOIN TBLTCMASTER ON TC_CODE = DT_TC_ID  LEFT JOIN TBLFEEDERMAST on FD_FEEDER_CODE = DT_FDRSLNO WHERE ";

                if (objDtcMast.sDtcCode != "" && objDtcMast.sDtcCode != null)
                {
                    strQry += " DT_CODE  = :dtcCode";
                    oledbcommand.Parameters.AddWithValue("dtcCode", objDtcMast.sDtcCode);
                }

                if (objDtcMast.sDtcName != "" && objDtcMast.sDtcName != null)
                {
                    if (objDtcMast.sDtcCode != "" && objDtcMast.sDtcCode != null)
                    {
                        strQry += " AND ";
                    }
                    strQry += "  DT_NAME  = :dtName";
                    oledbcommand.Parameters.AddWithValue("dtName", objDtcMast.sDtcName);
                }
                if (objDtcMast.sTcCode != "" && objDtcMast.sTcCode != null)
                {
                    if ((objDtcMast.sDtcCode != "" && objDtcMast.sDtcCode != null))
                    {
                        strQry += " AND ";
                    }
                    strQry += "   (DT_TC_ID)  = :tcCode";
                    oledbcommand.Parameters.AddWithValue("tcCode", objDtcMast.sTcCode);
                }

                if (objDtcMast.sFeederCode != "" && objDtcMast.sFeederCode != null)
                {
                    if ((objDtcMast.sDtcCode != "" && objDtcMast.sTcCode != null) || (objDtcMast.sTcCode != "" && objDtcMast.sTcCode != null))
                    {
                        strQry += " AND ";
                    }
                    strQry += " UPPER(FD_FEEDER_NAME)  = :feederName";
                    oledbcommand.Parameters.AddWithValue("feederName", objDtcMast.sFeederCode.ToUpper());

                }

                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsForSearch");
                return dt;
            }
        }



        public object GetDtcDetails(clsDtcMaster objDtcMaster)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                DataTable dtDcDetails = new DataTable();
                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("Did", objDtcMaster.lDtcId);
                strQry = " SELECT DT_ID,DT_CODE,DT_NAME,DT_TC_ID,DT_OM_SLNO,TO_CHAR(DT_TOTAL_CON_KW) DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP) DT_TOTAL_CON_HP,TO_CHAR(DT_KWH_READING) DT_KWH_READING,DT_PLATFORM,";
                strQry += " DT_INTERNAL_CODE,DT_TC_ID,to_char(DT_CON_DATE,'dd/MM/yyyy')DT_CON_DATE,to_char(DT_LAST_INSP_DATE,'dd/MM/yyyy')DT_LAST_INSP_DATE,";
                strQry += " to_char(DT_LAST_SERVICE_DATE,'dd/MM/yyyy')DT_LAST_SERVICE_DATE,to_char(DT_TRANS_COMMISION_DATE,'dd/MM/yyyy')DT_TRANS_COMMISION_DATE,";
                strQry += " to_char(DT_FDRCHANGE_DATE,'dd/MM/yyyy')DT_FDRCHANGE_DATE,to_char(DT_CON_DATE,'dd/MM/yyyy') DT_CON_DATE, NVL(DT_BREAKER_TYPE,0) DT_BREAKER_TYPE,  ";
                strQry += "  NVL(DT_DTCMETERS,0) DT_DTCMETERS,  NVL(DT_HT_PROTECT,0) DT_HT_PROTECT, NVL(DT_LT_PROTECT,0) DT_LT_PROTECT, NVL( DT_GROUNDING,0) DT_GROUNDING, ";
                strQry += " NVL(DT_ARRESTERS,0) DT_ARRESTERS, DT_LT_LINE, DT_HT_LINE FROM ";
                strQry += " TBLDTCMAST WHERE DT_ID=:Did";

                dtDcDetails = ObjCon.getDataTable(strQry, oledbcommand);



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
                objDtcMaster.sDTCMeters = Convert.ToString(dtDcDetails.Rows[0]["DT_DTCMETERS"]);
                objDtcMaster.sHTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_PROTECT"]);
                objDtcMaster.sLTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_PROTECT"]);
                objDtcMaster.sGrounding = Convert.ToString(dtDcDetails.Rows[0]["DT_GROUNDING"]);
                objDtcMaster.sArresters = Convert.ToString(dtDcDetails.Rows[0]["DT_ARRESTERS"]);
                objDtcMaster.sLtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_LINE"]);
                objDtcMaster.sHtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_LINE"]);
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("tccode", objDtcMaster.sTcCode);
                strQry = "SELECT TC_SLNO ||  '~' ||  TM_NAME || '~' || TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES  WHERE TC_MAKE_ID= TM_ID and TC_CODE=:tccode";

                string sResult = ObjCon.get_value(strQry, oledbcommand);

                if (sResult != "")
                {
                    objDtcMaster.sTcSlno = sResult.Split('~').GetValue(0).ToString();
                    objDtcMaster.sTCMakeName = sResult.Split('~').GetValue(1).ToString();
                    objDtcMaster.sTCCapacity = sResult.Split('~').GetValue(2).ToString();
                }


                return objDtcMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcDetails");
                return objDtcMaster;
            }

        }

        /// <summary>
        /// To get TC Details Used in DTCMaster Form
        /// </summary>
        /// <param name="objTCMaster"></param>
        /// <returns></returns>
        public object GetTCDetails(clsDtcMaster objDTCMaster)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                oledbcommand.Parameters.AddWithValue("TcCode5", objDTCMaster.sTcCode);
                sQry = "SELECT TC_SLNO,TC_CODE,TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES WHERE TC_MAKE_ID= TM_ID and ";
                sQry += "TC_CODE NOT IN (SELECT RSD_TC_CODE from TBLREPAIRSENTDETAILS where RSD_DELIVARY_DATE is NULL ) AND TC_STATUS=3 AND  ";
                sQry += "TC_CURRENT_LOCATION =1 AND TC_CODE=:TcCode5";
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
        }



        public string[] GetPONo(String sPoNO, string sdivcode)
        {
            oledbcommand = new OleDbCommand();
            string[] Arr = new string[5];
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                oledbcommand.Parameters.AddWithValue("divCode", sdivcode);
                oledbcommand.Parameters.AddWithValue("poNo", sPoNO.Trim());
                sQry = "SELECT RSM_PO_NO,TO_CHAR(RSM_PO_DATE,'dd/mm/yyyy') RSM_PO_DATE,RSM_HTLT_DIV_CODE,RSM_SUPREP_ID FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = :divCode AND RSM_PO_NO=:poNo";
                dt = ObjCon.getDataTable(sQry, oledbcommand);

                if (dt.Rows.Count > 0)
                {
                    Arr[0] = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                    Arr[1] = "1";
                    Arr[2] = Convert.ToString(dt.Rows[0]["RSM_PO_DATE"]);
                    Arr[3] = Convert.ToString(dt.Rows[0]["RSM_HTLT_DIV_CODE"]);
                    Arr[4] = Convert.ToString(dt.Rows[0]["RSM_SUPREP_ID"]);
                }
                else
                {
                    Arr[0] = "Entered Purchase Order Number Not Exist";
                    Arr[1] = "0";
                }

                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetPONo");
                Arr[0] = "Somthing went Wrong";
                Arr[1] = "3";
                return Arr;
            }
        }

        public DataTable GetDTCDetailsUsingFeederCode(clsDtcMaster objDTC)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = "SELECT DT_ID , DT_CODE , DT_NAME ,DT_TC_ID  FROM TBLDTCMAST WHERE DT_FDRSLNO = '" + objDTC.sFeederCode + "' ORDER BY DT_CODE";
                dt = ObjCon.getDataTable(strQry);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetPONo");
                return dt;
            }

        }

        /// <summary>
        /// to get the dtc codes other than dtc that are penidng in aet approval and pending for 
        /// bifurcation
        /// </summary>
        /// <param name="objDTC"></param>
        /// <returns></returns>
        public DataTable GetDTCDetailsUsingFeederCodeSectionOfficer(clsDtcMaster objDTC)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = "SELECT DT_ID , DT_CODE , DT_NAME ,DT_TC_ID , CASE WHEN DT_CODE =(SELECT DF_DTC_CODE FROM TBLDTCFAILURE  WHERE DF_DTC_CODE=DT_CODE AND  DF_REPLACE_FLAG=0) THEN 'FAILURE' ELSE 'GOOD' END AS STATUS" +
                    " FROM TBLDTCMAST WHERE DT_FDRSLNO = '" + objDTC.sFeederCode + "' " +
                    "  and  DT_CODE not  in   (SELECT FBDS_OLD_DTC_CODE  FROM TBLFEEDERBIFURCATION_SO , TBLFEEDER_BFCN_DETAILS_SO WHERE FBS_ID = FBDS_FB_ID and FBDS_STATUS in (0,1)) " +
                    " ORDER BY DT_CODE ";

                dt = ObjCon.getDataTable(strQry);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsUsingFeederCodeSectionOfficer");
                return dt;
            }

        }
        public string GetDTCDetailsfromPartialEnumeration(clsDtcMaster objDTC)
        {
            string strQry = string.Empty;
            string maxTempDTC = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = " select max(TE_DTC_CODE) AS DT_CODE from TBLTEMPENUMERATION  WHERE TE_FD_CODE='" + objDTC.sFeederCode + "'";
                maxTempDTC = ObjCon.get_value(strQry);
                return maxTempDTC;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsfromPartialEnumeration");
                return maxTempDTC;
            }

        }
        public DataTable GetDTCDetailsUsingIdFeederCode(clsDtcMaster objDTC, String dtcIDs = "")
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (objDTC.sFeederCode.Length == 0 || dtcIDs.Length == 0)
                {
                    return dt;
                }


                // if (((objDTC.sFeederCode != "") || (objDTC.sFeederCode != null) ) && ((dtcIDs == null) || (dtcIDs =="")))

                // first query to get the Data based on DTC ID for the checked values
                strQry = " SELECT * FROM ( SELECT DT_ID ,DT_CODE ,DT_TC_ID, '' AS  DT_SERIAL_NUMBER ,DT_NAME ,DT_FDRSLNO AS OLD_FEEDER_CODE , '" + objDTC.sFeederCode + "' AS NEW_FEEDER_CODE, DT_OM_SLNO AS OFFICECODE , 2 AS ORDER_STATUS ";
                strQry += " FROM TBLDTCMAST WHERE DT_ID IN (" + dtcIDs + ") ";
                strQry += "UNION ALL";
                // second one for the DTC's under the NEW  FEEDR CODE
                strQry += " SELECT DT_ID , DT_CODE, DT_TC_ID, SUBSTR(DT_CODE,5,2) AS  DT_SERIAL_NUMBER ,DT_NAME ,DT_FDRSLNO AS OLD_FEEDER_CODE , '" + objDTC.sFeederCode + "' AS NEW_FEEDER_CODE , DT_OM_SLNO AS OFFICECODE , 1 AS ORDER_STATUS  FROM TBLDTCMAST WHERE DT_FDRSLNO IN ";
                strQry += " ('" + objDTC.sFeederCode + "') )A ORDER BY ORDER_STATUS,DT_CODE ";

                //  strQry = "SELECT DT_ID , '" + newFeederCode + "' AS FEEDER_CODE ,DT_FDRSLNO AS OLD_FEEDER_CODE ,  DT_CODE , DT_NAME  FROM TBLDTCMAST WHERE DT_ID IN (" + dtcID + ") ORDER BY DT_CODE";

                if (strQry.Length != 0)
                {
                    dt = ObjCon.getDataTable(strQry);
                }

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsUsingIdFeederCode");
                return dt;
            }

        }

        public DataTable GetDTCDetailsUsingIdFeederCodeApproval(clsDtcMaster objDTC, String dtcIDs = "")
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (objDTC.sFeederCode.Length == 0 || dtcIDs.Length == 0)
                {
                    return dt;
                }


                // if (((objDTC.sFeederCode != "") || (objDTC.sFeederCode != null) ) && ((dtcIDs == null) || (dtcIDs =="")))

                // first query to get the Data based on DTC ID for the checked values
                strQry = " SELECT * FROM ( SELECT DT_ID ,DT_CODE ,DT_TC_ID, (SELECT substr(FBDS_NEW_DTC_CODE , 5 ,2 ) FROM  TBLFEEDER_BFCN_DETAILS_SO WHERE DT_CODE = FBDS_OLD_DTC_CODE) AS  DT_SERIAL_NUMBER ,DT_NAME ,DT_FDRSLNO AS OLD_FEEDER_CODE , '" + objDTC.sFeederCode + "' AS NEW_FEEDER_CODE, DT_OM_SLNO AS OFFICECODE , 2 AS ORDER_STATUS ";
                strQry += " FROM TBLDTCMAST WHERE DT_ID IN (" + dtcIDs + ") ";
                strQry += "UNION ALL";
                // second one for the DTC's under the NEW  FEEDR CODE
                strQry += " SELECT DT_ID , DT_CODE, DT_TC_ID, SUBSTR(DT_CODE,5,2) AS  DT_SERIAL_NUMBER ,DT_NAME ,DT_FDRSLNO AS OLD_FEEDER_CODE , '" + objDTC.sFeederCode + "' AS NEW_FEEDER_CODE , DT_OM_SLNO AS OFFICECODE , 1 AS ORDER_STATUS  FROM TBLDTCMAST WHERE DT_FDRSLNO IN ";
                strQry += " ('" + objDTC.sFeederCode + "') )A WHERE A.DT_SERIAL_NUMBER is not null  ORDER BY ORDER_STATUS,DT_CODE ";

                //  strQry = "SELECT DT_ID , '" + newFeederCode + "' AS FEEDER_CODE ,DT_FDRSLNO AS OLD_FEEDER_CODE ,  DT_CODE , DT_NAME  FROM TBLDTCMAST WHERE DT_ID IN (" + dtcID + ") ORDER BY DT_CODE";

                if (strQry.Length != 0)
                {
                    dt = ObjCon.getDataTable(strQry);
                }

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsUsingIdFeederCodeApproval");
                return dt;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="oldDTCIDs"></param>
        /// <param name="newDTCCodes"></param>
        /// <param name="selectedFeederCodes"></param>
        /// <param name="scrby"></param>
        /// <returns></returns>
        public Tuple<string[], List<string>> UpdateFeederBifurcation(ArrayList lst, StringBuilder oldDTCIDs, StringBuilder newDTCCodes, HashSet<string> selectedFeederCodes, clsDtcMaster objDTCMast, string clientIP, string through)
        {

            // 0-DTCID  // 1-OldDTCCode  // 2-OldFeederCode  //  3-NewFeederCode  //  4-NewDTCCode
            string[] Arr = new string[3];
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            List<string> lstfailedDTC = new List<string>();
            List<string> lstDuplicateDTC = new List<string>();
            string failedDTC = string.Empty;
            string duplicateDTC = string.Empty;
            string description = string.Empty;
            string[] arrFeederCodes = selectedFeederCodes.ToArray();
            string sFeederCodes = string.Join(",", arrFeederCodes);
            string scrby = objDTCMast.sCrBy;
            string sOmDate = objDTCMast.sDate;
            string fbs_id = string.Empty;

            //string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            //if (!Directory.Exists(sFolderPath))
            //{
            //    Directory.CreateDirectory(sFolderPath);

            //}
            //string sFileName = "CESCMAIN";
            //string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";


            try
            {
                #region Check for DTC Failure  if failed  than add it to  the list "lstfailedDTC" 

                //strQry = "SELECT LISTAGG(DT_ID , ',') WITHIN GROUP (ORDER BY DT_ID DESC)DT_ID , LISTAGG(DT_CODE , ',') WITHIN GROUP ";
                //strQry += " (ORDER BY DT_CODE DESC)DT_CODE    FROM TBLDTCMAST   WHERE  DT_ID in ("+ oldDTCIDs + ") AND  DF_REPLACE_FLAG = 0 ";

                strQry = "SELECT DT_ID ,  ";
                strQry += " DT_CODE FROM TBLDTCMAST,TBLDTCFAILURE    WHERE  DT_ID in (" + oldDTCIDs + ") AND DF_DTC_CODE = DT_CODE AND  DF_REPLACE_FLAG = 0 ";

                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lstfailedDTC.Add(Convert.ToString(dt.Rows[i]["DT_CODE"]));
                        failedDTC = failedDTC + Convert.ToString(dt.Rows[i]["DT_CODE"]) + ",";
                    }
                    //string[] str =  Convert.ToString(dt.Rows[0]["DT_CODE"]).Split(',');
                    //lstfailedDTC.Add(str);
                    //failedDTC = Convert.ToString( dt.Rows[0]["DT_CODE"]);

                    if (failedDTC.Length > 0)
                    {
                        failedDTC = failedDTC.Substring(0, failedDTC.Length - 1);
                    }
                    Arr[0] = failedDTC + " DTCs has been declared failure .Please complete the Transaction  ";
                    Arr[1] = "0";
                    Arr[2] = "";
                    //       return Arr;

                }
                #endregion

                #region check for DTC Exists if exists than add it to  the list "lstDuplicateDTC" 

                strQry = "SELECT  DT_CODE  FROM TBLDTCMAST WHERE DT_CODE IN (" + newDTCCodes + ")";
                dt = ObjCon.getDataTable(strQry);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lstDuplicateDTC.Add(Convert.ToString(dt.Rows[i]["DT_CODE"]));
                        duplicateDTC = duplicateDTC + Convert.ToString(dt.Rows[i]["DT_CODE"]) + ",";
                    }
                    if (duplicateDTC.Length > 0)
                    {
                        duplicateDTC = duplicateDTC.Substring(0, duplicateDTC.Length - 1);
                    }
                    //add duplicate and failed DTC to the single list 
                    lstDuplicateDTC.AddRange(lstfailedDTC);
                    Arr[0] = duplicateDTC + "DTCs Code already exists";
                    Arr[1] = "0";
                    Arr[2] = "";
                    // return Arr;
                }
                #endregion

                if (through == "SECTION OFFICER")
                {
                    #region Section Officer 
                    // insert into the TBLFEEDERBIFURCATION 
                    description = "Feeder Bifurcation ";
                    string feederBifurcationID = ObjCon.get_value("SELECT NVL(max(FBS_ID), 0) + 1 FROM TBLFEEDERBIFURCATION_SO");
                    ObjCon.BeginTrans();

                    strQry = "INSERT into TBLFEEDERBIFURCATION_SO (FBS_ID ,  FBS_CRON ,FBS_SECTION_CODE, FBS_CR_BY , FBS_DESC,FBS_CLIENTIP, FBS_OLD_FEEDER_CODE , FBS_NEW_FEEDER_CODE , FBS_STATUS     )" +
                        "VALUES (" + feederBifurcationID + " , SYSDATE , '" + objDTCMast.sOfficeCode + "' ," + scrby + " ,'" + description + " " +
                        " " + sFeederCodes + " Feeder Code To " + Convert.ToString(lst[0]).Split('~')[4] + " ' ,'" + clientIP + "' ,  '" + Convert.ToString(lst[0]).Split('~')[3] + "' ,  '" + Convert.ToString(lst[0]).Split('~')[4] + "' ,  0 )";
                    ObjCon.Execute(strQry);

                    //TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    //string result = objWCF.SendFeederBifercationDetailstoTrms(Convert.ToString(lst[0]).Split('~')[3], Convert.ToString(lst[0]).Split('~')[4], description, objDTCMast.sOfficeCode);


                    for (int i = 0; i < lst.Count; i++)
                    {

                        // 0-DTCID  // 1-OldDTCCode  // 2-sTCCode  //  3-OldFeederCode  //  4-NewFeederCode  // 5- NewDTCCode
                        string sDTCId = Convert.ToString(lst[i]).Split('~')[0];
                        string sOldDTCCode = Convert.ToString(lst[i]).Split('~')[1];
                        string sDTRCode = Convert.ToString(lst[i]).Split('~')[2];
                        string sOldFeederCode = Convert.ToString(lst[i]).Split('~')[3];
                        string sNewFeederCode = Convert.ToString(lst[i]).Split('~')[4];
                        string sNewDTCCode = Convert.ToString(lst[i]).Split('~')[5];

                        //DATA SHOULD NOT  UPDATE WHILE CREATING COMMENTED ON 20-12-2022
                        //TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                        //string result1 = objWCF1.SendFeederBifercationDetailstoTrms(sOldFeederCode, sNewFeederCode, sOldDTCCode, sNewDTCCode);


                        if (!(lstDuplicateDTC.Contains(sOldDTCCode) || lstfailedDTC.Contains(sOldDTCCode)) && !(lstDuplicateDTC.Contains(sNewDTCCode) || lstfailedDTC.Contains(sNewDTCCode)))
                        {
                            try
                            {
                                strQry = "INSERT INTO TBLFEEDER_BFCN_DETAILS_SO (FBDS_ID,FBDS_FB_ID,FBDS_OLD_DTC_CODE,FBDS_DTR_CODE,FBDS_NEW_DTC_CODE,FBDS_CRON,FBDS_CR_BY," +
                                    "FBDS_OLD_FEEDER_CODE,FBDS_NEW_FEEDER_CODE , FBDS_OLD_DTC_ID  ,FBDS_STATUS) " +
                               " VALUES ((SELECT NVL( max(FBDS_ID ),0)+1 FROM TBLFEEDER_BFCN_DETAILS_SO)," + feederBifurcationID + ",'" + sOldDTCCode + "','" + sDTRCode + "','" + sNewDTCCode + "',SYSDATE," + scrby + " , '" + sOldFeederCode + "' ,'" + sNewFeederCode + "' , '" + Convert.ToString(lst[i]).Split('~')[0] + "' ,  0)";
                                ObjCon.Execute(strQry);
                            }
                            catch (Exception x)
                            {
                                clsException.LogError(x.StackTrace, x.Message, strFormCode, "UPDATINGFEEDERBIFURCATION");
                            }
                        }

                    }

                    strQry = " SELECT * FROM TBLFEEDER_BFCN_DETAILS_SO WHERE FBDS_FB_ID = '" + feederBifurcationID + "'  ";
                    if (ObjCon.get_value(strQry) == "")
                    {
                        strQry = " DELETE FROM TBLFEEDERBIFURCATION_SO WHERE FBS_ID = '" + feederBifurcationID + "'  ";
                        ObjCon.Execute(strQry);

                    }

                    if (!(lstDuplicateDTC.Count == 0 && lstfailedDTC.Count == 0))
                    {
                        Arr[0] = "Some DTC has been decalred Failure or Duplicate exists";
                        Arr[1] = "0";
                        if (feederBifurcationID.Length > 0)
                        {
                            Arr[2] = feederBifurcationID;
                        }
                        else
                        {
                            Arr[2] = "";
                        }

                    }
                    else
                    {

                        Arr[0] = "Details have been updated Successfully";
                        Arr[1] = "1";
                        Arr[2] = feederBifurcationID;
                    }

                    #endregion

                }
                else
                {

                    #region check in both the lists and update  the DTC Code along with Feeder Code and save in the Tables .

                    // insert into the TBLFEEDERBIFURCATION 
                    description = "Feeder Bifurcation ";
                    string feederBifurcationID = ObjCon.get_value("SELECT NVL(max(FB_ID), 0) + 1 FROM TBLFEEDERBIFURCATION");
                    ObjCon.BeginTrans();

                    strQry = "INSERT into TBLFEEDERBIFURCATION (FB_ID , FB_CRON ,FB_OM_DATE, FB_CR_BY , FB_DESC,FB_CLIENTIP  ) VALUES (" + feederBifurcationID + " , SYSDATE ,TO_DATE('" + objDTCMast.sDate + "','dd/MM/yyyy') ," + scrby + " ,'" + description + " " + sFeederCodes + " Feeder Code To " + Convert.ToString(lst[0]).Split('~')[4] + " ' ,'" + clientIP + "')";
                    ObjCon.Execute(strQry);

                    for (int i = 0; i < lst.Count; i++)
                    {

                        // 0-DTCID  // 1-OldDTCCode  // 2-sTCCode  //  3-OldFeederCode  //  4-NewFeederCode  // 5- NewDTCCode
                        string sDTCId = Convert.ToString(lst[i]).Split('~')[0];
                        string sOldDTCCode = Convert.ToString(lst[i]).Split('~')[1];
                        string sDTRCode = Convert.ToString(lst[i]).Split('~')[2];
                        string sOldFeederCode = Convert.ToString(lst[i]).Split('~')[3];
                        string sNewFeederCode = Convert.ToString(lst[i]).Split('~')[4];
                        string sNewDTCCode = Convert.ToString(lst[i]).Split('~')[5];

                        TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                        string result2 = objWCF.SendFeederBifercationDetailstoTrms(sOldFeederCode, sNewFeederCode, sOldDTCCode, sNewDTCCode);

                        if (!(lstDuplicateDTC.Contains(sOldDTCCode) || lstfailedDTC.Contains(sOldDTCCode)) && !(lstDuplicateDTC.Contains(sNewDTCCode) || lstfailedDTC.Contains(sNewDTCCode)))
                        {
                            try
                            {
                                strQry = "INSERT INTO TBLFEEDER_BIFURCATION_DETAILS (FBD_ID,FBD_FB_ID,FBD_OLD_DTC_CODE,FBD_DTR_CODE,FBD_NEW_DTC_CODE,FBD_CRON,FBD_CR_BY,FBD_OLD_FEEDER_CODE,FBD_NEW_FEEDER_CODE) ";
                                strQry += " VALUES ((SELECT NVL( max(FBD_ID ),0)+1 FROM TBLFEEDER_BIFURCATION_DETAILS)," + feederBifurcationID + ",'" + sOldDTCCode + "','" + sDTRCode + "','" + sNewDTCCode + "',SYSDATE," + scrby + " , '" + sOldFeederCode + "' ,'" + sNewFeederCode + "')";
                                ObjCon.Execute(strQry);

                                strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_DTC_ID = '" + sNewDTCCode + "' WHERE TM_DTC_ID = '" + sOldDTCCode + "' and TM_LIVE_FLAG = 1";
                                ObjCon.Execute(strQry);


                                strQry = "UPDATE TBLDTCMAST SET DT_CODE = '" + sNewDTCCode + "' , DT_FDRSLNO = '" + sNewFeederCode + "',DT_OLD_DTC_CODE='" + sOldDTCCode + "'  WHERE DT_ID = " + sDTCId + "";
                                ObjCon.Execute(strQry);


                                strQry = " UPDATE TBLFEEDER_BFCN_DETAILS_SO set FBDS_STATUS = 2  WHERE FBDS_OLD_DTC_CODE = '" + sOldDTCCode + "' ";
                                ObjCon.Execute(strQry);


                                strQry = "INSERT into TBLDTCTRANSACTION (DCT_ID , DCT_DTC_CODE , DCT_DTR_STATUS,  DCT_DTR_CODE  , DCT_TRANS_DATE   , DCT_DESC ,DCT_ENTRYDATE , DCT_CANCEL_FLAG  )";
                                strQry += " VALUES ((SELECT NVL( max(DCT_ID),0)+1 FROM TBLDTCTRANSACTION),'" + sOldDTCCode + "' ,1 ,'" + sDTRCode + "' ,SYSDATE ,' " + description + " OLD DTC CODE : " + sOldDTCCode + " ; NEW DTC CODE : " + sNewDTCCode + " ', SYSDATE , '0' )";
                                ObjCon.Execute(strQry);

                                strQry = "SELECT FBDS_FB_ID  FROM TBLFEEDER_BFCN_DETAILS_SO  WHERE FBDS_OLD_DTC_CODE  ='" + sOldDTCCode + "' ";
                                fbs_id = ObjCon.get_value(strQry);

                                strQry = "SELECT FBDS_ID  FROM TBLFEEDER_BFCN_DETAILS_SO WHERE FBDS_FB_ID in   ( '" + fbs_id + "' ) and  FBDS_STATUS = 1 ";
                                if (ObjCon.getDataTable(strQry).Rows.Count == 0)
                                {
                                    strQry = " UPDATE TBLFEEDER_BFCN_DETAILS_SO set fbds_status = 2  WHERE FBDS_FB_ID = '" + fbs_id + "'  ";
                                    ObjCon.Execute(strQry);

                                    strQry = " UPDATE TBLFEEDERBIFURCATION_SO  set FBS_STATUS  = 2  WHERE FBS_ID  = '" + fbs_id + "' ";
                                    ObjCon.Execute(strQry);
                                }
                                //strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= ='" + sNewDTCCode + "' ";
                                //string Cap = ObjCon.get_value(strQry);

                                //TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                                //string result1 = objWCF.SendCapacitytoTrms(Cap, sNewDTCCode);

                                // strQry = "select TC_CAPACITY from TBLTCMASTER where TC_CODE= '" + sNewDTCCode + "'";
                                strQry = "select TC_CAPACITY from TBLTCMASTER, TBLDTCMAST where TC_CODE = DT_TC_ID And DT_CODE = '" + sNewDTCCode + "'";

                                string cap = ObjCon.get_value(strQry);

                                TRM_Service.Service1Client objWCF1 = new TRM_Service.Service1Client();
                                string result = objWCF1.SendCapacitytoTrms(cap, sNewDTCCode);

                            }
                            catch (Exception x)
                            {
                                clsException.LogError(x.StackTrace, x.Message, strFormCode, "btnbifurcate_click_inside");
                            }
                        }

                    }

                    if (!(lstDuplicateDTC.Count == 0 && lstfailedDTC.Count == 0))
                    {
                        Arr[0] = "Some DTC has been decalred Failure or Duplicate exists";
                        Arr[1] = "0";
                        if (feederBifurcationID.Length > 0)
                        {
                            Arr[2] = feederBifurcationID;
                        }
                        else
                        {
                            Arr[2] = "";
                        }

                    }

                    #endregion
                    else
                    {

                        Arr[0] = "Details have been updated Successfully";
                        Arr[1] = "1";
                        Arr[2] = feederBifurcationID;
                    }


                }
                // return Arr;
                ObjCon.CommitTrans();
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnbifurcate_click");
                Arr[0] = "Exception Occurred";
                Arr[1] = "-1";
                Arr[2] = "";
            }
            return new Tuple<string[], List<string>>(Arr, lstDuplicateDTC);

        }

        public string[] ApproveFbcnRecords(clsDtcMaster obj)
        {
            string[] resultArray = new string[3];
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT LISTAGG( DTE_DTCCODE  , ',') WITHIN GROUP (ORDER BY DTE_DTCCODE )   DTE_DTCCODE  FROM  " +
                     " TBLDTCENUMERATION  , TBLENUMERATIONDETAILS WHERE ED_ID = DTE_ED_ID and  DTE_DTCCODE in (  SELECT  " +
                    " FBDS_NEW_DTC_CODE  FROM TBLFEEDER_BFCN_DETAILS_SO  WHERE  FBDS_FB_ID in (1) ) and  ED_STATUS_FLAG  in ('" + obj.lDtcId + "')  ";
                string status = ObjCon.get_value(strQry);

                if (status != "")
                {
                    resultArray[0] = status + " DTC Codes were already Enumerated,Please contact support team";
                    resultArray[1] = "0";
                    return resultArray;
                }

                strQry = "	UPDATE TBLFEEDERBIFURCATION_SO set FBS_APP_BY = '" + obj.sCrBy + "' ";
                strQry += " ,FBS_APP_ON = SYSDATE ,FBS_STATUS = 1,FBS_PENDINGWITH_SO=1   WHERE FBS_ID = '" + obj.lDtcId + "'  ";
                ObjCon.Execute(strQry);

                strQry = "	UPDATE TBLFEEDER_BFCN_DETAILS_SO set FBDS_STATUS = 1,FBDS_ENUMERATION_SO = 0   WHERE FBDS_FB_ID = '" + obj.lDtcId + "'  ";
                ObjCon.Execute(strQry);

                strQry = "SELECT  US_MOBILE_NO  FROM TBLUSER , TBLFEEDERBIFURCATION_SO  WHERE US_ID = FBS_CR_BY and  FBS_ID = '" + obj.lDtcId + "' ";

                resultArray[0] = "Approved Successfully";
                resultArray[1] = "1";



                SendSMSToSectionOfficer(ObjCon.get_value(strQry));

                return resultArray;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetNewDTCCode");
                resultArray[0] = "Exception Occurred ";
                resultArray[1] = "-1";
                return resultArray;
            }
        }

        public void SendSMSToSectionOfficer(string sMobileNo)
        {
            try
            {
                clsCommunication objComm = new clsCommunication();
                objComm.sSMSkey = "SMS_FB_SO";
                objComm = objComm.GetsmsTempalte(objComm);
                objComm.DumpSms(sMobileNo, objComm.sSMSTemplate, objComm.sSMSTemplateID);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SendSMSToSectionOfficer");
            }

        }

        public string GetNewDTCCode(clsDtcMaster objDtcMaster)
        {
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT FBD_NEW_DTC_CODE  FROM TBLFEEDER_BIFURCATION_DETAILS ,TBLDTCMAST WHERE  DT_CODE = FBD_NEW_DTC_CODE and  FBD_OLD_DTC_CODE = '" + objDtcMaster.sDtcCode + "' ";
                strQry += " and DT_OM_SLNO LIKE '" + objDtcMaster.sOfficeCode + "%' ";
                string sdtc = ObjCon.get_value(strQry);
                return sdtc;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetNewDTCCode");
                return "";
            }

        }

        public DataTable GetFeederBfcnRecordsID(string strFbsId)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT case  FBS_STATUS when 1 then 'APPROVED' when 2 then 'BIFURCATED' WHEN 0 THEN 'PENDING' ELSE 'PENDING' END AS FBS_STATUS ," +
                    " FBDS_NEW_FEEDER_CODE ,  LISTAGG( FBDS_OLD_DTC_ID  , ',') WITHIN GROUP (ORDER BY FBDS_OLD_DTC_ID ) as" +
                    "  FBDS_OLD_DTC_ID  FROM  TBLFEEDER_BFCN_DETAILS_SO , TBLFEEDERBIFURCATION_SO  WHERE FBS_ID  =FBDS_FB_ID and  FBDS_FB_ID = '" + strFbsId + "' GROUP BY FBDS_NEW_FEEDER_CODE , FBS_STATUS  ";



                //strQry = "SELECT  FBDS_OLD_DTC_CODE  ,  FBDS_NEW_DTC_CODE , FBDS_NEW_FEEDER_CODE , FBDS_OLD_FEEDER_CODE , " +
                //    " FBDS_DTR_CODE  from TBLFEEDER_BFCN_DETAILS_SO WHERE FBDS_FB_ID = '" + strFbsId + "' ";
                dt = ObjCon.getDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetNewDTCCode");
                return dt;
            }
        }

    }
}

