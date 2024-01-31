using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;


namespace IIITS.DTLMS.BL
{
   public  class clsBillPayment
    {
       string strFormCode = "clsBillPayment";
       CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
       OleDbCommand oledbCommand;
        public string sWONo { get; set; }
       public string sBRNo { get; set; }
       public string sAmount { get; set; }
       public string sPaymentDate { get; set; }
       public string sBillId { get; set; }
       public string sCrby { get; set; }
       public string sPONo { get; set; }
       public string sPODate { get; set; }



       public DataTable  LoadBillDetails(clsBillPayment objPayment)
       {
           DataTable dt = new DataTable();
           oledbCommand = new OleDbCommand();
           try
           {
               
               string strQry = string.Empty;
               strQry = "SELECT TC_CODE,(CASE WHEN TR_DELIVERY_LOCATION IS NULL THEN 'NO' ELSE 'YES' END ) DELIVERED,SM_NAME,";
               strQry += "TO_CHAR(TR_DELIVERY_DATE,'DD-MON-YYYY') DELIVERY_DATE,TR_RI_NO  FROM TBLTRANFORMERREPAIRS ,TBLTCMASTER,TBLSTOREMAST ";
               strQry += "WHERE TC_CODE=TR_DEVICE_ID AND SM_ID(+)=TR_DELIVERY_LOCATION AND TR_WO_NO=:WoNo";
               oledbCommand.Parameters.AddWithValue("WoNo", objPayment.sWONo);


               dt = ObjCon.getDataTable(strQry, oledbCommand);
               return dt;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetFaultTCDetails");
               return dt;
           }
           finally
           {
               
           }
       }

       public string[] SaveBillPayment(clsBillPayment objPayment)
       {
           string[] Arr = new string[2];
           string strQry = string.Empty;
           try
           {
                oledbCommand = new OleDbCommand();
                ObjCon.BeginTrans();

               strQry = " Update TBLBILLTC set BT_BR_NO=:sBRNo,BT_AMOUNT=:sAmount,BT_PAYMENT_AUTH=:sCrby,";
               strQry += " BT_PAYMENT_DATE=to_date(:sPaymentDate,'DD/MM/YYYY'),BT_PAYMENT_FLAG=1, BT_PAYMENT_ENTRY_DATE=SYSDATE";
               strQry += " WHERE BT_ID=:sBillId";

                oledbCommand.Parameters.AddWithValue("sBRNo", objPayment.sBRNo.ToUpper());
                oledbCommand.Parameters.AddWithValue("sAmount", objPayment.sAmount.Trim().ToUpper());
                oledbCommand.Parameters.AddWithValue("sCrby", objPayment.sCrby);
                oledbCommand.Parameters.AddWithValue("sPaymentDate", objPayment.sPaymentDate);
                oledbCommand.Parameters.AddWithValue("sBillId", objPayment.sBillId);
                //oledbCommand.Parameters.AddWithValue("WoNo", objPayment.sWONo);
                ObjCon.Execute(strQry, oledbCommand);

               ObjCon.CommitTrans();
               Arr[0] = "Bill Passed Successfully To Account Section";
               Arr[1] = "0";
               return Arr;
               
           }
           catch (Exception ex)
           {
               ObjCon.RollBack();
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetFaultTCDetails");
               return Arr;
           }
           finally
           {
               
           }
       }

       public clsBillPayment   GetBillDetails(clsBillPayment objBill)
       {
           DataTable dtBill = new DataTable();
           try
           {
               oledbCommand = new OleDbCommand();
               string strQry = string.Empty;

               strQry = "  SELECT BT_WO_NO,TO_CHAR(BT_ID) BT_ID,BT_PO_NO,to_char(BT_PO_DATE,'DD/MM/YYYY') BT_PO_DATE FROM TBLBILLTC WHERE BT_PAYMENT_FLAG=0 AND BT_WO_NO=:WoNo";
               oledbCommand.Parameters.AddWithValue("WoNo", objBill.sWONo);


               dtBill = ObjCon.getDataTable(strQry, oledbCommand);
              // dtBill = ObjCon.getDataTable(strQry);
               if (dtBill.Rows.Count > 0)
               {
                   objBill.sBillId = dtBill.Rows[0]["BT_ID"].ToString();
                   objBill.sWONo = dtBill.Rows[0]["BT_WO_NO"].ToString();
                   objBill.sPONo = dtBill.Rows[0]["BT_PO_NO"].ToString();
                   objBill.sPODate = dtBill.Rows[0]["BT_PO_DATE"].ToString();
               }

               return objBill;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetBillID");
               return objBill;
           }
           finally
           {
               
           }
       }
      
    }
}
