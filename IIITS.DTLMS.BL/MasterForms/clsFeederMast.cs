using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Data.SQLite;

namespace IIITS.DTLMS.BL
{
    public class clsFeederMast
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strQry = string.Empty;
        public Int64 FeederID { get; set; }
        public string FeederCode { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string StationCode { get; set; }
        public Int64 Stationid { get; set; }
        public Int64 BankId { get; set; }
        public Int64 BusId { get; set; }
        public string FeederName { get; set; }
        public string FeederType { get; set; }
        public string FeederCategory { get; set; }
        public string FeederInterflow { get; set; }
        public string FeederDCC { get; set; }
        public string UserLogged { get; set; }
        public bool IsSave { get; set; }

        string strFormCode = "clsFeederMast";

        SQLiteConnection sql_con;
        OleDbCommand oledbcommand;
        public string[] FeederMaster(clsFeederMast objFeederMaster)
        {

            string[] Arrmsg = new string[2];
            try
            {

                string[] strQryVallist = null;

                if (objFeederMaster.OfficeCode != "")
                {
                    strQryVallist = objFeederMaster.OfficeCode.Split(',');

                }

                if (objFeederMaster.IsSave)
                {
                    string StrOfficeCodeExist = string.Empty;
                    oledbcommand = new OleDbCommand();
                    foreach (string OfficeCode in strQryVallist)
                    {

                        oledbcommand.Parameters.AddWithValue("FeederCode", objFeederMaster.FeederCode);
                        string strFeederId = ObjCon.get_value("select FD_FEEDER_ID FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE= :FeederCode", oledbcommand);


                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("FeederId", strFeederId);
                        oledbcommand.Parameters.AddWithValue("officecode", OfficeCode);
                        if (!ObjCon.get_value("SELECT count(*) FROM TBLFEEDEROFFCODE WHERE FDO_FEEDER_ID= :FeederId and FDO_OFFICE_CODE=:officecode", oledbcommand).ToString().Equals("0"))
                        {
                            StrOfficeCodeExist += OfficeCode + ",";
                        }
                        if (strFeederId != "")
                        {
                            //Arrmsg[0] = "Feeder Code For  Office Code/s '" + StrOfficeCodeExist + "' Already Present";
                            Arrmsg[0] = "Feeder Code  Already Present";
                            Arrmsg[1] = "4";
                            return Arrmsg;
                        }
                    }
                    if (StrOfficeCodeExist != "")
                    {
                        Arrmsg[0] = "Feeder Code  Already Present";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }

                    string strQry = string.Empty;

                    long slno = ObjCon.Get_max_no("FD_FEEDER_ID", "TBLFEEDERMAST");
                    strQry = "INSERT INTO TBLFEEDERMAST(FD_FEEDER_ID,FD_FEEDER_CODE,FD_FEEDER_NAME,";
                    strQry += " FD_CREATED_AUTH,FD_BS_ID,FD_FC_ID,FD_IS_INTERFLOW,FD_DTC_CAPACITY,FD_ST_ID,FD_FEEDER_TYPE)";
                    strQry += " VALUES(:slno ,:FeederCode,:FeederName,";
                    strQry += ":UserLogged,:BusId ,:FeederCategory,";
                    strQry += ":FeederInterflow ,:FeederDCC,:Stationid,:FeederType)";
                    OleDbCommand command = new OleDbCommand();

                    command.Parameters.AddWithValue("slno", slno);
                    command.Parameters.AddWithValue("FeederCode", objFeederMaster.FeederCode);
                    command.Parameters.AddWithValue("FeederName", objFeederMaster.FeederName.Replace("'", "''"));
                    command.Parameters.AddWithValue("UserLogged", objFeederMaster.UserLogged);
                    command.Parameters.AddWithValue("BusId", objFeederMaster.BusId);

                    command.Parameters.AddWithValue("FeederCategory", objFeederMaster.FeederCategory);
                    command.Parameters.AddWithValue("FeederInterflow", objFeederMaster.FeederInterflow);
                    command.Parameters.AddWithValue("FeederDCC", objFeederMaster.FeederDCC);
                    command.Parameters.AddWithValue("Stationid", objFeederMaster.Stationid);
                    command.Parameters.AddWithValue("FeederType", objFeederMaster.FeederType);
                    // command.Parameters.AddWithValue("Stationcode", objFeederMaster.StationCode);

                    ObjCon.Execute(strQry, command);

                    //TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    //string result = objWCF.SendFeederDetailstoTrms(objFeederMaster.FeederCode, FeederType, objFeederMaster.FeederDCC, objFeederMaster.FeederName.Replace("'", "''"), "0", OfficeCode, "0", Convert.ToString(objFeederMaster.Stationid));

                    foreach (string OfficeCode in strQryVallist)
                    {
                        strQry = "Insert into TBLFEEDEROFFCODE (FDO_ID,FDO_FEEDER_ID,FDO_OFFICE_CODE) VALUES ";
                        strQry += " ('" + ObjCon.Get_max_no("FDO_ID", "TBLFEEDEROFFCODE") + "',:slnoo,:OfficeCode)";
                        command = new OleDbCommand();
                        command.Parameters.AddWithValue("slnoo", slno);

                        command.Parameters.AddWithValue("OfficeCode", OfficeCode);

                        ObjCon.Execute(strQry, command);

                        TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                        string result = objWCF.SendFeederDetailstoTrms(objFeederMaster.FeederCode, FeederType, objFeederMaster.FeederDCC, objFeederMaster.FeederName.Replace("'", "''"), "0", OfficeCode, "0", Convert.ToString(objFeederMaster.StationCode));

                    }
                    foreach (string OfficeCode in strQryVallist)
                    {
                        TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                        string result = objWCF.SendupdateFeederDetailstoTrms(objFeederMaster.FeederCode, FeederType, objFeederMaster.FeederDCC, objFeederMaster.FeederName.Replace("'", "''"), OfficeCode, Convert.ToString(objFeederMaster.StationCode));
                    }

                    Arrmsg[0] = "Feeder Information Saved Successfully ";
                    Arrmsg[1] = "0";
                    return Arrmsg;
                }
                else
                {
                    string StrOfficeCodeExist = string.Empty;

                    string strFeederId = ObjCon.get_value("select FD_FEEDER_ID FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE= '" + objFeederMaster.FeederCode + "' and FD_FEEDER_ID <> '" + objFeederMaster.FeederID + "'");
                    if (strFeederId.Length > 0)
                    {
                        Arrmsg[0] = "Feeder Code  Already Exists";
                        Arrmsg[1] = "4";
                    }

                    strQry = "UPDATE TBLFEEDERMAST SET FD_FEEDER_CODE= '" + objFeederMaster.FeederCode + "',";
                    strQry += " FD_CREATED_DATE=SYSDATE ,FD_FC_ID='" + objFeederMaster.FeederCategory + "',";
                    strQry += " FD_FEEDER_NAME= '" + objFeederMaster.FeederName.Replace("'", "''") + "',";
                    strQry += " FD_BS_ID= " + objFeederMaster.BusId + "," +
                      " FD_DTC_CAPACITY=  " + Convert.ToInt64(objFeederMaster.FeederDCC) + ",FD_IS_INTERFLOW= " + Convert.ToInt64(objFeederMaster.FeederInterflow) + "," +
                      "FD_ST_ID= " + objFeederMaster.Stationid + " , FD_FEEDER_TYPE =  " + objFeederMaster.FeederType + " , FD_UPDATED_ON = SYSDATE , FD_UPDATED_BY  = '" + objFeederMaster.UserLogged + "'";
                    strQry += " where FD_FEEDER_ID =" + objFeederMaster.FeederID + "";

                    OleDbCommand command = new OleDbCommand();

                    //command.Parameters.AddWithValue("FeederCode", objFeederMaster.FeederCode);
                    //command.Parameters.AddWithValue("FeederName", objFeederMaster.FeederName.Replace("'", "''"));
                    //command.Parameters.AddWithValue("UserLogged", Convert.ToInt64(objFeederMaster.UserLogged));
                    //command.Parameters.AddWithValue("BusId", objFeederMaster.BusId);
                    //int iFeederId = Convert.ToInt32(objFeederMaster.FeederCategory);
                    //command.Parameters.AddWithValue("fd", BusId);
                    //command.Parameters.AddWithValue("FeederInterflow", Convert.ToInt64(objFeederMaster.FeederInterflow));
                    //command.Parameters.AddWithValue("FeederDCC", Convert.ToInt64(objFeederMaster.FeederDCC));
                    //command.Parameters.AddWithValue("Stationid", objFeederMaster.Stationid);
                    //command.Parameters.AddWithValue("FeederID", objFeederMaster.FeederID);
                    //command.Parameters.AddWithValue("FeederType", objFeederMaster.FeederType);

                    //ObjCon.Execute(strQry, command);
                    ObjCon.Execute(strQry);

                    strQry = " DELETE FROM TBLFEEDEROFFCODE WHERE FDO_FEEDER_ID = :FeederID";
                    command = new OleDbCommand();
                    command.Parameters.AddWithValue("FeederID", FeederID);
                    ObjCon.Execute(strQry, command);

                    foreach (string OfficeCode in strQryVallist)
                    {
                        strQry = "Insert into TBLFEEDEROFFCODE (FDO_ID,FDO_FEEDER_ID,FDO_OFFICE_CODE) VALUES ";
                        strQry += " ('" + ObjCon.Get_max_no("FDO_ID", "TBLFEEDEROFFCODE") + "',:FeederID1,:OfficeCode1)";
                        command = new OleDbCommand();
                        command.Parameters.AddWithValue("FeederID1", FeederID);
                        command.Parameters.AddWithValue("OfficeCode1", OfficeCode);

                        ObjCon.Execute(strQry, command);

                        TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                        string result = objWCF.SendupdateFeederDetailstoTrms(objFeederMaster.FeederCode, FeederType, objFeederMaster.FeederDCC, objFeederMaster.FeederName.Replace("'", "''"), OfficeCode, Convert.ToString(objFeederMaster.StationCode));

                    }
                    //foreach (string OfficeCode in strQryVallist)
                    //    {
                    //        TRM_Service.Service1Client objWCF = new TRM_Service.Service1Client();
                    //    string result = objWCF.SendupdateFeederDetailstoTrms(objFeederMaster.FeederCode, FeederType, objFeederMaster.FeederDCC, objFeederMaster.FeederName.Replace("'", "''"), OfficeCode, Convert.ToString(objFeederMaster.Stationid));
                    //}

                    Arrmsg[0] = "Feeder Information Updated Successfully ";
                    Arrmsg[1] = "0";
                    return Arrmsg;
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "FeederMaster", strQry);
                return Arrmsg;
            }
            finally
            {

            }

        }

        //public string FeederMast(string txtFeederCode, string cmbstationvalue, string txtName, string txtChangDate, string txtCTLowRange, string txtCTHighRange, string txtConstant, string txtLineVoltage, string cmbFeederTypevalue, string cmbSectionvalue, string txtCurLimit, string txtKWHReading, string cmbconscatvalue, string strUser)
        //{
        //    try
        //    {

        //        if (!objCon.get_value("SELECT count(*) FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE='1111' and FD_OFFICE_CODE='1112').ToString().Equals("0"))
        //        {
        //            return ("Feeder Code Already Present");
        //        }
        //        OleDbDataReader dr = objCon.Fetch("SELECT * FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        if (!dr.Read())
        //        {
        //            return ("Muss Code Not Found " + txtFeederCode.Substring(0, 2));
        //        }
        //        MussSlno = objCon.get_value("Select MU_SLNO from TBLMUSSMAST where MU_MUSS_CODE='" + cmbstationvalue.ToString() + "'");
        //        if (cmbconscatvalue == "10")
        //        {
        //            cmbconscatvalue = "0";
        //        }

        //        string mob = objCon.get_value("SELECT MU_MOBILE_NO FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        string qry = "INSERT INTO TBLFEEDERMAST(FD_FEEDER_ID,FD_MUSS_CODE,FD_NAME,FD_CHANGE_DT,FD_CT_LOW_RANGE,FD_CT_HIGH_RANGE,FD_CONST,FD_LINE_VOLTAGE,FD_SLNO,FD_TYPE,FD_MOBILE_NO,FD_OFFICE_CODE,FD_CUR_LIMIT,FD_KWH_READING,FD_CONS_CAT) VALUES('";
        //        qry = qry + txtFeederCode + "','";	//FDR CODE
        //        qry = qry + MussSlno + "','";				//MUSS CODE
        //        qry = qry + txtName + "',";		//FDR NAME
        //        qry = qry + "TO_DATE('" + txtChangDate + "','MM/DD/YYYY'),'";	//CT CHGD DATE3
        //        qry = qry + txtCTLowRange + "','";	//CT LOW RANGE
        //        qry = qry + txtCTHighRange + "','"; //CT HIGH RANGE
        //        qry = qry + txtConstant + "','";	//CONST
        //        qry = qry + txtLineVoltage + "','";	//LINE VOLTAGE
        //        qry = qry + slno + "','";	//FDR SLNO
        //        qry = qry + cmbFeederTypevalue + "','";	//FDR TYPE
        //        qry = qry + mob + "','";	//MOBILE NO
        //        qry = qry + cmbSectionvalue + "','";	//SECTION CODE
        //        qry = qry + txtCurLimit + "','";	//CUR LIMIT
        //        qry = qry + txtKWHReading + "','";	//KWH READNG
        //        qry = qry + cmbconscatvalue + "')";	//CONS CAT
        //        objCon.Execute(qry);


        //        // long bfslno = objCon.Get_max_no("BF_SLNO", "TBLBNKFDRLINK");
        //        //if (cmbFeederTypevalue.Equals("5") || cmbFeederTypevalue.Equals("6"))
        //        //{
        //        //   // objCon.Execute("ALTER TRIGGER  CESCDTC.BNK_FDR_VALIDATE DISABLE");
        //        //    objCon.Execute("INSERT INTO TBLBNKFDRLINK(BF_SLNO,BF_BNK_SLNO,BF_FDR_SLNO,BF_LINK_DATE,BF_CANCEL_FLAG,BF_ENTRY_AUTH) VALUES('" + bfslno + "','" + slno + "','" + slno + "',sysdate,0,'" + strUser + "')");
        //        //  //  objCon.Execute("ALTER TRIGGER  CESCDTC.BNK_FDR_VALIDATE ENABLE");
        //        //}
        //        //else if (!cmbIFPointvalue.Equals(string.Empty))
        //        //{
        //        //    objCon.Execute("INSERT INTO TBLBNKFDRLINK(BF_SLNO,BF_BNK_SLNO,BF_FDR_SLNO,BF_LINK_DATE,BF_CANCEL_FLAG,BF_ENTRY_AUTH) VALUES('" + bfslno + "','" + cmbIFPointvalue + "','" + slno + "',sysdate,0,'" + strUser + "')");
        //        //}
        //        return ("Feeder Information Saved Successfully " + txtFeederCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error:" + ex.Message;
        //    }


        //}


        //public string UpdateFeederDetails(string FdSlno, string txtFeederCode, string cmbstationvalue, string txtName, string txtChangDate, string txtCTLowRange, string txtCTHighRange, string txtConstant, string txtLineVoltage, string cmbFeederTypevalue, string cmbSectionvalue, string txtCurLimit, string txtKWHReading, string cmbconscatvalue, string strUser)
        //{
        //    try
        //    {
        //        OleDbDataReader dr;
        //        dr = objCon.Fetch("Select * from TBLFEEDERMAST where FD_SLNO='" + FdSlno + "'");
        //        if (!dr.Read())
        //        {
        //            return ("Feeder Does Not Exists");
        //        }
        //        dr.Close();

        //        dr = objCon.Fetch("SELECT * FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        if (!dr.Read())
        //        {
        //            return ("Muss Code Not Found " + txtFeederCode.Substring(0, 2));
        //        }

        //        MussSlno = objCon.get_value("Select MU_SLNO from TBLMUSSMAST where MU_MUSS_CODE='" + cmbstationvalue.ToString() + "'");
        //        if (cmbconscatvalue == "10")
        //        {
        //            cmbconscatvalue = "0";
        //        }
        //        string mob = objCon.get_value("SELECT MU_MOBILE_NO FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        string qry = "Update TBLFEEDERMAST set FD_FEEDER_ID='" + txtFeederCode + "',";
        //        qry += "FD_MUSS_CODE='" + MussSlno + "',";
        //        qry += "FD_NAME='" + txtName.Trim().ToUpper() + "',";
        //        if (txtChangDate.Trim().Length > 0)
        //        {
        //            qry += "FD_CHANGE_DT=TO_DATE('" + txtChangDate + "','MM/DD/YYYY'),";
        //        }
        //        qry += "FD_CT_LOW_RANGE='" + txtCTLowRange + "',";
        //        qry += "FD_CT_HIGH_RANGE='" + txtCTHighRange + "',";
        //        qry += "FD_CONST='" + txtConstant + "',";
        //        qry += "FD_LINE_VOLTAGE='" + txtLineVoltage + "',";
        //        qry += "FD_TYPE='" + cmbFeederTypevalue + "',";
        //        qry += "FD_MOBILE_NO='" + mob + "',";
        //        qry += "FD_OFFICE_CODE='" + cmbSectionvalue + "',";
        //        qry += "FD_CUR_LIMIT='" + txtCurLimit + "',";
        //        qry += "FD_KWH_READING='" + txtKWHReading + "',";
        //        qry += "FD_CONS_CAT='" + cmbconscatvalue + "' where FD_SLNO='" + FdSlno + "'";
        //        objCon.Execute(qry);
        //        return ("Feeder Information Updated Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error:" + ex.Message;
        //    }
        //}


        public DataTable LoadOfficeDet(clsFeederMast objStation)
        {
            oledbcommand = new OleDbCommand();
            DataTable DtStationDet = new DataTable();
            try
            {

                string strQry = string.Empty;

                strQry = "select OFF_CODE,OFF_NAME FROM VIEW_ALL_OFFICES,TBLSUBDIVMAST WHERE  OFF_NAME IS NOT NULL AND OFF_CODE = SD_SUBDIV_CODE ";
                if (objStation.OfficeCode != "")
                {

                    strQry += " AND OFF_CODE LIKE :officeCode||'%' ";
                    oledbcommand.Parameters.AddWithValue("officeCode", objStation.OfficeCode);
                }
                if (objStation.OfficeName != "")
                {
                    oledbcommand = new OleDbCommand();
                    strQry += " AND UPPER(OFF_NAME) LIKE : officeName||'%' ";
                    oledbcommand.Parameters.AddWithValue("officeName", objStation.OfficeName.ToUpper());
                }
                strQry += " order by OFF_CODE";
                DtStationDet = ObjCon.getDataTable(strQry, oledbcommand);

                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadOfficeDet");
                return DtStationDet;
            }
            finally
            {

            }

        }


        public DataTable LoadFeederMastDet(string strFeederID = "")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtFeederfDet = new DataTable();
            try
            {

                strQry = string.Empty;


                strQry = " SELECT FD_FEEDER_NAME,FD_FEEDER_CODE ,FD_FEEDER_ID,ST_NAME,BS_NAME,FC_NAME,LISTAGG(OFFNAME,',') WITHIN GROUP (ORDER BY OFFNAME) OFFNAME FROM";
                strQry += " (SELECT FD_FEEDER_NAME,FD_FEEDER_CODE ,FD_FEEDER_ID,(SELECT ST_NAME FROM TBLSTATION WHERE FD_ST_ID=ST_ID ) ST_NAME,";
                strQry += " (SELECT BS_NAME FROM TBLBUS WHERE FD_BS_ID=BS_ID) BS_NAME,(SELECT FC_NAME FROM TBLFEEDERCATEGORY WHERE  FD_FC_ID=FC_ID) FC_NAME,";
                strQry += " (SELECT OFF_NAME from VIEW_ALL_OFFICES WHERE OFF_CODE=FDO_OFFICE_CODE ) AS OFFNAME";
                strQry += " from TBLFEEDERMAST ,TBLFEEDEROFFCODE WHERE FD_FEEDER_ID=FDO_FEEDER_ID ";
                if (strFeederID != "")
                {
                    strQry += " AND FD_FEEDER_ID= :StrFeedId";
                    oledbcommand.Parameters.AddWithValue("StrFeedId", strFeederID);
                }
                strQry += " ) GROUP BY FD_FEEDER_NAME,FD_FEEDER_CODE ,FD_FEEDER_ID,ST_NAME,BS_NAME,FC_NAME ";
                DtFeederfDet = ObjCon.getDataTable(strQry, oledbcommand);


                return DtFeederfDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFeederMastDet");
                return DtFeederfDet;
            }
            finally
            {

            }

        }

