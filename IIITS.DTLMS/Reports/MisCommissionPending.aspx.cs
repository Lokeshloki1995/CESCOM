using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Reports
{
    public partial class MisCommissionPending : System.Web.UI.Page
    {
        String strFormName = "MisCommissionPending";
        clsSession objSession;
        static string offCode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (objSession.OfficeCode.Length > 2)
                {
                    offCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    offCode = objSession.OfficeCode;
                }

                CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);

                if (!IsPostBack)
                {
                    //Load the Circles if  length of office code is greater than 1 
                    //if (offCode.Length >= 1)
                    //{
                    //    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle1);
                    //    cmbCircle1.Items.FindByValue(offCode).Selected = true;
                    //    offCode = string.Empty;
                    //    offCode = objSession.OfficeCode;

                    //}
                    //if (offCode == null || offCode == "")
                    //{
                    //    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle1);
                    //}

                    //if (offCode.Length >= 1)
                    //{
                    //    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                    //    if (offCode.Length >= 2)
                    //    {
                    //        offCode = objSession.OfficeCode.Substring(0, 2);
                    //        cmbDiv1.Items.FindByValue(offCode).Selected = true;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "page_load");
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                clsReports objReport = new clsReports();

                //if (cmbCircle1.SelectedIndex > 0)
                //{
                //    strOfficeCode = cmbCircle1.SelectedValue;
                //}
                //else if (cmbDiv1.SelectedIndex > 0)
                //{
                //    strOfficeCode = cmbDiv1.SelectedValue;
                //}
                
                    strOfficeCode = "";


                strParam = "id=MisCommisionPending&Officecode=" + objReport.sOfficeCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "cmdReport_Click");
            }
        }

        protected void BtnReset1_Click(object sender, EventArgs e)
        {
            //cmbCircle1.SelectedIndex = 0;
            //cmbDiv1.SelectedIndex = 0;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
        }

        protected void Export_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                //if (cmbCircle1.SelectedIndex > 0)
                //{
                //    offCode = cmbCircle1.SelectedValue;
                //}
                //else if (cmbDiv1.SelectedIndex > 0)
                //{
                //    offCode = cmbDiv1.SelectedValue;
                //}
                //else
                    offCode = "";

                objReport.sOfficeCode = offCode ;

                dt = objReport.MisCommissionPending(objReport);
                // implementation of adding to excel

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "Export_clickFailureAbstract");
            }
        }

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