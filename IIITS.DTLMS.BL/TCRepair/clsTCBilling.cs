using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
   public  class clsTCBilling
    {
       string strFormCode = "clsRepairDeliver";
       CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string sBillId { get; set; }
       public string sWONo { get; set; }
       public string sPONo { get; set; }
       public string sPODate { get; set; }
       public string sDescription { get; set; }
       public string sCrby { get; set; }

       OleDbCommand oleDbCommand;
       public DataTable LoadTCforBill(clsTCBilling objBill)
       {
           DataTable dt = new DataTable();
           try
           {
               oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
               strQry = "SELECT TC_CODE,TC_SLNO,(CASE WHEN TR_DELIVERY_LOCATION IS NULL THEN 'NO' ELSE 'YES' END ) DELIVERED,SM_NAME,";
               strQry += "TO_CHAR(TR_DELIVERY_DATE,'DD-MON-YYYY') DELIVERY_DATE,TR_RI_NO  FROM TBLTRANFORMERREPAIRS ,TBLTCMASTER,TBLSTOREMAST ";
               strQry += "WHERE TC_CODE=TR_DEVICE_ID AND SM_ID(+)=TR_DELIVERY_LOCATION AND TR_WO_NO=:WONO";
               strQry += " AND TR_WO_NO NOT IN ( SELECT BT_WO_NO from TBLBILLTC WHERE  BT_PAYMENT_FLAG=1 ) ";
                oleDbCommand.Parameters.AddWithValue("WONO", objBill.sWONo);
               //if (!Validation.IsAdmin)
               //{
               //    strQry += " and TR_DELIVERY_LOCATION ='" + Validation.strStoreId + "'";
               //}

               dt = ObjCon.getDataTable(strQry, oleDbCommand);
               return dt;

           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTCforBill");
               return dt;
           }
           finally
           {
               
           }
       }

       public string[] SaveTCBillDetails(clsTCBilling objBill)
       {
           string[] Arr = new string[2];
           string strQry = string.Empty;
           try
           {
               
               OleDbDataReader dr;

               if (objBill.sBillId == "")
               {
                   oleDbCommand = new OleDbCommand();
                    oleDbCommand.Parameters.AddWithValue("WONO", objBill.sWONo.ToUpper());
                    dr = ObjCon.Fetch("SELECT * FROM TBLBILLTC WHERE BT_WO_NO=:WONO", oleDbCommand);
                   if (dr.Read())
                   {
                       Arr[0] = "Bill already issued for this Work Order";
                       Arr[1] = "2";
                       return Arr;
                   }
                   dr.Close();

                   ObjCon.BeginTrans();
                    oleDbCommand = new OleDbCommand();
                    objBill.sBillId  = Convert.ToString(ObjCon.Get_max_no("BT_ID", "TBLBILLTC"));
                   strQry = "INSERT INTO TBLBILLTC(BT_ID,BT_WO_NO,BT_PO_NO,BT_PO_DATE,BT_DESC,BT_ENTRY_AUTH,BT_ENTRY_DATE) VALUES";
                   strQry += "('" + objBill.sBillId + "','" + objBill.sWONo.ToUpper() + "','" + objBill.sPONo.ToUpper() + "',";
                   strQry += "TO_DATE('" + objBill.sPODate + "','DD/MM/YYYY'),'" + objBill.sDescription.ToUpper() + "','" + objBill.sCrby + "',SYSDATE)";
                   ObjCon.Execute(strQry);
                   ObjCon.CommitTrans();

                   Arr[0] = "Bill Passed Successfully To Account Section";
                   Arr[1] = "0";
                   return Arr;
               }
               else
               {
                   ObjCon.BeginTrans();

                   strQry = " Update TBLBILLTC set BT_WO_NO=:WONO,BT_PO_NO=:sPONo,";
                   strQry += " BT_PO_DATE=to_date(:sPODate,'DD/MM/YYYY'),BT_DESC=:sDescription";
                   strQry += " where BT_ID=:sBillId";
                    oleDbCommand.Parameters.AddWithValue("WONO", objBill.sWONo.ToUpper());
                    oleDbCommand.Parameters.AddWithValue("sPONo", objBill.sPONo.ToUpper());
                    oleDbCommand.Parameters.AddWithValue("sPODate", objBill.sPODate);
                    oleDbCommand.Parameters.AddWithValue("sDescription", objBill.sDescription.ToUpper());
                    oleDbCommand.Parameters.AddWithValue("sBillId", objBill.sBillId);
                    
                    ObjCon.Execute(strQry, oleDbCommand);

                   ObjCon.CommitTrans();

                   Arr[0] = "Bill Details Updated Successfully";
                   Arr[1] = "1";
                   return Arr;
               }
              
           }
           catch (Exception ex)
           {
               ObjCon.RollBack();
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveTCBillDetails");
               return Arr;
           }
           finally
           {
               
           }
       }

       public DataTable LoadBillData()
       {
           DataTable dtBill = new DataTable();
           try
           {
               
               string strQry = string.Empty;

               strQry = "select BT_ID,BT_WO_NO,BT_PO_NO,TO_CHAR(BT_PO_DATE,'DD-MON-YYYY') BT_PO_DATE,BT_DESC FROM TBLBILLTC where BT_BR_NO is null";
               dtBill = ObjCon.getDataTable(strQry);
               return dtBill;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadBillData");
               return dtBill;
           }
           finally
           {
               
           }
       }

       public clsTCBilling GetBillDetails(clsTCBilling objBill)
       {
           DataTable dtBill = new DataTable();
           try
           {
               oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

               strQry = "select BT_ID,BT_WO_NO,BT_PO_NO,TO_CHAR(BT_PO_DATE,'DD/MM/YYYY') BT_PO_DATE,BT_DESC FROM TBLBILLTC where ";
               strQry += " BT_BR_NO is null AND BT_ID=:BTID";
               oleDbCommand.Parameters.AddWithValue("BTID", objBill.sBillId);
               dtBill = ObjCon.getDataTable(strQry, oleDbCommand);
               if (dtBill.Rows.Count > 0)
               {
                   objBill.sWONo = dtBill.Rows[0]["BT_WO_NO"].ToString();
                   objBill.sPONo = dtBill.Rows[0]["BT_PO_NO"].ToString();
                   objBill.sPODate = dtBill.Rows[0]["BT_PO_DATE"].ToString();
                   objBill.sDescription = dtBill.Rows[0]["BT_DESC"].ToString();
               }
               return objBill;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetBillDetails");
               return objBill;
           }
           finally
           {
               
           }
       }
    }
}
