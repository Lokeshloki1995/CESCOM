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
    public partial class DTrReport : System.Web.UI.Page
    {
        string strFormCode = "DTrReport";
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

                    string strQry = string.Empty;

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

                    strQry = "SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_NAME";
                    Genaral.Load_Combo("SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_NAME", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C'", "--Select--", cmbCapacity);

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
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                string strValue = string.Empty;


                clsReports objReport = new clsReports();
                if (cmbMake.SelectedIndex != 0)
                {
                    objReport.sMake = cmbMake.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex != 0)
                {
                    objReport.sCapacity = cmbCapacity.SelectedValue;
                }

                string strofficecode = GetOfficeID();
                objReport.sOfficeCode = strofficecode;

                strParam = "id=DTrReportMake&OfficeCode=" + objReport.sOfficeCode + "&Make=" + objReport.sMake + "&Capacity=" + objReport.sCapacity;
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

                cmbMake.SelectedIndex = 0;
                cmbCapacity.SelectedIndex = 0;
                cmbCircle.SelectedIndex = 0;
                cmbDiv.Items.Clear();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
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



            return (strOfficeId);
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
        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            if (cmbMake.SelectedIndex != 0)
            {
                objReport.sMake = cmbMake.SelectedValue;
            }
            if (cmbCapacity.SelectedIndex != 0)
            {
                objReport.sCapacity = cmbCapacity.SelectedValue;
            }

            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;

            dt = objReport.PrintDTrReport(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

                string sMergeRange = arrAlpha[dt.Columns.Count - 1];

                using (XLWorkbook wb = new XLWorkbook())
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                    dt.Columns["TC_SLNO"].ColumnName = "Tc Sl No";
                    dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Capacity";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "Manufacturing Date";
                    dt.Columns["LOCATIONNAME"].ColumnName = "Tc Current Location";
                    dt.Columns["TC_STAR_RATE"].ColumnName = "DTR Star Rate";
                    dt.Columns["TC_OIL_CAPACITY"].ColumnName = "Oil Capacity";

                    dt.Columns["CIRCLE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].SetOrdinal(1);
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].SetOrdinal(2);
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].SetOrdinal(3);
                    dt.Columns["SECTION"].ColumnName = "Section";


                    wb.Worksheets.Add(dt, "DtrReport");

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
                    rangeReporthaed.SetValue("List of Transformers with details ");

                    wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DtrReport" + DateTime.Now + ".xls";
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