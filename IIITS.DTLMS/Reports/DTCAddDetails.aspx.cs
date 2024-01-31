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
    public partial class DTCAddDetails : System.Web.UI.Page
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
                //CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                //CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);
                if (!IsPostBack)
                {
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

                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='PT'", "--Select--", cmbSchemeType);
                    Genaral.Load_Combo("SELECT TO_NUMBER(MD_NAME)MD_NAME,TO_NUMBER(MD_NAME)MD_NAME1 FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_NAME", "--Select--", cmbCapacity);

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
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
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);

                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 3);
                            cmbSubDivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                        Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST,TBLFEEDEROFFCODE where FDO_FEEDER_ID=FD_FEEDER_ID and FDO_OFFICE_CODE like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbFeederName);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 4);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                        }

                    }

                    Genaral.Load_Combo("SELECT FT_ID,FT_NAME FROM TBLFDRTYPE", "--Select--", cmbFeederType);
                    Genaral.Load_Combo("SELECT TO_NUMBER(MD_NAME)MD_NAME,TO_NUMBER(MD_NAME)MD_NAME1 FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_NAME", "--Select--", cmbCapacity);
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
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
                else
                {

                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
            }
        }
        protected void cmbSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                    Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST,TBLFEEDEROFFCODE where FDO_FEEDER_ID=FD_FEEDER_ID and FDO_OFFICE_CODE like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbFeederName);
                }
                else
                {
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSubDivision_SelectedIndexChanged");
            }
        }
         protected void cmbDTCAddedthrough_click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDTCAddedthrough.SelectedValue == "1")
                {
                    cmbQcApproval.Visible = true;
                    lblQctype.Visible = true;
                }
                else
                {
                    cmbQcApproval.Visible = false;
                    lblQctype.Visible = false;
                }
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDTCAddedthrough_click");
            }
        }
         bool ValidateForm()
         {

             bool bValidate = false;

             if (cmbDTCAddedthrough.SelectedItem.Text == "--Select--")
             {
                 ShowMsgBox("Please Select DTC Added Type");
                 cmbDTCAddedthrough.Focus();
                 return bValidate;
             }
             else if (cmbDTCAddedthrough.SelectedValue == "1")
             {
                 if (cmbQcApproval.SelectedItem.Text == "--Select--")
                 {
                     ShowMsgBox("Please Select QC Type");
                     cmbQcApproval.Focus();
                     return bValidate;
                 }
             }
             bValidate = true;
             return bValidate;
         }

        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;

                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (ValidateForm() == true)
                {
                    if (cmbFeederName.SelectedIndex != 0)
                    {
                        objReport.sFeeder = cmbFeederName.SelectedValue;
                    }
                    if (cmbDTCAddedthrough.SelectedIndex == 1)
                    {
                        objReport.sDTCAddedThrough = cmbDTCAddedthrough.SelectedValue;
                        //objReport.sQCApprovaltype = 2  ;QC done and QC Pending  
                        if (cmbQcApproval.SelectedValue != "2")
                        {
                            objReport.sQCApprovaltype = cmbQcApproval.SelectedValue;
                        }
                        else
                        {
                            objReport.sQCApprovaltype = cmbQcApproval.SelectedValue;
                        }

                    }

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

                    objReport.sFromDate = txtFromDate.Text;
                    objReport.sTodate = txtToDate.Text;
                    // objReport.sType = rdbReportType.SelectedValue.ToString();
                    if (cmbFeederType.SelectedItem.Text != "--Select--")
                    {
                        objReport.sFeederType = cmbFeederType.SelectedValue;
                    }
                    if (cmbSchemeType.SelectedIndex != 0)
                    {
                        objReport.sSchemeType = cmbSchemeType.SelectedValue;
                    }
                    if (cmbCapacity.SelectedItem.Text != "--Select--")
                    {
                        objReport.sCapacity = cmbCapacity.SelectedValue;
                    }
                    //if (cmbCapacity.SelectedIndex > 15)
                    //{
                    //    objReport.sCapacity = cmbCapacity.SelectedValue;
                    //    objReport.sGreaterVal = "TRUE";
                    //}
                    //else objReport.sFeederType = "";
                    string strofficecode = GetOfficeID();
                    objReport.sOfficeCode = strofficecode;

                    strParam = "id=DTCAddDetails&OfficeCode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&FeederType=" + objReport.sFeederType + "&Capacity=" + objReport.sCapacity + "&FeederName=" + objReport.sFeeder + "&SchemaType=" + objReport.sSchemeType +"&DTCAddedThrough="+objReport.sDTCAddedThrough+"&QCApprovaltype="+objReport.sQCApprovaltype;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
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
            if (cmbSubDivision.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDivision.SelectedValue.ToString();
            }
            if (cmbOMSection.SelectedIndex > 0)
            {
                strOfficeId = cmbOMSection.SelectedValue.ToString();
            }
            return (strOfficeId);
        }


        protected void cmdReset_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            cmbFeederType.SelectedIndex = 0;
            cmbCapacity.SelectedIndex = 0;             
            cmbSchemeType.SelectedIndex = 0;
            cmbCircle.SelectedIndex = 0;
            cmbFeederName.Items.Clear();
            cmbDiv.Items.Clear();
            cmbSubDivision.Items.Clear();
            cmbOMSection.Items.Clear();
            if (cmbDTCAddedthrough.SelectedValue == "1")
            {
                cmbQcApproval.ClearSelection();
                cmbQcApproval.Visible = false;
                lblQctype.Visible = false;
            }
            cmbDTCAddedthrough.ClearSelection();


        }
        //protected void Export_click(object sender, EventArgs e)
        //{

        //    DataTable dt = new DataTable();
        //    clsReports objReport = new clsReports();
        //    string sResult = string.Empty;

        //    if (txtFromDate.Text != "" )
        //    {
        //         sResult = Genaral.DateValidation(txtFromDate.Text);
        //        if (sResult != "")
        //        {
        //            ShowMsgBox(sResult);
        //            txtFromDate.Focus();
        //            return;
        //        }
        //    }
        //    if ( txtToDate.Text != "")
        //    {

        //        sResult = Genaral.DateValidation(txtToDate.Text);
        //        if (sResult != "")
        //        {
        //            ShowMsgBox(sResult);
        //            txtToDate.Focus();
        //            return;
        //        }
        //    }

        //    if (txtFromDate.Text != "" && txtToDate.Text != "")
        //    {

        //        sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
        //        if (sResult == "2")
        //        {
        //            ShowMsgBox("To Date should be Greater than From Date");
        //            txtToDate.Focus();
        //            return;

        //        }
        //    }
        //    //objReport.sFromDate = txtFromDate.Text;
        //    //objReport.sTodate = txtToDate.Text;
        //   // objReport.sType = rdbReportType.SelectedValue.ToString();

        //    if (txtFromDate.Text != null && txtFromDate.Text != "")
        //    {
        //        objReport.sFromDate = txtFromDate.Text;
        //    }
        //    if (txtToDate.Text != null && txtToDate.Text != "")
        //    {
        //        objReport.sTodate = txtToDate.Text;
        //    }

        //    if (cmbFeederType.SelectedItem.Text != "--Select--")
        //    {
        //        objReport.sFeederType = cmbFeederType.SelectedValue;
        //    }
        //    if (cmbCapacity.SelectedItem.Text != "--Select--")
        //    {
        //        objReport.sCapacity = cmbCapacity.SelectedValue;
        //        objReport.sGreaterVal = "TRUE";
        //    }

        //    dt = objReport.DTCAddedReport(objReport);

        //    if (dt.Rows.Count > 0)
        //    {
        //        string[] arrAlpha = Genaral.getalpha();


        //        using (XLWorkbook wb = new XLWorkbook())
        //        {


        //            dt.Columns["COUNT"].ColumnName = "Count";
        //            dt.Columns["FD_DTC_CAPACITY"].ColumnName = "Tc Capacity";
        //            dt.Columns["SUBDIVOFF"].ColumnName = "SubDiv Off Code";

        //           // dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
        //            dt.Columns["FT_ID"].ColumnName = "Feeder ID";
        //            dt.Columns["FT_NAME"].ColumnName = "Feeder Type";
        //           // dt.Columns["CIRCLE"].ColumnName = "Circle";

        //            dt.Columns["CIRCLE"].SetOrdinal(0);
        //            dt.Columns["CIRCLE"].ColumnName = "Circle";
        //            dt.Columns["SUBDIVISION"].SetOrdinal(2);
        //            dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
        //            dt.Columns["COUNT"].SetOrdinal(3);
        //            dt.Columns["COUNT"].ColumnName = "Count";
        //            dt.Columns["DIVISION"].SetOrdinal(1);
        //            dt.Columns["DIVISION"].ColumnName = "Division";
        //            dt.Columns.Remove("FROMDATE");
        //            dt.Columns.Remove("TODATE");
        //            dt.Columns.Remove("TODAY");


        //            wb.Worksheets.Add(dt, "DTCAddDetails");

        //            wb.Worksheet(1).Row(1).InsertRowsAbove(3);
        //            string sMergeRange = arrAlpha[dt.Columns.Count - 1];

        //            var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
        //            rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
        //            rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
        //            rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //            rangehead.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

        //            var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
        //            rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
        //            rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
        //            rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //            if (txtFromDate.Text != "" && txtToDate.Text != "")
        //            {
        //                rangeReporthead.SetValue("DTC Added Report  From " + objReport.sFromDate + "  To " + objReport.sTodate);
        //            }
        //            if (txtFromDate.Text != "" && txtToDate.Text == "")
        //            {
        //                rangeReporthead.SetValue("DTC Added Report  From " + objReport.sFromDate + "  To " + DateTime.Now);
        //            }
        //            if (txtFromDate.Text == "" && txtToDate.Text != "")
        //            {
        //                rangeReporthead.SetValue("DTC Added Report  as on " + objReport.sTodate);
        //            }
        //            if (txtFromDate.Text == "" && txtToDate.Text == "")
        //            {
        //                rangeReporthead.SetValue("DTC Added Report as on " + DateTime.Now);
        //            }

        //            //if (objReport.sFromDate != null)
        //            //{
        //            //    rangeReporthead.SetValue("DTC Added Report As On " +DateTime.Now);
        //            //}
        //            //else
        //            //{
        //            //    rangeReporthead.SetValue("DTC Added Report ");

        //            //}

        //            wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.Charset = "";
        //            string FileName = "DTCAddDetails " + DateTime.Now + ".xls";
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

        //            using (MemoryStream MyMemoryStream = new MemoryStream())
        //            {
        //                wb.SaveAs(MyMemoryStream);
        //                MyMemoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ShowMsgBox("No Records Found");
        //    }




        //}
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

            string sResult = string.Empty;
            if (ValidateForm() == true)
            {

                if (cmbDTCAddedthrough.SelectedIndex == 1)
                {
                    objReport.sDTCAddedThrough = cmbDTCAddedthrough.SelectedValue;
                    if (cmbQcApproval.SelectedValue != "2")
                    {
                        objReport.sQCApprovaltype = cmbQcApproval.SelectedValue;
                    }
                    else
                    {
                        objReport.sQCApprovaltype = cmbQcApproval.SelectedValue;
                    }

                }

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

                if (txtFromDate.Text != null && txtFromDate.Text != "")
                {
                    objReport.sFromDate = txtFromDate.Text;

                    DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if (txtToDate.Text != null && txtToDate.Text != "")
                {
                    objReport.sTodate = txtToDate.Text;
                    DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                }
                if (cmbFeederType.SelectedItem.Text != "--Select--")
                {
                    objReport.sFeederType = cmbFeederType.SelectedValue;
                }
                if (cmbSchemeType.SelectedIndex != 0)
                {
                    objReport.sSchemeType = cmbSchemeType.SelectedValue;
                }
                if (cmbFeederName.SelectedIndex != 0)
                {
                    objReport.sFeeder = cmbFeederName.SelectedValue;
                }

                if (cmbCapacity.SelectedItem.Text != "--Select--")
                {
                    objReport.sCapacity = cmbCapacity.SelectedValue;
                }
                //if (cmbCapacity.SelectedIndex > 15)
                //{
                //    objReport.sCapacity = cmbCapacity.SelectedValue;
                //    objReport.sGreaterVal = "TRUE";
                //}
                //else objReport.sFeederType = "";
                string strofficecode = GetOfficeID();
                objReport.sOfficeCode = strofficecode;


                dt = objReport.DTCAddedReport(objReport);

                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();
                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                        dt.Columns["DT_NAME"].ColumnName = "DTC Name";
                        dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";
                        dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                        dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";

                        dt.Columns["FD_FEEDER_NAME"].ColumnName = "Feeder Name";
                        dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";
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

                        wb.Worksheets.Add(dt, "DTCAddedReport");

                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                        var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

                        var rangeReporthaed = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthaed.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthaed.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthaed.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeReporthaed.SetValue("List of DTC Added Details ");

                        // wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                        //wb.Worksheet(1).Cell(2, 1).Value = "List of DTC with Details ";
                        //wb.Worksheet(1).Cell(2, 1).Style.Font.FontColor = XLColor.AirForceBlue;
                        //wb.Worksheet(1).Cell(2, 1).Style.Font.FontSize = 14;

                        if (txtFromDate.Text != "" && txtToDate.Text != "")
                        {
                            rangeReporthaed.SetValue("List of DTC Added Details  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                        }
                        if (txtFromDate.Text != "" && txtToDate.Text == "")
                        {
                            rangeReporthaed.SetValue("List of DTC Added Details  From " + objReport.sFromDate + "  To " + DateTime.Now);
                        }
                        if (txtFromDate.Text == "" && txtToDate.Text != "")
                        {
                            rangeReporthaed.SetValue("List of DTC Added Details  as on " + objReport.sTodate);
                        }
                        if (txtFromDate.Text == "" && txtToDate.Text == "")
                        {
                            rangeReporthaed.SetValue("List of DTC Added Details as on " + DateTime.Now);
                        }

                        wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "DTCAddedReport " + DateTime.Now + ".xls";
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

        //protected void cmdReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        string strParam = string.Empty;

        //        clsReports objReport = new clsReports();



        //        if (ValidateForm() == true)
        //        {
        //            objReport.sReportType = cmbReportType.SelectedValue;

        //            if (txtFromMonth.Text != null && txtFromMonth.Text != "")
        //            {
        //                objReport.sFromDate = txtFromMonth.Text;
        //            }
        //            if (txtToMonth.Text != null && txtToMonth.Text != "")
        //            {
        //                objReport.sTodate = txtToMonth.Text;
        //            }

        //             strParam = "id=DTCAddedAbstractReport&ReportType=" + objReport.sReportType + "&FromMonth=" + objReport.sFromDate + "&ToMonth=" + objReport.sTodate;
        //            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmpReport_Click");
        //    }

        //}


        //bool ValidateForm()
        //{

        //    bool bValidate = false;

        //    if (cmbReportType.SelectedItem.Text == "--Select--")
        //    {
        //        ShowMsgBox("Please Select ReportType");
        //        return bValidate;
        //    }
        //    bValidate = true;
        //    return bValidate;
        //}
        
        //protected void cmdReset_Click1(object sender, EventArgs e)
        //{
        //    txtFromMonth.Text = "";
        //    txtToMonth.Text = "";
        //    cmbReportType.SelectedIndex = 0;
            
        //}
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