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
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsBackEndActivity
    {
        string strFormCode = "clsBackEndActivity";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sFeederCode { get; set; }
        public string sDtcCode { get; set; }
        public string sListBoxValue1 { get; set; }
        public string sListBoxValue2 { get; set; }

        //public string sDtcCode { get; set; }
        public string sDTrCode { get; set; }
        public string sPhotoType { get; set; }
        public string sColumnName { get; set; }

        public string sEnumId { get; set; }
        public string sSectionCode { get; set; }
        public string sSecondSectionCode { get; set; }
        public string sDTcCode { get; set; }
        public string oldSectionCode { get; set; }
        public string ticketNumber { get; set; }
        public string createdBy { get; set; }


        /// <summary>
        /// Delete Data FeederWise and Particular DTC
        /// </summary>
        /// <param name="objFeeder"></param>
        /// <returns></returns>
        public string[] DeleteRecordFeederDTCWise(clsBackEndActivity objFeeder)
        {
            string[] Arr = new string[2];
           
            try
            {
                string strQry = string.Empty;

                if (objFeeder.sFeederCode != "" && objFeeder.sListBoxValue1 == null && objFeeder.sListBoxValue2 == null)
                {

                    strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='5',ED_NOTES='DELETE FROM BACK END UI',ED_NOTES_ON=SYSDATE ";
                    strQry += " WHERE ED_FEEDERCODE='" + objFeeder.sFeederCode + "'";
                    ObjCon.Execute(strQry);
                    Arr[0] = "Deleted Successfully ";
                    Arr[1] = "0";
                    return Arr;

                }

                if (objFeeder.sFeederCode != "" && objFeeder.sListBoxValue1 != null && objFeeder.sListBoxValue2 != null)
                {
                    strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='5',ED_NOTES='DELETE FROM BACK END UI',ED_NOTES_ON=SYSDATE WHERE ED_FEEDERCODE='" + objFeeder.sFeederCode + "'";
                    strQry += "  AND ED_ID IN (SELECT DTE_ED_ID FROM TBLDTCENUMERATION";
                    strQry += "  WHERE DTE_DTCCODE between '" + objFeeder.sListBoxValue1 + "' and '" + objFeeder.sListBoxValue2 + "')";
                    ObjCon.Execute(strQry);
                    Arr[0] = "Deleted Successfully ";
                    Arr[1] = "0";
                    return Arr;
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DeleteFeederDetails");
                return Arr;
            }
        }

        /// <summary>
        /// Delete Photos DTC Wise(need to SELECT Photo Type)
        /// objPhotos.sColumnName Specifies which Photo Type Column need to be empty
        /// </summary>
        /// <param name="objPhotos"></param>
        /// <returns></returns>

        public string[] DeletePhotos(clsBackEndActivity objBackEnd)
        {
            string[] Arr = new string[2];         
            try
            {
                string strQry = string.Empty;
                if (objBackEnd.sDtcCode != "" && objBackEnd.sColumnName != "")
                {
                    strQry = "UPDATE TBLENUMERATIONPHOTOS SET " + objBackEnd.sColumnName.ToString() + " = ''  WHERE EP_ED_ID IN (SELECT DTE_ED_ID FROM ";
                    strQry += " TBLDTCENUMERATION where DTE_DTCCODE='" + objBackEnd.sDtcCode + "') ";

                    ObjCon.Execute(strQry);
                    Arr[0] = "Photo Deleted Successfully ";
                    Arr[1] = "0";
                    return Arr;
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DeletePhotos");
                return Arr;
            }
        }


        /// <summary>
        /// Get DTC count for Particular Feeder and Section
        /// </summary>
        /// <param name="objBackEnd"></param>
        /// <returns></returns>
        public string GenerateCount(clsBackEndActivity objBackEnd)
        {
            try
            {
                string sMaxNo = string.Empty;
                string strQry = string.Empty;

                strQry = " SELECT count(DTE_DTCCODE) FROM TBLDTCENUMERATION,";
                strQry += " TBLENUMERATIONDETAILS WHERE ED_ID=DTE_ED_ID and ED_STATUS_FLAG=0 and ED_FEEDERCODE = '" + objBackEnd.sFeederCode + "' ";
                strQry += " and ED_OFFICECODE = '" + objBackEnd.sSectionCode + "'";
                string sFeederCountNo = ObjCon.get_value(strQry);
                return sFeederCountNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateCount");
                return "";
            }
        }



        public DataTable LoadAllDTRDetails(clsBackEndActivity objFeederCode)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "select DTE_ED_ID,DTE_DTCCODE,DTE_TC_CODE,DTE_TC_SLNO,ED_OFFICECODE,(SELECT OFF_NAME";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=ED_OFFICECODE ) AS OFFICEnAME ";
                strQry += " from TBLDTCENUMERATION,TBLENUMERATIONDETAILS WHERE ED_ID=DTE_ED_ID AND ED_STATUS_FLAG=0";
                strQry += " and DTE_DTCCODE LIKE '" + objFeederCode.sDtcCode + "%'";
                strQry += " and DTE_TC_CODE LIKE '" + objFeederCode.sDTrCode + "%' ";
          
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllDTRDetails");
                return dt;
            }

        }


        /// <summary>
        /// Delete Duplicate record, DTC or DTR wise
        /// </summary>
        /// <param name="objBackEnd"></param>
        /// <returns></returns>
        public string[] DeleteDuplicateRecord(clsBackEndActivity objBackEnd)
        {
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                strQry = "  UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='5',ED_NOTES='DELETE FROM BACK END UI',ED_NOTES_ON=SYSDATE ";
                strQry += " WHERE ED_ID='" + objBackEnd.sEnumId + "' ";

                ObjCon.Execute(strQry);
                Arr[0] = "Deleted Successfully ";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DeleteDuplicateDetails");
                return Arr;
            }

        }


        /// <summary>
        /// Move Selected Data from One Section to Another Section
        /// </summary>
        /// <param name="objBackEnd"></param>
        /// <returns></returns>
        public string[] MoveToRequiredSection(clsBackEndActivity objBackEnd)
        {
            string[] Arr = new string[2];
     
            try
            {
                string strQry = string.Empty;
                if (objBackEnd.sSecondSectionCode != "" && objBackEnd.sFeederCode != "")
                {
                    #region old query
                    //strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_OFFICECODE='" + objBackEnd.sSecondSectionCode + "',";
                    //strQry += " ED_NOTES='UPDATE LOCATION IN BACKEND FROM  " + objBackEnd.sSectionCode + " TO " + objBackEnd.sSecondSectionCode + "',";
                    //strQry += " ED_NOTES_ON=SYSDATE  where ED_FEEDERCODE='" + objBackEnd.sFeederCode + "' and ED_OFFICECODE = '" + objBackEnd.sSectionCode + "' ";
                    //ObjCon.Execute(strQry);
                    //Arr[0] = "Data Moved Successfully ";
                    //Arr[1] = "0";
                    //return Arr;
                    #endregion

                    strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_OFFICECODE='" + objBackEnd.sSecondSectionCode + "',";
                    strQry += " ED_NOTES='UPDATE LOCATION IN BACKEND FROM  " + objBackEnd.sSectionCode + " TO " + objBackEnd.sSecondSectionCode + "',";
                    strQry += " ED_NOTES_ON=SYSDATE  where ED_FEEDERCODE='" + objBackEnd.sFeederCode + "' and ED_OFFICECODE = '" + objBackEnd.sSectionCode + "' ";
                    ObjCon.Execute(strQry);
                    Arr[0] = "Data Moved Successfully ";
                    Arr[1] = "0";
                    return Arr;
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "MoveToRequiredSection");
                return Arr;
            }
        }

        public DataTable GetFailDetails( string DtcCode)
        {
            DataTable dtFailDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT DF_ID,DF_REPLACE_FLAG,DF_DTC_CODE,DF_EQUIPMENT_ID,FAILURE_TYPE FROM ((SELECT DF_ID,CASE WHEN DF_REPLACE_FLAG=0 THEN 'CR PENDING' ";
                strQry += "WHEN DF_REPLACE_FLAG=1 THEN 'CR COMPLETED' END DF_REPLACE_FLAG,DF_DTC_CODE,DF_EQUIPMENT_ID,CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN 'FAILURE' ";
                strQry += "WHEN DF_ENHANCE_CAPACITY IS NOT NULL THEN 'ENHANCEMENT' END AS FAILURE_TYPE FROM TBLDTCFAILURE WHERE DF_DTC_CODE='" + DtcCode + "')UNION ALL";
                strQry += "(SELECT 0 AS DF_ID, 'PENDING' AS DF_REPLACE_FLAG, WO_DATA_ID AS DF_DTC_CODE,0 AS DF_EQUIPMENT_ID,CASE WHEN WO_DESCRIPTION LIKE 'Failure%' THEN ";
                strQry += "'JUST DECLARE FAILURE' WHEN WO_DESCRIPTION LIKE 'Enhance%' THEN 'JUST DECLARE ENHANCEMENT' END AS FAILURE_TYPE FROM TBLWORKFLOWOBJECTS WHERE ";
                strQry += "WO_DATA_ID='"+ DtcCode +"' AND WO_RECORD_ID LIKE '-%'))";
                dtFailDetails = ObjCon.getDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailDetails");
            }
            return dtFailDetails;
        }

        public string[] DeleteData(string sDfId, string sDtcCode)
        {
            string[] arr = new string[2];
            string StrQry = string.Empty;
            string sWoSlno, sTiId, sInNo, sTrId,sTcId;
            int k = 1, BOid = 0;
            string sDtcId = string.Empty;
            string sDrtId = string.Empty;
            string[] ArrBOID = {"9","11","12","29","13","14","15","26" };
            DataTable dtAllId = new DataTable();
            DataTable dtWO = new DataTable();
            DataTable dtWFOAuto = new DataTable();
            DataTable dtWF = new DataTable();
            int count=0;
            try
            {
                StrQry = "SELECT DF_DTC_CODE,DF_ID,WO_SLNO,TI_ID,IN_NO,TR_ID,TR_RI_NO,TR_RV_NO,TR_INVENTORY_QTY,DF_EQUIPMENT_ID FROM TBLDTCFAILURE LEFT JOIN TBLWORKORDER ON ";
                StrQry += "DF_ID=WO_DF_ID LEFT JOIN TBLINDENT ON WO_SLNO=TI_WO_SLNO LEFT JOIN TBLDTCINVOICE ON IN_TI_NO=TI_ID LEFT JOIN TBLTCREPLACE ON TR_IN_NO=IN_NO ";
                StrQry += "WHERE DF_DTC_CODE='" + sDtcCode + "' AND DF_ID='" + sDfId + "'";
                dtAllId = ObjCon.getDataTable(StrQry);

                if (dtAllId.Rows.Count > 0) // after approved from sdo for estimation in failre. If not then control will not come inside
                {

                    StrQry = "SELECT DRT_ID FROM TBLDTRTRANSACTION WHERE DRT_DTR_CODE='" + dtAllId.Rows[0]["DF_EQUIPMENT_ID"] + "' AND DRT_DESC LIKE '%FAILURE%' AND ";
                    StrQry += " DRT_ACT_REFNO='" + dtAllId.Rows[0]["DF_ID"] + "'";
                    sDrtId = ObjCon.get_value(StrQry);

                    StrQry = "SELECT DCT_ID FROM TBLDTCTRANSACTION WHERE DCT_DTC_CODE='" + dtAllId.Rows[0]["DF_DTC_CODE"] + "' AND DCT_DESC LIKE '%FAILURE%' AND ";
                    StrQry += " DCT_ACT_REFNO='" + dtAllId.Rows[0]["DF_ID"] + "'";
                    sDtcId = ObjCon.get_value(StrQry);

                    for (int i = 0; i < 5; i++)
                    {
                        // 0 - failure, 1 - WO , 2 - Indent, 3 - Invoice , 4 - Decomm

                        if (i >= 4) // checking for decommissioning and more
                        {
                            StrQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "'";
                        }
                        else
                        {
                            StrQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][k] + "'";
                        }
                        string sBOid = ObjCon.get_value(StrQry);

                        if (sBOid == null || sBOid == "") // if sBOid null then checking in pending data
                        {
                            StrQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE  WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + dtAllId.Rows[0][0] + "'";
                            sBOid = ObjCon.get_value(StrQry);

                            if (sBOid == "14" || sBOid == "15" || sBOid == "26")
                            {
                                BOid++;
                                if (ArrBOID[BOid] == "14")// checking data pending with sdo in decommissioning
                                {
                                    StrQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + sDtcCode + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWO = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + sDtcCode + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWF = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + sDtcCode + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    string sWoaId = ObjCon.get_value(StrQry);

                                    for (int l = 0; l < dtWO.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[l]["WO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    for (int l = 0; l < dtWF.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[l]["WO_WFO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    break;
                                }
                                else
                                {
                                    StrQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID = '" + dtAllId.Rows[0][4] + "' AND WO_DATA_ID='" + sDtcCode + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWO = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID = '" + dtAllId.Rows[0][4] + "' AND WO_DATA_ID='" + sDtcCode + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWF = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID = '" + dtAllId.Rows[0][4] + "' AND WO_DATA_ID='" + sDtcCode + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    string sWoaId = ObjCon.get_value(StrQry);

                                    for (int l = 0; l < dtWO.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[l]["WO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    for (int l = 0; l < dtWF.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[l]["WO_WFO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                StrQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                dtWO = ObjCon.getDataTable(StrQry);

                                StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                dtWF = ObjCon.getDataTable(StrQry);

                                //StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID='" + ArrBOID[BOid] + "' AND WO_RECORD_ID LIKE '-%' AND WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                //string sWoaId = ObjCon.get_value(StrQry);

                                for (int l = 0; l < dtWO.Rows.Count; l++)
                                {
                                    StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[l]["WO_ID"] + "'";
                                    ObjCon.Execute(StrQry);
                                }
                                for (int l = 0; l < dtWF.Rows.Count; l++)
                                {
                                    StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[l]["WO_WFO_ID"] + "'";
                                    ObjCon.Execute(StrQry);
                                }
                                break;
                            }
                        }

                        if (sBOid == "12" || sBOid == "29")
                        {
                            string[] arrboid = { "12", "29" };
                            int temp = 2, val = 3;
                            for (int j = 0; j < 2; j++)
                            {
                                for (int h = 0; h < temp; h++)
                                {
                                    StrQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][val] + "' AND WO_BO_ID='" + arrboid[j] + "'";
                                    dtWO = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][val] + "' AND WO_BO_ID='" + arrboid[j] + "'";
                                    dtWF = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][val] + "' AND WO_BO_ID='" + arrboid[j] + "' ";
                                    string sWO_ID = ObjCon.get_value(StrQry);

                                    StrQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + sWO_ID + "'";
                                    string sWoaId = ObjCon.get_value(StrQry);

                                    for (int l = 0; l < dtWO.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[l]["WO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    for (int l = 0; l < dtWF.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[l]["WO_WFO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }

                                    StrQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID='" + sWoaId + "'";
                                    ObjCon.Execute(StrQry);

                                }
                                temp--;
                            }
                        }

                        if (sBOid == "14" || sBOid == "15" || sBOid == "26") // checking already approved records and inserted in to main tables
                        {
                            string[] arrboid = { "14", "15", "26" };
                            for (int j = 0; j < 3; j++)
                            {
                                if (j == 1) //  checking RI record
                                {
                                    StrQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "' AND WO_BO_ID='" + arrboid[j] + "' AND WO_DESCRIPTION LIKE '%" + dtAllId.Rows[0][9] + "%'";
                                    dtWO = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "' AND WO_BO_ID='" + arrboid[j] + "' AND WO_DESCRIPTION LIKE '%" + dtAllId.Rows[0][9] + "%'";
                                    dtWF = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "' AND WO_BO_ID='" + arrboid[j] + "' AND WO_DESCRIPTION LIKE '%" + dtAllId.Rows[0][9] + "%'";
                                    string sWO_ID = ObjCon.get_value(StrQry);

                                    StrQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + sWO_ID + "'";
                                    string sWoaId = ObjCon.get_value(StrQry);

                                    for (int l = 0; l < dtWO.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[l]["WO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    for (int l = 0; l < dtWF.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[l]["WO_WFO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }

                                    StrQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID='" + sWoaId + "'";
                                    ObjCon.Execute(StrQry);
                                }
                                else  // execute both decomm and cr
                                {
                                    StrQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "' AND WO_BO_ID='" + arrboid[j] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWO = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "' AND WO_BO_ID='" + arrboid[j] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWF = ObjCon.getDataTable(StrQry);

                                    StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][k] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][5] + "' AND WO_BO_ID='" + arrboid[j] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    string sWO_ID = ObjCon.get_value(StrQry);

                                    StrQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + sWO_ID + "'";
                                    string sWoaId = ObjCon.get_value(StrQry);

                                    for (int l = 0; l < dtWO.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[l]["WO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    for (int l = 0; l < dtWF.Rows.Count; l++)
                                    {
                                        StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[l]["WO_WFO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }

                                    StrQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID='" + sWoaId + "'";
                                    ObjCon.Execute(StrQry);
                                }
                            }

                            StrQry = "SELECT DT_ID FROM TBLDTCMAST WHERE DT_CODE='" + sDtcCode + "'";
                            string sDTID = ObjCon.get_value(StrQry);

                            StrQry = "UPDATE TBLDTCMAST SET DT_TC_ID='" + dtAllId.Rows[0][9] + "' WHERE DT_ID='" + sDTID + "'";
                            ObjCon.Execute(StrQry);

                            StrQry = "SELECT MAX(TM_ID) FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + sDtcCode + "' AND TM_TC_ID='" + dtAllId.Rows[0][9] + "'";
                            string sTMid = ObjCon.get_value(StrQry);

                            StrQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG='1' WHERE TM_ID='" + sTMid + "'";
                            ObjCon.Execute(StrQry);

                            StrQry = "SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_DF_ID='" + dtAllId.Rows[0]["DF_ID"] + "'";
                            string sDrawDTR = ObjCon.get_value(StrQry);

                            StrQry = "SELECT TM_ID FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + sDtcCode + "' AND TM_TC_ID='" + sDrawDTR + "' AND TM_LIVE_FLAG='1'";
                            string sNewTMid = ObjCon.get_value(StrQry);

                            StrQry = "DELETE FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + sDtcCode + "' AND TM_TC_ID='" + sDrawDTR + "' AND TM_LIVE_FLAG='1' AND TM_ID='" + sNewTMid + "'";
                            ObjCon.Execute(StrQry);

                            StrQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_DTR_CODE='" + sDrawDTR + "' AND DRT_DESC LIKE '%" + sDtcCode + "%' AND DRT_ACT_REFNO='" + sNewTMid + "'";
                            ObjCon.Execute(StrQry);

                            StrQry = "DELETE FROM TBLDTCTRANSACTION WHERE DCT_ACT_REFNO='" + sNewTMid + "' AND DCT_DTC_CODE='" + sDtcCode + "' AND DCT_DESC LIKE '%" + sDrawDTR + "%'";
                            ObjCon.Execute(StrQry);

                        }

                        if (sBOid == "12" || sBOid == "29" || sBOid == "14" || sBOid == "15" || sBOid == "26")
                        {

                        }
                        else // execute failure and WO
                        {
                            StrQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][k] + "'";
                            dtWO = ObjCon.getDataTable(StrQry);


                            if (dtWO.Rows.Count != 0)
                            {

                                StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][k] + "'";
                                dtWF = ObjCon.getDataTable(StrQry);

                                StrQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_RECORD_ID='" + dtAllId.Rows[0][k] + "'";
                                string sWO_ID = ObjCon.get_value(StrQry);

                                StrQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + sWO_ID + "'";
                                string sWoaId = ObjCon.get_value(StrQry);

                                for (int j = 0; j < dtWO.Rows.Count; j++)
                                {
                                    StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[j]["WO_ID"] + "'";
                                    ObjCon.Execute(StrQry);
                                }
                                for (int j = 0; j < dtWF.Rows.Count; j++)
                                {
                                    StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[j]["WO_WFO_ID"] + "'";
                                    ObjCon.Execute(StrQry);
                                }

                                StrQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID='" + sWoaId + "'";
                                ObjCon.Execute(StrQry);
                                //k++;
                            }
                            else
                            {
                                StrQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%' ";
                                dtWO = ObjCon.getDataTable(StrQry);

                                if (dtWO.Rows.Count != 0)
                                {
                                    StrQry = "SELECT DISTINCT WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtAllId.Rows[0][i] + "' AND WO_DESCRIPTION LIKE '%" + sDtcCode + "%'";
                                    dtWF = ObjCon.getDataTable(StrQry);

                                    for (int j = 0; j < dtWO.Rows.Count; j++)
                                    {
                                        StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dtWO.Rows[j]["WO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                    for (int j = 0; j < dtWF.Rows.Count; j++)
                                    {
                                        StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dtWF.Rows[j]["WO_WFO_ID"] + "'";
                                        ObjCon.Execute(StrQry);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        if (sBOid == "13")
                        {
                            k = 0;
                        }
                        else
                        {
                            k++;
                        }

                        BOid++;
                    }

                    StrQry = "SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE='" + dtAllId.Rows[0]["DF_EQUIPMENT_ID"] + "'";
                    sTcId = ObjCon.get_value(StrQry);

                    StrQry = "SELECT DF_EQUIPMENT_ID FROM TBLDTCFAILURE WHERE DF_DTC_CODE='" + sDtcCode + "'";
                    DataTable dt = new DataTable();
                    dt = ObjCon.getDataTable(StrQry);

                    if (dt.Rows.Count > 1)
                    {
                        StrQry = "UPDATE TBLTCMASTER SET TC_STATUS='2',TC_UPDATED_EVENT='',TC_UPDATED_EVENT_ID='0' WHERE TC_ID='" + sTcId + "'";
                        ObjCon.Execute(StrQry);
                    }
                    else
                    {
                        StrQry = "UPDATE TBLTCMASTER SET TC_STATUS='1',TC_UPDATED_EVENT='',TC_UPDATED_EVENT_ID='0' WHERE TC_ID='" + sTcId + "'";
                        ObjCon.Execute(StrQry);
                    }

                    StrQry = "DELETE FROM TBLTCREPLACE WHERE TR_ID='" + dtAllId.Rows[0]["TR_ID"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLDTCINVOICE WHERE IN_NO='" + dtAllId.Rows[0]["IN_NO"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLINDENT WHERE TI_ID='" + dtAllId.Rows[0]["TI_ID"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLWORKORDER WHERE WO_SLNO='" + dtAllId.Rows[0]["WO_SLNO"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLESTIMATION WHERE EST_DF_ID='" + dtAllId.Rows[0]["DF_ID"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLDTCFAILURE WHERE DF_ID='" + dtAllId.Rows[0]["DF_ID"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_DF_ID='" + dtAllId.Rows[0]["DF_ID"] + "'";
                    string sDrawTc = ObjCon.get_value(StrQry);

                    if (sDrawTc != null)
                    {
                        StrQry = "SELECT SUBSTR(TC_LOCATION_ID,0,2) FROM TBLTCMASTER WHERE TC_CODE='"+ sDrawTc +"'";
                        string sTCOffcode = ObjCon.get_value(StrQry);

                        StrQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + sTCOffcode + "',TC_CURRENT_LOCATION='1',TC_UPDATED_EVENT='',TC_UPDATED_EVENT_ID='0' WHERE TC_CODE='" + sDrawTc + "'";
                        ObjCon.Execute(StrQry);
                    }

                    StrQry = "DELETE FROM TBLTCDRAWN WHERE TD_DF_ID='" + dtAllId.Rows[0]["DF_ID"] + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID='" + sDrtId + "'";
                    ObjCon.Execute(StrQry);

                    StrQry = "DELETE FROM TBLDTCTRANSACTION WHERE DCT_ID='" + sDtcId + "'";
                    ObjCon.Execute(StrQry);
                }
                else
                {
                    StrQry = "SELECT WO_ID,WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='"+ sDtcCode +"' AND WO_RECORD_ID LIKE '-%' AND WO_BO_ID='9'";
                    DataTable dt = new DataTable();
                    dt = ObjCon.getDataTable(StrQry);

                    StrQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + dt.Rows[0]["WO_ID"] + "'";
                    ObjCon.Execute(StrQry);
                
                    StrQry = "DELETE FROM TBLWFODATA WHERE WFO_ID='" + dt.Rows[0]["WO_WFO_ID"] + "'";
                    ObjCon.Execute(StrQry);

                }

                arr[0] = "Record Deleted Succesfully";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DeleteData");
                arr[0] = "Exception Occured";
            }
            return arr;
        }

        /// <summary>
        /// change the section of the  DTC and DTR 
        /// </summary>
        /// <param name="objBackend"></param>
        /// <returns></returns>
        public string[] ChangeDTCSection(clsBackEndActivity objBackend)
        {
            string[] arr = new string[2];
            arr[0] = "1"; 
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //if DTC and DTR exists
                strQry = "SELECT * FROM TBLDTCMAST WHERE DT_CODE = '"+objBackend.sDtcCode+"'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count == 0)
                {
                    arr[1] = "DTC doesnt exists";
                    return arr;
                }
                
                strQry = "SELECT * FROM TBLTCMASTER WHERE TC_CODE = '"+objBackend.sDTrCode+"'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count == 0)
                {
                    arr[1] = "DTR doesnt exists";
                    return arr;
                }
                //check whether DTR has been failed 
                strQry = "SELECT * FROM TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = '" + objBackend.sDTrCode + "' AND DF_REPLACE_FLAG ='0'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    arr[1] = "DTR has been failed Please contact backend team";
                    return arr;
                }
                // check whether dtc has been partially feeder bifurcated
                strQry = " SELECT FBS_STATUS FROM TBLFEEDERBIFURCATION_SO , TBLFEEDER_BFCN_DETAILS_SO WHERE FBS_ID = FBDS_FB_ID and " +
                    " FBDS_OLD_DTC_CODE = '" + objBackend.sDtcCode + "' and   FBS_STATUS in (0,1)  ";
                  
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    arr[1] = "DTC has been partially Feeder Bifrucated";
                    return arr;
                }

                // if DTC has been  declared Failure 
                strQry = "SELECT * FROM TBLDTCFAILURE  WHERE DF_DTC_CODE='" + objBackend.sDtcCode + "' AND DF_REPLACE_FLAG = 0";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    arr[1] = "Cannot update Section as DTC has been declared failure";
                    return arr;
                }
                //check if DTC has just declared failure
                strQry = "SELECT WO_ID,WO_WFO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + sDtcCode + "' AND WO_RECORD_ID LIKE '-%' AND WO_BO_ID='9'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    arr[1] = "DTC was Just Declared Failure Please contact Backend ";
                    return arr;
                }
                //if DTC is in the same section 
                strQry = "SELECT * FROM TBLDTCMAST WHERE DT_CODE = '"+objBackend.sDtcCode+"' AND DT_OM_SLNO = '"+objBackend.sSectionCode+"'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    arr[1] = "DTC has already mapped to the specified  section";
                    return arr;
                }
                //whether the DTC and Entered DTr is mapped  
                strQry = "SELECT * FROM TBLDTCMAST WHERE DT_CODE = '" + objBackend.sDtcCode + "' AND DT_TC_ID = '" + objBackend.sDTrCode + "' ";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count == 0)
                {
                    arr[1] = "DTC and DTR are not mapped in Masters";
                    return arr;
                }
                //whether the mapped DTC and DTr are present in the Transdtcmapping table
                strQry = "SELECT * FROM TBLTRANSDTCMAPPING WHERE TM_TC_ID ='"+objBackend.sDTrCode+"' AND  TM_DTC_ID = '"+objBackend.sDtcCode+"' AND TM_LIVE_FLAG = '1' ";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count == 0)
                {
                    arr[1] = "DTC and DTR are not mapped in Mapping table";
                    return arr;
                }

                // if DTC was declared Failure before changing section 
                strQry = "SELECT * FROM TBLDTCFAILURE  WHERE DF_DTC_CODE='" + objBackend.sDtcCode + "' AND DF_REPLACE_FLAG = 1";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    //logic to change in the TBLdtcfailure table 
                    strQry = "SELECT DF_ID ,WO_SLNO ,TI_ID ,IN_NO ,TR_ID,DT_OM_SLNO AS DF_LOC_CODE  FROM TBLDTCFAILURE LEFT JOIN TBLWORKORDER ";
                    strQry += "ON WO_DF_ID = DF_ID LEFT JOIN TBLINDENT ON TI_WO_SLNO = WO_SLNO  LEFT JOIN TBLDTCINVOICE ON TI_ID = IN_TI_NO ";
                    strQry += " LEFT JOIN TBLTCREPLACE  ON TR_IN_NO = IN_NO left JOIN TBLDTCMAST on DT_CODE = DF_DTC_CODE   WHERE DF_DTC_CODE = '" + objBackend.sDtcCode + "'";
                    dt = ObjCon.getDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        string oldDivCode =  Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]).Substring(0,2) ;
                        string oldSubDivCode = Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]).Substring(0,3) ; 
                        string oldSecCode =  Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]) ;
                        objBackend.oldSectionCode =  Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]);
                        string newDivCode = objBackend.sSectionCode.Substring(0,2) ;
                        string newSubDivCode = objBackend.sSectionCode.Substring(0,3) ;
                        string newSectionCode =  objBackend.sSectionCode ;

                        string [] transactionIDs= new string[8];
                        ObjCon.BeginTrans();
                        // loop for multiple failure transactions
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            transactionIDs[0] = Convert.ToString(dt.Rows[i]["DF_ID"]) + ",9";
                            transactionIDs[1] = Convert.ToString(dt.Rows[i]["WO_SLNO"]) + ",11";
                            transactionIDs[2] = Convert.ToString(dt.Rows[i]["TI_ID"]) + ",12";
                            transactionIDs[3] = Convert.ToString(dt.Rows[i]["TI_ID"]) + ",29";
                            transactionIDs[4] = Convert.ToString(dt.Rows[i]["IN_NO"]) + ",13";
                            transactionIDs[5] = Convert.ToString(dt.Rows[i]["TR_ID"]) + ",14";
                            transactionIDs[6] = Convert.ToString(dt.Rows[i]["TR_ID"]) + ",15";
                            transactionIDs[7] = Convert.ToString(dt.Rows[i]["TR_ID"]) + ",26";

                            strQry = "UPDATE TBLDTCFAILURE SET DF_LOC_CODE = '" + newSectionCode + "'  WHERE DF_ID = '" + Convert.ToString(transactionIDs[0]).Split(',')[0] + "'";
                            ObjCon.Execute(strQry);

                            for (int j = 0; j < 8; j++)
                            {
                                // for  section code 
                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_OFFICE_CODE = " + newSectionCode + "   WHERE WO_RECORD_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[0] + " AND WO_BO_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[1] + "  AND WO_OFFICE_CODE = " + oldSecCode + " ";
                                ObjCon.Execute(strQry);
                                // for subdiv code 
                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_OFFICE_CODE = " + newSubDivCode + "   WHERE WO_RECORD_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[0] + " AND WO_BO_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[1] + "  AND WO_OFFICE_CODE = " + oldSubDivCode + " ";
                                ObjCon.Execute(strQry);
                                // for section in WO_REF_OFFCODE
                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_REF_OFFCODE = " + newSectionCode + "   WHERE WO_RECORD_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[0] + " AND WO_BO_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[1] + "  AND WO_REF_OFFCODE = " + oldSecCode + " ";
                                ObjCon.Execute(strQry);
                                // for wo_auto_object WOA_OFFICE_CODE 
                                strQry = "UPDATE  TBLWO_OBJECT_AUTO set WOA_OFFICE_CODE = " + newSubDivCode + "    WHERE WOA_PREV_APPROVE_ID  IN ";
                                strQry += "(SELECT WO_ID FROM TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE WO_RECORD_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[0] + " and WO_BO_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[1] + " ";
                                strQry += " AND WO_NEXT_ROLE = 0 and WO_ID = WOA_PREV_APPROVE_ID AND WOA_OFFICE_CODE = " + oldSubDivCode + " )";
                                ObjCon.Execute(strQry);
                                // for wo_object_auto WOA_REF_OFFCODE
                                strQry = "UPDATE  TBLWO_OBJECT_AUTO set WOA_REF_OFFCODE = " + newSectionCode + "  WHERE WOA_PREV_APPROVE_ID  IN ";
                                strQry += " (SELECT WO_ID FROM TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE WO_RECORD_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[0] + " and WO_BO_ID = " + Convert.ToString(transactionIDs[j]).Split(',')[1] + "";
                                strQry += " AND WO_NEXT_ROLE = 0 and WO_ID = WOA_PREV_APPROVE_ID AND WOA_REF_OFFCODE  = " + oldSecCode + " )";
                                ObjCon.Execute(strQry);
                            }
                        }

                        ObjCon.CommitTrans();
                    }

                }


                //start updating in the main table 
                ObjCon.BeginTrans();
                //select the id from TBLENUMERATION
                strQry = "SELECT ED_ID FROM TBLENUMERATIONDETAILS,TBLDTCENUMERATION WHERE ED_ID=DTE_ED_ID AND DTE_DTCCODE='" + objBackend.sDtcCode + "' AND ED_STATUS_FLAG = '1' ";
                string sEnumid =  ObjCon.get_value(strQry);

                if (sEnumid.Length != 0)
                {
                    strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_OFFICECODE='" + objBackend.sSectionCode + "' WHERE ED_ID='" + sEnumid + "'";
                    ObjCon.Execute(strQry);
                }
                

                //select id from TBLQCAPPROVED
                strQry = "SELECT QA_ID FROM TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS WHERE QA_ID=QAO_QA_ID AND QAO_DTCCODE='" + objBackend.sDtcCode+ "'";
                string sQaID = ObjCon.get_value(strQry);

                if (sQaID.Length != 0)
                {
                    strQry = "UPDATE TBLQCAPPROVED SET QA_OFFICECODE='" + objBackend.sSectionCode + "' WHERE QA_ID='" + sQaID + "'";
                    ObjCon.Execute(strQry);
                }
                
               //udpate in  TBLDTCMAST
                strQry = "UPDATE TBLDTCMAST SET DT_OM_SLNO='" + objBackend.sSectionCode + "' WHERE DT_CODE='"+objBackend.sDtcCode+"'";
                ObjCon.Execute(strQry);
                
                //update in TBLtcmaster
                strQry = "UPDATE TBLTCMASTER SET TC_LOCATION_ID='" + objBackend.sSectionCode + "' WHERE TC_CODE='"+objBackend.sDTrCode+"'";
                ObjCon.Execute(strQry);

                DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                objWCF.SaveTcDetails(strQry);

                //update in DTRTRANSACTION
                strQry = "SELECT DRT_ID FROM TBLDTRTRANSACTION where DRT_DTR_CODE IN (SELECT DT_TC_ID FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CODE=DT_TC_ID AND DT_CODE ='" + objBackend.sDtcCode + "') AND DRT_LOC_TYPE = 2";
                string sdrtid = ObjCon.get_value(strQry);

                if (sdrtid.Length != 0)
                {
                    strQry = "UPDATE TBLDTRTRANSACTION SET DRT_LOC_ID='" + objBackend.sSectionCode + "' WHERE DRT_ID='" + sdrtid + "'";
                    ObjCon.Execute(strQry);
                }
                else
                {
                    arr[0] = "0";
                    arr[1] = "Oops Something Went Wrong with DTRTRansaction";
                    return arr;
                }
                ObjCon.CommitTrans();

                // finally keep a track 
                string temp = "Changed " + objBackend.sDtcCode + " DTCs Section from" + objBackend.oldSectionCode + " to " + objBackend.sSectionCode + " ";

                strQry = "INSERT into TBLBACKENDACTIVITYDETAILS (BAD_ID,BAD_BM_ID , BAD_OLDDATA , BAD_NEWDATA,BAD_TICKETNUMBER, ";
                strQry += " BAD_DESCRITION,BAD_DTCCODE,BAD_DTRCODE,BAD_ENTRY_BY) VALUES ((SELECT MAX(BAD_ID)+1 FROM TBLBACKENDACTIVITYDETAILS),2,'" + objBackend.oldSectionCode + "','" + objBackend.sSectionCode + "','" + objBackend.ticketNumber + "', ";
                strQry += " '" + temp + "','" + objBackend.sDtcCode + "'," + objBackend.sDTrCode + ","+objBackend.createdBy+")";
                ObjCon.Execute(strQry);
                arr[1] = "Updated Successfully";
                return arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ChangeDTCSection");
                arr[0] = "0";
                arr[1] = "Oops Something Went Wrong";
                return arr;
            }
           
        }
    }
}
