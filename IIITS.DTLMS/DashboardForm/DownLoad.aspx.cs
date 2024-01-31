using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Net;
using System.IO;
using System.Configuration;



namespace IIITS.DTLMS.DashboardForm
{
    public partial class DownLoad : System.Web.UI.Page
    {
        string strFormCode = "DownLoad";
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
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }


        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/WebApp.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkDownload_Click");
            }
        }

        protected void lnkAndroidManual_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = MapPath("~/UserManual/Android.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkAndroidManual_Click");
            }
        }
        protected void lnkAndroidApk_Click(object sender, EventArgs e)
        {
            try
            {
                clsApkDownload objApk = new clsApkDownload();   
                //String FTP_HOST = ConfigurationManager.AppSettings["FTP_HOST"].ToString();
                //String FTP_USER = ConfigurationManager.AppSettings["FTP_USER"].ToString();
                //String FTP_PASS = ConfigurationManager.AppSettings["FTP_PASS"].ToString();
                //String ApkFileName = ConfigurationManager.AppSettings["ApkFileName"].ToString();
                string sFoldername = objApk.RetrieveLatestApkDetails();
              //  FTP_HOST = FTP_HOST + sFoldername;
                    //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTP_HOST + ApkFileName);
                    //request.Method = WebRequestMethods.Ftp.DownloadFile;

                    ////Enter FTP Server credentials.
                    //request.Credentials = new NetworkCredential(FTP_USER, FTP_PASS);
                    //request.UsePassive = true;
                    //request.UseBinary = true;
                    //request.EnableSsl = false;

                    //FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    
                    //using (MemoryStream stream = new MemoryStream())
                    //{
                    //    //Stream responseStream = response.GetResponseStream();
                    //    response.GetResponseStream().CopyTo(stream);
                    //    Response.AddHeader("content-disposition", "attachment;filename=" + ApkFileName);
                    //    Response.ContentType = "application/msi";
                    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //    Response.BinaryWrite(stream.ToArray());
                    //    Response.OutputStream.Close();
                    //}

                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string sFileDownloadPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_APK_PATH"]);
                string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
                string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
                string fileName = ConfigurationManager.AppSettings["ApkFileName"].ToString();
                bool status = false;
                sFileDownloadPath = sFileDownloadPath + sFoldername;
                try
                {

                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                    status = objFtp.Download(sFileDownloadPath + fileName, fileName); 
                }
                catch (WebException ex)
                {
                    throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                }

            }

            catch (Exception ex)
            {
            //    lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkAndroidApk_Click");
            }
        }

    }
}