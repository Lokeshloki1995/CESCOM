using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Approval
{
    public partial class ApprovalHistory : System.Web.UI.Page
    {
        string strFormCode = "ApprovalHistory";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {                    
                    if (Request.QueryString["RecordId"] != null && Request.QueryString["RecordId"].ToString() != "")
                    {
                        txtRecordId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RecordId"]));
                        txtBOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["BOId"]));
                        LoadApprovalHistory();
                        GetCurrentStatus();
                        GetDTCDetails();

                        EnableDisableControls();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public void LoadApprovalHistory()
        {
            try
            {
                clsAprovalHistory objHistory = new clsAprovalHistory();
                DataTable dt = new DataTable();

                dt = objHistory.LoadApprovalHistory(txtRecordId.Text,txtBOId.Text);
                grdApprovalHistory.DataSource = dt; 
                grdApprovalHistory.DataBind();
                ViewState["Approval"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadApprovalHistory");
            }
        }

        public void GetCurrentStatus()
        {
            try
            {
                clsAprovalHistory objHistory = new clsAprovalHistory();
                objHistory.sRecordId = txtRecordId.Text;
                objHistory.sBOId = txtBOId.Text;

                objHistory.GetStatusofApproval(objHistory);
                lblCurrentStatus.Text = objHistory.sStatus;
                lblWorkName.Text = objHistory.sDescription;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCurrentStatus");
            }
        }


        public void GetDTCDetails()
        {
            try
            {
                clsAprovalHistory objHistory = new clsAprovalHistory();

                objHistory.sRecordId = txtRecordId.Text;
                objHistory.sBOId = txtBOId.Text;

                objHistory.GetDTCDetails(objHistory);

                txtDTCCode.Text = objHistory.sDTCCode;
                txtDTCName.Text = objHistory.sDTCName;
                txtDTrCode.Text = objHistory.sDTRCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCurrentStatus");
            }
        }

        protected void grdApprovalHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdApprovalHistory.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Approval"];
                grdApprovalHistory.DataSource = dt;
                grdApprovalHistory.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdApprovalHistory_PageIndexChanging");
            }
        }

        public void EnableDisableControls()
        {
            try
            {
                if (txtBOId.Text == "23" || txtBOId.Text == "24" || txtBOId.Text == "30" || txtBOId.Text == "52" || txtBOId.Text == "32" || txtBOId.Text == "7" || txtBOId.Text == "15")
                {
                    dvDTCPanel.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "EnableDisableControls");
            }
        }
    }
}