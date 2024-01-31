using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;


namespace IIITS.DTLMS.Internal
{
    public partial class InternalChangePwd : System.Web.UI.Page
    {
        string strFormCode = "InternalChangePwd";       
        clsSession objSession;       
  
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsInternalChangePwd objChangepwd = new clsInternalChangePwd();
               
                objChangepwd.strOldPwd = txtOldPwd.Text.Replace("'", "");
                objChangepwd.strNewPwd = txtNewPwd.Text.Replace("'", "");
                objChangepwd.strConfirmPwd = txtConfirmPwd.Text.Replace("'", "");

                objChangepwd.struserId = objSession.UserId;
                if (ValidateForm() == true)
                {
                    if (txtNewPwd.Text == txtConfirmPwd.Text)
                    {

                        Arr = objChangepwd.ChangePwd(objChangepwd);
                        if (Arr[1].ToString() == "0")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }           
                    }
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnsubmit_Click");
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (txtOldPwd.Text.Trim().Length == 0)
            {
                txtOldPwd.Focus();
                ShowMsgBox("Enter Old Password");
                return false;
            }


            if (txtNewPwd.Text.Trim().Length == 0)
            {
                txtNewPwd.Focus();
                ShowMsgBox("Enter New Password");
                return false;
            }
            if (txtConfirmPwd.Text.Trim().Length == 0)
            {
                txtConfirmPwd.Focus();
                ShowMsgBox("Enter Confirm Password");
                return false;
            }
            if (txtOldPwd.Text == txtNewPwd.Text)
            {
                ShowMsgBox("New Password and Old password should not be same");
                txtConfirmPwd.Focus();
                return false;
            }
            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                ShowMsgBox("New Password and Confirm password should  be same");
                txtConfirmPwd.Focus();
                return false;
            }
           

            bValidate = true;
            return bValidate;
        }

    }
}