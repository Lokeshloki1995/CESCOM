using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.BasicForms
{
    public partial class Station : System.Web.UI.Page
    {

        ArrayList userdetails = new ArrayList();
        string[] tmpuserlist = new string[50];
        string strUserLogged = string.Empty;
        string sFormCode = "Station";
        string strEmpId = string.Empty;
        clsSession objSession;
       // int Circle_code = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Circle_code"]);
        
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];

                Form.DefaultButton = cmdSave.UniqueID;

                lblErrormsg.Text = string.Empty;
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT DT_CODE,DT_CODE || '-' || DT_NAME FROM TBLDIST ORDER BY DT_CODE", "--Select--", cmbDistrict);
                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE ", "--Select--", cmbCircle);
                    Genaral.Load_Combo("SELECT STC_CAP_ID,STC_CAP_VALUE FROM TBLSTATIONCAPACITY ORDER BY STC_CAP_ID", "--Select--", cmbCapacity);

                   
                    if (Request.QueryString["StationId"] != null && Request.QueryString["StationId"].ToString() != "")
                    {
                        txtStationId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StationId"]));
                        txtStationCode.ReadOnly = true;
                    }

                    if (txtStationId.Text != "0")
                    {
                        Genaral.Load_Combo("SELECT TQ_CODE ,TQ_CODE || '-' || TQ_NAME FROM TBLTALQ ORDER BY TQ_CODE ", "--Select--", cmbTalq);

                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "--Select--", cmbDivision);
                        

                        LoadStationDet(txtStationId.Text);
                        // commented as no need to call the index hanged funtion  . 

                      //  cmbDistrict_SelectedIndexChanged(sender, e);
                      //  cmbCircle_SelectedIndexChanged(sender, e);
                      //cmbDivision_SelectedIndexChanged(sender, e);
                      //  cmbTalq.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 2));

                    }
                   }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "Page_Load");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arrmsg = new string[3];


                strUserLogged = objSession.UserId;
                if (txtStatName.Text.Trim().Length == 0 )
                {
                     ShowMsgBox("Enter Station Name");
                     txtStatName.Focus();
                    return;
                }
                if (cmbDistrict.SelectedValue == "0")
                {
                     ShowMsgBox("Select District");
                     cmbDistrict.Focus();
                    return;
                }
                if (cmbTalq.SelectedValue == "0")
                {
                    ShowMsgBox("Select Taluk");
                    cmbTalq.Focus();
                    return;
                }
                if (cmbDistrict.SelectedValue.ToString() != cmbTalq.Text.Substring(0, 1))
                {
                    ShowMsgBox("District Code and Taluk Code Does not Match");
                    cmbTalq.Focus();
                    return;
                }
                if (cmbCircle.SelectedValue == "0")
                {
                    ShowMsgBox("Enter Circle Code");
                    cmbCircle.Focus();
                    return;
                }
                if (cmbDivision.SelectedValue == "0")
                {
                    ShowMsgBox("Enter Division Code");
                    cmbDivision.Focus();
                    return;
                }

                if (txtStationCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Station Code");
                    txtStationCode.Focus();
                    return;
                }
                if (txtStationCode.Text.Length != 3)
                {
                     ShowMsgBox("Station Code should be 3 digits");
                     txtStationCode.Focus();
                    return;
                }
                

                //if (cmbTalq.SelectedValue.ToString() != txtStationCode.Text.Substring(0, 2))
                //{
                //    ShowMsgBox("Taluk Code and Station Code Does not Match");
                //    txtStationCode.Focus();
                //    return;
                //}

                if (cmbCapacity.SelectedIndex==0)
                {
                     ShowMsgBox("Select the required Voltage Class");
                     cmbCapacity.Focus();
                    return;
                }
                if (txtMobileNo.Text.Trim().Length == 0)
                {
                   ShowMsgBox("Enter Valid Mobile Number");
                   txtMobileNo.Focus();
                    return;
                }
                if (txtMobileNo.Text.Length != 10)
                {
                    ShowMsgBox("Enter Valid 10 Digit Mobile Number");
                    txtMobileNo.Focus();
                    return;
                }
                if (txtEmailId.Text.Trim().Length == 0)
                {
                     ShowMsgBox("Enter Valid Email Id");
                     txtEmailId.Focus();
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                     ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                     txtEmailId.Focus();
                    return ;
                }

                clsStation objStation = new clsStation();
                objStation.StationId = Convert.ToInt64(txtStationId.Text);
                objStation.StationName = txtStatName.Text.Trim();
                objStation.StationCode = txtStationCode.Text.Trim();
                objStation.EmailId = txtEmailId.Text.Trim();
                objStation.MobileNo = txtMobileNo.Text.Trim();
                objStation.OfficeCode = cmbDivision.SelectedValue;

                objStation.UserLogged = strUserLogged;
               objStation.talukcode= cmbTalq.SelectedValue;
                
                objStation.Description = txtDesc.Text.Trim();
                objStation.Capacity = cmbCapacity.SelectedValue;

                if (cmdSave.Text.Equals("Save"))
                {
                    objStation.IsSave = true;
                    Arrmsg = objStation.SaveStationDetails(objStation);
                    if (Arrmsg[1].ToString() == "0")
                    {
                        ShowMsgBox(Arrmsg[0]);
                        txtStationId.Text = Arrmsg[2];
                       // objStation.StationId =Convert.ToInt64(Arrmsg[2]);
                        cmdSave.Text = "Update";
                      //  Reset();
                        DisableTextField();
                        Response.Redirect("~/BasicForms/StationView.aspx", false);
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0]);
                    }
                 


                }
                if (cmdSave.Text.Equals("Update"))
                {

                    //if (Request.QueryString["StationId"] != null && Request.QueryString["StationId"].ToString() != "")
                    //{
                    //   objStation.StationId=Convert.ToInt32(txtStationId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StationId"])));
                    //}
                    
                    objStation.IsSave = false;
                    Arrmsg = objStation.SaveStationDetails(objStation);
                    if (Arrmsg[1].ToString() == "0")
                    {
                        ShowMsgBox(Arrmsg[0]);
                        Response.Redirect("~/BasicForms/StationView.aspx", false);

                        // Reset();
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0]);
                    }
                   
                }

              
                //LoadStation();
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmdSave_Click");
            }
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
                Response.Write(ex);
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "ShowMsgBox");
            }
        }

        /// <summary>
        /// Reset Fn
        /// </summary>
        private void Reset()
        {
            try
            {
                txtStationCode.Text = string.Empty;
                txtStatName.Text = string.Empty;
                txtDesc.Text = string.Empty;
                userdetails.Clear();
                cmbDistrict.SelectedIndex = 0;
                cmbCircle.SelectedIndex = 0;
                cmbDivision.SelectedIndex = 0;
                cmbTalq.SelectedIndex = 0;
                txtMobileNo.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                cmdSave.Text = "Save";
                cmbCapacity.SelectedIndex = 0;
                cmbDistrict.Enabled = true;
                cmbCircle.Enabled = true;
                cmbTalq.Enabled = true;
                cmbDivision.Enabled = true;
                //cmbTalq.Enabled = true;
                txtStationCode.Enabled = true;
                txtStationCode.ReadOnly = false;
                
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "Reset");
            }

        }

        public void LoadStationDet(string strStationId)
        {
            try
            {
                clsStation ObjStation = new clsStation();
                ArrayList arrOffCode = new ArrayList();
                ArrayList arrOffCodeValue = new ArrayList();

                DataTable DtStationDet = ObjStation.LoadStationDetail(strStationId);

                txtStatName.Text = Convert.ToString(DtStationDet.Rows[0]["ST_NAME"]);
                txtStationCode.Text = Convert.ToString(DtStationDet.Rows[0]["ST_STATION_CODE"]);
                txtDesc.Text = Convert.ToString(DtStationDet.Rows[0]["ST_DESCRIPTION"]);
               
                txtMobileNo.Text = Convert.ToString(DtStationDet.Rows[0]["ST_MOBILE_NO"]);
                txtEmailId.Text = Convert.ToString(DtStationDet.Rows[0]["ST_EMAILID"]);
                ObjStation.OfficeCode = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]);
                cmbCapacity.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_STC_CAP_ID"]);
                DisableTextField();

                if (txtStationCode.Text != "" && txtStationCode.Text != null)
                {
                    cmbDistrict.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 1));
                    cmbTalq.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 2));
                }
                else
                {
                    txtStationCode.Enabled = true;
                    cmbDistrict.Enabled = true;
                    cmbTalq.Enabled = true;
                }
                if (ObjStation.OfficeCode != "" && ObjStation.OfficeCode != null)
               {
                   cmbCircle.SelectedValue = Convert.ToString(ObjStation.OfficeCode.Substring(0, 1));
                   cmbDivision.SelectedValue = Convert.ToString(ObjStation.OfficeCode);
               }
                else
                {
                    cmbCircle.Enabled = true;
                    cmbDivision.Enabled = true;
                     
                }
              
                
            
              //  cmbTalq.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 2));
           
              
                ViewState["CHECKED_ITEMS"] = arrOffCodeValue;

               // LoadOffice();
              

                
                cmdSave.Text = "Update";
              

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "LoadStationDet");
            }

        }
        public void DisableTextField()
        {
            //cmbDistrict.Enabled = false;
            //cmbCircle.Enabled = false;
            //cmbDivision.Enabled = false;
            //cmbTalq.Enabled = false;
            
            //txtStationCode.Enabled = false;
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE ='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
 
                    
                }

                else
                {
                    cmbDivision.Items.Clear();


                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }
        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // for station code auto generate
                //clsStation objStation = new clsStation();
                //if (cmbDivision.SelectedIndex > 0)
                //{
                //    string div = cmbDivision.SelectedValue;
                //    txtStationCode.Text = objStation.AutoGenerateStationCode(div);
                //    txtStationCode.ReadOnly = true;
                //}

                // old code
                //clsStation objStation = new clsStation();
                //objStation.OfficeCode = cmbDivision.SelectedValue;
                //objStation.OfficeCode = objStation.GenerateSectionCode(objStation);
                //txtStationCode.ReadOnly = true;
            }
            catch (Exception ex)
            {

                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmbDivision_SelectedIndexChanged");
            }
        }

        protected void cmdReset_Click1(object sender, EventArgs e)
        {
            Reset();
        }

        public void LoadOffice(string sOfficeCode="",string sOffName="")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsStation objStation = new clsStation();
                objStation.OfficeCode = sOfficeCode;
                objStation.OfficeName = sOffName;
                dtPageDetaiils = objStation.LoadOfficeDet(objStation);
              
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "LoadOffice");
            }
        }

        protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDistrict.SelectedIndex > 0)
                {
                 Genaral.Load_Combo(" SELECT TQ_CODE,TQ_CODE || '-' || TQ_NAME FROM TBLTALQ WHERE TQ_DT_ID like '" + cmbDistrict.SelectedValue + "%'  order by TQ_CODE", "--Select--", cmbTalq);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmbDistrict_SelectedIndexChanged");
            }
        }
        //protected void cmbTalq_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbTalq.SelectedIndex > 0)
        //        {
        //            //   Genaral.Load_Combo(" SELECT TQ_CODE,TQ_CODE || '-' || TQ_NAME FROM TBLTALQ WHERE TQ_DT_ID like '" + cmbDistrict.SelectedValue + "%'  order by TQ_CODE", "--Select--", cmbTalq);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblErrormsg.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmbDistrict_SelectedIndexChanged");
        //    }
        //}


        protected void cmdParentStID_Click(object sender, EventArgs e)
        {
          
            string strLocationId = string.Empty;
            strLocationId += "Title=Search Window&";
            string strDeviceId = string.Empty;
            strEmpId += "Title=Search and Select Parent station Details&";
            strEmpId += "Query=select ST_ID \"StationID\",ST_NAME \"StationName\",ST_STATION_CODE \"StationCode\" FROM   TBLSTATION,TBLSTATIONCAPACITY  WHERE ST_STC_CAP_ID=STC_CAP_ID  and STC_CAP_ID>" + cmbCapacity.SelectedValue + " and  LOWER({0}) like %{1}% order by ST_NAME&";
            strEmpId += "DBColName=ST_NAME~ST_STATION_CODE&";
            strEmpId += "ColDisplayName=StationName~StationCode&";
          
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Station";
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        protected void cmbTalq_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsStation objStation = new clsStation();

                string talk = cmbTalq.SelectedValue;
                txtStationCode.Text = objStation.AutoGenerateStationCode(talk);
                txtStationCode.ReadOnly = true;

                // // objStation.StationCode = cmbTalq.SelectedValue;
                // //     txtStationCode.Text = objStation.GenerateSectionCode(objStation);

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmbTalq_SelectedIndexChanged");
            }
        }

    }
}
        
    



      
  
