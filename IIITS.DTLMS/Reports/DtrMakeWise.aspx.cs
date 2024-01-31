using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using ClosedXML.Excel;
using System.Data;
namespace IIITS.DTLMS.Reports
{
    public partial class DtrMakeWise : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();
        string strFormCode = "DtrMakeWise";
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
                clsReports objReport = new clsReports();
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                string strMakeValue = ConfigurationManager.AppSettings["DtrMakeWiserpt"].ToString();
                strParam = "id=DTr make wise Reports&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&MakeValue=" + strMakeValue + " &offcode=" + strofficecode + "";
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmpReport_Click");
            }
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

            string strofficecode = GetOfficeID();
            clsReports objReports = new clsReports();
          //  string strOfficeCode = string.Empty;
            string strParam = string.Empty;


            if (txtFromDate.Text != null && txtFromDate.Text != "")
            {
                objReports.sFromDate = txtFromDate.Text;

            }
            if (txtToDate.Text != null && txtToDate.Text != "")
            {
                objReports.sTodate = txtToDate.Text;

            }

              //  objReport.sMake = Request.QueryString["MakeValue"].ToString();
            objReport.sMake = ConfigurationManager.AppSettings["DtrMakeWiserpt"].ToString();

            objReport.sOfficeCode = strofficecode;

            dt = objReport.Printdtrwise(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
             

                using (XLWorkbook wb = new XLWorkbook())
                {

                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["TCCOUNT"].ColumnName = "Tc Count";
                    dt.Columns["FCOUNT"].ColumnName = "Failure Count";
                    dt.Columns["FAILUREPERCENTAGE"].ColumnName = "Failure Percentage";
                    //dt.Columns["ROWNUM"].ColumnName = "Sl.no";
                    //dt.Columns["ROWNUM"].Dispose();

                    dt.Columns.Remove("ROWNUM");
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("CURRENTDATE");


                    wb.Worksheets.Add(dt, "DTR Make Wise");
                   

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                   // wb.Worksheet(1).Column(5).Delete();

                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    
                    var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 10;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

                    var rangeReporthaed = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthaed.Merge().Style.Font.SetBold().Font.FontSize = 8;
                    rangeReporthaed.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthaed.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    if (txtFromDate.Text != "" && txtToDate.Text != "")
                    {
                        rangeReporthaed.SetValue("Make Wise Failure Analysis Report  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthaed.SetValue("Make Wise Failure Analysis Report  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthaed.SetValue("Make Wise Failure Analysis Report  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthaed.SetValue("Make Wise Failure Analysis Report as on " + DateTime.Now);
                    }
                    //if(objReport.sFromDate != null)
                    //{
                    //rangeReporthaed.SetValue("Make Wise Failure Analysis Report" + objReport.sFromDate + "  To" + objReport.sTodate );
                    //}
                    //else{

                    //    rangeReporthaed.SetValue("Make Wise Failure Analysis Report");

                    //}

                    wb.Worksheet(1).Cell(3, 4).Value = DateTime.Now;
                  

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTR Make Wise " + DateTime.Now + ".xls";
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