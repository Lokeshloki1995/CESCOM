using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace IIITS.DTLMS.Reports
{
    public partial class DTCFailureReport : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate2.Attributes.Add("readonly", "readonly");
                txtFromDate3.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate2.Attributes.Add("readonly", "readonly");
                txtToDate3.Attributes.Add("readonly", "readonly");
                txtFreqDTRFromDate.Attributes.Add("readonly", "readonly");
                txtFreqDTRToDate.Attributes.Add("readonly", "readonly");
                if (objSession.OfficeCode.Length > 1)
                {
                    stroffCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_ID", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME FROM TBLMASTERDATA " +
                        " WHERE MD_TYPE='FT' ORDER BY MD_ID", "--Select--", cmbFailureType);
                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME FROM TBLMASTERDATA " +
                        " WHERE MD_TYPE='FT' ORDER BY MD_ID", "--Select--", cmbFailureType1);
                    Genaral.Load_Combo("SELECT MD_NAME AS MD_NAME1,MD_NAME FROM TBLMASTERDATA " +
                        " WHERE MD_TYPE='C' ORDER BY TO_NUMBER(MD_NAME)", "--Select--", cmbCapacity1);
                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME FROM TBLMASTERDATA " +
                        " WHERE MD_TYPE='FT' ORDER BY MD_ID", "--Select--", cmdFreqDTRFailType);
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle2);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle3);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmdFreqDTRCircle);
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle2);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle3);
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME " +
                            " FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmdFreqDTRCircle);
                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        cmbCircle2.Items.FindByValue(stroffCode).Selected = true;
                        cmbCircle3.Items.FindByValue(stroffCode).Selected = true;
                        cmdFreqDTRCircle.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                            " WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                            " WHERE DIV_CICLE_CODE='" + cmbCircle2.SelectedValue + "'", "--Select--", cmbDiv2);
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                            " WHERE DIV_CICLE_CODE='" + cmbCircle3.SelectedValue + "'", "--Select--", cmbDivision3);
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                            " WHERE DIV_CICLE_CODE='" + cmdFreqDTRCircle.SelectedValue + "'", "--Select--", cmdFreqDTRDivision);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 2);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv2.Items.FindByValue(stroffCode).Selected = true;
                            cmbDivision3.Items.FindByValue(stroffCode).Selected = true;
                            cmdFreqDTRDivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                            " WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                            " WHERE SD_DIV_CODE='" + cmbDiv2.SelectedValue + "'", "--Select--", cmbSubDiv2);
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                            " WHERE SD_DIV_CODE='" + cmbDivision3.SelectedValue + "'", "--Select--", cmbSubDivision3);
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                            " WHERE SD_DIV_CODE='" + cmdFreqDTRDivision.SelectedValue + "'", "--Select--", cmdFreqDTRSubDivision);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 3);
                            cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv2.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDivision3.Items.FindByValue(stroffCode).Selected = true;
                            cmdFreqDTRSubDivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST " +
                            " WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST " +
                            " WHERE OM_SUBDIV_CODE='" + cmbSubDiv2.SelectedValue + "'", "--Select--", cmbSection);
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST " +
                            " WHERE OM_SUBDIV_CODE='" + cmbSubDivision3.SelectedValue + "'", "--Select--", cmbOMSection3);
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST " +
                            " WHERE OM_SUBDIV_CODE='" + cmdFreqDTRSubDivision.SelectedValue + "'", "--Select--", cmdFreqDTROMSection);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 4);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbOMSection3.Items.FindByValue(stroffCode).Selected = true;
                            cmdFreqDTROMSection.Items.FindByValue(stroffCode).Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                        "WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                        "WHERE DIV_CICLE_CODE='" + cmbCircle2.SelectedValue + "'", "--Select--", cmbDiv2);
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbDiv2.Items.Clear();
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                        " WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDiv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                        "WHERE SD_DIV_CODE='" + cmbDiv2.SelectedValue + "'", "--Select--", cmbSubDiv2);
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST " +
                        " WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                }
                else
                {
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSubDiv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST " +
                        "WHERE OM_SUBDIV_CODE='" + cmbSubDiv2.SelectedValue + "'", "--Select--", cmbSection);
                }
                else
                {
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
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
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                List<string> selectedFalureTypes = checkboxlist1.Items.Cast<ListItem>()
                   .Where(li => li.Selected)
                   .Select(li => li.Value)
                   .ToList();

                if (selectedFalureTypes.Count == 0)
                {
                    ShowMsgBox("Please select  Failure Type");
                    checkboxlist1.Focus();
                    return;
                }
                else
                {
                    objReport.sSelectedFailureType = string.Join(",", selectedFalureTypes.ToArray());
                }
                string sResult = string.Empty;
                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;
                    }
                }
                if (cmbReportType.SelectedValue == "1")
                {
                    ShowMsgBox("Please Select Report Type");
                    cmbReportType.Focus();
                    return;
                }
                objReport.sType = cmbStage.SelectedValue.ToString();
                if (cmbOMSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbOMSection.SelectedValue;
                }

                else if (cmbSubDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDiv.SelectedValue;
                }
                else if (cmbDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                else objReport.sOfficeCode = "";

                if (cmbMake.SelectedIndex > 0)
                    objReport.sMake = cmbMake.SelectedValue;

                if (cmbFailureType.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmbFailureType.SelectedValue;
                }
                else
                    objReport.sFailureType = "";

                if (cmbCapacity1.SelectedIndex > 0)
                {
                    objReport.sCapacity = cmbCapacity1.SelectedValue;
                }

                if (cmbGrntyType.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmbGrntyType.SelectedValue;
                }

                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                objReport.sReportType = cmbReportType.SelectedValue;

                string sParam = "id=TCFail&Type=" + objReport.sType + "&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&FailType=" + objReport.sFailureType + "&Make=" + objReport.sMake + "&ReportType=" + objReport.sReportType + "&Capacity=" + objReport.sCapacity + "&GrntyType=" + objReport.sGuranteeType + "&FailureType=" + objReport.sSelectedFailureType;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (txtFromDate2.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate2.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate2.Focus();
                        return;
                    }
                }
                if (txtToDate2.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate2.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate2.Focus();
                        return;
                    }
                }
                if (txtFromDate2.Text != "" && txtToDate2.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate2.Text, txtFromDate2.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate2.Focus();
                        return;
                    }
                }
                objReport.sType = cmbStage.SelectedValue.ToString();
                if (cmbSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSection.SelectedValue;
                }

                else if (cmbSubDiv2.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDiv2.SelectedValue;
                }
                else if (cmbDiv2.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv2.SelectedValue;
                }
                else if (cmbCircle2.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle2.SelectedValue;
                else objReport.sOfficeCode = "";

                objReport.sFromDate = txtFromDate2.Text;
                objReport.sTodate = txtToDate2.Text;

                string sParam = "id=WorkOderReg&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }

        protected void BtnWOReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate2.Text = "";
                txtToDate2.Text = "";
                cmbCircle2.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                cmbFailureType.SelectedIndex = 0;
                cmbDiv2.Items.Clear();
                cmbSubDiv2.Items.Clear();
                cmbSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                cmbCircle.SelectedIndex = 0;
                cmbReportType.SelectedIndex = 0;
                cmbDiv.Items.Clear();
                cmbSubDiv.Items.Clear();
                cmbOMSection.Items.Clear();
                cmbStage.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                cmbFailureType.SelectedIndex = 0;
                cmbCapacity1.SelectedIndex = 0;
                cmbGrntyType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetOfficeIDs()
        {
            string strOfficeId = string.Empty;
            if (cmbCircle2.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle2.SelectedValue.ToString();
            }
            if (cmbDiv2.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv2.SelectedValue.ToString();
            }
            if (cmbSubDiv2.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv2.SelectedValue.ToString();
            }
            if (cmbSection.SelectedIndex > 0)
            {
                strOfficeId = cmbSection.SelectedValue.ToString();
            }
            return (strOfficeId);
        }
        /// <summary>
        /// Get Office ID
        /// </summary>
        /// <returns></returns>
        public string GetOfficeID()
        {
            string strOfficeId = string.Empty;
            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle.SelectedValue.ToString();
            }

            if (cmbDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv.SelectedValue.ToString();
            }

            if (cmbSubDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv.SelectedValue.ToString();
            }
            if (cmbOMSection.SelectedIndex > 0)
            {
                strOfficeId = cmbOMSection.SelectedValue.ToString();
            }
            return (strOfficeId);
        }
        /// <summary>
        /// Export click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            try
            {
                List<string> selectedValues = checkboxlist1.Items.Cast<ListItem>()
                    .Where(li => li.Selected)
                    .Select(li => li.Value)
                    .ToList();
                if (selectedValues.Count == 0)
                {
                    ShowMsgBox("Please select Failure Type");
                    checkboxlist1.Focus();
                    return;
                }
                else
                {
                    objReport.sSelectedFailureType = string.Join(",", selectedValues);
                }
                string sResult = string.Empty;
                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;
                    }
                }
                if (checkboxlist1.Items.Count == 0)
                {
                    ShowMsgBox("Please Select Failure Type");
                    checkboxlist1.Focus();
                    return;
                }
                objReport.sType = cmbStage.SelectedValue.ToString();
                if (cmbMake.SelectedIndex > 0)
                    objReport.sMake = cmbMake.SelectedValue;
                if (cmbFailureType.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmbFailureType.SelectedValue;
                }
                else
                    objReport.sFailureType = "";
                if (cmbCapacity1.SelectedIndex > 0)
                {
                    objReport.sCapacity = cmbCapacity1.SelectedValue;
                }
                if (cmbGrntyType.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmbGrntyType.SelectedValue;
                }
                if (txtFromDate.Text != null && txtFromDate.Text != "")
                {
                    objReport.sFromDate = txtFromDate.Text;
                    DateTime DToDate = DateTime.ParseExact(
                        objReport.sFromDate, "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if (txtToDate.Text != null && txtToDate.Text != "")
                {
                    objReport.sTodate = txtToDate.Text;
                    DateTime DToDate = DateTime.ParseExact(
                        objReport.sTodate, "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                }
                string strofficecode = GetOfficeID();
                objReport.sOfficeCode = strofficecode;
                dt = objReport.TCFailReport(objReport);
                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        if (dt.Columns.Contains("TC_MANF_DATE"))
                        {
                            dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                        }
                        if (dt.Columns.Contains("DF_LOC_CODE"))
                        {
                            dt.Columns["DF_LOC_CODE"].ColumnName = "Failure Loc Code";
                        }
                        if (dt.Columns.Contains("FC_NAME"))
                        {
                            dt.Columns["FC_NAME"].ColumnName = "Feeder Category";
                        }
                        dt.Columns["CIRCLE"].SetOrdinal(0);
                        dt.Columns["CIRCLE"].ColumnName = "Circle";
                        dt.Columns["DIVISION"].SetOrdinal(1);
                        dt.Columns["DIVISION"].ColumnName = "Division";
                        dt.Columns["SUBDIVISION"].SetOrdinal(2);
                        dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                        dt.Columns["SECTION"].SetOrdinal(3);
                        dt.Columns["SECTION"].ColumnName = "Section";
                        dt.Columns["FD_FEEDER_CODE"].SetOrdinal(4);
                        dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";
                        if (dt.Columns.Contains("FD_FEEDER_NAME"))
                        {
                            dt.Columns["FD_FEEDER_NAME"].SetOrdinal(5);
                            dt.Columns["FD_FEEDER_NAME"].ColumnName = "Feeder";
                        }
                        if (dt.Columns.Contains("FEEDER_TYPE"))
                        {
                            dt.Columns["FEEDER_TYPE"].SetOrdinal(6);
                            dt.Columns["FEEDER_TYPE"].ColumnName = "Feeder Type";
                        }
                        dt.Columns["DT_CODE"].SetOrdinal(7);
                        dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                        dt.Columns["TC_CODE"].SetOrdinal(8);
                        dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                        dt.Columns["TC_CAPACITY"].SetOrdinal(9);
                        dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                        dt.Columns["TC_MAKE_ID"].SetOrdinal(10);
                        dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";
                        dt.Columns["DT_NAME"].SetOrdinal(11);
                        dt.Columns["DT_NAME"].ColumnName = "DTC Name";
                        dt.Columns["DF_DATE"].SetOrdinal(12);
                        dt.Columns["DF_DATE"].ColumnName = "Fail Date";
                        dt.Columns["FAILURE_TYPE"].SetOrdinal(13);
                        dt.Columns["FAILURE_TYPE"].ColumnName = "Failure Type";
                        dt.Columns["MD_NAME"].SetOrdinal(14);
                        dt.Columns["MD_NAME"].ColumnName = "Failure Reason";
                        if (dt.Columns.Contains("WO_NO"))
                        {
                            dt.Columns["WO_NO"].SetOrdinal(15);
                            dt.Columns["WO_NO"].ColumnName = "Wo No";
                        }
                        if (dt.Columns.Contains("COMMISSION"))
                        {
                            dt.Columns["COMMISSION"].SetOrdinal(16);
                            dt.Columns["COMMISSION"].ColumnName = "Com(Amount)";
                        }
                        if (dt.Columns.Contains("DECOMMISSION"))
                        {
                            dt.Columns["DECOMMISSION"].SetOrdinal(17);
                            dt.Columns["DECOMMISSION"].ColumnName = "Decom(Amount)";
                        }
                        if (dt.Columns.Contains("WO_DATE"))
                        {
                            dt.Columns["WO_DATE"].SetOrdinal(18);
                            dt.Columns["WO_DATE"].ColumnName = "Wo Date";
                        }
                        if (dt.Columns.Contains("TI_INDENT_NO"))
                        {
                            dt.Columns["TI_INDENT_NO"].SetOrdinal(19);
                            dt.Columns["TI_INDENT_NO"].ColumnName = "Indent No";
                        }
                        if (dt.Columns.Contains("TI_INDENT_DATE"))
                        {
                            dt.Columns["TI_INDENT_DATE"].SetOrdinal(20);
                            dt.Columns["TI_INDENT_DATE"].ColumnName = "Indent Date";
                        }
                        if (dt.Columns.Contains("IN_INV_NO"))
                        {
                            dt.Columns["IN_INV_NO"].SetOrdinal(21);
                            dt.Columns["IN_INV_NO"].ColumnName = "Invoice No";
                        }
                        if (dt.Columns.Contains("IN_DATE"))
                        {
                            dt.Columns["IN_DATE"].SetOrdinal(22);
                            dt.Columns["IN_DATE"].ColumnName = "Invoice Date";
                        }
                        // added by santhosh on 13-09-2023
                        if (dt.Columns.Contains("Invoiced_DTr"))
                        {
                            dt.Columns["Invoiced_DTr"].SetOrdinal(23);
                            dt.Columns["Invoiced_DTr"].ColumnName = "Invoiced DTr Code";
                        }
                        if (dt.Columns.Contains("TR_RI_NO"))
                        {
                            dt.Columns["TR_RI_NO"].SetOrdinal(24);
                            dt.Columns["TR_RI_NO"].ColumnName = "RI No";
                        }
                        if (dt.Columns.Contains("TR_RI_DATE"))
                        {
                            dt.Columns["TR_RI_DATE"].SetOrdinal(25);
                            dt.Columns["TR_RI_DATE"].ColumnName = "RI Date";
                        }
                        if (dt.Columns.Contains("TR_RV_NO"))
                        {
                            dt.Columns["TR_RV_NO"].SetOrdinal(26);
                            dt.Columns["TR_RV_NO"].ColumnName = "RV No";
                        }
                        if (dt.Columns.Contains("TR_RV_DATE"))
                        {
                            dt.Columns["TR_RV_DATE"].SetOrdinal(27);
                            dt.Columns["TR_RV_DATE"].ColumnName = "RV Date";
                        }
                        if (dt.Columns.Contains("DELETE_DATE"))
                        {
                            dt.Columns["DELETE_DATE"].SetOrdinal(27);
                            dt.Columns["DELETE_DATE"].ColumnName = "Deleted Date";
                        }
                        if (dt.Columns.Contains("STATUS"))
                        {
                            dt.Columns["STATUS"].SetOrdinal(dt.Columns.Count - 1);
                        }
                        dt.Columns.Remove("FROMDATE");
                        dt.Columns.Remove("TODATE");
                        dt.Columns.Remove("TODAY");
                        if (dt.Columns.Contains("DFT_OFFICE_CODE"))
                        {
                            dt.Columns.Remove("DFT_OFFICE_CODE");
                        }
                        if (dt.Columns.Contains("REPORT_CATEGORY"))
                        {
                            dt.Columns.Remove("REPORT_CATEGORY");
                        }
                        wb.Worksheets.Add(dt, "DTCFAILURE");
                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                        var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");
                        var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (txtFromDate.Text != "" && txtToDate.Text != "")
                        {
                            rangeReporthead.SetValue("List of DTC with Details From " + objReport.sFromDate + " To " + objReport.sTodate);
                        }
                        if (txtFromDate.Text != "" && txtToDate.Text == "")
                        {
                            rangeReporthead.SetValue("List of DTC with Details From " + objReport.sFromDate + " To " + DateTime.Now);
                        }
                        if (txtFromDate.Text == "" && txtToDate.Text != "")
                        {
                            rangeReporthead.SetValue("List of DTC with Details as on " + objReport.sTodate);
                        }
                        if (txtFromDate.Text == "" && txtToDate.Text == "")
                        {
                            rangeReporthead.SetValue("List of DTC with Details as on " + DateTime.Now);
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "DTCFAILURE " + DateTime.Now + ".xls";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ShowMsgBox("No Records Found");
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        protected void Export_clickworkorder(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string sResult = string.Empty;
            if (txtFromDate2.Text != "")
            {
                sResult = Genaral.DateValidation(txtFromDate2.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtFromDate2.Focus();
                    return;
                }
            }
            if (txtToDate2.Text != "")
            {
                sResult = Genaral.DateValidation(txtToDate2.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtToDate2.Focus();
                    return;
                }
            }
            if (txtFromDate2.Text != "" && txtToDate2.Text != "")
            {
                sResult = Genaral.DateComparision(txtToDate2.Text, txtFromDate2.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
                    txtToDate2.Focus();
                    return;

                }
            }
            if (txtFromDate2.Text != null && txtFromDate2.Text != "")
            {
                objReport.sFromDate = txtFromDate2.Text.ToString();
                DateTime DToDate = DateTime.ParseExact(
                    objReport.sFromDate, "d/M/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture
                    );
                objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
            }
            if (txtToDate2.Text != null && txtToDate2.Text != "")
            {
                objReport.sTodate = txtToDate2.Text.ToString();
                DateTime DToDate = DateTime.ParseExact(
                    objReport.sTodate, "d/M/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture
                    );
                objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
            }
            string strofficecodes = GetOfficeIDs();
            objReport.sOfficeCode = strofficecodes;
            dt = objReport.WoRegDetails(objReport);
            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["DF_DTC_CODE"].ColumnName = "DTC Code";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTR Code";
                    dt.Columns["EST_NO"].ColumnName = "Estimation No";
                    dt.Columns["EST_CRON"].ColumnName = "Estimate Date";
                    dt.Columns["RSM_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["FAILURE_TYPE"].ColumnName = "Failure Type";
                    dt.Columns["WO_NO"].ColumnName = "Wo No";
                    dt.Columns["WO_NO_DECOM"].ColumnName = "DeCommission No";
                    dt.Columns["WO_AMT"].ColumnName = "Wo Amount";
                    dt.Columns["WO_AMT_DECOM"].ColumnName = "Wo DeCommission Amount";
                    dt.Columns["WO_ACC_CODE"].ColumnName = "Commission Acc Code";
                    dt.Columns["WO_ACCCODE_DECOM"].ColumnName = "Decommission Acc Code";
                    dt.Columns["WO_DATE_DECOM"].ColumnName = "DeCommission Date";
                    dt.Columns["REPLACE_CAPACITY"].ColumnName = "Replace Capacity";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["DF_REASON"].ColumnName = "Reason For Failure";
                    dt.Columns["CIRCLE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].SetOrdinal(1);
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].SetOrdinal(2);
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].SetOrdinal(3);
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns["TODATE"].SetOrdinal(5);
                    dt.Columns["TODATE"].ColumnName = "TODATE";
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("today");
                    wb.Worksheets.Add(dt, "DTC WorkOrder Failure ");
                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");
                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (txtFromDate.Text != "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT as on " + DateTime.Now);
                    }
                    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTC WorkOrder Failure " + DateTime.Now + ".xls";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                ShowMsgBox("No Records Found");
            }
        }
        protected void btnFailreset_Click(object sender, EventArgs e)
        {
            txtFailMonth.Text = string.Empty;
        }
        protected void btnFailGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                if (txtFailMonth.Text == "")
                {
                    ShowMsgBox("Please Select The Month");
                    txtFailMonth.Focus();
                    return;
                }
                objReport.sMonth = txtFailMonth.Text;
                objReport.sReportType = cmbAbstract_ReportType.SelectedValue;
                string sParam = "id=FailureAbstract&Officecode=" + objSession.OfficeCode + "&Month=" + objReport.sMonth + "&ReportType=" + objReport.sReportType;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// Failure abstract Export Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_clickFailure(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            if (txtFailMonth.Text == "")
            {
                ShowMsgBox("Please Select The Month");
                txtFailMonth.Focus();
                return;
            }
            if (cmbAbstract_ReportType.SelectedValue == "0")
            {
                ShowMsgBox("Please Select Report Type ");
                cmbAbstract_ReportType.Focus();
                return;
            }
            objReport.sReportType = cmbAbstract_ReportType.SelectedValue;
            objReport.sMonth = txtFailMonth.Text;
            objReport.sOfficeCode = objSession.OfficeCode;
            dt = objReport.FailureAbstract(objReport);
            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["EST_PEND"].ColumnName = "Estimate Pending";
                    dt.Columns["WO_Pending"].ColumnName = "WorkOrder Pending";
                    dt.Columns["Indent_Pending"].ColumnName = "Indent Pending";
                    dt.Columns["Invoice_Pending"].ColumnName = "Invoice Pending";
                    dt.Columns["CR_RI_Pending"].ColumnName = "CR/RI Pending";
                    dt.Columns["DIV_NAME"].SetOrdinal(0);
                    dt.Columns["DIV_NAME"].ColumnName = "Division";
                    dt.Columns["TOTAL_DTC"].SetOrdinal(1);
                    dt.Columns["TOTAL_DTC"].ColumnName = "Total Dtc";
                    dt.Columns["Total_Fail_Pending"].SetOrdinal(2);
                    dt.Columns["Total_Fail_Pending"].ColumnName = "Total TC's Fail ";
                    dt.Columns["PENDING_FOR_REPLACEMENT"].SetOrdinal(3);
                    dt.Columns["PENDING_FOR_REPLACEMENT"].ColumnName = "Pending for replacement";
                    dt.Columns.Remove("CURRENT_MONTH");
                    dt.Columns.Remove("wo_office_code");
                    wb.Worksheets.Add(dt, "Failure Abstract");
                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");
                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (txtFailMonth.Text != "" || txtFailMonth.Text != null)
                    {
                        rangeReporthead.SetValue("Comparision Of DTLMS Details With Stastics During " + objReport.sMonth);
                    }
                    wb.Worksheet(1).Cell(3, 9).Value = DateTime.Now;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "Failure Abstract " + DateTime.Now + ".xls";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                ShowMsgBox("No Records Found");
            }
        }
        /// <summary>
        /// Circle3 Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle3.SelectedIndex > 0)
                {

                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" 
                        + cmbCircle3.SelectedValue + "'", "--Select--", cmbDivision3);
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                }
                else
                {
                    cmbDivision3.Items.Clear();
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// Division3 Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDivision3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                        " WHERE SD_DIV_CODE='" + cmbDivision3.SelectedValue + "'", "--Select--", cmbSubDivision3);
                    cmbOMSection3.Items.Clear();
                }
                else
                {
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSubDivision3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST" +
                        " WHERE OM_SUBDIV_CODE='" + cmbSubDivision3.SelectedValue + "'", "--Select--", cmbOMSection3);
                }
                else
                {
                    cmbOMSection3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerate3_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (txtFromDate3.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate3.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate3.Focus();
                        return;
                    }
                }
                if (txtToDate3.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate3.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate3.Focus();
                        return;
                    }
                }
                if (txtFromDate3.Text != "" && txtToDate3.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate3.Text, txtFromDate3.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate3.Focus();
                        return;
                    }
                }
                if (cmbguranteetype1.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmbguranteetype1.SelectedValue.Trim();
                }
                if (txtDTCCode.Text != "")
                {
                    objReport.sDtcCode = txtDTCCode.Text;
                }
                if (txtDTRCode.Text != "")
                {
                    objReport.sDtrCode = txtDTRCode.Text;
                }
                if (cmbOMSection3.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbOMSection3.SelectedValue;
                }
                else if (cmbSubDivision3.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDivision3.SelectedValue;
                }
                else if (cmbDivision3.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDivision3.SelectedValue;
                }
                else if (cmbCircle3.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle3.SelectedValue;
                else objReport.sOfficeCode = "";
                if (cmbFailureType1.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmbFailureType1.SelectedValue;
                }
                else
                    objReport.sFailureType = "";
                objReport.sFromDate = txtFromDate3.Text;
                objReport.sTodate = txtToDate3.Text;
                string sParam = "id=FrequentTCFail&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&FailType=" + objReport.sFailureType + "&GuranteeType=" + objReport.sGuranteeType + "&DTCCode=" + objReport.sDtcCode + "&DTRCode=" + objReport.sDtrCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// Reset Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReset3_Click(object sender, EventArgs e)
        {
            try
            {
                txtDTCCode.Text = "";
                txtDTRCode.Text = "";
                txtFromDate3.Text = "";
                txtToDate3.Text = "";
                cmbguranteetype1.SelectedIndex = 0;
                cmbCircle3.SelectedIndex = 0;
                cmbReportType.SelectedIndex = 0;
                cmbDivision3.Items.Clear();
                cmbSubDivision3.Items.Clear();
                cmbOMSection3.Items.Clear();
                cmbStage.SelectedIndex = 0;
                cmbFailureType1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// Export click3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click3(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string sResult = string.Empty;
            if (txtFromDate3.Text != "")
            {
                sResult = Genaral.DateValidation(txtFromDate3.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtFromDate3.Focus();
                    return;
                }
            }
            if (txtToDate3.Text != "")
            {
                sResult = Genaral.DateValidation(txtToDate3.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtToDate3.Focus();
                    return;
                }
            }
            if (txtFromDate3.Text != "" && txtToDate3.Text != "")
            {
                sResult = Genaral.DateComparision(txtToDate3.Text, txtFromDate3.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
                    txtToDate3.Focus();
                    return;
                }
            }
            if (cmbFailureType1.SelectedIndex > 0)
            {
                objReport.sFailureType = cmbFailureType1.SelectedValue;
            }
            else
                objReport.sFailureType = null;
            if (txtFromDate3.Text != null && txtFromDate3.Text != "")
            {
                objReport.sFromDate = txtFromDate3.Text;
            }
            if (txtToDate3.Text != null && txtToDate3.Text != "")
            {
                objReport.sTodate = txtToDate3.Text;
            }
            if (cmbOMSection3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbOMSection3.SelectedValue;
            }
            else if (cmbSubDivision3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSubDivision3.SelectedValue;
            }
            else if (cmbDivision3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbDivision3.SelectedValue;
            }
            else if (cmbCircle3.SelectedIndex > 0)
                objReport.sOfficeCode = cmbCircle3.SelectedValue;
            else objReport.sOfficeCode = "";

            if (cmbguranteetype1.SelectedIndex > 0)
            {
                objReport.sGuranteeType = cmbguranteetype1.SelectedValue.Trim();
            }
            if (txtDTCCode.Text != "")
            {
                objReport.sDtcCode = txtDTCCode.Text;
            }
            if (txtDTRCode.Text != "")
            {
                objReport.sDtrCode = txtDTRCode.Text;
            }
            if (Convert.ToString(txtFromDate3.Text ?? "").Length > 0)
            {
                objReport.sFromDate = txtFromDate3.Text.ToString();
                DateTime DToDate = DateTime.ParseExact(
                    objReport.sFromDate, "d/M/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture
                    );
                objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
            }
            if (txtToDate3.Text.ToString() != null && txtToDate3.Text.ToString() != "")
            {
                objReport.sTodate = txtToDate3.Text.ToString();
                DateTime DToDate = DateTime.ParseExact(
                    objReport.sTodate, "d/M/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture
                    );
                objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
            }
            dt = objReport.FrequentTcFail(objReport);
            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                    dt.Columns["DF_DATE"].ColumnName = "Fail Date";
                    dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                    dt.Columns["DF_LOC_CODE"].ColumnName = "Failure Loc Code";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["MD_NAME"].ColumnName = "Failure Type";
                    dt.Columns["TC_SLNO"].ColumnName = "Tc Serial No";
                    dt.Columns["DF_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";
                    dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";
                    dt.Columns["DT_NAME"].ColumnName = "DTC Name";
                    dt.Columns["CIRCLE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].SetOrdinal(1);
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].SetOrdinal(2);
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].SetOrdinal(3);
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("TODAY");
                    wb.Worksheets.Add(dt, "FREQUENTDTCFAILURE");
                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");
                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (txtFromDate3.Text != "" && txtToDate3.Text != "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details " +
                            " From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate3.Text != "" && txtToDate3.Text == "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details " +
                            " From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate3.Text == "" && txtToDate3.Text != "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details  as on " + objReport.sTodate);
                    }
                    if (txtFromDate3.Text == "" && txtToDate3.Text == "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details as on " + DateTime.Now);
                    }
                    wb.Worksheet(1).Cell(3, 9).Value = DateTime.Now;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "FREQUENTDTCFAILURE " + DateTime.Now + ".xls";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                ShowMsgBox("No Records Found");
            }
        }
        #region FrequentFailingDTR's
        /// <summary>
        /// FreqDTRCircle Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFreqDTRCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmdFreqDTRCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION" +
                        " WHERE DIV_CICLE_CODE='" + cmdFreqDTRCircle.SelectedValue + "'", "--Select--", cmdFreqDTRDivision);
                    cmdFreqDTRSubDivision.Items.Clear();
                    cmdFreqDTROMSection.Items.Clear();
                }
                else
                {
                    cmdFreqDTRDivision.Items.Clear();
                    cmdFreqDTRSubDivision.Items.Clear();
                    cmdFreqDTROMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// FreqDTR Division Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFreqDTRDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmdFreqDTRDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST" +
                        " WHERE SD_DIV_CODE='" + cmdFreqDTRDivision.SelectedValue + "'", "--Select--", cmdFreqDTRSubDivision);
                    cmdFreqDTROMSection.Items.Clear();
                }
                else
                {
                    cmdFreqDTRSubDivision.Items.Clear();
                    cmdFreqDTROMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// FreqDTRSubDivision Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFreqDTRSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmdFreqDTRSubDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST" +
                        " WHERE OM_SUBDIV_CODE='" + cmdFreqDTRSubDivision.SelectedValue + "'", "--Select--", cmdFreqDTROMSection);
                }
                else
                {
                    cmdFreqDTROMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// FreqDTR Generate Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFreqDTRGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (txtFreqDTRFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFreqDTRFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFreqDTRFromDate.Focus();
                        return;
                    }
                }
                if (txtFreqDTRToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFreqDTRToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFreqDTRToDate.Focus();
                        return;
                    }
                }
                if (txtFreqDTRFromDate.Text != "" && txtFreqDTRToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtFreqDTRToDate.Text, txtFreqDTRFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtFreqDTRToDate.Focus();
                        return;
                    }
                }
                if (cmdFreqDTRGuaranteeType.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmdFreqDTRGuaranteeType.SelectedValue.Trim();
                }
                if (txtFreqDTRDTCCode.Text != "")
                {
                    objReport.sDtcCode = txtFreqDTRDTCCode.Text;
                }
                if (txtFreqDTRDTRCode.Text != "")
                {
                    objReport.sDtrCode = txtFreqDTRDTRCode.Text;
                }
                if (cmdFreqDTROMSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmdFreqDTROMSection.SelectedValue;
                }
                else if (cmdFreqDTRSubDivision.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmdFreqDTRSubDivision.SelectedValue;
                }
                else if (cmdFreqDTRDivision.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmdFreqDTRDivision.SelectedValue;
                }
                else if (cmdFreqDTRCircle.SelectedIndex > 0)
                    objReport.sOfficeCode = cmdFreqDTRCircle.SelectedValue;
                else objReport.sOfficeCode = "";

                if (cmdFreqDTRFailType.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmdFreqDTRFailType.SelectedValue;
                }
                else
                    objReport.sFailureType = "";
                objReport.sFromDate = txtFreqDTRFromDate.Text;
                objReport.sTodate = txtFreqDTRToDate.Text;
                string sParam = "id=FrequentDTRFail&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&FailType=" + objReport.sFailureType + "&GuranteeType=" + objReport.sGuranteeType + "&DTCCode=" + objReport.sDtcCode + "&DTRCode=" + objReport.sDtrCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// Frequent Failing DTR Reset Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFreqDTRReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFreqDTRDTCCode.Text = "";
                txtFreqDTRDTRCode.Text = "";
                txtFreqDTRFromDate.Text = "";
                txtFreqDTRToDate.Text = "";
                cmdFreqDTRGuaranteeType.SelectedIndex = 0;
                cmdFreqDTRCircle.SelectedIndex = 0;
                cmdFreqDTRFailType.SelectedIndex = 0;
                cmdFreqDTRDivision.Items.Clear();
                cmdFreqDTRSubDivision.Items.Clear();
                cmdFreqDTROMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        /// <summary>
        /// Frequent Failing DTR Export Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFreqDTRExcel_click3(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (txtFreqDTRFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFreqDTRFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFreqDTRFromDate.Focus();
                        return;
                    }
                }
                if (txtFreqDTRToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFreqDTRToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFreqDTRToDate.Focus();
                        return;
                    }
                }
                if (txtFreqDTRFromDate.Text != "" && txtFreqDTRToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(
                        txtFreqDTRToDate.Text,
                        txtFreqDTRFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtFreqDTRToDate.Focus();
                        return;
                    }
                }
                if (cmdFreqDTRFailType.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmdFreqDTRFailType.SelectedValue;
                }
                else
                    objReport.sFailureType = null;
                if (txtFreqDTRFromDate.Text != null && txtFreqDTRFromDate.Text != "")
                {
                    objReport.sFromDate = txtFreqDTRFromDate.Text;
                }
                if (txtFreqDTRToDate.Text != null && txtFreqDTRToDate.Text != "")
                {
                    objReport.sTodate = txtFreqDTRToDate.Text;
                }
                if (cmdFreqDTROMSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmdFreqDTROMSection.SelectedValue;
                }
                else if (cmdFreqDTRSubDivision.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmdFreqDTRSubDivision.SelectedValue;
                }
                else if (cmdFreqDTRDivision.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmdFreqDTRDivision.SelectedValue;
                }
                else if (cmdFreqDTRCircle.SelectedIndex > 0)
                    objReport.sOfficeCode = cmdFreqDTRCircle.SelectedValue;
                else objReport.sOfficeCode = "";
                if (cmdFreqDTRGuaranteeType.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmdFreqDTRGuaranteeType.SelectedValue.Trim();
                }
                if (txtFreqDTRDTCCode.Text != "")
                {
                    objReport.sDtcCode = txtFreqDTRDTCCode.Text;
                }
                if (txtFreqDTRDTRCode.Text != "")
                {
                    objReport.sDtrCode = txtFreqDTRDTRCode.Text;
                }
                if (txtFreqDTRFromDate.Text.ToString() != null && txtFreqDTRFromDate.Text.ToString() != "")
                {
                    objReport.sFromDate = txtFreqDTRFromDate.Text.ToString();
                    DateTime DToDate = DateTime.ParseExact(
                        objReport.sFromDate, "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if (txtFreqDTRToDate.Text.ToString() != null && txtFreqDTRToDate.Text.ToString() != "")
                {
                    objReport.sTodate = txtFreqDTRToDate.Text.ToString();
                    DateTime DToDate = DateTime.ParseExact(
                        objReport.sTodate, "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                    objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                }
                dt = objReport.FrequentFailDTR(objReport);
                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                        dt.Columns["DF_DATE"].ColumnName = "Fail Date";
                        dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                        dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                        dt.Columns["MD_NAME"].ColumnName = "Failure Type";
                        dt.Columns["TC_SLNO"].ColumnName = "Tc Serial No";
                        dt.Columns["DF_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                        dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";
                        dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";
                        dt.Columns.Remove("FROMDATE");
                        dt.Columns.Remove("TODATE");
                        dt.Columns.Remove("TODAY");
                        dt.Columns.Remove("DT_NAME");
                        dt.Columns.Remove("DF_ID");
                        dt.Columns.Remove("CIRCLE");
                        dt.Columns.Remove("DF_LOC_CODE");
                        wb.Worksheets.Add(dt, "FREQUENTDTRFAILURE");
                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                        var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");
                        var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (txtFromDate3.Text != "" && txtToDate3.Text != "")
                        {
                            rangeReporthead.SetValue("List of Frequent Failed DTR with Details" +
                                " From " + objReport.sFromDate + "  To " + objReport.sTodate);
                        }
                        if (txtFromDate3.Text != "" && txtToDate3.Text == "")
                        {
                            rangeReporthead.SetValue("List of Frequent Failed DTR with Details " +
                                " From " + objReport.sFromDate + "  To " + DateTime.Now);
                        }
                        if (txtFromDate3.Text == "" && txtToDate3.Text != "")
                        {
                            rangeReporthead.SetValue("List of Frequent Failed DTR with Details  as on " + objReport.sTodate);
                        }
                        if (txtFromDate3.Text == "" && txtToDate3.Text == "")
                        {
                            rangeReporthead.SetValue("List of Frequent Failed DTR with Details as on " + DateTime.Now);
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "FREQUENTDTRFAILURE " + DateTime.Now + ".xls";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ShowMsgBox("No Records Found");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        #endregion
    }
}