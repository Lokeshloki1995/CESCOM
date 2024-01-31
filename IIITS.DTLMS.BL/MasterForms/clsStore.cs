using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{

    public class clsStore
    {
        string strFormCode = "clsStore";
        public string sSlNo { get; set; }
        public string sStoreCode { get; set; }
        public string sStoreName { get; set; }
        public string sStoreDescription { get; set; }
        public string sOfficeCode { get; set; }
        public string sCrby { get; set; }
        public string sStoreIncharge { get; set; }
        public string sAddress { get; set; }
        public string sEmailId { get; set; }
        public string sPhoneNo { get; set; }
        public string sMobile { get; set; }
        public string sStatus { get; set; }

        public string sEffectFrom { get; set; }
        public string sReason { get; set; }

        OleDbCommand oledbcommand;
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string[] SaveUpdateStoreDetails(clsStore objStore)
        {
           
            string[] Arr = new string[2];
            OleDbDataReader dr;
            string strQry = string.Empty;
            try
            {
               
                if (objStore.sSlNo == "")
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("SmCode",objStore.sStoreCode);
                    
                    dr = ObjCon.Fetch("select SM_CODE from TBLSTOREMAST where SM_CODE=:SmCode AND SM_STATUS='A'",oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Store Code Already Exists";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("SmName",objStore.sStoreName.ToUpper());
                    dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST where UPPER(SM_NAME)=:SmName AND SM_STATUS='A' ",oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Store Name Already Exists";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("SmOffCode", objStore.sOfficeCode);
                    dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST where SM_OFF_CODE=:SmOffCode AND SM_STATUS='A' ", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Store Already Exists in Selected Division";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    string sMaxNo = Convert.ToString(ObjCon.Get_max_no("SM_ID", "TBLSTOREMAST"));
                    strQry = "Insert into TBLSTOREMAST (SM_ID,SM_CODE,SM_NAME,SM_DESC,SM_OFF_CODE,SM_CRBY,SM_STORE_INCHARGE,";
                    strQry+= " SM_MOBILENO,SM_PHONENO,SM_ADDRESS,SM_EMAILID) VALUES ";
                    strQry += " (:sMaxNo,:sStoreCode,:sStoreName,:sStoreDescription,";
                    strQry += ":sOfficeCode,:sCrby,:sStoreIncharge,:sMobile,";
                    strQry+= ":sPhoneNo,:sAddress,:sEmailId) ";
                    oledbcommand.Parameters.AddWithValue("sMaxNo", sMaxNo);
                    oledbcommand.Parameters.AddWithValue("sStoreCode", objStore.sStoreCode);
                    oledbcommand.Parameters.AddWithValue("sStoreName", objStore.sStoreName);
                    oledbcommand.Parameters.AddWithValue("sStoreDescription", objStore.sStoreDescription);
                    oledbcommand.Parameters.AddWithValue("sOfficeCode", objStore.sOfficeCode);
                    oledbcommand.Parameters.AddWithValue("sCrby", objStore.sCrby);
                    oledbcommand.Parameters.AddWithValue("sStoreIncharge", objStore.sStoreIncharge);
                    oledbcommand.Parameters.AddWithValue("sMobile", objStore.sMobile);
                    oledbcommand.Parameters.AddWithValue("sPhoneNo", objStore.sPhoneNo);
                    oledbcommand.Parameters.AddWithValue("sAddress", objStore.sAddress);
                    oledbcommand.Parameters.AddWithValue("sEmailId", objStore.sEmailId);
                    ObjCon.Execute(strQry, oledbcommand);

                    Arr[0] = sMaxNo;
                    Arr[1] = "0";

                    return Arr;

                }
                else
                {
                    oledbcommand = new OleDbCommand();
                     oledbcommand.Parameters.AddWithValue("smcode1",objStore.sStoreCode);
                    oledbcommand.Parameters.AddWithValue("smid1",objStore.sSlNo);
                    dr = ObjCon.Fetch("select SM_CODE from TBLSTOREMAST where SM_CODE=:smcode1 AND SM_ID<>:smid1 AND SM_STATUS='A'",oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Store Code Already Exists";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("sStoreName", objStore.sStoreName);
                    oledbcommand.Parameters.AddWithValue("sSlNo", objStore.sSlNo);
                    dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST WHERE SM_NAME=:sStoreName  AND SM_ID<>:sSlNo AND SM_STATUS='A'", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Store Name Already Exists";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("smoffcode12", objStore.sOfficeCode);
                    oledbcommand.Parameters.AddWithValue("Smid", objStore.sSlNo);
                    dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST where SM_OFF_CODE=:smoffcode12 AND SM_STATUS='A' AND SM_ID<>:Smid", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Store Already Exists in Selected Division";
                        Arr[1] = "4";
                        return Arr;

                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    strQry = "UPDATE TBLSTOREMAST SET SM_CODE=:sStoreCode,SM_NAME=:sStoreName,";
                    strQry += " SM_DESC=:sStoreDescription,SM_OFF_CODE=:sOfficeCode,SM_STORE_INCHARGE= ";
                    strQry += ":sStoreIncharge,SM_MOBILENO=:sMobile,SM_PHONENO=:sPhoneNo, ";
                    strQry += " SM_ADDRESS=:sAddress,SM_EMAILID=:sEmailId WHERE SM_ID=:sSlNo";
                    oledbcommand.Parameters.AddWithValue("sStoreCode", objStore.sStoreCode);
                    oledbcommand.Parameters.AddWithValue("sStoreName", objStore.sStoreName);
                    oledbcommand.Parameters.AddWithValue("sStoreDescription", objStore.sStoreDescription);
                    oledbcommand.Parameters.AddWithValue("sOfficeCode", objStore.sOfficeCode);
                    oledbcommand.Parameters.AddWithValue("sStoreIncharge", objStore.sStoreIncharge);
                    oledbcommand.Parameters.AddWithValue("sMobile", objStore.sMobile);
                    oledbcommand.Parameters.AddWithValue("sPhoneNo", objStore.sPhoneNo);
                    oledbcommand.Parameters.AddWithValue("sAddress", objStore.sAddress);
                    oledbcommand.Parameters.AddWithValue("sEmailId", objStore.sEmailId);
                    oledbcommand.Parameters.AddWithValue("sSlNo", objStore.sSlNo);
                    ObjCon.Execute(strQry, oledbcommand);
                    Arr[0] = "Store Details Updated Successfully";
                    Arr[1] = "1";
                    return Arr;

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateStoreDetails");
                return Arr;

            }
            finally
            {
                
            }

        }

        public DataTable LoadStoreGrid(clsStore objStore)
        {
            oledbcommand = new OleDbCommand();
            
            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                
                if (objStore.sOfficeCode.Length > 1)
                {
                    objStore.sOfficeCode = objStore.sOfficeCode.Substring(0, 2);
                }

                //strQry = "SELECT (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=SM_OFF_CODE) AS SM_OFF_CODE,SM_ID,SM_MOBILENO,SM_NAME,";
                //strQry += " SM_STORE_INCHARGE,SM_EMAILID,SM_STATUS FROM TBLSTOREMAST WHERE SM_OFF_CODE LIKE '"+ objStore.sOfficeCode +"'  ORDER BY SM_ID DESC";

                oledbcommand.Parameters.AddWithValue("SmOffCode", objStore.sOfficeCode);
                strQry = "SELECT (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=SM_OFF_CODE) AS SM_OFF_CODE,SM_ID,SM_MOBILENO,SM_NAME,";
                strQry += " SM_STORE_INCHARGE,SM_EMAILID,SM_STATUS,";
                strQry += "CASE  WHEN TO_CHAR(SM_EFFECT_FROM,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') AND SM_STATUS='D' THEN 'A'  ELSE SM_STATUS END  SM_STATUS1 ";
                strQry += " FROM TBLSTOREMAST WHERE SM_OFF_CODE LIKE :SmOffCode||'%'  ORDER BY SM_ID DESC";

                dtStoreDetails = ObjCon.getDataTable(strQry, oledbcommand);
                
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStoreGrid");
                return dtStoreDetails;

            }
            finally
            {
                
            }

        }


        public object GetStoreDetails(clsStore objStore)
        {
            oledbcommand = new OleDbCommand();
            
            try
            {
                
                DataTable dtStoreDetails = new DataTable();
                
                string strQry = string.Empty;

                oledbcommand.Parameters.AddWithValue("SmId", objStore.sSlNo);
                strQry = "SELECT SM_ID,SM_CODE,SM_NAME,SM_DESC,SM_OFF_CODE,SM_STORE_INCHARGE,SM_MOBILENO,SM_PHONENO,SM_ADDRESS,SM_EMAILID FROM TBLSTOREMAST WHERE SM_ID=:SmId";
                dtStoreDetails = ObjCon.getDataTable(strQry, oledbcommand);
                
                if (dtStoreDetails.Rows.Count > 0)
                {
                    objStore.sSlNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_ID"]);
                    objStore.sStoreCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_CODE"]);
                    objStore.sStoreDescription = Convert.ToString(dtStoreDetails.Rows[0]["SM_DESC"]);
                    objStore.sStoreName = Convert.ToString(dtStoreDetails.Rows[0]["SM_NAME"]);
                    objStore.sOfficeCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_OFF_CODE"]);

                    objStore.sStoreIncharge = Convert.ToString(dtStoreDetails.Rows[0]["SM_STORE_INCHARGE"]);
                    objStore.sMobile = Convert.ToString(dtStoreDetails.Rows[0]["SM_MOBILENO"]);
                    objStore.sPhoneNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_PHONENO"]);
                    objStore.sAddress = Convert.ToString(dtStoreDetails.Rows[0]["SM_ADDRESS"]);
                    objStore.sEmailId = Convert.ToString(dtStoreDetails.Rows[0]["SM_EMAILID"]);

                }
                return objStore;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStoreDetails");
                return objStore;

            }
            finally
            {
                
            }

        }

       

        public bool ActiveDeactiveStore(clsStore objStore)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                
                string strQry = string.Empty;
                strQry = "UPDATE TBLSTOREMAST SET SM_STATUS=:sStatus,SM_EFFECT_FROM = TO_DATE(:sEffectFrom,'dd/MM/yyyy'),";
                strQry += " SM_REASON=:sReason   WHERE SM_ID=:sSlNo";
                oledbcommand.Parameters.AddWithValue("sStatus", objStore.sStatus);
                oledbcommand.Parameters.AddWithValue("sEffectFrom", objStore.sEffectFrom);
                oledbcommand.Parameters.AddWithValue("sReason", objStore.sReason);
                oledbcommand.Parameters.AddWithValue("sSlNo", objStore.sSlNo);
                ObjCon.Execute(strQry, oledbcommand);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ActiveDeactiveStore");
                return false;

            }
            finally
            {
                
            }

        }
    }
}
       

       
    


        
        
    

    
    

