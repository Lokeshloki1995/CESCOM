using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Drawing;

namespace IIITS.DTLMS.ScrapEntry
{
    public partial class ScrapTest : System.Web.UI.Page
    {
        string strFormCode = "ScrapTest";
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
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    LoadComboFiled();
                    GetStoreId();
                    CheckAccessRights("2");
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
                Genaral.Load_Combo("SELECT SM_ID,SM_NAME FROM TBLSTOREMAST WHERE SM_STATUS='A' AND SM_OFF_CODE LIKE '" + sOfficeCode + "%' ORDER BY SM_NAME", "--Select--", cmbStore);
                Genaral.Load_Combo(strQry, "--Select--", cmbMake);
                Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", cmbCapacity);
                Genaral.Load_Combo("SELECT TS_ID,TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_ID NOT IN (SELECT TS_ID FROM TBLTRANSSUPPLIER WHERE TS_BLACK_LISTED=1 AND TS_BLACKED_UPTO>=SYSDATE) ORDER BY TS_NAME", "--Select--", cmbSupplier);
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
                if (rdbAlreadyTested.Checked)
                    LoadAlreadyDone();
                if (rdbPendingForTest.Checked)
                    LoadFaultTc();
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
                clsScrap objScrap = new clsScrap();

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objScrap.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objScrap.sMakeId = cmbMake.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objScrap.sStoreId = cmbStore.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objScrap.sSupplierId = cmbSupplier.SelectedValue;
                }

                objScrap.sOfficeCode = objSession.OfficeCode;
                dt = objScrap.LoadFaultTCForScrap(objScrap);

