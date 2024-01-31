using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCDetails : System.Web.UI.Page
    {
        string strFormCode = "DTCDetails";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }

            objSession = (clsSession)Session["clsSession"];
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'PT'", "--Select--", cmbprojecttype);
                Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'LT'", "--Select--", cmbLoadtype);

                if (Request.QueryString["QryDtcId"] != null && Request.QueryString["QryDtcId"].ToString() != "")
                {
                    txtDTCId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDtcId"]));
                }

                if (txtDTCId.Text != "")
                {
                    LoadDtcDetails(txtDTCId.Text);                   
                }
            }
        }        
        
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {              
                cmbPlatformType.SelectedIndex = 0;
                cmdSave.Text = "Update";
                ddlArresters.SelectedIndex = 0;
                ddlBreakertype.SelectedIndex = 0;
                ddldtcmeters.SelectedIndex = 0;
                ddlgrounding.SelectedIndex = 0;
                ddlhtprotection.SelectedIndex = 0;
                ddlLTProtection.SelectedIndex = 0;
                txthtLine.Text = string.Empty;
                txtltLine.Text = string.Empty;
                cmbprojecttype.SelectedIndex = 0;
                cmbPlatformType.SelectedIndex = 0;
                txtLatitude.Text = string.Empty;
                txtlongitude.Text = string.Empty;
                cmbLoadtype.SelectedIndex = 0;
                txtDepreciation.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdReset_Click");
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsDTCCommision objDtcCommision = new clsDTCCommision();
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

                    string[] Arr = new string[3];
                    objDtcCommision.lDtcId = Convert.ToString(txtDTCId.Text);
                    objDtcCommision.sCrBy = objSession.UserId;
                    objDtcCommision.sHtlinelength = txthtLine.Text;
                    objDtcCommision.sLtlinelength = txtltLine.Text;
                    if (cmbPlatformType.SelectedIndex > 0)
                    {
                        objDtcCommision.sPlatformType = cmbPlatformType.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sPlatformType = "0";
                    }
                    if (ddlArresters.SelectedIndex > 0)
                    {
                        objDtcCommision.sArresters = ddlArresters.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sArresters = "0";
                    }
                    if (ddlgrounding.SelectedIndex > 0)
                    {
                        objDtcCommision.sGrounding = ddlgrounding.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sGrounding = "0";
                    }
                    if (ddlLTProtection.SelectedIndex > 0)
                    {
                        objDtcCommision.sLTProtect = ddlLTProtection.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sLTProtect = "0";
                    }
                    if (ddlhtprotection.SelectedIndex > 0)
                    {
                        objDtcCommision.sHTProtect = ddlhtprotection.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sHTProtect = "0";
                    }
                    if (ddldtcmeters.SelectedIndex > 0)
                    {
                        objDtcCommision.sDTCMeters = ddldtcmeters.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sDTCMeters = "0";
                    }
                    if (ddldtcmeters.SelectedIndex > 0)
                    {
                        objDtcCommision.sBreakertype = ddlBreakertype.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sBreakertype = "0";
                    }
                    if (cmbLoadtype.SelectedIndex > 0)
                    {
                        objDtcCommision.sLoadtype = cmbLoadtype.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sLoadtype = "0";
                    }
                    //if (cmbprojecttype.SelectedIndex > 0)
                    //{
                    //    objDtcCommision.sProjecttype = cmbprojecttype.SelectedValue;
                    //}
                    //else
                    //{
                    //    objDtcCommision.sProjecttype = "0";
                    //}  
                    objDtcCommision.sLongitude = txtlongitude.Text;
                    objDtcCommision.sLatitude = txtLatitude.Text;
                    objDtcCommision.sDepreciation = txtDepreciation.Text.Trim();
                
                    Arr = objDtcCommision.SaveUpdateDtcSpecification(objDtcCommision);

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtDTCId.Text = objDtcCommision.lDtcId;
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

        public void LoadDtcDetails(string strId)
        {
            try
            {
                clsDTCCommision objDtcCommision = new clsDTCCommision();
                objDtcCommision.lDtcId = Convert.ToString(strId);

                objDtcCommision.GetDTCDetails(objDtcCommision);

                txtDTCCode.Text = Convert.ToString(objDtcCommision.sDtcCode);
                txtDTCName.Text = objDtcCommision.sDtcName;
                txtDTCId.Text = Convert.ToString(objDtcCommision.lDtcId);               
                cmbPlatformType.SelectedValue = objDtcCommision.sPlatformType;              
                ddlBreakertype.SelectedValue = objDtcCommision.sBreakertype;
                ddlArresters.SelectedValue = objDtcCommision.sArresters;
                ddldtcmeters.SelectedValue = objDtcCommision.sDTCMeters;
                ddlgrounding.SelectedValue = objDtcCommision.sGrounding;
                ddlhtprotection.SelectedValue = objDtcCommision.sHTProtect;
                ddlLTProtection.SelectedValue = objDtcCommision.sLTProtect;
                txtltLine.Text = objDtcCommision.sLtlinelength;
                txthtLine.Text = objDtcCommision.sHtlinelength;
                cmbprojecttype.SelectedValue = objDtcCommision.sProjecttype;
                cmbLoadtype.SelectedValue = objDtcCommision.sLoadtype;
                txtLatitude.Text = objDtcCommision.sLatitude;
                txtlongitude.Text = objDtcCommision.sLongitude;
                txtDepreciation.Text = objDtcCommision.sDepreciation;

                if (Request.QueryString["Ref"] != null && Request.QueryString["Ref"].ToString() != "")
                {
                    cmdSave.Text = "Update";
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadDtcDetails");
            }
        }

        protected void cmbBack_Click(object sender, EventArgs e)
        {
            try
            {
                string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                Response.Redirect("DTCCommision.aspx?QryDtcId=" + strDtcId + "", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbBack_Click");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTCCommision";
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