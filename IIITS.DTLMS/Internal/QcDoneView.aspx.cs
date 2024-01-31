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
    public partial class QcDoneView : System.Web.UI.Page
    {
        string strFormCode = "QcDoneView";
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
                    string strQry = string.Empty;
                 
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);
                  
                    Genaral.Load_Combo("SELECT IU_ID,IU_FULLNAME FROM TBLINTERNALUSERS where IU_USERTYPE='1' ORDER BY IU_ID", "--Select--", cmbDeEnter);
                    Genaral.Load_Combo("SELECT IU_ID,IU_FULLNAME FROM TBLINTERNALUSERS where IU_USERTYPE='2' ORDER BY IU_ID", "--Select--", cmbQcDone);
                    
                    strQry = "Title=Search and Select DTC Code Details&";
                    strQry += "Query=select DT_CODE,DT_NAME FROM TBLDTCMAST where {0} like %{1}% order by DT_CODE&";
                    strQry += "DBColName=DT_CODE~DT_NAME&";
                    strQry += "ColDisplayName=DTC Code~DTC Name&";

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");
                    
                    strQry = "Title=Search and Select DTC Code Details&";
                    strQry += "Query=select TC_CODE,TC_SLNO FROM TBLTCMASTER where {0} like %{1}% order by TC_CODE&";
                    strQry += "DBColName=TC_CODE~TC_SLNO&";
                    strQry += "ColDisplayName=DTr Code~DTr SlNo&";

                    cmdSearch1.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTrCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTrCode.ClientID + ")");
                    
                    LoadQcDoneGrid();
                   
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadQcDoneGrid()
        {
            clsQCApproval objApproval=new clsQCApproval();
            string strQry = string.Empty;
            DataTable dt=new DataTable();
            try
            {
                dt = objApproval.LoadQcDoneBy(objApproval);
                ViewState["dtQcApproval"] = dt;
                grdQcDone.DataSource = dt;
                grdQcDone.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadQcDoneGrid");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            clsQCApproval objApproval = new clsQCApproval();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (cmbQcDone.SelectedIndex > 0)
                {
                    objApproval.sQcDoneBy = cmbQcDone.SelectedValue;
                }
                if (cmbDeEnter.SelectedIndex > 0)
                {
                    objApproval.sDeEnteredBy = cmbDeEnter.SelectedValue;
                }
                objApproval.sDtcCode = txtDtcCode.Text;
                objApproval.sDtrCode = txtDTrCode.Text;
                dt = objApproval.LoadQcDoneBy(objApproval);
                ViewState["dtQcApproval"] = dt;
                grdQcDone.DataSource = dt;
                grdQcDone.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        protected void grdQcDone_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt=new DataTable();
            try
            {
                dt =(DataTable) ViewState["dtQcApproval"];
                grdQcDone.PageIndex = 0;
                grdQcDone.PageIndex = e.NewPageIndex;
                grdQcDone.DataSource = dt;
                grdQcDone.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdQcDone_PageIndexChanging");
            }
        }

        protected void grdQcDone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            try
            {
                clsQCApproval objApproval = new clsQCApproval();
                if (e.CommandName == "search")
                {
                    DataView dv = new DataView();
                    string sFilter = string.Empty;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDTCCode");
                    TextBox txtDtcName = (TextBox)row.FindControl("txtDtcName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtTcCode");
                    objApproval.sDtcCode = txtDtcCode.Text;
                    objApproval.sDtrCode = txtDtrCode.Text;
                    objApproval.sDtcName = txtDtcName.Text;
                    DataTable dt = (DataTable)ViewState["dtQcApproval"];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "DTE_DTCCODE Like '%" + txtDtcCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDtcName.Text != "")
                    {
                        sFilter += " DTE_NAME Like '%" + txtDtcName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " DTE_TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdQcDone.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdQcDone.DataSource = dv;
                            ViewState["dtQcApproval"] = dv.ToTable();
                            grdQcDone.DataBind();

                        }
                        else
                        {
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadQcDoneGrid();
                    }
                    
                    
                    //if (dtQcSearch.Rows.Count > 0)
                    //{
                    //    grdQcDone.DataSource = dtQcSearch;
                    //    grdQcDone.DataBind();
                    //}
                    //else
                    //{
                    //    grdQcDone.DataSource = dtQcSearch;
                    //    grdQcDone.DataBind();
                        
                    }


                }
            
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdQcDone_RowCommand");
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
                dt.Columns.Add("ED_OPERATOR1");
                dt.Columns.Add("ED_APPROVED_BY");
                grdQcDone.DataSource = dt;
                grdQcDone.DataBind();

                int iColCount = grdQcDone.Rows[0].Cells.Count;
                grdQcDone.Rows[0].Cells.Clear();
                grdQcDone.Rows[0].Cells.Add(new TableCell());
                grdQcDone.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdQcDone.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

      
    }
}