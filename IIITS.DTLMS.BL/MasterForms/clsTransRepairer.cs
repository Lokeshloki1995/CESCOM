using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;

namespace IIITS.DTLMS.BL
{
    public class clsTransRepairer
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string RepairerId { get; set; }
        public string RepairerName { get; set; }
        public string RegisterAddress { get; set; }
        public string RepairerPhoneNo { get; set; }
        public string RepairerEmail { get; set; }
        public string RepairerType { get; set; }
        public string RepairerBlacklisted { get; set; }
        public string RepairerBlackedupto { get; set; }
        public string CommAddress { get; set; }
        public string sCrby { get; set; }

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
        public string sOilType { get; set; }
        public string sMasterStatus { get; set; }

        // for saving the repairer at the taluk level 
        public string[] SaveRepairerDetails(clsTransRepairer objRepairer)
        {
            string[] Arr = new string[2];

            try
            {

                string strQry = string.Empty;
                OleDbDataReader dr;
                // if update then we will have taluk id else its a new one 
                if (!(objRepairer.RepairerId == "" || objRepairer.RepairerId == null))
                {

                    //dr = ObjCon.Fetch("Select * from TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_NAME='" + objRepairer.RepairerName + "'");
                    //dr = ObjCon.Fetch("SELECT * FROM TBLTRANSREPAIRER, TBLREPAIRERDIV , TBLREPAIRERADDR WHERE RA_STATUS='A' AND    TR_ID = RD_TR_ID and RD = RA_RD_ID AND  TR_NAME = '" + objRepairer.RepairerName + "' and RD_DIV_CODE = '" + objRepairer.sOffCode + "' and  RA_TALQ_ID = '" + objRepairer.staluq + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Repairer Name already exists for that taluq and division";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();

                    //dr = ObjCon.Fetch("SELECT * FROM TBLTRANSREPAIRER, TBLREPAIRERDIV , TBLREPAIRERADDR WHERE RA_STATUS='A' AND    TR_ID = RD_TR_ID AND RD = RA_RD_ID  AND TR_PHONE='" + objRepairer.RepairerPhoneNo + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Phone No already exists";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();

                    // validatios 

                    //dr = ObjCon.Fetch("SELECT * FROM TBLREPAIRERADDR WHERE RA_TALQ_ID = "+objRepairer.staluq+"");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Repairer already exists for the selected Taluq";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();

                    //dr = ObjCon.Fetch("SELECT * FROM TBLTRANSREPAIRER , TBLREPAIRERDIV , TBLREPAIRERADDR WHERE TR_ID =  RD_TR_ID and RD = RA_RD_ID and TR_ID = " + objRepairer.RepairerId + " and  RD_DIV_CODE = " + objRepairer.sOffCode + "  and RA_TALQ_ID =" + objRepairer.staluq + " AND RA_ID <> '" + objRepairer.sRepairerTalukID + "' ");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = " "+objRepairer.RepairerName+" Repairer already exists for the taluk";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();
                    //dr = ObjCon.Fetch("SELECT * FROM TBLREPAIRERADDR WHERE RA_ID <> '"+objRepairer.sRepairerTalukID+"' AND RA_CONTACT_NO = '"+objRepairer.sMobileNo+"' ");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Contact number  already exists for the taluk";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();

                    // u need to validate this if its a new repairer .
                    //dr = ObjCon.Fetch("SELECT * FROM TBLTRANSREPAIRER, TBLREPAIRERDIV , TBLREPAIRERADDR WHERE RA_STATUS='A' AND    TR_ID = RD_TR_ID and RD = RA_RD_ID  AND TR_EMAIL='" + objRepairer.RepairerEmail + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "EmailId already exists";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();


                    // updating in the Taluk Table

                    //check if blacklisted is greater than then status = 'D'

                    if (objRepairer.RepairerBlacklisted == "1")
                    {
                        DateTime dateTime = DateTime.ParseExact(objRepairer.RepairerBlackedupto, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        if (dateTime < System.DateTime.Now)
                        {
                            objRepairer.sStatus = "D";
                        }
                        else
                        {
                            objRepairer.sStatus = "A";
                        }
                    }
                    else
                    {
                        objRepairer.sStatus = "A";
                    }

                    //strQry = "UPDATE TBLREPAIRERADDR SET RA_TALQ_ADDR = '" + objRepairer.CommAddress + "' ,RA_TALQ_ID  = '" + objRepairer.staluq + "' , RA_CONTACT_NO = '" + objRepairer.sMobileNo + "' ";
                    //strQry += " ,RA_BLACK_LISTED = '"+objRepairer.RepairerBlacklisted+"',RA_BLACK_LISTED_UPTO = TO_DATE('"+objRepairer.RepairerBlackedupto+"','dd/MM/yyyy') , RA_STATUS = '"+objRepairer.sStatus+"' " ;
                    //strQry += " , RA_CONTRACT_PERIOD = '" + objRepairer.sContractPeriod + "',RA_CONTACT_PERSON = '"+objRepairer.sContactPerson+"' WHERE RA_ID = " + objRepairer.sRepairerTalukID + " ";

                    strQry = "UPDATE TBLTRANSREPAIRER SET TR_NAME='" + objRepairer.RepairerName + "',TR_ADDRESS='" + objRepairer.RegisterAddress + "',";
                    strQry += "TR_PHONE='" + objRepairer.RepairerPhoneNo + "',TR_EMAIL='" + objRepairer.RepairerEmail + "',";
                    strQry += "TR_OFFICECODE='" + objRepairer.sOffCode + "',TR_BLACK_LISTED='" + objRepairer.RepairerBlacklisted + "',TR_COMM_ADDRESS='" + objRepairer.CommAddress + "',";
                    strQry += "TR_BLACKED_UPTO=TO_DATE('" + objRepairer.RepairerBlackedupto + "','dd/MM/yyyy') ,";
                    strQry += " TR_CONT_PERSON_NAME='" + objRepairer.sContactPerson + "',TR_CONTRACT_PERIOD='" + objRepairer.sContractPeriod + "',TR_FAX='" + sFax + "',TR_MOBILE_NO='" + objRepairer.sMobileNo + "' , TR_LOC_CODE = '" + objRepairer.staluq + "'";
                    strQry += " , TR_UPDATED_ON = SYSDATE , TR_UPDATED_BY = '" + objRepairer.sCrby + "' WHERE TR_ID= '" + objRepairer.RepairerId + "'";
                    ObjCon.Execute(strQry);



                    Arr[0] = "Updated Successfully";
                    Arr[1] = "0";
                    return Arr;
                    //string StrGetMaxNo = ObjCon.Get_max_no("TR_ID", "TBLTRANSREPAIRER").ToString();



                    //strQry = "INSERT INTO TBLTRANSREPAIRER(TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,";
                    //strQry += "TR_OFFICECODE,TR_BLACK_LISTED,TR_BLACKED_UPTO,TR_ENTRY_AUTH,TR_COMM_ADDRESS,TR_CONT_PERSON_NAME,TR_CONTRACT_PERIOD,TR_FAX,TR_MOBILE_NO)";
                    //strQry += " VALUES('" + StrGetMaxNo + "','" + objRepairer.RepairerName + "',";
                    //strQry += "'" + objRepairer.RegisterAddress + "','" + objRepairer.RepairerPhoneNo + "',";
                    //strQry += " '" + objRepairer.RepairerEmail + "','" + objRepairer.sOffCode + "',";
                    //strQry += "'" + objRepairer.RepairerBlacklisted + "',TO_DATE('" + objRepairer.RepairerBlackedupto + "','dd/MM/yyyy') ,";
                    //strQry += " '" + objRepairer.sCrby + "','" + objRepairer.CommAddress + "',";
                    //strQry += " '" + objRepairer.sContactPerson + "','" + objRepairer.sContractPeriod + "','" + sFax + "','" + objRepairer.sMobileNo + "')";
                    //ObjCon.Execute(strQry);
                    //Arr[0] = StrGetMaxNo.ToString();
                    //Arr[1] = "0";
                    //return Arr;
                }

                else
                {
                    // algo is 
                    // 1) check for the taluk  if exits then revert back 
                    // 2) check if the repairer  has been mapped to the DIV 
                    // 3) if mapped to DIV -> then dont insert into DIV .
                    // 4) if not mapped to DIV  - > then insert into DIV table and TALUK .
                    //dr = ObjCon.Fetch("SELECT * FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_NAME='" + objRepairer.RepairerName + "' AND TR_ID<>'" + objRepairer.RepairerId + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Repairer Name already exists";
                    //    Arr[1] = "4";
                    //    return Arr;
                    //}
                    //dr.Close();

                    //dr = ObjCon.Fetch("Select * from TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_PHONE='" + objRepairer.RepairerPhoneNo + "' AND TR_ID<>'" + objRepairer.RepairerId + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Phone No already exists";
                    //    Arr[1] = "4";

                    //    return Arr;
                    //}
                    //dr.Close();



                    // uncomment this code afterwords  

                    /*
                  dr = ObjCon.Fetch("SELECT * FROM TBLTRANSREPAIRER WHERE TR_STATUS = 'A' AND TR_LOC_CODE = '" + objRepairer.staluq + "'");
                  if (dr.Read())
                  {
                      dr.Close();
                      Arr[0] = "Repairer already exists for the Selected Taluk , Please deactivate the existing  Repairer";
                      Arr[1] = "4";
                      return Arr;
                  }
                  dr.Close();
                  */

                    //if it has not mapped to DIV then insert into the DIV Table
                    //DataTable dt = new DataTable();
                    //string divisionTableId = string.Empty;
                    //dt = ObjCon.getDataTable("SELECT * FROM TBLTRANSREPAIRER , TBLREPAIRERDIV WHERE TR_ID = RD_TR_ID  AND TR_ID = '" + objRepairer.RepairerId + "' AND RD_DIV_CODE = '" + objRepairer.sOffCode + "'");
                    //ObjCon.BeginTrans();
                    //if (dt.Rows.Count == 0)
                    //{
                    //    divisionTableId = ObjCon.Get_max_no("RD", "TBLREPAIRERDIV").ToString();
                    //    strQry = "INSERT into TBLREPAIRERDIV (RD , RD_TR_ID , RD_DIV_CODE ,RD_CRBY ) VALUES (" + divisionTableId + " ," + objRepairer.RepairerId + "," + objRepairer.sOffCode + ","+objRepairer.sCrby+" )";
                    //    ObjCon.Execute(strQry);
                    //}
                    //  else
                    //  {
                    //      divisionTableId = Convert.ToString(dt.Rows[0]["RD"]);
                    //  }

                    //strQry = "INSERT into TBLREPAIRERADDR (RA_ID , RA_TALQ_ADDR , RA_RD_ID , RA_TALQ_ID , RA_CONTACT_NO , RA_BLACK_LISTED  ";
                    //strQry += " , RA_BLACK_LISTED_UPTO  , RA_CONTRACT_PERIOD , RA_CRBY ,RA_CONTACT_PERSON ) VALUES ((SELECT NVL(MAX(RA_ID)+1,1) FROM TBLREPAIRERADDR) , ";
                    //strQry += " '" + objRepairer.CommAddress + "','" + divisionTableId + "','" + objRepairer.staluq + "','" + objRepairer.sMobileNo + "', ";
                    //strQry += " '" + objRepairer.RepairerBlacklisted + "',to_date('" + objRepairer.RepairerBlackedupto + "','dd/MM/yyyy'),'"+objRepairer.sContractPeriod+"','" + objRepairer.sCrby + "','" + objRepairer.sContactPerson + "') ";

                    //ObjCon.Execute(strQry);

                    //ObjCon.CommitTrans();

                    strQry = "SELECT TR_TR_ID  FROM TBLTRANSREPAIRER WHERE TR_NAME = '" + objRepairer.RepairerName + "'  GROUP BY TR_TR_ID";
                    string strMasterID = ObjCon.get_value(strQry);

                    dr = ObjCon.Fetch(" select TR_TR_ID from TBLTRANSREPAIRER where TR_STATUS = 'A' AND TR_OFFICECODE = '" + objRepairer.sOffCode + "'  and  TR_LOC_CODE = '" + objRepairer.staluq + "' and TR_TR_ID= '" + strMasterID + "' GROUP BY TR_TR_ID ");
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
                    strQry += " VALUES('" + StrGetMaxNo + "','" + objRepairer.RepairerName + "',";
                    strQry += "'" + objRepairer.RegisterAddress + "','" + objRepairer.RepairerPhoneNo + "',";
                    strQry += " '" + objRepairer.RepairerEmail + "','" + objRepairer.sOffCode + "',";
                    strQry += "'" + objRepairer.RepairerBlacklisted + "',TO_DATE('" + objRepairer.RepairerBlackedupto + "','dd/MM/yyyy') ,";
                    strQry += " '" + objRepairer.sCrby + "','" + objRepairer.CommAddress + "',";
                    strQry += " '" + objRepairer.sContactPerson + "','" + objRepairer.sContractPeriod + "','" + sFax + "','" + objRepairer.sMobileNo + "','" + strMasterID + "' , '" + objRepairer.staluq + "')";
                    ObjCon.Execute(strQry);


                    //strQry = "UPDATE TBLTRANSREPAIRER SET TR_NAME='" + objRepairer.RepairerName + "',TR_ADDRESS='" + objRepairer.RegisterAddress + "',";
                    //strQry += "TR_PHONE='" + objRepairer.RepairerPhoneNo + "',TR_EMAIL='" + objRepairer.RepairerEmail + "',";
                    //strQry += "TR_OFFICECODE='" + objRepairer.sOffCode + "',TR_BLACK_LISTED='" + objRepairer.RepairerBlacklisted + "',TR_COMM_ADDRESS='" + objRepairer.CommAddress + "',";
                    //strQry += "TR_BLACKED_UPTO=TO_DATE('" + objRepairer.RepairerBlackedupto + "','dd/MM/yyyy') ,";
                    //strQry += " TR_CONT_PERSON_NAME='" + objRepairer.sContactPerson + "',TR_CONTRACT_PERIOD='" + objRepairer.sContractPeriod + "',TR_FAX='" + sFax + "',TR_MOBILE_NO='" + objRepairer.sMobileNo + "'";
                    //strQry += " WHERE TR_ID= '" + objRepairer.RepairerId + "'";
                    //ObjCon.Execute(strQry);
                    Arr[0] = "Saved Successfully ";
                    Arr[1] = "0";
                    return Arr;

                }
            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "SaveRepairerDetails");
                Arr[0] = "Something Went Wrong Please Contact Support Team ";
                Arr[1] = "0";
                return Arr;
            }
            finally
            {

            }

        }

