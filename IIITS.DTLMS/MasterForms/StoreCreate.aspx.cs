using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class StoreCreate : System.Web.UI.Page
    {
        string strFormCode = "StoreCreate";
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
                if (!IsPostBack)
                {
                    if (Request.QueryString["QryStoreId"] != null && Request.QueryString["QryStoreId"].ToString() != "")
                    {
                        txtStoreId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryStoreId"]));
                    }

                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "-Select-", cmbDivision);

                    if (txtStoreId.Text != "")
                    {
                        CheckAccessRights("4");
                        LoadStoreDetails(txtStoreId.Text);
                    }

                   
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

                if (txtStoreCode.Text.Trim().Length == 0)
                {
                    txtStoreCode.Focus();
                    ShowMsgBox("Enter Valid Store Code");
                    return bValidate;
                }

                if (txtStoreName.Text.Trim().Length == 0)
                {
                    txtStoreName.Focus();
                    ShowMsgBox("Enter valid Store Name");
                    return bValidate;
                }
                if (cmbDivision.SelectedIndex == 0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Select Division");
                    return bValidate;
                }
                if (txtStoreDescription.Text.Trim().Length == 0)
                {
                    txtStoreDescription.Focus();
                    ShowMsgBox("Enter valid Store Description");
                    return bValidate;
                }

                if (txtInchargeName.Text.Trim().Length == 0)
                {
                    txtInchargeName.Focus();
                    ShowMsgBox("Enter Store Incharge name");
                    return bValidate;
                }
                if (txtEmailId.Text.Trim().Length == 0)
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Enter valid Email Id");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                    return bValidate;
                }

                if (txtAddress.Text.Trim().Length == 0)
                {
                    txtAddress.Focus();
                    ShowMsgBox("Enter address");
                    return bValidate;
                }

                if (txtMobile.Text.Trim().Length == 0)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Enter Valid Mobile number");
                    return bValidate;
                }
                if (txtMobile.Text.Trim().Length < 10)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Enter Valid 10 digit Mobile number");
                    return bValidate;
                }

                //if (objSession.OfficeCode.Length > 2)
                //{
                //    objSession.OfficeCode = objSession.OfficeCode.Substring(0, 2);
                //}

                //if (cmbDivision.SelectedValue != objSession.OfficeCode)
                //{
                //    txtMobile.Focus();
                //    ShowMsgBox("Enter Valid 10 digit Mobile number");
                //    return bValidate;
                //}

                if (txtPhone.Text != "")
                {

                    if (txtPhone.Text.Length < 10)
                    {
                        ShowMsgBox("Enter valid  Phone no");
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


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsStore ObjStore = new clsStore();
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
                    string[] Arr = new string[2];
                    ObjStore.sSlNo = txtStoreId.Text;
                    ObjStore.sStoreCode = txtStoreCode.Text;
                    ObjStore.sStoreName = txtStoreName.Text;
                    ObjStore.sStoreDescription = txtStoreDescription.Text;
                    ObjStore.sOfficeCode = cmbDivision.SelectedValue;
                    ObjStore.sCrby = objSession.UserId;

                    ObjStore.sEmailId = txtEmailId.Text;

                    ObjStore.sMobile = txtMobile.Text;
                    ObjStore.sPhoneNo = txtPhone.Text;
                    ObjStore.sStoreIncharge = txtInchargeName.Text;
                    ObjStore.sAddress = txtAddress.Text;

                    Arr = ObjStore.SaveUpdateStoreDetails(ObjStore);
                    if (Arr[1].ToString() == "0")
                    {

                        ShowMsgBox("Saved Successfully");
                        txtStoreId.Text = Arr[0].ToString();
                        cmdSave.Text = "Update";

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        return;
                    }
                    if (Arr[1].ToString() == "4")
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
        


        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdReset_Click"); 
            }
        }

        public void Reset()
        {
            try
            {
                txtStoreId.Text = string.Empty;
                cmbDivision.SelectedIndex = 0;
                txtStoreCode.Text = string.Empty;
                txtStoreName.Text = string.Empty;
                txtStoreDescription.Text = string.Empty;

                txtEmailId.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtInchargeName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                cmdSave.Text = "Save";

                txtStoreCode.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset"); 
            }

        }

   
        public void LoadStoreDetails(string strId)
        {

            try
            {
                clsStore objStore = new clsStore();
                objStore.sSlNo = strId;
                objStore.GetStoreDetails(objStore);
                txtStoreId.Text = objStore.sSlNo;
                txtStoreCode.Text = objStore.sStoreCode;
                txtStoreName.Text = objStore.sStoreName;
                txtStoreDescription.Text = objStore.sStoreDescription;
                cmbDivision.SelectedValue = objStore.sOfficeCode;
                txtEmailId.Text = objStore.sEmailId;
                txtMobile.Text = objStore.sMobile;
                txtPhone.Text = objStore.sPhoneNo;
                txtInchargeName.Text = objStore.sStoreIncharge;
                txtAddress.Text = objStore.sAddress;

                txtStoreCode.Enabled = false;
                cmdSave.Text = "Update";
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " LoadStoreDetails");

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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("StoreView.aspx", false);
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "StoreCreate";
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion
    }
}