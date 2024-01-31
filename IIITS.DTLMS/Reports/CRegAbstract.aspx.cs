using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class CRegAbstract : System.Web.UI.Page
    {
        string strFormCode = "CRegAbstract";
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
                string stroffCode = string.Empty;
                if (objSession.OfficeCode.Length > 1)
                {
                    stroffCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                //CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                //CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);
                if (!IsPostBack)
                {

                    //Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 2);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 2)
                    {
                        
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 3);
                            cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                        if (stroffCode.Length == 4)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 4);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                        }
                    }
                  

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }




        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
            }
        }

        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                }
                else
                {
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                //if (txtFromDate.Text == "")
                //{
                //    ShowMsgBox("Please Enter From Date");
                //    txtFromDate.Focus();
                //    return;

                //}
                //if (txtToDate.Text == "")
                //{
                //    ShowMsgBox("Please Enter To Date");
                //    txtToDate.Focus();
                //    return;
                //}

                //string sResult = Genaral.DateValidation(txtFromDate.Text);
                //if (sResult != "")
                //{
                //    ShowMsgBox(sResult);
                //    return;
                //}

                //sResult = Genaral.DateValidation(txtToDate.Text);
                //if (sResult != "")
                //{
                //    ShowMsgBox(sResult);
                //    return;
                //}

                string sResult=string.Empty;
                if (txtFromDate.Text != "" )
                {
                     sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                  if ( txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }

                  if (txtToDate.Text != "" && txtFromDate.Text != "")
                  {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;

                    }

                }
                string strofficecode = GetOfficeID();
                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;

                //"&Circle=" + objReport.sCircle + "&Division=" + objReport.sDivision + "&SubDivision=" + objReport.sSubDivision+"&Section=" + objReport.sSection
                string sParam = "id=RegAbstract&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&offcode=" + strofficecode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdGenerate_Click");
            }


        }
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                cmbCircle.SelectedIndex = 0;
                cmbSubDiv.Items.Clear();
                cmbDiv.Items.Clear();
                cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        protected void Export_clickCregAbstract(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

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

            if (txtToDate.Text != "" && txtFromDate.Text != "")
            {
                sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
                    txtToDate.Focus();
                    return;

                }

            }

            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;
            if (txtFromDate.Text == "")
            {
                objReport.sFromDate = null;
                objReport.sTodate = null;
            }
            else
            {
                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
            }


            dt = objReport.PrintRegAbstact(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

               
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["EST_NO"].ColumnName = "Estimation No";
                    dt.Columns["EST_CRON"].ColumnName = "Estimate Date";
                    dt.Columns["WO_NO"].ColumnName = "Wo No";
                    dt.Columns["WO_NO_DECOM"].ColumnName = "Decommission Wo/no";
                    dt.Columns["WO_DATE"].ColumnName = "Wo Date";
                    dt.Columns["WO_DATE_DECOM"].ColumnName = "Decommission Date";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["DF_REASON"].ColumnName = "Failure Reason";
                    dt.Columns["IN_INV_NO"].ColumnName = "Invoice No ";
                    dt.Columns["IN_DATE"].ColumnName = "Invoice Date";
                    dt.Columns["REPLACE_CAPACITY"].ColumnName = "Replace Capacity";
                   
                    dt.Columns["SECTION"].SetOrdinal(3);
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("currentdate");
                    

                    wb.Worksheets.Add(dt, "CregAbstract");

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
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT as on " + DateTime.Now);
                    }

                   
                    //if (objReport.sFromDate != null)
                    //{
                    //    rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT   From" + objReport.sFromDate + "  To" + objReport.sTodate);
                    //}
                    //else
                    //{
                    //    rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT   ");

                    //}

                    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "CregAbstract " + DateTime.Now + ".xls";
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
    }
}