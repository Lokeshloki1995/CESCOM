using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.Reports
{
    public partial class MisRepairCentre : System.Web.UI.Page
    {
        String strFormName = "MisCommissionPending";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtMonth.Attributes.Add("readonly", "readonly");
                txtMonth1.Attributes.Add("readonly", "readonly");

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                string offCode = string.Empty;
                if (!IsPostBack)
                {

                    if (objSession.OfficeCode.Length > 2)
                    {
                        offCode = objSession.OfficeCode.Substring(0, 1);
                    }
                    else
                    {
                        offCode = objSession.OfficeCode;
                    }

                    Genaral.Load_Combo("select FY_ID,FY_YEARS from TBLFINANCIALYEAR order by FY_ID", "--Select--", cmdFinancialYear);
                    Genaral.Load_Combo("select FY_ID,FY_YEARS from TBLFINANCIALYEAR order by FY_ID", "--Select--", cmdFinancialYear1);
                    Genaral.Load_Combo("select FY_ID,FY_YEARS from TBLFINANCIALYEAR order by FY_ID", "--Select--", cmdFinancialYear3);

                    //Load the Circles if  length of office code is greater than 1 
                    if (offCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle1);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmdPerformanceCircle);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle3);
                        cmbCircle1.Items.FindByValue(offCode).Selected = true;
                        cmbCircle3.Items.FindByValue(offCode).Selected = true;
                        cmdPerformanceCircle.Items.FindByValue(offCode).Selected = true;
                        offCode = string.Empty;
                        offCode = objSession.OfficeCode;

                    }
                    if (offCode == null || offCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle1);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmdPerformanceCircle);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle3);

                    }

                    if (offCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmdPerformanceCircle.SelectedValue + "'", "--Select--", cmdPerformanceDiv);
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle3.SelectedValue + "'", "--Select--", cmbDivision3);

                        if (offCode.Length >= 2)
                        {
                            offCode = objSession.OfficeCode.Substring(0, 2);
                            cmbDiv1.Items.FindByValue(offCode).Selected = true;
                            cmbDivision3.Items.FindByValue(offCode).Selected = true;
                            cmdPerformanceDiv.Items.FindByValue(offCode).Selected = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "Page_Load");
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    string strParam = string.Empty;
                    string FinancialYear = string.Empty;

                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = GetOfficeID();


                    if (cmdFinancialYear.SelectedIndex > 0)
                    {
                        FinancialYear = cmdFinancialYear.SelectedItem.Text.Substring(0,4);
                    }



                        strParam = "id=MisRepairerStatus&Officecode=" + objReport.sOfficeCode + "&Month=" + txtMonth.Text.Trim() + "&FinancialYear=" + FinancialYear + "";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    BtnReset1_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ValidateForm");

            }

        }

        protected void BtnReset1_Click(object sender, EventArgs e)
        {
            cmbCircle1.SelectedIndex = 0;
            cmbDiv1.SelectedIndex = 0;
            txtMonth.Text = string.Empty;
            cmdFinancialYear.SelectedIndex = 0;
        }

        public bool ValidateForm()
        {
            bool status = true;
            try
            {
                if ((txtMonth.Text.Trim() == "") && (cmdFinancialYear.SelectedIndex == 0))
                {
                    ShowMsgBox("Please Enter Either Financial Year or  Month  ");
                    txtMonth.Focus();
                    status = false;
                }

                if ((txtMonth.Text.Trim() != "") && (cmdFinancialYear.SelectedIndex != 0))
                {
                    ShowMsgBox("Please Enter Either Financial Year or  Month  ");
                    txtMonth.Focus();
                    status = false;
                }
                
                return status;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ValidateForm");
                return false;
            }
        }

        protected void Export_clickFailureAbstract(object sender, EventArgs e)
        {

        }

        protected void cmbCircle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                }
                else
                {
                    cmbDiv1.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "cmbTaluk_SelectedIndexChanged");
            }
        }

        protected void cmdPerformanceCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmdPerformanceCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmdPerformanceCircle.SelectedValue + "'", "--Select--", cmdPerformanceDiv);
                }
                else
                {
                    cmdPerformanceDiv.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "cmdPerformanceCircle_SelectedIndexChanged");
            }
        }

        protected void cmdCircle3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle3.SelectedValue + "'", "--Select--", cmbDivision3);
                }
                else
                {
                    cmbDivision3.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "cmbCircle3_SelectedIndexChanged");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ShowMsgBox");
            }
        }


        public string GetOfficeID()
        {
            string strOfficeId = string.Empty;
            if ((cmbCircle1.SelectedIndex > 0) || (cmdPerformanceCircle.SelectedIndex > 0) || (cmbCircle3.SelectedIndex > 0))
            {
                if (cmbCircle1.SelectedIndex > 0)
                {
                    strOfficeId = cmbCircle1.SelectedValue.ToString();
                }
                if (cmdPerformanceCircle.SelectedIndex > 0)
                {
                    strOfficeId = cmdPerformanceCircle.SelectedValue.ToString();
                }
                if (cmbCircle3.SelectedIndex > 0)
                {
                    strOfficeId = cmbCircle3.SelectedValue.ToString();
                }
            }

            if ((cmbDiv1.SelectedIndex > 0) || (cmdPerformanceDiv.SelectedIndex > 0) || (cmbDivision3.SelectedIndex > 0))
            {
                if (cmbDiv1.SelectedIndex > 0)
                {
                    strOfficeId = cmbDiv1.SelectedValue.ToString();
                }
                if(cmdPerformanceDiv.SelectedIndex > 0)
                {
                    strOfficeId = cmdPerformanceDiv.SelectedValue.ToString();
                }
                if(cmbDivision3.SelectedIndex>0)
                {
                    strOfficeId = cmbDivision3.SelectedValue.ToString();
                }
            }
            return (strOfficeId);
        }

        #region Repairer Performance 

        protected void cmdPerformanceReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidatePerformanceForm())
                {
                    string strParam = string.Empty;
                    string FinancialYear = string.Empty;

                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = GetOfficeID();

                    if (cmdFinancialYear1.SelectedIndex > 0)
                    {
                        FinancialYear = cmdFinancialYear1.SelectedItem.Text.Substring(0, 4);
                    }

                    strParam = "id=MisRepairerPerformance&Officecode=" + objReport.sOfficeCode + "&Month=" + txtMonth1.Text.Trim() + "&FinancialYear=" + FinancialYear + "";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    BtnPerformanceReset1_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ValidateForm");

            }

        }

        protected void BtnPerformanceReset1_Click(object sender, EventArgs e)
        {
            try
            {
                cmdPerformanceCircle.SelectedIndex = 0;
                cmdPerformanceDiv.SelectedIndex = 0;
                cmdFinancialYear1.SelectedIndex = 0;
                txtMonth1.Text = string.Empty;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "BtnPerformanceReset1_Click");
                
            }
        }

        public bool ValidatePerformanceForm()
        {
            bool status = true;
            try
            {
                if ((txtMonth1.Text.Trim() == "") && (cmdFinancialYear1.SelectedIndex == 0))
                {
                    ShowMsgBox("Please Enter Either Financial Year or  Month  ");
                    txtMonth.Focus();
                    status = false;
                }

                if ((txtMonth1.Text.Trim() != "") && (cmdFinancialYear1.SelectedIndex != 0))
                {
                    ShowMsgBox("Please Enter Either Financial Year or  Month  ");
                    txtMonth.Focus();
                    status = false;
                }
                return status;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ValidatePerformanceForm");
                return false;
            }
        }
       
        public bool ValidateRepPending()
        {
            bool status = true;
            try
            {
                if ((txtMonth3.Text.Trim() != "") && (cmdFinancialYear3.SelectedIndex != 0))
                {
                    ShowMsgBox("Please Enter Either Financial Year or  Month  ");
                    txtMonth3.Focus();
                    status = false;
                }
                return status;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ValidateRepPending");
                return false;
            }
        }
        #endregion
        protected void cmdBtnRepPending_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRepPending())
                {
                    string strParam = string.Empty;
                    string FinancialYear = string.Empty;

                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = GetOfficeID();

                    if (cmdFinancialYear3.SelectedIndex > 0)
                    {
                        FinancialYear = cmdFinancialYear3.SelectedItem.Text.Substring(0, 4);
                    }

                    strParam = "id=MisRepairerPending&Officecode=" + objReport.sOfficeCode + "&Month=" + txtMonth3.Text.Trim() + "&FinancialYear=" + FinancialYear + "";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    BtnReset3_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ValidateForm");

            }

        }

        protected void BtnReset3_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCircle3.SelectedIndex = 0;
                cmbDivision3.SelectedIndex = 0;
                cmdFinancialYear3.SelectedIndex = 0;
                txtMonth3.Text = string.Empty;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "BtnPerformanceReset1_Click");

            }
        }
    }
}
