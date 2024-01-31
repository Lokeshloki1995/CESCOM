using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;


namespace IIITS.DTLMS
{
    public partial class ContactUs : System.Web.UI.Page
    {
        string strFormCode = "ContactUs.aspx";
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
                   
                   LoadGrid();
                   // LoadDesignationGrid();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadDesignationGrid()
        {
            try
            {
                DataSet set = new DataSet();
                clsContactUs objDet = new clsContactUs();
              
                set = objDet.LoadDetails();
                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1)
                    {
                        DataTable t1Again = set.Tables[i];
                        grdFirstContactDetails.DataSource = t1Again;
                        grdFirstContactDetails.DataBind();
                    }
                    if (i == 2)
                    {
                        DataTable t2Again = set.Tables[i];
                        grdSecondContactDetails.DataSource = t2Again;
                        grdSecondContactDetails.DataBind();
                    }
                    if (i == 3)
                    {
                        DataTable t3Again = set.Tables[i];
                        grdThirdGrid.DataSource = t3Again;
                        grdThirdGrid.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDesignationGrid");
            }
        }


        public void LoadGrid() 
        {
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                clsContactUs objDesgn = new clsContactUs();

                dt1 = objDesgn.LoadDetailsForFirstGrid();
                if (dt1.Rows.Count > 0)
                {
                    grdFirstContactDetails.DataSource = dt1;
                    grdFirstContactDetails.DataBind();
                    ViewState["FirstContactDetails"] = dt1;
                }
                dt2 = objDesgn.LoadDetailsForSecondGrid();
                if (dt2.Rows.Count > 0)
                {

                    grdSecondContactDetails.DataSource = dt2;
                    grdSecondContactDetails.DataBind();
                    ViewState["SecondContactDetails"] = dt2;
                }
                dt3 = objDesgn.LoadDetailsForThirdGrid();
                if (dt3.Rows.Count > 0)
                {

                    grdThirdGrid.DataSource = dt3;
                    grdThirdGrid.DataBind();
                    ViewState["ThirdContactDetails"] = dt3;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadGrid");
            }
        }

