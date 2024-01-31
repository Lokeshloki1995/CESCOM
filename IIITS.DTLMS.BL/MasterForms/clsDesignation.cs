using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
   public class clsDesignation
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sDesignationId { get; set; }
        public string sDesignationName { get; set; }
        public string sDesignationDesc { get; set; }
        public string sCrby { get; set; }

        OleDbCommand oledbcommand ;
        public string[] SaveDetails(clsDesignation objDesignation)
        {
         
            string[] Arr = new string[2];
            try
            {
                oledbcommand = new OleDbCommand();
                string strQry = string.Empty;
                
                if (objDesignation.sDesignationId == "")
                {
                    oledbcommand.Parameters.AddWithValue("DesignationName", objDesignation.sDesignationName.ToUpper());
                    OleDbDataReader dr = ObjCon.Fetch("SELECT DM_NAME FROM TBLDESIGNMAST WHERE UPPER(DM_NAME)=:DesignationName", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Designation Name Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();

                    string StrGetMaxNo = ObjCon.Get_max_no("DM_DESGN_ID", "TBLDESIGNMAST").ToString();
                    strQry = "INSERT INTO TBLDESIGNMAST(DM_DESGN_ID,DM_NAME,DM_DESC,DM_CRBY)";
                    strQry += "VALUES('" + StrGetMaxNo + "','" + objDesignation.sDesignationName.ToUpper() + "','"+objDesignation.sDesignationDesc+"','"+ objDesignation.sCrby  +"')";   
                    ObjCon.Execute(strQry);
                    Arr[0] = StrGetMaxNo.ToString();
                    Arr[1] = "0";
                    return Arr;

                }

                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("designationname1", objDesignation.sDesignationName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("DesignationId1", objDesignation.sDesignationId);

                    OleDbDataReader dr = ObjCon.Fetch("SELECT DM_NAME FROM TBLDESIGNMAST WHERE UPPER(DM_NAME)=:designationname1 AND DM_DESGN_ID<> :DesignationId1", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Designation Name Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();

                    strQry = "UPDATE TBLDESIGNMAST SET DM_UPDATED_BY  = '"+ objDesignation.sCrby + "' , DM_UPDATED_ON = SYSDATE  ,  DM_NAME='" + objDesignation.sDesignationName.ToUpper() + "',";
                    strQry += "DM_DESC='" + objDesignation.sDesignationDesc + "' WHERE DM_DESGN_ID='" + objDesignation.sDesignationId+ "'";       
                    ObjCon.Execute(strQry);
                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "1";
                    return Arr;


                }

            }

            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, "clsDesignation", "SaveDetails");
                return Arr;
            }
            finally
            {
                
            }

        
        }
        public DataTable LoadDetails()
        {
            oledbcommand = new OleDbCommand();
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                strQry = "Select DM_DESGN_ID,DM_NAME,DM_DESC from TBLDESIGNMAST ORDER BY DM_DESGN_ID DESC";
                dt = ObjCon.getDataTable(strQry);

               

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, "clsDesignation", "LoadDetails");
                return dt;
            }
            finally
            {
                
            }
            
        }




        public object getDesignationDetails(clsDesignation objDesignation)
        {
           
            DataTable dtDetails = new DataTable();
           
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("DesignationId", objDesignation.sDesignationId);
                String strQry = "SELECT DM_DESGN_ID,DM_NAME,DM_DESC FROM TBLDESIGNMAST ";
                strQry += " WHERE DM_DESGN_ID= :DesignationId";
                dtDetails = ObjCon.getDataTable(strQry, oledbcommand);
                

                if (dtDetails.Rows.Count > 0)
                {
                    objDesignation.sDesignationId = Convert.ToString(dtDetails.Rows[0]["DM_DESGN_ID"].ToString());
                    objDesignation.sDesignationName = Convert.ToString(dtDetails.Rows[0]["DM_NAME"].ToString());
                    objDesignation.sDesignationDesc = Convert.ToString(dtDetails.Rows[0]["DM_DESC"].ToString());
                    
                }
                return objDesignation;
            }


            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, "clsDesignation", "getDesignationDetails");
                return objDesignation;
            }
            finally
            {
                
            }

        }

   }
}
