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
    public partial class District : System.Web.UI.Page
    {
        string sformcode = "District";
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
                    GenerateDistrictCode();
                    if (Request.QueryString["DistrictId"] != null && Request.QueryString["DistrictId"].ToString() != "")
                    {
                        CheckAccessRights("4");
                       txtDistId.Text= Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DistrictId"]));
                       GetDistrictDetails(txtDistId.Text);
                    }
                }
            }
            catch (Exception ex)
            {
               
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "Page_Load");
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
                if (txtDistrictCode.Text.Trim().Length == 0 )
                {
                    ShowMsgBox("Please Enter District Code");
                    txtDistrictCode.Focus();
                    return;
                }
                if (txtDistrictName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter District Name");
                    txtDistrictName.Focus();
                    return;
                }

                clsDistrict objDis = new clsDistrict();
                string[] Arr = new string[2];
                objDis.sDistId = txtDistId.Text;
                objDis.sDistrictCode = txtDistrictCode.Text;
                objDis.sButtonname = cmdSave.Text;
                String name = txtDistrictName.Text.Replace("'","''");
                
                objDis.sDistrictName = name;

                Arr = objDis.SaveDetails(objDis);
                if (Arr[1].ToString() == "0")
                {
                    txtDistId.Text = objDis.sDistId;
                    cmdSave.Text = "Update";
                    ShowMsgBox(Arr[0].ToString());
                }
                else
                {
                    ShowMsgBox(Arr[0].ToString());
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
                txtDistrictCode.Text = string.Empty ;
                txtDistrictName.Text = string.Empty;
                txtDistId.Text = string.Empty;
                txtDistrictCode.Enabled = true;
                cmdSave.Text = "Save";
                txtDistrictCode.ReadOnly = false;
                GenerateDistrictCode();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "cmdClear_Click");
            }
        }


        public void GetDistrictDetails(string strDistrictId)
        {
            try
            {
                clsDistrict objDistrict = new clsDistrict();
                DataTable dtDetails = new DataTable();
                objDistrict.sDistId = Convert.ToString(strDistrictId);
              
                objDistrict.GetDistDetails(objDistrict);
                txtDistrictCode.Text = Convert.ToString(objDistrict.sDistrictCode);
                txtDistrictName.Text = Convert.ToString(objDistrict.sDistrictName);
                txtDistId.Text = objDistrict.sDistId;
                txtDistrictCode.Enabled = false;
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

                objApproval.sFormName = "District";
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
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        public void GenerateDistrictCode()
        {
            try
            {
                clsDistrict objDistCode = new clsDistrict();

                txtDistrictCode.Text = objDistCode.GenerateDistrictCode();
                txtDistrictCode.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GenerateIndentNo");
            }
        }


    }
}