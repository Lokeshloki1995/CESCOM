using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.IO;


namespace IIITS.DTLMS.MasterForms
{
    public partial class UserCreate : System.Web.UI.Page
    {
       
        string strFormCode = "UserCreate";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtOffCode.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {
                    if (Request.QueryString["QryUserId"] != null && Request.QueryString["QryUserId"].ToString() != "")
                    {
                        txtuserID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryUserId"]));
                    }

                    Genaral.Load_Combo("SELECT RO_ID,RO_NAME FROM TBLROLES ORDER BY RO_ID", "-Select-", cmbRole);
                    Genaral.Load_Combo("SELECT DM_DESGN_ID,DM_NAME FROM TBLDESIGNMAST ORDER BY DM_DESGN_ID", "-Select-", cmbDesignation);
                    

                    if (txtuserID.Text != "")
                    {
                        LoadUserDetails(txtuserID.Text);
                        btnSearch_Click(sender, e);
                    }

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Office Details&";
                    strQry += "Query= select * from(select NVL(OFF_CODE,-1) OFF_CODE,LTRIM(SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+1)) AS OFF_NAME FROM VIEW_ALL_OFFICES)  where  {0} like %{1}% order by OFF_NAME&";
                    strQry += "DBColName=OFF_CODE~OFF_NAME&";
                    strQry += "ColDisplayName=OFF_CODE~OFF_NAME&";
                    strQry = strQry.Replace("'", @"\'");
                    strQry = strQry.Replace("+", @"8TT8");

                    btnSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtOffCode.ClientID + "&btn=" + btnSearch.ClientID + "',520,520," + txtOffCode.ClientID + ")");

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
                if (txtLoginName.Text.Trim().Length == 0)
                {
                    txtLoginName.Focus();
                    ShowMsgBox("Enter Login Name");
                    return bValidate;
                }

                //if (txtOffCode.Text.Trim().Length == 0)
                //{
                //    txtOffCode.Focus();
                //    ShowMsgBox("Enter Office Code");
                //    return bValidate;
                //}
                if (txtOfficeName.Text.Trim().Length == 0)
                {
                    txtOffCode.Focus();
                    ShowMsgBox("Select Valid Office Code");
                    return bValidate;
                }
                if (txtEmailId.Text.Trim() == "")
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Enter Email Id");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
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
                if (txtPhone.Text.Trim().Length != 0)
                {
                    if (txtPhone.Text.Trim().Length < 10)
                    {
                        ShowMsgBox("Enter Valid Phone Number");
                        txtPhone.Focus();
                        return bValidate;
                    }
                    if ((txtPhone.Text.Length - txtPhone.Text.Replace("-", "").Length) >= 2)
                    {
                        txtPhone.Focus();
                        ShowMsgBox("You cannot use more than one hyphen (-)");
                        return bValidate;
                    }
                    if (txtPhone.Text.Contains(".") == true)
                    {
                        txtPhone.Focus();
                        ShowMsgBox("You cannot enter (.) in Phone Number");
                        return bValidate;
                    }

                }
              
                if (cmbRole.SelectedIndex == 0)
                {
                    cmbRole.Focus();
                    ShowMsgBox("Enter Role");
                    return bValidate;
                }
                if (cmbDesignation.SelectedIndex == 0)
                {
                    cmbDesignation.Focus();
                    ShowMsgBox("Enter Designation");
                    return bValidate;
                }
               
              
                
                if (txtAddress.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Address");
                    txtAddress.Focus();
                    return bValidate;
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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("UserView.aspx", false);
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }
       
        
        
