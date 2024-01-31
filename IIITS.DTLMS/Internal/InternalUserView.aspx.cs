using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.Internal
{
    public partial class InternalUserView : System.Web.UI.Page
    {
        string strFormCode = "UserView";
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
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    LoadUserDetails();
                    //AdminAccess();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }

        }

        public void LoadUserDetails()
        {
            try
            {
                clsInternalUser objUser = new clsInternalUser();
                DataTable dtUserDetails = new DataTable();
                //objUser.sFullName = sFullName;
               
                dtUserDetails = objUser.LoadUserGrid(objUser);
                if (dtUserDetails.Rows.Count > 0)
                {
                    grdInternalUser.DataSource = dtUserDetails;
                    grdInternalUser.DataBind();
                    ViewState["INTERNALUSER"] = dtUserDetails;
                }
                else
                {
                    ShowEmptyGrid();
                    ViewState["INTERNALUSER"] = dtUserDetails;               
                }
               

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadUserDetails");
            }
        }


         public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                 
                dt.Columns.Add("IU_ID");
                dt.Columns.Add("IU_FULLNAME");
                dt.Columns.Add("IU_MOBILENO");
                dt.Columns.Add("IU_DOJ");
                dt.Columns.Add("IU_USERTYPE");

                grdInternalUser.DataSource = dt;
                grdInternalUser.DataBind();

                int iColCount = grdInternalUser.Rows[0].Cells.Count;
                grdInternalUser.Rows[0].Cells.Clear();
                grdInternalUser.Rows[0].Cells.Add(new TableCell());
                grdInternalUser.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdInternalUser.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("InternalUserCreate.aspx", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNew_Click");
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


        protected void grdInternalUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdInternalUser.PageIndex = e.NewPageIndex;
                LoadUserDetails();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdUser_PageIndexChanging");

            }
        }

        protected void grdInternalUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strUserId = ((Label)row.FindControl("lblUserId")).Text;
                    strUserId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strUserId));
                    Response.Redirect("InternalUserCreate.aspx?QryUserId=" + strUserId + "", false);

                }


                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtIUserName = (TextBox)row.FindControl("txtsFullName");
                    DataTable dt = (DataTable)ViewState["INTERNALUSER"];

                    dv = dt.DefaultView;
                    if (txtIUserName.Text != "")
                    {
                        sFilter = "IU_FULLNAME Like '%" + txtIUserName.Text.ToUpper().Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdInternalUser.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdInternalUser.DataSource = dv;
                            ViewState["INTERNALUSER"] = dv.ToTable();
                            grdInternalUser.DataBind();

                        }
                        else
                        {
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadUserDetails();
                    }

                }

            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowCommand");
            }
        }
    }
}