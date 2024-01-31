using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using IIITS.DTLMS.BL;
using System.Web.Security;
using System.Data;
using IIITS.DTLMS.BL.MasterForms;
using System.IO;

namespace IIITS.DTLMS
{
    public partial class DTLMS : System.Web.UI.MasterPage
    {
        clsSession objSession = new clsSession();
        string strFormCode = "DTLMS.Master";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
                {
                    //if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                    //{
                    //   // Response.Redirect("~/Login.aspx", false);
                    //}
                    //else

                    if (!IsPostBack)
                    {
                        if (Session["FullName"] != null && Session["FullName"].ToString() != "")
                        {
                            lblUserName.Text = "Welcome " + Session["FullName"].ToString();
                            clsSession objSession = (clsSession)Session["clsSession"];

                            string strAdminRole = string.Empty;
                            strAdminRole = objSession.RoleId;
                            if (strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["AdminRole"]) || strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["SupAdminRole"]) || strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["TrackLead"]))
                            {
                                liAdminActivities.Visible = true;
                                liKanAdminActivities.Visible = true;
                            }
                            else
                            {
                                liAdminActivities.Visible = false;
                                liKanAdminActivities.Visible = false;
                            }

                            if (objSession.OfficeCode != "" && objSession.OfficeCode != "0")
                            {
                                // lblOfficeName.Text = objSession.OfficeName + " [ " + objSession.OfficeCode + " ]";
                                lblOfficeName.Text = objSession.OfficeNameWithType;
                            }
                            else
                            {
                                lblOfficeName.Text = objSession.OfficeName;
                            }

                            //coded by sandeep for common login

                            clsUser objuser = new clsUser();
                            Version_Id.Text = objuser.getVersion();