                grdFaultTC.DataSource = dt;
                ViewState["TestDTR"] = dt;
                grdFaultTC.DataBind();
                grdFaultTC.Visible = true;
                if (dt.Rows.Count > 0)
                {
                    cmdSend.Visible = true;
                }
                else
                {
                    cmdSend.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFaultTc");
            }
        }

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "Download")
                {

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sTcCode = ((Label)row.FindControl("lblTCCode")).Text;
                    download(sTcCode);

                }
                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    DataTable dt = (DataTable)ViewState["TestDTR"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TestDTR"] = null;
                    }
                    else
                    {
                        ViewState["TestDTR"] = dt;
                    }

                    grdFaultTC.DataSource = dt;
                    grdFaultTC.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_RowCommand");
            }


        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            #region old code
            //try
            //{
            //    clsScrap objScrap = new clsScrap();
            //    //Check AccessRights
            //    bool bAccResult = CheckAccessRights("2");
            //    if (bAccResult == false)
            //    {
            //        return;
            //    }


            //        bool AtleastOneApp = false;
            //        int i = 0;
            //        string[] Arr = new string[3];
            //        grdFaultTC.AllowPaging = false;
            //        SaveCheckedValues();
            //        LoadFaultTc();

            //        PopulateCheckedValues();
            //        string[] strQryVallist = new string[grdFaultTC.Rows.Count];
            //        foreach (GridViewRow row in grdFaultTC.Rows)
            //        {
            //            if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
            //            {
            //                strQryVallist[i] = ((Label)row.FindControl("lblTCId")).Text.Trim();
            //                AtleastOneApp = true;
            //            }
            //            i++;

            //        }

            //        grdFaultTC.AllowPaging = true;
            //        SaveCheckedValues();
            //        LoadFaultTc();
            //        PopulateCheckedValues();

            //        if (!AtleastOneApp)
            //        {
            //            ShowMsgBox("Please Select TC to Declare As Scrap");
            //            SaveCheckedValues();
            //            LoadFaultTc();
            //            PopulateCheckedValues();
            //            return;
            //        }

            //        string sSelectedValue = string.Empty;
            //        for (int j = 0; j < strQryVallist.Length; j++)
            //        {
            //            if (strQryVallist[j] != null)
            //            {
            //                sSelectedValue += strQryVallist[j].ToString() + "~";
            //            }
            //        }
            //        Arr = objScrap.DeclareTcScrap(strQryVallist, objScrap);
            //        if (Arr[1].ToString() == "0")
            //        {
            //            ShowMsgBox(Arr[0].ToString());
            //            LoadFaultTc();
            //        }
            //        else if (Arr[1].ToString() == "1")
            //        {
            //            ShowMsgBox(Arr[0].ToString());
            //            return;
            //        }

            //    }

            //catch (Exception ex)
            //{
            //    lblMessage.Text = clsException.ErrorMsg();
            //    clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSend_Click");
            //}
            #endregion

            try
            {
                string[] Arr = new string[2];
                clsScrap objScrap = new clsScrap();
                int i = 0;
                objScrap.sTestResult = "0";
                bool bChecked = false;

                string[] strQrylist = new string[grdFaultTC.Rows.Count];
                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    bool ChekSelc = false;
                    // For Pass value will be 1 ;   For Test Again value will be 2
                    bool result = ((RadioButton)row.FindControl("rdbScrap")).Checked;
                    if (result)
                    {
                        objScrap.sTestResult = "1";
                        bChecked = true;
                        ChekSelc = true;
                    }

                    result = ((RadioButton)row.FindControl("rdbTest")).Checked;
                    if (result)
                    {
                        objScrap.sTestResult = "2";
                        bChecked = true;
                        ChekSelc = true;
                    }



                    //string sRemarks = ((TextBox)row.FindControl("txtRemarks")).Text;
                    if (ChekSelc == true)
                    {
                        strQrylist[i] = ((Label)row.FindControl("lblTCId")).Text.Trim() + "~" + objScrap.sTestResult;
                        i++;
                    }
                }

                if (bChecked == false)
                {
                    ShowMsgBox("Please Add Testing Result to Transformers");
                    return;
                }

                Arr = objScrap.DeclareTcScrap(strQrylist, objScrap); ;
                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());
                    LoadFaultTc();
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Scrap");
                    }
                }
                else if (Arr[1].ToString() == "1")
                {
                    ShowMsgBox(Arr[0].ToString());
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Scrap");
                    }
                    return;
                }
            }



            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSend_Click");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        void Reset()
        {
            try
            {
                //cmbStore.SelectedIndex = 0;

                cmbCapacity.SelectedIndex = 0;
                cmbStore.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }



        //This method is used to save the checkedstate of values


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



        protected void cmdReset_Click(object sender, EventArgs e)
        {
            cmbCapacity.SelectedIndex = 0;
            cmbMake.SelectedIndex = 0;

            cmbSupplier.SelectedIndex = 0;
            grdFaultTC.Visible = false;
            cmdSend.Visible = false;

            if (cmbStore.Enabled == true)
            {
                cmbStore.SelectedIndex = 0;
            }
        }
        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);

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

                objApproval.sFormName = "ScrapTest";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    Response.Redirect("~/UserRestrict.aspx", false);

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

        protected void grdFaultTC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sTcStatus = ((Label)e.Row.FindControl("lblTCStatus")).Text;
                    string sTcCode = ((Label)e.Row.FindControl("lblTCCode")).Text;

                    DataTable dt = (DataTable)ViewState["TestDTR"];
                    DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                    LinkButton lnkDnld = ((LinkButton)e.Row.FindControl("lnkDwnld"));
                    LinkButton lnkNodnld = ((LinkButton)e.Row.FindControl("lnkNodownload"));
                    if (dtrow[0]["IND_DOC"].ToString() == null || dtrow[0]["IND_DOC"].ToString() == "")
                    {
                        lnkDnld.Visible = false;
                        lnkNodnld.Visible = true;
                        lnkNodnld.CssClass = "blockpointer";


                    }
                    else
                    {
                        lnkDnld.Enabled = true;
                        lnkNodnld.Visible = false;
                        lnkDnld.CssClass = "handPointer";
                    }


                    if (sTcStatus == "7")
                    {
                        grdFaultTC.Columns[8].Visible = false;
                        grdFaultTC.Columns[12].Visible = false;
                    }
                    else
                    {
                        grdFaultTC.Columns[8].Visible = true;
                        grdFaultTC.Columns[12].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_RowDataBound");
            }
        }


        private void download(string sTcCode)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["TestDTR"];
                DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                Byte[] bytes = (Byte[])dtrow[0]["IND_DOC"];


                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";

                Response.AppendHeader("Content-Disposition", "attachment; filename=" + dtrow[0]["TC_CODE"].ToString() + ".png");

                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "download");
            }

        }

       

        public void LoadAlreadyDone()
        {
            try
            {
                DataTable dt = new DataTable();
                clsScrap objScrap = new clsScrap();
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objScrap.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objScrap.sMakeId = cmbMake.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objScrap.sStoreId = cmbStore.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objScrap.sSupplierId = cmbSupplier.SelectedValue;
                }

                objScrap.sOfficeCode = objSession.OfficeCode;
                dt = objScrap.LoadAlreadyDone(objScrap);
                grdFaultTC.DataSource = dt;
                ViewState["TestDTR"] = dt;
                grdFaultTC.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyDone");
            }
        }

        protected void Export_clickscraptest(object sender, EventArgs e)
        {

            //try
            //{
            //    DataTable dt = new DataTable();
            //    if (rdbAlreadyTested.Checked)
            //    {
            //       // DataTable dt = new DataTable();
            //        clsScrap objScrap = new clsScrap();
            //        if (cmbCapacity.SelectedIndex > 0)
            //        {
            //            objScrap.sCapacity = cmbCapacity.SelectedValue;
            //        }
            //        if (cmbMake.SelectedIndex > 0)
            //        {
            //            objScrap.sMakeId = cmbMake.SelectedValue;
            //        }
            //        if (cmbStore.SelectedIndex > 0)
            //        {
            //            objScrap.sStoreId = cmbStore.SelectedValue;
            //        }
            //        if (cmbSupplier.SelectedIndex > 0)
            //        {
            //            objScrap.sSupplierId = cmbSupplier.SelectedValue;
            //        }

            //        objScrap.sOfficeCode = objSession.OfficeCode;
            //        dt = objScrap.LoadAlreadyDone(objScrap);  
            //    }
            //    else
            //    {
                    
            //        clsScrap objScrap = new clsScrap();

            //        if (cmbCapacity.SelectedIndex > 0)
            //        {
            //            objScrap.sCapacity = cmbCapacity.SelectedValue;
            //        }
            //        if (cmbMake.SelectedIndex > 0)
            //        {
            //            objScrap.sMakeId = cmbMake.SelectedValue;
            //        }
            //        if (cmbStore.SelectedIndex > 0)
            //        {
            //            objScrap.sStoreId = cmbStore.SelectedValue;
            //        }
            //        if (cmbSupplier.SelectedIndex > 0)
            //        {
            //            objScrap.sSupplierId = cmbSupplier.SelectedValue;
            //        }

            //        objScrap.sOfficeCode = objSession.OfficeCode;
            //        dt = objScrap.LoadFaultTCForScrap(objScrap);

            //    }


             DataTable dt = (DataTable)ViewState["TestDTR"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "MANF. DATE";
                    dt.Columns["TS_NAME"].ColumnName = "Supplier";


                    List<string> listtoRemove = new List<string> { "TC_ID", "TC_PURCHASE_DATE", "TC_STATUS" };
                    string filename = "ScrapTest" + DateTime.Now + ".xls";
                    string pagetitle = " Faulty Transformer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                }

            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = clsException.ErrorMsg();
            //    clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_clickscraptest");
            //}

        }
        protected void grdFaultTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFaultTC.PageIndex;
            DataTable dt = (DataTable)ViewState["TestDTR"];
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
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else
                        {

                            ViewState["TestDTR"] = dataView.ToTable();
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
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else
                        {

                            ViewState["TestDTR"] = dataView.ToTable();
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

        

    }
}

