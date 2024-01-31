using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsEstimation
    {
        string strFormCode = "clsEstimation";

        public string sOfficeCode { get; set; }
        public string sFailureId { get; set; }
        public string sEstimationNo { get; set; }
        public string sFaultCapacity { get; set; }
        public string sReplaceCapacity { get; set; }
        public string sUnit { get; set; }
        public string sQuantity { get; set; }
        public string sUnitPrice { get; set; }
        public string sAmount { get; set; }
        public string sUnitLabour { get; set; }
        public string sTotalLabour { get; set; }
        public string sLabourCharge { get; set; }
        public string s10PercLabourCharge { get; set; }
        public string sContig2Perc { get; set; }
        public string sTotal { get; set; }
        public string sDecommUnitPrice { get; set; }
        public string sDecommUnitLabour { get; set; }
        public string sDecommTotalLabour { get; set; }
        public string sDecommLabourCharge { get; set; }
        public string sDecomm10PercLabourCharge { get; set; }
        public string sDecommContig2Perc { get; set; }
        public string sDecommTotal { get; set; }
        public string sLastRepair { get; set; }

        public string sCrby { get; set; }

        public string sEmployeeCost { get; set; }
        public string sESI { get; set; }
        public string ServiceTax { get; set; }
        public string DecomLabourCost { get; set; }

        public string CGST { get; set; }
        public string SGST { get; set; }


        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        public string GenerateEstimationNo(string sOfficeCode)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string sMaxNo = string.Empty;
                oledbCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sEstNo = ObjCon.get_value("SELECT MAX(EST_NO)+1 FROM TBLESTIMATION WHERE EST_NO LIKE:sOfficeCode||'%' ", oledbCommand);
                if (sEstNo == "")
                {
                    //sMaxNo = "001";
                    sEstNo = sOfficeCode + "001";
                }
                //else
                //{
                //    sEstNo = ObjCon.get_value("SELECT MAX(EST_ID)+1 FROM TBLESTIMATION WHERE EST_NO LIKE '" + sOfficeCode + "%'");
                //}            

                return sEstNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateEstimationNo");
                return ex.Message;
            }
            finally
            {

            }
        }

        public string GenerateEstimationNoZoneWise(string sOfficeCode)
        {
            string tempOfficeCode = string.Empty;
            string sEstNo = string.Empty;
            string strQry = string.Empty;

            try
            {
                oledbCommand = new OleDbCommand();
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                if (sOfficeCode.StartsWith("4"))
                {
                    tempOfficeCode = "2" + sOfficeCode; // for hassan  zone  
                }
                else
                {
                    tempOfficeCode = "1" + sOfficeCode; // for other zone  
                }
                
                // tempOfficeCode = Convert.ToString(ConfigurationSettings.AppSettings["MYSOREZONE"]) + sOfficeCode;

                oledbCommand.Parameters.AddWithValue("sOfficeCode", tempOfficeCode);
               // strQry = "SELECT NVL(MAX(EST_NO),0)+1 FROM TBLESTIMATION WHERE EST_NO LIKE '"+tempOfficeCode+"%' " ;
                strQry = "SELECT NVL(MAX(EST_NO),0)+1 FROM TBLESTIMATION WHERE EST_OFF_CODE = '" + sOfficeCode + "' ";

                sEstNo = ObjCon.get_value(strQry);
                if (sEstNo.Length == 1)
                {

                    //4  digit Section Code with 1920 as financial year
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }

                    sEstNo = tempOfficeCode + sFinancialYear + "001";
                }
                else
                {
                    //4 digit Section Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sFinancialYear == sEstNo.Substring(5,4))
                        {
                            return sEstNo;
                        }
                        else
                        {
                            sEstNo = sOfficeCode + sFinancialYear + "001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        return sEstNo;
                    }
                }
                return sEstNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateEstimationNoZoneWise" , "tempOfficeCode = "+ tempOfficeCode + " estimationno = " + sEstNo + " query = " + strQry );
                return "FAILURE";
            }
            finally
            {

            }
        }

        public string SaveEstimationDetails(clsEstimation objEstimation)
        {
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sFailureId", objEstimation.sFailureId);
                OleDbDataReader dr = ObjCon.Fetch("SELECT * FROM TBLESTIMATION WHERE EST_DF_ID=:sFailureId", oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    return "SUCCESS";
                }
                dr.Close();
                string sMaxNo = Convert.ToString(ObjCon.Get_max_no("EST_ID", "TBLESTIMATION"));

                GetCommEstimatedDetails(objEstimation);
                GetDecomEstimatedDetails(objEstimation);

                //objEstimation.sEstimationNo = GenerateEstimationNo(objEstimation.sOfficeCode);
                objEstimation.sEstimationNo = GenerateEstimationNoZoneWise(objEstimation.sOfficeCode);
                if (objEstimation.sEstimationNo == "FAILURE")
                {
                    return "FAILURE";
                }
                oledbCommand = new OleDbCommand();
                // OleDbCommand oledbCommand;
                strQry = "INSERT INTO TBLESTIMATION (EST_ID,EST_DF_ID,EST_NO,EST_FAULT_CAPACITY,EST_REPLACE_CAPACITY,EST_UNIT,EST_QUANTY,";
                strQry += " EST_UNIT_PRICE,EST_AMOUNT,EST_UNIT_LABOUR,EST_TOTAL_LABOUR,EST_LABOUR_CHARGE,EST_10PERC_LABOUR_CHARGE,EST_CONTIG_2PERC,";
                strQry += " EST_TOTAL,EST_DECOM_UNIT_PRICE,EST_DECOM_UNIT_LABOUR,EST_DECOM_TOTAL_LABOUR,EST_DECOM_LABOUR_CHARGE,EST_DECOM_10PERC_LABOUR_CHARGE,";
                strQry += " EST_DECOM_CONTIG_2PERC,EST_DECOM_TOTAL,EST_CRBY,EST_REPAIR, EST_OFF_CODE) VALUES (";
                strQry += "  '" + sMaxNo + "', '" + objEstimation.sFailureId + "', '" + objEstimation.sEstimationNo + "', '" + objEstimation.sFaultCapacity + "', '" + objEstimation.sReplaceCapacity + "',";
                strQry += " '" + objEstimation.sUnit + "', '" + objEstimation.sQuantity + "','" + objEstimation.sUnitPrice + "','" + objEstimation.sAmount + "',";
                strQry += " '" + objEstimation.sUnitLabour + "','" + objEstimation.sTotalLabour + "','" + objEstimation.sLabourCharge + "','" + objEstimation.s10PercLabourCharge + "',";
                strQry += " '" + objEstimation.sContig2Perc + "','" + objEstimation.sTotal + "','" + objEstimation.sDecommUnitPrice + "',";
                strQry += " '" + objEstimation.sDecommUnitLabour + "','" + objEstimation.sDecommTotalLabour + "','" + objEstimation.sDecommLabourCharge + "',";
                strQry += " '" + objEstimation.sDecomm10PercLabourCharge + "','" + objEstimation.sDecommContig2Perc + "','" + objEstimation.sDecommTotal + "',";
                strQry += " '" + objEstimation.sCrby + "','" + objEstimation.sLastRepair + "', '" + objEstimation.sOfficeCode + "')";


                //oledbCommand.Parameters.AddWithValue("sMaxNo", sMaxNo);
                //oledbCommand.Parameters.AddWithValue("sFailureId1", objEstimation.sFailureId);
                //oledbCommand.Parameters.AddWithValue("sEstimationNo", objEstimation.sEstimationNo);
                //oledbCommand.Parameters.AddWithValue("sFaultCapacity", objEstimation.sFaultCapacity);
                //oledbCommand.Parameters.AddWithValue("sReplaceCapacity", objEstimation.sReplaceCapacity);
                //oledbCommand.Parameters.AddWithValue("sUnit", objEstimation.sUnit);
                //oledbCommand.Parameters.AddWithValue("sQuantity", objEstimation.sQuantity);
                //oledbCommand.Parameters.AddWithValue("sUnitPrice", objEstimation.sUnitPrice);
                //oledbCommand.Parameters.AddWithValue("sAmount", objEstimation.sAmount);
                //oledbCommand.Parameters.AddWithValue("sUnitLabour", objEstimation.sUnitLabour);
                //oledbCommand.Parameters.AddWithValue("sTotalLabour", objEstimation.sTotalLabour);

                //oledbCommand.Parameters.AddWithValue("sLabourCharge", objEstimation.sLabourCharge);
                //oledbCommand.Parameters.AddWithValue("s10PercLabourCharge", objEstimation.s10PercLabourCharge);
                //oledbCommand.Parameters.AddWithValue("sContig2Perc", objEstimation.sContig2Perc);
                //oledbCommand.Parameters.AddWithValue("sTotal", objEstimation.sTotal);
                //oledbCommand.Parameters.AddWithValue("sDecommUnitPrice", objEstimation.sDecommUnitPrice);
                //oledbCommand.Parameters.AddWithValue("sDecommUnitLabour", objEstimation.sDecommUnitLabour);
                //oledbCommand.Parameters.AddWithValue("sDecommTotalLabour", objEstimation.sDecommTotalLabour);
                //oledbCommand.Parameters.AddWithValue("sDecommLabourCharge", objEstimation.sDecommLabourCharge);
                //oledbCommand.Parameters.AddWithValue("sDecomm10PercLabourCharge", objEstimation.sDecomm10PercLabourCharge);

                //oledbCommand.Parameters.AddWithValue("sDecommContig2Perc", objEstimation.sDecommContig2Perc);
                //oledbCommand.Parameters.AddWithValue("sDecommTotal", objEstimation.sDecommTotal);
                //oledbCommand.Parameters.AddWithValue("sCrby", objEstimation.sCrby);
                //oledbCommand.Parameters.AddWithValue("sLastRepair", objEstimation.sLastRepair);
                //  oledbCommand.Parameters.AddWithValue("sDecommUnitPrice", objEstimation.sDecommUnitPrice);

                ObjCon.Execute(strQry);
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveEstimationDetails", strQry);
                return "FAILURE";

            }
            finally
            {

            }
        }


        public clsEstimation GetCommEstimatedDetails(clsEstimation objEstimation)
        {

            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();

                sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
                sESI = ConfigurationSettings.AppSettings["ESI"];
                CGST = ConfigurationSettings.AppSettings["CGST"];
                SGST = ConfigurationSettings.AppSettings["SGST"];
                ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];

                DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];

                strQry = " select DF_DTC_CODE,DF_EQUIPMENT_ID,TO_CHAR(DF_DATE,'dd/MM/yyyy')DF_DATE,DF_LOC_CODE,(SELECT SD_SUBDIV_NAME FROM ";
                strQry += " TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) as SubDivision,(SELECT OM_NAME FROM TBLOMSECMAST ";
                strQry += " where OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) as Location,'No' AS Unit,'1' as Quantity,(select TO_CHAR(TC_CAPACITY) ";
                strQry += " from TBLTCMASTER where TC_CODE=DF_EQUIPMENT_ID) Capacity,";
                strQry += " TE_RATE as Price,1*TE_RATE AS TotalAmount,TE_COMMLABOUR as labourcharge,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,";
                strQry += " ((TE_RATE+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+TE_COMMLABOUR)/100)*2 as ContingencyCost,(TE_RATE+TE_COMMLABOUR+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+((TE_COMMLABOUR*'" + sESI + "')/100)+((TE_COMMLABOUR*'" + ServiceTax + "')/100) +((TE_RATE+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+TE_COMMLABOUR)/100)*2) as FinalTotal";
                strQry += " FROM TBLDTCFAILURE,TBLITEMMASTER,TBLTCMASTER where DF_ID=:sFailureId AND TC_CODE=DF_EQUIPMENT_ID AND  TC_CAPACITY=TE_CAPACITY ";
                strQry += " AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0)";
                oledbCommand.Parameters.AddWithValue("sFailureId", objEstimation.sFailureId);
                dtDetailedReport = ObjCon.getDataTable(strQry, oledbCommand);
                if (dtDetailedReport.Rows.Count > 0)
                {
                    objEstimation.sFaultCapacity = Convert.ToString(dtDetailedReport.Rows[0]["Capacity"]);
                    objEstimation.sReplaceCapacity = Convert.ToString(dtDetailedReport.Rows[0]["Capacity"]);
                    objEstimation.sUnit = Convert.ToString(dtDetailedReport.Rows[0]["Unit"]);
                    objEstimation.sQuantity = Convert.ToString(dtDetailedReport.Rows[0]["Quantity"]);
                    objEstimation.sUnitPrice = Convert.ToString(dtDetailedReport.Rows[0]["Price"]);
                    objEstimation.sAmount = Convert.ToString(dtDetailedReport.Rows[0]["TotalAmount"]);
                    objEstimation.sUnitLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sTotalLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.s10PercLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["EmployeeCost"]);
                    objEstimation.sContig2Perc = Convert.ToString(dtDetailedReport.Rows[0]["ContingencyCost"]);
                    objEstimation.sTotal = Convert.ToString(dtDetailedReport.Rows[0]["FinalTotal"]);
                }
                return objEstimation;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCommEstimatedDetails");
                return objEstimation;

            }
            finally
            {

            }
        }

        public clsEstimation GetDecomEstimatedDetails(clsEstimation objEstimation)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = " SELECT DF_EQUIPMENT_ID,to_char(DF_DATE,'dd/MM/yyyy')DF_DATE,DF_REASON,DF_LOC_CODE,TO_CHAR(DF_CRON,'dd/MM/yyyy')DF_CRON,";
                strQry += " TE_RATE as Price ,(1*TE_COMMLABOUR*'" + DecomLabourCost + "') as labourcharge,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_CODE,TC_SLNO,'OLD' AS Rep,";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID)TM_NAME,DT_TOTAL_CON_KW,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,";
                strQry += " ((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2 as ContingencyCost, ";
                strQry += " ((TE_COMMLABOUR*'" + DecomLabourCost + "')+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+((TE_COMMLABOUR*'" + sESI + "')/100)+((TE_COMMLABOUR*'" + ServiceTax + "')/100)+((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2) as FinalTotal,";
                strQry += " 'No' as Unit,'1' as Quantity,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) as SubDivision ,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) as Location ";
                strQry += "  from  TBLDTCFAILURE,TBLITEMMASTER,TBLTCMASTER,TBLDTCMAST WHERE DF_DTC_CODE=DT_CODE AND DF_EQUIPMENT_ID=TC_CODE AND TC_CAPACITY=TE_CAPACITY ";
                strQry += " AND DF_ID=:sFailureId AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0)";
                oledbCommand.Parameters.AddWithValue("sFailureId", objEstimation.sFailureId);
                dtDetailedReport = ObjCon.getDataTable(strQry, oledbCommand);
                if (dtDetailedReport.Rows.Count > 0)
                {
                    objEstimation.sDecommUnitPrice = Convert.ToString(dtDetailedReport.Rows[0]["Price"]);
                    //objEstimation.s = Convert.ToString(dtDetailedReport.Rows[0]["TotalAmount"]);
                    objEstimation.sDecommUnitLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sDecommTotalLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sDecommLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sDecomm10PercLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["EmployeeCost"]);
                    objEstimation.sDecommContig2Perc = Convert.ToString(dtDetailedReport.Rows[0]["ContingencyCost"]);
                    objEstimation.sDecommTotal = Convert.ToString(dtDetailedReport.Rows[0]["FinalTotal"]);
                }
                return objEstimation;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDecomEstimatedDetails");
                return objEstimation;

            }
            finally
            {

            }
        }




        public clsEstimation GetCommAndDecommAmount(clsEstimation objEst)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT EST_TOTAL,EST_DECOM_TOTAL FROM TBLESTIMATION WHERE EST_DF_ID=:sFailureId";
                oledbCommand.Parameters.AddWithValue("sFailureId", objEst.sFailureId);
                DataTable dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objEst.sTotal = Convert.ToString(dt.Rows[0]["EST_TOTAL"]);
                    objEst.sDecommTotal = Convert.ToString(dt.Rows[0]["EST_DECOM_TOTAL"]);
                }

                return objEst;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCommAndDecommAmount");
                return objEst;

            }
            finally
            {

            }
        }
    }
}
