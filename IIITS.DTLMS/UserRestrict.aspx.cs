using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS
{
    public partial class UserRestrict : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
    }
}