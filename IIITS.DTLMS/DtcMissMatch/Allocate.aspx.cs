using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class Allocate : System.Web.UI.Page
    {
        string strFormCode = "Allocate";
        clsSession objSession;
        string sRemark;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }

            objSession = (clsSession)Session["clsSession"];
            if (!IsPostBack)
            {

                LoadSearchWindow();
                CheckAccessRights("1", "1");
                sRemark = ConfigurationManager.AppSettings["AllocatedRemarks"].ToString();
                txtremarks.Text = sRemark;
            }
        }

        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "AllocateUnmapp";
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Userid = Convert.ToString(ConfigurationSettings.AppSettings["SELECTEDUSER"]);
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTC Details&";
                //strQry += "Query=SELECT UNA_DTCCODE,(SELECT DT_NAME FROM TBLDTCMAST WHERE dt_code=UNA_DTCCODE)DT_NAME FROM TBLUNALLOCATEDDTCS ";
                //strQry += " WHERE UNA_REALLOCATED_DTR_SLNO IS NULL AND UNA_DTCCODE LIKE '" + txtDtcCode.Text + "%'";
                strQry += "Query=SELECT UNA_DTCCODE,DT_NAME FROM TBLUNALLOCATEDDTCS,TBLDTCMAST WHERE una_dtccode=dt_code AND UNA_REALLOCATED_DTR_SLNO IS NULL AND UNA_DTCCODE LIKE '" + txtDtcCode.Text + "%'";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=UNA_DTCCODE~DT_NAME&";
                strQry += "ColDisplayName=Dtc Code~Dtc Name&";

                strQry = strQry.Replace("'", @"\'");

                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");



                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT DISTINCT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY FROM (SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY FROM TBLUNALLOCATEDDTRS,TBLDTCMISMATCHENTRY,TBLTCMASTER WHERE DME_SL_NO=UAD_SL_MISMATCHENTRY_SLNO ";
                strQry += "AND DME_EXISTING_DTR_CODE=TC_CODE UNION ALL SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_STATUS IN(1,2)) WHERE  ROWNUM <100";
                strQry += " AND  TC_CODE LIKE '" + txtDtrCode.Text + "%'";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=TC_CODE~TC_CAPACITY&";
                strQry += "ColDisplayName=Tc Code~Tc Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");
            }
        }

        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                DataTable dt = new DataTable();
                objDtcMissMatch.sDtcCode = txtDtcCode.Text;
                dt = objDtcMissMatch.GetUnmapDetails(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    hdfOffCode.Value = dt.Rows[0]["DT_OM_SLNO"].ToString();
                    grdDtcDetails.DataSource = dt;
                    grdDtcDetails.DataBind();
                }
                else
                {
                    ShowMsgBox("DTC Already GOt Allocated");
                    txtDtcCode.Text = string.Empty;
                }
                

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnDtcSearch_Click");
            }
        }

        protected void btnDtrSearch_Click(object sender, EventArgs e)
        {
            clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
            DataTable dt = new DataTable();
            string dtccode = string.Empty;
            objDtcMissMatch.sDtrCode = txtDtrCode.Text;
            dt = objDtcMissMatch.GetUnmapDTRDetails(objDtcMissMatch);
            if (dt.Columns.Contains("DT_CODE"))
            {
               dtccode = dt.Rows[0]["DT_CODE"].ToString();                
            }


            if (dtccode == null || dtccode == "")
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                ShowMsgBox("TC Already Allocated");
                txtDtrCode.Text = string.Empty;
               
            }

        }

        protected void grdDtcDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTC DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtcDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtcDetails_RowCreated");
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTR DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtcDetails_RowCreated");
            }
        }


        protected void grdDtrDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTR DETAILS";
                    HeaderCell.ColumnSpan = 5;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    //grdDtrDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtrDetails_RowCreated");
            }
        }

        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                bool bResult = CheckAccessRights("1", "2");
                if (bResult == true)
                {
                    clsDtcMissMatchEntry objMissMatch = new clsDtcMissMatchEntry();
                    string[] sArr = new string[2];
                    objMissMatch.sDtrCode = txtDtrCode.Text;
                    objMissMatch.sDtcCode = txtDtcCode.Text;

                    objMissMatch.sRemarks = txtremarks.Text;
                    objMissMatch.sCrBy = objSession.UserId;
                    objMissMatch.sRemarks = txtremarks.Text;
                    //objMissMatch.sLocType = hdfLocType.Value;
                    //objMissMatch.sNewDTCCode = txtOldDtc.Text;
                    //objMissMatch.sNewLocType = hdfNewLocType.Value;
                    objMissMatch.sOfficeCode = hdfOffCode.Value;
                    //objMissMatch.sNewOfficeCode = hdfNewOffCode.Value;
                    //objMissMatch.sNewDtrCode = txtDtrCode.Text;

                    if (ValidateForm(objMissMatch))
                    {
                        sArr = objMissMatch.AllocateUnMappDTC(objMissMatch);

                        if (sArr[0] == "1")
                        {
                            ShowMsgBox("DTC Allocated Successfully");
                            cmdReset_Click(sender, e);

                        }
                        else if (sArr[0] == "2")
                        {
                            ShowMsgBox("Something Went Wrong......");
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdAllocate_Click");
            }
        }

        public bool ValidateForm(clsDtcMissMatchEntry objDtcMisEntry)
        {
            if (txtDtcCode.Text == "" || txtDtcCode.Text == null)
            {
                ShowMsgBox("Please Enter DTC Code");
                txtDtcCode.Focus();
                return false;
            }
            if (txtDtrCode.Text == "" || txtDtrCode.Text == null)
            {
                ShowMsgBox("please Enter TC Code");
                txtDtrCode.Focus();
                return false;
            }
            return true;

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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            grdDtcDetails.DataSource = null;
            grdDtcDetails.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
            txtDtcCode.Text = string.Empty;
            txtDtrCode.Text = string.Empty;
        }
    }
}