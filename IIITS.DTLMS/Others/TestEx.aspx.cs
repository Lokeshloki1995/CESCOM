using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace IIITS.DTLMS.Others
{
    public partial class TestEx : System.Web.UI.Page
    {
        private delegate void GetDatafromUc1(string sCircleCode);
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDatafromUc1 objDel = new GetDatafromUc1(GetData);
        }

        public void GetData(string sCircleCode)
        {
            txtValue.Text = sCircleCode;          
        }
    }
}