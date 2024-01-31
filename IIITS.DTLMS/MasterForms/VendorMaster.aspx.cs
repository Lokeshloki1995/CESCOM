using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.Internal
{
    public partial class VendorMaster : System.Web.UI.Page
    {
        string strFormCode = "VendorMaster";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] Delete_Session_array = new string[7];
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (Session["arrSave_ImageSession_String"] != null)
                {
                    Delete_Session_array = Session["arrSave_ImageSession_String"] as string[];
                    for (int i = 0; i < 7; i++)
                    {
                        if (Delete_Session_array[i] != "")
                        {
                            Session.Remove(Delete_Session_array[i]);
                        }
                    }
                    Session.Remove("arrSave_ImageSession_String");
                }

                if (!IsPostBack)
                {
                    clsVendorMaster objVendor = new clsVendorMaster();
                    DataTable dtVendorDetails = objVendor.LoadVendorDetails();
                    ViewState["VendorDetails"] = dtVendorDetails;
                    grdVendorDetails.DataSource = dtVendorDetails;
                    grdVendorDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            string res = string.Empty;
            try
            {
                if (Validate()==true)
                {
                    clsVendorMaster objVendor = new clsVendorMaster();

                    objVendor.sVendorName = txtFullName.Text;
                    objVendor.sVendorNumber = txtMobile.Text;
                    objVendor.sVendorId = tblVmId.Text;
                    if (objSession != null)
                    {
                        objVendor.sCrBy = objSession.UserId;
                    }

                    if (cmdSave.Text == "Save")
                    {
                        res = objVendor.save_Vendor_Details(objVendor);
                        if (res == "1")
                        {
                            ShowMsgBox("Saved Succesfully");
                        }
                    }
                    else if (cmdSave.Text == "UPDATE")
                    {
                        res = objVendor.update_Vendor_Details(objVendor);
                        if (res == "1")
                        {
                            ShowMsgBox("Updated Succesfully");
                        }
                    }
                    DataTable dtVendorDetails = objVendor.LoadVendorDetails();
                    ViewState["VendorDetails"] = dtVendorDetails;
                    grdVendorDetails.DataSource = dtVendorDetails;
                    grdVendorDetails.DataBind();
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFullName.Text = "";
                txtMobile.Text = "";

                clsVendorMaster objVendor = new clsVendorMaster();
                DataTable dtVendorDetails = objVendor.LoadVendorDetails();
                ViewState["VendorDetails"] = dtVendorDetails;
                grdVendorDetails.DataSource = dtVendorDetails;
                grdVendorDetails.DataBind();
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
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

        public bool Validate()
        {
            if (txtFullName.Text == "")
            {
                ShowMsgBox("Enter Vendor Name");
                return false;
            }
            else if (txtMobile.Text == "")
            {
                ShowMsgBox("Enter Mobile Number");
                return false;
            }
            else
                return true;
        }

        protected void grdVendorDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtVendorDetails = new DataTable();
            try
            {
                if (e.CommandName == "edit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                    Label lblVendorId = (Label)row.FindControl("lblVendorId");
                    //Label lblEnumType = (Label)row.FindControl("lblEnumType");
                    //Label lblStatusFlag = (Label)row.FindControl("lblStatusFlag");

                    string sVendorId = lblVendorId.Text;
                    tblVmId.Text = sVendorId;
                    clsVendorMaster objVendor = new clsVendorMaster();
                    dtVendorDetails = objVendor.GetVendorDetails(sVendorId);
                    txtFullName.Text = dtVendorDetails.Rows[0]["VM_NAME"].ToString();
                    txtMobile.Text = dtVendorDetails.Rows[0]["VM_MOBILE_NUM"].ToString();
                    cmdSave.Text = "UPDATE";
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataTable dtVendorDetails1 = new DataTable();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtVmName = (TextBox)row.FindControl("txtVmName");

                    clsVendorMaster objVendor = new clsVendorMaster();
                    dtVendorDetails1 = objVendor.LoadVendorDetails(txtVmName.Text);
                    grdVendorDetails.DataSource = dtVendorDetails1;
                    grdVendorDetails.DataBind();
                    
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdVendorDetails_RowCommand");
            }
        }

        protected void grdVendorDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdVendorDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["VendorDetails"];
                grdVendorDetails.DataSource = dt;
                grdVendorDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdVendorDetails_PageIndexChanging");
            }
        }

        protected void grdVendorDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}