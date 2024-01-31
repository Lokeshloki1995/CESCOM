using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsChangePwd
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsChangePwd";
        public string strOldPwd { get; set; }
        public string strNewPwd { get; set; }
        public string strConfirmPwd { get; set; }
        public string struserId { get; set; }
        public string usId { get; set; }
        public string iStatus { get; set; }
        public string iMessage { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        OleDbCommand oledbcommand;
        public String[] ChangePwd(clsChangePwd objChangepwd)
        {

            string[] Arr = new string[2];
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("UsId", objChangepwd.struserId);
                OleDbDataReader dr = ObjCon.Fetch("SELECT US_PWD FROM TBLUSER where US_ID=:UsId", oledbcommand);
                if (dr.Read())
                {
                    string oldpwd = dr.GetValue(dr.GetOrdinal("US_PWD")).ToString();

                    if (Genaral.CompareLogin(oldpwd, objChangepwd.strOldPwd))
                    {
                        if (Genaral.CompareLogin(oldpwd, objChangepwd.strNewPwd))
                        {

                            Arr[0] = "Old and New password Should not be same";
                            Arr[1] = "0";
                            return Arr;
                        }
                        else
                        {
                            try
                            {
                                string strQry = string.Empty;
                                strQry = "UPDATE TBLUSER SET US_PWD =:strNewPwd, US_CHPWD_ON=SYSDATE WHERE US_ID =:struserId";
                                OleDbCommand command = new OleDbCommand();
                                command.Parameters.AddWithValue("strNewPwd", Genaral.EncryptPassword(objChangepwd.strNewPwd));
                                command.Parameters.AddWithValue("struserId", objChangepwd.struserId);
                                ObjCon.Execute(strQry, command);

                                Arr[0] = "Password changed sucessfully";
                                Arr[1] = "0";
                                return Arr;

                            }
                            catch (Exception ex)
                            {
                                ObjCon.Con.Close();
                                Arr[0] = "Error:" + ex.Message;
                                Arr[1] = "4";

                                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ChangePwd");
                                return Arr;
                            }
                        }

                    }
                    else
                    {

                        Arr[0] = "Invalid Current Password";
                        Arr[1] = "4";
                        return Arr;
                    }
                }
                dr.Close();
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ChangePwd");
                return Arr;
            }
        }

        public bool GetStatus(string encryptedPass, string userId)
        {
            oledbcommand = new OleDbCommand();
            string query = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("UserId", userId);
                oledbcommand.Parameters.AddWithValue("UserPsw", encryptedPass);
                query = "SELECT * FROM TBLUSER_OLD_PASSWORD WHERE  UOP_US_ID = :UserId AND UOP_PWD = :UserPsw";
                dt = ObjCon.getDataTable(query, oledbcommand);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return true;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatus");
                return true;
            }

        }

        public String[] ChangePwdSinglesignin(clsChangePwd objChangepwd)
        {
            string[] Arr = new string[2];
            oledbcommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "UPDATE TBLUSER SET US_PWD =:strNewPwd, US_CHPWD_ON=SYSDATE WHERE US_ID =:struserId";
                OleDbCommand command = new OleDbCommand();
                command.Parameters.AddWithValue("strNewPwd", Genaral.EncryptPassword(objChangepwd.strNewPwd));
                command.Parameters.AddWithValue("struserId", objChangepwd.struserId);
                ObjCon.Execute(strQry, command);

                Arr[0] = "Password changed sucessfully";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                ObjCon.Con.Close();
                Arr[0] = "Error:" + ex.Message;
                Arr[1] = "4";

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ChangePwdSinglesignin");
                return Arr;
            }
        }
    }
}
