using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.Internal
{
    public partial class InternalUserCreate : System.Web.UI.Page
    {
        string strFormCode = "InternalUserCreate";
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

                if (!IsPostBack)
                {
                    if (Request.QueryString["QryUserId"] != null && Request.QueryString["QryUserId"].ToString() != "")
                    {
                        txtuserID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryUserId"]));
                    }

                    Genaral.Load_Combo("SELECT UT_ID,UT_NAME FROM TBLUSERTYPE ORDER BY UT_ID", "-Select-", cmbUserType);


                    if (txtuserID.Text != "")
                    {
                        LoadUserDetails(txtuserID.Text);
                        chkCreateLogin_CheckedChanged(sender, e);
                        if (cmbUserType.SelectedValue == "1")
                        {
                            cmbUserType_SelectedIndexChanged(sender, e);
                            cmbSupervisor.SelectedValue = hdfSupervisor.Value;
                        }
                    }

                    txtDob.Attributes.Add("onblur", "return ValidateDate(" + txtDob.ClientID + ");");
                    txtDoj.Attributes.Add("onblur", "return ValidateDate(" + txtDoj.ClientID + ");");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }


        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {              
                if (txtFullName.Text.Trim().Length == 0)
                {
                    txtFullName.Focus();
                    ShowMsgBox("Enter Full Name");
                    return bValidate;
                }

                if (txtDob.Text.Trim().Length == 0)
                {
                    txtDob.Focus();
                    ShowMsgBox("Enter Date of birth");
                    return bValidate;
                }
                string sResult = Genaral.DateValidation(txtDob.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtDob.Text, "", true, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Date of Birth should be Less than Current Date");
                    return bValidate;
                }

                if (txtMobile.Text.Trim().Length == 0)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Enter Mobile Number");
                    return bValidate;
                }
                if (txtMobile.Text.Length < 10)
                {
                    ShowMsgBox("Enter Valid Mobile Number");
                    txtMobile.Focus();
                    return bValidate;
                }

                if (cmbUserType.SelectedIndex == 0)
                {
                    cmbUserType.Focus();
                    ShowMsgBox("Select User Type");
                    return bValidate;
                }
                if (cmbUserType.SelectedValue == "1")
                {
                    if (cmbSupervisor.SelectedIndex == 0)
                    {
                        cmbSupervisor.Focus();
                        ShowMsgBox("Select Supervisor");
                        return bValidate;
                    }
                }
                if (txtDoj.Text.Trim().Length == 0)
                {
                    txtDoj.Focus();
                    ShowMsgBox("Enter Date of Joining");
                    return bValidate;
                }
                sResult = Genaral.DateValidation(txtDoj.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    return bValidate;
                }

                sResult = Genaral.DateComparision(txtDoj.Text, txtDob.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Date of Joining should be Greater than Date of Birth");
                    return bValidate;
                }
                if (txtAddress.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Address");
                    txtAddress.Focus();
                    return bValidate;
                }

                if (txtPassword.Text != txtConPwd.Text)
                {
                    ShowMsgBox("New Password and Confirm password should  be same");
                    txtConPwd.Focus();
                    return false;
                }

                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateForm");
                return bValidate;
            }

        }

    
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
               

                if (ValidateForm() == true)
                {

                   
                    clsInternalUser objUser = new clsInternalUser();
                    string[] Arr = new string[2];
                    objUser.sUserId = txtuserID.Text;
                    objUser.sFullName = txtFullName.Text.Trim().Replace("'", "");
                   // objUser.sLoginName = txtLoginName.Text;
                    objUser.sPassword = txtPassword.Text;
                    objUser.sDob = txtDob.Text;
                    objUser.sDoj = txtDoj.Text;
                    objUser.sPhoneNo = txtMobile.Text;
                    objUser.sUserType = cmbUserType.SelectedValue;
                    objUser.sAddress = txtAddress.Text.Trim().Replace("'", "");
                    objUser.sCrby = objSession.UserId;
                    if (cmbSupervisor.SelectedIndex > 0)
                    {
                        objUser.sSupervisorId = cmbSupervisor.SelectedValue;
                    }

                    Arr = objUser.SaveUpdateInternalUserDetails(objUser);
                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        txtuserID.Text = objUser.sUserId;
                        cmdSave.Text = "Update";
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");

            }
        }


        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public void Reset()
        {
            try
            {
               
                txtFullName.Text = string.Empty;
               // txtLoginName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                cmbUserType.SelectedIndex = 0;
                txtMobile.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtuserID.Text = string.Empty;
                cmdSave.Text = "Save";
                txtDob.Text = string.Empty;
                txtDoj.Text = string.Empty;
                cmbSupervisor.Items.Clear();
                chkCreateLogin.Checked = false;
                txtPassword.Visible = false;
                txtConPwd.Visible = false;
                lblConPwd.Visible = false;
                lblPwd.Visible = false;


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

        protected void chkCreateLogin_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkCreateLogin.Checked == true)
                {
                    txtPassword.Visible = true;
                    txtConPwd.Visible = true;
                    lblConPwd.Visible = true;
                    lblPwd.Visible = true;
                }

                else
                {
                    txtPassword.Visible = false;
                    txtConPwd.Visible = false;
                    lblConPwd.Visible = false;
                    lblPwd.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "chkCreateLogin_CheckedChanged");
            }


        }




        public void LoadUserDetails(string strId)
        {
            try
            {
                clsInternalUser objUser = new clsInternalUser();

                objUser.sUserId = strId;

                objUser.GetUserDetails(objUser);

                txtuserID.Text = objUser.sUserId;
                txtFullName.Text = objUser.sFullName;
                //  txtLoginName.Text = objUser.sLoginName;
                cmbUserType.SelectedValue = objUser.sUserType;
                if (cmbUserType.SelectedValue == "1")
                {
                    cmbSupervisor.Visible = true;
                }
                txtMobile.Text = objUser.sPhoneNo;
                txtDob.Text = objUser.sDob;
                txtDoj.Text = objUser.sDoj;

                txtPassword.Attributes.Add("Value", Genaral.Decrypt(objUser.sPassword));
                txtConPwd.Attributes.Add("Value", Genaral.Decrypt(objUser.sPassword));
                hdfPwd.Value = objUser.sPassword;
                txtAddress.Text = objUser.sAddress;
                hdfSupervisor.Value = objUser.sSupervisorId;

                cmdSave.Text = "Update";
                if (hdfPwd.Value == "")
                {
                    txtPassword.Visible = false;
                    txtConPwd.Visible = false;
                    lblConPwd.Visible = false;
                    lblPwd.Visible = false;
                }
                else
                {
                    txtPassword.Visible = true;
                    txtConPwd.Visible = true;
                    lblConPwd.Visible = true;
                    lblPwd.Visible = true;
                    chkCreateLogin.Checked = true;

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " LoadUserDetails");

            }

        }

       
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("InternalUserView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbUserType.SelectedValue == "1")
                {
                    dvSupervisor.Style.Add("display", "block");
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 3", "--Select--", cmbSupervisor);
                }
                else
                {
                    dvSupervisor.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbUserType_SelectedIndexChanged");
            }
        }

       
    }
}