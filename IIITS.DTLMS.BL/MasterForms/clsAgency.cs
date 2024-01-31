using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.BL
{
    public class clsAgency
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string AgencyId { get; set; }
        //public string AgencyName { get; set; }
        public string AgencyName { get; set; }
        public string AgencyAddress { get; set; }
        public string AgencyPhoneNo { get; set; }
        public string AgencyEmail { get; set; }
        public string RepairerType { get; set; }
        public string RepairerBlacklisted { get; set; }
        public string RepairerBlackedupto { get; set; }
        public string CommAddress { get; set; }
        public string sCrby { get; set; }
        public string sDivcode { get; set; }
        public string sContactPerson { get; set; }
        public string sFax { get; set; }
        public string sMobileNo { get; set; }
        public string sOffCode { get; set; }
        public string sContractPeriod { get; set; }

        public string sStatus { get; set; }
        public string sDeactivateReason { get; set; }
        public string sDeactivateFrom { get; set; }
        public string sDeactivateTo { get; set; }
        public string sExtensionTill { get; set; }

        public string sRepairOffCode { get; set; }
        public string staluq { get; set; }
        public string sRepairerTalukID { get; set; }

        public string sMasterStatus { get; set; }
        public ArrayList sExistedOffCode { get; set; }
        public string sMappingId { get; set; }
        // for saving the repairer at the taluk level 
        public string[] SaveRepairerDetails(clsAgency objAgency)
        {
            string[] Arr = new string[2];

            try
            {

                string strQry = string.Empty;
                OleDbDataReader dr;
                // if update then we will have taluk id else its a new one 
                if (!(objAgency.AgencyId == "" || objAgency.AgencyId == null))
                {

                   

                    if (objAgency.RepairerBlacklisted == "1")
                    {
                        DateTime dateTime = DateTime.ParseExact(objAgency.RepairerBlackedupto, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        if (dateTime < System.DateTime.Now)
                        {
                            objAgency.sStatus = "D";
                        }
                        else
                        {
                            objAgency.sStatus = "A";
                        }
                    }
                    else
                    {
                        objAgency.sStatus = "A";
                    }
                    
                    strQry = "UPDATE TBLTRANSREPAIRER SET TR_NAME='" + objAgency.AgencyName + "',TR_ADDRESS='" + objAgency.AgencyAddress + "',";
                    strQry += "TR_PHONE='" + objAgency.AgencyPhoneNo + "',TR_EMAIL='" + objAgency.AgencyEmail + "',";
                    strQry += "TR_OFFICECODE='" + objAgency.sOffCode + "',TR_BLACK_LISTED='" + objAgency.RepairerBlacklisted + "',TR_COMM_ADDRESS='" + objAgency.CommAddress + "',";
                    strQry += "TR_BLACKED_UPTO=TO_DATE('" + objAgency.RepairerBlackedupto + "','dd/MM/yyyy') ,";
                    strQry += " TR_CONT_PERSON_NAME='" + objAgency.sContactPerson + "',TR_CONTRACT_PERIOD='" + objAgency.sContractPeriod + "',TR_FAX='" + sFax + "',TR_MOBILE_NO='" + objAgency.sMobileNo + "' , TR_LOC_CODE = '" + objAgency.staluq + "'";
                    strQry += " , TR_UPDATED_ON = SYSDATE , TR_UPDATED_BY = '" + objAgency.sCrby + "' WHERE TR_ID= '" + objAgency.AgencyId + "'";
                    ObjCon.Execute(strQry);



                    Arr[0] = "Updated Successfully";
                    Arr[1] = "0";
                    return Arr;
                 
                }

                else
                {
                   

                    strQry = "SELECT TR_TR_ID  FROM TBLTRANSREPAIRER WHERE TR_NAME = '" + objAgency.AgencyName + "'  GROUP BY TR_TR_ID";
                    string strMasterID = ObjCon.get_value(strQry);

                    dr = ObjCon.Fetch(" select TR_TR_ID from TBLTRANSREPAIRER where TR_STATUS = 'A' AND TR_OFFICECODE = '" + objAgency.sOffCode + "'  and  TR_LOC_CODE = '" + objAgency.staluq + "' and TR_TR_ID= '" + strMasterID + "' GROUP BY TR_TR_ID ");
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "This Repairer already  mapped to selected division and taluq";
                        Arr[1] = "1";
                        return Arr;
                    }
                    dr.Close();

                    // concept change in repairere before updating repairer 
                    string StrGetMaxNo = ObjCon.Get_max_no("TR_ID", "TBLTRANSREPAIRER").ToString();
                    strQry = "INSERT INTO TBLTRANSREPAIRER(TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,";
                    strQry += "TR_OFFICECODE,TR_BLACK_LISTED,TR_BLACKED_UPTO,TR_ENTRY_AUTH,TR_COMM_ADDRESS,TR_CONT_PERSON_NAME,TR_CONTRACT_PERIOD,TR_FAX,TR_MOBILE_NO,TR_TR_ID ,TR_LOC_CODE )";
                    strQry += " VALUES('" + StrGetMaxNo + "','" + objAgency.AgencyName + "',";
                    strQry += "'" + objAgency.AgencyAddress + "','" + objAgency.AgencyPhoneNo + "',";
                    strQry += " '" + objAgency.AgencyEmail + "','" + objAgency.sOffCode + "',";
                    strQry += "'" + objAgency.RepairerBlacklisted + "',TO_DATE('" + objAgency.RepairerBlackedupto + "','dd/MM/yyyy') ,";
                    strQry += " '" + objAgency.sCrby + "','" + objAgency.CommAddress + "',";
                    strQry += " '" + objAgency.sContactPerson + "','" + objAgency.sContractPeriod + "','" + sFax + "','" + objAgency.sMobileNo + "','" + strMasterID + "' , '" + objAgency.staluq + "')";
                    ObjCon.Execute(strQry);


                 
                    Arr[0] = "Saved Successfully ";
                    Arr[1] = "0";
                    return Arr;

                }
            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "SaveRepairerDetails");
                Arr[0] = "Something Went Wrong Please Contact Support Team ";
                Arr[1] = "0";
                return Arr;
            }
            finally
            {

            }

        }

        public DataTable LoadRepairerDetails(clsAgency objAgency)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                // aftere nitin sir  concept 
                strQry = "SELECT B.TR_ID ,B.TR_TR_ID ,A.TR_NAME   , B.TR_PHONE , B.TR_EMAIL , B.TR_ADDRESS , B.TR_COMM_ADDRESS  AS RA_TALQ_ADDR  , B.TR_MOBILE_NO as RA_CONTACT_NO , ";
                strQry += " DECODE(B.TR_BLACK_LISTED  ,'0','NO','1','YES') AS RA_BLACK_LISTED , TO_CHAR(B.TR_BLACKED_UPTO ,'DD-MON-YYYY') RA_BLACK_LISTED_UPTO , ";
                strQry += " DIV_NAME AS DIVISION , TQ_NAME AS TALUK  , B.TR_STATUS  AS RA_STATUS    FROM TBLTRANSREPAIRER A  INNER JOIN TBLTRANSREPAIRER B ON  A.TR_ID = B.TR_TR_ID  LEFT JOIN   TBLTALQ  ";
                strQry += " ON B.TR_LOC_CODE  = TQ_SLNO LEFT JOIN TBLDIVISION ON DIV_CODE = B.TR_OFFICECODE  WHERE  B.TR_OFFICECODE LIKE '" + objAgency.sRepairOffCode + "%'  ORDER BY B.TR_OFFICECODE  ";

                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "LoadRepairerDetails");
                return dt;
            }

        }

        public DataTable LoadAgencyRepairerMasterDetails(clsAgency objAgency)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " select AM_ID, RA_ID, RA_NAME, RA_ADDRESS, RA_PHNO, RA_MAIL, AM_OFFICE_CODE, AM_STATUS, RA_STATUS,";
                strQry += "(SELECT DIV_NAME from TBLDIVISION where DIV_CODE = AM_OFFICE_CODE) OFFICE_NAME FROM TBLREPAIRERAGENCYMASTER ";
                strQry += " inner join TBLAGENCYDIVMAPPING on RA_ID = AM_RA_ID  ORDER BY AM_ID DESC ";

                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "LoadRepairerDetails");
                return dt;
            }


        }

        // query has been  modified
        public object GetAgencyRepairerDetails(clsAgency objAgency)
        {
            DataTable dtDetails = new DataTable();
            String strQry = string.Empty;
            OleDbDataReader dr = null;
            try
            {
                strQry = "  SELECT RA_ID ,RA_NAME, RA_ADDRESS , RA_PHNO, RA_MAIL,";
                strQry += " AM_OFFICE_CODE ,RA_STATUS FROM TBLREPAIRERAGENCYMASTER inner join TBLAGENCYDIVMAPPING on RA_ID = AM_RA_ID  WHERE AM_ID = '" + objAgency.AgencyId + "' ";

                dr = ObjCon.Fetch(strQry);
                dtDetails.Load(dr);
                if (dtDetails.Rows.Count > 0)
                {
                    objAgency.AgencyId = Convert.ToString(dtDetails.Rows[0]["RA_ID"].ToString());
                    objAgency.AgencyName = Convert.ToString(dtDetails.Rows[0]["RA_NAME"].ToString());
                    objAgency.AgencyAddress = Convert.ToString(dtDetails.Rows[0]["RA_ADDRESS"].ToString());
                    objAgency.AgencyPhoneNo = Convert.ToString(dtDetails.Rows[0]["RA_PHNO"].ToString());
                    objAgency.AgencyEmail = Convert.ToString(dtDetails.Rows[0]["RA_MAIL"].ToString());

                    foreach (DataRow dtRow in dtDetails.Rows)
                    {
                        objAgency.sOffCode += Convert.ToString(dtRow["AM_OFFICE_CODE"]);
                       
                    }
                    //objAgency.sOffCode = Convert.ToString(dtDetails.Rows[0]["AM_OFFICE_CODE"]);
                    objAgency.sStatus = Convert.ToString(dtDetails.Rows[0]["RA_STATUS"]);

                }
                return objAgency;
            }


            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "GetRepairerDetails");
                return objAgency;
            }
            finally
            {

            }

        }

        public bool ActiveDeactiveRepairer(clsAgency objAgency)
        {
            try
            {



                string strQry = string.Empty;
                //strQry = "UPDATE TBLTRANSREPAIRER SET TR_STATUS='" + objAgency.sStatus + "' WHERE TR_ID='" + objAgency.AgencyId + "'";
                strQry = "UPDATE TBLTRANSREPAIRER SET TR_EFFECT_FROM =TO_DATE('" + objAgency.sDeactivateFrom + "','dd/MM/yyyy'), ";
                strQry += " TR_STATUS = '" + objAgency.sStatus + "', ";
                strQry += " TR_DEACTIVATE_REASON = '" + objAgency.sDeactivateReason + "'  ";
                if (objAgency.sStatus == "D")
                {
                    strQry += " , TR_BLACK_LISTED = '1', TR_BLACKED_UPTO = TO_DATE('" + objAgency.sDeactivateTo + "','dd/MM/yyyy')  ";
                    //TR_BLACKED_UPTO = TO_DATE('" + objAgency.sDeactivateTo + "','dd/MM/yyyy')
                }
                strQry += " WHERE TR_ID  = '" + objAgency.AgencyId + "'";
                ObjCon.Execute(strQry);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "ActiveDeactiveRepairer");
                return false;

            }

        }

        public bool ActiveDeactiveMasterAgency(clsAgency objAgency)
        {
            bool status = false;
            string strQry = string.Empty;
            try
            {
                strQry = "update TBLAGENCYDIVMAPPING set AM_STATUS = '" + objAgency.sMasterStatus + "'  WHERE  AM_ID =  '" + objAgency.sMappingId + "' ";
                ObjCon.Execute(strQry);
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "ActiveDeactiveMasterAgency");
                return status;

            }

        }

        //save only repairer details 
        public string[] SaveAgencyMasterDetails(clsAgency objAgency)
        {
        
            string[] Arr = new string[2];
            try
            {

                string strQry = string.Empty;
                string[] strQryVallist = null;
                string[] strexistedoffcode = null;
                OleDbDataReader dr;
                // for new repairer
                if (objAgency.AgencyId == "")
                {
                    dr = ObjCon.Fetch("SELECT RA_NAME,RA_PHNO,RA_MAIL,RA_ADDRESS  FROM TBLREPAIRERAGENCYMASTER WHERE  RA_NAME = '" + objAgency.AgencyName + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Repairer Agency Name already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT RA_NAME, RA_PHNO, RA_MAIL,RA_ADDRESS  FROM TBLREPAIRERAGENCYMASTER  WHERE   RA_PHNO = '" + objAgency.AgencyPhoneNo + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Phone No already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT RA_NAME, RA_PHNO, RA_MAIL,RA_ADDRESS  FROM TBLREPAIRERAGENCYMASTER WHERE   RA_MAIL = '" + objAgency.AgencyEmail + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Email already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    if (objAgency.sOffCode != "")
                    {
                        strQryVallist = objAgency.sOffCode.Split(',');
                    }

                    string sResult = string.Empty;
                 
                        string StrGetMaxNo = ObjCon.Get_max_no("RA_ID", "TBLREPAIRERAGENCYMASTER").ToString();

                        strQry = "INSERT INTO TBLREPAIRERAGENCYMASTER(RA_ID,RA_NAME,RA_ADDRESS,RA_PHNO,RA_MAIL,RA_STATUS)";
                        strQry += " VALUES('" + StrGetMaxNo + "','" + objAgency.AgencyName + "',";
                        strQry += "'" + objAgency.AgencyAddress + "','" + objAgency.AgencyPhoneNo + "',";
                        strQry += " '" + objAgency.AgencyEmail + "','A')";
                        ObjCon.Execute(strQry);
                        Arr[0] = StrGetMaxNo.ToString();
                        Arr[1] = "0";


                    foreach (string sOffCode in strQryVallist)
                    {
                        string GetMaxNo = ObjCon.Get_max_no("AM_ID", "TBLAGENCYDIVMAPPING").ToString();

                        strQry = "INSERT INTO TBLAGENCYDIVMAPPING(AM_ID,AM_RA_ID,AM_OFFICE_CODE,AM_CR_ON,AM_STATUS)";
                        strQry += " VALUES('" + GetMaxNo + "','" + StrGetMaxNo + "',";
                        strQry += "'" + sOffCode + "',SYSDATE,'A')";
                        ObjCon.Execute(strQry);
                    }
                    return Arr;
                }

                //for updating the Agency
                else
                {

                    dr = ObjCon.Fetch("SELECT RA_NAME,RA_PHNO,RA_MAIL  FROM TBLREPAIRERAGENCYMASTER WHERE  RA_NAME = '" + objAgency.AgencyName + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        strQry = "UPDATE TBLREPAIRERAGENCYMASTER SET RA_NAME='" + objAgency.AgencyName + "',RA_ADDRESS='" + objAgency.AgencyAddress + "',";
                        strQry += "RA_PHNO='" + objAgency.AgencyPhoneNo + "',RA_MAIL='" + objAgency.AgencyEmail + "'";
                        strQry += " WHERE RA_NAME= '" + objAgency.AgencyName + "'";
                        ObjCon.Execute(strQry);
                        Arr[0] = "Updated Successfully ";
                        Arr[1] = "1";
                        //return Arr;

                    }
                    dr.Close();
                    if (objAgency.sOffCode != "")
                    {
                        strQryVallist = objAgency.sOffCode.Split(',');
                    }

                    if (!objAgency.sExistedOffCode.Equals(""))
                    {
                       
                        foreach (string existed in objAgency.sExistedOffCode)
                        {
                            if (!strQryVallist.Contains(Convert.ToString(existed)))
                            {
                                strQry = "UPDATE TBLAGENCYDIVMAPPING SET AM_STATUS='D',AM_UPDATED_ON=SYSDATE";
                                strQry += " WHERE AM_OFFICE_CODE= '" + existed + "' AND AM_RA_ID= '" + objAgency.AgencyId + "' ";
                                ObjCon.Execute(strQry);
                            }
                        }
                    }

                    foreach (string sOffCode in strQryVallist)
                    {
                        string sQry = string.Empty;
                        sQry = "SELECT \"AM_ID\" FROM \"TBLAGENCYDIVMAPPING\" WHERE \"AM_RA_ID\" = '" + objAgency.AgencyId+"' AND  \"AM_OFFICE_CODE\" = '" + sOffCode + "'";
                    
                        string sResult2 = ObjCon.get_value(sQry);

                        if (sResult2.Length > 0)
                        {
                            
                            strQry = "UPDATE TBLAGENCYDIVMAPPING SET AM_OFFICE_CODE='" + sOffCode + "',AM_RA_ID='" + objAgency.AgencyId + "',AM_UPDATED_ON=SYSDATE";                          
                            strQry += " WHERE AM_ID= '" + sResult2 + "'";
                            ObjCon.Execute(strQry);
                            Arr[0] = "Updated Successfully ";
                            Arr[1] = "1";

                        }
                        else
                        {
                            string StrGetMaxNo = ObjCon.Get_max_no("AM_ID", "TBLAGENCYDIVMAPPING").ToString();
                            strQry = "INSERT INTO TBLAGENCYDIVMAPPING(AM_ID,AM_RA_ID,AM_OFFICE_CODE,AM_CR_ON,AM_STATUS)";
                            strQry += " VALUES('" + StrGetMaxNo + "','" + objAgency.AgencyId + "',";
                            strQry += "'" + sOffCode + "',SYSDATE,'A')";                
                            ObjCon.Execute(strQry);

                        }
                        

                    }
                    return Arr;

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsAgency", "ActiveDeactiveRepairer");
                return Arr;
            }
        }
    }
}
