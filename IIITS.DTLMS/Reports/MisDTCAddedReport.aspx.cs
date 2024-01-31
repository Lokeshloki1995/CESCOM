using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using ClosedXML.Excel;
using System.Data;
namespace IIITS.DTLMS.Reports
{
    public partial class MisDTCAddedReport : System.Web.UI.Page
    {
        string strFormCode = "DTCAddDetails";
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
               
                if (!IsPostBack)
                {
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

     

     
     

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {

                string strParam = string.Empty;

                clsReports objReport = new clsReports();



                if (ValidateForm() == true)
                {
                    objReport.sReportType = cmbReportType.SelectedValue;

                    if (txtFromMonth.Text != null && txtFromMonth.Text != "")
                    {
                        objReport.sFromDate = txtFromMonth.Text;
                    }
                    if (txtToMonth.Text != null && txtToMonth.Text != "")
                    {
                        objReport.sTodate = txtToMonth.Text;
                    }

                    strParam = "id=DTCAddedAbstractReport&ReportType=" + objReport.sReportType + "&FromMonth=" + objReport.sFromDate + "&ToMonth=" + objReport.sTodate;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmpReport_Click");
            }

        }


        bool ValidateForm()
        {

            bool bValidate = false;

            if (cmbReportType.SelectedItem.Text == "--Select--")
            {
                ShowMsgBox("Please Select ReportType");
                return bValidate;
            }
            bValidate = true;
            return bValidate;
        }

        protected void cmdReset_Click1(object sender, EventArgs e)
        {
            txtFromMonth.Text = "";
            txtToMonth.Text = "";
            cmbReportType.SelectedIndex = 0;

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
    }
}