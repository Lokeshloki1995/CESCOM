using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsReceiveTrans
    {
        string strQry = string.Empty;
        string strFormCode = "clsReceiveTrans";
        public string sInvoiceId { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sFromStoreId { get; set; }
        public string sCreatedBy { get; set; }
        public string sQuantity { get; set; }
        public string sRemarks { get; set; }
        public string sOfficeCode { get; set; }

        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sIndentId { get; set; }

        public string sRVNo { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public DataTable LoadReceiveTcGrid(string sOfficeCode)
        {
            DataTable dtTcReceive = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {

                strQry = "SELECT IS_ID,IS_NO,TO_CHAR(IS_DATE,'DD-MON-YYYY')IS_DATE,(SELECT SM_NAME FROM TBLSTOREMAST WHERE  SM_ID=SI_TO_STORE) AS SI_FROM_STORE,COUNT(IO_TCCODE)IO_TCCODE,IO_IS_ID,SI_NO";
                strQry += " FROM TBLSTOREINVOICE,TBLSTOREINDENT,TBLSINVOICEOBJECTS,TBLSTOREMAST WHERE IS_SI_ID=SI_ID AND IS_APPROVE_FLAG = 0 AND ";
                strQry += " IO_IS_ID=IS_ID and SM_ID=SI_FROM_STORE AND SM_OFF_CODE LIKE :OfficeCode ||'%' ";
                strQry += " GROUP BY IO_IS_ID,IS_ID,IS_NO,IS_DATE,SM_NAME,SI_NO,SI_TO_STORE ORDER BY IS_NO DESC";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //AND SM_OFF_CODE='" + sOfficeCode.Substring(0, 2) + "' ";
                dtTcReceive = ObjCon.getDataTable(strQry, oleDbCommand);

                return dtTcReceive;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadReceiveTcGrid");
                return dtTcReceive;
            }
            finally
            {
                
            }

        }

        public DataTable LoadComplededReceiveTcGrid(string sOfficeCode)
        {
            DataTable dtTcReceive = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = "SELECT IS_ID,IS_NO,TO_CHAR(IS_DATE,'DD-MON-YYYY')IS_DATE,(SELECT SM_NAME FROM TBLSTOREMAST WHERE  SM_ID=SI_TO_STORE) AS SI_FROM_STORE,COUNT(IO_TCCODE)IO_TCCODE,IO_IS_ID,SI_NO";
                strQry += " FROM TBLSTOREINVOICE,TBLSTOREINDENT,TBLSINVOICEOBJECTS,TBLSTOREMAST WHERE IS_SI_ID=SI_ID AND IS_APPROVE_FLAG = 1 AND ";
                strQry += " IO_IS_ID=IS_ID and SM_ID=SI_FROM_STORE AND SM_OFF_CODE LIKE :OfficeCode || '%' ";
                strQry += " GROUP BY IO_IS_ID,IS_ID,IS_NO,IS_DATE,SM_NAME,SI_NO,SI_TO_STORE ORDER BY IS_NO DESC";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //AND SM_OFF_CODE='" + sOfficeCode.Substring(0, 2) + "' ";
                dtTcReceive = ObjCon.getDataTable(strQry, oleDbCommand);

                return dtTcReceive;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTrceiveTcGrid");
                return dtTcReceive;
            }
            finally
            {
                
            }

        }


        public object LoadInvoiceDetails(clsReceiveTrans objInvoice)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = "select IS_ID,IS_NO,IS_SI_ID,TO_CHAR(IS_DATE,'DD-MON-YYYY')IS_DATE,SI_TO_STORE,SI_NO,TO_CHAR(SI_DATE,'DD-MON-YYYY')SI_DATE,SI_ID, ";
                strQry += " (select COUNT(IO_TCCODE)AS SI_TO_STORE  from TBLSINVOICEOBJECTS where IO_IS_ID=IS_ID ) QUANTITY ";
                strQry += " from TBLSTOREINVOICE,TBLSTOREINDENT  where IS_SI_ID=SI_ID ";
                if (objInvoice.sInvoiceId != "")
                {
                    strQry += " AND IS_ID=:ISID";
                    oleDbCommand.Parameters.AddWithValue("ISID", objInvoice.sInvoiceId);
                }

                strQry += " GROUP BY IS_ID,IS_NO,IS_SI_ID,IS_DATE,SI_FROM_STORE,SI_TO_STORE,SI_NO,SI_DATE,SI_ID";
                dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objInvoice.sInvoiceId = Convert.ToString(dtIndentDetails.Rows[0]["IS_ID"]);
                    objInvoice.sInvoiceNo = Convert.ToString(dtIndentDetails.Rows[0]["IS_NO"]);
                    objInvoice.sInvoiceDate = Convert.ToString(dtIndentDetails.Rows[0]["IS_DATE"]);
                    objInvoice.sQuantity = Convert.ToString(dtIndentDetails.Rows[0]["QUANTITY"]);
                    objInvoice.sFromStoreId = Convert.ToString(dtIndentDetails.Rows[0]["SI_TO_STORE"]);

                    objInvoice.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
                    objInvoice.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
                    objInvoice.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadInvoiceDetails");
                return objInvoice;
            }
            finally
            {
                
            }
        }



        public DataTable LoadCapacityGrid(clsReceiveTrans objInvoice)
        {
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = " SELECT TC_CODE,TC_SLNO,TO_CHAR(TC_CAPACITY) TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                strQry += " TM_ID=TC_MAKE_ID) TM_NAME FROM TBLTCMASTER,TBLSINVOICEOBJECTS ";
                strQry += "WHERE IO_IS_ID=:ISID AND IO_TCCODE=TC_CODE ";
                oleDbCommand.Parameters.AddWithValue("ISID", objInvoice.sInvoiceId);
                dtCapacityDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadCapacityGrid");
                return dtCapacityDetails;
            }
            finally
            {
                
            }
        }

        public string[] RecieveTransformer(clsReceiveTrans objInvoice)
        {
            string strQry = string.Empty;
            string[] Arr = new string[2];
            DataTable dt = new DataTable();
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                strQry = "SELECT DISTINCT (SELECT DISTINCT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=TC_LOCATION_ID) FROM TBLTCMASTER WHERE TC_CODE IN (SELECT IO_TCCODE FROM ";
                strQry += " TBLSINVOICEOBJECTS WHERE IO_IS_ID=:ISID)";
                oleDbCommand.Parameters.AddWithValue("ISID", objInvoice.sInvoiceId);
                string sOld_Store_name = ObjCon.get_value(strQry, oleDbCommand);

                OleDbCommand oleDbCommand1 = new OleDbCommand();
                strQry = "SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=:DivisionCode";
                oleDbCommand1.Parameters.AddWithValue("DivisionCode", objInvoice.sOfficeCode);
                string sNew_Store_name = ObjCon.get_value(strQry, oleDbCommand1);

                OleDbCommand oleDbCommand2 = new OleDbCommand();
                strQry = "SELECT IO_TCCODE FROM TBLSINVOICEOBJECTS WHERE IO_IS_ID=:ISID";
                oleDbCommand2.Parameters.AddWithValue("ISID", objInvoice.sInvoiceId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand2);

                ObjCon.BeginTrans();

                if (objInvoice.sOfficeCode.Length > 1)
                {
                    objInvoice.sOfficeCode = objInvoice.sOfficeCode.Substring(0, 2);
                }

                clsTcMaster objTcMaster = new clsTcMaster();
                string sStoreId = objTcMaster.GetStoreId(objInvoice.sOfficeCode);

                strQry = "UPDATE TBLSTOREINVOICE set IS_APPROVE_FLAG=1,IS_APPROVE_DATE=sysdate,IS_RV_NO='" + objInvoice.sRVNo + "',";
                strQry += " IS_APPROVE_BY='" + objInvoice.sCreatedBy + "', IS_APPROVE_REMARKS='" + objInvoice.sRemarks + "' where IS_ID='" + objInvoice.sInvoiceId + "' ";
                ObjCon.Execute(strQry);

                strQry = "Update TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_LOCATION_ID='" + objInvoice.sOfficeCode + "',TC_STORE_ID='" + sStoreId + "' WHERE TC_CODE IN ";
                strQry += " (SELECT IO_TCCODE FROM TBLSINVOICEOBJECTS WHERE IO_IS_ID='" + objInvoice.sInvoiceId + "')";
                ObjCon.Execute(strQry);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strQry = "INSERT INTO TBLDTRTRANSACTION VALUES ((SELECT NVL(max(DRT_ID), 0) + 1 FROM TBLDTRTRANSACTION),'" + dt.Rows[i]["IO_TCCODE"].ToString() + "',";
                    strQry += "'" + sStoreId + "','1',SYSDATE,'','1','IUT MOVED FROM STORE : " + sOld_Store_name + "  TO STORE : " + sNew_Store_name + "','1',";
                    strQry += "SYSDATE,'0','','')";
                    ObjCon.Execute(strQry);
                }

                ObjCon.CommitTrans();

                //Workflow / Approval
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objInvoice.sFormName;
                objApproval.sRecordId = objInvoice.sInvoiceId;
                objApproval.sOfficeCode = objInvoice.sOfficeCode;
                objApproval.sClientIp = objInvoice.sClientIP;
                objApproval.sCrby = objInvoice.sCreatedBy;
                objApproval.sWFObjectId = objInvoice.sWFOId;

                string sResult = ObjCon.get_value("SELECT SI_NO ||'~' || IS_NO FROM TBLSTOREINDENT,TBLSTOREINVOICE WHERE IS_ID='" + objInvoice.sInvoiceId + "' AND SI_ID=IS_SI_ID");

                objApproval.sDescription = "Response for Store Indent No " + sResult.Split('~').GetValue(0).ToString() + " with Store Invoice Number " + sResult.Split('~').GetValue(1).ToString();

                objApproval.SaveWorkflowObjects(objApproval);

                Arr[0] = "Recieved Successfully";
                Arr[1] = "0";
                return Arr;
            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "RecieveTransformer");
                return Arr;
            }
            finally
            {
                
            }
        }

        public clsReceiveTrans LoadReceivedInvoiceDetails(clsReceiveTrans objInvoice)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = "select IS_ID,IS_NO,IS_SI_ID,TO_CHAR(IS_DATE,'DD-MON-YYYY')IS_DATE,SI_TO_STORE,SI_NO,TO_CHAR(SI_DATE,'DD-MON-YYYY')SI_DATE,SI_ID,";
                strQry += " (select COUNT(IO_TCCODE)AS SI_TO_STORE  from TBLSINVOICEOBJECTS where IO_IS_ID=IS_ID ) QUANTITY,IS_APPROVE_REMARKS,IS_RV_NO ";
                strQry += " from TBLSTOREINVOICE,TBLSTOREINDENT  WHERE IS_SI_ID=SI_ID ";
                if (objInvoice.sInvoiceId != "")
                {
                    strQry += " AND IS_ID=:ISID";
                    oleDbCommand.Parameters.AddWithValue("ISID", objInvoice.sInvoiceId);
                }
               
                dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objInvoice.sInvoiceId = Convert.ToString(dtIndentDetails.Rows[0]["IS_ID"]);
                    objInvoice.sInvoiceNo = Convert.ToString(dtIndentDetails.Rows[0]["IS_NO"]);
                    objInvoice.sInvoiceDate = Convert.ToString(dtIndentDetails.Rows[0]["IS_DATE"]);
                    objInvoice.sQuantity = Convert.ToString(dtIndentDetails.Rows[0]["QUANTITY"]);
                    objInvoice.sFromStoreId = Convert.ToString(dtIndentDetails.Rows[0]["SI_TO_STORE"]);
                    objInvoice.sRemarks = Convert.ToString(dtIndentDetails.Rows[0]["IS_APPROVE_REMARKS"]);
                    objInvoice.sRVNo = Convert.ToString(dtIndentDetails.Rows[0]["IS_RV_NO"]);

                    objInvoice.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
                    objInvoice.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
                    objInvoice.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]); 
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadReceivedInvoiceDetails");
                return objInvoice;
            }
            finally
            {
                
            }
        }


        public DataTable LoadIndentDetGrid(clsReceiveTrans objInvoice)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = " SELECT SO_QNTY,TO_CHAR(SO_CAPACITY)SO_CAPACITY ";
                strQry += "  FROM TBLSTOREINDENT,TBLSINDENTOBJECTS ";
                strQry += "WHERE SO_SI_ID=:SIID AND SO_SI_ID=SI_ID ";
                oleDbCommand.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
                dtIndentDetDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtIndentDetDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCapacityGrid");
                return dtIndentDetDetails;
            }
            finally
            {
                
            }
        }

    }
}
