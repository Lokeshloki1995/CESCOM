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
    public partial class EnumReport : System.Web.UI.Page
    {
        string strFormCode = "EnumerationReport";
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

                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "--All--", cmbDiv);
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }


      

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE, SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--All--", cmbSubDiv);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbDiv.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--All--", cmbFeeder);
                }
                else
                {

                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
            }
        }


        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE, OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE ='" + cmbSubDiv.SelectedValue + "' ORDER BY OM_CODE", "--All--", cmbSection);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbDiv.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--All--", cmbFeeder);

                }
                else
                {

                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
            }
        }


        protected void cmpReport_Click(object sender, EventArgs e)
        {
            try
            {
                 string strOfficeCode = string.Empty;

                 if (cmbType.SelectedIndex == 0)
                 {
                     ShowMsgBox("Select Report Type");
                     return;
                 }

                if (cmbType.SelectedValue == "1")
                {


                    //if (cmbDiv.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select Division  name");
                    //    return;

                    //}
                   
                    //else if (cmbFeeder.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select Feeder name");
                    //    return;
                    //}
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                       
                    }

                   
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                 

                    strOfficeCode = "id=StoreLoc&OfficeCode=" + strOfficeCode + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text;
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strOfficeCode + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }

                else if(cmbType.SelectedValue == "2")
                {

                    //if (cmbDiv.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Please Select Division Name");
                    //    return;
                    //}
                   

                    //if (cmbSubDiv.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select Sub division Name");
                    //    return;

                    //}
                   

                    //if (cmbSection.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select Section Name");
                    //    return;
                    //}
                                      
                    //if (cmbFeeder.SelectedIndex == 0)
                    //{

                    //    ShowMsgBox("Select Feeder Name");
                    //    return;
                    //}
                    

                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue; 
                    }
                    if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }

                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }

                    string sFeederCode = string.Empty;

                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }

                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }

                    strOfficeCode = "id=FieldLoc&OfficeCode=" + strOfficeCode + "&sFeeder=" + sFeederCode + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text;
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strOfficeCode + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmpReport_Click");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }


    }
}