        public DataTable LoadRepairerDetails(clsTransRepairer objRepairer)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                //strQry = " Select TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,TR_COMM_ADDRESS,TR_STATUS,";
                //strQry += "  (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=TR_OFFICECODE)TR_OFFICECODE,";
                //strQry += "DECODE(TR_BLACK_LISTED,'0','NO','1','YES') AS TR_BLACK_LISTED,TO_CHAR(TR_BLACKED_UPTO,'DD-MON-YYYY') TR_BLACKED_UPTO ";
                //strQry += "FROM TBLTRANSREPAIRER ORDER BY TR_ID DESC";


                // before updating the repairere concept .
                //strQry = " Select TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,TR_COMM_ADDRESS,TR_ADDRESS,TR_STATUS,";
                //strQry += "  (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=TR_OFFICECODE)TR_OFFICECODE,";
                //strQry += "DECODE(TR_BLACK_LISTED,'0','NO','1','YES') AS TR_BLACK_LISTED,TO_CHAR(TR_BLACKED_UPTO,'DD-MON-YYYY') TR_BLACKED_UPTO ";
                //strQry += "FROM TBLTRANSREPAIRER where TR_OFFICECODE LIKE '" + objRepairer.sRepairOffCode + "%' ORDER BY TR_ID DESC";

