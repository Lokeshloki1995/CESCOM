using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using ClosedXML.Excel;
using System.IO;


namespace IIITS.DTLMS.Reports
{
    public partial class FeederBifurcation : System.Web.UI.Page
    {
        string strFormCode = "FeederBifurcation";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
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
                Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST,TBLFEEDEROFFCODE where FDO_FEEDER_ID=FD_FEEDER_ID and FDO_OFFICE_CODE like '" + stroffCode + "%'  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbOldFeederCode);
                Genaral.Load_Combo("SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST,TBLFEEDEROFFCODE where FDO_FEEDER_ID=FD_FEEDER_ID and FDO_OFFICE_CODE like '" + stroffCode + "%'  ORDER BY FD_FEEDER_CODE ", "--Select--", cmbNewFeederCode);


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
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender3.EndDate = DateTime.Now;

            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {

                cmbOldFeederCode.SelectedIndex = 0;
                cmbNewFeederCode.SelectedIndex = 0;
                cmbEnumerationStatus.SelectedIndex = 0;
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

        protected void Export_click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;

                if (ValidateForm())
                {

                    clsReports objReport = new clsReports();
                    if (cmbOldFeederCode.SelectedIndex != 0)
                    {
                        objReport.sOldFeederCode = cmbOldFeederCode.SelectedValue;
                    }
                    if (cmbNewFeederCode.SelectedIndex != 0)
                    {
                        objReport.sNewFeederCode = cmbNewFeederCode.SelectedValue;
                    }
                     objReport.sReportType = cmbEnumerationStatus.SelectedValue;

                    if (txtFromDate.Text != null && txtFromDate.Text != "")
                    {
                        objReport.sFromDate = txtFromDate.Text;
                    }
                    if (txtToDate.Text != null && txtToDate.Text != "")
                    {
                        objReport.sTodate = txtToDate.Text;
                    }



                    string strofficecode = GetOfficeID();
                    objReport.sOfficeCode = strofficecode;

                    DataTable dt = objReport.PrintFeederBifurcationReport(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        string[] arrAlpha = Genaral.getalpha();

                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];

                        using (XLWorkbook wb = new XLWorkbook())
                        {

                            dt.Columns["CIRCLE"].SetOrdinal(0);
                            dt.Columns["CIRCLE"].ColumnName = "Circle";
                            dt.Columns["DIVISION"].SetOrdinal(1);
                            dt.Columns["DIVISION"].ColumnName = "Division";
                            dt.Columns["SUBDIVISION"].SetOrdinal(2);
                            dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                            dt.Columns["SECTION"].SetOrdinal(3);
                            dt.Columns["SECTION"].ColumnName = "Section";

                            dt.Columns["FBD_OLD_FEEDER_CODE"].ColumnName = "Old Feeder Code";
                            dt.Columns["FBD_NEW_FEEDER_CODE"].ColumnName = "New Feeder Code";
                            dt.Columns["FBD_OLD_FEEDER_NAME"].ColumnName = "Old Feeder Name";
                            dt.Columns["FBD_NEW_FEEDER_NAME"].ColumnName = "New Feeder Name";

                            dt.Columns["FBD_OLD_DTC_CODE"].ColumnName = "Old DTC Code";
                            dt.Columns["FBD_NEW_DTC_CODE"].ColumnName = "New DTC Code";
                            dt.Columns["DTC_NAME"].ColumnName = "DTC Name";


                            dt.Columns["DTR_CODE"].ColumnName = "DTR Code";
                            dt.Columns["ENUMSTATUS"].ColumnName = "Enumeration Status";


                            dt.Columns.Remove("ED_ID");
                            dt.Columns.Remove("DT_OM_SLNO");

                            wb.Worksheets.Add(dt, "FeederBifurcationReport");

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
                            rangeReporthaed.SetValue("Feeder Bifurcation details ");

                            wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            string FileName = "FeederBifutrcation" + DateTime.Now + ".xls";
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
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReport_Click");
            }
        }

        private bool ValidateForm()
        {
            bool status = true;
            string sResult = string.Empty;
            try
            {
                if (cmbEnumerationStatus.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Enumeration Type");
                    status  = false;
                    return status;
                }

                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        
                    }
                }

                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        status = false;
                        return status;
                    }
                }

                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtFromDate.Text, txtToDate.Text, false, false);
                    if (sResult == "1")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        status = false;
                        return status;
                    }
                }

                return status;
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateForm");
                return status;
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;
                string sOldFeederCode = string.Empty;
                string sNewFeederClode = string.Empty;
                string sReportType = string.Empty;
                string strOfficeCode = string.Empty;
                string sFromDate = string.Empty;
                string sToDate = string.Empty;

                if (ValidateForm())
               {
                    clsReports objReport = new clsReports();
                    if (cmbOldFeederCode.SelectedIndex != 0)
                    {
                        sOldFeederCode = cmbOldFeederCode.SelectedValue;
                    }
                    if (cmbNewFeederCode.SelectedIndex != 0)
                    {
                        sNewFeederClode = cmbNewFeederCode.SelectedValue;
                    }
                    if (cmbEnumerationStatus.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select Enumeration Type");
                        return;
                    }
                    else
                    {
                        sReportType = cmbEnumerationStatus.SelectedValue;
                    }

                    if(txtFromDate.Text != null  && txtFromDate.Text != "")
                    {
                        sFromDate = txtFromDate.Text;
                    }
                    if (txtToDate.Text != null && txtToDate.Text != "")
                    {
                        sToDate = txtToDate.Text;
                    }

                    string strofficecode = GetOfficeID();
                    objReport.sOfficeCode = strofficecode;



                    strParam = "id=FeederBifurcation&BifurcationID=" + string.Empty + "&officeCode=" + strofficecode + "&oldFeederCode=" + sOldFeederCode + "&newFeederCode=" + sNewFeederClode + "&ReportType=" + sReportType + "&FromDate="+sFromDate+"&ToDate="+sToDate ;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReport_Click");
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