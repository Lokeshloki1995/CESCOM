using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsLatestUpdates
    {
        string strFormCode = "clsContactUs";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public DataTable GetLatestUpdates()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT UPDATEDESCRIPTION , TO_CHAR(EFFECTFROM,'dd-MON-yyyy')EFFECTFROM   FROM TBLLATESTUPDATES WHERE STATUS = 1";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDetails");
                return dt;
            }

        }

    }
}
