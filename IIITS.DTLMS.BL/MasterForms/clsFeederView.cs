using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;


namespace IIITS.DTLMS.BL
{
    public class clsFeederView
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsFeederView";
        OleDbCommand oledbcommand;
        public DataTable LoadFeederMastDet(string sOfficeCode,string strFeederName="",string strFeederCode="",string sStationName="",string sStationCode="")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtFeederfDet = new DataTable();
            string strQry = string.Empty;
            try
            {              

                    if (sOfficeCode.Length >= 4)
                    {
                        sOfficeCode = sOfficeCode.Substring(0, 3);
                    }
                    oledbcommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                    strQry = " SELECT * FROM (SELECT FD_FEEDER_NAME,FD_FEEDER_CODE,FD_FEEDER_ID,(SELECT DIV_NAME   FROM TBLDIVISION  WHERE SUBSTR(OFF_CODE ,1 , 2 ) = DIV_CODE   ) as div_name  , SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+1) OFF_NAME, ";
                    strQry += " (SELECT ST_NAME FROM TBLSTATION WHERE ST_ID=FD_ST_ID ) ST_NAME,  (SELECT ST_STATION_CODE FROM TBLSTATION WHERE ST_ID=FD_ST_ID ) ST_STATION_CODE , FT_NAME as FD_TYPE ";
                    strQry += " FROM TBLFDRTYPE , TBLFEEDERMAST,TBLFEEDEROFFCODE,VIEW_ALL_OFFICES WHERE  FD_FEEDER_TYPE = FT_ID and  FD_FEEDER_CODE IS NOT NULL";
                    strQry += " AND FDO_FEEDER_ID=FD_FEEDER_ID AND OFF_CODE=FDO_OFFICE_CODE AND FDO_OFFICE_CODE LIKE :OfficeCode||'%' Order BY FD_FEEDER_ID) WHERE FD_FEEDER_ID IS NOT NULL AND ST_NAME IS NOT NULL ";                   
                    if (strFeederName != "")
                    {
                        oledbcommand.Parameters.AddWithValue("FeederName", strFeederName.ToUpper());
                        strQry += " AND UPPER(FD_FEEDER_NAME) like :FeederName||'%'";
                    }
                    if (strFeederCode != "")
                    {
                        oledbcommand.Parameters.AddWithValue("FeederCode", strFeederCode.ToUpper());
                        strQry += " AND UPPER(FD_FEEDER_CODE) like :FeederCode||'%'";
                    }
                    if (sStationName != "")
                    {
                        oledbcommand.Parameters.AddWithValue("StationName", sStationName.ToUpper());
                        strQry += " AND UPPER(ST_NAME) like :StationName||'%' ";
                    }
                if (sStationCode != "")
                {
                  
                    strQry += " AND UPPER(ST_STATION_CODE) like  '" + sStationCode+"%' ";
                }

                strQry += " ORDER BY FD_FEEDER_CODE  ";
                DtFeederfDet = ObjCon.getDataTable(strQry, oledbcommand);
                   

                    return DtFeederfDet;
                }

                catch (Exception ex)
                {                   
                    clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadFeederMastDet");
                    return DtFeederfDet;
                }            

            }
        }
    }
