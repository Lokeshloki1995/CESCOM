using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;


namespace IIITS.DTLMS.BL.MasterForms
{
     public class clsIncharge
    {
        string strFormCode = "clsIncharge";

        public string sEmployeeName { get; set; }
        public string sActualDesignation { get; set; }
        public string sInchargeDesignation { get; set; }
        public string sHandoverEmp { get; set; }
        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public string sRemarks { get; set; }
        public string sAutoOmNumber { get; set; }
        public string sOmNumber { get; set; }
        public string sCrby { get; set; }
        public string sOfficeCode { get; set; }
        public string sMaxid { get; set; }
        public string sMaxOmid { get; set; }
        public string sOmdate { get; set; }
        public string sTotalDays { get; set; }
        public string sofficeName { get; set; }
        public string sUserId { get; set; }
        public string sFullName { get; set; }
        public string  sOfficeName { get; set; }
        public string sRoleId { get; set; }
        public string sOfficeNamewithType { get; set; }
        public string sDesignation { get; set; }
        public string sGeneralLog { get; set; }
        public string sPasswordChangeInDays { get; set; }
        public string sPasswordChangeRequest { get; set; }
        public string sTransactionLog { get; set; }
        public string sPassordAcceptance { get; set; }
        public string sInchargeRoleID { get; set; }
        public string sActualRoleID { get; set; }


        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;

        public string GenerateAutoOmNo()
        {
            try
            {

                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;

                oledbCommand = new OleDbCommand();
                string sOmNo = ObjCon.get_value("SELECT NVL(MAX(IOMD_AUTOGEN_OM_NUMBER),0)+1 FROM TBL_INCHARGE_OM_DETAILS", oledbCommand);

                if (sOmNo.Length == 1)
                {

                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }

                    sOmNo = sFinancialYear + "00001";
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sFinancialYear == sOmNo.Substring(0, 4))
                        {
                            return sOmNo;
                        }
                        else
                        {
                            sOmNo =  sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sOmNo;
                    }



                }

