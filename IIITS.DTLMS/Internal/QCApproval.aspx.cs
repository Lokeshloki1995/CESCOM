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
    public partial class QCApproval : System.Web.UI.Page
    {
        string strFormCode = "QCApproval";
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
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {

                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "--Select--", cmbDivision);
                  
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
            try
            {
                if (ValidateForm() == true)
                {
                    LoadEnumeration();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        public void LoadEnumeration(string sDTCCode = "", string sDTCName = "", string sTCCode = "", string sOperator1 = "", string sOperator2 = "")
        {
            try
            {
                clsQCApproval objQC = new clsQCApproval();

                if (rdbPendingQC.SelectedValue == "1")
                {
                    grdEnumerationDetails.Columns[8].Visible = false;// Supervisor Column
                    grdEnumerationDetails.Columns[1].Visible = false;// Check box column
                }
                else
                {
                    grdEnumerationDetails.Columns[8].Visible = true;// Supervisor Column
                    grdEnumerationDetails.Columns[1].Visible = true;// Check box column
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
                    ViewState["QC"] = dt;


                    cmdApprove.Visible = true;
                }
                else
                {
                    ShowEmptyGrid();

                    cmdApprove.Visible = false;
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
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_DIV_CODE = '" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbSubdivision);
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
                    Genaral.Load_Combo("SELECT OM_CODE,OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbSubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSection);
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE '" + cmbSubdivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
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
                    int rowindex = row.RowIndex;
                    Label lblEnumDetailsId = (Label)grdEnumerationDetails.Rows[rowindex].FindControl("lblEnumDetailsId");
                    Label lblEnumType = (Label)grdEnumerationDetails.Rows[rowindex].FindControl("lblEnumType");
                    string sStatusFlag = ((Label)row.FindControl("lblStatusFlag")).Text;

                    string sEnumDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnumDetailsId.Text));
                    string sEnumType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnumType.Text));
                    sStatusFlag = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStatusFlag));


                    grdEnumerationDetails.AllowPaging = false;
                    DataTable dt = (DataTable)ViewState["QC"];
                    grdEnumerationDetails.DataSource = dt;
                    grdEnumerationDetails.DataBind();

                    int i = 0;
                    string[] strQryVallist = new string[grdEnumerationDetails.Rows.Count];
                    foreach (GridViewRow row1 in grdEnumerationDetails.Rows)
                    {

                        strQryVallist[i] = ((Label)row1.FindControl("lblEnumDetailsId")).Text.Trim() + "`" + ((Label)row1.FindControl("lblEnumType")).Text.Trim();                      
                        i++;

                    }

                    string sSelectedValue = string.Empty;
                    for (int j = 0; j < strQryVallist.Length; j++)
                    {
                        if (strQryVallist[j] != null)
                        {
                            sSelectedValue += strQryVallist[j].ToString() + "~";
                        }
                    }

                    string sAllEnumDetails = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                    Session["AllEnumID"] = sSelectedValue;

                    Response.Redirect("QCEnumApprove.aspx?QryEnumId=" + sEnumDetailsId + "&EnumType=" + sEnumType  + "&Status=" + sStatusFlag,false);
                    //+ "&AllEnumID=" + sAllEnumDetails
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

                    //DataTable dt = (DataTable)ViewState["QC"];
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
                    //        ViewState["QC"] = dv.ToTable();
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
                DataTable dt = (DataTable)ViewState["QC"];
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

        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string[] arr = new string[2];
                bool AtleastOneApp = false;
                int i = 0;

                string[] strQryVallist = new string[grdEnumerationDetails.Rows.Count];
                foreach (GridViewRow row in grdEnumerationDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblEnumDetailsId")).Text.Trim();
                        AtleastOneApp = true;
                    }
                    i++;

                }

                if (!AtleastOneApp)
                {
                    ShowMsgBox("Please Select DTC to Approve");
                    return;
                }

                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                arr = objFieldEnum.GetEnumerationInfoForApprove(objFieldEnum,strQryVallist);
                if (arr[1]!=null)
                {
                    ShowMsgBox(arr[0].ToString());
                    LoadEnumeration();
                }
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
            }
        }
    }
}