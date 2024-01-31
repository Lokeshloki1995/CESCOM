using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
namespace IIITS.DTLMS.TCRepair
{
    public partial class FaultTCSearch : System.Web.UI.Page
    {
        string strFormCode = "FaultTCSearch";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["UserId"] == null || Request.QueryString["UserId"].ToString() == "")
                {
                    if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                    {
                        Response.Redirect("~/Login.aspx", false);
                    }
                }

                if (Request.QueryString["UserId"] != null && Request.QueryString["UserId"].ToString() != "")
                {
                    string PId = Genaral.Decrypt(Request.QueryString["UserId"].ToString());
                    if (PId != null)
                    {
                        clsLogin objLogin = new clsLogin();
                        objSession = new clsSession();
                        objLogin.sUserId = PId;
                        objLogin.MMSUserLogin(objLogin);

                        if (objLogin.sMessage == null)
                        {

                            Session["FullName"] = objLogin.sFullName;
                            Session["ChangPwd"] = objLogin.sChangePwd;

                            if (objLogin.sOfficeCode == "0")
                            {
                                objLogin.sOfficeCode = "";
                            }

                            objSession.UserId = objLogin.sUserId;
                            objSession.FullName = objLogin.sFullName;
                            objSession.RoleId = objLogin.sRoleId;
                            objSession.OfficeCode = objLogin.sOfficeCode;
                            objSession.OfficeName = objLogin.sOfficeName;
                            objSession.Designation = objLogin.sDesignation;
                            objSession.OfficeNameWithType = objLogin.sOfficeNamewithType;
                            Session["ProjectType"] = "MMS";
                            Session["clsSession"] = objSession;
                            if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                            {
                                Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                            }
                            //else
                            //{
                            //    Response.Redirect("Dashboard.aspx", false);
                            //}

                            //HttpUtility.UrlEncode(General.Encrypt(e.Item.Cells(0).Text))
                            //General.Decrypt(HttpUtility.UrlDecode(Request.QueryString("EmpId")))
                        }
                        //else
                        //{
                        //    lblMsg.Text = objLogin.sMessage;
                        //}

                    }
                }

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {

                    LoadComboFiled();
                    GetStoreId();
                    //Genaral.Load_Combo("SELECT TS_ID,TS_NAME FROM TBLTRANSSUPPLIER where TS_ID NOT IN (SELECT TS_ID FROM TBLTRANSSUPPLIER WHERE TS_BLACK_LISTED=1 AND TS_BLACKED_UPTO>=SYSDATE)", "--Select--", cmbSupplier);

                    CheckAccessRights("4");
                    //CheckAccessRights("1");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadComboFiled()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_NAME";


                string sOfficeCode = string.Empty;
                if (objSession.OfficeCode.Length > 1)
                {
                    sOfficeCode = objSession.OfficeCode.Substring(0, 2);
                }
                else
                {
                    sOfficeCode = objSession.OfficeCode;
                }
                Genaral.Load_Combo("SELECT IT_ID,IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  ORDER BY IT_CODE", "--Select--", cmbItemCode);
                Genaral.Load_Combo("SELECT SM_ID,SM_NAME FROM TBLSTOREMAST WHERE SM_STATUS='A' AND SM_OFF_CODE LIKE '" + sOfficeCode + "%' ORDER BY SM_NAME", "--Select--", cmbStore);
                Genaral.Load_Combo("select MD_ID,MD_NAME from TBLMASTERDATA where MD_TYPE='SRT' ORDER BY MD_ID", "--Select--", cmbstarrating);
                Genaral.Load_Combo(strQry, "--Select--", cmbMake);
                Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", cmbCapacity);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadComboFiled");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCapacity.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Capacity to Filter the DTR Codes");
                    return;
                }
                //if (cmbstarrating.SelectedIndex == 0)
                //{
                //    ShowMsgBox("Please Select Star Rating");
                //    return;
                //}
                //if (txtOilQuantity.Text == "")
                //{
                //    ShowMsgBox("Please Select correct Star Rating");
                //    return;
                //}
                LoadFaultTc();
                if (ViewState["CheckedItem"] != null)
                {
                    DataTable dtCheckItems = (DataTable)ViewState["CheckedItem"];
                    ArrayList arrItemCode = new ArrayList();
                    for (int i = 0; i < dtCheckItems.Rows.Count; i++)
                    {
                        arrItemCode.Add(dtCheckItems.Rows[i]["TC_ID"]);
                    }
                    DisableCheckBox(arrItemCode);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        public void LoadFaultTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objTcFailure = new clsDTrRepairActivity();

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTcFailure.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbstarrating.SelectedIndex > 0)
                {
                    objTcFailure.sStarRating = cmbstarrating.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objTcFailure.sMakeId = cmbMake.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objTcFailure.sStoreId = cmbStore.SelectedValue;
                }
                //if (cmbSupplier.SelectedIndex > 0)
                //{
                //    objTcFailure.sSupplierId = cmbSupplier.SelectedValue;
                //}
                if (cmbGuarantyType.SelectedIndex > 0)
                {
                    objTcFailure.sGuarantyType = cmbGuarantyType.SelectedValue;
                }
                objTcFailure.sOfficeCode = objSession.OfficeCode;

                dt = objTcFailure.LoadFaultTC(objTcFailure);
                if (dt.Rows.Count > 0)
                {
                    ViewState["FaultTC"] = dt;
                    grdFaultTC.DataSource = SortDataTable(dt as DataTable, true);

                    grdFaultTC.DataBind();
                    foreach (GridViewRow row in grdFaultTC.Rows)
                    {
                        Label lblstatus = (Label)row.FindControl("lblStatus");
                        if (lblstatus.Text == "Already Sent")
                        {
                            ((CheckBox)row.FindControl("chkSelect")).Enabled = false;
                        }
                    }
                    grdFaultTC.Visible = true;
                    cmdLoadItemCode.Visible = true;
                    divItemCode.Visible = true;

                }
                else
                {
                    grdFaultTC.Visible = true;
                    cmdSend.Visible = false;
                    cmdLoadItemCode.Visible = true;
                    ViewState["FaultTC"] = dt;
                    grdFaultTC.DataSource = dt;  //sort datatable
                    grdFaultTC.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFaultTc");
            }
        }

        private bool CheckItemStockStatus()
        {
            // return true;
            try
            {
                int i = 0;
                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                    if (chk != null && chk.Checked)
                    {
                        i++;
                    }
                }
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                objRepair.sItemCode = cmbItemCode.SelectedValue;
                objRepair.sOfficeCode = objSession.OfficeCode;
                // objRepair.sStoreId = cmbStore.SelectedValue;



                objRepair.sQty = Convert.ToString(i);
                string[] Arr = new string[2];
                Arr = objRepair.getItemQnty(objRepair);
                if (Arr != null)
                {
                    if (Arr[0].ToString() == "0")
                    {
                        ShowMsgBox(Arr[1].ToString());
                        return false;
                    }
                    else if (Arr[0].ToString() == "-1")
                    {
                        ShowMsgBox("Oops Something Went Wrong");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckItemStockStatus");
                throw ex;
            }
        }

        protected void grdFaultTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFaultTC.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultTC"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdFaultTC.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdFaultTC.DataSource = dt;
            }
            grdFaultTC.DataBind();
            grdFaultTC.PageIndex = pageIndex;
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
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultTC"] = dataView.ToTable();
                        }


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultTC"] = dataView.ToTable();
                        }

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


        protected void Export_ClickTcsearch(object sender, EventArgs e)
        {
            clsDTrRepairActivity objTcFailure = new clsDTrRepairActivity();
            DataTable dt = new DataTable();

            // Jira ID: DTLMS-1831 on 22-09-2023.
            if ((ViewState["FaultTC"]) != null)
            {
                dt = (DataTable)ViewState["FaultTC"];
            }
            else
            {
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTcFailure.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbstarrating.SelectedIndex > 0)
                {
                    objTcFailure.sStarRating = cmbstarrating.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objTcFailure.sMakeId = cmbMake.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objTcFailure.sStoreId = cmbStore.SelectedValue;
                }
                //if (cmbSupplier.SelectedIndex > 0)
                //{
                //    objTcFailure.sSupplierId = cmbSupplier.SelectedValue;
                //}
                if (cmbGuarantyType.SelectedIndex > 0)
                {
                    objTcFailure.sGuarantyType = cmbGuarantyType.SelectedValue;
                }

                objTcFailure.sOfficeCode = objSession.OfficeCode;

                dt = objTcFailure.LoadFaultTC(objTcFailure);
            }

            if (dt.Rows.Count > 0 || dt == null)
            {

                dt.Columns["TC_CODE"].ColumnName = "DTr Code";
                dt.Columns["TC_SLNO"].ColumnName = "DTr SlNo";
                dt.Columns["IT_CODE"].ColumnName = "Item Code";
                dt.Columns["IT_NAME"].ColumnName = "Item Name";
                dt.Columns["TM_NAME"].ColumnName = "Make Name";
                dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(In KVA)";
                dt.Columns["TC_MANF_DATE"].ColumnName = "Manf. Date";
                dt.Columns["RCOUNT"].ColumnName = "Sent To Repairer";
                dt.Columns["TC_GUARANTY_TYPE"].ColumnName = "Guarantee Type";
                dt.Columns["TC_STAR_RATE"].ColumnName = "Star Rating";
                dt.Columns["STATUS"].ColumnName = "Status";


                dt.Columns["DTr Code"].SetOrdinal(0);
                dt.Columns["DTr SlNo"].SetOrdinal(1);
                dt.Columns["Item Code"].SetOrdinal(2);
                dt.Columns["Item Name"].SetOrdinal(3);
                dt.Columns["Make Name"].SetOrdinal(4);
                dt.Columns["Capacity(In KVA)"].SetOrdinal(5);
                dt.Columns["Manf. Date"].SetOrdinal(6);
                dt.Columns["Sent To Repairer"].SetOrdinal(7);
                dt.Columns["Guarantee Type"].SetOrdinal(8);
                dt.Columns["Star Rating"].SetOrdinal(9);
                dt.Columns["Status"].SetOrdinal(10);

                List<string> listtoRemove = new List<string> { "IT_ID", "TC_ID", "TC_PURCHASE_DATE", "TC_WARANTY_PERIOD", "TS_NAME", "AMOUNT" };
                string filename = "FaultTCDetails" + DateTime.Now + ".xls";
                string pagetitle = "Fault TC Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");

            }



        }

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblTCId = (Label)grdFaultTC.Rows[rowindex].FindControl("lblTCId");
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDTRCode");
                    TextBox txtDtrSlNo = (TextBox)row.FindControl("txtSlNo");

                    clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                    if (txtDtrCode.Text != "")
                    {
                        objTcRepair.sTcCode = txtDtrCode.Text;
                    }
                    if (txtDtrSlNo.Text != "")
                    {
                        objTcRepair.sTcSlno = txtDtrSlNo.Text;
                    }

                    SaveCheckedValues();
                    objTcRepair.sOfficeCode = objSession.OfficeCode;
                    objTcRepair.sStoreId = cmbStore.SelectedValue;
                    //DataTable dt = objTcRepair.LoadFaultTC(objTcRepair);
                    DataTable dt = (DataTable)ViewState["FaultTC"];
                    dv = dt.DefaultView;
                    //sFilter = string.Format("convert(DME_EXISTING_DTR_CODE , 'System.String') Like '%{0}%' ",txttcCode.Text);
                    if (txtDtrCode.Text != "")
                    {
                        sFilter = string.Format("convert(TC_CODE , 'System.String') Like '%{0}%' ", txtDtrCode.Text.Replace("'", "'"));
                        //sFilter = "TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDtrSlNo.Text != "")
                    {
                        sFilter = string.Format("convert(TC_SLNO , 'System.String') Like '%{0}%' ", txtDtrSlNo.Text.Replace("'", "'"));
                        //sFilter += " TC_SLNO Like '%" + txtDtrSlNo.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {

                        grdFaultTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFaultTC.DataSource = dv;
                            ViewState["FaultTC"] = dv.ToTable();
                            grdFaultTC.DataBind();
                            List<String> arrItemCode = new List<String>();
                            if (ViewState["CheckedItem"] != null)
                            {
                                DataTable dtCheckItems = (DataTable)ViewState["CheckedItem"];
                                for (int i = 0; i < dtCheckItems.Rows.Count; i++)
                                {
                                    // arrItemCode.Add(dtCheckItems.Rows[i]["TC_ID"]);
                                    arrItemCode.Add(dtCheckItems.Rows[i]["TC_ID"].ToString().Trim());
                                }
                            }
                            foreach (GridViewRow rows in grdFaultTC.Rows)
                            {
                                Label lblstatus = (Label)rows.FindControl("lblStatus");
                                Label lblTcid = (Label)rows.FindControl("lblTCId");
                                int item = Convert.ToInt32(lblTcid.Text);
                                if (lblstatus.Text == "Already Sent" || arrItemCode.Contains(lblTcid.Text))
                                {
                                    ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                                }
                            }

                        }
                        else
                        {
                            ViewState["FaultTC"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {

                        dt = objTcRepair.LoadFaultTC(objTcRepair);
                        grdFaultTC.DataSource = dt;
                        grdFaultTC.DataBind();
                        ArrayList arrItemCode = new ArrayList();
                        if (ViewState["CheckedItem"] != null)
                        {
                            DataTable dtCheckItems = (DataTable)ViewState["CheckedItem"];
                            for (int i = 0; i < dtCheckItems.Rows.Count; i++)
                            {
                                arrItemCode.Add(dtCheckItems.Rows[i]["TC_ID"]);
                            }
                        }
                        foreach (GridViewRow rows in grdFaultTC.Rows)
                        {
                            Label lblstatus = (Label)rows.FindControl("lblStatus");
                            Label lblTcid = (Label)rows.FindControl("lblTCId");
                            if (lblstatus.Text == "Already Sent" || arrItemCode.Contains(lblTcid.Text))
                            {
                                ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            }
                        }
                        ViewState["FaultTC"] = dt;

                    }
                    //if (dt.Rows.Count == 0)
                    //{
                    //    ShowEmptyGrid();
                    //    ViewState["FaultTC"] = dt;
                    //}
                    //else
                    //{
                    //    grdFaultTC.DataSource = dt;
                    //    grdFaultTC.DataBind();
                    //    foreach (GridViewRow rows in grdFaultTC.Rows)
                    //    {
                    //        Label lblstatus = (Label)rows.FindControl("lblStatus");
                    //        if (lblstatus.Text == "Already Sent")
                    //        {
                    //            ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                    //        }
                    //    }
                    //    ViewState["FaultTC"] = dt;
                    //}

                    // PopulateCheckedValues();

                    //if (ViewState["CheckedItem"] != null)
                    //{
                    //    DataTable dtCheckItems = (DataTable)ViewState["CheckedItem"];
                    //    ArrayList arrItemCode = new ArrayList();
                    //    for (int i = 0; i < dtCheckItems.Rows.Count; i++)
                    //    {
                    //        arrItemCode.Add(dtCheckItems.Rows[i]["TC_ID"]);
                    //    }
                    //    DisableCheckBox(arrItemCode);
                    //}
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_RowCommand");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_MANF_DATE");
                dt.Columns.Add("TC_PURCHASE_DATE");
                dt.Columns.Add("TC_WARANTY_PERIOD");
                dt.Columns.Add("TS_NAME");
                dt.Columns.Add("RCOUNT");
                dt.Columns.Add("TC_GUARANTY_TYPE");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("IT_CODE");
                dt.Columns.Add("IT_NAME");
                dt.Columns.Add("IT_ID");
                dt.Columns.Add("TC_STAR_RATE");

                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                int iColCount = grdFaultTC.Rows[0].Cells.Count;
                grdFaultTC.Rows[0].Cells.Clear();
                grdFaultTC.Rows[0].Cells.Add(new TableCell());
                grdFaultTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFaultTC.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {
                #region
                //Check AccessRights
                //bool bAccResult = CheckAccessRights("2");
                //if (bAccResult == false)
                //{
                //    return;
                //}

                //bool AtleastOneApp = false;
                //int i = 0;
                //string[] Arr = new string[3];
                //grdFaultTC.AllowPaging = false;
                //SaveCheckedValues();
                //LoadFaultTc();

                //PopulateCheckedValues();

                ////To check Selected Transformers Already Sent for Supplier/Repair and Waiting For Approval
                //clsApproval objApproval = new clsApproval();
                //string sResult = objApproval.GetDataReferenceId("30");

                //if (!sResult.StartsWith(","))
                //{
                //    sResult = "," + sResult;
                //}
                //if (!sResult.EndsWith(","))
                //{
                //    sResult = sResult + ",";
                //}

                //string[] strQryVallist = new string[grdFaultTC.Rows.Count];
                //foreach (GridViewRow row in grdFaultTC.Rows)
                //{
                //    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                //    {
                //        strQryVallist[i] = ((Label)row.FindControl("lblTCId")).Text.Trim();

                //        string sTCCode = ((Label)row.FindControl("lblTCCode")).Text.Trim();

                //        if (sResult.Contains("," + sTCCode + ","))
                //        {
                //            ShowMsgBox("Selected DTr " + sTCCode + "Already sent for Supplier/Repairer, Waiting for Approval");
                //            return;
                //        }

                //        AtleastOneApp = true;
                //    }
                //    i++;

                //}

                //grdFaultTC.AllowPaging = true;
                //SaveCheckedValues();
                //LoadFaultTc();
                //PopulateCheckedValues();

                //if (!AtleastOneApp)
                //{
                //    ShowMsgBox("Please Select DTr to Send for Repairer/Supplier");
                //    SaveCheckedValues();
                //    LoadFaultTc(); 
                //    PopulateCheckedValues();
                //    return;
                //}

                //string sSelectedValue = string.Empty;
                //for (int j = 0; j < strQryVallist.Length; j++)
                //{
                //    if (strQryVallist[j] != null)
                //    {
                //        sSelectedValue += strQryVallist[j].ToString() + "~";
                //    }
                //}

                //string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                //Session["TcId"] = sSelectedValue;

                //string sStoreId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbStore.SelectedValue));
                ////string sGuaranteType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbGuarantyType.SelectedValue));
                //Response.Redirect("TCRepairIssue.aspx?StoreId=" + sStoreId, false);
                #endregion
                if (ViewState["CheckedItem"] != null)
                {
                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("2");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    foreach (GridViewRow row in grdItemCode.Rows)
                    {
                        string sStarrate = ((Label)row.FindControl("lblstarrating")).Text;
                        if (sStarrate == "")
                        {
                            ShowMsgBox("Star Rate Not Updates Please Contact Support Team");
                            return;
                        }
                    }
                    DataTable dt = (DataTable)ViewState["CheckedItem"];
                    Session["Table"] = dt;
                    string sStoreId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbStore.SelectedValue));
                    string sStarRating = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbStore.SelectedValue));
                    string sCapacity = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbStore.SelectedValue));
                    //   string sOilQuantity = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtOilQuantity.Text));

                    Response.Redirect("TCRepairIssue.aspx?StoreId=" + sStoreId + "&StarRating=" + sStarRating + "&Capacity=" + sCapacity, false);
                }
                else
                {
                    ShowMsgBox("Please Select The Dtr Codes");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSend_Click");
            }
        }

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdFaultTC.Rows)
                    {
                        if (grdFaultTC.DataKeys[gvrow.RowIndex].Values[0].ToString() != "")
                        {
                            int index = Convert.ToInt32(grdFaultTC.DataKeys[gvrow.RowIndex].Values[0]);
                            if (arrCheckedValues.Contains(index))
                            {
                                CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                                myCheckBox.Checked = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PopulateCheckedValues");

            }
        }


        //This method is used to save the checkedstate of values
        private void SaveCheckedValues()
        {
            try
            {

                ArrayList arrCheckedValues = new ArrayList();
                int index = -1;
                foreach (GridViewRow gvrow in grdFaultTC.Rows)
                {
                    if (grdFaultTC.DataKeys[gvrow.RowIndex].Values[0].ToString() != "")
                    {
                        index = Convert.ToInt32(grdFaultTC.DataKeys[gvrow.RowIndex].Values[0]);

                        bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;

                        // Check in the viewstate
                        if (ViewState["CHECKED_ITEMS"] != null)
                            arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                        if (result)
                        {
                            if (!arrCheckedValues.Contains(index))
                                arrCheckedValues.Add(index);
                        }
                        else
                            arrCheckedValues.Remove(index);
                    }
                    if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                        ViewState["CHECKED_ITEMS"] = arrCheckedValues;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveCheckedValues");
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void grdFaultTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                grdFaultTC.PageIndex = e.NewPageIndex;
                LoadFaultTc();
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["FaultTC"];
                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                foreach (GridViewRow rows in grdFaultTC.Rows)
                {
                    Label lblstatus = (Label)rows.FindControl("lblStatus");
                    if (lblstatus.Text == "Already Sent")
                    {
                        ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                    }
                }
                PopulateCheckedValues();
                if (ViewState["ArrTcId"] != null)
                {
                    ArrayList temp = (ArrayList)ViewState["ArrTcId"];
                    DisableCheckBox(temp);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_PageIndexChanging");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                cmbGuarantyType.SelectedIndex = 0;
                cmbstarrating.SelectedIndex = 0;
                //cmbStore.SelectedIndex = 0;
                //cmbSupplier.SelectedIndex = 0;
                grdFaultTC.Visible = false;
                cmdSend.Visible = false;

                if (cmbStore.Enabled == true)
                {
                    cmbStore.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                string strId = objTcMaster.GetStoreId(objSession.OfficeCode);
                cmbStore.SelectedValue = strId;

                if (objSession.OfficeCode == "" || objSession.OfficeCode.Length == 1)
                {
                    cmbStore.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreId");
            }
        }




        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FaultTCSearch";
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

        public void cmdLoadItemCode_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbItemCode.SelectedValue == "--Select--")
                {
                    ShowMsgBox("Please Select Item Code");
                    return;
                }
                if (!(CheckItemStockStatus()))
                {
                    return;
                }

                SaveCheckedValues();
                // PopulateCheckedValues();
                divgrdItem.Visible = true;

                // to get the checked  tc code from the faulty tc grid .......
                bool AtleastOneApp = false;


                ArrayList arrCheckedItems = new ArrayList();
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    arrCheckedItems = (ArrayList)ViewState["CHECKED_ITEMS"];
                    if (arrCheckedItems.Count == 0)
                    {
                        ShowMsgBox("Please Select DTR Codes");
                        return;
                    }
                }

                if (ViewState["CHECKED_ITEMS"] != null)
                    AtleastOneApp = true;

                if (AtleastOneApp == false)
                {
                    ShowMsgBox("Please Select DTR Codes");
                    return;
                }
                LoadFaultTc();



                //to check whether there are two selections 
                int flag = 1;
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["CheckedItem"];
                ArrayList arrItemCode = (ArrayList)ViewState["CHECKED_ITEMS"];


                UpdateItemCode();
                DisableCheckBox(arrCheckedItems);
                //
                cmdSend.Visible = true;

                cmbCapacity.SelectedIndex = 0;
                // cmbItemCode.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoadItemCode_Click");
            }
        }

        public void cmdCapacity_Click(object sender, EventArgs e)
        {
            try
            {
                cmbItemCode.SelectedIndex = 0;
                // cmbstarrating_Click( sender, e);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        //public void cmbstarrating_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
        //        if (cmbstarrating.SelectedIndex>0 &&cmbCapacity.SelectedIndex>0)
        //        {
        //            string starrating = cmbstarrating.SelectedValue;
        //            string cap = cmbCapacity.SelectedValue;

        //            if(starrating=="3"|| starrating=="4"|| starrating == "5")
        //            {
        //                starrating = "1";
        //            }
        //            else
        //            {
        //                starrating = "0";
        //            }
        //            string Quantity = objRepair.GetOilCalculation(starrating, cap);
        //            if (Quantity!="")
        //            {
        //                txtOilQuantity.Text = Quantity;
        //            }
        //            else
        //            {
        //                ShowMsgBox("For the Selected Stat Rating and Capacity there is No Oil Quantity");
        //                txtOilQuantity.Text = "";
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
        //    }
        //}
        private void DisableCheckBox(ArrayList arrCheckedItems)
        {
            try
            {
                if (arrCheckedItems != null)
                {
                    ArrayList tempArrayList = null;
                    LoadFaultTc();
                    //DataTable dtFaultyTC = new DataTable();
                    //dtFaultyTC = (DataTable)ViewState["FaultTC"];
                    //grdFaultTC.DataSource = dtFaultyTC;
                    //grdFaultTC.DataBind();
                    if (ViewState["ArrTcId"] != null)
                    {
                        tempArrayList = (ArrayList)ViewState["ArrTcId"];
                        foreach (var row in arrCheckedItems)
                        {
                            if (!(tempArrayList.Contains(row)))
                                tempArrayList.Add((row));
                        }
                    }
                    if (tempArrayList != null)
                    {
                        arrCheckedItems = tempArrayList;
                    }
                    foreach (GridViewRow rows in grdFaultTC.Rows)
                    {
                        Label lblTCId = (Label)rows.FindControl("lblTCId");
                        foreach (var row in arrCheckedItems)
                        {
                            if (Convert.ToString(row) == lblTCId.Text)
                            {
                                ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                                //((CheckBox)rows.FindControl("chkSelect")).Checked = true;
                            }
                        }
                    }
                    //if (tempArrayList != null)
                    //{
                    //    ViewState["ArrTcId"] = tempArrayList;
                    //}
                    //else
                    {
                        ViewState["ArrTcId"] = arrCheckedItems;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableCheckBox");
            }
        }

        public void grdItemCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //int rowIndex = row.RowIndex;
                    Label sTcId = (Label)row.FindControl("lblTCId");
                    // int sTcId = Convert.ToInt32(grdItemCode.DataKeys[row.RowIndex].Values[0]);
                    DataTable dt = (DataTable)ViewState["CheckedItem"];
                    ArrayList arrItemCode = (ArrayList)ViewState["CHECKED_ITEMS"];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (sTcId.Text == Convert.ToString(dt.Rows[i]["TC_ID"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                            int sTcid = Convert.ToInt32(sTcId.Text);
                            if (arrItemCode.Contains(sTcid))
                            {
                                arrItemCode.Remove(sTcid);
                            }
                        }
                    }
                    dt.AcceptChanges();

                    if (dt.Rows.Count == 0)
                    {
                        ViewState["CheckedItem"] = null;
                        ViewState["CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        ViewState["CheckedItem"] = dt;
                        ViewState["CHECKED_ITEMS"] = arrItemCode;
                    }

                    grdItemCode.DataSource = dt;
                    grdItemCode.DataBind();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                    }
                    if (ViewState["ArrTcId"] != null)
                    {
                        ArrayList tempTCId = (ArrayList)ViewState["ArrTcId"];
                        ArrayList permTCId = (ArrayList)tempTCId.Clone();
                        int i = 0;
                        foreach (var rows in tempTCId)
                        {
                            if (Convert.ToInt32(rows) == Convert.ToInt32(sTcId.Text))
                            {
                                if(permTCId.Count == 1 && i > 0)
                                {
                                    permTCId.RemoveAt(0);
                                }
                                else
                                {
                                    permTCId.RemoveAt(i);
                                }
                            }
                            i++;
                        }
                        ViewState["ArrTcId"] = permTCId;
                        DisableCheckBox(permTCId);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdItemCode_RowCommand");
            }
        }

        public void UpdateItemCode()
        {
            try
            {
                DataTable itemCode = new DataTable();
                ArrayList arrItemCode = (ArrayList)ViewState["CHECKED_ITEMS"];
                string sItemId = cmbItemCode.SelectedValue;
                string sItemCode = cmbItemCode.SelectedItem.Text.Split('-').GetValue(0).ToString();
                string sItemName = cmbItemCode.SelectedItem.Text.Split('-').GetValue(1).ToString();
                itemCode = (DataTable)ViewState["FaultTC"];
                DataTable dtDet = new DataTable();
                DataTable dt = itemCode.Copy();
                dt.Clear();
                foreach (int s in arrItemCode)
                {
                    var filteredMRList = itemCode.AsEnumerable().Where(r => r.Field<Decimal>("TC_ID") == (Convert.ToDecimal(s)));
                    if (filteredMRList.Any())
                    {
                        dtDet = filteredMRList.CopyToDataTable();
                        dt.ImportRow(dtDet.Rows[0]);
                    }
                    //getting null here
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["IT_ID"] = sItemId;
                    dt.Rows[i]["IT_CODE"] = sItemCode;
                    dt.Rows[i]["IT_NAME"] = sItemName;
                }
                if (dt.Rows.Count > 0)
                {
                    DataTable dtFinal = new DataTable();
                    if (ViewState["CheckedItem"] != null)
                    {
                        dtFinal = (DataTable)ViewState["CheckedItem"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sTCCode = Convert.ToString(dt.Rows[i]["TC_CODE"]);
                            string sTCID = Convert.ToString(dt.Rows[i]["TC_ID"]);
                            string sTCSlno = Convert.ToString(dt.Rows[i]["TC_SLNO"]);
                            string sItID = Convert.ToString(dt.Rows[i]["IT_ID"]);
                            string sItCode = Convert.ToString(dt.Rows[i]["IT_CODE"]);
                            string sITName = Convert.ToString(dt.Rows[i]["IT_NAME"]);
                            string sTCName = Convert.ToString(dt.Rows[i]["TM_NAME"]);
                            string sTCCapacity = Convert.ToString(dt.Rows[i]["TC_CAPACITY"]);
                            string sGuarantee = Convert.ToString(dt.Rows[i]["TC_GUARANTY_TYPE"]);
                            string sStarrate = Convert.ToString(dt.Rows[i]["TC_STAR_RATE"]);
                            string Amount = string.Empty;
                            if (dt.Columns.Contains("AMOUNT"))
                            {
                                Amount = Convert.ToString(dt.Rows[i]["AMOUNT"]);
                            }
                            DataView dv = new DataView();
                            dv = dtFinal.DefaultView;
                            string sFilter = string.Empty;
                            sFilter = " TC_CODE = '" + sTCCode + "'";
                            if (sFilter.Length > 0)
                            {
                                dv.RowFilter = sFilter;
                                if (dv.Count == 0)
                                {
                                    DataRow rw = dtFinal.NewRow();
                                    rw["TC_ID"] = sTCID;
                                    rw["TC_CODE"] = sTCCode;
                                    rw["TC_SLNO"] = sTCSlno;
                                    rw["IT_ID"] = sItID;
                                    rw["IT_CODE"] = sItCode;
                                    rw["IT_NAME"] = sITName;
                                    rw["TM_NAME"] = sTCName;
                                    rw["TC_CAPACITY"] = sTCCapacity;
                                    rw["TC_GUARANTY_TYPE"] = sGuarantee;
                                    rw["TC_STAR_RATE"] = sStarrate;
                                    if (Amount != "")
                                    {
                                        rw["AMOUNT"] = Amount;
                                    }

                                    dtFinal.Rows.Add(rw);
                                    dtFinal.AcceptChanges();
                                }
                            }
                        }

                        // dt.Merge(dt1, true, MissingSchemaAction.Ignore);
                        //dt1 = dt1.DefaultView.ToTable(true, "TC_ID", "TC_CODE", "TC_SLNO", "IT_ID", "IT_CODE", "IT_NAME", "TM_NAME", "TC_CAPACITY", "TC_GUARANTY_TYPE");
                    }
                    else
                    {
                        dtFinal = dt;
                    }
                    dt = dtFinal.Copy();
                    // dt = dt.DefaultView.ToTable(true, "TC_ID", "TC_CODE", "TC_SLNO", "IT_ID", "IT_CODE", "IT_NAME", "TM_NAME", "TC_CAPACITY", "TC_GUARANTY_TYPE");
                    var result = dt.AsEnumerable()
                                   .GroupBy(r => r.Field<Decimal>("TC_ID"))
                                   .Select(g => g.First())
                                   .CopyToDataTable();
                    ViewState["CheckedItem"] = result;
                    grdItemCode.DataSource = dt;
                    grdItemCode.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateItemCode");
            }
        }
    }
}