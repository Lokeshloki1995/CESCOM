using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;



namespace IIITS.DTLMS.Query
{
    public partial class BackEndActivity : System.Web.UI.Page
    {
        string strFormCode = "DeleteFailureTrans";
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
                lblmessage.Text = string.Empty;
                lblCount.Text = string.Empty;
                if (!IsPostBack)
                {
                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "--Select--", cmbSecondDevision);
                        //if (objSession.UserType != "4")
                        //{
                        //    Response.Redirect("~/UserRestrict.aspx", false);
                        //}
                        CheckAccessRights("1", "1");
                    }
                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }
        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DeleteFailureTrans";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    //&& objSession.UserId != "39"
                    if (UserValid() == false)
                    {
                        if (bResult == true)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                            bResult = false;
                        }
                    }

                }
                else if (flag == "1")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == false)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                        }
                    }
                }

                return bResult;
            }
            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }
        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Userid = Convert.ToString((ConfigurationSettings.AppSettings["SELECTEDUSER"]));
                string[] sUserid = Userid.Split(',');
                for (int i = 0; i < sUserid.Length; i++)
                {
                    if (objSession.UserId != sUserid[i])
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                        return res;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFeeder.SelectedValue == "")
                {
                    ShowMsgBox("Please select Feeder");
                    return;
                }

                txtImageDtcCode.Text = txtDTCCode.Text;
                txtDuplicateDtcCode.Text = txtDTCCode.Text;


            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION  WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "' ORDER BY DIV_CODE", "--Select--", cmbDivision);
                }
                else
                {
                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }

        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbsubdivision);
                }
                else
                {
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDivision_SelectedIndexChanged");
            }
        }

        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE,OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbsubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSection);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbsubdivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);

                    //Genaral.Load_Combo("SELECT OM_CODE,OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbsubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSecondSection);


                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbsubdivision_SelectedIndexChanged");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDTCCode.Text == "")
                {
                    ShowMsgBox("Please Select DTC Code");
                    txtDTCCode.Focus();
                    return;

                }
                if (txtDTCCode.Text.Length < 6)
                {
                    ShowMsgBox("Enter Valid Dtr Code");
                    txtDTCCode.Focus();
                    return;

                }
                if (ListDtcCode.Items.Count < 2)
                {
                    ListDtcCode.Visible = true;
                    ListDtcCode.Items.Add(txtDTCCode.Text);
                    txtDTCCode.Text = string.Empty;
                }
                else
                {
                    txtDTCCode.Text = string.Empty;
                    ShowMsgBox("You cannot add More than 2 values");
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnAdd_Click");
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                clsBackEndActivity objBackend = new clsBackEndActivity();
                if (ValidateForm() == true)
                {
                   
                    string[] Arr = new string[2];
                    objBackend.sFeederCode = cmbFeeder.SelectedValue;

                  

                    for (int i = 0; i < ListDtcCode.Items.Count; i++)
                    {
                        ListItem l = ListDtcCode.Items[0];
                        objBackend.sListBoxValue1 = l.ToString();
                        ListItem list = ListDtcCode.Items[1];
                        objBackend.sListBoxValue2 = list.ToString();
                    }

                    Arr = objBackend.DeleteRecordFeederDTCWise(objBackend);

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        GetRecordCount();
                    }
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdDelete_Click");
            }
        }

        public void GetRecordCount()
        {
            try
            {
                clsBackEndActivity objBackEnd = new clsBackEndActivity();
                objBackEnd.sFeederCode = cmbFeeder.SelectedValue;
                objBackEnd.sSectionCode = cmbSection.SelectedValue;
                string strCount = objBackEnd.GenerateCount(objBackEnd);
                lblCount.Text = " " + strCount + " Entries Exists For Feeder " + objBackEnd.sFeederCode + " and Section Code " + objBackEnd.sSectionCode;
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRecordCount");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (cmbCircle.SelectedIndex == 0)
            {
                cmbCircle.Focus();
                ShowMsgBox("Please Select the Circle");
                return false;
            }
            if (cmbDivision.SelectedIndex == 0)
            {
                cmbDivision.Focus();
                ShowMsgBox("Please Select the Division");
                return false;
            }
            if (cmbsubdivision.SelectedIndex == 0)
            {
                cmbsubdivision.Focus();
                ShowMsgBox("Please Select the Subdivision");
                return false;
            }
            if (cmbSection.SelectedIndex == 0)
            {
                cmbSection.Focus();
                ShowMsgBox("Please Select the Section");
                return false;
            }
            if (cmbFeeder.SelectedIndex == 0)
            {
                cmbFeeder.Focus();
                ShowMsgBox("Please Select the Feeder");
                return false;
            }

            bValidate = true;
            return bValidate;
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCircle.SelectedIndex = 0;
                ListDtcCode.Items.Clear();
                ListDtcCode.Visible = false;
                txtDTCCode.Text = string.Empty;
                cmbDivision.Items.Clear();
                cmbsubdivision.Items.Clear();
                cmbSection.Items.Clear();
                cmbFeeder.Items.Clear();
                txtImageDtcCode.Text = string.Empty;
                grdTcDetails.Visible = false;
                txtDuplicateDtcCode.Text = string.Empty;
                txtImageDtcCode.Text = string.Empty;
                cmbDeletePhotos.SelectedIndex = 0;
                lblCount.Visible = false;
                txtDTRCode.Text = string.Empty;
                cmbSecondSection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }

        }

        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string strQry = string.Empty;
                strQry = "Title=Search and Select DTC Details&";
                strQry += "Query=select DTE_DTCCODE,DTE_NAME FROM TBLENUMERATIONDETAILS,TBLDTCENUMERATION WHERE DTE_ED_ID=ED_ID and ED_STATUS_FLAG = 0 and  DTE_DTCCODE  LIKE '" + cmbFeeder.SelectedValue + "%'   AND {0} like %{1}% order by DTE_DTCCODE&";
                strQry += "DBColName=DTE_NAME~DTE_DTCCODE&";
                strQry += "ColDisplayName=DTC Name~DTC Code&";
                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

                GetRecordCount();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbFeeder_SelectedIndexChanged");
            }

        }

        protected void cmdDeleteImage_Click(object sender, EventArgs e)
        {
            try
            {

                string[] Arr = new string[2];
                clsBackEndActivity objBackEnd = new clsBackEndActivity();
                if (txtImageDtcCode.Text == "")
                {
                    ShowMsgBox("Please Select DTC Code");
                    txtImageDtcCode.Focus();
                    return;

                }
                if (cmbDeletePhotos.SelectedIndex==0)
                {
                    ShowMsgBox("Please select PhotoType To delete");
                    cmbDeletePhotos.Focus();
                    return;
                }
                else if (cmbDeletePhotos.SelectedValue == "1")
                {
                    objBackEnd.sColumnName = "EP_SSPLATE_PATH";
                }
                else if (cmbDeletePhotos.SelectedValue == "2")
                {
                    objBackEnd.sColumnName = "EP_NAMEPLATE_PATH";
                }
                else if (cmbDeletePhotos.SelectedValue == "3")
                {
                    objBackEnd.sColumnName = "EP_DTLMSDTC_PATH";
                }
                else if (cmbDeletePhotos.SelectedValue == "4")
                {
                    objBackEnd.sColumnName = "EP_IPENUMDTC_PATH";
                }
                else if (cmbDeletePhotos.SelectedValue == "5")
                {
                    objBackEnd.sColumnName = "EP_OLDDTC_PATH";
                }
                else if (cmbDeletePhotos.SelectedValue == "6")
                {
                    objBackEnd.sColumnName = "EP_INFOSYSDTC_PATH";

                }
                else if (cmbDeletePhotos.SelectedValue == "7")
                {
                    objBackEnd.sColumnName = "EP_DTC_PATH";
                }
                objBackEnd.sDTcCode = txtImageDtcCode.Text;
                Arr = objBackEnd.DeletePhotos(objBackEnd);

                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdDeleteImage_Click");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsBackEndActivity objBackEnd = new clsBackEndActivity();

                objBackEnd.sDTrCode = txtDTRCode.Text;
                objBackEnd.sDTcCode = txtDuplicateDtcCode.Text;

                dt = objBackEnd.LoadAllDTRDetails(objBackEnd);

                grdTcDetails.DataSource = dt;
                grdTcDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbLoad_Click");
            }
        }

        protected void grdTcDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string[] Arr = new string[2];

                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    clsBackEndActivity objBackEnd = new clsBackEndActivity();
                   
                    
                    objBackEnd.sEnumId = ((Label)row.FindControl("lblEdId")).Text;
                    objBackEnd.sFeederCode = cmbFeeder.SelectedValue;
                    objBackEnd.sDtcCode = txtDuplicateDtcCode.Text;
                    objBackEnd.sDTrCode = txtDTRCode.Text;
                    objBackEnd.sSectionCode = cmbSection.SelectedValue;

                    Arr = objBackEnd.DeleteDuplicateRecord(objBackEnd);

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox("Deleted Successfully");

                        GetRecordCount();

                        cmdLoad_Click(sender, e);
                    }

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTcDetails_RowCommand");
            }
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsBackEndActivity objBackEnd = new clsBackEndActivity();

                if (cmbSecondSection.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Section to Move the data");
                    cmbSecondSection.Focus();
                    return;
                }
                if (cmbFeeder.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Feeder");
                    cmbFeeder.Focus();
                    return;
                }
                if (cmbsecondFeeder.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Feeder to move data");
                    cmbsecondFeeder.Focus();
                    return;
                }

                objBackEnd.sSecondSectionCode = cmbSecondSection.SelectedValue;
                objBackEnd.sSectionCode = cmbSection.SelectedValue;
                //objBackEnd.sFeederCode = cmbFeeder.SelectedValue;
                objBackEnd.sFeederCode = cmbsecondFeeder.SelectedValue;

                Arr = objBackEnd.MoveToRequiredSection(objBackEnd);

                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());

                    GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnMove_Click");
            }

        }


        protected void cmbSecondDevision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSecondDevision.SelectedIndex > 0)
            {
                Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_DIV_CODE = '" + cmbSecondDevision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbSecondSubDevision);
            }
            else
            {
                cmbSecondSubDevision.Items.Clear();
                cmbSecondSection.Items.Clear();
            }
        }

        protected void cmbSecondSubDevision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSecondSubDevision.SelectedIndex > 0)
            {
                Genaral.Load_Combo("SELECT OM_CODE,OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbSecondSubDevision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSecondSection);

                string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                strQry += " FDO_OFFICE_CODE LIKE '" + cmbSecondSubDevision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                Genaral.Load_Combo(strQry, "--Select--", cmbsecondFeeder);
            }
            else
            {
                cmbSecondSection.Items.Clear();
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            DataTable dtCRDetails = new DataTable();
            try
            {
                clsBackEndActivity objBackEnd = new clsBackEndActivity();
                if (txtDtCode.Text == "" || txtDtCode.Text == null)
                {
                    ShowMsgBox("Please Enter DTC CODE");
                }
                dtCRDetails = objBackEnd.GetFailDetails(txtDtCode.Text);
                
                if (dtCRDetails.Rows.Count > 0)
                {
                    FailureID.Value = dtCRDetails.Rows[0]["DF_ID"].ToString();
                    ViewState["FailDetails"] = dtCRDetails;
                    GrdFailDetail.DataSource = dtCRDetails;
                    GrdFailDetail.DataBind();
                }
                else
                {
                    ShowMsgBox("DTC Not Failed or CR Not completed");
                    ShowEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnLoad_Click");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_DTC_CODE");
                dt.Columns.Add("DF_REPLACE_FLAG");
                dt.Columns.Add("DF_EQUIPMENT_ID");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("WO_NO_DECOM");

                GrdFailDetail.DataSource = dt;
                GrdFailDetail.DataBind();

                int iColCount = GrdFailDetail.Rows[0].Cells.Count;
                GrdFailDetail.Rows[0].Cells.Clear();
                GrdFailDetail.Rows[0].Cells.Add(new TableCell());
                GrdFailDetail.Rows[0].Cells[0].ColumnSpan = iColCount;
                GrdFailDetail.Rows[0].Cells[0].Text = "No Records Found";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        protected void GrdFailDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdFailDetail.PageIndex = e.NewPageIndex;
                DataTable dtFailDetails = (DataTable)ViewState["FailDetails"];
                GrdFailDetail.DataSource = dtFailDetails;
                grdTcDetails.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GrdFailDetail_PageIndexChanging");
            }
        }

        protected void GrdFailDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] arr=new string[2];
            DataTable dt = new DataTable();
            try
            {
                if (e.CommandName == "Delete")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sDfId = ((Label)row.FindControl("lblDfId")).Text.Trim();
                    string sDtcCode = txtDtCode.Text.Trim();
                    clsBackEndActivity objBackEnd = new clsBackEndActivity();
                    arr = objBackEnd.DeleteData(sDfId, sDtcCode);
                    ShowMsgBox(arr[0]);
                    ShowEmptyGrid1();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GrdFailDetail_RowCommand");
            }
        }

        public void ShowEmptyGrid1()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_ID");
                dt.Columns.Add("DF_DTC_CODE");
                dt.Columns.Add("DF_REPLACE_FLAG");
                dt.Columns.Add("DF_EQUIPMENT_ID");
                dt.Columns.Add("FAILURE_TYPE");

                GrdFailDetail.DataSource = dt;
                GrdFailDetail.DataBind();

                int iColCount = GrdFailDetail.Rows[0].Cells.Count;
                GrdFailDetail.Rows[0].Cells.Clear();
                GrdFailDetail.Rows[0].Cells.Add(new TableCell());
                GrdFailDetail.Rows[0].Cells[0].ColumnSpan = iColCount;
                GrdFailDetail.Rows[0].Cells[0].Text = "No Records Found";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid1");

            }
        }

        protected void GrdFailDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdFailDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
                Label lblStatus = (Label)e.Row.FindControl("lblReplaceFlag");
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (lblStatus.Text == "CR COMPLETED")
                    {
                        lnkView.Visible = false;
                    }
                    else
                        lnkView.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}