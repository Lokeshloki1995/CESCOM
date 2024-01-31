using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsCircle
    {
        CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsCircle";
        public string sCircleCode { get; set; }
        public string sCircleName { get; set; }
        public string sName { get; set; }
        public string sPhone { get; set; }
        public string sMobileNo { get; set; }
        public string sEmail { get; set; }
        public string sMaxid { get; set; }
        public string sUserId { get; set; }

        OleDbCommand oledbcommand;
        public string[] SaveCircle(clsCircle objCircle)
        {
            oledbcommand = new OleDbCommand();
            OleDbDataReader dr ;
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                
                if (objCircle.sMaxid == "")
                {
                    objCircle.sMaxid = objcon.Get_max_no("CM_ID", "TBLCIRCLE").ToString();
                    oledbcommand.Parameters.AddWithValue("CircleName", objCircle.sCircleName.ToUpper());
                    dr = objcon.Fetch("Select * from TBLCIRCLE where UPPER(CM_CIRCLE_NAME) =:CircleName", oledbcommand);


                    
                    if (dr.Read())
                    {
                        
                        Arr[0] = "Circle Name already exists";
                        Arr[1] = "3";
                        return Arr;
                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("CircleCode", objCircle.sCircleCode.ToUpper());
                    dr = objcon.Fetch("Select * from TBLCIRCLE where UPPER(CM_CIRCLE_CODE) =:CircleCode", oledbcommand);
                    if (dr.Read())
                    {
                       
                        Arr[0] = "Circle Code already exists";
                        Arr[1] = "3";
                        return Arr;
                    }
                    dr.Close();
                   
                    strQry = "INSERT INTO TBLCIRCLE(CM_ID, CM_CIRCLE_CODE ,CM_CIRCLE_NAME,CM_ZO_ID,CM_HEAD_EMP,CM_MOBILE_NO,CM_PHONE,CM_EMAIL)";
                    strQry += "VALUES(:sMaxid,:sCircleCode,";
                    strQry += ":sCircleName,1,:sName,";
                    strQry += ":sMobileNo,:sPhone,:sEmail )";
                    OleDbCommand command = new OleDbCommand();
                    command.Parameters.AddWithValue("sMaxid", objCircle.sMaxid);
                    command.Parameters.AddWithValue("sCircleCode", objCircle.sCircleCode);
                    command.Parameters.AddWithValue("sCircleName", objCircle.sCircleName);
                    command.Parameters.AddWithValue("sName", objCircle.sName);
                    command.Parameters.AddWithValue("sMobileNo", objCircle.sMobileNo);

                    command.Parameters.AddWithValue("sPhone", objCircle.sPhone);
                    command.Parameters.AddWithValue("sEmail", objCircle.sEmail);
                    objcon.Execute(strQry, command);




                    Arr[0] = "Saved Successfully ";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("circlename", objCircle.sCircleName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("CircleId", objCircle.sMaxid);
                    dr = objcon.Fetch("Select * from TBLCIRCLE where UPPER(CM_CIRCLE_NAME) =:circlename and CM_ID  <> :CircleId", oledbcommand);
                    if (dr.Read())
                    {
                       
                        Arr[0] = "Circle Name already exists";
                        Arr[1] = "3";
                        return Arr;
                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("circlecode1", objCircle.sCircleCode.ToUpper());
                    oledbcommand.Parameters.AddWithValue("circleid1", objCircle.sMaxid);
                    dr = objcon.Fetch("Select * from TBLCIRCLE where UPPER(CM_CIRCLE_CODE) =:circlecode1 and CM_ID <> :circleid1", oledbcommand);
                    if (dr.Read())
                    {
                     
                        Arr[0] = "Circle Code already exists";
                        Arr[1] = "3";
                        return Arr;
                    }

                    dr.Close();
                    strQry = " UPDATE TBLCIRCLE SET CM_CIRCLE_CODE= '"+ objCircle.sCircleCode + "', CM_CIRCLE_NAME= '"+ objCircle.sCircleName + "', ";
                    strQry += " CM_HEAD_EMP= '"+ objCircle.sName + "',CM_MOBILE_NO= '" + objCircle.sMobileNo + "', CM_PHONE= '" + objCircle.sPhone + "',";
                    strQry += " CM_EMAIL= '"+ objCircle.sEmail + "' , CM_UPDATED_ON = SYSDATE ,  CM_UPDATED_BY= '"+ objCircle.sUserId + "' WHERE CM_ID =" + objCircle.sMaxid + " ";
                   // OleDbCommand command = new OleDbCommand();
                   // command.Parameters.AddWithValue("sMaxid", objCircle.sMaxid);
                    //command.Parameters.AddWithValue("sCircleCode", objCircle.sCircleCode);
                    //command.Parameters.AddWithValue("sCircleName", objCircle.sCircleName);
                    //command.Parameters.AddWithValue("sName", objCircle.sName);
                    //command.Parameters.AddWithValue("sMobileNo", objCircle.sMobileNo);

                    //command.Parameters.AddWithValue("sPhone", objCircle.sPhone);
                    //command.Parameters.AddWithValue("sEmail", objCircle.sEmail);
                    //command.Parameters.AddWithValue("sUserId", objCircle.sUserId);

                   
                    objcon.Execute(strQry);







                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveCircle");
                return Arr;
            }
        }

        public DataTable LoadAllCircleDetails()
        {
            
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT CM_CIRCLE_CODE, To_char(CM_CIRCLE_CODE)CM_CIRCLE_CODE ,CM_CIRCLE_NAME FROM TBLCIRCLE";
                dt = objcon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllCircleDetails");
                return dt;
            }

        }

        public object getCircleDetails(clsCircle objCircle)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            
            try
            {
                oledbcommand.Parameters.AddWithValue("CircleId", objCircle.sMaxid);
                String strQry = "SELECT CM_CIRCLE_CODE ,CM_CIRCLE_NAME,CM_HEAD_EMP,CM_MOBILE_NO,CM_PHONE,CM_EMAIL FROM TBLCIRCLE ";
                strQry += " WHERE CM_CIRCLE_CODE =:CircleId";
                dtDetails = objcon.getDataTable(strQry, oledbcommand);
               

                if (dtDetails.Rows.Count > 0)
                {
                    objCircle.sCircleName = Convert.ToString(dtDetails.Rows[0]["CM_CIRCLE_NAME"].ToString());
                    objCircle.sName = Convert.ToString(dtDetails.Rows[0]["CM_HEAD_EMP"].ToString());
                    objCircle.sMobileNo = Convert.ToString(dtDetails.Rows[0]["CM_MOBILE_NO"].ToString());
                    objCircle.sPhone = Convert.ToString(dtDetails.Rows[0]["CM_PHONE"].ToString());
                    objCircle.sEmail = Convert.ToString(dtDetails.Rows[0]["CM_EMAIL"].ToString());
                    objCircle.sCircleCode = Convert.ToString(dtDetails.Rows[0]["CM_CIRCLE_CODE"].ToString());
                }
                return objCircle;
            }


            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getCircleDetails");
                return objCircle;
            }

        }

        public string GenerateCircleCode()
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                string sCircleCodeNo = objcon.get_value(" SELECT NVL(MAX(CM_CIRCLE_CODE),0)+1 FROM TBLCIRCLE ");
                if (sCircleCodeNo.Length > 0)
                {
                    sCircleCode = sCircleCodeNo.ToString();
                }
                return sCircleCodeNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateCircleCode");
                return "";
            }
        }

    }
}
