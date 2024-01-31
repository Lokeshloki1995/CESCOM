using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL.DTCFailure
{
    public class clsFailureRepTImeLine
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strQry = string.Empty;
        OleDbCommand oledbCommand;
        /// <summary>
        /// Load Circle Details
        /// </summary>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadCircleDetails(string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = "  SELECT NVL(CircleCode, CircleCode1)as \"CIRCLECODE\", NVL(CIRCLE, CIRCLE1)as \"CIRCLE\", ";
                strQry += " nvl(LESSTHAN1DAY,0) as \"LESSTHAN1DAY\", nvl(BW1TO7,0) as \"BW1TO7\", nvl(BW7TO15,0) as \"BW7TO15\", ";
                strQry += " nvl(BW15TO30,0) as \"BW15TO30\", nvl(ABOVE30,0)as \"ABOVE30\", nvl(TOTAL,0) as \"TOTAL\", ";
                strQry += " nvl(LESSTHAN1DAYnew,0) as \"LESSTHAN1DAYNEW\", nvl(BW1TO7new,0) as \"BW1TO7NEW\", ";
                strQry += " nvl(BW7TO15new,0) as \"BW7TO15NEW\", nvl(BW15TO30new,0) as \"BW15TO30NEW\", ";
                strQry += " nvl(ABOVE30new,0) as \"ABOVE30NEW\", nvl(TOTALnew,0) as \"TOTALNEW\" FROM ";
                strQry += " (SELECT NVL(CircleCode,0)CircleCode, CIRCLE, NVL(SUM(LESSTHAN1),0)LESSTHAN1DAY, NVL(SUM(BETWEEN1TO7),0)BW1TO7,  ";
                strQry += " NVL(SUM(BETWEEN7TO15),0)BW7TO15, NVL(SUM(BETWEEN15TO30),0)BW15TO30, NVL(SUM(ABOVE30),0)ABOVE30,  NVL(SUM(TOTAL),0)TOTAL ";
                strQry += " FROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE   OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))";
                strQry += " CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, CASE WHEN(TR_DECOMM_DATE - DF_DATE)   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END ";
                strQry += " LESSTHAN1, CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0 END BETWEEN1TO7, ";
                strQry += " CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END  BETWEEN7TO15, CASE WHEN(TR_DECOMM_DATE - DF_DATE)  ";
                strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30,    CASE WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ";
                strQry += " ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL   FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE ";
                strQry += " INNER JOIN    TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN   TBLINDENT ON ";
                strQry += " WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE   ON IN_NO = TR_IN_NO AND ";
                strQry += " DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) ";
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate";
                    oledbCommand.Parameters.AddWithValue("sFromDate", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sTodate1 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate1 ";
                    oledbCommand.Parameters.AddWithValue("sFromDate1", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate1", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate2 ";
                    oledbCommand.Parameters.AddWithValue("sTodate2", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A ";
                strQry += " GROUP BY CIRCLE, CircleCode ORDER BY CircleCode)A full outer join (SELECT NVL(CircleCode,0) AS CircleCode1,CIRCLE AS CIRCLE1, ";
                strQry += " NVL(SUM(LESSTHAN1),0)LESSTHAN1DAYNEW,NVL(SUM(BETWEEN1TO7),0)BW1TO7NEW, NVL(SUM(BETWEEN7TO15),0) BW7TO15NEW, ";
                strQry += " NVL(SUM(BETWEEN15TO30),0) BW15TO30NEW,NVL(SUM(ABOVE30),0) ABOVE30NEW, NVL(SUM(TOTAL),0) TOTALNEW FROM ";
                strQry += " (SELECT IN_NO,(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = ";
                strQry += " SUBSTR(DF_LOC_CODE, 0, 1)) CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1) CircleCode, CASE WHEN (IN_DATE - DF_DATE) BETWEEN 0 AND 1 THEN ";
                strQry += " COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE) BETWEEN 2 AND 7 THEN COUNT(*) ELSE 0 END BETWEEN1TO7, ";
                strQry += " CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE) ";
                strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30, CASE WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ";
                strQry += " ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON DT_FDRSLNO = FD_FEEDER_CODE ";
                strQry += " INNER JOIN TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE INNER JOIN TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ";
                strQry += " ON WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 AND DF_STATUS_FLAG   IN(1, 4) ";
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate3 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate3 ";
                    oledbCommand.Parameters.AddWithValue("sFromDate3", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate3", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate4 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate4 ";
                    oledbCommand.Parameters.AddWithValue("sFromDate4", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate4", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate5 ";
                    oledbCommand.Parameters.AddWithValue("sTodate5", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, ";
                strQry += " DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO where IN_NO IS NOT NULL AND TR_IN_NO IS ";
                strQry += " NULL GROUP BY CIRCLE, CircleCode ORDER BY CircleCode)B ON A.CircleCode = B.CircleCode1 ";
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadAllDetails(string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)DIVISIONCODE, ";
                strQry += " NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, NVL(SubDivisionCode, SubDivisionCode1) ";
                strQry += " SUBDIVISIONCODE, NVL(sectioncode, sectioncode1)SECTIONCODE, NVL(SECTION, SectionName1)SECTION, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, ";
                strQry += " NVL(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL,";
                strQry += " NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)BW7TO15NEW, NVL(BW15TO30NEW, 0)BW15TO30NEW, ";
                strQry += " NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW  FROM(SELECT CircleCode, CIRCLE, DivisionCode, DivisionName as Division,";
                strQry += " SubDivisionName as SUBDIVISION, SubDivisionCode, Sectionname as SECTION, sectioncode, SUM(LESSTHAN1)LESSTHAN1DAY, ";
                strQry += " SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL ";
                strQry += " fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)  FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1)) ";
                strQry += " CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   ";
                strQry += " WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, (SELECT SUBSTR(OFF_NAME, ";
                strQry += " INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, ";
                strQry += " SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)   FROM VIEW_ALL_OFFICES ";
                strQry += "  WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4)) SectionName, SUBSTR(DF_LOC_CODE, 0, 4)sectioncode,   CASE WHEN(TR_DECOMM_DATE - DF_DATE)";
                strQry += " BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, CASE   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  ";
                strQry += "  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 ";
                strQry += " END BETWEEN7TO15, CASE    WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30,";
                strQry += " CASE  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL ";
                strQry += " FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE    ";
                strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON    ";
                strQry += " WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON   ";
                strQry += " IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4)  ";
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate";
                    oledbCommand.Parameters.AddWithValue("sFromDate", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate1 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate1";
                    oledbCommand.Parameters.AddWithValue("sFromDate1", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate1", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate2";
                    oledbCommand.Parameters.AddWithValue("sTodate2", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A ";
                strQry += " GROUP BY CIRCLE, CircleCode, ";
                strQry += " DivisionCode, DivisionName, SubDivisionName, SubDivisionCode, SectionName, sectioncode ORDER BY CircleCode,  ";
                strQry += " DivisionCode,SubDivisionCode)A full outer JOIN   (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1,";
                strQry += " SectionName1, sectioncode1, CircleCode AS  CircleCode1, CIRCLE AS CIRCLE1, SUM(LESSTHAN1)LESSTHAN1DAYNEW, ";
                strQry += " SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, ";
                strQry += " SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT  ";
                strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) ";
                strQry += " DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
                strQry += " VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))   SubDivisionName1, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1,";
                strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)   FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4))  ";
                strQry += " SectionName1, SUBSTR(DF_LOC_CODE, 0, 4)  sectioncode1,  CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*) ";
                strQry += " ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7,";
                strQry += " CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE) ";
                strQry += " BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END BETWEEN15TO30, CASE   WHEN(IN_DATE - DF_DATE) > 30 THEN ";
                strQry += " COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON ";
                strQry += " DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN   TBLDTCFAILURE   ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON ";
                strQry += "  DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON   WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND";
                strQry += "  DF_REPLACE_FLAG <> 1 AND  DF_STATUS_FLAG  IN(1, 4)  ";
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate3 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate3";
                    oledbCommand.Parameters.AddWithValue("sFromDate3", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate3", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate4 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate4";
                    oledbCommand.Parameters.AddWithValue("sFromDate4", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate4", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate5";
                    oledbCommand.Parameters.AddWithValue("sTodate5", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE,";
                strQry += " DF_DTC_CODE, FD_FEEDER_NAME)  LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL";
                strQry += " GROUP BY CIRCLE, CircleCode, DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, SectionName1,";
                strQry += " sectioncode1 ORDER BY CircleCode,DivisionCode1)B on   A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1  ";
                strQry += "  and A.SubDivisionCode = b.SubDivisionCode1   and A.sectioncode = B.sectioncode1 ORDER BY";
                strQry += "  NVL(sectioncode, sectioncode1)";
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CirclCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadDiviSionDetails(string CirclCode, string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1) ";
                strQry += " DIVISIONCODE, NVL(Division, DivisionName1)DIVISION, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, ";
                strQry += "  NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL,  ";
                strQry += " NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)  BW7TO15NEW, ";
                strQry += "  NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)  ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT  ";
                strQry += " SUBSTR(DIV_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE  OFF_CODE = SUBSTR(DIV_CODE, 0, 1))CIRCLE, SUBSTR(DIV_CODE, 0, 2)DivisionCode, (SELECT   ";
                strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(DIV_CODE, 0, 2))";
                strQry += "  Division,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM(SELECT CircleCode, CIRCLE, DivisionCode,";
                strQry += "  DivisionName as Division, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, ";
                strQry += "  SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, ";
                strQry += "  INSTR(OFF_NAME, ':') + 1)  FROM    VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, ";
                strQry += "  SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES";
                strQry += " WHERE     OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, ";
                strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, CASE ";
                strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
                strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE   ";
                strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
                strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, ";
                strQry += " COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER   ";
                strQry += "  JOIN  TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  ";
                strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN ";
                strQry += "  TBLTCREPLACE  ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR";
                strQry += "  (DF_LOC_CODE, 0, 1) ='" + CirclCode + "' ";
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
                }
                strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE,";
                strQry += " FD_FEEDER_NAME) A GROUP BY CIRCLE, CircleCode, DivisionCode, DivisionName ORDER BY CircleCode, ";
                strQry += "  DivisionCode)A RIGHT JOIN(SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CICLE_CODE ='" + CirclCode + "')B ON ";
                strQry += "  A.DivisionCode = B.DIV_CODE)A LEFT  JOIN(SELECT SUBSTR(DIV_CODE, 0, 1)CircleCode1, (SELECT SUBSTR(OFF_NAME, ";
                strQry += "  INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(DIV_CODE, 0, 1))CIRCLE1, SUBSTR(DIV_CODE, 0, 2) ";
                strQry += "   DivisionCode1, (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
                strQry += "  WHERE OFF_CODE = SUBSTR(DIV_CODE, 0, 2)) DivisionName1,LESSTHAN1DAYNEW,BW1TO7NEW,BW7TO15NEW,BW15TO30NEW, ";
                strQry += "   ABOVE30NEW,TOTALNEW FROM(SELECT DivisionCode1, DivisionName1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1,";
                strQry += "  SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, ";
                strQry += "  SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, ";
                strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE   ";
                strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT       ";
                strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) ";
                strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 ";
                strQry += "  THEN COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ";
                strQry += "  ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15,";
                strQry += "  CASE   WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
                strQry += "  WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  ";
                strQry += "  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   INNER JOIN TBLDTCFAILURE    ";
                strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN    TBLINDENT ON  ";
                strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1    AND  ";
                strQry += "  DF_STATUS_FLAG IN(1, 4) ";
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
                }
                strQry += "  GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME)  ";
                strQry += "  LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE,";
                strQry += "  CircleCode, DivisionCode1, DivisionName1 ORDER BY CircleCode,DivisionCode1)A RIGHT JOIN(SELECT DIV_CODE ";
                strQry += "  FROM TBLDIVISION WHERE DIV_CICLE_CODE = '" + CirclCode + "')B ON ";
                strQry += "  A.DivisionCode1 = B.DIV_CODE)B on    A.CircleCode = B.CircleCode1  and A.DivisionCode = B.DivisionCode1 ";
                strQry += "  WHERE B.CircleCode1 = '" + CirclCode + "'   ORDER BY NVL(DivisionCode, DivisionCode1), NVL(CIRCLE, CIRCLE1) ";
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DivisionCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadSubDiviSionDetails(string DivisionCode, string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)";
                strQry += " DIVISIONCODE, NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, ";
                strQry += "  NVL(SubDivisionCode, SubDivisionCode1)SUBDIVISIONCODE, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, ";
                strQry += " NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL,";
                strQry += "  NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)  BW7TO15NEW, ";
                strQry += " NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT";
                strQry += " SUBSTR(SD_SUBDIV_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE  OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 1))CIRCLE, SUBSTR(SD_SUBDIV_CODE, 0, 2)DivisionCode, ";
                strQry += " (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES  ";
                strQry += " WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 2)) Division, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
                strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 3)) SUBDIVISION,  SUBSTR(SD_SUBDIV_CODE, 0, 3) ";
                strQry += "  SubDivisionCode,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM (SELECT CircleCode, CIRCLE, ";
                strQry += "  DivisionCode, DivisionName as Division, SubDivisionName as SUBDIVISION, SubDivisionCode, ";
                strQry += "  SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, ";
                strQry += "  SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
                strQry += "  FROM  VIEW_ALL_OFFICES WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1) ";
                strQry += "  CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE   ";
                strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode,";
                strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE ";
                strQry += " OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode,  ";
                strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1,";
                strQry += " CASE WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
                strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE   ";
                strQry += " WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE ";
                strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) ";
                strQry += "  TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER     ";
                strQry += "  JOIN TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN   ";
                strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ";
                strQry += " ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 2) = :DivisionCode   ";
                oledbCommand.Parameters.AddWithValue("DivisionCode", DivisionCode);
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate";
                    oledbCommand.Parameters.AddWithValue("sFromDate", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate1 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate1";
                    oledbCommand.Parameters.AddWithValue("sFromDate1", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate1", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate2";
                    oledbCommand.Parameters.AddWithValue("sTodate2", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE, ";
                strQry += " CircleCode, DivisionCode, DivisionName, SubDivisionName, SubDivisionCode ORDER BY CircleCode, ";
                strQry += " DivisionCode,SubDivisionCode)A RIGHT JOIN(SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_DIV_CODE =:DivisionCode1 )B ";
                strQry += " ON A.SUBDIVISIONCODE = B.SD_SUBDIV_CODE ";
                strQry += " )A LEFT JOIN   (SELECT SUBSTR(SD_SUBDIV_CODE, 0, 1)CircleCode1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
                strQry += " VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 1))CIRCLE1, SUBSTR(SD_SUBDIV_CODE, 0, 2)DivisionCode1, ";
                strQry += " (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES  ";
                strQry += " WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 2)) DivisionNAME1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
                strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 3)) SUBDIVISIONNAME1, ";
                strQry += " SUBSTR(SD_SUBDIV_CODE, 0, 3)SubDivisionCode1,LESSTHAN1DAYNEW,BW1TO7NEW,BW7TO15NEW,BW15TO30NEW,ABOVE30NEW,TOTALNEW";
                strQry += " FROM ";
                strQry += " (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1,";
                strQry += " SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW, ";
                strQry += "  SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM  ";
                strQry += " VIEW_ALL_OFFICES   WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT    ";
                strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2))    ";
                strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName1,   SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1, ";
                strQry += "  CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*)   ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE) ";
                strQry += "  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN ";
                strQry += "  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   ";
                strQry += "  END BETWEEN15TO30, CASE   WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, ";
                strQry += " FD_FEEDER_NAME, COUNT(*) TOTAL    FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   ";
                strQry += " INNER JOIN TBLDTCFAILURE    ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN ";
                strQry += " TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 ";
                strQry += " AND  DF_STATUS_FLAG IN(1, 4) ";
                oledbCommand.Parameters.AddWithValue("DivisionCode1", DivisionCode);
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate3 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate3";
                    oledbCommand.Parameters.AddWithValue("sFromDate3", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate3", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate4 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate4";
                    oledbCommand.Parameters.AddWithValue("sFromDate4", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate4", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate5";
                    oledbCommand.Parameters.AddWithValue("sTodate5", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) ";
                strQry += " LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE,";
                strQry += " CircleCode, DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1 ORDER BY CircleCode,DivisionCode1)A ";
                strQry += " RIGHT JOIN(SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_DIV_CODE =:DivisionCode2)B ON ";
                strQry += "  A.SubDivisionCode1 = B.SD_SUBDIV_CODE)B on    A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1";
                strQry += " and A.SubDivisionCode = b.SubDivisionCode1  WHERE B.DivisionCode1 =:DivisionCode3   ORDER BY NVL(SubDivisionCode, ";
                strQry += " SubDivisionCode1)";
                oledbCommand.Parameters.AddWithValue("DivisionCode2", DivisionCode);
                oledbCommand.Parameters.AddWithValue("DivisionCode3", DivisionCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SubDivisionCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadSectionDetails(string SubDivisionCode, string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)DIVISIONCODE,";
                strQry += " NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, NVL(SubDivisionCode, ";
                strQry += " SubDivisionCode1)SUBDIVISIONCODE, NVL(sectioncode, sectioncode1)SECTIONCODE, NVL(SECTION, SectionName1)SECTION,";
                strQry += " NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, ";
                strQry += "  NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL, NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW,";
                strQry += " NVL(BW7TO15NEW, 0)BW7TO15NEW, NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW ";
                strQry += "  FROM(SELECT SUBSTR(OM_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
                strQry += "  WHERE  OFF_CODE = SUBSTR(OM_CODE, 0, 1))CIRCLE, SUBSTR(OM_CODE, 0, 2)DivisionCode, (SELECT  ";
                strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(OM_CODE, 0, 2)) ";
                strQry += "  Division, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = ";
                strQry += " SUBSTR(OM_CODE, 0, 3)) SUBDIVISION,  SUBSTR(OM_CODE, 0, 3)SubDivisionCode,(SELECT SUBSTR(OFF_NAME, ";
                strQry += " INSTR(OFF_NAME, ':') + 1)   FROM VIEW_ALL_OFFICES WHERE OFF_CODE = OM_CODE) SECTION,OM_CODE as ";
                strQry += " sectioncode,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM(SELECT CircleCode, CIRCLE, ";
                strQry += " DivisionCode, DivisionName as Division, SubDivisionName as SUBDIVISION, SubDivisionCode, ";
                strQry += "  Sectionname as SECTION, sectioncode, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, ";
                strQry += "  SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL ";
                strQry += "  fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)  FROM VIEW_ALL_OFFICES WHERE ";
                strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))  CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT ";
                strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  ";
                strQry += " OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode,";
                strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = ";
                strQry += " SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode,";
                strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)   FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4)) ";
                strQry += " SectionName, SUBSTR(DF_LOC_CODE, 0, 4)sectioncode,   CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN ";
                strQry += " COUNT(*) ELSE 0 END LESSTHAN1, CASE   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END ";
                strQry += " BETWEEN1TO7, CASE   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END BETWEEN7TO15,";
                strQry += " CASE    WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30, CASE ";
                strQry += " WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME,";
                strQry += " COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE";
                strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON  ";
                strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON  ";
                strQry += "  IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 3) =:SubDivisionCode ";
                oledbCommand.Parameters.AddWithValue("SubDivisionCode", SubDivisionCode);
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate ";
                    oledbCommand.Parameters.AddWithValue("sFromDate", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {

                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate1 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate1 ";
                    oledbCommand.Parameters.AddWithValue("sFromDate1", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate1", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate2";
                    oledbCommand.Parameters.AddWithValue("sTodate2", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE,";
                strQry += " CircleCode, DivisionCode, DivisionName, SubDivisionName, SubDivisionCode, SectionName, sectioncode ORDER BY ";
                strQry += " CircleCode,  DivisionCode,SubDivisionCode )A RIGHT JOIN(SELECT OM_CODE FROM TBLOMSECMAST WHERE ";
                strQry += " om_subdiv_code =:SubDivisionCode1)B ON A.SECTIONCODE = B.OM_CODE)A LEFT JOIN(SELECT(SELECT SUBSTR(OFF_NAME, ";
                strQry += " INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(OM_CODE, 0, 1))CIRCLE1,";
                strQry += " SUBSTR(OM_CODE, 0, 1)CircleCode1, SUBSTR(OM_CODE, 0, 2)DivisionCode1, (SELECT   SUBSTR(OFF_NAME, ";
                strQry += " INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(OM_CODE, 0, 2))  DivisionName1,";
                strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(OM_CODE, 0, 3))";
                strQry += " SubDivisionName1, SUBSTR(OM_CODE, 0, 3)SubDivisionCode1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE = OM_CODE) SECTIONNAME1,OM_CODE as sectioncode1,LESSTHAN1DAYNEW,";
                strQry += " BW1TO7NEW,BW7TO15NEW,BW15TO30NEW,ABOVE30NEW,TOTALNEW FROM (SELECT DivisionCode1, DivisionName1, ";
                strQry += " SubDivisionName1, SubDivisionCode1, SectionName1, sectioncode1, CircleCode AS  CircleCode1, ";
                strQry += " CIRCLE AS CIRCLE1, SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, ";
                strQry += " SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT ";
                strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))";
                strQry += " CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
                strQry += " VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)";
                strQry += " DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE ";
                strQry += " OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))   SubDivisionName1, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1,";
                strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4))";
                strQry += " SectionName1, SUBSTR(DF_LOC_CODE, 0, 4) sectioncode1, CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN ";
                strQry += " COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE) BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END ";
                strQry += " BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END BETWEEN7TO15, ";
                strQry += " CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END BETWEEN15TO30, CASE  ";
                strQry += " WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL ";
                strQry += " FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN   TBLDTCFAILURE   ";
                strQry += " ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON   ";
                strQry += " WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 AND  ";
                strQry += " DF_STATUS_FLAG  IN(1, 4)  ";
                oledbCommand.Parameters.AddWithValue("SubDivisionCode1", SubDivisionCode);
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate3 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate3";
                    oledbCommand.Parameters.AddWithValue("sFromDate3", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate3", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate4 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate4";
                    oledbCommand.Parameters.AddWithValue("sFromDate4", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate4", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate5";
                    oledbCommand.Parameters.AddWithValue("sTodate5", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) ";
                strQry += " LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY ";
                strQry += " CIRCLE, CircleCode, DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, SectionName1,";
                strQry += " sectioncode1 ORDER BY CircleCode,DivisionCode1)A RIGHT JOIN(SELECT OM_CODE FROM TBLOMSECMAST WHERE";
                strQry += " om_subdiv_code = :SubDivisionCode2)B ON A.SECTIONCODE1 = B.OM_CODE)B on   A.CircleCode = B.CircleCode1 ";
                strQry += " and A.DivisionCode = B.DivisionCode1  and A.SubDivisionCode = b.SubDivisionCode1 AND ";
                strQry += " A.sectioncode = b.sectioncode1 ";
                strQry += " where b.SubDivisionCode1 =:SubDivisionCode3 ORDER BY  NVL(sectioncode, sectioncode1)";
                oledbCommand.Parameters.AddWithValue("SubDivisionCode2", SubDivisionCode);
                oledbCommand.Parameters.AddWithValue("SubDivisionCode3", SubDivisionCode);
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadCategoryWiseDetails(string SectionCode, string sFromDate, string sTodate, string RoleId)
        {

            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " SELECT nvl(sectioncode, sectioncode1)sectioncode, nvl(FC_ID, FC_ID1)FC_ID, ";
                strQry += " nvl(FC_NAME, FC_NAME1)FC_NAME, nvl(LESSTHAN1DAY, 0) ";
                strQry += " LESSTHAN1DAY, nvl(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15, ";
                strQry += " NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0) ";
                strQry += " TOTAL, NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, ";
                strQry += " NVL(BW7TO15NEW, 0)BW7TO15NEW, NVL(BW15TO30NEW, 0)";
                strQry += " BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW ";
                strQry += " from(SELECT sectioncode, FC_ID, FC_NAME, SUM(LESSTHAN1) ";
                strQry += " LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, ";
                strQry += " SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, ";
                strQry += " SUM(TOTAL)TOTAL  fROM(SELECT  DF_DATE, SUBSTR(DF_LOC_CODE, 0, 4)AS sectioncode, ";
                strQry += " CASE WHEN(TR_DECOMM_DATE - DF_DATE) ";
                strQry += " BETWEEN 0 AND 1 THEN     COUNT(*) ELSE 0 END   LESSTHAN1, ";
                strQry += " CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN ";
                strQry += " COUNT(*)   ELSE 0 END   BETWEEN1TO7, ";
                strQry += " CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 ";
                strQry += " END BETWEEN7TO15, CASE WHEN(TR_DECOMM_DATE - DF_DATE) ";
                strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END    BETWEEN15TO30, ";
                strQry += " CASE   WHEN(TR_DECOMM_DATE - DF_DATE) > 30 ";
                strQry += " THEN COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, FC_NAME, ";
                strQry += " DT_OM_SLNO, COUNT(*) TOTAL, FC_ID  FROM TBLFEEDERMAST ";
                strQry += " INNER JOIN TBLFEEDERCATEGORY ON FD_FC_ID = FC_ID   INNER JOIN ";
                strQry += " TBLDTCMAST ON DT_FDRSLNO = FD_FEEDER_CODE ";
                strQry += " INNER JOIN TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE  ";
                strQry += " INNER JOIN TBLWORKORDER ON DF_ID = WO_DF_ID ";
                strQry += " INNER JOIN TBLINDENT ON WO_SLNO = TI_WO_SLNO ";
                strQry += " INNER JOIN TBLDTCINVOICE ON TI_ID = IN_TI_NO ";
                strQry += " INNER JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND   DF_STATUS_FLAG IN(1, 4) ";
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate";
                    oledbCommand.Parameters.AddWithValue("sFromDate", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate1 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate1";
                    oledbCommand.Parameters.AddWithValue("sFromDate1", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate1", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate2";
                    oledbCommand.Parameters.AddWithValue("sTodate2", sTodate);
                }
                strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FC_NAME, FD_FEEDER_NAME, DT_OM_SLNO, FC_ID) A ";
                if (RoleId != "8")
                {
                    strQry += " WHERE DT_OM_SLNO = :SectionCode ";
                }
                strQry += " GROUP BY FC_NAME, sectioncode, FC_ID)a full outer join ";
                strQry += " (SELECT FC_ID AS FC_ID1, TO_CHAR(FC_NAME)FC_NAME1, TO_CHAR(sectioncode)sectioncode1, TO_CHAR(LESSTHAN1)LESSTHAN1DAYNEW, ";
                strQry += "  TO_CHAR(BETWEEN1TO7)BW1TO7NEW, TO_CHAR(BETWEEN7TO15)BW7TO15NEW, TO_CHAR(BETWEEN15TO30)BW15TO30NEW, TO_CHAR(ABOVE30)ABOVE30NEW, ";
                strQry += "  TO_CHAR(TOTAL)TOTALNEW FROM(select FC_ID, FC_NAME, sectioncode, sum(LESSTHAN1)LESSTHAN1, sum(BETWEEN1TO7)BETWEEN1TO7, ";
                strQry += "  sum(BETWEEN7TO15)BETWEEN7TO15, sum(BETWEEN15TO30)BETWEEN15TO30, sum(ABOVE30)ABOVE30, sum(TOTAL) TOTAL  from(SELECT FC_ID,";
                strQry += " FC_NAME, DF_LOC_CODE AS sectioncode, CASE WHEN(IN_DATE - DF_DATE)   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END   LESSTHAN1, ";
                strQry += " CASE WHEN(IN_DATE - DF_DATE)   BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0  END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  ";
                strQry += " BETWEEN 8 AND 15 THEN     COUNT(*) ELSE 0 END  BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ";
                strQry += " ELSE 0 END     BETWEEN15TO30, CASE  WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE)  ELSE 0 END ABOVE30, COUNT(*) TOTAL ";
                strQry += "  FROM TBLDTCFAILURE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON WO_SLNO = TI_WO_SLNO   INNER JOIN ";
                strQry += "  TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLFEEDERMAST ON  SUBSTR(df_dtc_code, 0, 4) = FD_FEEDER_CODE  INNER JOIN ";
                strQry += "  TBLFEEDERCATEGORY on fc_id = FD_FC_ID  AND IN_NO NOT IN(SELECT TR_IN_NO FROM TBLTCREPLACE) ";
                if (RoleId != "8")
                {
                    strQry += " AND DF_LOC_CODE =:SectionCode1  ";
                }
                oledbCommand.Parameters.AddWithValue("SectionCode", SectionCode);
                oledbCommand.Parameters.AddWithValue("SectionCode1", SectionCode);
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate3 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate3 ";
                    oledbCommand.Parameters.AddWithValue("sFromDate3", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate3", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate4 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate4 ";
                    oledbCommand.Parameters.AddWithValue("sFromDate4", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate4", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate5 ";
                    oledbCommand.Parameters.AddWithValue("sTodate5", sTodate);
                }
                strQry += " GROUP BY IN_DATE, DF_DATE, FC_NAME, DF_LOC_CODE, FC_ID) ";
                strQry += " GROUP by FC_NAME, sectioncode, FC_ID))b on a.FC_ID = b.FC_ID1 and ";
                strQry += " sectioncode = sectioncode1 and FC_NAME = FC_NAME1 ";
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="SectionCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadFeederDetails(string CategoryId, string SectionCode, string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " (SELECT TO_CHAR(sectioncode)sectioncode, TO_CHAR(FC_ID)FC_ID,TO_CHAR(FD_FEEDER_NAME)FD_FEEDER_NAME, ";
                strQry += " TO_CHAR(TC_CODE)TC_CODE,TO_CHAR(tc_slno)TC_SLNO,TO_CHAR(WO_NO) AS COMISSIONWONO, TO_CHAR(WO_NO_DECOM)AS ";
                strQry += " DECOMISSIONWONO, TO_CHAR(DF_DATE, 'DD-MON-YYYY')AS FAILUREDATE, STATUS, TO_CHAR(DT_CODE)DT_CODE ";
                strQry += " fROM(SELECT DT_CODE, DT_TC_ID, WO_NO_DECOM, WO_NO, TC_CODE, TC_SLNO, DF_DATE, SUBSTR(DF_LOC_CODE, 0, 4)AS sectioncode,";
                strQry += " FD_FEEDER_NAME, FC_NAME, DT_OM_SLNO, FC_ID, 'COMPLETED' AS STATUS  FROM TBLFEEDERMAST INNER JOIN TBLFEEDERCATEGORY ";
                strQry += " ON FD_FC_ID = FC_ID   INNER JOIN  TBLDTCMAST ON DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE ";
                strQry += " INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON WO_SLNO = TI_WO_SLNO INNER JOIN ";
                strQry += " TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  INNER JOIN TBLTCMASTER ON   ";
                strQry += " DF_EQUIPMENT_ID = TC_CODE  AND DF_REPLACE_FLAG <> 1  AND   DF_STATUS_FLAG IN(1, 4)    GROUP BY DF_ID, DF_DATE, ";
                strQry += " TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FC_NAME, FD_FEEDER_NAME, DT_OM_SLNO, FC_ID, dt_code, TC_CODE,";
                strQry += " TC_SLNO, WO_NO, WO_NO_DECOM, TR_DECOMM_DATE, DT_TC_ID, DT_CODE) A WHERE DT_OM_SLNO = :SectionCode   AND FC_ID = :CategoryId";
                oledbCommand.Parameters.AddWithValue("SectionCode", SectionCode);
                oledbCommand.Parameters.AddWithValue("CategoryId", CategoryId);
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate";
                    oledbCommand.Parameters.AddWithValue("sFromDate", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(
                        sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sFromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(
                        sTodate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    sTodate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate1 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate1";
                    oledbCommand.Parameters.AddWithValue("sFromDate1", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate1", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate2";
                    oledbCommand.Parameters.AddWithValue("sTodate2", sTodate);
                }
                strQry += " GROUP BY sectioncode, FC_ID, FD_FEEDER_NAME, TC_CODE, tc_slno, DF_DATE, WO_NO, WO_NO_DECOM, STATUS, TC_CODE, DT_CODE) ";
                strQry += " UNION ALL (SELECT TO_CHAR(DF_LOC_CODE)sectioncode, TO_CHAR(FC_ID)FC_ID, TO_CHAR(FD_FEEDER_NAME)FD_FEEDER_NAME, ";
                strQry += " TO_CHAR(TC_CODE)TC_CODE, TO_CHAR(tc_slno)TC_SLNO, TO_CHAR(WO_NO) AS COMISSIONWONO, TO_CHAR(WO_NO_DECOM)  ";
                strQry += " AS DECOMISSIONWONO, TO_CHAR(DF_DATE, 'DD-MON-YYYY')AS FAILUREDATE, 'PENDING' AS STATUS, DF_DTC_CODE AS DT_CODE ";
                strQry += " FROM TBLDTCFAILURE INNER JOIN    TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON WO_SLNO = TI_WO_SLNO ";
                strQry += " INNER JOIN TBLDTCINVOICE   ON TI_ID = IN_TI_NO INNER JOIN TBLFEEDERMAST ON  SUBSTR(df_dtc_code, 0, 4) = FD_FEEDER_CODE INNER JOIN ";
                strQry += " TBLFEEDERCATEGORY on fc_id = FD_FC_ID INNER JOIN TBLTCMASTER ON  DF_EQUIPMENT_ID = TC_CODE INNER JOIN TBLDTCMAST ";
                strQry += " ON DT_FDRSLNO = FD_FEEDER_CODE AND IN_NO NOT IN(SELECT TR_IN_NO FROM TBLTCREPLACE) ";
                strQry += " AND DF_LOC_CODE =:SectionCode1  AND FC_ID =:CategoryId1 ";
                oledbCommand.Parameters.AddWithValue("SectionCode1", SectionCode);
                oledbCommand.Parameters.AddWithValue("CategoryId1", CategoryId);
                if (sFromDate != "" && sTodate != "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate3 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate3";
                    oledbCommand.Parameters.AddWithValue("sFromDate3", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate3", sTodate);
                }
                else if (sFromDate != "" && sTodate == "")
                {
                    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>=:sFromDate4 AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate4";
                    oledbCommand.Parameters.AddWithValue("sFromDate4", sFromDate);
                    oledbCommand.Parameters.AddWithValue("sTodate4", sTodate);
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<=:sTodate5";
                    oledbCommand.Parameters.AddWithValue("sTodate5", sTodate);
                }
                strQry += " GROUP BY IN_DATE, DF_DATE, DF_LOC_CODE, FD_FEEDER_NAME, FC_ID, TC_CODE, TC_SLNO, WO_NO_DECOM, WO_NO, DF_DTC_CODE)";
                dtDetails = ObjCon.getDataTable(strQry, oledbCommand);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtDetails;
            }
        }
    }
}
