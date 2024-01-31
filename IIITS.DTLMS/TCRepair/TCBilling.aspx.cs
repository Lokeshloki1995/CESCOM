using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TCBilling : System.Web.UI.Page
    {
        string strFormCode = "TCBilling";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                Form.DefaultButton = cmdSend.UniqueID;
                if (!IsPostBack)
                {

                    if (Request.QueryString["BillId"] != null && Request.QueryString["BillId"].ToString() != "")
                    {
                        txtBillId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["BillId"]));
                        GetBillDetails();
                        LoadWOGrid();
                    }

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query=SELECT distinct(TR_WO_NO) TR_WO_NO,TR_RI_NO,SM_NAME FROM TBLSTOREMAST RIGHT OUTER JOIN TBLTRANFORMERREPAIRS ON SM_ID=TR_DELIVERY_LOCATION  WHERE  TR_CANCEL_FLAG=0 ";
                    strQry+= " AND TR_TESTING=1 AND TR_WO_NO NOT IN (SELECT BT_WO_NO from TBLBILLTC WHERE  BT_PAYMENT_FLAG=1) and {0} like %{1}% &";
                    strQry += "DBColName=TR_WO_NO~TR_RI_NO~SM_NAME&";
                    strQry += "ColDisplayName=Work Order No~RI No~Store Name&";

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtWONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtWONo.ClientID + ")");
                    txtPOdate.Attributes.Add("onblur", "return ValidateDate(" + txtPOdate.ClientID + ");");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadWOGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSearch_Click");
            }
        }

        public void LoadWOGrid()
        {
            try
            {
                clsTCBilling objBill = new clsTCBilling();
                DataTable dt = new DataTable();

                if (txtWONo.Text != "")
                {

                    objBill.sWONo = txtWONo.Text.Trim();
                    dt = objBill.LoadTCforBill(objBill);
                    grdPendingTc.DataSource = dt;
                    grdPendingTc.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadWOGrid");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    clsTCBilling objBill = new clsTCBilling();
                    string[] Arr = new string[2];

                    objBill.sBillId = txtBillId.Text;
                    objBill.sWONo = txtWONo.Text;
                    objBill.sPONo = txtPONo.Text;
                    objBill.sPODate = txtPOdate.Text;
                    objBill.sDescription = txtDescription.Text.Replace("'", "''");


                   Arr = objBill.SaveTCBillDetails(objBill);
                   if (Arr[1].ToString() == "0")
                   {
                       ShowMsgBox(Arr[0].ToString());
                       txtBillId.Text = objBill.sBillId;
                       cmdSend.Text = "Update";
                       return;
                   }
                   if (Arr[1].ToString() == "1")
                   {
                       ShowMsgBox(Arr[0].ToString());
                       return;
                   }
                   if (Arr[1].ToString() == "2")
                   {
                       ShowMsgBox(Arr[0].ToString());
                       return;
                   }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSend_Click");
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtWONo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Work Order No");
                    txtWONo.Focus();
                    return bValidate;
                }
                if (txtPONo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid PO Number");
                    txtPONo.Focus();
                    return bValidate;
                }
                if (txtPOdate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid PO Date");
                    txtPOdate.Focus();
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValiidateForm");
                return bValidate;
            }
        }

        protected void grdPendingTc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPendingTc.PageIndex = e.NewPageIndex;
                LoadWOGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdPendingTc_PageIndexChanging");
            }
        }

        public void GetBillDetails()
        {
            try
            {
                clsTCBilling objBill = new clsTCBilling();
                objBill.sBillId = txtBillId.Text;

                objBill.GetBillDetails(objBill);

                txtWONo.Text = objBill.sWONo;
                txtPONo.Text = objBill.sPONo;
                txtPOdate.Text = objBill.sPODate;
                txtDescription.Text = objBill.sDescription;
                cmdSend.Text = "Update";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetBillDetails");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtWONo.Text = string.Empty;
                txtPONo.Text = string.Empty;
                txtPOdate.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtBillId.Text = string.Empty;
                cmdSend.Text = "Send to Payment";
                grdPendingTc.DataSource = null;
                grdPendingTc.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdReset_Click");
            }
        }
    }
}