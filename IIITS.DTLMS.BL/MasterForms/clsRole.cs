using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsRole
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sRoleId { get; set; }
        public string sRoleName { get; set; }
        public string sRoleDesig { get; set; }
        public string sCrby { get; set; }
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string iStatus { get; set; }
        public string sMessage { get; set; }

        OleDbCommand oledbcommand;
        public string[] SaveDetails(clsRole objRole)
        {
            string sFolderPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            oledbcommand = new OleDbCommand();
            string[] Arr = new string[2];
            try
            {
                
                string strQry = string.Empty;
                if (objRole.sRoleId == "")
                {
                    oledbcommand.Parameters.AddWithValue("RoName", objRole.sRoleName.ToUpper());
                    OleDbDataReader dr = ObjCon.Fetch("SELECT RO_NAME FROM TBLROLES WHERE UPPER(RO_NAME)=:RoName", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Role Name Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();

                    string StrGetMaxNo = ObjCon.Get_max_no("RO_ID", "TBLROLES").ToString();
                    ObjCon.BeginTrans();
                    strQry = "INSERT INTO TBLROLES(RO_ID,RO_NAME,RO_DESIGNATION,RO_CRBY)";
                    strQry += "VALUES('" + StrGetMaxNo + "','" + objRole.sRoleName.ToUpper() + "','" + objRole.sRoleDesig + "','" + objRole.sCrby + "')";
                    ObjCon.Execute(strQry);
                    Arr[0] = StrGetMaxNo.ToString();
                    Arr[1] = "0";


                    if (Convert.ToString(ConfigurationSettings.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                    {
                        objRole.RoleId = Convert.ToInt32(Arr[0].ToString());
                        objRole.RoleName = objRole.sRoleName.ToUpper();
                        objRole.ModuleId = 4;
                        WebClient objclien = new WebClient();
                        objclien.Headers.Add("Authorization", "HRMS14062019");
                        DataContractJsonSerializer objS = new DataContractJsonSerializer(typeof(string));
                        objclien.Headers["Content-type"] = "application/json";
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(clsRole));
                        serializerToUplaod.WriteObject(ms, objRole);
                        string hrmsip = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SaveRoleDetails"]);
                        byte[] data = objclien.UploadData(hrmsip, "POST", ms.ToArray());
                        Stream stream = new System.IO.MemoryStream(data);
                        objS = new DataContractJsonSerializer(typeof(clsRole));
                        var result = objS.ReadObject(stream) as clsRole;
                        var Status = result.iStatus;
                        var Message = result.sMessage;

                        if (Status == "-1")
                        {
                            ObjCon.RollBack();
                            Arr[0] = "Something Went Wrong";
                            Arr[1] = "-1";
                        }
                        else
                        {
                            ObjCon.CommitTrans();
                        }

                        File.AppendAllText(sPath, "\n DTLMS \n DateTime    : " + System.DateTime.Now +  Environment.NewLine +"Inputs : " + Environment.NewLine+" RoleId = " + objRole.RoleId + Environment.NewLine+" RoleName = " + objRole.RoleName + Environment.NewLine+" ModuleId = " + objRole.ModuleId + Environment.NewLine+" ******************************** " + Environment.NewLine+" Result : " + Environment.NewLine+" Status = " + Status + Environment.NewLine +"Message = " + Message + Environment.NewLine+" ##############################################################"+ Environment.NewLine);
                        return Arr;
                    }
                    else
                    {
                        ObjCon.CommitTrans();
                        return Arr;
                    }
                }

                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("roname1", objRole.sRoleName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("roid1", objRole.sRoleId);
                    OleDbDataReader dr = ObjCon.Fetch("SELECT RO_NAME FROM TBLROLES WHERE UPPER(RO_NAME)=:roname1 AND RO_ID<>:roid1", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Role Name Already Exists";
                        Arr[1] = "2";
                        dr.Close();
                        return Arr;
                    }
                    dr.Close();
                    ObjCon.BeginTrans();
                    strQry = "UPDATE TBLROLES SET RO_NAME='" + objRole.sRoleName.ToUpper() + "',";
                    strQry += "RO_DESIGNATION='" + objRole.sRoleDesig + "' WHERE RO_ID='" + objRole.sRoleId + "'";
                    ObjCon.Execute(strQry);
                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "1";

                    if (Convert.ToString(ConfigurationSettings.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                    {
                        objRole.RoleId = Convert.ToInt32( objRole.sRoleId);
                        objRole.RoleName = objRole.sRoleName.ToUpper();
                        objRole.ModuleId = 4;
                        WebClient objclien = new WebClient();
                        objclien.Headers.Add("Authorization", "HRMS14062019");
                        DataContractJsonSerializer objS = new DataContractJsonSerializer(typeof(string));
                        objclien.Headers["Content-type"] = "application/json";
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(clsRole));
                        serializerToUplaod.WriteObject(ms, objRole);
                        string hrmsip = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SaveRoleDetails"]);
                        byte[] data = objclien.UploadData(hrmsip, "POST", ms.ToArray());
                        Stream stream = new System.IO.MemoryStream(data);
                        objS = new DataContractJsonSerializer(typeof(clsRole));
                        var result = objS.ReadObject(stream) as clsRole;
                        var Status = result.iStatus;
                        var Message = result.sMessage;

                        if (Status == "-1")
                        {
                            ObjCon.RollBack();
                            Arr[0] = "Something Went Wrong";
                            Arr[1] = "-1";
                        }
                        else
                        {
                            ObjCon.CommitTrans();
                        }
                        File.AppendAllText(sPath, "\n DTLMS \n DateTime    : " + System.DateTime.Now + Environment.NewLine+" Inputs : " + Environment.NewLine+" RoleId = " + objRole.RoleId + Environment.NewLine+" RoleName = " + objRole.RoleName + Environment.NewLine+" ModuleId = " + objRole.ModuleId + Environment.NewLine+" ******************************** " + Environment.NewLine+" Result : " + Environment.NewLine+"  Status = " + Status + Environment.NewLine+" Message = " + Message + Environment.NewLine+" ############################################################## "+Environment.NewLine);

                        return Arr;
                    }
                    else
                    {
                        ObjCon.CommitTrans();
                        return Arr;
                    }
                }

            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsRole", "SaveDetails");
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
                
                strQry = "Select RO_ID,RO_NAME,RO_DESIGNATION from TBLROLES ORDER BY RO_ID DESC";
                 dt = ObjCon.getDataTable(strQry);

               
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsRole", "LoadDetails");
                return dt;
            }
            finally
            {
                
            }
        }

        public object getRoleDetails(clsRole objRole)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
           
            try
            {
                oledbcommand.Parameters.AddWithValue("RoId", objRole.sRoleId);
                String strQry = "SELECT RO_ID,RO_NAME,RO_DESIGNATION FROM TBLROLES ";
                strQry += " WHERE RO_ID=:RoId";
                dtDetails = ObjCon.getDataTable(strQry, oledbcommand); 


                if (dtDetails.Rows.Count > 0)
                {
                    objRole.sRoleId = Convert.ToString(dtDetails.Rows[0]["RO_ID"].ToString());
                    objRole.sRoleName = Convert.ToString(dtDetails.Rows[0]["RO_NAME"].ToString());
                    objRole.sRoleDesig = Convert.ToString(dtDetails.Rows[0]["RO_DESIGNATION"].ToString());

                }
                return objRole;
            }


            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsRole", "getRoleDetails");
                return objRole;
            }
            finally
            {
                
            }

        }
    }
}
