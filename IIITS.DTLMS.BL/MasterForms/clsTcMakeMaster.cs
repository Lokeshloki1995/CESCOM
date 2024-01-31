using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;


namespace IIITS.DTLMS.BL
{
    public class clsTcMakeMaster
    {
        string strFormCode = "clsTcMakeMaster";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sMakeId { get; set; }
        public string sMakeName { get; set; }
        public string sDescription { get; set; }
        public string sCrby { get; set; }
        public string sStatus { get; set; }

        public string sEffectFrom { get; set; }
        public string sReason { get; set; }

        OleDbCommand oledbcommand;
        public string[] SaveUpdateTcMakeMaster(clsTcMakeMaster objTcMakeMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            string[] Arr = new string[2];

            OleDbDataReader dr;
            try
            {
                
                if (objTcMakeMaster.sMakeId  == "")
                {
                    oledbcommand.Parameters.AddWithValue("TmName", objTcMakeMaster.sMakeName.ToUpper());
                    dr = ObjCon.Fetch("select TM_NAME from TBLTRANSMAKES where UPPER(TM_NAME)=:TmName", oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Make Name Already Exists";
                        Arr[1] = "4";
                        dr.Close();
                        return Arr;
                       
                    }
                    dr.Close();


                    string sMaxNo = Convert.ToString(ObjCon.Get_max_no("TM_ID", "TBLTRANSMAKES"));
                    strQry = "Insert into TBLTRANSMAKES (TM_ID,TM_NAME,TM_DESC,TM_CRBY) VALUES ('" + sMaxNo + "',";
                   strQry+= " '" + objTcMakeMaster.sMakeName.ToUpper()  + "','" + objTcMakeMaster.sDescription + "','"+ objTcMakeMaster.sCrby  +"') ";

                    ObjCon.Execute(strQry);
                    Arr[0] = sMaxNo;
                    Arr[1] = "0";
                    return Arr;

                }

                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tmname1", objTcMakeMaster.sMakeName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("tmid1", objTcMakeMaster.sMakeId);
                    dr = ObjCon.Fetch("select TM_NAME from TBLTRANSMAKES where UPPER(TM_NAME)=:tmname1 AND TM_ID<>:tmid1",oledbcommand);
                    if (dr.Read())
                    {
                        Arr[0] = "Make Name Already Exists";
                        Arr[1] = "4";
                        dr.Close();
                        return Arr;

                    }
                    dr.Close();

                    strQry = "UPDATE TBLTRANSMAKES SET TM_NAME='" + objTcMakeMaster.sMakeName.ToUpper() + "',TM_DESC='" + objTcMakeMaster.sDescription + "' ,  ";
                    strQry += " TM_UPDATED_ON = SYSDATE ,  TM_UPDATED_BY = '"+objTcMakeMaster.sCrby+"'"; 
                    strQry+= " where TM_ID='"+ objTcMakeMaster.sMakeId +"'";
                    ObjCon.Execute(strQry);

                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;


                }
            }

            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateTcMakeMaster");
                return Arr;

            }
            finally
            {
                
            }
        }

        public clsTcMakeMaster  GetTCMakeMasterDetails(clsTcMakeMaster objTcMakeMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            
            DataTable dtStoreDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("TmId", objTcMakeMaster.sMakeId);
                strQry = "SELECT TM_ID,TM_NAME,TM_DESC FROM TBLTRANSMAKES WHERE TM_ID=:TmId";
                dtStoreDetails = ObjCon.getDataTable(strQry, oledbcommand);
                             
                if (dtStoreDetails.Rows.Count > 0)
                {

                    objTcMakeMaster.sMakeId  = dtStoreDetails.Rows[0]["TM_ID"].ToString();
                    objTcMakeMaster.sMakeName  = dtStoreDetails.Rows[0]["TM_NAME"].ToString();
                    objTcMakeMaster.sDescription = dtStoreDetails.Rows[0]["TM_DESC"].ToString();
                   
                }
                return objTcMakeMaster;
            }

            
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTCMakeMasterDetails");
                return objTcMakeMaster;
            }
            finally
            {
                
            }

        }



        public DataTable LoadTcMakeMaster()
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
           
            DataTable dtStoreDetails = new DataTable();
            try
            {
                
                //strQry = "SELECT TM_ID,TM_NAME,TM_DESC,TM_STATUS FROM TBLTRANSMAKES ORDER BY TM_ID DESC";

                strQry = "SELECT TM_ID,TM_NAME,TM_DESC,TM_STATUS,TM_EFFECT_FROM,";
                strQry += " CASE   WHEN TO_CHAR(TM_EFFECT_FROM,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') AND TM_STATUS='D' THEN 'A' ";
                strQry += " ELSE TM_STATUS END  TM_STATUS1 ";
                strQry += " FROM TBLTRANSMAKES ORDER BY TM_ID DESC ";

                dtStoreDetails = ObjCon.getDataTable(strQry);
               
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcMakeMaster");
                return dtStoreDetails;
            }
            finally
            {
                
            }

        }

        public bool ActiveDeactiveMake(clsTcMakeMaster objMakeMaster)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                
                string strQry = string.Empty;
                strQry = "UPDATE TBLTRANSMAKES SET TM_STATUS='" + objMakeMaster.sStatus + "',TM_EFFECT_FROM = TO_DATE('" + objMakeMaster.sEffectFrom + "','dd/MM/yyyy'),";
                strQry += "TM_REASON='" + objMakeMaster.sReason + "' WHERE TM_ID='" + objMakeMaster.sMakeId + "'";
                ObjCon.Execute(strQry);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ActiveDeactiveMake");
                return false;

            }
            finally
            {
                
            }
        }

    }
}