        public DataTable GetFeederDetails(string strFeederID)
        {
            oledbcommand = new OleDbCommand();
            DataTable DtFeederfDet = new DataTable();
            try
            {

                strQry = string.Empty;

                //strQry = " SELECT FD_FEEDER_NAME, FD_DTC_CAPACITY, FD_IS_INTERFLOW, TO_CHAR(FD_FEEDER_CODE) FD_FEEDER_CODE,FD_FEEDER_ID,ST_ID,BS_ID,FC_ID,BN_ID,FC_FT_ID, LISTAGG( ";
                //strQry += "  FDO_OFFICE_CODE,',')   WITHIN GROUP (ORDER BY FDO_OFFICE_CODE) OFFCODE FROM  (SELECT FD_FEEDER_NAME,TO_CHAR(FD_FEEDER_CODE)FD_FEEDER_CODE, FD_DTC_CAPACITY,  ";
                //strQry += " FD_IS_INTERFLOW ,   FDO_OFFICE_CODE,FD_FEEDER_ID,ST_ID,BS_ID,FC_ID,BN_ID,FC_FT_ID from  TBLFEEDERMAST,TBLBUS,TBLFEEDERCATEGORY,TBLSTATION,TBLBANK, ";
                //strQry += " TBLFEEDEROFFCODE,TBLFDRTYPE  WHERE FD_FEEDER_ID = FDO_FEEDER_ID  AND  FD_ST_ID=ST_ID AND FD_BS_ID=BS_ID  AND FD_FC_ID=FC_ID AND BN_ID=BS_BN_ID AND FC_FT_ID=FT_ID  ";

                strQry = " SELECT FD_FEEDER_NAME, FD_DTC_CAPACITY, FD_IS_INTERFLOW, TO_CHAR(FD_FEEDER_CODE) FD_FEEDER_CODE,FD_FEEDER_ID,ST_ID,";
                strQry += " BS_ID,FC_ID,FC_FT_ID,BN_ID, LISTAGG(   FDO_OFFICE_CODE,',')   WITHIN GROUP (ORDER BY FDO_OFFICE_CODE) OFFCODE FROM  ";
                strQry += " (SELECT FD_FEEDER_NAME,TO_CHAR(FD_FEEDER_CODE)FD_FEEDER_CODE, FD_DTC_CAPACITY,   FD_IS_INTERFLOW ,   FDO_OFFICE_CODE,";
                strQry += " FD_FEEDER_ID,FD_ST_ID AS ST_ID,FD_BS_ID AS BS_ID,FD_FC_ID AS FC_ID, ";
                strQry += "  FD_FEEDER_TYPE as  FC_FT_ID,(SELECT DISTINCT BN_ID FROM TBLBANK,TBLBUS WHERE BS_BN_ID=BN_ID AND BS_ID=FD_BS_ID) BN_ID ";
                strQry += " from  TBLFEEDERMAST, TBLFEEDEROFFCODE  WHERE FD_FEEDER_ID = FDO_FEEDER_ID";

                if (strFeederID.Length > 0)
                {
                    oledbcommand.Parameters.AddWithValue("StrFeederId", strFeederID);
                    strQry += " AND FD_FEEDER_ID=:StrFeederId";
                }

                strQry += " ) GROUP BY FD_FEEDER_NAME, FD_FEEDER_CODE,FD_FEEDER_ID,ST_ID,BS_ID,FC_ID,BN_ID,FC_FT_ID,FD_DTC_CAPACITY, FD_IS_INTERFLOW ";

                DtFeederfDet = ObjCon.getDataTable(strQry, oledbcommand);


                return DtFeederfDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFeederDetails");
                return DtFeederfDet;
            }
            finally
            {

            }

        }

