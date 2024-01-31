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
    public partial class EnumPerfView : System.Web.UI.Page
    {
        string strFormCode = "EnumerationView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE, DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "--Select--", cmbDivision);
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);
                    Genaral.Load_Combo("SELECT SM_ID, SM_NAME FROM TBLSTOREMAST ORDER BY SM_NAME", "--Select--", cmbStore);
                    Genaral.Load_Combo("SELECT TR_ID, TR_NAME FROM TBLTRANSREPAIRER ORDER BY TR_NAME", "--Select--", cmbRepairer);

                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = '1'  ORDER BY IU_FULLNAME", "--Select--", cmbOperator);
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = '3'  ORDER BY IU_FULLNAME", "--Select--", cmbSupervisor);
                    rdbLocationType.SelectedValue = "4";
                    rdbPendingQC.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == true)
            {
                LoadEnumeration();

            }
        }

        public void LoadEnumeration(string sDTCCode="",string sDTCName="",string sTCCode="",string sOperator1="",string sOperator2="")
        {
            try
            {
                clsQCApproval objQC = new clsQCApproval();

                if (rdbPendingQC.SelectedValue == "2" || rdbPendingQC.SelectedValue == "3")
                {
                    grdEnumerationDetails.Columns[9].Visible = true;
                }
                else
                {
                    grdEnumerationDetails.Columns[9].Visible = false;
                }

                if (cmbOperator.SelectedIndex > 0)
                {
                    objQC.sOperator = cmbOperator.SelectedValue;
                }

                if (cmbSupervisor.SelectedIndex > 0)
                {
                    objQC.sSupervisor = cmbSupervisor.SelectedValue;
                }

                if (cmbDivision.SelectedIndex > 0)
                {
                    objQC.sOffcode = cmbDivision.SelectedValue;
                }

                if (cmbSubdivision.SelectedIndex > 0)
                {
                    objQC.sOffcode = cmbSubdivision.SelectedValue;
                }

                if (cmbSection.SelectedIndex > 0)
                {
                    objQC.sOffcode = cmbSection.SelectedValue;
                }

                if (cmbFeeder.SelectedIndex > 0)
                {
                    objQC.sFeeder = cmbFeeder.SelectedValue;
                }

                if (cmbStore.SelectedIndex > 0)
                {
                    objQC.sStore = cmbStore.SelectedValue;
                }

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objQC.sRepairer = cmbRepairer.SelectedValue;
                }

                objQC.sPendingforQC = rdbPendingQC.SelectedValue;
                objQC.sLocationType = rdbLocationType.SelectedValue;

                if (rdbLocationType.SelectedValue == "4")
                {
                    objQC.sLocationType = "";
                }

                if (chkMobileApp.Checked == true)
                {
                    objQC.sMobileEntry = true;
                }

                objQC.sDtcCode = sDTCCode;
                objQC.sDtcName = sDTCName;
                objQC.sDtrCode = sTCCode;
                objQC.sOperator1 = sOperator1;
                objQC.sOperator2 = sOperator2;

                DataTable dt = new DataTable();
                dt = objQC.LoadEnumearionDetails(objQC);
                if (dt.Rows.Count > 0)
                {
                    grdEnumerationDetails.DataSource = dt;
                    grdEnumerationDetails.DataBind();
                    ViewState["Enum"] = dt;
                }
                else
                {
                    ShowEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadEnumeration");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (rdbPendingQC.SelectedItem.Value == "")
            {
                rdbPendingQC.Focus();
                ShowMsgBox("Please Select the QC Type");
                return false;
            }
            if (rdbLocationType.SelectedItem.Value == "")
            {
                rdbLocationType.Focus();
                ShowMsgBox("Please Select the Location Type");
                return false;
            }
                     

            bValidate = true;
            return bValidate;
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE, SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_DIV_CODE = '" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbSubdivision);
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);


                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbSubdivision.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbDivision_SelectedIndexChanged");
            }
        }

        protected void cmbSubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE, OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbSubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSection);
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE '" + cmbSubdivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbSubdivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);

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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbSubdivision_SelectedIndexChanged");
            }
        }

        protected void grdEnumerationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "Submit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                    Label lblEnumDetailsId = (Label)row.FindControl("lblEnumDetailsId");
                    Label lblEnumType = (Label)row.FindControl("lblEnumType");
                    Label lblStatusFlag = (Label)row.FindControl("lblStatusFlag");

                    string sEnumDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnumDetailsId.Text));
                    string sEnumType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnumType.Text));
                    string sStatusFlag = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblStatusFlag.Text));

                    if (lblEnumType.Text == "2")
                    {
                        Response.Redirect("FieldEnum.aspx?QryEnumId=" + sEnumDetailsId +"&Status="+ sStatusFlag,false );
                    }
                    else
                    {
                        Response.Redirect("StoreEnumeration.aspx?QryEnumId=" + sEnumDetailsId + "&Status=" + sStatusFlag,false);
                    }
                }
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sEnumDetailsId = ((Label)row.FindControl("lblEnumDetailsId")).Text;
                    string sStatusFlag = ((Label)row.FindControl("lblStatusFlag")).Text;

                    clsQCApproval objQC = new clsQCApproval();
                    objQC.sEnumDetailsId = sEnumDetailsId;
                    objQC.sStatusFlag = sStatusFlag;
                    txtRemark.Text = objQC.GetPendingRejectRemarks(objQC);

                    mdlPopup.Show();
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDTCcode = (TextBox)row.FindControl("txtDTCcode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtTCcode = (TextBox)row.FindControl("txtTCcode");
                    TextBox txtOperator1 = (TextBox)row.FindControl("txtOperator1");
                    TextBox txtOperator2 = (TextBox)row.FindControl("txtOperator2");

                    LoadEnumeration(txtDTCcode.Text.Replace("'", "`"), txtDTCName.Text.ToUpper().Replace("'", "`"),
                        txtTCcode.Text.Replace("'", "`"), txtOperator1.Text.ToUpper().Replace("'", "`"), txtOperator2.Text.ToUpper().Replace("'", "`"));

                    //DataTable dt = (DataTable)ViewState["Enum"];
                    //dv = dt.DefaultView;
                    //if (txtDTCcode.Text != "")
                    //{
                    //    sFilter = "DTE_DTCCODE Like '%" + txtDTCcode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtDTCName.Text != "")
                    //{
                    //    sFilter = "DTE_NAME Like '%" + txtDTCName.Text.ToUpper().Replace("'", "`") + "%' AND";
                    //}
                    //if (txtTCcode.Text != "")
                    //{
                    //    sFilter = "DTE_TC_CODE Like '%" + txtTCcode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtOperator1.Text != "")
                    //{
                    //    sFilter += " OPERATOR1 Like '%" + txtOperator1.Text.ToUpper().Replace("'", "`") + "%' AND";
                    //}
                    //if (txtOperator2.Text != "")
                    //{
                    //    sFilter += " OPERATOR2 Like '%" + txtOperator2.Text.ToUpper().Replace("'", "`") + "%' AND";
                    //}
                    //if (sFilter.Length > 0)
                    //{
                    //    sFilter = sFilter.Remove(sFilter.Length - 3);
                    //    grdEnumerationDetails.PageIndex = 0;
                    //    dv.RowFilter = sFilter;
                    //    if (dv.Count > 0)
                    //    {
                    //        grdEnumerationDetails.DataSource = dv;
                    //        ViewState["Enum"] = dv.ToTable();
                    //        grdEnumerationDetails.DataBind();

                    //    }
                    //    else
                    //    {

                    //        ShowEmptyGrid();
                    //    }
                    //}
                    //else
                    //{
                    //    cmdLoad_Click(sender, e);

                    //}
                }

              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdEnumerationDetails_RowCommand");
            }

        }

        protected void grdEnumerationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdEnumerationDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Enum"];
                grdEnumerationDetails.DataSource = dt;
                grdEnumerationDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdEnumerationDetails_PageIndexChanging");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("ED_ID");
                dt.Columns.Add("TYPE");
                dt.Columns.Add("DTE_DTCCODE");
                dt.Columns.Add("DTE_NAME");
                dt.Columns.Add("DTE_TC_CODE");
                dt.Columns.Add("OPERATOR1");
                dt.Columns.Add("OPERATOR2");
                dt.Columns.Add("SUPERVISOR");
                dt.Columns.Add("ED_LOCTYPE");
                dt.Columns.Add("ED_STATUS_FLAG");

                grdEnumerationDetails.DataSource = dt;
                grdEnumerationDetails.DataBind();

                int iColCount = grdEnumerationDetails.Rows[0].Cells.Count;
                grdEnumerationDetails.Rows[0].Cells.Clear();
                grdEnumerationDetails.Rows[0].Cells.Add(new TableCell());
                grdEnumerationDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdEnumerationDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

     
    }
}