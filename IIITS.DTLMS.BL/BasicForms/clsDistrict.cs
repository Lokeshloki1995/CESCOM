using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
   public class clsDistrict
    {
       string sformcode = "ClsDistrict";
       CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
       public string sDistId { get; set; }
       public string sDistrictCode { get; set; }
       public string sDistrictName { get; set; }
       public string sButtonname { get; set; }

       OleDbCommand oledbcommand;
        public string[] SaveDetails(clsDistrict objDis)
        {
           
            string strQry=string.Empty;
            string[] Arr = new string[3];
            OleDbDataReader dr ;
            try
            {

                    if (objDis.sDistId == "")
                    {
                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("DtCode", objDis.sDistrictCode.ToUpper());
                        strQry = "select * from tbldist where UPPER(DT_CODE)=:DtCode";
                        dr = objcon.Fetch(strQry, oledbcommand);
                        if (dr.Read())
                        {
                            Arr[0] = "District Code AlreadyExist ";
                            Arr[1] = "1";
                            return Arr;
                        }
                        dr.Close();

                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("DtName", objDis.sDistrictName.ToUpper());
                        strQry = "select * from tbldist where UPPER(DT_NAME)=:DtName";
                        dr = objcon.Fetch(strQry, oledbcommand);
                        if (dr.Read())
                        {
                            Arr[0] = "District Name AlreadyExist ";
                            Arr[1] = "1";
                            return Arr;
                        }
                        dr.Close();

                        objDis.sDistId = objcon.Get_max_no("DT_ID", "TBLDIST").ToString();
                        strQry = "insert into tbldist (DT_CODE,DT_NAME,DT_ID) values('" + objDis.sDistrictCode + "',";
                        strQry += "'" + objDis.sDistrictName.Trim().Replace(" ", "") + "','" + objDis.sDistId + "')";
                        objcon.Execute(strQry);
                        Arr[0] = "Saved Succesfully";
                        Arr[1] = "0";
                        return Arr;
                    }
                    else
                    {
                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("dtname1", objDis.sDistrictName.ToUpper());
                        oledbcommand.Parameters.AddWithValue("dtcode1", objDis.sDistrictCode);
                        strQry = "select * from tbldist where UPPER(DT_NAME)= :dtname1 and DT_CODE <> :dtcode1";
                        dr = objcon.Fetch(strQry, oledbcommand);
                        if (dr.Read())
                        {
                            Arr[0] = "District Name AlreadyExist ";
                            Arr[1] = "1";
                            return Arr;
                        }
                        dr.Close();

                        strQry = "update tbldist set ";
                        strQry += " DT_NAME='" + objDis.sDistrictName.Trim().Replace(" ", "") + "' where DT_ID='" + objDis.sDistId + "'";
                        objcon.Execute(strQry);
                        Arr[0] = "Updated Successfully ";
                        Arr[1] = "1";
                        return Arr;
                    }
              
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GetDistDetails");
            }
            return Arr;
        }

        public object GetDistDetails(clsDistrict objDistrict)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("DtId", objDistrict.sDistId);
                String strQry = "SELECT * FROM TBLDIST ";
                strQry += " WHERE DT_ID=:DtId";
                dtDetails = objcon.getDataTable(strQry, oledbcommand);

                if (dtDetails.Rows.Count > 0)
                {
                    objDistrict.sDistrictCode = Convert.ToString(dtDetails.Rows[0]["DT_CODE"].ToString());
                    objDistrict.sDistrictName = Convert.ToString(dtDetails.Rows[0]["DT_NAME"].ToString());
                }
                return objDistrict;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GetDistDetails");
                return objDistrict;
            }
        }

        public DataTable LoadAllDistDetails()
        {
            oledbcommand = new OleDbCommand();
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT DT_ID,To_char(DT_CODE)DT_CODE,DT_NAME FROM TBLDIST ORDER BY DT_ID";
                dt = objcon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "LoadAllDistDetails");
                return dt;
            }

        }

        public string GenerateDistrictCode()
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                string sDistCode = objcon.get_value(" SELECT NVL(MAX(DT_CODE),0)+1 FROM TBLDIST ");
                if (sDistCode.Length > 0)
                {
                    sDistrictCode = sDistCode.ToString();
                }
                return sDistCode;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GenerateCircleCode");
                return "";
            }
        }
    }
}
