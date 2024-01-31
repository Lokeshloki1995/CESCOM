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
   public class clsDtcMapping
    {
       string strFormCode = "clsDtcMapping";

       CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sMappingId { get; set; }
        public string sTcMakeId { get; set; }
        public string sTcSlNo { get; set; }
        public string sTcCapacity { get; set; }
        public string sTcCode { get; set; }
        public string sMappingDate { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sDTCId { get; set; }
        public string sTCId { get; set; }

        OleDbCommand oledbcommand;
        public clsDtcMapping GetDTCDetails(clsDtcMapping objDtcMapping)
        {
            oledbcommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;
                DataTable dtDtcDetails = new DataTable();
               
                oledbcommand.Parameters.AddWithValue("TmId", objDtcMapping.sMappingId);
                strQry = "select To_char(TM_MAPPING_DATE,'dd/MM/yyyy')TM_MAPPING_DATE,TM_TC_ID,TM_DTC_ID,(select TM_NAME from TBLTRANSMAKES where ";
                strQry += " TC_MAKE_ID=TM_ID)TC_MAKE_ID,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TC_CODE,DT_CODE,DT_NAME,TC_ID,DT_ID from  ";
                strQry += " TBLTRANSDTCMAPPING,TBLTCMASTER,TBLDTCMAST where TM_TC_ID=TC_CODE  and DT_CODE=TM_DTC_ID and TM_ID = :TmId";

                dtDtcDetails = ObjCon.getDataTable(strQry, oledbcommand);
               
                if (dtDtcDetails.Rows.Count > 0)
                {

                    objDtcMapping.sTcCode = dtDtcDetails.Rows[0]["TC_CODE"].ToString();
                    objDtcMapping.sTcSlNo = dtDtcDetails.Rows[0]["TC_SLNO"].ToString();
                    objDtcMapping.sTcMakeId = dtDtcDetails.Rows[0]["TC_MAKE_ID"].ToString();
                    objDtcMapping.sTcCapacity = dtDtcDetails.Rows[0]["TC_CAPACITY"].ToString();
                    objDtcMapping.sDtcCode = dtDtcDetails.Rows[0]["DT_CODE"].ToString();
                    objDtcMapping.sDtcName = dtDtcDetails.Rows[0]["DT_NAME"].ToString();
                    objDtcMapping.sMappingDate = dtDtcDetails.Rows[0]["TM_MAPPING_DATE"].ToString();
                    objDtcMapping.sTCId = dtDtcDetails.Rows[0]["TC_ID"].ToString();
                    objDtcMapping.sDTCId = dtDtcDetails.Rows[0]["DT_ID"].ToString();

                }
                return objDtcMapping;
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetDTCDetails");
                return objDtcMapping;

            }            

        }



    }
}
