using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.BasicForms
{
    public partial class Circle : System.Web.UI.Page
    {
        string strFormCode = "Circle";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }

            objSession = (clsSession)Session["clsSession"];
            lblMessage.Text = string.Empty;

            Form.DefaultButton = cmdSave.UniqueID;
            if (!IsPostBack)
            {
                GenerateCircleCode();
                if (Request.QueryString["CircleId"] != null && Request.QueryString["CircleId"].ToString() != "")
                {

                    CheckAccessRights("4");
                    txtCrID.Text = Request.QueryString["CircleId"];
                    txtCrID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["CircleId"]));
                    GetCircleDetails(txtCrID.Text);
                }
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


                clsCircle objCircle = new clsCircle();
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {
                    //objCircle.sDepartmentId = Convert.ToString(txtDepId.Text);
                    objCircle.sCircleCode = txtCircleCode.Text.Trim().Replace("'", "");
                    objCircle.sCircleName = txtCircleName.Text.Trim().Replace("'", "");
                    objCircle.sName = txtFullName.Text.Trim().Replace("'", "");
                    objCircle.sMobileNo = txtMobile.Text.Trim().Replace("'", "");
                    objCircle.sPhone = txtPhone.Text.Trim().Replace("'", "");
                    objCircle.sEmail = txtEmailId.Text.Trim().Replace("'", "");
                    objCircle.sMaxid = txtCrID.Text.Trim().Replace("'", "");
                    if (objSession != null)
                    {
                        objCircle.sUserId = objSession.UserId;
                    }
                 

                    Arr = objCircle.SaveCircle(objCircle);
                    if (Arr[1].ToString() == "0")
                    {
                        txtCrID.Text = objCircle.sMaxid;
                        cmdSave.Text = "Update";
                        txtCircleCode.Enabled = false;
                        ShowMsgBox(Arr[0].ToString());

                    }
                    else if (Arr[1].ToString() == "1")
                    {

                        ShowMsgBox(Arr[0].ToString());
                    }
                    else
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");

            }
        }


        bool ValidateForm()
        {

            bool bValidate = false;

            if (txtCircleCode.Text.Trim().Length == 0)
            {
                txtCircleCode.Focus();
                ShowMsgBox("Please Enter the Circle Code");
                return bValidate;
            }

            if (txtCircleName.Text.Trim().Length == 0)
            {
                txtCircleCode.Focus();
                ShowMsgBox("Please Enter the Circle Name");
                return bValidate;
            }
            if (txtFullName.Text.Trim().Length == 0)
            {
                txtFullName.Focus();
                ShowMsgBox("Please Enter Name Of Head");
                return bValidate;
            }
            if (txtMobile.Text.Trim().Length == 0 || txtMobile.Text.Length < 10)
            {
                txtMobile.Focus();
                ShowMsgBox("Please Enter the Valid Mobile No ");
                return bValidate;
            }
            if (txtEmailId.Text.Trim().Length == 0)
            {
                txtEmailId.Focus();
                ShowMsgBox("Please Enter tEmail Id");
                return bValidate;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
            {
                ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                return bValidate;
            }

            if (txtPhone.Text != "")
            {
                if (txtPhone.Text.Length < 13)
                {
                    txtPhone.Focus();
                    ShowMsgBox("Please enter Valid Phone No");
                    return bValidate;
                }
                //if (txtPhone.Text.indexOf("-") > 1)
                //{
                //}
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }

        }


        public void Reset()
        {
            try
            {
                txtCircleCode.Text = "";
                txtCircleName.Text = "";
                txtEmailId.Text = "";
                txtFullName.Text = "";
                txtMobile.Text = "";
                txtPhone.Text = "";
                txtCircleCode.Enabled = true;
                cmdSave.Text = "Save";
                txtCrID.Text = "";
                txtCircleCode.Enabled = true;
                txtCircleCode.ReadOnly = false;

                GenerateCircleCode();
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }



        public void GetCircleDetails(string strCircleId)
        {
            try
            {
                clsCircle objCircle = new clsCircle();
                DataTable dtDetails = new DataTable();

                objCircle.sMaxid = Convert.ToString(strCircleId);
                objCircle.getCircleDetails(objCircle);
                txtCircleCode.Text = Convert.ToString(objCircle.sCircleCode);
                txtCircleName.Text = Convert.ToString(objCircle.sCircleName);
                txtFullName.Text = Convert.ToString(objCircle.sName);
                txtMobile.Text = Convert.ToString(objCircle.sMobileNo);
                txtPhone.Text = Convert.ToString(objCircle.sPhone);
                txtEmailId.Text = Convert.ToString(objCircle.sEmail);
                txtCrID.Text = Convert.ToString(objCircle.sMaxid);
                txtCircleCode.Enabled = false;
                //cmdSave.Text = "Update";
                cmdSave.Text = "Update";


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCircleDetails");
            }

        }

        public void GenerateCircleCode()
        {
            try
            {
                clsCircle objCircleCode = new clsCircle();

                txtCircleCode.Text = objCircleCode.GenerateCircleCode();
                txtCircleCode.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateCircleCode");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Circle";
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
    }
}