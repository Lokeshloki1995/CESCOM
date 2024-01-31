using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;

namespace IIITS.DTLMS.BL
{
    public class clsPoMaster
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsPoMaster";
        public DataTable ddtCapacityGrid { get; set; }
        public string sPoId { get; set; }
        public string sPoNo { get; set; }
        public string sDate { get; set; }
        public string sTcCapacity { get; set; }
        public string sTcMake { get; set; }
        public string sTcQuantity { get; set; }
        public string sCapacity { get; set; }
        public string sSupplierId { get; set; }
        public string sPoRate { get; set; }

        public string sPbId { get; set; }
        public string sCrBy { get; set; }

        OleDbCommand oledbcommand;

        public string[] SavePoMaster(clsPoMaster objPoMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            string[] Arr = new string[2];
            OleDbDataReader dr;
            try
            {

                if (objPoMaster.sPoId == "")
                {
                    ObjCon.BeginTrans();
                    oledbcommand.Parameters.AddWithValue("PoNo", objPoMaster.sPoNo.ToUpper());
                    dr = ObjCon.Fetch("select PO_NO from TBLPOMASTER where UPPER(PO_NO)=:PoNo", oledbcommand);
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Entered PO Number Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    objPoMaster.sPoId = Convert.ToString(ObjCon.Get_max_no("PO_ID", "TBLPOMASTER"));
                    strQry = "INSERT INTO TBLPOMASTER(PO_ID,PO_NO,PO_DATE,PO_CRBY,PO_CRON,PO_SUPPLIER_ID,PO_RATE)";
                    strQry += " VALUES('" + objPoMaster.sPoId + "','" + objPoMaster.sPoNo + "',TO_DATE('" + objPoMaster.sDate + "','DD/MM/YYYY')";
                    strQry += ",'" + objPoMaster.sCrBy + "',SYSDATE,'" + objPoMaster.sSupplierId + "','" + objPoMaster.sPoRate + "')";
                    oledbcommand.Parameters.AddWithValue(" sPoId", objPoMaster.sPoId);
                    oledbcommand.Parameters.AddWithValue(" sPoNo", objPoMaster.sPoNo);
                    oledbcommand.Parameters.AddWithValue(" sDate", objPoMaster.sDate);
                    oledbcommand.Parameters.AddWithValue(" sCrBy", objPoMaster.sCrBy);
                    oledbcommand.Parameters.AddWithValue(" sSupplierId", objPoMaster.sSupplierId);
                    oledbcommand.Parameters.AddWithValue(" sPoRate", objPoMaster.sPoRate);
                    ObjCon.Execute(strQry, oledbcommand);

                    for (int i = 0; i < objPoMaster.ddtCapacityGrid.Rows.Count; i++)
                    {
                        oledbcommand = new OleDbCommand();
                        objPoMaster.sPbId = Convert.ToString(ObjCon.Get_max_no("PB_ID", "TBLPOOBJECTS"));
                        strQry = "INSERT INTO TBLPOOBJECTS(PB_ID,PB_PO_ID,PB_MAKE,PB_CAPACITY,PB_QUANTITY,PB_CRBY,PB_CRON)";
                        strQry += " VALUES(:sPbId,:sPoId,:MAKE_ID";
                        strQry += ",:PB_CAPACITY,:PB_QUANTITY,:sCrBy,SYSDATE)";
                        oledbcommand.Parameters.AddWithValue(" sPbId", objPoMaster.sPbId);
                        oledbcommand.Parameters.AddWithValue(" sPoId", objPoMaster.sPoId);
                        oledbcommand.Parameters.AddWithValue(" MAKE_ID", objPoMaster.ddtCapacityGrid.Rows[i]["MAKE_ID"]);
                        oledbcommand.Parameters.AddWithValue(" PB_CAPACITY", objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY"]);
                        oledbcommand.Parameters.AddWithValue(" PB_QUANTITY", objPoMaster.ddtCapacityGrid.Rows[i]["PB_QUANTITY"]);
                        oledbcommand.Parameters.AddWithValue(" sCrBy", objPoMaster.sCrBy);
                        ObjCon.Execute(strQry, oledbcommand);

                    }
                    ObjCon.CommitTrans();
                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;

                }
                else
                {
                    ObjCon.BeginTrans();
                    oledbcommand = new OleDbCommand();
                    strQry = "DELETE FROM TBLPOOBJECTS WHERE PB_PO_ID=:sPoId";
                    oledbcommand.Parameters.AddWithValue("sPoId", objPoMaster.sPoId);

                    ObjCon.Execute(strQry, oledbcommand);

                    oledbcommand = new OleDbCommand();
                    strQry = " UPDATE TBLPOMASTER SET PO_SUPPLIER_ID=:sSupplierId WHERE PO_ID=:sPoId";
                    oledbcommand.Parameters.AddWithValue("sSupplierId", objPoMaster.sSupplierId);
                    oledbcommand.Parameters.AddWithValue("sPoId", objPoMaster.sPoId);

                    ObjCon.Execute(strQry, oledbcommand);

                    for (int i = 0; i < objPoMaster.ddtCapacityGrid.Rows.Count; i++)
                    {
                        oledbcommand = new OleDbCommand();
                        objPoMaster.sPbId = Convert.ToString(ObjCon.Get_max_no("PB_ID", "TBLPOOBJECTS"));
                        strQry = "INSERT INTO TBLPOOBJECTS(PB_ID,PB_PO_ID,PB_MAKE,PB_CAPACITY,PB_QUANTITY,PB_CRBY,PB_CRON)";
                        strQry += " VALUES(:sPbId,:sPoId,:MAKE_ID";
                        strQry += ",:PB_CAPACITY,:PB_QUANTITY,:sCrBy ,SYSDATE)";
                        oledbcommand.Parameters.AddWithValue("sPbId", objPoMaster.sPbId);
                        oledbcommand.Parameters.AddWithValue("sPoId", objPoMaster.sPoId);
                        oledbcommand.Parameters.AddWithValue("MAKE_ID", objPoMaster.ddtCapacityGrid.Rows[i]["MAKE_ID"]);
                        oledbcommand.Parameters.AddWithValue("PB_CAPACITY", objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY"]);
                        oledbcommand.Parameters.AddWithValue("PB_QUANTITY", objPoMaster.ddtCapacityGrid.Rows[i]["PB_QUANTITY"]);
                        oledbcommand.Parameters.AddWithValue("sCrBy", objPoMaster.sCrBy);
                        ObjCon.Execute(strQry, oledbcommand);

                    }

                    ObjCon.CommitTrans();
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "0";
                    return Arr;


                }
            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SavePoMaster");
                return Arr;
            }
            finally
            {

            }

        }



        public DataTable LoadPoDetailGrid(clsPoMaster objPoMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dtPoDetails = new DataTable();
            try
            {

                strQry = "SELECT PO_ID,PO_NO,TO_CHAR(PO_DATE,'dd-MON-yyyy')PO_DATE,SUM(PB_QUANTITY)PB_QUANTITY,(SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID=PO_SUPPLIER_ID)";
                strQry += " PO_SUPPLIER_ID FROM TBLPOMASTER,";
                strQry += " TBLPOOBJECTS WHERE PO_ID=PB_PO_ID ";
                if (objPoMaster.sPoNo != "")
                {
                    oledbcommand.Parameters.AddWithValue("PoNo1", objPoMaster.sPoNo.ToUpper());
                    strQry += " and  UPPER(PO_NO)=:PoNo1";
                }


                strQry += " GROUP BY PO_ID,PO_NO,PO_DATE,PO_SUPPLIER_ID";

                dtPoDetails = ObjCon.getDataTable(strQry, oledbcommand);
                return dtPoDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadIndentGrid");
                return dtPoDetails;
            }
            finally
            {

            }

        }


        public object GetPoDetails(clsPoMaster objPoMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            try
            {

                DataTable dtIndentDetails = new DataTable();
                oledbcommand.Parameters.AddWithValue("Pid1", objPoMaster.sPoId);
                strQry = "select PO_ID,PO_NO,TO_CHAR(PO_DATE,'dd-MON-yyyy')PO_DATE,PO_SUPPLIER_ID,PO_RATE  from tblpomaster ";
                strQry += " WHERE  PO_ID=:Pid1";

                dtIndentDetails = ObjCon.getDataTable(strQry, oledbcommand);
                objPoMaster.sPoId = Convert.ToString(dtIndentDetails.Rows[0]["PO_ID"]);
                objPoMaster.sPoNo = Convert.ToString(dtIndentDetails.Rows[0]["PO_NO"]);
                objPoMaster.sDate = Convert.ToString(dtIndentDetails.Rows[0]["PO_DATE"]);
                objPoMaster.sSupplierId = Convert.ToString(dtIndentDetails.Rows[0]["PO_SUPPLIER_ID"]);
                objPoMaster.sPoRate = Convert.ToString(dtIndentDetails.Rows[0]["PO_RATE"]);

                return objPoMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetPoDetails");
                return objPoMaster;


            }
            finally
            {

            }

        }

        public DataTable LoadCapacityGrid(clsPoMaster objPoMaster)
        {
            oledbcommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            try
            {

                oledbcommand.Parameters.AddWithValue("Pid2", objPoMaster.sPoId);
                strQry = "select PO_ID,PO_NO,TO_CHAR(PO_DATE,'dd-MON-yyyy')PO_DATE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=PB_MAKE) PB_MAKE,PB_MAKE AS MAKE_ID, TO_CHAR(PB_CAPACITY) as PB_CAPACITY ,";
                strQry += " PB_QUANTITY,PB_PO_ID from tblpomaster,TBLPOOBJECTS  WHERE PO_ID= PB_PO_ID and PO_ID=:Pid2";
                dtCapacityDetails = ObjCon.getDataTable(strQry, oledbcommand);

                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCapacityGrid");
                return dtCapacityDetails;

            }
            finally
            {

            }

        }
    }
}
