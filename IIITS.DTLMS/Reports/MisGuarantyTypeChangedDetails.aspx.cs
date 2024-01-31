using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class MisGuarantyTypeChangedDetails : System.Web.UI.Page
    {
        private clsSession objSession;

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
                    txtToDate.Attributes.Add("readonly", "readonly");
                    txtFromDate.Attributes.Add("readonly", "readonly");

                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);                }
            }

            catch (Exception ex)            {                lblMessage.Text = clsException.ErrorMsg();                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);            }
        }

        /// <summary>
        /// For Displaying Div name in DropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)        {            try            {                if (cmbCircle.SelectedIndex > 0)                {                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);                }                else                {                    cmbDiv.Items.Clear();                }            }            catch (Exception ex)            {                lblMessage.Text = clsException.ErrorMsg();                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);            }        }

        
        /// <summary>
        /// For Getting Office Code
        /// </summary>
        /// <returns></returns>
        private string GetOfficeID()        {            string strOfficeId = string.Empty;            if (cmbCircle.SelectedIndex > 0)            {                strOfficeId = cmbCircle.SelectedValue.ToString();            }            if (cmbDiv.SelectedIndex > 0)            {                strOfficeId = cmbDiv.SelectedValue.ToString();            }                        return (strOfficeId);        }

        /// <summary>
        /// For generating WRGP to AGP the Reoprt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmdGenerate_Click(object sender, EventArgs e)        {            try            {                string sResult = string.Empty;                string strOfficeCode = string.Empty;                string strParam = string.Empty;                clsReports objReport = new clsReports();
                if (ValidateForm() == true)
                {
                    objReport.sFromDate = txtFromDate.Text;                    objReport.sTodate = txtToDate.Text;                    string strofficecode = GetOfficeID();                    objReport.sOfficeCode = strofficecode;                    strParam = "id=GrntyTypeChange&OfficeCode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "";                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");                }            }            catch (Exception ex)            {                lblMessage.Text = clsException.ErrorMsg();                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);            }        }

        /// <summary>
        /// For text box Field made mandatory
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            bool bValidate = false;

            if (Convert.ToString(txtFromDate.Text).Length == 0)
            {
                ShowMsgBox("Please Select From Date");
                return bValidate;
            }
            if (Convert.ToString(txtToDate.Text).Length == 0)
            {
                ShowMsgBox("Please Select To Date");
                return bValidate;
            }
            
            bValidate = true;
            return bValidate;
        }


        /// <summary>
        /// For reseting the text field Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmdReset_Click(object sender, EventArgs e)
        {
            cmbCircle.SelectedIndex = 0;
            cmbDiv.Items.Clear();
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
        }

        /// <summary>
        /// For displaying Msg in pop up
        /// </summary>
        /// <param name="sMsg"></param>
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
                lblMessage.Text = clsException.ErrorMsg();                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}