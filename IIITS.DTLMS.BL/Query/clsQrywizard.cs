using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsQrywizard
    {
        CustOledbConnection objCon = new CustOledbConnection(Constants.Password);

        public DataTable GetResult(string strRequestQry, string strUserid)
        {
            string strResult = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strQuery = string.Empty;
                strQuery = "INSERT INTO TBLQUERYLOG(QL_ID,QL_TEXT,QL_ENTRYAUTH) VALUES ";
                strQuery+= " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strRequestQry.Replace("'", "''") + "','" + strUserid + "')";
                objCon.Execute(strQuery);
                dt = objCon.getDataTable(strRequestQry);
                return dt;
            }
            catch (Exception ex)
            {
                dt.Columns.Add("Error");
                dt.Rows.Add(ex.Message);
                return dt;
            }
        } 
    }
}
