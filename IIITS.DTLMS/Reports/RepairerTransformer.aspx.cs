using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using ClosedXML.Excel;
namespace IIITS.DTLMS.Reports
{
    public partial class RepairerTransformer : System.Web.UI.Page
    {
        string strFormCode = "RepairerTransformer";
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
                //objSession = (clsSession)Session["sOfficeCode"];
                string stroffCode=string.Empty;
                if(objSession.OfficeCode.Length>1)
                {
                   stroffCode=objSession.OfficeCode.Substring(0,1);
                }
                else
                {
                    stroffCode=objSession.OfficeCode;
                }

                //CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                //CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);
                if (!IsPostBack)
                {
                    if (objSession.OfficeCode != "")
                    {
                        Genaral.Load_Combo("SELECT  TR_ID, TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE ) AND TR_OFFICECODE LIKE'" + objSession.OfficeCode.Substring(0, 2) + "%' ORDER BY TR_NAME", "--Select--", cmbRepairerName);
                    }
                    Genaral.Load_Combo("SELECT  TR_ID, TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE ) AND TR_OFFICECODE LIKE'" + objSession.OfficeCode + "%' ORDER BY TR_NAME", "--Select--", cmbRepairerName);
                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY to_number(MD_NAME)", "--Select--", cmbCapacity);
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    }

                    if (stroffCode.Length>=1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 2);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
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
                }
                else
                {
                    cmbDiv.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
            }
        }

        //protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
        //        }
             
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
        //    }
        //}

        //protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSubDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
        //        }
          
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
        //    }
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string strRepriername = string.Empty;
                if (cmbReportType.SelectedItem.Text.Trim() == "--Select--")
                {
                    ShowMsgBox("Please Select Report Type");
                    cmbReportType.Focus();
                    return;
                }
                if (cmbRepairerName.SelectedItem.Text.Trim() == "--Select--")
                {
                    strRepriername = string.Empty;

                }
                else
                {
                    strRepriername = cmbRepairerName.SelectedValue.ToString();
                }
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
                
                clsReports objReport = new clsReports();
                string strofficecode = GetOfficeID();
                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                string strReportType = cmbRepairerName.SelectedItem.Text;

                if (cmbReportType.SelectedItem.ToString() == "Pending Analysis Report")
                {

                    string sParam = "id=Pending Analysis Report&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&offcode=" + strofficecode + "&StrReprierName=" + strRepriername;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                if (cmbReportType.SelectedItem.ToString() == "Delivered Analysis Report")
                {

                    string sParam = "id=Delivered Analysis Report&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&offcode=" + strofficecode + "&StrReprierName=" + strRepriername;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
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

            //if (cmbSubDiv.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbSubDiv.SelectedValue.ToString();
            //}
            //if (cmbOMSection.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbOMSection.SelectedValue.ToString();
            //}

            return (strOfficeId);
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCircle.SelectedIndex = 0;
                cmbDiv.SelectedIndex = 0;
                //cmbSubDiv.SelectedIndex = 0;
                //cmbOMSection.SelectedIndex = 0;
                cmbRepairerName.SelectedIndex = 0;
                cmbReportType.SelectedIndex = 0;
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }

        }
        protected void Export_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtcompletedRepairCount = new DataTable();
                clsReports objReport = new clsReports();

                string strRepriername = string.Empty;
                if (cmbReportType.SelectedItem.Text.Trim() == "--Select--")
                {
                    ShowMsgBox("Please Select Report Type");
                    cmbReportType.Focus();
                    return;
                }
                if (cmbRepairerName.SelectedItem.Text.Trim() == "--Select--")
                {
                    strRepriername = null;

                }
                else
                {
                    strRepriername = cmbRepairerName.SelectedValue.ToString();
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

                string strofficecode = GetOfficeID();

                if (txtFromDate.Text != string.Empty)
                {
                    objReport.sFromDate = txtFromDate.Text;
                    DateTime DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sFromDate = DFromDate.ToString("yyyyMMdd");
                }
                if (txtToDate.Text != string.Empty)
                {
                    objReport.sTodate = txtToDate.Text;
                    DateTime DToDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sTodate = DToDate.ToString("yyyyMMdd");
                }
                if (strofficecode != null && strofficecode != "")
                {
                    objReport.sOfficeCode = strofficecode;
                }
                //objReport.sOfficeCode = strofficecode;

                objReport.sRepriername = strRepriername;

                string strReportType = cmbRepairerName.SelectedItem.Text;

                if (cmbReportType.SelectedItem.ToString() == "Pending Analysis Report")
                {
                    dt = objReport.PENDINGWTREPARIER(objReport);
                    // dt1 = objReport.TransformerWiseDetails(objReport);

                }
                if (cmbReportType.SelectedItem.ToString() == "Delivered Analysis Report")
                {
                    dt = objReport.ReperierCompleted(objReport);
                    //dt1 = objReport.TransformerWiseDetailsCompleted(objReport);

                }


                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();


                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt.Columns["CIRLCE"].ColumnName = "Circle";
                        dt.Columns["CIRCLE_CODE"].ColumnName = " Circle Off Code";
                        dt.Columns["DURATION"].ColumnName = "Duration";
                        dt.Columns["DIV_CODE"].ColumnName = "Div Off code";
                        dt.Columns["DIVISION_NAME"].ColumnName = "Division";
                        dt.Columns["UPTO_25"].ColumnName = "UPTO_25 Capacity";
                        dt.Columns.Remove("FROMDATE");
                        dt.Columns.Remove("TODATE");
                        dt.Columns.Remove("currentdate");


                        wb.Worksheets.Add(dt, "Repairer Performence");
                        //wb.Worksheet(1).Cell(1,5).InsertTable(dtcompletedRepairCount);

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
                        if (cmbReportType.SelectedItem.ToString() == "Pending Analysis Report")
                        {
                            rangeReporthead.SetValue(" Repairer Performance Pending Report ");
                        }
                        else
                        {
                            rangeReporthead.SetValue(" Repairer Perforamance Delivered  Report");
                        }



                        wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "Repairer Performence " + DateTime.Now + ".xls";
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
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_click");
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT  TR_ID, TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE ) AND TR_OFFICECODE LIKE'" + cmbDiv.SelectedValue + "%' ORDER BY TR_NAME", "--Select--", cmbRepairerName);
                }
                else
                {
                    if (objSession.OfficeCode != "")
                    {
                        Genaral.Load_Combo("SELECT  TR_ID, TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE ) AND TR_OFFICECODE LIKE'" + objSession.OfficeCode.Substring(0, 2) + "%' ORDER BY TR_NAME", "--Select--", cmbRepairerName);
                    }
                    Genaral.Load_Combo("SELECT  TR_ID, TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE ) AND TR_OFFICECODE LIKE'" + objSession.OfficeCode + "%' ORDER BY TR_NAME", "--Select--", cmbRepairerName);
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
            }
        }
    }
}