using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;
namespace IIITS.DTLMS.BL
{
   public class clsStockStatus
    {
       string strFormCode = "clsStockStatus";
       CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sStoreId { get; set; }
       public string sStoreName { get; set; }
       public string sStorelocation { get; set; }
       public string sStockCount { get; set; }
       public string sCapacity { get; set; }
       OleDbCommand oledbCommand;
       public DataTable LoadStockGrid(clsStockStatus ObjStore)
       {
        
           DataTable dtStoreDetails = new DataTable();
           string strQry = string.Empty;
           try
           {
               
               //strQry = "SELECT * FROM ( select SM_ID,SM_NAME ,(select SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+2) from VIEW_ALL_OFFICES where OFF_CODE=SM_OFF_CODE) SM_OFF_CODE,";
               //strQry+= " TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,COUNT(TC_CODE)TC_CODE from TBLTCMASTER,TBLSTOREMAST WHERE TC_STORE_ID=SM_ID and TC_STATUS IN(1,2) AND ";
               //strQry += " TC_CURRENT_LOCATION=1  AND TC_CAPACITY IS NOT NULL GROUP BY SM_ID,SM_NAME,TC_CAPACITY,SM_OFF_CODE ORDER BY SM_NAME ) WHERE ";
               //strQry += " UPPER(NVL(SM_NAME,0)) like '%" + ObjStore.sStoreName.ToUpper() + "%' AND UPPER(NVL(SM_OFF_CODE,0)) LIKE '%" + ObjStore.sStorelocation.ToUpper() + "%'";
               oledbCommand = new OleDbCommand();
               strQry = "SELECT  SM_ID,SM_NAME,NVL(TC_CAPACITY,'TOTAL')TC_CAPACITY,SUM(TC_CODE)TC_CODE FROM ( select SM_ID,SM_NAME ,(select SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+2) from VIEW_ALL_OFFICES where OFF_CODE=SM_OFF_CODE) SM_OFF_CODE,";
               strQry += " TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,COUNT(TC_CODE)TC_CODE from TBLTCMASTER,TBLSTOREMAST WHERE TC_STORE_ID=SM_ID and TC_STATUS IN(1,2) AND ";
               strQry += " TC_CURRENT_LOCATION=1  AND TC_CAPACITY IS NOT NULL GROUP BY SM_ID,SM_NAME,TC_CAPACITY,SM_OFF_CODE ORDER BY SM_NAME ) WHERE ";
               strQry += " UPPER(NVL(SM_NAME,0)) like'%'||:sStoreName||'%' ";
               oledbCommand.Parameters.AddWithValue("sStoreName", ObjStore.sStoreName.ToUpper());
               if (ObjStore.sCapacity != "")
               {
                   strQry += " AND TC_CAPACITY=:sCapacity";
                   oledbCommand.Parameters.AddWithValue("sCapacity", ObjStore.sCapacity);
               }
               strQry += " AND UPPER(NVL(SM_OFF_CODE, 0)) LIKE :sStorelocation||'%'";
               strQry += " GROUP BY GROUPING SETS ((SM_ID,SM_NAME,SM_OFF_CODE,TC_CAPACITY,TC_CODE),SM_NAME,()) ";
               strQry += "  ORDER BY SM_NAME ";
               oledbCommand.Parameters.AddWithValue("sStorelocation", ObjStore.sStorelocation);


               dtStoreDetails = ObjCon.getDataTable(strQry, oledbCommand);
               return dtStoreDetails;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStockGrid");
               return dtStoreDetails;

           }           

       }
    }
}
