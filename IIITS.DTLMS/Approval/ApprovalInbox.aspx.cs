using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Web.Security;
using System.Configuration;

namespace IIITS.DTLMS.Approval
{
    public partial class ApprovalInbox : System.Web.UI.Page
    {
        string strFormCode = "ApprovalInbox";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {   
                    Response.Redirect("~/Login.aspx", true);
                }

              //  Response.Cookies["AuthToken"].Path = "/Approval";
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtFromDate_CalendarExtender1.EndDate = DateTime.Now;
                    txtToDate_CalendarExtender1.EndDate = DateTime.Now;

                if (Request.QueryString["PId"] != null && Request.QueryString["PId"].ToString() != "")
                {
                    string PId = Genaral.Decrypt(Request.QueryString["PId"].ToString());
                    if (PId == "MMS")
                    {
                        Session.Abandon();
                        Session.Clear();
                        Response.Redirect(ConfigurationSettings.AppSettings["MMS_Redirect_Path"].ToString(), false);
                    }
                }
                if (!IsPostBack)
                {
                    clsUser objUser = new clsUser();
                    LoadCombo();
                    ViewState["MMS_USID"] = objUser.getMMSUserId(objSession.UserId);
                    LoadPendingApprovalInbox();
                    //CheckAccessRights("4");
                    

                    if (Request.QueryString["RefType"] != null && Request.QueryString["RefType"].ToString() != "")
                    {
                        hdfRefType.Value = Convert.ToString(Request.QueryString["RefType"]);
                        if (hdfRefType.Value == "1")
                        {
                            rdbAlready.Checked = true;
                            rdbPending.Checked = false;
                            LoadAlreadyApprovedInbox();
                        }
                        if (hdfRefType.Value == "3")
                        {
                            rdbAlready.Checked = false;
                            rdbPending.Checked = false;
                            rdbRejected.Checked = true;
                            //LoadAlreadyApprovedInbox();
                            LoadRejectedApprovedInbox();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.Intigration_LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load", objSession.UserId);
            }

        }


        public void LoadCombo()
        {
            try
            {
                Genaral.Load_Combo("SELECT BO_ID,BO_NAME FROM TBLWORKFLOWMASTER,TBLBUSINESSOBJECT WHERE WM_ROLEID='" + objSession.RoleId + "' AND WM_BOID=BO_ID", "-Select--", cmbSubject);

                string strQry = " SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE US_ROLE_ID IN (SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID IN ";
                strQry += " (SELECT WM_BOID FROM TBLWORKFLOWMASTER WHERE WM_ROLEID='" + objSession.RoleId + "')) AND US_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND US_ROLE_ID<>'" + objSession.RoleId + "'";

                Genaral.Load_Combo(strQry, "-Select--", cmbSentBy);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCombo");
            }
        }

        public void LoadPendingApprovalInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                string sFilter = string.Empty;
                DataTable dtView = new DataTable();
                DataView dv = new DataView();
                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sOfficeCode = objSession.OfficeCode;

                if (cmbSubject.SelectedIndex > 0)
                {
                    objApproval.sBOId = cmbSubject.SelectedValue;
                }
                if (cmbSentBy.SelectedIndex > 0)
                {
                    objApproval.sCrby = cmbSentBy.SelectedValue;
                }

                objApproval.sFromDate = txtFromDate.Text;
                objApproval.sToDate = txtToDate.Text;

                objApproval.sFormName = sFormName;
                objApproval.sDescription = sDesc;

                if (objApproval.sBOId == "7")
                {
                    dtView = (DataTable)ViewState["Approval"];
                    dv = dtView.DefaultView;
                    sFilter = "BO_NAME ='DeCommission Entry' AND WO_DESCRIPTION LIKE 'Commissioning of DTC%' ";
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdApprovalInbox.DataSource = dv;
                        ViewState["Approval"] = dv.ToTable();
                        grdApprovalInbox.DataBind();

                    }
                    else
                    {

                        ShowEmptyGrid();
                    }

                }
                else
                {

                    DataTable dt = objApproval.LoadPendingApprovalInbox(objApproval);
                    if (dt.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["Approval"] = dt;
                    }
                    else
                    {
                        grdApprovalInbox.DataSource = dt;
                        grdApprovalInbox.DataBind();
                        ViewState["Approval"] = dt;
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadApprovalInbox");
            }
        }


        public void LoadAlreadyApprovedInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                string sFilter = string.Empty;
                DataTable dtView = new DataTable();
                DataView dv = new DataView();
                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sOfficeCode = objSession.OfficeCode;

                if (cmbSubject.SelectedIndex > 0)
                {
                    objApproval.sBOId = cmbSubject.SelectedValue;
                }
                if (cmbSentBy.SelectedIndex > 0)
                {
                    objApproval.sCrby = cmbSentBy.SelectedValue;
                }

                objApproval.sFromDate = txtFromDate.Text;
                objApproval.sToDate = txtToDate.Text;

                objApproval.sFormName = sFormName;
                objApproval.sDescription = sDesc;

                if (objApproval.sBOId == "7")
                {
                    dtView = (DataTable)ViewState["Approval"];
                    dv = dtView.DefaultView;
                    sFilter = "BO_NAME ='DeCommission Entry' AND WO_DESCRIPTION LIKE 'Commissioning of DTC%'";
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdApprovalInbox.DataSource = dv;
                        ViewState["Approval"] = dv.ToTable();
                        grdApprovalInbox.DataBind();

                    }
                    else
                    {

                        ShowEmptyGrid();
                    }

                }
                else
                {

                    DataTable dt = objApproval.LoadAlreadyApprovedInbox(objApproval);
                    if (dt.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["Approval"] = dt;
                    }
                    else
                    {
                        grdApprovalInbox.DataSource = dt;
                        grdApprovalInbox.DataBind();
                        ViewState["Approval"] = dt;
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadApprovalInbox");
            }
        }

        public void LoadRejectedApprovedInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sOfficeCode = objSession.OfficeCode;

                if (cmbSubject.SelectedIndex > 0)
                {
                    objApproval.sBOId = cmbSubject.SelectedValue;
                }
                if (cmbSentBy.SelectedIndex > 0)
                {
                    objApproval.sCrby = cmbSentBy.SelectedValue;
                }

                objApproval.sFromDate = txtFromDate.Text;
                objApproval.sToDate = txtToDate.Text;

                objApproval.sFormName = sFormName;
                objApproval.sDescription = sDesc;

