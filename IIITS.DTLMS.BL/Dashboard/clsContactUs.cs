using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsContactUs
    {
        string strFormCode = "clsContactUs";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oleDbCommand = new OleDbCommand();

        public DataSet LoadDetails()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataSet Dset = new DataSet();
            string[] strQry = new string[4];
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    oleDbCommand.Parameters.AddWithValue("Value", i);
                    strQry[i] = "Select CC_NAME,CC_MOBILENO,CC_EMAIL,(select DIV_NAME from TBLDIVISION where DIV_CODE=CC_OFFCODE)DIV,(select CM_CIRCLE_NAME from TBLCIRCLE where CM_CIRCLE_CODE=substr(CC_OFFCODE,0,1))CIRCLE from TBLCONTACTCONFIG where CC_LEVEL=:Value";
                    if (i == 1)
                    {
                        dt1 = ObjCon.getDataTable(strQry[i], oleDbCommand);
                        Dset.Tables.Add(dt1);
                    }
                    else if (i == 2)
                    {
                        dt2 = ObjCon.getDataTable(strQry[i], oleDbCommand);
                        Dset.Tables.Add(dt2);
                    }
                    else if(i==3)
                    {
                         dt3 = ObjCon.getDataTable(strQry[i], oleDbCommand);
                         Dset.Tables.Add(dt3);
                    }
                }
                return Dset;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDetails");
                return Dset;
            }

        }


        public DataTable LoadDetailsForFirstGrid()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select CC_NAME,CC_MOBILENO,CC_EMAIL,(select DIV_NAME from TBLDIVISION where DIV_CODE=CC_OFFCODE)DIV,";
                strQry += "   (SELECT CM_CIRCLE_NAME from TBLCIRCLE where CM_CIRCLE_CODE=substr(CC_OFFCODE,0,1))CIRCLE";
                 strQry += "       from TBLCONTACTCONFIG  where CC_LEVEL= '1'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDetailsForFirstGrid");
                return dt;
            }

        }

        public DataTable LoadDetailsForSecondGrid()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select CC_NAME,CC_MOBILENO,CC_EMAIL";
               
                strQry += "   from TBLCONTACTCONFIG  where CC_LEVEL= '2'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDetailsForSecondGrid");
                return dt;
            }

        }


        public DataTable LoadDetailsForThirdGrid()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select CC_NAME,CC_MOBILENO,CC_EMAIL";
           
                strQry += "       from TBLCONTACTCONFIG  where CC_LEVEL= '3'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDetailsForThirdGrid");
                return dt;
            }

        }

    }
}

