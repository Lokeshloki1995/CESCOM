using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
public class clsRoleMapping
{

       string strFormCode = "clsRoleMapping";
        CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        public string sModuleId { get; set; }
        public string sMappingId { get; set; }
        public string sRoleId { get; set; }
        public string sBusinessobjId { get; set; }
        public string sAccessId { get; set; }
        public string sCrby { get; set; }
        OleDbCommand oledbcommand;

        // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

        public DataTable LoadAllModuleDetails(clsRoleMapping objModuleDetails)
       {
           DataTable dt = new DataTable();
           string strQry = string.Empty;
           try
           {
               strQry = "SELECT BO_ID,BO_NAME,'' AS UR_ID  FROM TBLMODULES,TBLBUSINESSOBJECT WHERE MO_ID=BO_MO_ID ";
               strQry += " and MO_ID='" + objModuleDetails.sModuleId + "'";
               dt = objCon.getDataTable(strQry);
               return dt;

           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllModuleDetails");
               return dt;
           }

       }



        public DataTable LoadAllRoleDetails(clsRoleMapping objModuleDetails)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select  DISTINCT(BO_NAME), BO_ID,'' AS UR_ID, MO_ID,UR_ACCESSTYPE FROM TBLBUSINESSOBJECT, TBLMODULES,";
                strQry += " TBLUSERROLEMAPPING WHERE UR_ROLEID='" + objModuleDetails.sRoleId + "' AND UR_BOID='" + objModuleDetails.sBusinessobjId + "' ";
                strQry += " AND  BO_ID=UR_BOID AND MO_ID=BO_MO_ID";
                dt = objCon.getDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllRoleDetails");
                return dt;
            }

        }
        public string[] SaveAccessRights(clsRoleMapping objAccessDetails,string[] sAccessId)
        {
          
            string strQry = string.Empty;
            string[] sAccessTypes;
            string[] Arr = new string[2];

            try
            {
              //  objCon.BeginTrans();

                bool bResult = DeleteAccessRights(objAccessDetails);
                if (bResult == true)
                {

                    string[] strDetailVal = sAccessId.ToArray();
                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        if (strDetailVal[i] != null)
                        {
                            sAccessTypes = strDetailVal[i].Split(';');

                            for (int j = 1; j < sAccessTypes.Length; j++)
                            {
                                if (sAccessTypes[j] != "")
                                {
                                    oledbcommand = new OleDbCommand();
                                    objAccessDetails.sMappingId = objCon.Get_max_no("UR_ID", "TBLUSERROLEMAPPING").ToString();
                                    strQry = "INSERT INTO TBLUSERROLEMAPPING(UR_ID,UR_ROLEID,UR_BOID,UR_ACCESSTYPE,UR_CRON,UR_CRBY,UR_MOID)";
                                    strQry += "VALUES(:sMappingId,:sRoleId,:strDetailVal,";
                                    strQry+= ":sAccessTypes,SYSDATE,:sCrby,:sModuleId)";
                                    oledbcommand.Parameters.AddWithValue("sMappingId", objAccessDetails.sMappingId);
                                    oledbcommand.Parameters.AddWithValue("sRoleId", objAccessDetails.sRoleId);
                                    oledbcommand.Parameters.AddWithValue("strDetailVal", strDetailVal[i].Split(';').GetValue(0).ToString());
                                    oledbcommand.Parameters.AddWithValue("sAccessTypes", sAccessTypes[j]);
                                    oledbcommand.Parameters.AddWithValue("sCrby", objAccessDetails.sCrby);
                                    oledbcommand.Parameters.AddWithValue("sModuleId", objAccessDetails.sModuleId);
                                    objCon.Execute(strQry, oledbcommand);
                                }
                            }
                        }
                    }


                   // objCon.CommitTrans();
                    Arr[0] = "Saved Successfully ";
                    Arr[1] = "0";
                    
                }
                return Arr;
            }

            catch (Exception ex)
            {
                objCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveAccessRight");
                return Arr;
            }


        }

        public bool  DeleteAccessRights(clsRoleMapping objRoleMap)
        {
            try
            {
                string strQry = string.Empty;
                oledbcommand = new OleDbCommand();
                strQry = "DELETE FROM TBLUSERROLEMAPPING WHERE UR_ROLEID='" + objRoleMap.sRoleId + "' ";
                strQry += " AND UR_MOID='" + objRoleMap.sModuleId + "'";
                oledbcommand.Parameters.AddWithValue("sRoleId", objRoleMap.sRoleId);
                oledbcommand.Parameters.AddWithValue("sModuleId", objRoleMap.sModuleId);
                objCon.Execute(strQry, oledbcommand);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "DeleteAccessRights");
                return false;
            }
        }

    }
}