                DataTable dt = objApproval.LoadRejectedApprovedInbox(objApproval);
                if (dt.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["Approval"] = dt;
                }
                else
                {
                    grdApprovalInbox.DataSource = dt;
                    grdApprovalInbox.DataBind();
                    ViewState["Approval"] = dt;
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadApprovalInbox");
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


        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("WO_ID");
                dt.Columns.Add("WO_RECORD_ID");
                dt.Columns.Add("WO_BO_ID");
                dt.Columns.Add("BO_NAME");
                dt.Columns.Add("USER_NAME");
                dt.Columns.Add("CR_ON");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("CURRENT_STATUS");
                dt.Columns.Add("RO_NAME");
                dt.Columns.Add("WO_APPROVE_STATUS");
                dt.Columns.Add("WO_DESCRIPTION");
                dt.Columns.Add("WOA_ID");
                dt.Columns.Add("WO_WFO_ID");
                dt.Columns.Add("WO_INITIAL_ID");
                dt.Columns.Add("CREATOR");

                grdApprovalInbox.DataSource = dt;
                grdApprovalInbox.DataBind();
                int iColCount = grdApprovalInbox.Rows[0].Cells.Count;
                grdApprovalInbox.Rows[0].Cells.Clear();
                grdApprovalInbox.Rows[0].Cells.Add(new TableCell());
                grdApprovalInbox.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdApprovalInbox.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        protected void grdApprovalInbox_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdApprovalInbox.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Approval"];
                grdApprovalInbox.DataSource = SortDataTable(dt as DataTable, true);
                grdApprovalInbox.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdApprovalInbox_PageIndexChanging");
            }
        }

        protected void grdApprovalInbox_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdApprovalInbox.PageIndex;
            DataTable dt = (DataTable)ViewState["Approval"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdApprovalInbox.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdApprovalInbox.DataSource = dt;
            }
            grdApprovalInbox.DataBind();
            grdApprovalInbox.PageIndex = pageIndex;
        }

        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";

                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";

                    break;
            }


            return GridViewSortDirection;
        }

        protected void grdApprovalInbox_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Approve")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    //string sApproveStatus = "1";

                    //ViewState["WFObject"] = sWFOId;
                    //ViewState["AppStatus"] = sApproveStatus;

                    //cmdApprove.Text = "Approve";
                    //txtComment.Text = "";

                    //cmdApprove.Enabled = true;
                    //this.mdlPopup.Show();

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;

                    RedirectToForm(sBOId, sRecordId, "A", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus);

                }

                if (e.CommandName == "Reject")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    //string sApproveStatus = "3";

                    //ViewState["WFObject"] = sWFOId;
                    //ViewState["AppStatus"] = sApproveStatus;

                    //cmdApprove.Text = "Reject";
                    //txtComment.Text = "";

                    //cmdApprove.Enabled = true;
                    //this.mdlPopup.Show();   


                    //Redirect to Form to Approve

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;

                    RedirectToForm(sBOId, sRecordId, "R", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus);

                }
                //Modify
                if (e.CommandName == "Modify")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;

                    RedirectToForm(sBOId, sRecordId, "M", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus);

                }

                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;

                    RedirectToForm(sBOId, sRecordId, "V", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                   

                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
              
                    TextBox txtFormName = (TextBox)row.FindControl("txtFormName");
                    TextBox txtDesc = (TextBox)row.FindControl("txtDesc");
             

                    if (rdbAlready.Checked == true)
                    {
                        LoadAlreadyApprovedInbox(txtFormName.Text.Trim().Replace("'", "''"), txtDesc.Text.Trim().Replace("'", "''"));
                    }
                    if (rdbPending.Checked == true)
                    {
                        LoadPendingApprovalInbox(txtFormName.Text.Trim().Replace("'", "''"), txtDesc.Text.Trim().Replace("'", "''"));
                    }
                    if (rdbRejected.Checked == true)
                    {
                        LoadRejectedApprovedInbox(txtFormName.Text.Trim().Replace("'", "''"), txtDesc.Text.Trim().Replace("'", "''"));
                    }

                }

                if (e.CommandName == "History")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sSubject = ((Label)row.FindControl("lblSubject")).Text;

                    if (sBOId == "14" && sSubject.Contains("Commissioning"))
                    {
                        sBOId = "7";
                    }

                    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                    sBOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sBOId));


                    Response.Redirect("ApprovalHistory.aspx?RecordId=" + sRecordId + "&BOId=" + sBOId, false);

                }

                if (e.CommandName == "Redirect")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    clsUser objUser = new clsUser();
                    string sMMSUid = objUser.getMMSUserId(objSession.UserId);
                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;

                    Session["ProjectType"] = "MMS";

                    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                    sBOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sBOId));

                    //LinkButton sbutton = ((LinkButton)row.FindControl("lnkRedirect"));
                    ////string url = ConfigurationSettings.AppSettings["MMS_Redirect_Path"].ToString() + "ApprovalInbox/ApprovalInbox?UId=" + sMMSUid;
                    //string url = "http://www.cescerp.com";
                    //sbutton.Attributes.Add("href", url);
                    //  sbutton.Attributes.Add("target", "_blank");


                    //if (sMMSUid == null || sMMSUid == "")
                    //{
                    //    //Response.Redirect("http://www.cescerp.com");
                    //    string url = ConfigurationSettings.AppSettings["MMS_Redirect_Path"].ToString();
                    //    sbutton.Attributes.Add("href", url);
                    //    sbutton.Attributes.Add("target", "_blank");
                    //    //string s = "window.open('" + url + "', '_blank');";
                    //    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    //}
                    //else
                    //{
                    //    //Response.Redirect("http://www.cescerp.com");
                    //    string url = ConfigurationSettings.AppSettings["MMS_Redirect_Path"].ToString() + "ApprovalInbox/ApprovalInbox?UId=" + sMMSUid;
                    //    sbutton.Attributes.Add("href", url);
                    //    sbutton.Attributes.Add("target", "_blank");
                    //    //string s = "window.open('" + url + "', '_blank');";
                    //    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    //}
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdApprovalInbox_RowCommand");

            }
        }


        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "TcMaster";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            try
            {
                clsApproval objApproval = new clsApproval();


                if (txtComment.Text.Trim() == "")
                {
                    lblMsg.Text = "Enter Comments/Remarks";
                    txtComment.Focus();
                    this.mdlPopup.Show();
                    return;

                }

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = Convert.ToString(ViewState["WFObject"]);
                objApproval.sApproveStatus = Convert.ToString(ViewState["AppStatus"]);


                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                objApproval.sClientIp = sClientIP;

                bool bResult = objApproval.ApproveWFRequest(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        LoadPendingApprovalInbox();
                        ShowMsgBox("Approved Successfully");
                        cmdApprove.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        LoadAlreadyApprovedInbox();
                        ShowMsgBox("Rejected Successfully");
                        cmdApprove.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
            }
        }

        protected void grdApprovalInbox_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime dtime = new DateTime();
            string sSubject = string.Empty;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton sbutton = ((LinkButton)e.Row.FindControl("lnkRedirect"));
                    string url = ConfigurationSettings.AppSettings["MMS_Redirect_Path"].ToString();
                    if(Convert.ToString(ViewState["MMS_USID"]).Length > 0)
                    {
                        url += "ApprovalInbox/ApprovalInbox?UId=" + ViewState["MMS_USID"];
                    }
                       
                    //string url = "http://www.cescerp.com";
                    sbutton.Attributes.Add("href", url);
                    sbutton.Attributes.Add("target", "_blank");


                    LinkButton lnkApprove = (LinkButton)e.Row.FindControl("lnkApprove");
                    LinkButton lnkReject = (LinkButton)e.Row.FindControl("lnkReject");
                    LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
                    LinkButton lnkHistory = (LinkButton)e.Row.FindControl("lnkHistory");
                    LinkButton lnkRedirect = (LinkButton)e.Row.FindControl("lnkRedirect");

                    string sWFAutoId = ((Label)e.Row.FindControl("lblWFAutoId")).Text;

                    string sBOId = ((Label)e.Row.FindControl("lblBOId")).Text;
                    sSubject = ((Label)e.Row.FindControl("lblSubject")).Text;
                    string sCron = ((Label)e.Row.FindControl("lblCrOn")).Text;
                    string sRecordId = ((Label)e.Row.FindControl("lblRecordId")).Text;
                    if(sCron != "")
                    {
                        dtime = Convert.ToDateTime(sCron);
                    }
                    
                    DateTime HostedDate = Convert.ToDateTime(ConfigurationSettings.AppSettings["dHost"].ToString());

                    if (rdbAlready.Checked == true || rdbRejected.Checked == true)
                    {
                        lnkApprove.Visible = false;
                        lnkReject.Visible = false;
                        lnkModify.Visible = false;
                        lnkView.Visible = true;
                    }
                    if (rdbPending.Checked == true)
                    {
                        lnkApprove.Visible = true;
                        lnkReject.Visible = true;
                        lnkModify.Visible = true;
                        lnkView.Visible = false;

                        //From Auto Table ID
                        if (sWFAutoId != "0")
                        {
                            lnkReject.Visible = false;
                            lnkHistory.Visible = false;
                        }

                        // Check for Creator of Form
                        bool bResult = CheckFormCreatorLevel(sBOId);
                        if (bResult == true)
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }

                        if (sBOId == "26")
                        {
                            //lnkModify.Visible = false;
                        }
                        if (sBOId == "30")
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }
                        if (sBOId == "24")
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }
                        if (sBOId == "52")
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                            lnkView.Visible = false;
                        }
                        if (sBOId == "48")
                        {
                            if (objSession.RoleId == "2")
                            {
                                lnkReject.Visible = false;
                                lnkModify.Visible = false;
                                lnkView.Visible = false;
                                lnkHistory.Visible = false;
                            }
                            else
                            {
                                lnkReject.Visible = false;
                                lnkModify.Visible = false;
                                lnkView.Visible = false;
                                lnkHistory.Visible = false;
                            }

                        }
                        if (sBOId == "13" || sBOId == "15" || sBOId == "29")
                        {
                            clsApproval objApproval = new clsApproval();
                            dtime = objApproval.getWorkorder_CreatedDate(sRecordId, sBOId);

                            if (dtime < HostedDate)
                            {
                                lnkModify.Visible = true;
                                lnkApprove.Visible = true;
                                lnkReject.Visible = true;
                                lnkView.Visible = true;
                                lnkHistory.Visible = true;
                                lnkRedirect.Visible = false;
                            }
                            else
                            {
                                lnkModify.Visible = false;
                                lnkApprove.Visible = false;
                                lnkReject.Visible = false;
                                lnkView.Visible = false;
                                lnkHistory.Visible = false;
                                lnkRedirect.Visible = true;
                            }
                            
                        }

                    }

                    //For NEW DTC Commssion
                    if (sBOId == "14" && sSubject.Contains("Commissioning"))
                    {
                        Label lblFormName = (Label)e.Row.FindControl("lblFormName");
                        lblFormName.Text = "Commissioning of DTC";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.Intigration_LogError(ex.StackTrace, ex.Message, strFormCode, "grdApprovalInbox_RowDataBound", objSession.UserId +"~"+ sSubject);
            }
        }

        public bool CheckFormCreatorLevel(string sBOId)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel(sBOId, objSession.RoleId);
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFormCreatorLevel");
                return false;
            }
        }

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAlreadyApprovedInbox();
                grdApprovalInbox.Columns[7].Visible = false;
                grdApprovalInbox.Columns[10].Visible = false;
                grdApprovalInbox.Columns[11].Visible = true;
                grdApprovalInbox.Columns[12].Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rdbAlready_CheckedChanged");

            }
        }

        protected void rdbPending_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPendingApprovalInbox();
                grdApprovalInbox.Columns[10].Visible = true;
                grdApprovalInbox.Columns[11].Visible = false;
                grdApprovalInbox.Columns[12].Visible = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rdbPending_CheckedChanged");

            }
        }


        protected void rdbRejected_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadRejectedApprovedInbox();
                grdApprovalInbox.Columns[7].Visible = false;
                grdApprovalInbox.Columns[10].Visible = false;
                grdApprovalInbox.Columns[11].Visible = true;
                grdApprovalInbox.Columns[12].Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rdbRejected_CheckedChanged");

            }
        }

        public void RedirectToForm(string sBOId, string sRecordId, string sActionType, string sWFOId,
            string sWFOAutoId, string sWFDataId, string sWFInitialId, string sApproveStatus)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                clsFormValues objForm = new clsFormValues();

                //Declaration
                string sDTCId = string.Empty;
                string sApprove = string.Empty;
                string sTaskType = string.Empty;
                string sStatusFlag = string.Empty;

                string sFormName = objApproval.GetFormName(sBOId);
                sFormName = sFormName + ".aspx";

                switch (sFormName)
                {
                    case "FailureEntry.aspx":

                        objForm.sFailureId = sRecordId;
                        sDTCId = objForm.GetDTCId(objForm);

                        sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/DTCFailure/" + sFormName + "?DTCId=" + sDTCId + "&FailureId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "Enhancement.aspx":

                        objForm.sFailureId = sRecordId;
                        sDTCId = objForm.GetDTCId(objForm);

                        sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sStatusFlag = HttpUtility.UrlEncode(Genaral.UrlEncrypt("2"));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/DTCFailure/" + sFormName + "?DTCId=" + sDTCId + "&EnhanceId=" + sRecordId + "&ActionType=" + sActionType + "&StatusFalg=" + sStatusFlag, false);
                        break;
                    case "Reduction Entry.aspx":

                        objForm.sFailureId = sRecordId;
                        sDTCId = objForm.GetDTCId(objForm);

                        sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sStatusFlag = HttpUtility.UrlEncode(Genaral.UrlEncrypt("5"));
                        // sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/DTCFailure/Enhancement.aspx?DTCId=" + sDTCId + "&EnhanceId=" + sRecordId + "&ActionType=" + sActionType + "&StatusFalg=" + sStatusFlag, false);
                        break;

                    case "AGENTPENDINGSEARCH.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/OilFlow/" + sFormName + "?TransId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;


                    case "RepairerInvoiceCreate.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        string sDataId = objForm.GetWODataIdForRepairerInvoice(sRecordId, sWFOId);
                        sDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDataId));
                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/TCRepair/" + sFormName + "?Wodataid=" + sDataId + "&ActionType=" + sActionType, false);
                        break;

                    case "WorkOrder.aspx":


                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForWOFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sFailureId = objForm.sFailureId;

                            sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            // sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            // sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);
                            }
                            else
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType, false);
                            }


                        }
                        else
                        {
                            objForm.sFailureId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForWO(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "IndentCreation.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForIndentFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sWOId = objForm.sWorkOrderId;


                            sWOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sWOId + "&ActionType=" + sActionType + "&IndentId=" + sRecordId, false);
                        }
                        else
                        {
                            objForm.sWorkOrderId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForIndent(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "InvoiceCreation.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForInvoiceFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sIndentId = objForm.sIndentId;

                            sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sIndentId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            //Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sIndentId + "&ActionType=" + sActionType + "&InvoiceId=" + sRecordId, false);

                        }
                        else
                        {
                            objForm.sIndentId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForInvoiceFromIndent(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            //Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "DeCommissioning.aspx":

                        if (sWFOAutoId == "0")
                        {

                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForDecommFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sInvoiceId = objForm.sInvoiceId;


                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                objForm.GetWOnoForDTCCommission(objForm);

                                string sWOSlno = objForm.sWorkOrderId;
                                string sTCcode = objForm.sTCcode;

                                sDTCId = objForm.GetDTCIdFromWO(sWOSlno);

                                sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                                sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCcode));

                                if (sDTCId != "")
                                {
                                    sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                                    Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType + "&QryDtcId=" + sDTCId, false);
                                }
                                else
                                {

                                    Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType, false);
                                }
                                break;
                            }
                            else
                            {
                                string sFailureId = objForm.GetFailureIdFromInvoice(sInvoiceId);

                                sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType + "&ReplaceId=" + sRecordId, false);
                                break;
                            }
                        }
                        else
                        {
                            objForm.sInvoiceId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForDecommissionFromInvoice(objForm);

                            string sFailureId = objForm.GetFailureIdFromInvoice(sRecordId);
                            sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                objForm.GetWOnoForDTCCommission(objForm);

                                string sWOSlno = objForm.sWorkOrderId;
                                string sTCcode = objForm.sTCcode;

                                sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                                sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCcode));

                                Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType, false);

                                break;
                            }
                            else
                            {

                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType, false);
                                break;
                            }
                        }

                        break;
                    case "PseudoWorkOrder.aspx":

                        objForm.sFailureId = sRecordId;
                        sTaskType = objForm.GetStatusFlagForWO(objForm);

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));

                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&WFOId=" + sWFOId + "&WFOAutoId=" + sWFOAutoId, false);
                        break;

                    case "RIApprove.aspx":

                        objForm.sDecommisionId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForDecommission(objForm);

                        string sFailueId = objForm.GetFailureIdFromDecommId(sRecordId);

                        sFailueId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailueId));

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&DecommId=" + sRecordId + "&ActionType=" + sActionType + "&FailureId=" + sFailueId, false);
                        break;


                    case "CRReport.aspx":

                        objForm.sDecommisionId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForDecommission(objForm);

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&DecommId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "PseudoIndent.aspx":

                        objForm.sIndentId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForInvoiceFromIndent(objForm);

                        string sWorkOrderId = objForm.GetWorkOrderId(sRecordId);
                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sWorkOrderId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWorkOrderId));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;


                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sWorkOrderId + "&ActionType=" + sActionType + "&IndentId=" + sRecordId, false);
                        // Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "TCRepairIssue.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;


                        Response.Redirect("/TCRepair/" + sFormName + "?TransId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "StoreIndent.aspx":


                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?QryIndentId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "StoreInvoiceCreation.aspx":


                        string sStoreIndentId = objForm.GetStoreIndentIdFromWF(sWFInitialId, sWFOId);
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                        sStoreIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStoreIndentId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?QryIndentId=" + sStoreIndentId + "&ActionType=" + sActionType + "&RecordId=" + sRecordId, false);

                        break;


                    case "RecieveTransCreate.aspx":


                        string sStoreInvoiceId = objForm.GetStoreInvoiceIdFromWF(sWFOId);
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sStoreInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStoreInvoiceId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?QryInvoiceId=" + sStoreInvoiceId + "&ActionType=" + sActionType + "&RecordId=" + sRecordId, false);

                        break;

                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "RedirectToForm");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    string sResult = Genaral.DateComparision(txtFromDate.Text, txtToDate.Text, false, false);
                    if (sResult == "1")
                    {
                        ShowMsgBox("From Date  should be Lesser than To Date");
                        return;
                    }
                }

                if (rdbPending.Checked == true)
                {
                    LoadPendingApprovalInbox();
                }
                else if (rdbAlready.Checked == true)
                {
                    LoadAlreadyApprovedInbox();
                }
                else if (rdbRejected.Checked == true)
                {
                    LoadRejectedApprovedInbox();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }

        }

        protected void grdApprovalInbox_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }
    }
}