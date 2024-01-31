using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS
{
    public partial class InternalDashboard : System.Web.UI.Page
    {
        string strFormCode = "InternalDashboard";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    GetCurrentEnumerationCount();
                    GetTotalEnumerationCount();

                    lblDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

                    var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTime dFromDate = firstDayOfMonth;
                    DateTime dToDate = System.DateTime.Now;

                    //lblMonthwiseTitle.Text = "Details From " + dFromDate.ToString("dd-MM-yyyy") + " and " + dToDate.ToString("dd-MM-yyyy") + " ";

                    

                    txtFromDate.Text = dFromDate.ToString("dd/MM/yyyy");
                    txtToDate.Text = dToDate.ToString("dd/MM/yyyy");

                    GetDateWiseEnumerationCount();

                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "--All--", cmbDiv);
                }

            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");

            }
        }


        public void GetCurrentEnumerationCount()
        {
            try
            {
                clsInterDashboard objDashboard = new clsInterDashboard();

                // User Type--->1  Operator
                if (objSession.UserType == "1")
                {

                    //Total Enumeration Count
                    lblCurrentTotalEnum.Text = objDashboard.GetToatlEnumerationCount(objSession.UserId,true);

                    //QC pending
                    lblCurrentPending.Text = objDashboard.GetQCPendingEnumCount(objSession.UserId,true);

                    //QC Done
                    lblCurrentQCDone.Text = objDashboard.GetQCDoneEnumCount(objSession.UserId,true);

                    //Reject Enumeration Count
                    lblCurrentReject.Text = objDashboard.GetRejectEnumCount(objSession.UserId,true);

                    //Pending for Clarification Enumeration Count
                    lblCurrentPendingClarif.Text = objDashboard.GetPendingForClarificationEnumCount(objSession.UserId, true);

                    dvReject.Style.Add("display", "block");

                }
                
                 // User Type--->3  SuperVisor
                else if (objSession.UserType == "3")
                {

                    //Total Enumeration Count
                    lblOperatorCount.Text = objDashboard.GetOperatorCountForSuperVisor(objSession.UserId);

                    //Total Enumeration Count
                    lblCurrentTotalEnum.Text = objDashboard.GetTotalEnumCountForSuperVisor(objSession.UserId, true);

                    //QC pending
                    lblCurrentPending.Text = objDashboard.GetQCPendingCountForSuperVisor(objSession.UserId, true);

                    //QC Done
                    lblCurrentQCDone.Text = objDashboard.GetQCDoneCountForSuperVisor(objSession.UserId, true);

                    //Reject Enumeration Count
                    lblCurrentReject.Text = objDashboard.GetQCRejectCountForSuperVisor(objSession.UserId, true);

                    //Pending for Clarification Enumeration Count
                    lblCurrentPendingClarif.Text = objDashboard.GetPendingForClarificationCountForSuperVisor(objSession.UserId, true);

                    dvOperator.Style.Add("display", "block");

                }

                // User Type--->2  QC Executive
                else if (objSession.UserType == "2")
                {

                    //Total Enumeration Count
                    lblCurrentTotalEnum.Text = objDashboard.GetToatlEnumerationCount("", true);

                    //QC pending
                    lblCurrentPending.Text = objDashboard.GetQCPendingEnumCount("", true);

                    //QC Done
                    lblCurrentQCDone.Text = objDashboard.GetQCDoneEnumCount("", true);

                    //Reject Enumeration Count
                    lblCurrentReject.Text = objDashboard.GetRejectEnumCount("", true);

                    //Pending for Clarification Enumeration Count
                    lblCurrentPendingClarif.Text = objDashboard.GetPendingForClarificationEnumCount("", true);

                }

                 // User Type--->4 Internal Admin
                else if (objSession.UserType == "4")
                {

                    //Total Enumeration Count
                    lblCurrentTotalEnum.Text = objDashboard.GetToatlEnumerationCount("", true);

                    //QC pending
                    lblCurrentPending.Text = objDashboard.GetQCPendingEnumCount("", true);

                    //QC Done
                    lblCurrentQCDone.Text = objDashboard.GetQCDoneEnumCount("", true);

                    //Reject Enumeration Count
                    lblCurrentReject.Text = objDashboard.GetRejectEnumCount("", true);

                    //Pending for Clarification Enumeration Count
                    lblCurrentPendingClarif.Text = objDashboard.GetPendingForClarificationEnumCount("", true);

                    //Operator  Count
                    lblOperatorCount.Text = objDashboard.GetOperatorCount();

                }

                
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTotalEnumCount");
            }
        }


        public void GetTotalEnumerationCount()
        {
            try
            {
                clsInterDashboard objDashboard = new clsInterDashboard();

                // User Type--->1  Operator
                if (objSession.UserType == "1")
                {

                    //Total Enumeration Count
                    lblTotalEnum.Text = objDashboard.GetToatlEnumerationCount(objSession.UserId);

                    //QC pending
                    lblQCPending.Text = objDashboard.GetQCPendingEnumCount(objSession.UserId);

                    //QC Done
                    lblQCDone.Text = objDashboard.GetQCDoneEnumCount(objSession.UserId);

                    //Reject Enumeration Count
                    lblReject.Text = objDashboard.GetRejectEnumCount(objSession.UserId);

                    //Pending for Clarification Enumeration Count
                    lblPendingForClarif.Text = objDashboard.GetPendingForClarificationEnumCount(objSession.UserId);

                    dvReject.Style.Add("display", "block");

                }

                 // User Type--->3  SuperVisor
                else if (objSession.UserType == "3")
                {

                    //Total Enumeration Count
                    lblOperatorCount.Text = objDashboard.GetOperatorCountForSuperVisor(objSession.UserId);

                    //Total Enumeration Count
                    lblTotalEnum.Text = objDashboard.GetTotalEnumCountForSuperVisor(objSession.UserId);

                    //QC pending
                    lblQCPending.Text = objDashboard.GetQCPendingCountForSuperVisor(objSession.UserId);

                    //QC Done
                    lblQCDone.Text = objDashboard.GetQCDoneCountForSuperVisor(objSession.UserId);

                    //Reject Enumeration Count
                    lblReject.Text = objDashboard.GetQCRejectCountForSuperVisor(objSession.UserId);


                    //Pending for Clarification Enumeration Count
                    lblPendingForClarif.Text = objDashboard.GetPendingForClarificationCountForSuperVisor(objSession.UserId);

                    dvOperator.Style.Add("display", "block");

                }


                // User Type--->2  QC Executive
                else if (objSession.UserType == "2")
                {

                    //Total Enumeration Count
                    lblTotalEnum.Text = objDashboard.GetToatlEnumerationCount();

                    //QC pending
                    lblQCPending.Text = objDashboard.GetQCPendingEnumCount();

                    //QC Done
                    lblQCDone.Text = objDashboard.GetQCDoneEnumCount();

                    //Reject Enumeration Count
                    lblReject.Text = objDashboard.GetRejectEnumCount();

                    //Pending for Clarification Enumeration Count
                    lblPendingForClarif.Text = objDashboard.GetPendingForClarificationEnumCount();

                }


                // User Type--->4  Internal Admin
                else if (objSession.UserType == "4")
                {

                    //Total Enumeration Count
                    lblOperatorCount.Text = objDashboard.GetOperatorCount();

                    //Total Enumeration Count
                    lblTotalEnum.Text = objDashboard.GetToatlEnumerationCount();

                    //QC pending
                    lblQCPending.Text = objDashboard.GetQCPendingEnumCount();

                    //QC Done
                    lblQCDone.Text = objDashboard.GetQCDoneEnumCount();

                    //Reject Enumeration Count
                    lblReject.Text = objDashboard.GetRejectEnumCount();

                    //Pending for Clarification Enumeration Count
                    lblPendingForClarif.Text = objDashboard.GetPendingForClarificationEnumCount();

                }



            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTotalEnumCount");
            }
        }


        public void GetDateWiseEnumerationCount(string sOfficeCode="",string sFeederCode="")
        {
            try
            {
                clsInterDashboard objDashboard = new clsInterDashboard();

                // User Type--->1  Operator
                if (objSession.UserType == "1")
                {

                    //Total Enumeration Count
                    lblTotalEnumDateWise.Text = objDashboard.GetToatlEnumerationCount(objSession.UserId, false,
                        txtFromDate.Text.Trim(),txtToDate.Text.Trim(),sOfficeCode,sFeederCode);

                    //QC pending
                    lblPendingQCDateWise.Text = objDashboard.GetQCPendingEnumCount(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC Done
                    lblQCdoneDateWise.Text = objDashboard.GetQCDoneEnumCount(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Reject Enumeration Count
                    lblRejectDateWise.Text = objDashboard.GetRejectEnumCount(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Pending for Clarification Enumeration Count
                    lblPendingClarifDateWise.Text = objDashboard.GetPendingForClarificationEnumCount(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    dvReject.Style.Add("display", "block");

                }

                 // User Type--->3  SuperVisor
                else if (objSession.UserType == "3")
                {

                    //Total Enumeration Count
                    lblOperatorCount.Text = objDashboard.GetOperatorCountForSuperVisor(objSession.UserId);

                    //Total Enumeration Count
                    lblCurrentTotalEnum.Text = objDashboard.GetTotalEnumCountForSuperVisor(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC pending
                    lblCurrentPending.Text = objDashboard.GetQCPendingCountForSuperVisor(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC Done
                    lblCurrentQCDone.Text = objDashboard.GetQCDoneCountForSuperVisor(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Reject Enumeration Count
                    lblCurrentReject.Text = objDashboard.GetQCRejectCountForSuperVisor(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Pending for Clarification Enumeration Count
                    lblCurrentPendingClarif.Text = objDashboard.GetPendingForClarificationCountForSuperVisor(objSession.UserId, false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    dvOperator.Style.Add("display", "block");

                }

                // User Type--->2  QC Executive
                else if (objSession.UserType == "2")
                {

                    //Total Enumeration Count
                    lblTotalEnumDateWise.Text = objDashboard.GetToatlEnumerationCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC pending
                    lblPendingQCDateWise.Text = objDashboard.GetQCPendingEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC Done
                    lblQCdoneDateWise.Text = objDashboard.GetQCDoneEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Reject Enumeration Count
                    lblRejectDateWise.Text = objDashboard.GetRejectEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Pending for Clarification Enumeration Count
                    lblPendingClarifDateWise.Text = objDashboard.GetPendingForClarificationEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                }

                 // User Type--->4 Internal Admin
                else if (objSession.UserType == "4")
                {

                    //Total Enumeration Count
                    lblTotalEnumDateWise.Text = objDashboard.GetToatlEnumerationCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC pending
                    lblPendingQCDateWise.Text = objDashboard.GetQCPendingEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //QC Done
                    lblQCdoneDateWise.Text = objDashboard.GetQCDoneEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Reject Enumeration Count
                    lblRejectDateWise.Text = objDashboard.GetRejectEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Pending for Clarification Enumeration Count
                    lblPendingClarifDateWise.Text = objDashboard.GetPendingForClarificationEnumCount("", false,
                        txtFromDate.Text.Trim(), txtToDate.Text.Trim(), sOfficeCode, sFeederCode);

                    //Operator  Count
                    lblOperatorCount.Text = objDashboard.GetOperatorCount();

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDateWiseEnumerationCount");
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = string.Empty;
                string sFeederCode = string.Empty;

                //if (txtFromDate.Text.Trim() == "")
                //{
                //    ShowMsgBox("Enter From Date");
                //    return;

                //}

                if (txtToDate.Text.Trim() == "")
                {
                    txtToDate.Text = txtFromDate.Text;
                }

                if (cmbDiv.SelectedIndex > 0)
                {
                    sOfficeCode = cmbDiv.SelectedValue;
                }
                if (cmbFeeder.SelectedIndex > 0)
                {
                    sFeederCode = cmbFeeder.SelectedValue;
                }

                GetDateWiseEnumerationCount(sOfficeCode,sFeederCode);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearch_Click");
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbDiv.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--All--", cmbFeeder);

                    
                }
                else
                {
                    cmbFeeder.Items.Clear();
                }

                cmdSearch_Click(sender, e);
            }
            catch (Exception ex)
            {              
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
            }
        }

        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmdSearch_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbFeeder_SelectedIndexChanged");
            }
        }

        protected void lnkMonthWiseTitle_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = string.Empty;
                string sFeederCode = string.Empty;

                if (txtToDate.Text.Trim() == "")
                {
                    txtToDate.Text = txtFromDate.Text;
                }

                if (cmbDiv.SelectedIndex > 0)
                {
                    sOfficeCode = cmbDiv.SelectedValue;
                }
                if (cmbFeeder.SelectedIndex > 0)
                {
                    sFeederCode = cmbFeeder.SelectedValue;
                }

                string url = "StatusReport.aspx?RefId=Custom&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&OffCode=" + sOfficeCode + "&FeederCode=" + sFeederCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                //Response.Redirect("StatusReport.aspx?RefId=Custom&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&OffCode=" + sOfficeCode + "&FeederCode=" + sFeederCode, false);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkMonthWiseTitle_Click");
            }
        }
    }
}