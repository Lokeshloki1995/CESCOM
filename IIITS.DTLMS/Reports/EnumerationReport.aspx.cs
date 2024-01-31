using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Reports
{
    public partial class EnumerationReport : System.Web.UI.Page
    {
        string strFormCode = "EnumerationReport";
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
                if (!IsPostBack)
                {

                    txtFromDate.Attributes.Add("onblur", "return ValidateDate(" + txtFromDate.ClientID + ");");
                    txtToDate.Attributes.Add("onblur", "return ValidateDate(" + txtToDate.ClientID + ");");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objEnumReport = new clsReports();

                if (txtFromDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter From Date");
                    return;

                }
                if (txtToDate.Text.Trim() == "")
                {
                    txtToDate.Text = txtFromDate.Text;
                }

                objEnumReport.sFromDate = txtFromDate.Text;
                objEnumReport.sTodate = txtToDate.Text;
                objEnumReport.sType = cmbType.SelectedValue;
                //objEnumReport.EnumerationReport(objEnumReport);

                if (cmbType.SelectedValue == "1")
                {
                    string strParam = string.Empty;
                    strParam = "id=EnumReport&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
                if (cmbType.SelectedValue == "2")
                {
                    string strParam = string.Empty;
                    strParam = "id=LocOperator&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
                if (cmbType.SelectedValue == "3")
                {
                    string strParam = string.Empty;
                    strParam = "id=DetailedField&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
                if (cmbType.SelectedValue == "4")
                {
                    string strParam = string.Empty;
                    strParam = "id=DetailedStore&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
              
            } 
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmpReport_Click");
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

    }
}