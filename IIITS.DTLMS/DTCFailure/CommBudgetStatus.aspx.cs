using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS.DTCFailure

{
    public partial class CommBudgetStatus : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "CommBudgetStatus";
        clsSession objSession;
        clsWorkOrder objbudgetmast = new clsWorkOrder();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblErrormsg.Text = string.Empty;
                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                if (!IsPostBack)
                {
                    if (objSession.OfficeCode.Length > 1)
                    {
                        objbudgetmast.sOfficeCode = objSession.OfficeCode;
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = '" + objbudgetmast.sOfficeCode + "' ORDER BY DIV_CODE", "--Select--", cmbDivision);
                        cmbDivision.SelectedValue = objbudgetmast.getdivcode(objbudgetmast);
                    }
                    else
                    {
                        Genaral.Load_Combo(" SELECT DIV_CODE,DIV_NAME from TBLDIVISION WHERE DIV_CICLE_CODE='" + objSession.OfficeCode + "'", "--Select--", cmbDivision);
                        cmbDivision.Enabled = true;

                    }
                    Genaral.Load_Combo("SELECT MD_SLNO,MD_UPDATED_ENHANCE_ACCCODE FROM TBLMASTERDATA WHERE MD_TYPE='C' AND ROWNUM<=1", "--Select--", cmbAccCode);

                    Genaral.Load_Combo("SELECT FY_YEARS,FY_YEARS FROM TBLFINANCIALYEAR  WHERE FY_STATUS='1'", "--Select--", cmbFinYear);
                    
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ArrResult = new string[4];
                DataTable dt = new DataTable();
                string availableamnt = string.Empty;
                string availablebudget = string.Empty;
                clsWorkOrder objbudgetmast = new clsWorkOrder();
                objbudgetmast.sBudgetAccCode = cmbAccCode.SelectedItem.Text;
                objbudgetmast.sBudgetDivCode = cmbDivision.SelectedValue;
                objbudgetmast.sFinancialYear = cmbFinYear.SelectedValue;

                //dt = objbudgetmast.ViewBudgetstatusgrid(objbudgetmast);
                availableamnt = objbudgetmast.ViewBudgetstatusaval(objbudgetmast);
                txtavlamnt.Text = availableamnt;

                if (dt.Rows.Count <= 0)
                {
                    //DataTable dtBudgetDetails = new DataTable();
                    //DataRow newRow = dtBudgetDetails.NewRow();
                    //dtBudgetDetails.Rows.Add(newRow);
                    //dtBudgetDetails.Columns.Add("BT_ID");
                    //dtBudgetDetails.Columns.Add("BT_ACC_CODE");
                    //dtBudgetDetails.Columns.Add("BT_BM_AMNT");
                    //dtBudgetDetails.Columns.Add("BT_AVL_AMNT");
                    //dtBudgetDetails.Columns.Add("BT_CRON");
                    //dtBudgetDetails.Columns.Add("BT_DEBIT_AMNT");
                    //dtBudgetDetails.Columns.Add("BT_FIN_YEAR");
                    //dtBudgetDetails.Columns.Add("BT_PONO");
                    //dtBudgetDetails.Columns.Add("BT_DIV_CODE");
                    //dtBudgetDetails.Columns.Add("BT_CREDIT_AMNT");
                    //dtBudgetDetails.Columns.Add("OSD_PO_DATE");

                    //grdBudgetstatus.DataSource = dtBudgetDetails;
                    //grdBudgetstatus.DataBind();

                    //int iColCount = grdBudgetstatus.Rows[0].Cells.Count;
                    //grdBudgetstatus.Rows[0].Cells.Clear();
                    //grdBudgetstatus.Rows[0].Cells.Add(new TableCell());
                    //grdBudgetstatus.Rows[0].Cells[0].ColumnSpan = iColCount;
                    //grdBudgetstatus.Rows[0].Cells[0].Text = "No Records Found";


                }

                else
                {

                    grdBudgetstatus.DataSource = dt;
                    grdBudgetstatus.DataBind();
                    ViewState["Budgetstatus"] = dt;
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdBudgetstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdBudgetstatus.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Budgetstatus"];
                //dt.Columns["BM_NO"].AllowDBNull = true;
                grdBudgetstatus.DataSource = dt;
                grdBudgetstatus.DataBind();


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("BT_ID");
                dt.Columns.Add("BT_ACC_CODE");
                dt.Columns.Add("BT_BM_AMNT");
                dt.Columns.Add("BT_AVL_AMNT");
                dt.Columns.Add("BT_CRON");
                dt.Columns.Add("BT_DEBIT_AMNT");
                dt.Columns.Add("BT_FIN_YEAR");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("BT_DIV_CODE");
                dt.Columns.Add("BT_CREDIT_AMNT");
                dt.Columns.Add("WO_DATE");

                grdBudgetstatus.DataSource = dt;
                grdBudgetstatus.DataBind();

                int iColCount = grdBudgetstatus.Rows[0].Cells.Count;
                grdBudgetstatus.Rows[0].Cells.Clear();
                grdBudgetstatus.Rows[0].Cells.Add(new TableCell());
                grdBudgetstatus.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdBudgetstatus.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

    }
}