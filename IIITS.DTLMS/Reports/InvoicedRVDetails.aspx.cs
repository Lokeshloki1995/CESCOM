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
    public partial class DTRFailureReport : System.Web.UI.Page
    {
        string strFormCode = "DTRFailureReport";
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
               
                txtToDate.Attributes.Add("readonly", "readonly");
                

                if (objSession.OfficeCode.Length > 1)
                {
                    stroffCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                //CalendarExtender1.EndDate = System.DateTime.Now.AddDays(0);
                //CalendarExtender2.EndDate = System.DateTime.Now.AddDays(0);
               

                if (!IsPostBack)
                {
                   // Genaral.Load_Combo("SELECT MD_NAME AS MD_NAME1,MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY TO_NUMBER(MD_NAME)", "--Select--", cmbCapacity1);

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);


                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                       
                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                       
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-'|| DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);

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
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_CODE ||'-'|| SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);

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
                        Genaral.Load_Combo("SELECT OM_CODE,OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);

                        if (stroffCode.Length >= 4)
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
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-'|| DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }

        }
        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_CODE ||'-'|| SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
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
                    Genaral.Load_Combo("SELECT OM_CODE,OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                }

                else
                {
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSubDiv_SelectedIndexChanged");
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
        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
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
                if (cmdReportType.SelectedIndex == 0)
                {
                    ShowMsgBox(" Please select Report Type");
                    cmdReportType.Focus();
                    return;
                }

            


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


                //if (cmbCapacity1.SelectedIndex > 0)
                //{
                //    objReport.sCapacity = cmbCapacity1.SelectedValue;
                //}

               

                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                objReport.sReportType = cmdReportType.SelectedValue;




                string sParam = "id=TCFailDetails&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&ReportType=" + objReport.sReportType;
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
        protected void Export_click(object sender, EventArgs e)
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
            if (cmdReportType.SelectedIndex == 0)
            {
                ShowMsgBox(" Please Select Report Type ");
                cmdReportType.Focus();
                return;
            }

            string strofficecode = GetOfficeID();
            string sReportName = string.Empty;

            objReport.sOfficeCode = strofficecode;
            objReport.sReportType = cmdReportType.SelectedValue;
            if (objReport.sReportType == "1")
            {
                sReportName = "Invoiced ";
            }
            else
            {
                sReportName = "RV ";
            }
            
                

             dt = objReport.InvoicedandRVDetails(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
                
                using (XLWorkbook wb = new XLWorkbook())
                {
                    int i = 1;
                    dt.Columns["CIRCLE"].SetOrdinal(i++);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].SetOrdinal(i++);
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].SetOrdinal(i++);
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].SetOrdinal(i++);
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns["DTC_CODE"].SetOrdinal(i++);
                    dt.Columns["DTC_CODE"].ColumnName = "DTC Code";
                    dt.Columns["TC_CODE"].SetOrdinal(i++);
                    dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                    dt.Columns["TC_CAPACITY"].SetOrdinal(i++);
                    dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["DF_GUARANTY_TYPE"].SetOrdinal(i++);
                    dt.Columns["DF_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["DF_DATE"].SetOrdinal(i++);
                    dt.Columns["DF_DATE"].ColumnName = "Fail Date";
                    dt.Columns["COMMISSION"].SetOrdinal(i++);
                    dt.Columns["COMMISSION"].ColumnName = "Commission Wo No and Date";
                    //dt.Columns["COMM_DATE"].SetOrdinal(9);
                    //dt.Columns["COMM_DATE"].ColumnName = "Com_Date";
                    dt.Columns["DECOMMISSION"].SetOrdinal(i++);
                    dt.Columns["DECOMMISSION"].ColumnName = "De Commission Wo No and Date";
                    //dt.Columns["DECOM_DATE"].SetOrdinal(11);
                    //dt.Columns["DECOM_DATE"].ColumnName = "Decom_Date";
                    dt.Columns["INV_NO"].SetOrdinal(i++);
                    dt.Columns["INV_NO"].ColumnName = "Inv_No and Date";
                    //dt.Columns["INV_DATE"].SetOrdinal(13);
                    //dt.Columns["INV_DATE"].ColumnName = "Inv_Date";
                    dt.Columns["INVOICED_DTR"].SetOrdinal(i++);
                    dt.Columns["INVOICED_DTR"].ColumnName = "Invoiced_Dtr";
                    dt.Columns["RV_NO"].SetOrdinal(i++);
                    dt.Columns["RV_NO"].ColumnName = "Rv_No and Date";
                    dt.Columns["TR_CR_DATE"].SetOrdinal(i++);
                    dt.Columns["TR_CR_DATE"].ColumnName = "CR Date";
                    //dt.Columns["RV_DATE"].SetOrdinal(16);
                    //dt.Columns["RV_DATE"].ColumnName = "Rv_Date";




                    dt.Columns.Remove("DF_LOC_CODE");
                    dt.Columns.Remove("DF_REPLACE_FLAG");
                    dt.Columns.Remove("FD_FEEDER_CODE");
                //    dt.Columns.Remove("FD_FEEDER_NAME");
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("TODAY");
                //    dt.Columns.Remove("Fail Date");
                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    wb.Worksheets.Add(dt, "DTRFAILURE");


                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                    //string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                    var rangeInvoiceDetails = wb.Worksheet(1).Range("J3:K3");
                    rangeInvoiceDetails.Merge().Style.Font.SetBold().Font.FontSize = 12;
                  //  rangeInvoiceDetails.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeInvoiceDetails.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeInvoiceDetails.SetValue("COMMISSIONED TRANSFORMERS DETAILS");

                    var rangeRVDetails = wb.Worksheet(1).Range("L3:M3");
                    rangeRVDetails.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    //rangeRVDetails.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeRVDetails.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeRVDetails.SetValue("RELEASED & RETURNED TRANSFORMER DETAILS ");


                    if (txtFromDate.Text != "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue(sReportName + " Details From  " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue(sReportName + " Details From  " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue(sReportName + " Details as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue(sReportName + " Details as on " + DateTime.Now);
                    }

                    //rangeReporthead.SetValue("List of DTC with Details ");

                //    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTRFAILURE " + DateTime.Now + ".xls";
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
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                cmbCircle.SelectedIndex = 0;
             
                cmbDiv.Items.Clear();
                cmbSubDiv.Items.Clear();
                cmbOMSection.Items.Clear();
               


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }

    }
}