                return sOmNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
                return "";
            }
        }

        public string[] SaveInchargeDetails(clsIncharge objIncharge)
        {
            oledbCommand = new OleDbCommand();
            string[] Arr = new string[2];
            try
            {
                
                string strQry = string.Empty;

                if (objIncharge.sHandoverEmp!="")
                {                                     
                    strQry = "SELECT IOML_ID  FROM TBL_INCHARGE_OM_LINKUP WHERE IOML_CHO_EMP_ID = '" + objIncharge.sHandoverEmp + "' AND TO_CHAR(CURRENT_DATE,'dd-MON-yyyy') BETWEEN TO_CHAR(IOML_CHARGE_FROM_DATE,'dd-MON-yyyy') AND TO_CHAR(IOML_CHARGE_TO_DATE,'dd-MON-yyyy')  ORDER BY IOML_ID";
                    DataTable sExist = ObjCon.getDataTable(strQry, oledbCommand);
                    if (sExist.Rows.Count > 0)
                    {
                        Arr[0] = "Incharge Already Exist";
                        Arr[1] = "1";
                        return Arr;
                    }
                }

                objIncharge.sMaxOmid = ObjCon.Get_max_no("IOMD_ID", "TBL_INCHARGE_OM_DETAILS").ToString();

                strQry = "INSERT INTO TBL_INCHARGE_OM_DETAILS(IOMD_ID, IOMD_AUTOGEN_OM_NUMBER ,IOMD_MAN_OM_NUMBER,IOMD_OM_DATE,IOMD_OFF_CODE,IOMD_CRBY,IOMD_CRON,IOMD_UPBY,IOMD_UPON)";
                strQry += "VALUES(:sMaxOmid,:sAutoOmNumber,:sOmNumber,TO_DATE('" + objIncharge.sOmdate + "','DD/MM/YYYY'),";
                strQry += ":sOfficeCode,:sCrby,SYSDATE,'','')";
                OleDbCommand command = new OleDbCommand();
                command.Parameters.AddWithValue("sMaxOmid", objIncharge.sMaxOmid);
                command.Parameters.AddWithValue("sAutoOmNumber", objIncharge.sAutoOmNumber);
                command.Parameters.AddWithValue("sOmNumber", objIncharge.sOmNumber);
                command.Parameters.AddWithValue("sOfficeCode", objIncharge.sOfficeCode);
                command.Parameters.AddWithValue("sCrby", objIncharge.sCrby);
                ObjCon.Execute(strQry, command);

                objIncharge.sMaxid = ObjCon.Get_max_no("IOML_ID", "TBL_INCHARGE_OM_LINKUP").ToString();

                strQry = "INSERT INTO TBL_INCHARGE_OM_LINKUP(IOML_ID, IOML_IOMD_ID ,IOML_EMP_ID,IOML_ACT_RL_ID,IOML_INC_RL_ID,IOML_CHO_EMP_ID,IOML_INC_LOC_CODE,IOML_CHARGE_FROM_DATE,IOML_CHARGE_TO_DATE,IOML_OMD_NO_OF_DAYS,IOML_REMARKS,IOML_WO_STATUS)";
                strQry += "VALUES('"+ objIncharge.sMaxid + "','"+ objIncharge.sMaxOmid + "','"+ objIncharge.sEmployeeName + "','"+ objIncharge.sActualRoleID + "','"+ objIncharge.sInchargeRoleID + "','"+ objIncharge.sHandoverEmp + "',";
                strQry += "'"+ objIncharge.sOfficeCode + "',TO_DATE('" + objIncharge.sFromDate + "','DD/MM/YYYY'),TO_DATE('" + objIncharge.sToDate + "','DD/MM/YYYY'),'"+ objIncharge.sTotalDays + "','"+ objIncharge.sRemarks + "',1) ";
                ObjCon.Execute(strQry);
                Arr[0] = "Saved Successfully ";
                Arr[1] = "0";
                return Arr;
                
         
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveInchargeDetails");
                return Arr;
            }
        }
        public DataTable LoadUserGrid(clsIncharge objInchargeUser)
        {
            oledbCommand = new OleDbCommand();
            DataTable dtInchargeUserDetails = new DataTable();

            string strQry = string.Empty;
            try
            {

                strQry = "select IOMD_AUTOGEN_OM_NUMBER, IOMD_MAN_OM_NUMBER, to_char(IOMD_OM_DATE, 'DD-MON-YYYY') as IOMD_OM_DATE,OFF_NAME,to_char(IOML_CHARGE_FROM_DATE, 'dd-MON-yyyy') as IOML_CHARGE_FROM_DATE,";
                strQry +=  " to_char(IOML_CHARGE_TO_DATE, 'dd-MON-yyyy') as IOML_CHARGE_TO_DATE,A.US_LG_NAME || '~' || A.US_FULL_NAME as Actual_User , B.US_LG_NAME || '~' || B.US_FULL_NAME as Handover_User ";
                strQry += " from TBL_INCHARGE_OM_DETAILS inner join TBL_INCHARGE_OM_LINKUP on IOML_IOMD_ID = IOMD_ID join TBLUSER A on IOML_EMP_ID = A.US_ID INNER JOIN TBLUSER B on IOML_CHO_EMP_ID = B.US_ID";
                strQry += " inner join VIEW_ALL_OFFICES on IOML_INC_LOC_CODE = OFF_CODE where A.US_OFFICE_CODE like '" + objInchargeUser.sOfficeCode + "%'";


                dtInchargeUserDetails = ObjCon.getDataTable(strQry, oledbCommand);

                return dtInchargeUserDetails;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUserGrid");
                return dtInchargeUserDetails;

            }
            finally
            {

            }
        }

        public DataTable LoadOfficeDet(clsIncharge objSwapUser)
        {
            oledbCommand = new OleDbCommand();
            DataTable DtStationDet = new DataTable();
            try
            {

                string strQry = string.Empty;

                strQry = "SELECT 'DEFAULT' as INCHARGE_TYPE,OFF_NAME||'~'||OFF_CODE AS LOCATION,OFF_CODE, DM_NAME,  0 as IOML_ID , RO_NAME ,RO_ID, US_LG_NAME || '~' || US_FULL_NAME AS USER_NAME, 'N/A' as FROM_DATE, 'N/A' as TO_DATE FROM TBLUSER  INNER JOIN TBLROLES ON  US_ROLE_ID =RO_ID  JOIN VIEW_ALL_OFFICES ON US_OFFICE_CODE = OFF_CODE  JOIN TBLDESIGNMAST ON US_DESG_ID = DM_DESGN_ID  AND US_ID = '" + objSwapUser.sUserId+ "'";                       
                strQry += " UNION SELECT 'INCHARGE' as INCHARGE_TYPE, OFF_NAME || '~' || OFF_CODE AS LOCATION,OFF_CODE, DM_NAME, IOML_ID ,RO_NAME ,RO_ID,  US_LG_NAME || '~' || US_FULL_NAME AS USER_NAME, to_char(IOML_CHARGE_FROM_DATE, 'dd-MON-yyyy') as FROM_DATE , to_char(IOML_CHARGE_TO_DATE, 'dd-MON-yyyy') as TO_DATE FROM TBL_INCHARGE_OM_LINKUP JOIN VIEW_ALL_OFFICES ON IOML_INC_LOC_CODE = OFF_CODE";  
                strQry += " JOIN TBLDESIGNMAST ON IOML_ACT_RL_ID = DM_DESGN_ID JOIN TBLUSER ON IOML_EMP_ID = US_ID INNER JOIN TBLROLES ON RO_ID = US_ROLE_ID   AND IOML_CHO_EMP_ID = '"+ objSwapUser.sUserId+ "' AND TO_CHAR(CURRENT_DATE,'dd-MON-yyyy') BETWEEN TO_CHAR(IOML_CHARGE_FROM_DATE,'dd-MON-yyyy') AND TO_CHAR(IOML_CHARGE_TO_DATE,'dd-MON-yyyy') ORDER BY IOML_ID ";
                DtStationDet = ObjCon.getDataTable(strQry, oledbCommand);

                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadOfficeDet");
                return DtStationDet;
            }
            finally
            {

            }

        }
        //        public static void Load_Combo(string Qry, string strSelect, System.Web.UI.WebControls.DropDownList cmb)
        //{
        //    OleDbDataReader reader;
        //    if (objCon.Con.State == ConnectionState.Closed)
        //        objCon.Con.Open();
        //    reader = objCon.Fetch(Qry);
        //    cmb.Items.Clear();
        //    if (strSelect.Length > 0)
        //    {
        //        cmb.Items.Add(strSelect);

        //    }
        //    while (reader.Read() == true)
        //    {
        //        System.Web.UI.WebControls.ListItem itm = new System.Web.UI.WebControls.ListItem();
        //        itm.Value = Get_Reader_res(reader, 0);
        //        itm.Text = Get_Reader_res(reader, 1);
        //        cmb.Items.Add(itm);
        //    }
        //    reader.Close();
        //}
    }
}


