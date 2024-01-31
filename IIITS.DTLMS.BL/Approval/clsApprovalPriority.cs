using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
namespace IIITS.DTLMS.BL
{
    public class clsApprovalPriority
    {
        string strFormCode = "clsApprovalPriority";
        
        CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        public string sModuleId { get; set; }
        public string sRoleId { get; set; }
        public string sPriority { get; set; }
        public DataTable dtRoles { get; set; }
        public string sApprovalId { get; set; }
        public string sCrBy { get; set; }
        public string sBOId { get; set; }


        // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

        public DataTable GetRoleNames(clsApprovalPriority objApproval)
        {
            string strQry = string.Empty;
            OleDbDataReader dr;
            DataTable dtRoleNames = new DataTable();
            try
            {
                strQry = "SELECT DISTINCT RO_ID, RO_NAME FROM TBLROLES,TBLUSERROLEMAPPING,TBLBUSINESSOBJECT WHERE RO_ID=UR_ROLEID AND ";
                strQry+= "  UR_BOID=BO_ID AND BO_ID='"+ objApproval.sBOId +"' AND UR_ACCESSTYPE IN (1,2,3) ORDER BY RO_ID";
               // strQry = "SELECT RO_NAME FROM TBLROLES,TBLUSERROLEMAPPING,TBLBUSINESSOBJECT WHERE RO_ID=UR_ROLEID AND UR_BOID=BO_ID AND BO_ID='"+objApproval.sModuleId+"'";                
                dtRoleNames = objCon.getDataTable(strQry);
                return dtRoleNames;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetRoleNames");
                return dtRoleNames;
            }
        }
        public DataTable LoadSavedRoles(string strModuleId)
        {
            string strQry = string.Empty;
            OleDbDataReader dr;
            DataTable dtRoleNames = new DataTable();
            try
            {
                strQry = "SELECT BO_ID,RO_ID,BO_NAME,RO_NAME,WM_LEVEL FROM TBLWORKFLOWMASTER,TBLBUSINESSOBJECT,TBLROLES WHERE WM_BOID='" + strModuleId + "' AND BO_ID=WM_BOID AND RO_ID=WM_ROLEID";
                dr = objCon.Fetch(strQry);
                dtRoleNames.Load(dr);
                dr.Close();
                return dtRoleNames;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadSavedRoles");
                return dtRoleNames;
            }
        }
        
        
        public string[] SaveRoles(clsApprovalPriority objApproval)
        {
            string strQry = string.Empty;
            string[] Arr = new string[2];
            try
            {

                if (objApproval.sModuleId == "")
                {
                    objCon.BeginTrans();
                    for (int i = 0; i < objApproval.dtRoles.Rows.Count; i++)
                    {
                        objApproval.sApprovalId = Convert.ToString(objCon.Get_max_no("WM_ID", "TBLWORKFLOWMASTER"));
                        objApproval.sModuleId = Convert.ToString(dtRoles.Rows[i]["BO_ID"]);
                        strQry = "INSERT INTO TBLWORKFLOWMASTER(WM_ID,WM_BOID,WM_ROLEID,WM_LEVEL,WM_CRBY,WM_CRON) VALUES('" + objApproval.sApprovalId + "','" + dtRoles.Rows[i]["BO_ID"] + "','" + dtRoles.Rows[i]["RO_ID"] + "','" + dtRoles.Rows[i]["WM_LEVEL"] + "','" + objApproval.sCrBy + "',SYSDATE)";
                        objCon.Execute(strQry);
                      
                    }
                    objCon.CommitTrans();
                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    objCon.BeginTrans();
                    strQry = "DELETE FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sModuleId + "'";
                    objCon.Execute(strQry);

                    if (objApproval.dtRoles != null)
                    {
                        //deleting old records
                        for (int i = 0; i < objApproval.dtRoles.Rows.Count; i++)
                        {
                            objApproval.sApprovalId = Convert.ToString(objCon.Get_max_no("WM_ID", "TBLWORKFLOWMASTER"));
                            strQry = "INSERT INTO TBLWORKFLOWMASTER(WM_ID,WM_BOID,WM_ROLEID,WM_LEVEL,WM_CRBY,WM_CRON) VALUES('" + objApproval.sApprovalId + "','" + dtRoles.Rows[i]["BO_ID"] + "','" + dtRoles.Rows[i]["RO_ID"] + "','" + dtRoles.Rows[i]["WM_LEVEL"] + "','" + objApproval.sCrBy + "',SYSDATE)";
                            objCon.Execute(strQry);
                        }
                    }
                    objCon.CommitTrans();
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                objCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveRoles");
                return Arr;
            }
        }
    }
}
