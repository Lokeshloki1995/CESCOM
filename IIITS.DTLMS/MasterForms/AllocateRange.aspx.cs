using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Internal
{
    public partial class AllocateRange : System.Web.UI.Page
    {
        string strFormCode = "AllocateRange";
        clsSession objSession;
        //string Start_Range = string.Empty;
        //int End_Range = 0;
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
                    Genaral.Load_Combo("SELECT VM_ID,VM_NAME FROM TBLVENDORMASTER", "--Select--", cmbMake);
                    LoadPlateAllocatedRangeDetails();
                 //   Genaral.Load_Combo("SELECT VM_ID,VM_NAME FROM TBLVENDORMASTER", txtStartRange.Text);
                    clsAllocateRange objRange = new clsAllocateRange();
                    txtStartRange.Text= objRange.getmaxssplate_no();

                    txtStartRange.ReadOnly = true;
                    txtEndRange.ReadOnly = true;
                    //clsAllocateRange objRange = new clsAllocateRange();
                    //Start_Range = objRange.getmaxssplate_no();
                    //txt_Max_ssplate_no.Text = Start_Range;
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }
        protected void LoadPlateAllocatedRangeDetails(String splateNumber = "")
        {
            try
            {
                clsAllocateRange objRange = new clsAllocateRange();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                dt = objRange.GetPlateAllocatedRangeDetails(splateNumber);
                ViewState["plateRangeDetails"] = dt;
                GridAllocateRange.DataSource = dt;
                GridAllocateRange.DataBind();
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            string[] arr = new string[2];
            try
            {
                if (ValidateForm() == true)
                {
                    clsAllocateRange objRange = new clsAllocateRange();
                    objRange.sMake_Id = cmbMake.SelectedValue;
                    objRange.sStart_Range = txtStartRange.Text;
                    objRange.sEnd_Range = txtEndRange.Text;
                    objRange.sCrby = objSession.UserId;
                    objRange.sPONumber = txtPONumber.Text;
                    objRange.sPODate = txtPODate.Text;
                    objRange.sDwaNumber = txtDwaNumber.Text;
                    objRange.sDWADate = txtDWADate.Text;
                    objRange.squantity = txtcapacity.Text;

                   
                    objRange.sQty = Convert.ToInt32(objRange.sEnd_Range) - Convert.ToInt32(objRange.sStart_Range) + 1;
                    arr = objRange.SaveDetails(objRange);
                    ResetForm();
                    LoadPlateAllocatedRangeDetails();
                    ShowMsgBox(arr[0]);

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        bool ValidateForm()
        {
            bool bvalidate = false;
            if (cmbMake.SelectedIndex == 0)
            {
                cmbMake.Focus();
                ShowMsgBox("Select Vendor ");
                return bvalidate;
            }

            if (txtStartRange.Text.Trim().Length == 0)
            {
                txtStartRange.Focus();
                ShowMsgBox("Select Start Range");
                return bvalidate;
            }
            if (txtEndRange.Text.Trim().Length == 0)
            {
                txtEndRange.Focus();
                ShowMsgBox("Select End Range");
                return bvalidate;
            }
            if (Convert.ToInt32(txtStartRange.Text) > Convert.ToInt32(txtEndRange.Text))
            {
                txtStartRange.Focus();
                ShowMsgBox("End Range should be greater than Start Range");
                return bvalidate;
            }

            bvalidate = true;
            return bvalidate;
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            cmbMake.SelectedIndex = 0;
            txtcapacity.Text = "";
            txtDwaNumber.Text = "";
            txtDWADate.Text = "";
            txtPONumber.Text = "";
            txtPODate.Text = "";
          //  txtStartRange.Text = "";
            txtEndRange.Text = "";
        }

        protected void txtcapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsAllocateRange objRange = new clsAllocateRange();
                int Cap = Convert.ToInt32(txtcapacity.Text);
               string max = objRange.getmaxssplate_no();
                int maxnum = Convert.ToInt32(max);
                int endrange = maxnum + Cap-1;
                txtEndRange.Text = endrange.ToString();
               // endrange.ToString();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        protected void grdAllocateRange_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)ViewState["plateRangeDetails"];
                GridAllocateRange.PageIndex = 0;
                GridAllocateRange.PageIndex = e.NewPageIndex;
                GridAllocateRange.DataSource = dt;
                GridAllocateRange.DataBind();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        protected void grdAllocateRange_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtPlateNumber = (TextBox)row.FindControl("txtPlateNumber");
                    LoadPlateAllocatedRangeDetails(txtPlateNumber.Text);
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

            }

        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            LoadPlateAllocatedRangeDetails();
        }
    }
}