        public DataTable LoadViewAllOffices(string strOfficeCode = "", string strOfficeName = "")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtOfficeDet = new DataTable();
            try
            {

                strQry = string.Empty;


                strQry = "select OFF_CODE ,OFF_NAME  FROM VIEW_ALL_OFFICE";
                //if (strOfficeCode != "" && strOfficeName != "")
                //{
                oledbcommand.Parameters.AddWithValue("stroffCode", strOfficeCode);
                oledbcommand.Parameters.AddWithValue("offName", strOfficeName.ToUpper());
                strQry += " where (OFF_CODE like   : stroffCode||'%' and UPPER(OFF_NAME) like : offName||'%') ";
                //}

                //else if(strOfficeCode != ""  && strOfficeName == "") 
                //{
                //    strQry += " where (OFF_CODE like  '%" + strOfficeCode + "%') ";
                //}

                //else if (strOfficeCode == "" && strOfficeName != "")
                //{
                //    strQry += " where (UPPER(OFF_NAME) like  '%" + strOfficeName.ToUpper() + "%') ";
                //}
                strQry += "order by OFF_NAME";

                DtOfficeDet = ObjCon.getDataTable(strQry, oledbcommand);


                return DtOfficeDet;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadViewAllOffices");
                return DtOfficeDet;
            }
            finally
            {

            }

        }

        public void SetSqlLiteConnection(string sDbName)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                //SQLiteConnection sql_con;
                string sURL = Convert.ToString(ConfigurationSettings.AppSettings["SQLLiteDB"]);
                string relative_path = sURL + sDbName + ".db";
                //System.IO.File.Exists("\\192.168.4.2\\DEV_Download\\1111.db");
                sql_con = new SQLiteConnection
                ("Data Source=" + relative_path + ";Version=3;");
                sql_con.Open();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SetSqlLiteConnection");

            }
            finally
            {

            }

        }

        public void ExecuteSqliteQuery(string Query, string sDbName)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                SQLiteCommand sql_cmd;

                SetSqlLiteConnection(sDbName);
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = Query;
                sql_cmd.ExecuteNonQuery();
                sql_con.Close();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ExecuteQuery");
            }
            finally
            {

            }



        }
        public bool SyncFeederDetailstoApp(clsFeederMast objFeeder)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string[] strQryVallist = null;
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                if (objFeeder.OfficeCode != "")
                {
                    strQryVallist = objFeeder.OfficeCode.Split(',');

                    foreach (string OfficeCode in strQryVallist)
                    {
                        oledbcommand.Parameters.AddWithValue("offCod", OfficeCode);
                        strQry = "SELECT OM_CODE FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE=:offCod";
                        dt = ObjCon.getDataTable(strQry, oledbcommand);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sSection = Convert.ToString(dt.Rows[i]["OM_CODE"]);
                            //SetSqlLiteConnection(sSection);
                            SyncFeederDetails(OfficeCode, objFeeder.FeederCode, sSection);

                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SyncFeederDetailstoApp");
                return false;
            }
            finally
            {

            }

        }

        public void SyncFeederDetails(string sOfficeCode, string sFeederCode, string sSection)
        {
            oledbcommand = new OleDbCommand();

            try
            {

                DataSet ds = new DataSet();
                strQry = "DELETE FROM TBLFEEDERDETAILS WHERE FD_FEEDER_CODE='" + sFeederCode + "'";
                ExecuteSqliteQuery(strQry, sSection);

                oledbcommand.Parameters.AddWithValue("soffCode", sOfficeCode);

                strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_NAME,FDO_OFFICE_CODE FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE";
                strQry += " FD_FEEDER_ID = FDO_FEEDER_ID AND (FDO_OFFICE_CODE LIKE :soffCode ||'%' OR ";
                strQry += "LENGTH(FDO_OFFICE_CODE) = 0)";

                ds = ObjCon.GetDataset(strQry, oledbcommand);
                // arrFinal.Add(OperateDBName + " -  " + OperateDivisionCode + "-FEEDER-" + DS.Tables[0].Rows.Count.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strQry = "INSERT INTO TBLFEEDERDETAILS (FD_FEEDER_CODE, FD_FEEDER_NAME,FD_OFFICE_CODE) VALUES";
                        strQry += " ('" + Convert.ToString(ds.Tables[0].Rows[i][0]).Replace("'", "") + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]).Replace("'", "") + "',";
                        strQry += " '" + Convert.ToString(ds.Tables[0].Rows[i][2]).Replace("'", "") + "')";
                        ExecuteSqliteQuery(strQry, sSection);
                    }

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SyncFeederDtails");
            }
            finally
            {

            }



        }

    }
}