                            if (objSession.UserType != null)
                            {
                                InternalUserLogin(objSession.UserType, objSession.UserId);
                            }
                            if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                            {
                                liDashboard.Style.Add("display", "none");
                                liMaster.Style.Add("display", "none");
                                liFailure.Style.Add("display", "none");
                                liRepair.Style.Add("display", "none");
                                liScrap.Style.Add("display", "none");
                                liMaintainance.Style.Add("display", "none");
                                liInterStore.Style.Add("display", "none");
                                liUser.Style.Add("display", "none");
                                liApproval.Style.Add("display", "none");
                                liTransaction.Style.Add("display", "none");
                                liOffDesg.Style.Add("display", "none");
                                // liLocMaster.Style.Add("display", "none");
                                liMainReport.Style.Add("display", "none");
                                liMISReports.Style.Add("display", "none");
                                liEngVideoTutorials.Style.Add("display", "none");
                                li2.Style.Add("display", "none");
                                li1.Style.Add("display", "none");
                                liFeederMaster.Style.Add("display", "none");
                                //liMismatch.Style.Add("display", "none");
                                //liAdminActivities.Style.Add("display", "none");
                            }
                            if (objSession.sProjectList != "" && objSession.sProjectList != null)
                            {
                                LoadCommonLoginProjectList();
                                dvhead.Style.Add("display", "block");
                                liUserManagement.Style.Add("display", "none");
                                string ChangePasswordFlag = Session["ChangePasswordFlag"].ToString();

                                if (ChangePasswordFlag == "1")
                                {
                                    liDashboard.Style.Add("display", "block");
                                    liMaster.Style.Add("display", "block");
                                    liFailure.Style.Add("display", "block");
                                    liRepair.Style.Add("display", "block");
                                    liScrap.Style.Add("display", "block");
                                    liMaintainance.Style.Add("display", "block");
                                    liInterStore.Style.Add("display", "block");
                                    liUser.Style.Add("display", "block");
                                    liApproval.Style.Add("display", "block");
                                    liTransaction.Style.Add("display", "block");
                                    liOffDesg.Style.Add("display", "block");
                                    liMainReport.Style.Add("display", "block");
                                    liMISReports.Style.Add("display", "block");
                                    liEngVideoTutorials.Style.Add("display", "block");
                                    li1.Style.Add("display", "block");
                                }
                                lblDesign.Text = objSession.Designation;
                                LoadUserDetails();
                            }
                            else
                            {
                                lblDesign.Text = objSession.Designation;
                            }
                        }

                    }


                    liDashboard.Visible = true;
                    liKanDashboard.Visible = false;
                    liKanUser.Visible = false;
                    liKanMaster.Visible = false;
                    //liKanMismatch.Visible = false;
                    liKanFailure.Visible = false;
                    liKanRepair.Visible = false;
                    liKanScrap.Visible = false;
                    liKanMaintainance.Visible = false;
                    liKanInterStore.Visible = false;
                    liKanApproval.Visible = false;
                    liKanTransaction.Visible = false;
                    //liKanLocMaster.Visible = false;
                    liKanMainReport.Visible = false;
                    liKan1.Visible = false;
                    liKanOffDesg.Visible = false;
                    liKanAdminActivities.Visible = false;
                    liKanMISReports.Visible = false;
                    //}


                }
                else
                {

                    Response.Redirect("~/Login.aspx", false);
                }

            }
            catch (Exception ex)
            {

            }
        }



        protected void lnkLogout_Click(object sender, EventArgs e)
        {

            try
            {

                //  to be commented  
                int cookieCount = Request.Cookies.Count;
                FormsAuthentication.SignOut();
                for (var i = 0; i < cookieCount; i++)
                {

                    var cookie = Request.Cookies[i];
                    if (cookie != null)
                    {
                        System.IO.File.AppendAllText("C:\\DTLMS_ERRORLOG\\202002\\note.txt", "logout click :   " + cookie.Name + Environment.NewLine);

                        var expiredCookie = new HttpCookie(cookie.Name)
                        {
                            Expires = DateTime.Now.AddDays(-30),
                            Domain = cookie.Domain,
                            Value = ""
                        };
                        Response.Cookies.Add(expiredCookie);

                        // overwrite it
                    }

                }
                //  to be commented  
                string clientIp = Genaral.GetClientIp();
                if (Session["ProjectType"] != null)
                {
                    if (Session["ProjectType"].ToString() == "MMS")
                    {
                        string url = ConfigurationSettings.AppSettings["MMS_Redirect_Path"].ToString() + "Account/LogOut_From_DTLMS?PId=" + Genaral.EncryptMMS("DTLMS");
                        string s = "window.open('" + url + "', '_blank');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                        ViewState["ProjectType"] = "MMS";
                    }
                }


                for (int i = 0; i < Session.Contents.Count; i++)
                {
                    var key = Session.Keys[i];
                    var value = Session[i];

                    if (value != null)
                    {
                        if (value.ToString() != "MMS")
                        {
                            Session.Remove(key);
                            i--;
                        }
                    }
                    throw new Exception();
                }

                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                    // Response.Cookies["ASP.NET_SessionId"].Path = "/MyApp";
                }

                if (Request.Cookies["AuthToken"] != null)
                {
                    Response.Cookies["AuthToken"].Value = string.Empty;
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                    //Response.Cookies["AuthToken"].Path = "/MyApp";
                }
                if (!liOffDesg.Style.Value.Contains("none"))
                {
                    //    if (ViewState["ProjectType"].ToString() == "MMS")
                    //    {
                    //        //Session["ProjectType"] = "MMS";
                    //        //Session.Add("ProjectType", "MMS");
                    //    }


                    Genaral.GeneralLog(clientIp, objSession.UserId, "LOGOUT");
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                    {
                        Genaral.GeneralLog(clientIp, objSession.UserId, "LOGOUT");
                        Response.Redirect("~/Login.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("~/InternalLogin.aspx", false);
                    }

                }

            }
            catch (Exception ex)
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                if (Convert.ToString(ConfigurationSettings.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                {
                    string Logouturl = ConfigurationSettings.AppSettings["CommonLogoutPath"].ToString();
                    Response.Redirect(Logouturl, false);
                }
                else
                {
                    Response.Redirect("~/Login.aspx", false);
                }

            }
        }

        public void InternalUserLogin(string sUserType, string sUserId)
        {
            try
            {
                liDashboard.Style.Add("display", "none");
                liMaster.Style.Add("display", "none");
                // liMismatch.Style.Add("display", "none");
                liFailure.Style.Add("display", "none");
                liRepair.Style.Add("display", "none");
                liScrap.Style.Add("display", "none");
                liMaintainance.Style.Add("display", "none");
                liInterStore.Style.Add("display", "none");
                liUser.Style.Add("display", "none");
                liApproval.Style.Add("display", "none");
                liTransaction.Style.Add("display", "none");
                liOffDesg.Style.Add("display", "none");
                //liLocMaster.Style.Add("display", "none");
                liMainReport.Style.Add("display", "none");
                liMISReports.Style.Add("display", "none");
                liEngVideoTutorials.Style.Add("display", "none");

                liInterDash.Style.Add("display", "block");
                liInterReports.Style.Add("display", "block");
                //liInterUser.Style.Add("display", "block");


                //QC Executive
                if (sUserType == "2")
                {
                    liQC.Style.Add("display", "block");
                }

                //Operator and Supervisor
                if (sUserType == "1" || sUserType == "3" || sUserType == "5")
                {
                    liEnumeration.Style.Add("display", "block");
                }

                // Internal Admin
                if (sUserType == "4")
                {
                    liQC.Style.Add("display", "block");
                    liEnumeration.Style.Add("display", "block");
                    liInterUser.Style.Add("display", "block");
                    liFeeder.Style.Add("display", "block");

                    if (sUserId == "7")
                    {
                        liEnumStage.Style.Add("display", "block");
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void rdbEnglish_CheckedChanged(object sender, EventArgs e)
        {
            //  rdbKannada.Checked = false;
            //  rdbEnglish.Checked = true;
            Session["Lang"] = "English";

            //RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            Response.Redirect("~/Dashboard.aspx", false);

        }

        protected void rdbKannada_CheckedChanged(object sender, EventArgs e)
        {
            //  rdbEnglish.Checked = false;
            //  rdbKannada.Checked = true;
            Session["Lang"] = "Kannada";
            Response.Redirect("~/Dashboard.aspx", false);
            //Response.Redirect("DashboardKan.aspx?Rdbval=rdbKannada");

        }

        public void LoadCommonLoginProjectList()
        {
            try
            {
                clsSession objSession = (clsSession)Session["clsSession"];
                clsIncharge objSwapUser = new clsIncharge();
                DataTable dt = new DataTable();

                string[] strArray = objSession.sProjectList.Split('|');

                dt.Columns.Add("Project_List", System.Type.GetType("System.String"));
                dt.Columns.Add("User_id", System.Type.GetType("System.String"));
                dt.Columns.Add("Location", System.Type.GetType("System.String"));
                dt.Columns.Add("Role_id", System.Type.GetType("System.String"));
                dt.Columns.Add("ModuleURl", System.Type.GetType("System.String"));
                foreach (string user in strArray)
                {
                    DataRow drow = dt.NewRow();
                    string[] Fields = user.Split(',');


                    drow[0] = Fields[0].ToString();
                    drow[1] = Fields[1].ToString();
                    drow[2] = Fields[2].ToString();
                    drow[3] = Fields[3].ToString();
                    drow[4] = Fields[4].ToString();
                    dt.Rows.Add(drow);
                }

                Load_Dropdown(dt, cmbProjectlist);
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCommonLoginProjectList");
            }
        }

        public void Load_Dropdown(DataTable dt, System.Web.UI.WebControls.DropDownList cmb)
        {
            DataTableReader reader1 = dt.CreateDataReader();
            try
            {
                cmb.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dRow in dt.Rows)
                    {
                        System.Web.UI.WebControls.ListItem itm = new System.Web.UI.WebControls.ListItem();
                        //itm.Text = Convert.ToString(dRow[1]);
                        itm.Value = Convert.ToString(dRow[0]);
                        //itm.Attributes.Add("data-icon", "glyphicon-heart");
                        cmb.Items.Add(itm);
                        cmb.Items.Remove("DTLMS");
                        if (itm.Value == "HRMS")
                        {
                            Session["HRMSUsID"] = Convert.ToString(dRow[1]);
                        }
                    }
                    cmb.Items.Insert(0, "DTLMS");
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Load_Dropdown");
            }
        }

        protected void lnkSwap_Click(object sender, EventArgs e)
        {
            try
            {
                LoadOfficeGrid();
                this.SwapPopUp.Show();

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkSwap_Click");
            }
        }


        public void grdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                clsSession objSession = (clsSession)Session["clsSession"];

                if (e.CommandName == "submit")
                {

                    clsIncharge objInchargesession = new clsIncharge();
                    objInchargesession.sUserId = objSession.UserId;
                    objInchargesession.sFullName = objSession.FullName;
                    objInchargesession.sRoleId = objSession.RoleId;
                    objInchargesession.sOfficeCode = objSession.OfficeCode;
                    objInchargesession.sOfficeName = objSession.OfficeName;
                    objInchargesession.sDesignation = objSession.Designation;
                    objInchargesession.sOfficeNamewithType = objSession.OfficeNameWithType;
                    objInchargesession.sGeneralLog = objSession.sGeneralLog;
                    objInchargesession.sTransactionLog = objSession.sTransactionLog;
                    objInchargesession.sPasswordChangeRequest = objSession.sPasswordChangeRequest;
                    objInchargesession.sPasswordChangeInDays = objSession.sPasswordChangeInDays;
                    objInchargesession.sPassordAcceptance = objSession.sPassordAcceptance;



                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sOffCode = ((Label)row.FindControl("lbloffcode")).Text;
                    string soffName = ((Label)row.FindControl("lbllocation")).Text;
                    string sDesignation = ((Label)row.FindControl("lbldesignation")).Text;
                    string sRoleid = ((Label)row.FindControl("lblroleid")).Text;

                    lblOfficeName.Text = soffName.Split('~').GetValue(0).ToString();
                    lblDesign.Text = sDesignation;
                    objInchargesession.sRoleId = sRoleid;
                    objInchargesession.sDesignation = sDesignation;


                    objSession.RoleId = objInchargesession.sRoleId;
                    objSession.Designation = objInchargesession.sDesignation;
                    objSession.UserId = objInchargesession.sUserId;
                    objSession.FullName = objInchargesession.sFullName;
                    objSession.OfficeCode = objInchargesession.sOfficeCode;
                    objSession.OfficeName = objInchargesession.sOfficeName;
                    objSession.OfficeNameWithType = objInchargesession.sOfficeNamewithType;
                    objSession.sGeneralLog = objInchargesession.sGeneralLog;
                    objSession.sTransactionLog = objInchargesession.sTransactionLog;
                    objSession.sPasswordChangeRequest = objInchargesession.sPasswordChangeRequest;
                    objSession.sPasswordChangeInDays = objInchargesession.sPasswordChangeInDays;
                    objSession.sPassordAcceptance = objInchargesession.sPassordAcceptance;


                    Session["clsSession"] = objSession;
                    Response.Redirect("~/Dashboard.aspx", false);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdOffices_RowCommand");
            }
        }

        protected void grdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdOffices.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Office"];
                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                this.SwapPopUp.Show();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdOffices_PageIndexChanging");
            }
        }
        public void LoadOfficeGrid(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                clsSession objSession = (clsSession)Session["clsSession"];
                clsIncharge objSwapUser = new clsIncharge();
                DataTable dt = new DataTable();

                objSwapUser.sUserId = objSession.UserId;
                objSwapUser.sOfficeCode = sOfficeCode;
                objSwapUser.sofficeName = sOffName;

                dt = objSwapUser.LoadOfficeDet(objSwapUser);
                if (dt.Rows.Count > 0)
                {
                    grdOffices.DataSource = dt;
                    grdOffices.DataBind();
                    ViewState["Office"] = dt;
                }
                else
                {
                    ShowEmptyGrid();
                }

            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadOfficeGrid");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("INCHARGE_TYPE");
                dt.Columns.Add("LOCATION");
                dt.Columns.Add("IOML_ID");
                dt.Columns.Add("DM_NAME");
                dt.Columns.Add("RO_NAME");
                dt.Columns.Add("USER_NAME");
                dt.Columns.Add("FROM_DATE");
                dt.Columns.Add("TO_DATE");
                dt.Columns.Add("RO_ID");
                dt.Columns.Add("OFF_CODE");


                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                int iColCount = grdOffices.Rows[0].Cells.Count;
                grdOffices.Rows[0].Cells.Clear();
                grdOffices.Rows[0].Cells.Add(new TableCell());
                grdOffices.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdOffices.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }
        public void LoadUserDetails()
        {
            try
            {
                clsSession objSession = (clsSession)Session["clsSession"];
                clsIncharge objSwapUser = new clsIncharge();
                DataTable dt = new DataTable();

                objSwapUser.sUserId = objSession.UserId;

                dt = objSwapUser.LoadOfficeDet(objSwapUser);
                if (dt.Rows.Count > 1)
                {
                    dvSwap.Style.Add("display", "block");
                }

            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadOfficeGrid");
            }
        }


        protected void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sFolderPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            clsSession objSession = (clsSession)Session["clsSession"];
            try
            {

                string Userid = string.Empty;
                string LocationCode = string.Empty;
                string RoleId = string.Empty;
                string ModuleURl = string.Empty;

                string ProjectList = Genaral.ERPUrlEncrypt(objSession.sProjectList);
                string ChangePasswordFlag = Genaral.ERPUrlEncrypt(Session["ChangePasswordFlag"].ToString());
                string[] Projects = objSession.sProjectList.Split('|');

                foreach (var project in Projects)
                {
                    if (project.Contains(cmbProjectlist.SelectedValue))
                    {
                        string[] ProjectsDetails = project.Split(',');
                        Userid = Genaral.ERPUrlEncrypt(ProjectsDetails[1]);
                        LocationCode = Genaral.ERPUrlEncrypt(ProjectsDetails[2]);
                        RoleId = Genaral.ERPUrlEncrypt(ProjectsDetails[3]);
                        ModuleURl = ProjectsDetails[4];

                    }
                }
                Session["clsSession"] = null;
                if (cmbProjectlist.SelectedValue == "FMS")
                {

                    Response.Redirect(ModuleURl + "Userid=" + Userid + "&LocationCode=" + LocationCode + "&RoleId=" + RoleId + "&ModuleName=" + ProjectList + "&ChangePasswordFlag=" + ChangePasswordFlag, false);
                    File.AppendAllText(sPath, " DTLMS " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " URL : " + ModuleURl + Environment.NewLine + " ##############################################################" + Environment.NewLine);

                }
                else if (cmbProjectlist.SelectedValue == "TRM")
                {
                    Response.Redirect(ModuleURl + "Userid=" + Userid + "&LocationCode=" + LocationCode + "&RoleId=" + RoleId + "&ModuleName=" + ProjectList + "&ChangePasswordFlag=" + ChangePasswordFlag, false);

                }
                else if (cmbProjectlist.SelectedValue == "DTLMS")
                {
                    Response.Redirect(ModuleURl + "Userid=" + Userid + "&LocationCode=" + LocationCode + "&RoleId=" + RoleId + "&ModuleName=" + ProjectList + "&ChangePasswordFlag=" + ChangePasswordFlag, false);

                }
                else if (cmbProjectlist.SelectedValue == "MMS")
                {
                    Response.Redirect(ModuleURl + "Userid=" + Userid + "&LocationCode=" + LocationCode + "&RoleId=" + RoleId + "&ModuleName=" + ProjectList + "&ChangePasswordFlag=" + ChangePasswordFlag, false);

                }
                else if (cmbProjectlist.SelectedValue == "HRMS")
                {
                    Response.Redirect(ModuleURl + "Userid=" + Userid + "&LocationCode=" + LocationCode + "&RoleId=" + RoleId + "&ModuleName=" + ProjectList + "&ChangePasswordFlag=" + ChangePasswordFlag, false);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCommonLoginProjectList");
            }
        }
    }
}