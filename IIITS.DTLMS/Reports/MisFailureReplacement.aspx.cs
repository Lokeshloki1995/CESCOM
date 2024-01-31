using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Reflection;
using System.Data;

namespace IIITS.DTLMS.Reports
{
    public partial class MisFailureReplacement : System.Web.UI.Page
    {
        String strFormName = "MisFailureReplacement";
        string offCode = string.Empty;
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            if (objSession != null)
            {
                offCode = objSession.OfficeCode;
            }
            else
            {
                Response.Redirect("~/Login.aspx", false);
            }
            if (offCode.Length > 3)
            {
                groupBy.SelectedValue = "4";
                groupBy.Enabled = false;
            }
            if (offCode.Length == 3)
            {
                groupBy.SelectedValue = "3";
                groupBy.Enabled = false;
            }
            if (offCode.Length == 2)
            {
                groupBy.SelectedValue = "2";
                groupBy.Enabled = false;
            }

            // CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
            CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);

            //selectedMonth.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Attributes.Add("readonly", "readonly");

        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;
                string guarantyTypes = string.Empty ;
                string groupByDIV = string.Empty;
               
                
                //List<string> selectedGuarantyTypeValues = guarantyType.Items.Cast<ListItem>()
                //   .Where(li => li.Selected)
                //   .Select(li => li.Value)
                //   .ToList();

                if (groupBy.SelectedIndex == 0)
                {
                    if (offCode.Length > 3)
                        groupByDIV = "4";
                    if (offCode.Length == 3)
                        groupByDIV = "3";
                    if (offCode.Length == 2)
                        groupByDIV = "2";
                }
                else
                {
                    groupByDIV = groupBy.SelectedValue;
                }

                //if (selectedGuarantyTypeValues.Count == 0)
                //{
                //    ShowMsgBox("Please Select atleast one Guaranty Type ");
                //    return;
                //}
                //else
                //{
                //    guarantyTypes = string.Join(",", selectedGuarantyTypeValues.ToArray());
                //}
                if (!(groupBy.SelectedIndex > 0))
                {
                    ShowMsgBox("Please Select Division or SubDivision or Section. ");
                    return;
                }
                if (Convert.ToString(txtFromDate.Text).Length == 0)
                {
                    ShowMsgBox("Please Select From Date");
                    return;
                }
                if (Convert.ToString(txtToDate.Text).Length == 0)
                {
                    ShowMsgBox("Please Select To Date");
                    return;
                }
                clsReports objReport = new clsReports();
               
                objReport.sOfficeCode = GetUserOfficeCode();

                Button button = (Button)sender;
                string buttonId = button.ID;

                if (buttonId == "cmdExcel")
                {
                    objReport.sType = groupByDIV;
                   // objReport.sMonth = Convert.ToString(selectedMonth.Text);
                    //objReport.sGuarantyTypes = guarantyTypes.Split(',').ToList();

                     DataTable dt  = objReport.GetMisFailureReplacementTable(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        ExporttoExcel(dt);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                        return;
                    }
                }
                else
                {
                    strParam = "id=MisFailureReplacement&Officecode=" + objReport.sOfficeCode + "&GroupBy=" + groupByDIV +"&FromMonth="+Convert.ToString(txtFromDate.Text)+"&ToMonth="+Convert.ToString(txtToDate.Text) + "&GuarantyType=" + guarantyTypes + "";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went Wrong,.");
                clsException.LogError(ex.StackTrace, ex.Message, Convert.ToString(this.GetType().Name), Convert.ToString(MethodBase.GetCurrentMethod()));
            }


        }

        private void ExporttoExcel(DataTable dt)
        {
            try
            {
                dt.Columns["DCR_RANGE"].ColumnName = "CAPACITY";
                dt.Columns["STATUS"].ColumnName = "STATUS";
                dt.Columns["DIVISION_NAME"].ColumnName = "DIVISION";
                dt.Columns["GUARANTY_TYPE"].ColumnName = "GUARANTY_TYPE";
                dt.Columns["TOTAL"].ColumnName = "TOTAL";
                dt.Columns["SELECTED_GUARANTY_TYPE"].ColumnName = "SELECTED_GUARANTY_TYPE";
                dt.Columns["SELECTED_MONTH"].ColumnName = "SELECTED_MONTH";

                List<string> listtoremove = new List<string> { "SELECTED_GUARANTY_TYPE" };
                string filename = "MisFailureReplacement" + DateTime.Now + ".xls";
                string pagetitle = "FailureReplacement";

                Genaral.getexcel(dt, listtoremove, filename, pagetitle);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went Wrong,.");
                clsException.LogError(ex.StackTrace, ex.Message, Convert.ToString(this.GetType().Name), Convert.ToString(MethodBase.GetCurrentMethod()));
            }
        }

        public string GetUserOfficeCode()
        {
            try
            {
                if (offCode.Length > 0)
                {
                    if (offCode.Length >= Convert.ToInt32(groupBy.SelectedValue))
                    {
                        offCode = offCode.Substring(0, Convert.ToInt32(groupBy.SelectedValue));
                    }
                    return offCode;
                }
                return "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went Wrong,.");
                clsException.LogError(ex.StackTrace, ex.Message, Convert.ToString(this.GetType().Name), Convert.ToString(MethodBase.GetCurrentMethod()));
                return "Exception Occured";
            }
        }

        protected void BtnReset1_Click(object sender, EventArgs e)
        {
           // guarantyType.ClearSelection();
            groupBy.SelectedIndex = 0;
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }

        //protected void Export_clickFailureAbstract(object sender, EventArgs e)
        //{

        //}

        //protected void cmbCircle1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbCircle1.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
        //        }
        //        else
        //        {
        //            cmbDiv1.Items.Clear();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormName, "cmbTaluk_SelectedIndexChanged");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "ShowMsgBox");
            }
        }


        //public string GetOfficeID()
        //{
        //    string strOfficeId = string.Empty;
        //    if (cmbCircle1.SelectedIndex > 0)
        //    {
        //        strOfficeId = cmbCircle1.SelectedValue.ToString();
        //    }

        //    if (cmbDiv1.SelectedIndex > 0)
        //    {
        //        strOfficeId = cmbDiv1.SelectedValue.ToString();
        //    }
        //    return (strOfficeId);
        //}
    }
}