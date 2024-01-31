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
    public partial class DTCReport : System.Web.UI.Page
    {
        string strFormCode = "DTCReport";
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
                   // Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST   ORDER BY FD_FEEDER_CODE ", "--Select--", cmbFeederName);
                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='PT'", "--Select--", cmbSchemeType);
                    Genaral.Load_Combo("SELECT TO_NUMBER(MD_NAME)MD_NAME,TO_NUMBER(MD_NAME)MD_NAME1 FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_NAME", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbFeederName);

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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {

                cmbFeederName.SelectedIndex = 0;
                cmbSchemeType.SelectedIndex = 0;
                cmbCircle.SelectedIndex = 0;
                cmbDiv.Items.Clear();
                cmbSubDivision.Items.Clear();
                cmbOMSection.Items.Clear();

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
                 //   Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST,TBLFEEDEROFFCODE where FDO_FEEDER_ID=FD_FEEDER_ID and FDO_OFFICE_CODE like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbFeederName);
                }
                else
                {
                    Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbFeederName);
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSubDivision_SelectedIndexChanged");
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

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
               
                clsReports objReport = new clsReports();
                if (cmbFeederName.SelectedIndex != 0)
                {
                   objReport.sFeeder = cmbFeederName.SelectedValue;
                }
                if (cmbSchemeType.SelectedIndex != 0)
                {
                   objReport.sSchemeType = cmbSchemeType.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex != 0)
                {
                    objReport.sCapacity = cmbCapacity.SelectedValue;
                }

                string strofficecode = GetOfficeID();
                objReport.sOfficeCode = strofficecode;

                strParam = "id=DTCReportFeeder&OfficeCode=" + objReport.sOfficeCode + "&FeederName=" + objReport.sFeeder + "&SchemaType=" + objReport.sSchemeType +"&Capacity="+objReport.sCapacity;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmpReport_Click");
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

            if (cmbFeederName.SelectedIndex != 0)
            {
                objReport.sFeeder = cmbFeederName.SelectedValue;
            }
            if (cmbSchemeType.SelectedIndex != 0)
            {
                objReport.sSchemeType = cmbSchemeType.SelectedValue;
            }

            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;

            dt = objReport.PrintDTCCReport(objReport);

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
                    dt.Columns["TC_MANF_DATE"].ColumnName = "Manufacturing Date";
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

                    wb.Worksheets.Add(dt, "DTCReport");

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
                    rangeReporthaed.SetValue("List of DTC with Details ");

                   // wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                    //wb.Worksheet(1).Cell(2, 1).Value = "List of DTC with Details ";
                    //wb.Worksheet(1).Cell(2, 1).Style.Font.FontColor = XLColor.AirForceBlue;
                    //wb.Worksheet(1).Cell(2, 1).Style.Font.FontSize = 14;


                    wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTCReport " + DateTime.Now + ".xls";
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