                //after updating the concept in repairer .
                //strQry = "SELECT RA_ID,TR_ID , TR_NAME ,TR_PHONE,TR_EMAIL,TR_ADDRESS, RA_TALQ_ADDR  ,RA_CONTACT_NO , DECODE(RA_BLACK_LISTED ,'0','NO','1','YES') AS ";
                //strQry += "RA_BLACK_LISTED ,  TO_CHAR(RA_BLACK_LISTED_UPTO,'DD-MON-YYYY') RA_BLACK_LISTED_UPTO ,(SELECT DIV_NAME  FROM ";
                //strQry += "TBLDIVISION WHERE DIV_CODE = RD_DIV_CODE  ) DIVISION ,(SELECT TQ_NAME  FROM TBLTALQ WHERE TQ_SLNO=RA_TALQ_ID) ";
                //strQry += "TALUK ,RA_STATUS  FROM TBLTRANSREPAIRER , TBLREPAIRERDIV,TBLREPAIRERADDR WHERE TR_ID = RD_TR_ID AND  RD = RA_RD_ID  AND ";
                //strQry += " RD_DIV_CODE  LIKE '"+objRepairer.sRepairOffCode+"%'  ";

                // aftere nitin sir  concept 
                strQry = "SELECT B.TR_ID ,B.TR_TR_ID ,A.TR_NAME   , B.TR_PHONE , B.TR_EMAIL , B.TR_ADDRESS , B.TR_COMM_ADDRESS  AS RA_TALQ_ADDR  , B.TR_MOBILE_NO as RA_CONTACT_NO , ";
                strQry += " DECODE(B.TR_BLACK_LISTED  ,'0','NO','1','YES') AS RA_BLACK_LISTED , TO_CHAR(B.TR_BLACKED_UPTO ,'DD-MON-YYYY') RA_BLACK_LISTED_UPTO , ";
                strQry += " DIV_NAME AS DIVISION , TQ_NAME AS TALUK  , B.TR_STATUS  AS RA_STATUS    FROM TBLTRANSREPAIRER A  INNER JOIN TBLTRANSREPAIRER B ON  A.TR_ID = B.TR_TR_ID  LEFT JOIN   TBLTALQ  ";
                strQry += " ON B.TR_LOC_CODE  = TQ_SLNO LEFT JOIN TBLDIVISION ON DIV_CODE = B.TR_OFFICECODE  WHERE  B.TR_OFFICECODE LIKE '" + objRepairer.sRepairOffCode + "%'  ORDER BY B.TR_OFFICECODE  ";

                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "LoadRepairerDetails");
                return dt;
            }

        }

        public DataTable LoadRepairerMasterDetails(clsTransRepairer objRepairer)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                //strQry = " Select TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,TR_COMM_ADDRESS,TR_STATUS,";
                //strQry += "  (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=TR_OFFICECODE)TR_OFFICECODE,";
                //strQry += "DECODE(TR_BLACK_LISTED,'0','NO','1','YES') AS TR_BLACK_LISTED,TO_CHAR(TR_BLACKED_UPTO,'DD-MON-YYYY') TR_BLACKED_UPTO ";
                //strQry += "FROM TBLTRANSREPAIRER ORDER BY TR_ID DESC";

                // old one  
                strQry = " SELECT distinct TR_TR_ID , TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,TR_FAX,TR_MASTER_STATUS FROM TBLTRANSREPAIRER WHERE  TR_TR_ID = TR_ID AND TR_MASTER_STATUS = 'A' ORDER BY TR_NAME DESC";


                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "LoadRepairerDetails");
                return dt;
            }
            finally
            {

            }

        }

        // query has been  modified
        public object GetRepairerDetails(clsTransRepairer objRepairer)
        {
            DataTable dtDetails = new DataTable();
            String strQry = string.Empty;
            OleDbDataReader dr = null;
            try
            {
                //if (!(objRepairer.sRepairerTalukID  == "" || objRepairer.sRepairerTalukID  == null) )

                //strQry = "SELECT TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,TR_TYPE,TR_BLACK_LISTED,TR_COMM_ADDRESS,TR_CONTRACT_PERIOD,TR_OFFICECODE,";
                //strQry += "TO_CHAR(TR_BLACKED_UPTO,'DD-MM-YYYY') TR_BLACKED_UPTO,TR_CONT_PERSON_NAME,TR_FAX,TO_CHAR(TR_MOBILE_NO) TR_MOBILE_NO FROM TBLTRANSREPAIRER ";
                //strQry += " WHERE TR_ID='" + objRepairer.RepairerId + "'";
                // before nitin sir conecpt 
                //strQry = " SELECT TR_ID,RA_ID,TR_NAME,RA_STATUS,TR_ADDRESS,RA_TALQ_ADDR,RA_TALQ_ID  ,TR_PHONE,RA_CONTACT_NO,TR_EMAIL,RA_BLACK_LISTED , ";
                //strQry += " RA_CONTRACT_PERIOD ,RD_DIV_CODE ,TO_CHAR(RA_BLACK_LISTED_UPTO,'dd/MM/yyyy') RA_BLACK_LISTED_UPTO , ";
                //strQry += " RA_CONTACT_PERSON,TR_FAX,TO_CHAR(TR_MOBILE_NO) TR_MOBILE_NO FROM TBLTRANSREPAIRER,TBLREPAIRERDIV, ";
                //strQry += " TBLREPAIRERADDR WHERE TR_ID =RD_TR_ID and RD = RA_RD_ID   AND RA_ID='" + objRepairer.sRepairerTalukID + "'";


                strQry = "  SELECT TR_ID,'' AS RA_ID ,TR_NAME, TR_STATUS AS  RA_STATUS, TR_ADDRESS, TR_COMM_ADDRESS AS  RA_TALQ_ADDR , TR_LOC_CODE AS  RA_TALQ_ID  , TR_PHONE, ";
                strQry += " TR_MOBILE_NO as   RA_CONTACT_NO,TR_EMAIL, TR_BLACK_LISTED AS  RA_BLACK_LISTED ,TR_CONTRACT_PERIOD AS   RA_CONTRACT_PERIOD , TR_OFFICECODE AS  RD_DIV_CODE , ";
                //strQry += " TO_CHAR(TR_BLACKED_UPTO,'dd/MM/yyyy') RA_BLACK_LISTED_UPTO , TR_CONT_PERSON_NAME AS  RA_CONTACT_PERSON ,TR_FAX  FROM TBLTRANSREPAIRER  WHERE TR_ID = '"+objRepairer.RepairerId+"' ";
                strQry += " TO_CHAR(TR_BLACKED_UPTO,'dd/MM/yyyy') RA_BLACK_LISTED_UPTO , TR_CONT_PERSON_NAME AS  RA_CONTACT_PERSON ,TR_FAX ,TR_OIL_TYPE FROM TBLTRANSREPAIRER  WHERE TR_ID = '" + objRepairer.RepairerId + "' ";



                dr = ObjCon.Fetch(strQry);
                dtDetails.Load(dr);
                if (dtDetails.Rows.Count > 0)
                {
                    objRepairer.RepairerId = Convert.ToString(dtDetails.Rows[0]["TR_ID"].ToString());
                    objRepairer.RepairerName = Convert.ToString(dtDetails.Rows[0]["TR_NAME"].ToString());
                    objRepairer.RegisterAddress = Convert.ToString(dtDetails.Rows[0]["TR_ADDRESS"].ToString());
                    objRepairer.RepairerPhoneNo = Convert.ToString(dtDetails.Rows[0]["TR_PHONE"].ToString());
                    objRepairer.RepairerEmail = Convert.ToString(dtDetails.Rows[0]["TR_EMAIL"].ToString());
                    objRepairer.RepairerBlacklisted = Convert.ToString(dtDetails.Rows[0]["RA_BLACK_LISTED"].ToString());
                    objRepairer.RepairerBlackedupto = Convert.ToString(dtDetails.Rows[0]["RA_BLACK_LISTED_UPTO"].ToString());
                    objRepairer.CommAddress = Convert.ToString(dtDetails.Rows[0]["RA_TALQ_ADDR"].ToString());
                    objRepairer.sFax = Convert.ToString(dtDetails.Rows[0]["TR_FAX"].ToString());
                    objRepairer.sMobileNo = Convert.ToString(dtDetails.Rows[0]["RA_CONTACT_NO"].ToString());
                    objRepairer.sOffCode = Convert.ToString(dtDetails.Rows[0]["RD_DIV_CODE"]);
                    objRepairer.staluq = Convert.ToString(dtDetails.Rows[0]["RA_TALQ_ID"]);
                    objRepairer.sStatus = Convert.ToString(dtDetails.Rows[0]["RA_STATUS"]);
                    objRepairer.sRepairerTalukID = Convert.ToString(dtDetails.Rows[0]["RA_TALQ_ID"]);
                    objRepairer.sContactPerson = Convert.ToString(dtDetails.Rows[0]["RA_CONTACT_PERSON"]);
                    objRepairer.sContractPeriod = Convert.ToString(dtDetails.Rows[0]["RA_CONTRACT_PERIOD"]);
                    objRepairer.sOilType = Convert.ToString(dtDetails.Rows[0]["TR_OIL_TYPE"]);

                }
                return objRepairer;



                //strQry = "SELECT TR_ID , TR_NAME  , TR_PHONE , TR_ADDRESS , TR_EMAIL , TR_FAX , TR_CONT_PERSON_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = " + objRepairer.RepairerId + "";
                //dr = ObjCon.Fetch(strQry);
                //dtDetails.Load(dr);

                //if (dtDetails.Rows.Count > 0)
                //{
                //    objRepairer.RepairerId = Convert.ToString(dtDetails.Rows[0]["TR_ID"].ToString());
                //    objRepairer.RepairerName = Convert.ToString(dtDetails.Rows[0]["TR_NAME"].ToString());
                //    objRepairer.RegisterAddress = Convert.ToString(dtDetails.Rows[0]["TR_ADDRESS"].ToString());
                //    objRepairer.RepairerPhoneNo = Convert.ToString(dtDetails.Rows[0]["TR_PHONE"].ToString());
                //    objRepairer.RepairerEmail = Convert.ToString(dtDetails.Rows[0]["TR_EMAIL"].ToString());
                //    objRepairer.sContactPerson = Convert.ToString(dtDetails.Rows[0]["TR_CONT_PERSON_NAME"].ToString());
                //    objRepairer.sFax = Convert.ToString(dtDetails.Rows[0]["TR_FAX"].ToString());

                //}
                //return objRepairer;




            }


            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "GetRepairerDetails");
                return objRepairer;
            }
            finally
            {

            }

        }

        public bool ActiveDeactiveRepairer(clsTransRepairer objRepairer)
        {
            try
            {



                string strQry = string.Empty;
                //strQry = "UPDATE TBLTRANSREPAIRER SET TR_STATUS='" + objRepairer.sStatus + "' WHERE TR_ID='" + objRepairer.RepairerId + "'";
                strQry = "UPDATE TBLTRANSREPAIRER SET TR_EFFECT_FROM =TO_DATE('" + objRepairer.sDeactivateFrom + "','dd/MM/yyyy'), ";
                strQry += " TR_STATUS = '" + objRepairer.sStatus + "', ";
                strQry += " TR_DEACTIVATE_REASON = '" + objRepairer.sDeactivateReason + "'  ";
                if (objRepairer.sStatus == "D")
                {
                    strQry += " , TR_BLACK_LISTED = '1', TR_BLACKED_UPTO = TO_DATE('" + objRepairer.sDeactivateTo + "','dd/MM/yyyy')  ";
                    //TR_BLACKED_UPTO = TO_DATE('" + objRepairer.sDeactivateTo + "','dd/MM/yyyy')
                }
                strQry += " WHERE TR_ID  = '" + objRepairer.RepairerId + "'";
                ObjCon.Execute(strQry);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "ActiveDeactiveRepairer");
                return false;

            }

        }

        public bool ActiveDeactiveMasterRepairer(clsTransRepairer objRepairer)
        {
            bool status = false;
            string strQry = string.Empty;
            try
            {
                strQry = "update TBLTRANSREPAIRER set TR_MASTER_STATUS = '" + objRepairer.sMasterStatus + "'  WHERE  TR_ID =  '" + objRepairer.RepairerId + "' ";
                ObjCon.Execute(strQry);
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "ActiveDeactiveMasterRepairer");
                return status;

            }

        }

        //save only repairer details 
        public string[] SaveRepairerMasterDetails(clsTransRepairer objRepairer)
        {
            string[] Arr = new string[2];
            try
            {

                string strQry = string.Empty;
                OleDbDataReader dr;
                // for new repairer
                if (objRepairer.RepairerId == "")
                {
                    dr = ObjCon.Fetch("SELECT TR_NAME,TR_PHONE,TR_EMAIL  FROM TBLTRANSREPAIRER WHERE  TR_NAME = '" + objRepairer.RepairerName + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Repairer Name already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT TR_NAME,TR_PHONE,TR_EMAIL  FROM TBLTRANSREPAIRER WHERE   TR_PHONE = '" + objRepairer.RepairerPhoneNo + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Phone No already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT TR_NAME,TR_PHONE,TR_EMAIL  FROM TBLTRANSREPAIRER WHERE   TR_EMAIL = '" + objRepairer.RepairerEmail + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Email already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();


                    string StrGetMaxNo = ObjCon.Get_max_no("TR_ID", "TBLTRANSREPAIRER").ToString();

                    strQry = "INSERT INTO TBLTRANSREPAIRER(TR_ID,TR_NAME,TR_ADDRESS,TR_PHONE,TR_EMAIL,";
                    strQry += "TR_ENTRY_AUTH,TR_CONT_PERSON_NAME,TR_FAX)";
                    strQry += " VALUES('" + StrGetMaxNo + "','" + objRepairer.RepairerName + "',";
                    strQry += "'" + objRepairer.RegisterAddress + "','" + objRepairer.RepairerPhoneNo + "',";
                    strQry += " '" + objRepairer.RepairerEmail + "',";
                    strQry += " '" + objRepairer.sCrby + "',";
                    strQry += " '" + objRepairer.sContactPerson + "','" + sFax + "')";
                    ObjCon.Execute(strQry);
                    Arr[0] = StrGetMaxNo.ToString();
                    Arr[1] = "0";
                    return Arr;

                }
                //for updating the repairer
                else
                {
                    dr = ObjCon.Fetch("SELECT TR_NAME,TR_PHONE,TR_EMAIL  FROM TBLTRANSREPAIRER WHERE  TR_NAME = '" + objRepairer.RepairerName + "' AND TR_TR_ID <> '" + objRepairer.RepairerId + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Repairer Name already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT TR_NAME,TR_PHONE,TR_EMAIL  FROM TBLTRANSREPAIRER WHERE   TR_PHONE = '" + objRepairer.RepairerPhoneNo + "' AND TR_TR_ID <> '" + objRepairer.RepairerId + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Phone No already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT TR_NAME,TR_PHONE,TR_EMAIL  FROM TBLTRANSREPAIRER WHERE   TR_EMAIL = '" + objRepairer.RepairerEmail + "' AND TR_TR_ID <> '" + objRepairer.RepairerId + "'");

                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Email already exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    dr.Close();

                    strQry = "UPDATE TBLTRANSREPAIRER SET TR_NAME='" + objRepairer.RepairerName + "',TR_ADDRESS='" + objRepairer.RegisterAddress + "',";
                    strQry += "TR_PHONE='" + objRepairer.RepairerPhoneNo + "',TR_EMAIL='" + objRepairer.RepairerEmail + "',";
                    strQry += " TR_CONT_PERSON_NAME='" + objRepairer.sContactPerson + "',TR_FAX='" + sFax + "'";
                    strQry += ", TR_UPDATED_ON = SYSDATE , TR_UPDATED_BY = '" + objRepairer.sCrby + "'";
                    strQry += " WHERE TR_ID= '" + objRepairer.RepairerId + "'";
                    ObjCon.Execute(strQry);
                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsTransRepairer", "ActiveDeactiveRepairer");
                return Arr;
            }
        }
    }
}
