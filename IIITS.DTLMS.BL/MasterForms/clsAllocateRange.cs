using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsAllocateRange
    {

        string StrQry = string.Empty;
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbcommand;
        public string sDwaNumber { get; set; }
        public string sPONumber { get; set; }
        public string sDWADate { get; set; }
        public string sPODate { get; set; }

        public string sMake_Id { get; set; }//sMake_Id is nothing but vendor id 
        public string sStart_Range { get; set; }
        public string sEnd_Range { get; set; }
        public string sCrby { get; set; }
        public int sQty { get; set; }
        public string squantity { get; set; }


        public string getmaxssplate_no()
        {           
            string maxno = string.Empty;
            
            try
            {
                oledbcommand = new OleDbCommand();
                //StrQry = "SELECT  NVL(MAX(MD_ID),0)+1 FROM TBLALLOCATERANGEDETAILS";
                StrQry = "SELECT  NVL(MAX(TCPM_ID),0)+1 FROM TBLTCPLATEALLOCATIONMASTER";
                int sMaxnum = Convert.ToInt16(ObjCon.get_value(StrQry));
                if (sMaxnum == 1)
                {
                    StrQry = "SELECT nvl(max(TC_CODE),0)+1 FROM TBLTCMASTER";
                    maxno = ObjCon.get_value(StrQry);
                    return maxno;
                }
                else
                {
                    // StrQry = "SELECT NVL(MAX(MD_END_RANGE),0)+1 FROM TBLALLOCATERANGEDETAILS";
                    StrQry = "SELECT NVL(MAX(TCPM_END_RANGE),0)+1 FROM TBLTCPLATEALLOCATIONMASTER";
                    maxno = ObjCon.get_value(StrQry);
                    return maxno;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

                return maxno;
            }
            finally
            {
                
            }
        }
       
        public string[] SaveDetails(clsAllocateRange objRange)
        {

            String[] arr = new String[2];
            string j;
            string res = string.Empty;
            int[] plateArray;

            try
            {
                oledbcommand = new OleDbCommand();
                plateArray = new int[Convert.ToInt32(objRange.sQty)];
                plateArray[0] = Convert.ToInt32(objRange.sStart_Range);//store the first plate number
                for (int i = 1; i < objRange.sQty; i++)
                {
                    plateArray[i] = plateArray[i - 1] + 1;
                }
                for (int i = 0; i < objRange.sQty; i++)
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TcpTcCode", plateArray[i]);
                    StrQry = "SELECT TCP_TC_CODE || '~' ||VM_NAME FROM TBLTCPLATEALLOCATION,TBLTCPLATEALLOCATIONMASTER,TBLVENDORMASTER WHERE TCP_TCPM_ID=TCPM_ID AND TCPM_VENDOR_ID=VM_ID AND TCP_TC_CODE=:TcpTcCode";
                    res = ObjCon.get_value(StrQry, oledbcommand);
                    if (res != "")
                    {
                        arr[0] = "TC code " + res.Split('~').GetValue(0).ToString() + " has been allocated to " + " " + res.Split('~').GetValue(1).ToString() + " " + "Vendor ";
                        arr[1] = "1";
                        return arr;
                    }
                }

                ObjCon.BeginTrans();
                StrQry = " INSERT INTO TBLTCPLATEALLOCATIONMASTER (TCPM_ID , TCPM_VENDOR_ID ,  TCPM_START_RANGE , TCPM_END_RANGE , TCPM_DESCRIPTION ," +
                    " TCPM_CRBY , TCPM_CRON , TCPM_PO_NO , TCPM_PO_DATE ,TCPM_DWA_NO ,  TCPM_DWA_DATE  ,TCPM_QUANTITY )  VALUES ( " +
                    " (SELECT NVL(MAX(TCPM_ID)+1,1) FROM TBLTCPLATEALLOCATIONMASTER), " + objRange.sMake_Id + "  ,      " +
                    " '" + objRange.sStart_Range + "','" + objRange.sEnd_Range + "','','" + objRange.sCrby + "',SYSDATE , " +
                    "  '" + objRange.sPONumber + "' , TO_DATE('" + objRange.sPODate + "','dd/MM/yyyy') ,'" + objRange.sDwaNumber + "' ,TO_DATE('" + objRange.sDWADate + "','dd/MM/yyyy') ,'"+objRange.sQty+"' ) ";
                ObjCon.Execute(StrQry);


                //StrQry = "INSERT INTO TBLTCPLATEALLOCATIONMASTER VALUES((SELECT NVL(MAX(TCPM_ID)+1,1) FROM TBLTCPLATEALLOCATIONMASTER),'" + objRange.sMake_Id + "',";
                //StrQry += "'" + objRange.sStart_Range + "','" + objRange.sEnd_Range + "','','" + objRange.sCrby + "',SYSDATE,'','')";
                //ObjCon.Execute(StrQry);
                j = ObjCon.get_value("SELECT MAX(TCPM_ID) FROM TBLTCPLATEALLOCATIONMASTER");
                for (int i = 0; i < sQty; i++)
                {
                    StrQry = "INSERT INTO TBLTCPLATEALLOCATION(TCP_ID,TCP_TCPM_ID,TCP_TC_CODE,TCP_MMS_DI_ID,TCP_STATUS_FLAG,TCP_DESC,TCP_CRBY,TCP_CRON) VALUES((SELECT NVL(MAX(TCP_ID)+1,1) FROM TBLTCPLATEALLOCATION),'" + j + "',";
                    StrQry += " '" + plateArray[i] + "' ,'','0','','" + objRange.sCrby + "',SYSDATE)";
                    ObjCon.Execute(StrQry);
                }
                ObjCon.CommitTrans();
                arr[0] = "Range Set Succesfully";
                arr[1] = "0";
                return arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace + StrQry, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

                arr[0] = "Exception occured";
                arr[1] = "1";
                return arr;
            }
            finally
            {

            }

        }

        public DataTable GetPlateAllocatedRangeDetails(string splateNumber)
        {

            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                if (!splateNumber.Equals(""))
                {

                    StrQry = "SELECT TCPM_ID,VM_NAME,  TCPM_PO_NO , to_char( TCPM_PO_DATE ,  'dd-MON-yyyy' ) TCPM_PO_DATE ,TCPM_DWA_NO , to_char( TCPM_DWA_DATE ,  'dd-MON-yyyy' )  TCPM_DWA_DATE ,  " +
                        "TCPM_START_RANGE,TCPM_END_RANGE,TCPM_QUANTITY,to_char( TCPM_CRON ,  'dd-MON-yyyy' ) TCPM_CRON FROM TBLTCPLATEALLOCATIONMASTER,";
                    StrQry += "TBLVENDORMASTER,TBLTCPLATEALLOCATION WHERE TCPM_VENDOR_ID = VM_ID AND TCPM_ID = TCP_TCPM_ID ";
                    //StrQry += "AND TCP_TC_CODE= :tcptccode  ORDER BY TCPM_START_RANGE";
                    StrQry += "AND TCP_TC_CODE= :tcptccode  ORDER BY TCPM_START_RANGE ";

                    oledbcommand.Parameters.AddWithValue("tcptccode", splateNumber);
                    dt = ObjCon.getDataTable(StrQry, oledbcommand);
                }
                else
                {
                    StrQry = "SELECT TCPM_ID,VM_NAME,  TCPM_PO_NO , to_char( TCPM_PO_DATE ,  'dd-MON-yyyy' ) TCPM_PO_DATE ,TCPM_DWA_NO , to_char( TCPM_DWA_DATE ,  'dd-MON-yyyy' )  TCPM_DWA_DATE , " +
                        "TCPM_START_RANGE,TCPM_END_RANGE, TCPM_QUANTITY,to_char( TCPM_CRON ,  'dd-MON-yyyy' ) TCPM_CRON FROM TBLTCPLATEALLOCATIONMASTER,";
                    //StrQry += "TBLVENDORMASTER WHERE TCPM_VENDOR_ID = VM_ID ORDER BY TCPM_START_RANGE";
                    StrQry += "TBLVENDORMASTER WHERE TCPM_VENDOR_ID = VM_ID ORDER BY TCPM_START_RANGE desc";
                    dt = ObjCon.getDataTable(StrQry);
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            finally
            {

            }


        }
    }
}
