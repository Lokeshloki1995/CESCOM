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
    public class clsTcMaintainance
    {

        string strFormCode = "clsTcMaintainance";
       
        public string sMaintainanceId { get; set; }
        public string sMaintainanceDetailsId { get; set; }
        public string sTcCode { get; set; }
        public string sDTCCode { get; set; }
        public string sTmDate { get; set; }    
        public string sDescription { get; set; }
        public string sDTCName { get; set; }
        
        public string sRadiator { get; set; }       
        public string sCrBy { get; set; }
        public string sCrOn { get; set; }

        public string sFeederId { get; set; }
        public string sOfficeCode { get; set; }

        //New
        public string sSupports { get; set; }
        public string sBreather { get; set; }
        public string sEarthing { get; set; }
        public string sDangerPlate { get; set; }
        public string sAntiClimbing { get; set; }
        public string sExplosion { get; set; }
        public string sConditionNuts { get; set; }
        public string sLTWsitch { get; set; }
        public string sArrestor { get; set; }
        public string sGOSwitches { get; set; }
        public string sConnections { get; set; }
        public string sFuses { get; set; }
        public string sOilLeakage { get; set; }
        public string sBushing { get; set; }
        public string sArcing { get; set; }
        public string sMaintainType { get; set; }
        public string sMaintainDate { get; set; }
        public string sMaintainBy { get; set; }
        public string sVoltage { get; set; }
        public string sLoadBalancing { get; set; }
        public string sEarthTesting { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbcommand;

       


        public string[] SaveUpdateTcMaintainance(clsTcMaintainance objTcMaintainance)
        {
            
            string strQry = string.Empty;
            string[] Arr = new string[2];
            DataTable dt1 = new DataTable();
            try
            {
               
               // ObjCon.BeginTrans();
                oledbcommand = new OleDbCommand();
                strQry = "SELECT DT_CODE FROM TBLDTCMAST WHERE DT_CODE =:DTCode";
                oledbcommand.Parameters.AddWithValue("DTCode", objTcMaintainance.sDTCCode);
                dt1 = ObjCon.getDataTable(strQry, oledbcommand);
                if (dt1.Rows.Count == 0 )
                {
                    
                    Arr[0] = "Enter Valid DTC Code";
                    Arr[1] = "2";
                    return Arr;
                }
               
                if (objTcMaintainance.sMaintainanceId == "" )
                {

                    //Check Maintainance Entry
                    bool bResult = CheckMaintainanceEntry(objTcMaintainance);
                    if (bResult == false)
                    {
                        if (objTcMaintainance.sMaintainType == "1")
                        {
                            Arr[0] = "Maintainance has been done for DTC Code "+ objTcMaintainance.sDTCCode +" Quarterly";
                            Arr[1] = "2";
                            return Arr;
                        }
                        if (objTcMaintainance.sMaintainType == "2")
                        {
                            Arr[0] = "Maintainance has been done for DTC Code " + objTcMaintainance.sDTCCode + " Half Yearly";
                            Arr[1] = "2";
                            return Arr;
                        }
                    
                    }
                    oledbcommand = new OleDbCommand();
                    objTcMaintainance.sMaintainanceId = Convert.ToString(ObjCon.Get_max_no("TM_ID", "TBLDTCMAINTENANCE"));
                    strQry = "Insert into TBLDTCMAINTENANCE (TM_ID,TM_DT_CODE,TM_TC_CODE,TM_DATE,TM_MAINTAIN_TYPE,TM_MAINTAIN_BY,";
                    strQry += " TM_CRBY,TM_CRON) VALUES (:sMaintainanceId,:sDTCCode, ";
                    strQry += " :sTcCode,TO_DATE(:sMaintainDate,'DD/MM/YYYY'), ";
                    strQry += " :sMaintainType,:sMaintainBy,";
                    strQry += " :sCrBy,SYSDATE )";
                    oledbcommand.Parameters.AddWithValue("sMaintainanceId", objTcMaintainance.sMaintainanceId);
                    oledbcommand.Parameters.AddWithValue("sDTCCode", objTcMaintainance.sDTCCode);
                    oledbcommand.Parameters.AddWithValue("sTcCode", objTcMaintainance.sTcCode);
                    oledbcommand.Parameters.AddWithValue("sMaintainDate", objTcMaintainance.sMaintainDate);
                    oledbcommand.Parameters.AddWithValue("sMaintainType", objTcMaintainance.sMaintainType);
                    oledbcommand.Parameters.AddWithValue("sMaintainBy", objTcMaintainance.sMaintainBy);
                    oledbcommand.Parameters.AddWithValue("sCrBy", objTcMaintainance.sCrBy);
                    
                    ObjCon.Execute(strQry, oledbcommand);


                    oledbcommand = new OleDbCommand();
                    objTcMaintainance.sMaintainanceDetailsId = Convert.ToString(ObjCon.Get_max_no("DMD_ID", "TBLDTCMAINTAINDETAILS"));
                    strQry = "Insert into TBLDTCMAINTAINDETAILS (DMD_ID,DMD_TM_ID,DMD_SUPPORTS,DMD_CONNECTIONS,DMD_FUSES, ";
                    strQry += " DMD_OIL,DMD_BUSHING,DMD_ARCING,DMD_BREATHER,DMD_EARTHING,DMD_DANGER,DMD_ANTI_CLIMB,";
                    strQry += " DMD_EXPLOSION,DMD_CONDITION_NUTS,DMD_LT_SWITCH,DMD_LIGHT_ARRESTER,DMD_GO_SWITCHES,DMD_VOTAGE,DMD_LOAD_BALANCE,DMD_EARTH_TESTING)";
                    strQry += "VALUES (:sMaintainanceDetailsId,:sMaintainanceId12, ";
                    strQry += " :sSupports12,:sConnections12,:sFuses12,:sOilLeakage12,:sBushing12,";
                    strQry += ":sArcing12,:sBreather12,:sEarthing12,:sDangerPlate12,";
                    strQry += ":sAntiClimbing12,:sExplosion12,:sConditionNuts12,";
                    strQry += ":sLTWsitch12,:sArrestor12,:sGOSwitches12,:sVoltage12,:sLoadBalancing12,:sEarthTesting12)";

                    oledbcommand.Parameters.AddWithValue("sMaintainanceDetailsId", objTcMaintainance.sMaintainanceDetailsId);
                    oledbcommand.Parameters.AddWithValue("sMaintainanceId12", objTcMaintainance.sMaintainanceId);
                    oledbcommand.Parameters.AddWithValue("sSupports12", objTcMaintainance.sSupports);
                    oledbcommand.Parameters.AddWithValue("sConnections12", objTcMaintainance.sConnections);
                    oledbcommand.Parameters.AddWithValue("sFuses12", objTcMaintainance.sFuses);
                    oledbcommand.Parameters.AddWithValue("sOilLeakage12", objTcMaintainance.sOilLeakage);
                    oledbcommand.Parameters.AddWithValue("sBushing12", objTcMaintainance.sBushing);

                    oledbcommand.Parameters.AddWithValue("sArcing12", objTcMaintainance.sArcing);
                    oledbcommand.Parameters.AddWithValue("sBreather12", objTcMaintainance.sBreather);
                    oledbcommand.Parameters.AddWithValue("sEarthing12", objTcMaintainance.sEarthing);
                    oledbcommand.Parameters.AddWithValue("sDangerPlate12",  objTcMaintainance.sDangerPlate);
                    oledbcommand.Parameters.AddWithValue("sAntiClimbing12", objTcMaintainance.sAntiClimbing);
                    oledbcommand.Parameters.AddWithValue("sExplosion12", objTcMaintainance.sExplosion);
                    oledbcommand.Parameters.AddWithValue("sConditionNuts12", objTcMaintainance.sConditionNuts);

                    oledbcommand.Parameters.AddWithValue("sLTWsitch12", objTcMaintainance.sLTWsitch);
                    oledbcommand.Parameters.AddWithValue("sArrestor12", objTcMaintainance.sArrestor);
                    oledbcommand.Parameters.AddWithValue("sGOSwitches12", objTcMaintainance.sGOSwitches);
                    oledbcommand.Parameters.AddWithValue("sVoltage12", objTcMaintainance.sVoltage);
                    oledbcommand.Parameters.AddWithValue("sLoadBalancing12", objTcMaintainance.sLoadBalancing);
                    oledbcommand.Parameters.AddWithValue("sEarthTesting12", objTcMaintainance.sEarthTesting);
                    //oledbcommand.Parameters.AddWithValue("sConditionNuts12", objTcMaintainance.sConditionNuts);

                    ObjCon.Execute(strQry, oledbcommand);

                    oledbcommand = new OleDbCommand();
                    strQry = "UPDATE TBLDTCMAST SET DT_LAST_SERVICE_DATE=TO_DATE(:sMaintainDate122,'DD/MM/YYYY') WHERE DT_CODE=:sDTCCode122";
                    oledbcommand.Parameters.AddWithValue("sMaintainDate122", objTcMaintainance.sMaintainDate);
                    oledbcommand.Parameters.AddWithValue("sDTCCode122", objTcMaintainance.sDTCCode);
                    ObjCon.Execute(strQry, oledbcommand);


                    string strDate = string.Empty;
                    string sMaintType = objTcMaintainance.sMaintainType;
                    if (sMaintType == "1")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(3);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Saved Successfully and Next Maintenance Date will be " + strDate;
                    }
                    if (sMaintType == "2")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(6);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Saved Successfully and Next Maintenance Date will be " + strDate;
                    }

                    ObjCon.CommitTrans();

                   
                    Arr[1] = "0";
                    return Arr;
                }

                else
                {

                    oledbcommand = new OleDbCommand();
                    strQry = "UPDATE TBLDTCMAINTENANCE SET TM_MAINTAIN_BY=:sMaintainBy12, ";
                    strQry += "TM_DATE=to_date(:sMaintainDate12,'DD/MM/YYYY') where TM_ID=:sMaintainanceId12";
                    oledbcommand.Parameters.AddWithValue("sMaintainBy12", objTcMaintainance.sMaintainBy);
                    oledbcommand.Parameters.AddWithValue("sMaintainDate12", objTcMaintainance.sMaintainDate);
                    oledbcommand.Parameters.AddWithValue("sMaintainanceId12", objTcMaintainance.sMaintainanceId);
                    
                    ObjCon.Execute(strQry, oledbcommand);


                    oledbcommand = new OleDbCommand();
                    strQry = "UPDATE TBLDTCMAINTAINDETAILS SET DMD_SUPPORTS=:sSupports13, ";
                    strQry += "DMD_CONNECTIONS=:sConnections13,DMD_FUSES=:sFuses13,DMD_OIL=:sOilLeakage13,"; 
                    strQry += "DMD_BUSHING=:sBushing13,DMD_ARCING=:sArcing13,DMD_BREATHER=:sBreather13,";
                    strQry += "DMD_EARTHING=:sEarthing13,DMD_DANGER=:sDangerPlate13,DMD_ANTI_CLIMB=:sAntiClimbing132,";
                    strQry += "DMD_EXPLOSION=:sExplosion13,DMD_CONDITION_NUTS=:sConditionNuts13,DMD_LT_SWITCH=:sLTWsitch13,";
                    strQry += "DMD_LIGHT_ARRESTER=:sArrestor13,DMD_GO_SWITCHES=:sGOSwitches13,DMD_VOTAGE=:sVoltage13,";
                    strQry+= "DMD_LOAD_BALANCE=:sLoadBalancing14,";
                    strQry += "DMD_EARTH_TESTING=:sEarthTesting13 where DMD_ID=:sMaintainanceDetailsId13";

                    oledbcommand.Parameters.AddWithValue("sSupports13", objTcMaintainance.sSupports);
                    oledbcommand.Parameters.AddWithValue("sConnections13", objTcMaintainance.sConnections);
                    oledbcommand.Parameters.AddWithValue("sFuses13", objTcMaintainance.sFuses);


                    oledbcommand.Parameters.AddWithValue("sOilLeakage13", objTcMaintainance.sOilLeakage);
                    oledbcommand.Parameters.AddWithValue("sBushing13", objTcMaintainance.sBushing);
                    oledbcommand.Parameters.AddWithValue("sArcing13", objTcMaintainance.sArcing);

                    oledbcommand.Parameters.AddWithValue("sBreather13", objTcMaintainance.sBreather);
                    oledbcommand.Parameters.AddWithValue("sEarthing13", objTcMaintainance.sEarthing);
                    oledbcommand.Parameters.AddWithValue("sDangerPlate13", objTcMaintainance.sDangerPlate);

                    oledbcommand.Parameters.AddWithValue("sAntiClimbing132", objTcMaintainance.sAntiClimbing);
                    oledbcommand.Parameters.AddWithValue("sExplosion13", objTcMaintainance.sExplosion);
                    oledbcommand.Parameters.AddWithValue("sConditionNuts13", objTcMaintainance.sConditionNuts);

                    oledbcommand.Parameters.AddWithValue("sLTWsitch13", objTcMaintainance.sLTWsitch);
                    oledbcommand.Parameters.AddWithValue("sArrestor13", objTcMaintainance.sArrestor);
                    oledbcommand.Parameters.AddWithValue("sGOSwitches13", objTcMaintainance.sGOSwitches);

                    oledbcommand.Parameters.AddWithValue("sVoltage13", objTcMaintainance.sVoltage);
                   // oledbcommand.Parameters.AddWithValue("sLTWsitch13", objTcMaintainance.sLTWsitch);
                    oledbcommand.Parameters.AddWithValue("sLoadBalancing14", objTcMaintainance.sLoadBalancing);
                    oledbcommand.Parameters.AddWithValue("sEarthTesting13", objTcMaintainance.sEarthTesting);
                    oledbcommand.Parameters.AddWithValue("sMaintainanceDetailsId13", objTcMaintainance.sMaintainanceDetailsId);
                    ObjCon.Execute(strQry, oledbcommand);

                    string strDate = string.Empty;
                    string sMaintType = objTcMaintainance.sMaintainType;
                    if (sMaintType == "1")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(3);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Updated Successfully and Next Maintenance Date will be " + strDate;
                    }
                    if (sMaintType == "2")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(6);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Updated Successfully and Next Maintenance Date will be " + strDate;
                    }


                    ObjCon.CommitTrans();

                 
                    Arr[1] = "1";
                    return Arr;
                }
            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateTcMaintainance");
                return Arr;
            }
            finally
            {
                
            }
        }

        public DataTable LoadDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                
                string strQry = string.Empty;
                oledbcommand = new OleDbCommand();
              
                strQry = "SELECT TM_ID,TO_CHAR(TM_TC_CODE) TM_TC_CODE,TO_CHAR(TM_DT_CODE) TM_DT_CODE,CASE TM_MAINTAIN_TYPE WHEN 1 THEN 'QUARTERLY' WHEN 2 THEN 'HALF YEARLY' END TM_MAINTAIN_TYPE,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=TM_MAINTAIN_BY) TM_MAINTAIN_BY, DT_NAME AS DTCNAME,TO_CHAR(TM_DATE,'DD-MON-YYYY') TM_DATE FROM TBLDTCMAINTENANCE M,TBLDTCMAST";
                strQry += " WHERE M.ROWID IN (SELECT MAX(ROWID) FROM TBLDTCMAINTENANCE GROUP BY TM_DT_CODE, TM_TC_CODE ) AND ";
                strQry += " DT_CODE = TM_DT_CODE AND TM_MAINTAIN_TYPE=1 AND DT_OM_SLNO LIKE :OfficeCode||'%'";

                oledbcommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);


                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDTCMaintainance");
                return dt;
            }
            finally
            {
                
            }
        }

        public object GetMaintainaceDetails(clsTcMaintainance objTcMaintainance)
        {
            try
            {
                
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                oledbcommand = new OleDbCommand();
             
               // DMD_ID,DMD_SUPPORTS,DMD_CONDITION_NUTS,DMD_CONNECTIONS,DMD_FUSES,DMD_BUSHING,DMD_ARCING,DMD_BREATHER,
                //DMD_EARTHING,DMD_DANGER,DMD_ANTI_CLIMB,DMD_EXPLOSION,DMD_CONDITION_NUTS,DMD_LT_SWITCH,DMD_LIGHT_ARRESTER,DMD_GO_SWITCHES,DMD_VOTAGE,DMD_LOAD_BALANCE,DMD_EARTH_TESTING
                strQry = "SELECT * FROM TBLDTCMAINTENANCE,TBLDTCMAINTAINDETAILS WHERE TM_ID=DMD_TM_ID AND TM_ID =:sMaintainanceId";
                oledbcommand.Parameters.AddWithValue("sMaintainanceId", objTcMaintainance.sMaintainanceId);
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                if (dt.Rows.Count > 0)
                {
                    objTcMaintainance.sMaintainanceId = Convert.ToString(dt.Rows[0]["TM_ID"]);
                    objTcMaintainance.sTcCode = Convert.ToString(dt.Rows[0]["TM_TC_CODE"]);
                    objTcMaintainance.sDTCCode = Convert.ToString(dt.Rows[0]["TM_DT_CODE"]);
                   
                   
                    objTcMaintainance.sTmDate = Convert.ToDateTime(dt.Rows[0]["TM_DATE"]).ToString("dd/MM/yyyy");                   
                    objTcMaintainance.sDescription = Convert.ToString(dt.Rows[0]["TM_DESC"]);
                    objTcMaintainance.sMaintainBy = Convert.ToString(dt.Rows[0]["TM_MAINTAIN_BY"]);
                    objTcMaintainance.sMaintainType = Convert.ToString(dt.Rows[0]["TM_MAINTAIN_TYPE"]);

                    objTcMaintainance.sSupports = Convert.ToString(dt.Rows[0]["DMD_SUPPORTS"]);
                    objTcMaintainance.sConditionNuts = Convert.ToString(dt.Rows[0]["DMD_CONDITION_NUTS"]);
                    objTcMaintainance.sConnections = Convert.ToString(dt.Rows[0]["DMD_CONNECTIONS"]);
                    objTcMaintainance.sFuses = Convert.ToString(dt.Rows[0]["DMD_FUSES"]);
                    objTcMaintainance.sBushing = Convert.ToString(dt.Rows[0]["DMD_BUSHING"]);
                    objTcMaintainance.sArcing = Convert.ToString(dt.Rows[0]["DMD_ARCING"]);
                    objTcMaintainance.sBreather = Convert.ToString(dt.Rows[0]["DMD_BREATHER"]);
                    objTcMaintainance.sEarthing = Convert.ToString(dt.Rows[0]["DMD_EARTHING"]);
                    objTcMaintainance.sDangerPlate = Convert.ToString(dt.Rows[0]["DMD_DANGER"]);
                    objTcMaintainance.sAntiClimbing = Convert.ToString(dt.Rows[0]["DMD_ANTI_CLIMB"]);
                    objTcMaintainance.sExplosion = Convert.ToString(dt.Rows[0]["DMD_EXPLOSION"]);
                    objTcMaintainance.sLTWsitch = Convert.ToString(dt.Rows[0]["DMD_LT_SWITCH"]);
                    objTcMaintainance.sArrestor = Convert.ToString(dt.Rows[0]["DMD_LIGHT_ARRESTER"]);
                    objTcMaintainance.sGOSwitches = Convert.ToString(dt.Rows[0]["DMD_GO_SWITCHES"]);
                    objTcMaintainance.sVoltage = Convert.ToString(dt.Rows[0]["DMD_VOTAGE"]);
                    objTcMaintainance.sLoadBalancing = Convert.ToString(dt.Rows[0]["DMD_LOAD_BALANCE"]);
                    objTcMaintainance.sEarthTesting = Convert.ToString(dt.Rows[0]["DMD_EARTH_TESTING"]);
                    objTcMaintainance.sOilLeakage = Convert.ToString(dt.Rows[0]["DMD_OIL"]);

                    objTcMaintainance.sMaintainanceDetailsId = Convert.ToString(dt.Rows[0]["DMD_ID"]);
                    objTcMaintainance.sCrOn = Convert.ToString(dt.Rows[0]["TM_CRON"]);                  
                }

                return objTcMaintainance;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetMaintainaceDetails");
                return objTcMaintainance;
            }
            finally
            {
                
            }
        }

        public string  GetTCDetails(clsTcMaintainance objTcMaintainance)
        {
            try
            {
                oledbcommand = new OleDbCommand();
                DataTable dtSearchDetails = new DataTable();
                string strQry = string.Empty;
                strQry = "SELECT TC_SLNO  || '~' || TC_CODE || '~' || TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE  FROM TBLDTCMAST, TBLTCMASTER  WHERE DT_TC_ID=TC_CODE AND DT_CODE=:sDTCCode";
                oledbcommand.Parameters.AddWithValue("sDTCCode", objTcMaintainance.sDTCCode);
                return ObjCon.get_value(strQry, oledbcommand);
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTCDetails");
                return ex.Message;
            }
            finally
            {
                
            }

        }
        public DataTable LoadPrevMaintainance(clsTcMaintainance objMaintain)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = " SELECT DT_CODE,DT_NAME, TO_CHAR(TC_CAPACITY) AS CAPACITY, TO_CHAR(DT_LAST_SERVICE_DATE,'DD-MON-YYYY') AS  LAST_SERVICE_DATE ,";
                strQry += " TO_CHAR(ADD_MONTHS(DT_LAST_SERVICE_DATE,MP_PERIOD),'DD-MON-YYYY') EXPECTED_SERVICEDATE,TC_CODE FROM TBLDTCMAST,TBLTCMASTER,";
                strQry += " TBLMAINTAINANCEPERIOD WHERE DT_TC_ID = TC_CODE  AND TC_CAPACITY BETWEEN MP_FROM AND MP_TO AND(SYSDATE - DT_LAST_SERVICE_DATE) / 30 > MP_PERIOD ";
                strQry += " AND SUBSTR (DT_CODE, 1, 4) =:sFeederId";
                strQry += " AND DT_OM_SLNO LIKE :sOfficeCode||'%'";
                oledbcommand.Parameters.AddWithValue("sFeederId", objMaintain.sFeederId);
                oledbcommand.Parameters.AddWithValue("sOfficeCode", objMaintain.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadPrevMaintainance");
                return dt;
            }
            finally
            {
                
            }


        }


        public clsTcMaintainance GetDtcBasicDetails(clsTcMaintainance objMaintain)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
               string strQry = string.Empty;
               strQry = "SELECT DT_TC_ID,DT_CODE,DT_NAME FROM TBLDTCMAST WHERE DT_CODE=:sDTCCode ";
               oledbcommand.Parameters.AddWithValue("sDTCCode", objMaintain.sDTCCode);
               dt = ObjCon.getDataTable(strQry, oledbcommand);
               if (dt.Rows.Count > 0)
               {
                   objMaintain.sTcCode = Convert.ToString(dt.Rows[0]["DT_TC_ID"]);
                   objMaintain.sDTCCode = Convert.ToString(dt.Rows[0]["DT_CODE"]);
                   objMaintain.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
               }
               return objMaintain;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcBasicDetails");
                return objMaintain;
            }
            finally
            {
                
            }
        }

        public DataTable LoadDtcMaintainGrid(string sDTCCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                
                string strQry = string.Empty;
                strQry = "select TO_CHAR(TM_TC_CODE)TM_TC_CODE,TO_CHAR(TM_DT_CODE)TM_DT_CODE,TO_CHAR(TM_DATE,'dd/MM/yyyy')TM_DATE,";
                strQry += "CASE TM_MAINTAIN_TYPE WHEN 1 THEN 'QUARTERLY' WHEN 2 THEN 'HALF YEARLY' END MD_NAME,US_FULL_NAME ";
                strQry += " FROM TBLDTCMAINTENANCE,TBLUSER WHERE US_ID=TM_MAINTAIN_BY AND TM_DT_CODE=:sDTCCode";
                oledbcommand.Parameters.AddWithValue("sDTCCode", sDTCCode);
                dt = ObjCon.getDataTable(strQry, oledbcommand);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcMaintainGrid");
                return dt;
            }
            finally
            {
                
            }


        }

        public bool CheckMaintainanceEntry(clsTcMaintainance objMaintain)
        {
            try
            {
                oledbcommand = new OleDbCommand();
                
                string strQry = string.Empty;
                string sResult = string.Empty;
                strQry = "SELECT DATEDIFF FROM (SELECT FLOOR(SYSDATE-TM_DATE) DATEDIFF FROM TBLDTCMAINTENANCE WHERE TM_DT_CODE=:sDTCCode";
                strQry += " AND TM_MAINTAIN_TYPE=:sMaintainType ORDER BY TM_ID DESC) WHERE ROWNUM=1 ";
                oledbcommand.Parameters.AddWithValue("sDTCCode", objMaintain.sDTCCode);
                oledbcommand.Parameters.AddWithValue("sMaintainType", objMaintain.sMaintainType);
                sResult = ObjCon.get_value(strQry, oledbcommand);
                if (sResult != "")
                {
                    //Quarterly
                    if (objMaintain.sMaintainType == "1")
                    {
                        if (Convert.ToInt32(sResult) > 90)
                        {
                            return true;
                        }
                    }
                    //Half Yearly
                    if (objMaintain.sMaintainType == "2")
                    {
                        if (Convert.ToInt32(sResult) > 180)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckMaintainanceEntry");
                return false;
            }
            finally
            {
                
            }
        }
        public DataTable LoadQuarPendingDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbcommand = new OleDbCommand();
                
                strQry = " SELECT DT_CODE , DT_NAME, DT_TC_ID, DT_OM_SLNO,TO_CHAR(DT_LAST_SERVICE_DATE,'dd/MM/yyyy')DT_LAST_SERVICE_DATE, 'QUARTERLY' AS TM_MAINTAIN_TYPE FROM ";
                strQry += " (SELECT DT_CODE, DT_NAME, DT_TC_ID, DT_OM_SLNO, NVL(DT_LAST_SERVICE_DATE,SYSDATE-91) AS  DT_LAST_SERVICE_DATE FROM TBLDTCMAST) A,";
                strQry += " (SELECT TM_DT_CODE, MAX(TM_DATE) LAST_SERVICE_DATE, MIN(TM_MAINTAIN_TYPE) KEEP (DENSE_RANK LAST ORDER BY TM_DATE) TM_MAINTAIN_TYPE ";
                strQry += " FROM TBLDTCMAINTENANCE GROUP BY TM_DT_CODE)B";
                strQry += " WHERE A.DT_CODE = B.TM_DT_CODE(+) AND SYSDATE - DT_LAST_SERVICE_DATE > 90 AND DT_OM_SLNO LIKE :sOfficeCode||'%' AND (TM_MAINTAIN_TYPE = 1 OR TM_MAINTAIN_TYPE IS NULL)";
                oledbcommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message + "~" + strQry , strFormCode, "LoadQuarPendingDTCMaintainance");
                return dt;
            }
            finally
            {
                
            }
        }


        public DataTable LoadHalfYearlyPendingDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "   SELECT DT_CODE , DT_NAME, DT_TC_ID, DT_OM_SLNO, TO_CHAR(DT_LAST_SERVICE_DATE,'dd/MM/yyyy')DT_LAST_SERVICE_DATE, 'HALFYEARLY' AS TM_MAINTAIN_TYPE FROM ";
                strQry += " (SELECT DT_CODE, DT_NAME, DT_TC_ID, DT_OM_SLNO, DT_LAST_SERVICE_DATE FROM TBLDTCMAST) A,";
                strQry += " (SELECT TM_DT_CODE, MAX(TM_DATE) LAST_SERVICE_DATE, MIN(TM_MAINTAIN_TYPE) KEEP (DENSE_RANK LAST ORDER BY TM_DATE) TM_MAINTAIN_TYPE ";
                strQry += " FROM TBLDTCMAINTENANCE GROUP BY TM_DT_CODE)B";
                strQry += " WHERE A.DT_CODE = B.TM_DT_CODE(+) AND SYSDATE - DT_LAST_SERVICE_DATE > 180 AND DT_OM_SLNO LIKE :sOfficeCode||'%' AND TM_MAINTAIN_TYPE = 2 ";
                oledbcommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadHalfYearlyPendingDTCMaintainance");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadHalfYearDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT TM_ID,TO_CHAR(TM_TC_CODE) TM_TC_CODE,TO_CHAR(TM_DT_CODE) TM_DT_CODE,CASE TM_MAINTAIN_TYPE WHEN 1 THEN 'QUARTERLY' WHEN 2 THEN 'HALF YEARLY' END TM_MAINTAIN_TYPE,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=TM_MAINTAIN_BY) TM_MAINTAIN_BY, DT_NAME AS DTCNAME,TO_CHAR(TM_DATE,'DD-MON-YYYY') TM_DATE FROM TBLDTCMAINTENANCE M,TBLDTCMAST";
                strQry += " WHERE M.ROWID IN (SELECT MAX(ROWID) FROM TBLDTCMAINTENANCE GROUP BY TM_DT_CODE, TM_TC_CODE ) AND ";
                strQry += " DT_CODE = TM_DT_CODE  AND TM_MAINTAIN_TYPE=2 AND DT_OM_SLNO LIKE :sOfficeCode||'%'";
                oledbcommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oledbcommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadHalfYearDTCMaintainance");
                return dt;
            }
            finally
            {
                
            }
        }

    }

}

