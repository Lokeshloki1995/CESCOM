using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS
{
    public partial class InternalLogin : System.Web.UI.Page
    {
        string strFormCode = "Login";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                Form.DefaultButton = cmdLogin.UniqueID;
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            try
            {

                clsInternalLogin objLogin = new clsInternalLogin();
                clsSession objSession = new clsSession();

                objLogin.sLoginName = txtUsername.Text.Trim().ToUpper();
                objLogin.sPassword =  txtPassword.Text.Trim();

                objLogin.UserLogin(objLogin);

                if (objLogin.sMessage == null )
                {
                    
                    Session["FullName"] = objLogin.sFullName;
                   
                    objSession.UserId = objLogin.sUserId;
                    objSession.FullName = objLogin.sFullName;
                    objSession.UserType = objLogin.sUserType;
                 
                    Session["clsSession"] = objSession;

                    Response.Redirect("/Internal/InternalDashboard.aspx", false);
                    //HttpUtility.UrlEncode(General.Encrypt(e.Item.Cells(0).Text))
                     //General.Decrypt(HttpUtility.UrlDecode(Request.QueryString("EmpId")))
                }
                else
                {
                    lblMsg.Text = objLogin.sMessage;
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdLogin_Click");
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            string sShowMsg = string.Empty;
            try
            {
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdFSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsInternalLogin objLogin = new clsInternalLogin();
                objLogin.sMobileNo = txtEmail.Text;
                objLogin.ForgtPassword(objLogin);
                if (objLogin.sMessage == null)
                {
                    lblFMsg.Text = "Password has been sent to your Registered Mobile No";
                    dvForgtPwd.Visible = true;
                }
                else
                {
                    lblFMsg.Text = objLogin.sMessage;
                    dvForgtPwd.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdFSave_Click");
            }
        }

    }
}