using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Query
{
    public partial class Qrywizard : System.Web.UI.Page
    {
        clsSession objSession;
        string strFormCode = "Qrywizard";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                string strQry = string.Empty;
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    if (objSession.UserType != "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {

                LoadDetails();
                divResult.Style["display"] = "block";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        private void LoadDetails()
        {
            try
            {
                string strUserId = objSession.UserId;
                string strQry = string.Empty;
                strQry = txtQuery.Text.Trim();
                if (strQry.Length > 10)
                {

                    if (!strQry.ToUpper().Substring(0, 10).Contains("SELECT"))
                    {
                        lblMessage.Text = "Only Provision to do Select, No Other Operaions";
                        return;
                    }
                }

                clsQrywizard objQry = new clsQrywizard();
                DataTable dt = new DataTable();
                dt = objQry.GetResult(strQry, strUserId);
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Clear();
                    dt.Columns.Add("Information");
                    dt.Rows.Add("No Rows Found");
                }
                grdResult.DataSource = dt;
                grdResult.DataBind();
                ViewState["Data"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDetails");
            }

        }

        protected void cmdExport_Click(object sender, EventArgs e)
        {
            
            //LoadDetails();
            //DataTable dt = (DataTable)ViewState["Data"];
            if (grdResult.Rows.Count > 0)
            {
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "DTLMS.xls"));
                //Response.ContentType = "application/ms-excel";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);
                //grdResult.RenderControl(htw);
                //Response.Write(sw.ToString());
                //Response.End();

                Response.Clear();
                Response.Buffer = true;

                Response.AddHeader("content-disposition", "attachment;filename=ResultExport.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                grdResult.AllowPaging = false;

                grdResult.DataSource = (DataTable)ViewState["Data"];
                grdResult.DataBind();

                //Change the Header Row back to white color
                grdResult.HeaderRow.Style.Add("background-color", "#FFFFFF");

                //Apply style to Individual Cells
                //grdResult.HeaderRow.Cells[0].Style.Add("background-color", "green");
                //grdResult.HeaderRow.Cells[1].Style.Add("background-color", "green");
                //grdResult.HeaderRow.Cells[2].Style.Add("background-color", "green");
                //grdIndexResult.HeaderRow.Cells(3).Style.Add("background-color", "green")

                for (int i = 0; i <= grdResult.Rows.Count - 1; i++)
                {
                    GridViewRow row = grdResult.Rows[i];

                    //Change Color back to white
                    row.BackColor = System.Drawing.Color.White;

                    //Apply text style to each Row
                    row.Attributes.Add("class", "textmode");

                    //Apply style to Individual Cells of Alternating Row
                    if (i % 2 != 0)
                    {
                        row.Cells[0].Style.Add("background-color", "#C2D69B");
                        row.Cells[1].Style.Add("background-color", "#C2D69B");
                        row.Cells[2].Style.Add("background-color", "#C2D69B");
                        //row.Cells(3).Style.Add("background-color", "#C2D69B")
                    }
                }
                grdResult.RenderControl(hw);

                //style to format numbers to string
                string style = "<style>.textmode{mso-number-format:\\@;}</style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();



            }
           
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtQuery.Text = string.Empty;
                grdResult.DataSource = null;
                grdResult.DataBind();
                divResult.Style["display"] = "none";
                lblMessage.Text = "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClear_Click");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}