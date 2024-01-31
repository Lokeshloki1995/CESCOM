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
    public partial class Taluk : System.Web.UI.Page
    {
        string sformcode = "Taluk";
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
                if (!IsPostBack)
                {
                    LoadDropDown();
                    if (Request.QueryString["TalukId"] != null && Request.QueryString["TalukId"].ToString() != "")
                    {
                        CheckAccessRights("4");
                       txtTlkId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TalukId"]));
                       GetTalukDetails(txtTlkId.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "Page_Load");
            }
        }

        public void LoadDropDown()
        {
            try
            {
                Genaral.Load_Combo("SELECT DT_CODE,DT_CODE || '-' ||DT_NAME FROM TBLDIST ORDER BY DT_NAME ", "---Select---", cmbDistName);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "LoadDropDown");
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

                if (cmbDistName.SelectedValue == "")
                {
                    ShowMsgBox("Select District Name");
                    cmbDistName.Focus();
                    return;
                }

                if (txtTalukCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Taluk Code");
                    txtTalukCode.Focus();
                    return;
                }

                if (txtTalukName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Taluk Name");
                    txtTalukName.Focus();
                    return;
                }

                if (cmbDistName.SelectedValue.ToString() != txtTalukCode.Text.Substring(0, 1))
                {
                    ShowMsgBox("District Code and TaluK Code Does not Match");
                    txtTalukCode.Focus();
                    return;
                }

                clsTaluk objTlk = new clsTaluk();
                string[] Arr = new string[2];
                string sname = string.Empty;
                objTlk.sTalukId = txtTlkId.Text;
                objTlk.sDistrictName = cmbDistName.SelectedValue.ToString();
                objTlk.sTalukCode = txtTalukCode.Text;
                objTlk.sButtonName = cmdSave.Text;
                sname = txtTalukName.Text.Replace("'","''");
                objTlk.sTalukName = sname;

                if (objSession != null)
                {
                    objTlk.sUserID = objSession.UserId;

                }


                Arr = objTlk.SaveDetails(objTlk);
                if (Arr[1] == "2")
                {
                    txtTlkId.Text = Arr[0].ToString();
                    cmdSave.Text = "Update";
                    ShowMsgBox(Arr[2]);
                   
                }
                else
                {
                    ShowMsgBox(Arr[0]);
                  
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "cmdSave_Click");
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
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "ShowMsgBox");
            }
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                cmbDistName.SelectedIndex = 0;
                txtTalukCode.Text = string.Empty;
                txtTalukName.Text = string.Empty;
                cmbDistName.Enabled = true;
                txtTalukCode.Enabled = true;
                txtTalukCode.ReadOnly = false;
                cmdSave.Text = "Save";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "cmdClear_Click");
            }
        }



        public void GetTalukDetails(string strTalukId)
        {
            try
            {
                clsTaluk objTaluk = new clsTaluk();
                DataTable dtDetails = new DataTable();
                objTaluk.sTalukId = Convert.ToString(strTalukId);
                objTaluk.GetTlkDetails(objTaluk);
                txtTalukCode.Text = Convert.ToString(objTaluk.sTalukCode);
                txtTalukName.Text = Convert.ToString(objTaluk.sTalukName);
                cmbDistName.SelectedValue = objTaluk.sDistrictName;
                txtTlkId.Text = objTaluk.sTalukId;
              //  txtTalukCode.Enabled = false;
                cmbDistName.Enabled = false;
                cmdSave.Text = "Update";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GetDistrictDetails");
            }

        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Taluk";
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
                //lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        protected void cmbDistName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsTaluk objTaluk = new clsTaluk();
                objTaluk.sDistrictName = cmbDistName.SelectedValue;
                txtTalukCode.Text = objTaluk.GenerateTalukCode(objTaluk);
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "cmbDistName_SelectedIndexChanged");
            }
        }

  
      
    }
}