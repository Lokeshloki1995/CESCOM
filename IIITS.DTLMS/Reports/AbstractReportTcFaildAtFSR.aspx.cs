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
    public partial class AbstractReportTcFaildAtFSR : System.Web.UI.Page
    {
        string strFormCode = "AbstractReportTcFaildAtFSR";
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
                    //if (stroffCode.Length >= 2)
                    //{

                    //    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    //    if (stroffCode.Length >= 3)
                    //    {
                    //        stroffCode = objSession.OfficeCode.Substring(0, 3);
                    //        cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                    //        stroffCode = string.Empty;
                    //        stroffCode = objSession.OfficeCode;
                    //    }
                    //}
                    //if (stroffCode.Length >= 3)
                    //{
                    //    Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                    //    if (stroffCode.Length == 4)
                    //    {
                    //        stroffCode = objSession.OfficeCode.Substring(0, 4);
                    //        cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                    //    }
                    //}


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
                   // cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }

                else
                {
                    cmbDiv.Items.Clear();
                   // cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();

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
        //            cmbOMSection.Items.Clear();
        //        }
        //        else
        //        {
        //            cmbSubDiv.Items.Clear();
        //            cmbOMSection.Items.Clear();
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
        //        else
        //        {
        //            cmbOMSection.Items.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
        //    }
        //}
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
        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;

                clsReports objReport = new clsReports();
                string strofficecode = GetOfficeID();

                strParam = "id=AbstractRptTcFailed&Officecode=" + strofficecode;
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
               
                cmbCircle.SelectedIndex = 0;
                //cmbSubDiv.Items.Clear();
                cmbDiv.Items.Clear();
                //cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            DataTable dtcompletedRepairCount = new DataTable();
            clsReports objReport = new clsReports();

            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;

            dt = objReport.PrintAbstractReportTcFailedAtFSR(objReport);
            //dtcompletedRepairCount = objReport.PrintCompletedRepairerTcCount();

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

               
                using (XLWorkbook wb = new XLWorkbook())
                {

                   
                   
                    dt.Columns["OFF_CODE"].ColumnName = "Off Code";
                    dt.Columns["FIELD_COUNT"].ColumnName = "@Field";
                    dt.Columns["FIELD_COUNT_TOBEREPLACED"].ColumnName = "Tc To Be Replace";
                    dt.Columns["STORE_COUNT"].ColumnName = "@Store";
                    dt.Columns["REPAIRER_COUNT"].ColumnName = "@Repairer";
                  

                    dt.Columns["CIRCLE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["CAPACITY"].SetOrdinal(2);
                    dt.Columns["CAPACITY"].ColumnName = "Capacity";
                    dt.Columns["DIV"].SetOrdinal(1);
                    dt.Columns["DIV"].ColumnName = "Division";
                    dt.Columns.Remove("Off Code");
                  
                    wb.Worksheets.Add(dt, "AbstractTcFailed");
                    //wb.Worksheet(1).Cell(1,5).InsertTable(dtcompletedRepairCount);

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                   // wb.Worksheet(1).Column(4).Delete();

                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 14;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue("Details of the failed Tc at Repair centre/ Store/ Field    as on  " + DateTime.Now);



                    wb.Worksheet(1).Cell(3, 7).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "AbstractTcFailed " + DateTime.Now + ".xls";
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
    }
}