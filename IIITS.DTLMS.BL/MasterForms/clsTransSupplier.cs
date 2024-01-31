using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;

namespace IIITS.DTLMS.BL
{
    public class clsTransSupplier
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string RegisterAddress { get; set; }
        public string SupplierPhoneNo { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierType { get; set; }
        public string SupplierBlacklisted { get; set; }
        public string SupplierBlackedupto { get; set; }
        public string sCrby { get; set; }
        public string CommAddress { get; set; }
        public string sStatus { get; set; }
        public string sContactPerson { get; set; }
        public string sFax { get; set; }
        public string sMobileNo { get; set; }

        OleDbCommand oledbcommand;
        public string[] SaveDetails(clsTransSupplier objSupplier)
        {
            oledbcommand = new OleDbCommand();
            string[] Arr = new string[2];

            try
            {
                
                OleDbDataReader dr;
                string strQry = string.Empty;
                if (objSupplier.SupplierId == "")
                {
                    oledbcommand.Parameters.AddWithValue("TsName", objSupplier.SupplierName);
                    dr = ObjCon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND UPPER(TS_NAME)=:TsName", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Supplier Name already exists";
                        Arr[1] = "4";

                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TsPhone", objSupplier.SupplierPhoneNo);
                    dr = ObjCon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_PHONE=:TsPhone", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Phone No already exists";
                        Arr[1] = "4";

                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("TsEmail", objSupplier.SupplierEmail);
                    dr = ObjCon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND  TS_EMAIL=:TsEmail", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "EmailId already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    string StrGetMaxNo = ObjCon.Get_max_no("TS_ID", "TBLTRANSSUPPLIER").ToString();
                    strQry = "INSERT INTO TBLTRANSSUPPLIER(TS_ID,TS_NAME,TS_ADDRESS,TS_PHONE,TS_EMAIL,";
                    strQry += "TS_BLACK_LISTED,TS_BLACKED_UPTO,TS_ENTRY_AUTH,TS_COMM_ADDRESS,TS_CONT_PERSON_NAME,TS_FAX,TS_MOBILE_NO)";
                    strQry += " VALUES(:StrGetMaxNo,:SupplierName,";
                    strQry += ":RegisterAddress,:SupplierPhoneNo ,:SupplierEmail,";
                    strQry += ":SupplierBlacklisted ,";
                    strQry += "TO_DATE(:SupplierBlackedupto,'dd/MM/yyyy') ,:sCrby,:CommAddress,";
                    strQry += ":sContactPerson ,:sFax,:sMobileNo)";

                    oledbcommand.Parameters.AddWithValue("StrGetMaxNo",StrGetMaxNo);
                    oledbcommand.Parameters.AddWithValue("SupplierName", objSupplier.SupplierName);
                    oledbcommand.Parameters.AddWithValue("RegisterAddress", objSupplier.RegisterAddress);
                    oledbcommand.Parameters.AddWithValue("SupplierPhoneNo", objSupplier.SupplierPhoneNo);
                    oledbcommand.Parameters.AddWithValue("SupplierEmail", objSupplier.SupplierEmail);
                    oledbcommand.Parameters.AddWithValue("SupplierBlacklisted", objSupplier.SupplierBlacklisted);
                    oledbcommand.Parameters.AddWithValue("SupplierBlackedupto", objSupplier.SupplierBlackedupto);
                    oledbcommand.Parameters.AddWithValue("sCrby", objSupplier.sCrby);
                    oledbcommand.Parameters.AddWithValue("CommAddress", objSupplier.CommAddress);
                    oledbcommand.Parameters.AddWithValue("sContactPerson", objSupplier.sContactPerson);
                    oledbcommand.Parameters.AddWithValue("sFax", objSupplier.sFax);
                    oledbcommand.Parameters.AddWithValue("sMobileNo", objSupplier.sMobileNo);
                    ObjCon.Execute(strQry, oledbcommand);
                    Arr[0] = StrGetMaxNo.ToString();
                    Arr[1] = "0";
                    return Arr;

                }
                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tsname1", objSupplier.SupplierName);
                    oledbcommand.Parameters.AddWithValue("tsid1", objSupplier.SupplierId);
                    dr = ObjCon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND UPPER(TS_NAME)= :tsname1 AND TS_ID<> :tsid1", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Supplier Name already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("tsphone1", objSupplier.SupplierPhoneNo);
                    oledbcommand.Parameters.AddWithValue("TsId1", objSupplier.SupplierId);
                    dr = ObjCon.Fetch("Select * from TBLTRANSSUPPLIER where TS_PHONE=:tsphone1 AND TS_ID<>:TsId1  AND TS_STATUS='A'", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Phone No already exists";
                        Arr[1] = "4";

                        return Arr;
                    }
                    dr.Close();
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("temail", objSupplier.SupplierEmail);
                    oledbcommand.Parameters.AddWithValue("tid", objSupplier.SupplierId);
                    dr = ObjCon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_EMAIL=:temail AND TS_ID<>:tid", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "EmailId already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    strQry = "UPDATE TBLTRANSSUPPLIER SET TS_NAME=:SupplierName,";
                    strQry += "TS_ADDRESS=:RegisterAddress, TS_PHONE=:SupplierPhoneNo,";
                    strQry += "TS_EMAIL=:SupplierEmail,";
                    strQry += "TS_BLACK_LISTED=:SupplierBlacklisted,TS_COMM_ADDRESS=:CommAddress,";
                    strQry += "TS_BLACKED_UPTO=TO_DATE(:SupplierBlackedupto,'dd/MM/yyyy') ,";
                    strQry += " TS_CONT_PERSON_NAME=:sContactPerson,TS_FAX=:sFax,TS_MOBILE_NO=:sMobileNo";
                    strQry += " , TS_UPDATED_ON = SYSDATE , TS_UPDATED_BY = '"+objSupplier.sCrby+"' WHERE TS_ID=:SupplierId";
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("SupplierName", objSupplier.SupplierName);
                    oledbcommand.Parameters.AddWithValue("RegisterAddress", objSupplier.RegisterAddress);
                    oledbcommand.Parameters.AddWithValue("SupplierPhoneNo", objSupplier.SupplierPhoneNo);
                    oledbcommand.Parameters.AddWithValue("SupplierEmail", objSupplier.SupplierEmail);
                    oledbcommand.Parameters.AddWithValue("SupplierBlacklisted", objSupplier.SupplierBlacklisted);
                    oledbcommand.Parameters.AddWithValue("CommAddress", objSupplier.CommAddress);
                    oledbcommand.Parameters.AddWithValue("SupplierBlackedupto", objSupplier.SupplierBlackedupto);
                    oledbcommand.Parameters.AddWithValue("sContactPerson", objSupplier.sContactPerson);
                    oledbcommand.Parameters.AddWithValue("sFax", objSupplier.sFax);
                    oledbcommand.Parameters.AddWithValue("sMobileNo", objSupplier.sMobileNo);
                    oledbcommand.Parameters.AddWithValue("SupplierId", objSupplier.SupplierId);
                    ObjCon.Execute(strQry, oledbcommand);
                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "1";
                    return Arr;
                }

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "SaveDetails");
                return Arr;
            }
            finally
            {
                
            }
        }
 
    

        public object GetSupplierDetails(clsTransSupplier objSupplier)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();         
           

                try
                {
                    oledbcommand.Parameters.AddWithValue("TsId", objSupplier.SupplierId);
                    string  strQry = "SELECT TS_ID,TS_NAME,TS_ADDRESS,TS_PHONE,TS_EMAIL,TS_BLACK_LISTED,TO_CHAR(TS_BLACKED_UPTO,'DD/MM/YYYY') TS_BLACKED_UPTO,";
                    strQry += " TS_COMM_ADDRESS,TS_CONT_PERSON_NAME,TS_FAX,TO_CHAR(TS_MOBILE_NO) TS_MOBILE_NO FROM TBLTRANSSUPPLIER  WHERE TS_ID=:TsId";
                    dtDetails = ObjCon.getDataTable(strQry, oledbcommand);
                   
                    if (dtDetails.Rows.Count > 0)
                    {
                        objSupplier.SupplierId = Convert.ToString(dtDetails.Rows[0]["TS_ID"].ToString());
                        objSupplier.SupplierName = Convert.ToString(dtDetails.Rows[0]["TS_NAME"].ToString());
                        objSupplier.RegisterAddress = Convert.ToString(dtDetails.Rows[0]["TS_ADDRESS"].ToString());
                        objSupplier.SupplierPhoneNo = Convert.ToString(dtDetails.Rows[0]["TS_PHONE"].ToString());
                        objSupplier.SupplierEmail = Convert.ToString(dtDetails.Rows[0]["TS_EMAIL"].ToString()); 
                        objSupplier.SupplierBlacklisted = Convert.ToString(dtDetails.Rows[0]["TS_BLACK_LISTED"].ToString());
                        objSupplier.SupplierBlackedupto = Convert.ToString(dtDetails.Rows[0]["TS_BLACKED_UPTO"].ToString());
                        objSupplier.CommAddress = Convert.ToString(dtDetails.Rows[0]["TS_COMM_ADDRESS"].ToString());
                        objSupplier.sContactPerson = Convert.ToString(dtDetails.Rows[0]["TS_CONT_PERSON_NAME"].ToString());
                        objSupplier.sFax = Convert.ToString(dtDetails.Rows[0]["TS_FAX"].ToString());
                        objSupplier.sMobileNo = Convert.ToString(dtDetails.Rows[0]["TS_MOBILE_NO"].ToString());
                    }
                   
                    return objSupplier;
                }
                catch (Exception ex)
                {
                    clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "GetSupplierDetails");
                    return objSupplier;
                }
                finally
                {
                    
                }

              
            
        }


        public DataTable LoadSupplierDetails()
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                
                strQry = "Select TS_ID,TS_NAME,TS_ADDRESS,TS_PHONE,TS_EMAIL,DECODE(TS_BLACK_LISTED,'0','NO','1','YES') AS TS_BLACK_LISTED,TS_STATUS,";
                strQry += " TO_CHAR(TS_BLACKED_UPTO,'DD-MON-YYYY') TS_BLACKED_UPTO,TS_COMM_ADDRESS FROM TBLTRANSSUPPLIER ORDER BY TS_ID DESC";
                dt = ObjCon.getDataTable(strQry);
               
                return dt;

            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "LoadDetails");
                return dt;
            }
            finally
            {
                
            }
        }

        public bool ActiveDeactiveSupplier(clsTransSupplier objSupplier)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                
                string strQry = string.Empty;
                strQry = "UPDATE TBLTRANSSUPPLIER SET TS_STATUS='" + objSupplier.sStatus + "' WHERE TS_ID='" + objSupplier.SupplierId + "'";
                ObjCon.Execute(strQry);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "ActiveDeactiveSupplier");
                return false;

            }
            finally
            {
                
            }
        }
    }
        
}
