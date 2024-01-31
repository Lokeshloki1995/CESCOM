using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsApkDownload
    {
        string strFormCode = "clsApkDownload";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string RetrieveLatestApkDetails()
        {
            string sFolderName = string.Empty;
            clsApkDownload objReturnApk = new clsApkDownload();
            try
            {
                sFolderName = ObjCon.get_value("SELECT MAX(AP_FOLDER_PATH) FROM TBLDTLMSAPK ");
            }
            catch (Exception e)
            {
                clsException.LogError(e.StackTrace, e.Message, strFormCode, "RetrieveLatestApkDetails");
                return sFolderName;
            }

            return sFolderName;
        }
    }
}
