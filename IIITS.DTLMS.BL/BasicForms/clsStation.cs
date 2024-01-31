using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using IIITS.DAL;
using System.Data;


namespace IIITS.DTLMS.BL
{
   public class clsStation
    {
        CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        public Int64 StationId { get; set; }
        public string OfficeCode { get; set; }
        public string StationCode { get; set; }   
        public string StationName { get; set; }
        public string Description { get; set; }
        public string UserLogged { get; set; }
        public string Capacity { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string StationParentID { get; set; }
        public string OfficeName { get; set; }
        public bool IsSave { get; set; }
        public string talukcode { get; set; }

        string strFormCode = "clsStation";

        OleDbCommand oledbcommand;
        public string[] SaveStationDetails(clsStation ObjStation)
        {
            oledbcommand = new OleDbCommand();
            string[] Arrmsg = new string[3];
            try
            {
                       
                if (ObjStation.IsSave)
                {
                     //Check For dup;licate Station Code
                    oledbcommand.Parameters.AddWithValue("StationCode", ObjStation.StationCode);
                    OleDbDataReader dr = objCon.Fetch("select ST_ID FROM TBLSTATION WHERE ST_STATION_CODE=:StationCode and  ST_TQ_CODE = '" + ObjStation.talukcode + "' ", oledbcommand);
                    if (dr.Read())
                    {
                        Arrmsg[0] = "Station Code  Already Exist";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("StationName", ObjStation.StationName);
                    dr = objCon.Fetch("select * FROM TBLSTATION WHERE ST_NAME=:StationName", oledbcommand);
                    if (dr.Read())
                    {
                        Arrmsg[0] = "Station Name  Already Exist";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr.Close();

                    long slno = objCon.Get_max_no("ST_ID", "TBLSTATION");
                    string strInsqry = "insert into TBLSTATION(ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_MOBILE_NO,ST_EMAILID,ST_OFF_CODE,ST_STC_CAP_ID,ST_ENTRY_AUTH,ST_PARENT_STATID,ST_TQ_CODE) ";
                    strInsqry += " values(:slno,:StationCode,:StationName,";
                    strInsqry += ":Description ,:MobileNo,:EmailId, ";
                    strInsqry += ":OfficeCode,:Capacity,:UserLogged,:StationParentID,:talukcode)";
                    OleDbCommand command = new OleDbCommand();
                    ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("slno", slno);
                    command.Parameters.AddWithValue("StationCode", ObjStation.StationCode);
                    command.Parameters.AddWithValue("StationName", ObjStation.StationName.Replace("'", "''"));


                    command.Parameters.AddWithValue("Description", ObjStation.Description.ToUpper().Replace("'", "''"));
                    command.Parameters.AddWithValue("MobileNo", ObjStation.MobileNo.Trim());
                    command.Parameters.AddWithValue("EmailId", ObjStation.EmailId.Trim());

                    command.Parameters.AddWithValue("OfficeCode", ObjStation.OfficeCode);
                    command.Parameters.AddWithValue("Capacity", ObjStation.Capacity);
                    command.Parameters.AddWithValue("UserLogged", ObjStation.UserLogged);
                    command.Parameters.AddWithValue("StationParentID", ObjStation.StationParentID);
                    command.Parameters.AddWithValue("talukcode", ObjStation.talukcode);

                    objCon.Execute(strInsqry, command);

                    UpdateBankBusDetails(ObjStation , slno);


                    TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    string result = objWCF.SendStationDetailstoTrms(ObjStation.StationName.Replace("'", "''"), ObjStation.StationCode, ObjStation.Capacity, ObjStation.MobileNo.Trim(), ObjStation.EmailId.Trim(), ObjStation.Description.ToUpper().Replace("'", "''"), ObjStation.UserLogged, ObjStation.OfficeCode);


                    Arrmsg[0] = "Station Information has been Saved Sucessfully.";
                    Arrmsg[1] = "0";
                    Arrmsg[2] = Convert.ToString(slno);
                    return Arrmsg;

                }
                else
                {
                    oledbcommand = new OleDbCommand();
                    //Check For dup;licate Station Code
                    oledbcommand.Parameters.AddWithValue("stationcode1", ObjStation.StationCode);
                    oledbcommand.Parameters.AddWithValue("StationId1", ObjStation.StationId);
                    OleDbDataReader dr = objCon.Fetch("select ST_ID FROM TBLSTATION WHERE ST_STATION_CODE=:stationcode1 AND ST_ID<>:StationId1", oledbcommand);
                    if (dr.Read())
                    {
                        Arrmsg[0] = "Station Code Already Exist";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("stationname1", ObjStation.StationName);
                    oledbcommand.Parameters.AddWithValue("stationid1", ObjStation.StationId);
                    dr = objCon.Fetch("select * FROM TBLSTATION WHERE ST_NAME=:stationname1 AND ST_ID<>:stationid1", oledbcommand);
                    if (dr.Read())
                    {
                        Arrmsg[0] = "Station Name Already Exist";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr.Close();

                    string strUpdqry = "update TBLSTATION set ST_NAME=:StationName,ST_STATION_CODE=:StationCode,";
                    strUpdqry += " ST_DESCRIPTION=:Description,ST_UPDATED_BY=:UserLogged,ST_UPDATED_ON=SYSDATE,";
                    strUpdqry += " ST_STC_CAP_ID=:Capacity,ST_MOBILE_NO=:MobileNo,ST_EMAILID=:EmailId, ";
                    strUpdqry += " ST_OFF_CODE =" + ObjStation.OfficeCode + ",ST_TQ_CODE=" + ObjStation.talukcode + ",";
                    strUpdqry += "ST_PARENT_STATID=:StationParentID    where ST_ID =:StationId";
                    OleDbCommand command = new OleDbCommand();
                    ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("StationName", ObjStation.StationName.Replace("'", "''"));
                    command.Parameters.AddWithValue("StationCode", ObjStation.StationCode);
                    command.Parameters.AddWithValue("Description", ObjStation.Description.ToUpper().Replace("'", "''"));
                    command.Parameters.AddWithValue("UserLogged", ObjStation.UserLogged);
                    command.Parameters.AddWithValue("Capacity", ObjStation.Capacity);
                    command.Parameters.AddWithValue("MobileNo", ObjStation.MobileNo.Trim());
                    command.Parameters.AddWithValue("EmailId", ObjStation.EmailId.Trim());
                    command.Parameters.AddWithValue("StationParentID", ObjStation.StationParentID);
                    command.Parameters.AddWithValue("StationId", ObjStation.StationId);
                    // command.Parameters.AddWithValue("stationOffCode", ObjStation.OfficeCode);

                    // command.Parameters.AddWithValue("OfficeCode", ObjStation.OfficeCode);

                    TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    string result = objWCF.SendupdateStationDetailstoTrms(ObjStation.StationName.Replace("'", "''"), ObjStation.StationCode, ObjStation.Capacity, ObjStation.MobileNo.Trim(), ObjStation.EmailId.Trim(), ObjStation.Description.ToUpper().Replace("'", "''"), ObjStation.UserLogged, ObjStation.OfficeCode);



                    objCon.Execute(strUpdqry, command);

                    Arrmsg[0] = "Station Information has been Updated Sucessfully.";
                    Arrmsg[1] = "0";
                   
                    return Arrmsg;

                }
               
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveStationDetails");
                return Arrmsg;
            }
        }

        private void UpdateBankBusDetails(clsStation objStation, long slno)
        {
            string strQry = string.Empty;
            string sBankId = string.Empty;
            try
            {
                sBankId = Convert.ToString(objCon.Get_max_no("BN_ID", "TBLBANK"));
                strQry = "INSERT  INTO TBLBANK VALUES ( '"+ sBankId + "' , 'BANK-1' ,'"+ slno + "' , 'BANK-1' , 1, SYSDATE  )";
                objCon.Execute(strQry);
                sBankId = sBankId+ "~"+ Convert.ToString(objCon.Get_max_no("BN_ID", "TBLBANK"));
                strQry = "INSERT  INTO TBLBANK VALUES ( '" + sBankId.Split('~')[1] + "' , 'BANK-2' ,'" + slno + "' , 'BANK-2' , 1, SYSDATE  )";
                objCon.Execute(strQry);

                for (int i = 0; i < 2; i++)
                {
                    strQry = " insert  into TBLBUS values ((SELECT max(BS_ID)+1 FROM TBLBUS ) , 'BUS-"+Convert.ToInt32( i+1) +"' , '"+ sBankId.Split('~')[i] + "' , 'BUS-" + Convert.ToInt32(i + 1) + "' , 1, SYSDATE  ) ";
                    objCon.Execute(strQry);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateBankBusDetails");
               
            }
        }

        public DataTable LoadStationDet(string strStationID="", string sLocation = "")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtStationDet = new DataTable();
            try
            {

                //SELECT substr(OFF_NAME,instr(OFF_NAME,' ',1,2)) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(44,1,1) and LENGTH(4) =1 as Circle

                 string strQry = string.Empty;
                 strQry = " SELECT ST_ID,TO_CHAR(ST_STATION_CODE) ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,STC_CAP_VALUE,ST_PARENT_STATID , TQ_NAME , DT_NAME ,  CIRCLE ,  DIVISION , ST_OFF_CODE";
                 strQry += " FROM  (SELECT ST_ID,ST_STATION_CODE,ST_PARENT_STATID,ST_NAME,ST_DESCRIPTION,STC_CAP_VALUE , TQ_NAME , DT_NAME";
              strQry += " , (SELECT substr(OFF_NAME,instr(OFF_NAME,':',1,1)+1)  FROM VIEW_ALL_OFFICES WHERE LENGTH(OFF_CODE) = 1 and OFF_CODE = SUBSTR(ST_OFF_CODE,0,1)) CIRCLE,";
              strQry += " (SELECT substr(OFF_NAME,instr(OFF_NAME,':',1,1)+1)  FROM VIEW_ALL_OFFICES WHERE LENGTH(OFF_CODE) = 2 and OFF_CODE = ST_OFF_CODE ) DIVISION ,ST_OFF_CODE ";
                 strQry += " FROM TBLSTATION left join TBLSTATIONCAPACITY on ST_STC_CAP_ID = STC_CAP_ID    left join TBLTALQ on TQ_CODE = SUBSTR(ST_STATION_CODE, 0, 2) left join TBLDIST on DT_CODE = SUBSTR(ST_STATION_CODE, 0, 1) ";
                 strQry += " WHERE  ST_STC_CAP_ID=STC_CAP_ID";

               if (strStationID != "")
               {
                   oledbcommand.Parameters.AddWithValue("StationId", strStationID);
                   strQry += " AND ST_ID=:StationId";
               }
               if (sLocation.Length > 0)
               {
                   oledbcommand.Parameters.AddWithValue("location", sLocation);
                   strQry += " AND ST_OFF_CODE LIKE :location||'%'";
               }
               strQry += "  ) STATION GROUP BY ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,STC_CAP_VALUE,ST_PARENT_STATID,CIRCLE,DIVISION,ST_OFF_CODE,  TQ_NAME , DT_NAME ORDER BY ST_STATION_CODE ";
               DtStationDet = objCon.getDataTable(strQry, oledbcommand);
               
               return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStationDet");
                return DtStationDet;
            }
        }


        public DataTable LoadStationDetail(string strStationID = "")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtStationDet = new DataTable();
            try
            {
                string strQry = string.Empty;

                strQry = " SELECT ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_STC_CAP_ID, ST_MOBILE_NO, ST_EMAILID,ST_OFF_CODE,CIRCLE,DIVISION";
               strQry += " FROM  (SELECT ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_STC_CAP_ID,";
               strQry += "  ST_MOBILE_NO, ST_EMAILID ,ST_OFF_CODE ";
              strQry += " ,(SELECT substr(OFF_NAME,instr(OFF_NAME,':',1,1)+1)  FROM VIEW_ALL_OFFICES WHERE LENGTH(OFF_CODE) = 1 and OFF_CODE = SUBSTR(ST_OFF_CODE,0,1))  CIRCLE,";
             strQry += "(SELECT substr(OFF_NAME,instr(OFF_NAME,':',1,1)+1)  FROM VIEW_ALL_OFFICES WHERE LENGTH(OFF_CODE) = 2 and OFF_CODE = ST_OFF_CODE ) DIVISION FROM TBLSTATION";

                if (strStationID != "")
                {
                    oledbcommand.Parameters.AddWithValue("StationId", Convert.ToInt32( strStationID));
                    strQry += " WHERE ST_ID=:StationId ";
                }
                strQry += "  ) STATION GROUP BY ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_STC_CAP_ID, ST_MOBILE_NO, ST_EMAILID,ST_OFF_CODE,CIRCLE , DIVISION  ";
                 DtStationDet = objCon.getDataTable(strQry, oledbcommand);
               
                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStationDetail");
                return DtStationDet;
            }
        }


        public DataTable LoadOfficeDet(clsStation objStation)
        {
            oledbcommand = new OleDbCommand();
            DataTable DtStationDet = new DataTable();
            try
            {
                string strQry = string.Empty;

                strQry = "select OFF_CODE,OFF_NAME FROM VIEW_ALL_OFFICES WHERE  OFF_CODE IS NOT NULL ";
                if (objStation.OfficeCode != "")
                {
                    oledbcommand.Parameters.AddWithValue("OfficeCode", objStation.OfficeCode);
                    strQry += " AND OFF_CODE LIKE :OfficeCode||'%'";
                }
                if (objStation.OfficeName  != "")
                {
                    oledbcommand.Parameters.AddWithValue("OfficeName", objStation.OfficeName);
                    strQry += " AND OFF_NAME LIKE :OfficeName||'%'";
                }
                strQry+= " order by OFF_CODE";
                 DtStationDet = objCon.getDataTable(strQry, oledbcommand);
               
                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadOfficeDet");
                return DtStationDet;
            }
        }

        public string GenerateSectionCode(clsStation objStation)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbcommand.Parameters.AddWithValue("StationCode", objStation.StationCode);
                string sStaCode = objCon.get_value("SELECT NVL(MAX(ST_STATION_CODE),0)+1 FROM TBLSTATION  where  substr(ST_STATION_CODE,1,2) =:StationCode", oledbcommand);
                if (sStaCode.Length > 0)
                {
                    StationCode = sStaCode.ToString();
                }
                return sStaCode;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateOmSecCode");
                return "";
            }
        }

        public string AutoGenerateStationCode(string Talq)
        {
            string final = string.Empty;
            try
            {

                string sLastStationCode;
                string strQry = string.Empty;

                 char last, lastupdated;

                //strQry = "select max(ST_STATION_CODE) from TBLSTATION where ST_OFF_CODE='" + Talq + "' and ST_STATION_CODE like '" + Talq + "%' ";
                strQry = "select max(ST_STATION_CODE) from TBLSTATION where ST_TQ_CODE='" + Talq + "' and ST_STATION_CODE like '" + Talq + "%' ";


                sLastStationCode = objCon.get_value(strQry);

                if (sLastStationCode.Length > 2)
                {
                    string s = sLastStationCode.Substring(2);

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(s);

                    if (asciiBytes[0] >= 48 && asciiBytes[0] <= 57)
                    {
                        int temp = Convert.ToInt32(s);
                        temp = temp + 1;

                        if (temp.ToString().Length == 1)
                        {
                            //   string strTemp = "0" + Convert.ToString(temp);
                            string strTemp = Convert.ToString(temp);
                            final = sLastStationCode.Substring(0, 2) + strTemp;
                            return final;
                        }

                        if (temp > 9)
                        {
                            final = "A";
                        }
                        else
                        {
                            final = Convert.ToString(temp);
                        }

                    }
                    else
                    {

                        s.ToUpper();
                        char[] arr = s.ToCharArray();
                        last = lastupdated = arr[0];

                        lastupdated++;
                        if (lastupdated.Equals('['))
                        {
                            lastupdated = 'A';
                        }

                        final = Convert.ToString(lastupdated);
                    }
                }
                else //  if there are not station code in the feeder 
                {
                    final = Talq + "1";
                    return final;
                }

                //      final = AutoGenerateCode(sLastStationCode);
                //strQry = "select ST_STATION_CODE from TBLSTATION where ST_STATION_CODE='" + sLastStationCode.Substring(0, 2) + final + "'";
                //string sCode = objCon.get_value(strQry);
                //if (sCode != "")
                //{
                //    //do
                //    //{

                //    //    final = AutoGenerateCode(sLastStationCode.Substring(0, 2) + final);
                //    //    final = sLastStationCode.Substring(0, 2) + final;
                //    //    strQry = "select ST_STATION_CODE from TBLSTATION where ST_STATION_CODE='" + final + "'";
                //    //    sCode = objCon.get_value(strQry);
                //    //} while (sCode != "");
                //    //return final;

                //     final = AutoGenerateCode(sLastStationCode.Substring(0, 2) + final);
                //  //  return final;

                //}
                //   final = AutoGenerateCode(sLastStationCode);

                final = sLastStationCode.Substring(0, 2) + final;
                return final;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AutoGenerateDTCCode");
                return final;
            }
        }

        public string AutoGenerateCode(string sLastStationCode)
        {
            string final = string.Empty;
            char last, lastupdated;
            if (sLastStationCode.Length > 2)
                {
                    string s = sLastStationCode.Substring(2);

        byte[] asciiBytes = Encoding.ASCII.GetBytes(s);

                    if (asciiBytes[0] >= 48 && asciiBytes[0] <= 57)
                    {
                        int temp = Convert.ToInt32(s);
        temp = temp + 1;


                        if (temp.ToString().Length == 1)
                        {
                            string strTemp = "0" + Convert.ToString(temp);
        final = sLastStationCode.Substring(0, 2) + strTemp;
                            return final;
                        }

                        if (temp > 9)
                        {
                            final = "A";
                        }
                        else
                        {
                            final = Convert.ToString(temp);
                        }

                    }
                    else
                    {

                        s.ToUpper();
                        char[] arr = s.ToCharArray();
                     last = lastupdated = arr[0];

                        lastupdated++;
                        if (lastupdated.Equals('['))
                        {
                            lastupdated = 'A';                           
                        }
                       
                        final =  Convert.ToString(lastupdated);
                    }
                }
                else //  if there are not station code in the feeder 
                {
                    final = talukcode + "1";
                    return final;
                }
            return final;
        }


    }
}

