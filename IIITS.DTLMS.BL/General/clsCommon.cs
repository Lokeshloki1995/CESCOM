using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;

namespace IIITS.DTLMS.BL
{
    public class clsCommon
    {
        
        string strFormCode = "clsCommon";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public DataTable  GetAppSettings()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
               
                strQry = "SELECT AP_KEY,AP_VALUE FROM TBLAPPSETTINGS";
                dt = ObjCon.getDataTable(strQry);
                return dt;
   
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetAppSettings");
                return dt;
            }
        }
        public string GetLastPricingDate(string offCode)
        {
            DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
            string lastPricingDate = string.Empty;
            try
            {
                lastPricingDate = objWCF.GetLastPricingDate(offCode);
                return lastPricingDate;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetLastPricingDate");
                return "Something Went Wrong";
            }
        }
    }
}