        protected void cmdSave_Click(object sender, EventArgs e)
        {          
            try
            {

                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }
            
                if (ValidateForm() == true)
                {
                    clsUser objUser = new clsUser();
                    string[] Arr = new string[2];
                    Byte[] Buffer;

                    objUser.lSlNo = txtuserID.Text;
                    objUser.sOfficeCode = txtOffCode.Text;
                    objUser.sFullName = txtFullName.Text;
                    objUser.sLoginName = txtLoginName.Text;
                    if (!(cmdSave.Text == "Update"))
                    {
                        objUser.sPassword = txtPassword.Text;
                    }
                   

                    objUser.sRole = cmbRole.SelectedValue;
                    objUser.sEmail = txtEmailId.Text.ToLower();
                    objUser.sMobileNo = txtMobile.Text;
                    objUser.sPhoneNo = txtPhone.Text;
                    objUser.sAddress = txtAddress.Text;
                    objUser.sCrby = objSession.UserId;
                    objUser.sDesignation = cmbDesignation.SelectedValue;

                    if (fupSign.PostedFile.ContentLength != 0)
                    {
                        string filename = Path.GetFileName(fupSign.PostedFile.FileName);
                        string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        if (strExt.ToLower().Equals("jpg") || strExt.ToLower().Equals("jpeg") || strExt.ToLower().Equals("png") || strExt.ToLower().Equals("gif"))
                        {
                            Stream strm = fupSign.PostedFile.InputStream;
                            Buffer = new byte[strm.Length];
                            strm.Read(Buffer, 0, (int)strm.Length);
                            objUser.sSignImage = Buffer;
                        }
                        else
                        {
                            lblMessage.Text = "Invalid Image File";
                            return;
                        }
                    }
                    else
                    {
                        Stream strm = fupSign.PostedFile.InputStream;
                        Buffer = new byte[strm.Length];
                        strm.Read(Buffer, 0, (int)strm.Length);
                        objUser.sSignImage = Buffer;
                    }

                    Arr = objUser.SaveUpdateUserDetails(objUser);
                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox("Saved Successfully");
                        txtuserID.Text = Arr[0].ToString();
                        cmdSave.Text = "Update";
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "UserCreate");
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        txtuserID.Text = Convert.ToString(objUser.lSlNo);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "UserCreate");
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "4")
                    {
                        ShowMsgBox(Arr[0]);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "UserCreate");
                        }
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
           Reset();
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
                txtOffCode.Text = string.Empty;
                txtFullName.Text = string.Empty;
                txtLoginName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                cmbRole.SelectedIndex = 0;
                txtEmailId.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtuserID.Text = string.Empty ;
                cmdSave.Text = "Save";
                cmbDesignation.SelectedIndex = 0;
                txtPassword.Attributes.Add("Value", "");
                txtPassword.Attributes.Remove("readonly");
                txtLoginName.Enabled = true;
                txtOfficeName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

    
        
        public void LoadUserDetails(string strId)
        {
            try
            {
                clsUser objUser = new clsUser();

                objUser.lSlNo = strId;

                objUser.GetUserDetails(objUser);

                txtuserID.Text = objUser.lSlNo;
                txtOffCode.Text = objUser.sOfficeCode;
                txtFullName.Text = objUser.sFullName;
                txtLoginName.Text = objUser.sLoginName;
                //txtPassword.Text = objUser.sPassword;
                // txtPassword.Attributes.Add("Value", Genaral.Decrypt(objUser.sPassword));

                divPassword.Visible = false;
                cmbRole.SelectedValue = objUser.sRole;
                txtEmailId.Text = objUser.sEmail;
                txtMobile.Text = objUser.sMobileNo;
                txtPhone.Text = objUser.sPhoneNo;
                txtAddress.Text = objUser.sAddress;
                cmbDesignation.SelectedValue = objUser.sDesignation;
                cmdSave.Text = "Update";
                txtPassword.Attributes.Add("readonly", "readonly");

                txtLoginName.Enabled = false;

                
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " LoadUserDetails");

            }

        }

        public void RetainImageOnPostback()
        {
            try
            {
                string sDirectory = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sNamePlateFileName = string.Empty;

                if (fupSign.HasFile)
                {
                    sSSPlateFileName = Path.GetFileName(fupSign.PostedFile.FileName);
                    fupSign.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    txtSignImagePath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "RetainImageOnPostback");
            }
        }

        #region Access Rights

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "UserCreate";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsLogin objLogin = new clsLogin();

                if (txtOffCode.Text == "-1")
                {
                    txtOffCode.Text = "";
                }
                txtOfficeName.Text = objLogin.Getofficename(txtOffCode.Text);

                //if (txtOffCode.Text == "0")
                //{
                //    txtOffCode.Text = "";
                //}
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnSearch_Click");
            }
        }
 
    }
}