        protected void lknLoginPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["FullName"] != null && Session["FullName"].ToString() != "")
                {
                    Response.Redirect("Dashboard.aspx", false);
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lknLoginPage_Click");
            }
        }

        protected void lnkOSTicket_Click(object sender, EventArgs e)
        {

            string userId = string.Empty;
            string UserName = string.Empty;
            string LoginName = string.Empty;
            string Email = string.Empty;
            string MONO = string.Empty;
            string PrjId = string.Empty;
            string roleName = string.Empty;
            string zone = string.Empty;
            string circle = string.Empty;
            string division = string.Empty;
            string subdivision = string.Empty;
            string section = string.Empty;
            string officeCode = string.Empty;
            string iticketRedirectPath = string.Empty;
            string url1 = string.Empty;

            var page = HttpContext.Current.Handler as Page;
            objSession = (clsSession)Session["clsSession"];
            clsUser objUsrDetail = new clsUser();
            objUsrDetail.lSlNo = objSession.UserId;
            objUsrDetail.GetUserDetails(objUsrDetail);

            if (ConfigurationManager.AppSettings["ITICKETNEWCHANGES"] == "NO")
            {
                userId = Genaral.Encrypt(objSession.UserId);
                UserName = Genaral.Encrypt(objUsrDetail.sFullName);
                Email = Genaral.Encrypt(objUsrDetail.sEmail);
                MONO = Genaral.Encrypt(objUsrDetail.sMobileNo);
                PrjId = Genaral.Encrypt("CESC DTLMS");
                roleName = Genaral.Encrypt(objUsrDetail.sRole);
                zone = Genaral.Encrypt(objUsrDetail.sZone);
                circle = Genaral.Encrypt(objUsrDetail.sCircle);
                division = Genaral.Encrypt(objUsrDetail.sDivision);
                subdivision = Genaral.Encrypt(objUsrDetail.ssubDivision);
                section = Genaral.Encrypt(objUsrDetail.sSection);
                officeCode = Genaral.Encrypt(objUsrDetail.sOfficeCode);

                iticketRedirectPath = ConfigurationManager.AppSettings["Iticket_url_path"].ToString();
                url1 = iticketRedirectPath + "?UserId=" + userId + "&UserName=" +
                    UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId +
                    "&RoleName=" + roleName + "&Zone=" + zone + "&Circle=" + circle + "&Division=" +
                    division + "&SubDivision=" + subdivision + "&Section=" + section + "&OfficeCode=" + officeCode;

            }
            else
            {
                userId = Genaral.EncryptTckt(objSession.UserId,"");
                UserName = Genaral.EncryptTckt(objUsrDetail.sFullName,"");
                LoginName = Genaral.EncryptTckt(objUsrDetail.sLoginName, "");
                Email = Genaral.EncryptTckt(objUsrDetail.sEmail,"");
                MONO = Genaral.EncryptTckt(objUsrDetail.sMobileNo,"");
                PrjId = Genaral.EncryptTckt(Convert.ToString(ConfigurationManager.AppSettings["ITICKETENCRYPTDECRPTKEY"]),"");
                roleName = Genaral.EncryptTckt(objUsrDetail.sRole,"");
                officeCode = Genaral.EncryptTckt(objUsrDetail.sOfficeCode,"");

                iticketRedirectPath = ConfigurationManager.AppSettings["Iticket_url_path"].ToString();
                url1 = iticketRedirectPath + "?UserName=" + LoginName + "&Email=" + Email
                    + "&ProductSuffix=" + PrjId + "&MobileNumber=" + MONO + "&RoleName=" + roleName
                    + "&OfficeCode=" + officeCode + "&UserId=" + userId + "&Name=" + UserName + " ";

                // changes done for i ticket application on 21-08-2023
                //iticketRedirectPath = ConfigurationManager.AppSettings["Iticket_url_path"].ToString();
                //url1 = iticketRedirectPath + "?UserName=" + UserName + "&Email=" + Email
                //    + "&ProductSuffix=" + PrjId + "&MobileNumber=" + MONO + "&RoleName=" + roleName + "&OfficeCode=" + officeCode + " ";
            }
            string s = "window.open('" + url1 + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            #region
            
            //Response.Redirect("http://192.168.4.47:999/Ticket/DTLMSApplication?UserId=" + userId + "&UserName=" + UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId, false);

            //string iticketRedirectPath = ConfigurationManager.AppSettings["Iticket_url_path"].ToString();
            //string url1 = iticketRedirectPath + "Ticket/DTLMSApplication?UserId=" + userId + "&UserName=" + UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId + "&RoleName=" + roleName + "&Zone=" + zone + "&Circle=" + circle + "&Division=" + division + "&SubDivision=" + subdivision + "&Section=" + section + "&OfficeCode=" + officeCode;

            ////string url = "http://iticket.co.in/Ticket/DTLMSApplication?UserId=" + userId + "&UserName=" + UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId+"&RoleName="+roleName+"&Zone="+zone+"&Circle="+circle+"&Division="+division+"&SubDivision="+subdivision+"&Section="+section+"&OfficeCode="+officeCode ;
            //string s = "window.open('" + url1 + "','_blank');"; // '_newtab'
            //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            #endregion
        }

        protected void firstLevelPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFirstContactDetails.PageIndex = e.NewPageIndex;
                grdFirstContactDetails.DataSource = ViewState["FirstContactDetails"];
                grdFirstContactDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "firstLevelPageIndexChanging");
            }
        }
        protected void secondLevelPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdSecondContactDetails.PageIndex = e.NewPageIndex;
                grdSecondContactDetails.DataSource = ViewState["SecondContactDetails"];
                grdSecondContactDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "secondLevelPageIndexChanging");
            }
        }
        protected void thirdLevelPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdThirdGrid.PageIndex = e.NewPageIndex;
                grdThirdGrid.DataSource = ViewState["ThirdContactDetails"];
                grdThirdGrid.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "thirdLevelPageIndexChanging");
            }
        }


    }
}