using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.REPORTS.Internal;
using IIITS.DTLMS.REPORTS;
using IIITS.DTLMS.BL;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting.WebControls;
using System.Collections;
using CrystalDecisions.Web;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace IIITS.DTLMS.Reports
{
    public partial class ReportView : System.Web.UI.Page
    {
        clsSession objSession;
        string strArr = string.Empty;
        ParameterFieldDefinitions crParameterFieldDefinitions;
        ParameterFieldDefinition crParameterFieldDefinition;
        ParameterValues crParameterValues = new ParameterValues();
        string strTodayDate = DateTime.Now.ToString("dd-MM-yyy");
        string strFormCode = "ReportView";
        #region Internal Application Report
        crpOperatorReport crpOper = new crpOperatorReport();
        crpDetailedReport crpDetailedFieldReport = new crpDetailedReport();
        crpDetStoreReport crpDetailedStoreReport = new crpDetStoreReport();
        crpLocOperator crpLocOperator = new crpLocOperator();
        crpFieldReport crpLocField = new crpFieldReport();
        crpStoreReport crpLocStore = new crpStoreReport();
        #endregion
        #region DTC Failure Transaction Report
        crpEstimationReport crpEstimationReport = new crpEstimationReport();
        CrpWorkOrder crpWorkOrder = new CrpWorkOrder();
        crpGatePass crpGatepassReport = new crpGatePass();
        CrReport crpCRReport = new CrReport();
        crpRiReport crpRIReport = new crpRiReport();
        crpRIAckReport crpRIAck = new crpRIAckReport();
        IndentReport crpIndent = new IndentReport();
        InvoiceReport crpInvoice = new InvoiceReport();
        CregAbstract objreg = new CregAbstract();
        crpReparierPerformance crpReprierPer = new crpReparierPerformance();
        Transformerwisedetails crpTransformerwise = new Transformerwisedetails();
        crpCompletedPerformance crpCompleted = new crpCompletedPerformance();
        crpInvoiceRVDetails objFailRep = new crpInvoiceRVDetails();
        #endregion
        #region Capacity Enhanacement Transaction Report
        crpEnhanceEstimation crpEnhanceEst = new crpEnhanceEstimation();
        EnhanceCrReport crpEnhanceCR = new EnhanceCrReport();
        EnhanceIndentReport crpEnhanceIndent = new EnhanceIndentReport();
        EnhanceInvoiceReport crpEnhanceInvoice = new EnhanceInvoiceReport();
        #endregion
        crpRepairGatepass crpRepairGatepass = new crpRepairGatepass();
        CrpMisGuarentyTypeChange CrpMisGuarentyTypeChange;
        CrpDtrwise objDtr = new CrpDtrwise();
        REPORTS.crpDTCAddedreport crpdtcAdded = new REPORTS.crpDTCAddedreport();
        crpScrapInvoice crpScrapInvoice = new crpScrapInvoice();
        crpStoreInvoice crpStoreInvoice = new crpStoreInvoice();
        crpCRAbstract crpCRAbstract = new crpCRAbstract();
        crpWoRegDetails crpWoRegDetails = new crpWoRegDetails();
        crpDtrRepairerWise crpRepairerWise = new crpDtrRepairerWise();
        crpMisCommissionPending crpMisCommissionPending = new crpMisCommissionPending();
        crpMisFaultyDtrDetails crpMisFaultyDtrDetails;
        MisPendingFailureReplacement crpMisPendingFailureReplacement = new MisPendingFailureReplacement();
        MisPendingFailureReplacement1 crpMisPendingFailureReplacement1 = new MisPendingFailureReplacement1();
        MisPendingFailureReplacementTotal crpMisPendingFailureReplacementTotal = new MisPendingFailureReplacementTotal();
        crpMisReapirerPerformance crpRepairerPerformance;
        crpMisReapirerPending crpMisReapirerPending;
        crpMisRepairCentre crpMisRepairCentre;
        crpReceiveDtr crpRecieveDTr = new crpReceiveDtr();
        StoreIndentReport crpStoreIndent = new StoreIndentReport();
        crpDTCFailreport objRep = new crpDTCFailreport();
        crpMisReplacableDTR objcrpMisReplacableDTR;
        crpMISDTCCountFeeder crpMISDTCCountFeeder = new crpMISDTCCountFeeder();
        crppurchaseorderoil crppurchaseorderoil = new crppurchaseorderoil();
        crpDTCreport crpDTCRep = new crpDTCreport();
        crpAddDtcReport crpAddDTC = new crpAddDtcReport();
        CrReport crpCRDetails = new CrReport();
        CrReportAbstract crpReportDetails = new CrReportAbstract();
        FailureAbstract crpFailureAbstract = new FailureAbstract();
        crpDTCFailFrequent crpDtcFailFrequent = new crpDTCFailFrequent();
        crpFrequentDTRFail crpFrequentDTRFail = new crpFrequentDTRFail();
        crpFeederBifurcation crpobjFeederBifurcation = new crpFeederBifurcation();
        crpFeederBifurcation_SO objcrpFeederBifurcationSO = new crpFeederBifurcation_SO();
        testreport obj = new testreport();
        #region DTC Added Abstract  
        crpCapacityAbstarctReport crpCapacityAbstract = new crpCapacityAbstarctReport();
        crpFeederOrCategoryAbstarctReport crpFeederAbstract = new crpFeederOrCategoryAbstarctReport();
        crpWorkwiseAbstarctReport crpWorkwiseAbstract = new crpWorkwiseAbstarctReport();
        #endregion
        string strReport = string.Empty;
        string stroffcode = string.Empty;
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objSession = (clsSession)Session["clsSession"];
                stroffcode = objSession.OfficeName;
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
                {
                    strReport = Request.QueryString["id"].ToString();
                }
                #region Internal Application Report
                if (Request.QueryString["id"] == "EnumReport")
                {
                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.EnumerationReport(strFromdate, strTodate);
                    crpOper.SetDataSource(dt);
                    crpPrint.ReportSource = crpOper;
                    crpPrint.DataBind();
                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpOper.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpOper.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }
                if (Request.QueryString["id"] == "DetaiedField")
                {
                    DataTable dtDetailed = new DataTable();
                    clsReports objreport = new clsReports();
                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    dtDetailed = objreport.PrintDetailedFieldReport(strFromdate, strTodate);
                    crpDetailedFieldReport.SetDataSource(dtDetailed);
                    crpPrint.ReportSource = crpDetailedFieldReport;
                    crpPrint.DataBind();
                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpDetailedFieldReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpDetailedFieldReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }
                if (Request.QueryString["id"] == "DetailedStore")
                {
                    DataTable dtDetailed = new DataTable();
                    clsReports objreport = new clsReports();
                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    dtDetailed = objreport.PrintDetailedStoreReport(strFromdate, strTodate);
                    crpDetailedStoreReport.SetDataSource(dtDetailed);
                    crpPrint.ReportSource = crpDetailedStoreReport;
                    crpPrint.DataBind();
                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpDetailedStoreReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpDetailedStoreReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }
                if (Request.QueryString["id"] == "LocOperator")
                {
                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.EnumReportLocationWise(strFromdate, strTodate);
                    crpLocOperator.SetDataSource(dt);
                    crpPrint.ReportSource = crpLocOperator;
                    crpPrint.DataBind();
                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpLocOperator.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpLocOperator.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }
                if (Request.QueryString["id"].ToString().Equals("FieldLoc"))
                {
                    string sFeederCode = Request.QueryString["sFeeder"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sFromdate = Request.QueryString["FromDate"].ToString();
                    string sTodate = Request.QueryString["ToDate"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.PrintFieldDetails(sFeederCode, sOfficeCode, sFromdate, sTodate);
                    crpLocField.SetDataSource(dt);
                    crpPrint.ReportSource = crpLocField;
                    crpPrint.DataBind();
                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    if (sFromdate != "")
                    {
                        crParameterDiscreteValue4.Value = "From " + sFromdate;
                    }
                    else
                    {
                        crParameterDiscreteValue4.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocField.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    if (sTodate != "")
                    {
                        crParameterDiscreteValue5.Value = "To " + sTodate;
                    }
                    else
                    {
                        crParameterDiscreteValue5.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocField.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }
                if (Request.QueryString["id"].ToString().Equals("StoreLoc"))
                {
                    string strOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sFromdate = Request.QueryString["FromDate"].ToString();
                    string sTodate = Request.QueryString["ToDate"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.PrintStoreDetails(strOfficeCode, sFromdate, sTodate);
                    crpLocStore.SetDataSource(dt);
                    crpPrint.ReportSource = crpLocStore;
                    crpPrint.DataBind();
                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    if (sFromdate != "")
                    {
                        crParameterDiscreteValue4.Value = "From " + sFromdate;
                    }
                    else
                    {
                        crParameterDiscreteValue4.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocStore.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    if (sTodate != "")
                    {
                        crParameterDiscreteValue5.Value = "To " + sTodate;
                    }
                    else
                    {
                        crParameterDiscreteValue5.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocStore.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }
                #endregion
                #region DTC Failure Transaction Report              
                if (Request.QueryString["id"].ToString().Equals("Estimation"))      // Failure Estimation Report SDO approval 
                {
                    string sFailureId = Request.QueryString["FailureId"].ToString();
                    clsReports objRep = new clsReports();
                    DataTable dtCommEstimation = new DataTable();
                    dtCommEstimation = objRep.PrintEstimatedReport(sFailureId);
                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimatedReport(sFailureId);
                    DataTable dtSurvey = new DataTable();
                    dtSurvey = objRep.PrintSurveyReport(sFailureId);
                    crpEstimationReport.OpenSubreport("CrpCommEstimationReport.rpt").SetDataSource(dtCommEstimation);
                    crpEstimationReport.OpenSubreport("crpDeCommEstReport.rpt").SetDataSource(dtDecomEstimation);
                    crpEstimationReport.OpenSubreport("crpServeyReport.rpt").SetDataSource(dtSurvey);
                    crpPrint.ReportSource = crpEstimationReport;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"].ToString().Equals("EstimationSO"))        // Failure Estimation SO Report 
                {
                    clsApproval objApp = new clsApproval();
                    DataTable dt = new DataTable();
                    string sTCcode = Request.QueryString["TCcode"].ToString();
                    string sWoId = Request.QueryString["WOId"].ToString();
                    string sRes = string.Empty;
                    string sEnhcCap = string.Empty;
                    string sGuaranty = string.Empty;
                    string statusflag = string.Empty;
                    clsReports objRep = new clsReports();
                    clsFailureEntry objFailure = new clsFailureEntry();
                    string sWfoID = objFailure.getWfoIDforEstimationSO(sWoId);
                    //Get Xml Data
                    if (sWfoID != "" && sWfoID != null)
                    {
                        dt = objApp.GetDatatableFromXML(sWfoID);
                    }
                    //Get Reason and Enhance Capacity to generate report
                    if (dt.Columns.Contains("DF_REASON"))
                        sRes = dt.Rows[0]["DF_REASON"].ToString().Replace("ç", ",");
                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                        sEnhcCap = dt.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();
                    if (dt.Columns.Contains("GUARENTEE"))
                        sGuaranty = Convert.ToString(dt.Rows[0]["GUARENTEE"]);
                    if (dt.Columns.Contains("DF_STATUS_FLAG"))
                        statusflag = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                    DataTable dtCommEstimation;
                    dtCommEstimation = objRep.PrintEstimatedReportSO(sTCcode, sWoId, sEnhcCap, statusflag);
                    DataTable dtDecomEstimation = new DataTable();
                    DataTable dtSurvey = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimationReportSO(sTCcode, sWoId, sRes, sEnhcCap, sGuaranty, statusflag);
                    dtSurvey = objRep.PrintSurveyReportSO(dt, sWoId, sTCcode, sEnhcCap);
                    crpEstimationReport.OpenSubreport("CrpCommEstimationReport.rpt").SetDataSource(dtCommEstimation);
                    crpEstimationReport.OpenSubreport("crpDeCommEstReport.rpt").SetDataSource(dtDecomEstimation);
                    crpEstimationReport.OpenSubreport("crpServeyReport.rpt").SetDataSource(dtSurvey);
                    crpPrint.ReportSource = crpEstimationReport;
                    crpPrint.DataBind();
                }
                #region WorkOrderReport
                if (Request.QueryString["id"].ToString().Equals("WorkOrderPreview"))       // WorkOrder Preview
                {
                    clsApproval objApproval = new clsApproval();
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    ArrayList sNameList = new ArrayList();
                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    string sTaskType = Request.QueryString["TaskType"].ToString();
                    string sWoId = Request.QueryString["WoId"].ToString();
                    string sFailureid = Request.QueryString["FailureID"].ToString();
                    if (sWoId != "")
                    {
                        clsReports objReport = new clsReports();
                        dtWorkOrderDetailsComm = objReport.PrintWorkOrderDetailsForNewDTC(sWoId);
                    }
                    if (sWFDataId != "")
                        dtWorkOrderDetailsComm = objApproval.GetDatatableFromXML(sWFDataId);
                    if (Session["UserNameList"] != null)
                    {
                        sNameList = (ArrayList)Session["UserNameList"];
                    }
                    DataTable dtWorkOrderDetailsDeComm = new DataTable();
                    if (sWFDataId != "" && (sTaskType == "1" || sTaskType == "2" || sTaskType == "4" || sTaskType == "5"))
                        dtWorkOrderDetailsDeComm = objApproval.GetDatatableFromXML(sWFDataId);
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("WO_AMT_DECOM", typeof(int));
                        DataRow dtrow = dt.NewRow();
                        dtrow["WO_AMT_DECOM"] = 0000;
                        dt.Rows.Add(dtrow);
                        dtWorkOrderDetailsDeComm = dt;
                    }
                    DataTable dtCutomerdetails = new DataTable();
                    dtCutomerdetails = objApproval.FailureCustomerdetails(sFailureid);
                    if (dtCutomerdetails.Rows.Count > 0)
                    {
                        dtWorkOrderDetailsDeComm.Columns.Add("DF_CUSTOMER_MOBILE", typeof(string));
                        dtWorkOrderDetailsDeComm.Columns.Add("DF_CUSTOMER_NAME", typeof(string));
                        dtWorkOrderDetailsDeComm.Columns.Add("DF_NUMBER_OF_INSTALLATIONS", typeof(string));
                        dtWorkOrderDetailsDeComm.Rows[0]["DF_CUSTOMER_MOBILE"] = dtCutomerdetails.Rows[0]["DF_CUSTOMER_MOBILE"].ToString();
                        dtWorkOrderDetailsDeComm.Rows[0]["DF_CUSTOMER_NAME"] = dtCutomerdetails.Rows[0]["DF_CUSTOMER_NAME"].ToString();
                        dtWorkOrderDetailsDeComm.Rows[0]["DF_NUMBER_OF_INSTALLATIONS"] = dtCutomerdetails.Rows[0]["DF_NUMBER_OF_INSTALLATIONS"].ToString();
                    }
                    crpWorkOrder.OpenSubreport("CrpWorkOrderComm.rpt").SetDataSource(dtWorkOrderDetailsComm);
                    crpWorkOrder.OpenSubreport("CrpWorkOrderDecomm.rpt").SetDataSource(dtWorkOrderDetailsDeComm);
                    crpPrint.ReportSource = crpWorkOrder;
                    if (sTaskType == "3")
                    {
                        crpWorkOrder.SetParameterValue("TaskType", "3");
                    }
                    else if (sTaskType == "1" || sTaskType == "2" || sTaskType == "4" || sTaskType == "5" || sWoId == "")
                        crpWorkOrder.SetParameterValue("TaskType", "1");
                    if (sLevelOfApproval == "1")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", "");
                        crpWorkOrder.SetParameterValue("aousername", "");
                        crpWorkOrder.SetParameterValue("dousername", "");
                    }
                    if (sLevelOfApproval == "2")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", sNameList[1]);
                        crpWorkOrder.SetParameterValue("aousername", "");
                        crpWorkOrder.SetParameterValue("dousername", "");
                    }
                    if (sLevelOfApproval == "3")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", sNameList[1]);
                        crpWorkOrder.SetParameterValue("aousername", sNameList[2]);
                        crpWorkOrder.SetParameterValue("dousername", "");
                    }
                    if (sLevelOfApproval == "4")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", sNameList[1]);
                        crpWorkOrder.SetParameterValue("aousername", sNameList[2]);
                        crpWorkOrder.SetParameterValue("dousername", sNameList[3]);
                    }
                    crpWorkOrder.SetParameterValue("Officename", sOffCode);
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"].ToString().Equals("WorkOrder"))
                {
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    clsReports objReport = new clsReports();
                    string sFailureId = Request.QueryString["FailureId"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    dtWorkOrderDetailsComm = objReport.PrintWorkOrderReport(sFailureId);
                    crpWorkOrder.OpenSubreport("CrpWorkOrderComm.rpt").SetDataSource(dtWorkOrderDetailsComm);
                    crpWorkOrder.OpenSubreport("CrpWorkOrderDecomm.rpt").SetDataSource(dtWorkOrderDetailsComm);
                    crpPrint.ReportSource = crpWorkOrder;
                    crpWorkOrder.SetParameterValue("aetusername", "");
                    crpWorkOrder.SetParameterValue("stousername", "");
                    crpWorkOrder.SetParameterValue("aousername", "");
                    crpWorkOrder.SetParameterValue("dousername", "");
                    crpWorkOrder.SetParameterValue("Officename", sOffCode);
                    crpPrint.DataBind();
                }
                #endregion
                if (Request.QueryString["id"] == "GatePass")        // GatePass
                {
                    DataTable dt = new DataTable();
                    DataTable dtSign = new DataTable();
                    DataSet ds = new DataSet();
                    clsReports objInvoice = new clsReports();
                    dt = objInvoice.LoadGatePass(Request.QueryString["InvoiceId"].ToString());
                    crpGatepassReport.SetDataSource(dt);
                    crpPrint.ReportSource = crpGatepassReport;
                    crpPrint.RefreshReport();
                }
                if (Request.QueryString["id"] == "CRReport")        //CR Report
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString().Split('~')[0];
                    string sCrDate = Request.QueryString["DecommId"].ToString().Split('~')[1];
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.CompletionReport(sDecommId, sCrDate);
                    crpReportDetails.SetDataSource(dt);
                    crpPrint.ReportSource = crpReportDetails;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "RIReport")        // RI Report
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.RIReport(sDecommId);

                    crpRIReport.SetDataSource(dt);
                    crpPrint.ReportSource = crpRIReport;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "RIAckReport")     // RI Acknoldgment Report
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.RIReport(sDecommId);

                    crpRIAck.SetDataSource(dt);
                    crpPrint.ReportSource = crpRIAck;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "IndentReport")        // Indent Report
                {
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.IndentDetails(sIndentId);
                    crpIndent.SetDataSource(dt);
                    crpPrint.ReportSource = crpIndent;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "InvoiceReport")       // Invoice Report
                {
                    string sInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sCapacity = Request.QueryString["Capacity"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.InvoiceReport(sInvoiceId, sOfficeCode, sCapacity);
                    crpInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpInvoice;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "TCFailDetails")       // TC Fail Details
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    objReport.sReportType = Request.QueryString["ReportType"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    dt = objReport.InvoicedandRVDetails(objReport);
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    if (dt.Rows.Count > 0)
                    {
                        objFailRep.SetDataSource(dt);
                        crpPrint.ReportSource = objFailRep;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #endregion
                #region Capacity Enhanacement Transaction Report
                if (Request.QueryString["id"].ToString().Equals("EnhanceEstimation"))       // EnhanceEstimation
                {
                    string sEnhanceId = Request.QueryString["EnhanceId"].ToString();
                    clsReports objRep = new clsReports();
                    DataTable dtCommEstimation = new DataTable();
                    dtCommEstimation = objRep.PrintEstimatedReport(sEnhanceId);
                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimatedReport(sEnhanceId);
                    crpEnhanceEst.OpenSubreport("crpEnhanceCommEst.rpt").SetDataSource(dtCommEstimation);
                    crpEnhanceEst.OpenSubreport("crpEnhanceDeCommEst.rpt").SetDataSource(dtDecomEstimation);
                    crpPrint.ReportSource = crpEnhanceEst;
                    crpPrint.DataBind();
                }
                #region EnhanceEstm Report For So
                if (Request.QueryString["id"].ToString().Equals("EnhanceEstimationSO"))     // Enhance Estimation SO
                {
                    #region Variable Declaration
                    clsApproval objApp = new clsApproval();
                    DataTable dt = new DataTable();
                    string sTCcode = Request.QueryString["TCcode"].ToString();
                    string sWoId = Request.QueryString["WOId"].ToString();
                    string sRes = string.Empty;
                    string sEnhncCap = string.Empty;
                    string sGuaranty = string.Empty;
                    string statusflag = string.Empty;
                    clsReports objRep = new clsReports();
                    clsEnhancement objEnhnc = new clsEnhancement();
                    #endregion
                    string sWfoID = objEnhnc.getWfoIDforEstimationSO(sWoId);
                    //Get Data from Xml
                    if (sWfoID != "" && sWfoID != null)
                    {
                        dt = objApp.GetDatatableFromXML(sWfoID);
                    }
                    //Get Reason and Enhance Capacity to Generate Report
                    if (dt.Columns.Contains("DF_REASON"))
                        sRes = dt.Rows[0]["DF_REASON"].ToString();
                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                        sEnhncCap = dt.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();
                    if (dt.Columns.Contains("GUARENTEE"))
                        sGuaranty = Convert.ToString(dt.Rows[0]["GUARENTEE"]);
                    if (dt.Columns.Contains("DF_STATUS_FLAG"))
                        statusflag = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                    DataTable dtCommEstimation;
                    dtCommEstimation = objRep.PrintEstimatedReportSO(sTCcode, sWoId, sEnhncCap, statusflag);
                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimationReportSO(sTCcode, sWoId, sRes, sEnhncCap, sGuaranty, statusflag);
                    crpEnhanceEst.OpenSubreport("crpEnhanceCommEst.rpt").SetDataSource(dtCommEstimation);
                    crpEnhanceEst.OpenSubreport("crpEnhanceDeCommEst.rpt").SetDataSource(dtDecomEstimation);
                    crpPrint.ReportSource = crpEnhanceEst;
                    crpPrint.DataBind();
                }
                #endregion
                if (Request.QueryString["id"] == "EnhanceCRReport")     // Enhance CR Report
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString().Split('~')[0];
                    string sCRDate = Request.QueryString["DecommId"].ToString().Split('~')[1];
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.CompletionReport(sDecommId, sCRDate);
                    crpReportDetails.SetDataSource(dt);
                    crpPrint.ReportSource = crpReportDetails;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "EnhanceIndentReport")     // Enhance Indent Report
                {
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.IndentDetails(sIndentId);
                    crpEnhanceIndent.SetDataSource(dt);
                    crpPrint.ReportSource = crpEnhanceIndent;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "EnhanceInvoiceReport")        // Enhance Invoice Report
                {
                    string sInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sCapacity = Request.QueryString["Capacity"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.InvoiceReport(sInvoiceId, sOfficeCode, sCapacity);
                    crpEnhanceInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpEnhanceInvoice;
                    crpPrint.DataBind();
                }
                #endregion
                if (Request.QueryString["id"] == "RepairGatepass")      // Repair Gate pass
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintRepairGatePassReport(strInvoiceNo);
                    crpRepairGatepass.SetDataSource(dt);
                    crpPrint.ReportSource = crpRepairGatepass;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "ScrapGatepass")       // Scrap Gate pass
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintScrapGatePass(strInvoiceNo);
                    crpRepairGatepass.SetDataSource(dt);
                    crpPrint.ReportSource = crpRepairGatepass;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "StoreGatepass")       // Store Gate pass
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintStoreInvoiceGatePass(strInvoiceNo);
                    crpRepairGatepass.SetDataSource(dt);
                    crpPrint.ReportSource = crpRepairGatepass;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "ScrapInvoice")        // Scrap Invoice
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strCsrapInvoiceId = Request.QueryString["scrapInvoice"].ToString();
                    dt = objReport.PrintScrapInvoicereport(strCsrapInvoiceId);
                    crpScrapInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpScrapInvoice;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "InterStoreInvoice")       // Inter Store Invoice
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string sInvoiceNo = Request.QueryString["InvoiceNo"].ToString();
                    dt = objReport.PrintInterStoreInvoicereport(sInvoiceNo);
                    crpStoreInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpStoreInvoice;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "RecieveDTR")      // Recieve DTR
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strCsrapInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintReceiveDTrReport(strCsrapInvoiceId);
                    crpRecieveDTr.SetDataSource(dt);
                    crpPrint.ReportSource = crpRecieveDTr;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "InterStoreIndent")        // Inter Store Indent
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string sStoreIndentId = Request.QueryString["IndentId"].ToString();
                    dt = objReport.PrintStoreIndentReport(sStoreIndentId);
                    #region new code for show if data is not there for report
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write("<script language=javascript>alert('Data Not Available')</script>");
                    }
                    else
                    {
                        crpStoreIndent.SetDataSource(dt);
                        crpPrint.ReportSource = crpStoreIndent;
                        crpPrint.DataBind();
                    }
                    #endregion
                }
                #region Abstract Report
                if (Request.QueryString["id"] == "AbstractReport")      // Abstract Report
                {
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    dt = objReport.PrintAbstractReport(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpAbstractReport objRep = new crpAbstractReport();
                        objRep.SetDataSource(dt);
                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "CRAbstract")      // CR Abstract
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    dt = objReport.CRAbstract(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpCRAbstract.SetDataSource(dt);
                        crpPrint.ReportSource = crpCRAbstract;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "AbstractRptTcFailed")     // Abstract Rpt Tc Failed
                {
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    DataTable dtRepairCount = new DataTable();
                    DataTable dtcompletedRepairCount = new DataTable();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    dt = objReport.PrintAbstractReportTcFailedAtFSR(objReport);
                    dtRepairCount = objReport.PrintRepairerTcCount(objReport);
                    dtcompletedRepairCount = objReport.PrintCompletedRepairerTcCount(objReport);
                    crpAbstractReportTcFailedReplaceAtFSR objRep = new crpAbstractReportTcFailedReplaceAtFSR();
                    if (dt.Rows.Count > 0)
                    {
                        objRep.SetDataSource(dt);
                        objRep.OpenSubreport("CrpRepairerTcCountSub.rpt").SetDataSource(dtRepairCount);
                        objRep.OpenSubreport("CrpRepairCompleted.rpt").SetDataSource(dtcompletedRepairCount);
                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #endregion
                #region MisReports
                if (Request.QueryString["id"] == "MisCommisionPending")     // Mis Commision Pending
                {

                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    dt = objReport.MisCommissionPending(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpMisCommissionPending.SetDataSource(dt);
                        crpPrint.ReportSource = crpMisCommissionPending;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "MisFaultyDtr")        // Mis Faulty Dtr
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    crpMisFaultyDtrDetails = new crpMisFaultyDtrDetails();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    dt = objReport.MisFaultyDtr(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpMisFaultyDtrDetails.SetDataSource(dt);
                        crpPrint.ReportSource = crpMisFaultyDtrDetails;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "MisFailureReplacement")       // Mis Failure Replacement
                {
                    DataTable dtMainReport = new DataTable();
                    DataTable dtSubReportWGP = new DataTable();
                    DataTable dtSubReportTotal = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["GroupBy"].ToString() != null && Request.QueryString["GroupBy"].ToString() != "")
                    {
                        objReport.sType = Request.QueryString["GroupBy"].ToString();
                    }
                    if (Request.QueryString["GuarantyType"].ToString() != null && Request.QueryString["GuarantyType"].ToString() != "")
                    {
                        objReport.sGuarantyTypes = Request.QueryString["GuarantyType"].ToString().Split(',').ToList();
                    }
                    if (Request.QueryString["FromMonth"].ToString() != null && Request.QueryString["FromMonth"].ToString() != "")
                    {
                        objReport.sFromMonth = Request.QueryString["FromMonth"].ToString();
                    }
                    if (Request.QueryString["ToMonth"].ToString() != null && Request.QueryString["ToMonth"].ToString() != "")
                    {
                        objReport.sToMonth = Request.QueryString["ToMonth"].ToString();
                    }
                    objReport.sReportType = "AGP";
                    dtMainReport = objReport.GetReplacement(objReport);
                    objReport.sReportType = "WGP";
                    dtSubReportWGP = objReport.GetReplacement(objReport);
                    objReport.sReportType = "TOTAL";
                    dtSubReportTotal = objReport.GetReplacement(objReport);
                    if (dtMainReport.Rows.Count > 0)
                    {
                        crpMisPendingFailureReplacement.SetDataSource(dtMainReport);
                        crpMisPendingFailureReplacement.OpenSubreport("MisPendingFailureReplacementWGP.rpt").SetDataSource(dtSubReportWGP);
                        crpMisPendingFailureReplacement.OpenSubreport("MisPendingFailureReplacementTotal.rpt").SetDataSource(dtSubReportTotal);
                        crpMisPendingFailureReplacement.SetParameterValue("sFromMonth", objReport.sFromMonth);
                        crpMisPendingFailureReplacement.SetParameterValue("sToMonth", objReport.sToMonth);
                        crpPrint.ReportSource = crpMisPendingFailureReplacement;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "MisRepairerStatus")       // Mis Repairer Status
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    DataTable dtWGP = new DataTable();
                    clsReports objReport = new clsReports();
                    crpMisRepairCentre = new crpMisRepairCentre();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    objReport.sCurrentMonth = Request.QueryString["Month"].ToString();
                    objReport.sFinancialYear = Request.QueryString["FinancialYear"].ToString();
                    objReport.sGuranteeType = "'AGP'";
                    dt = objReport.MisRepairerStatus(objReport);
                    objReport.sGuranteeType = "'WGP','WRGP'";
                    dtWGP = objReport.MisRepairerStatus(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpMisRepairCentre.SetDataSource(dt);
                        crpMisRepairCentre.OpenSubreport("crpMisRepairCentreWGP.rpt").SetDataSource(dtWGP);
                        crpPrint.ReportSource = crpMisRepairCentre;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "MisRepairerPerformance")      // Mis Repairer Performance
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    crpRepairerPerformance = new crpMisReapirerPerformance();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    objReport.sCurrentMonth = Request.QueryString["Month"].ToString();
                    objReport.sFinancialYear = Request.QueryString["FinancialYear"].ToString();
                    dt = objReport.MisRepairerPerformance(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpRepairerPerformance.SetDataSource(dt);
                        crpPrint.ReportSource = crpRepairerPerformance;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "MisRepairerPending")      // Mis Repairer Performance
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    crpMisReapirerPending = new crpMisReapirerPending();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    objReport.sCurrentMonth = Request.QueryString["Month"].ToString();
                    objReport.sFinancialYear = Request.QueryString["FinancialYear"].ToString();
                    dt = objReport.MisRepairerPending(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        //crpRepairerPerformance.SetDataSource(dt);
                        //crpPrint.ReportSource = crpRepairerPerformance;
                        //crpPrint.DataBind();

                        crpMisReapirerPending.SetDataSource(dt);
                        crpPrint.ReportSource = crpMisReapirerPending;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"] == "GrntyTypeChange")      // Mis Guranty type chaged Details
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    CrpMisGuarentyTypeChange = new CrpMisGuarentyTypeChange();

                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    dt = objReport.GuarentyTypeChangedDetails(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        CrpMisGuarentyTypeChange.SetDataSource(dt);
                        crpPrint.ReportSource = CrpMisGuarentyTypeChange;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"] == "MisReplacableDTR")        // Mis Replacable DTR
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objcrpMisReplacableDTR = new crpMisReplacableDTR();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    dt = objReport.MisReplacableDTr(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        objcrpMisReplacableDTR.SetDataSource(dt);
                        crpPrint.ReportSource = objcrpMisReplacableDTR;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #endregion
                #region  Bifurcation Report 
                if (Request.QueryString["id"] == "FeederBifurcation")      // Feeder Bifurcation
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if ((Convert.ToString(Request.QueryString["BifurcationID"]).Length > 0))
                    {
                        objReport.sFeederBifurcationID = Request.QueryString["BifurcationID"].ToString();
                    }
                    if ((Convert.ToString(Request.QueryString["officeCode"]) == " "))
                    {
                        objReport.sOfficeCode = Request.QueryString["officeCode"].ToString();
                    }
                    if ((Convert.ToString(Request.QueryString["oldFeederCode"]).Length > 0))
                    {
                        objReport.sOldFeederCode = Request.QueryString["oldFeederCode"].ToString();
                    }
                    if ((Convert.ToString(Request.QueryString["newFeederCode"]).Length > 0))
                    {
                        objReport.sNewFeederCode = Request.QueryString["newFeederCode"].ToString();
                    }
                    if ((Convert.ToString(Request.QueryString["ReportType"]).Length > 0))
                    {
                        objReport.sReportType = Request.QueryString["ReportType"].ToString();
                    }
                    if ((Request.QueryString["FromDate"]).ToString() != null && (Request.QueryString["FromDate"]).ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if ((Request.QueryString["ToDate"]).ToString() != null && (Request.QueryString["ToDate"]).ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    dt = objReport.PrintFeederBifurcationReport(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpobjFeederBifurcation.SetDataSource(dt);
                        crpPrint.ReportSource = crpobjFeederBifurcation;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "FeederBifurcationSO")      // Feeder Bifurcation SO
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    clsReports objReport = new clsReports();
                    if ((Convert.ToString(Request.QueryString["FBS_Id"]).Length > 0))
                    {
                        objReport.sFeederBifurcationID = Request.QueryString["FBS_Id"].ToString();
                    }
                    dt = objReport.PrintFeederBifurcationReportSO(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        objcrpFeederBifurcationSO.SetDataSource(dt);
                        crpPrint.ReportSource = objcrpFeederBifurcationSO;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #endregion
                if (Request.QueryString["id"] == "DTrReportMake")       // DTr Report Make
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["Make"].ToString() != null && Request.QueryString["Make"].ToString() != "")
                    {
                        objReport.sMake = Request.QueryString["Make"].ToString();
                    }
                    if (Request.QueryString["Capacity"].ToString() != null && Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    dt = objReport.PrintDTrReport(objReport);
                    crpDTrReport objRep = new crpDTrReport();
                    if (dt.Rows.Count > 0)
                    {
                        objRep.SetDataSource(dt);
                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "DTCReportFeeder")         // DTC Report Feeder
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    if (Request.QueryString["SchemaType"].ToString() != null && Request.QueryString["SchemaType"].ToString() != "")
                    {
                        objReport.sSchemeType = Request.QueryString["SchemaType"].ToString();
                    }
                    if (Request.QueryString["Capacity"].ToString() != null && Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                    }
                    dt = objReport.PrintDTCCReport(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpDTCRep.SetDataSource(dt);
                        crpPrint.ReportSource = crpDTCRep;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "TCFail")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["Type"].ToString() != null && Request.QueryString["Type"].ToString() != "")
                    {
                        objReport.sType = Request.QueryString["Type"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["FailType"].ToString() != null && Request.QueryString["FailType"].ToString() != "")
                    {
                        objReport.sFailureType = Request.QueryString["FailType"].ToString();
                    }
                    if (Request.QueryString["Make"].ToString() != null && Request.QueryString["Make"].ToString() != "")
                    {
                        objReport.sMake = Request.QueryString["Make"].ToString();
                    }
                    if (Request.QueryString["Capacity"].ToString() != null && Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                    }
                    if (Request.QueryString["GrntyType"].ToString() != null && Request.QueryString["GrntyType"].ToString() != "")
                    {
                        objReport.sGuranteeType = Request.QueryString["GrntyType"].ToString();
                    }
                    if (Request.QueryString["ReportType"].ToString() != null && Request.QueryString["ReportType"].ToString() != "")
                    {
                        objReport.sReportType = Request.QueryString["ReportType"].ToString();
                    }
                    objReport.sSelectedFailureType = Convert.ToString(Request.QueryString["FailureType"]);
                    dt = objReport.TCFailReport(objReport);
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    if (dt.Rows.Count > 0)
                    {
                        objRep.SetDataSource(dt);
                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #region RegAbstract
                if (Request.QueryString["id"] == "RegAbstract")
                {
                    clsReports objReport = new clsReports();
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    ReportDocument rptDoc = new ReportDocument();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    dt = objReport.PrintRegAbstact(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        objreg.SetDataSource(dt);
                        crpPrint.ReportSource = objreg;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #endregion
                if (Request.QueryString["id"] == "WorkOderReg")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    dt = objReport.WoRegDetails(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpWoRegDetails.SetDataSource(dt);
                        crpPrint.ReportSource = crpWoRegDetails;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "DTCAddDetails")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["DTCAddedThrough"].ToString() != "")
                    {
                        objReport.sDTCAddedThrough = Request.QueryString["DTCAddedThrough"].ToString();
                        if (Request.QueryString["QCApprovaltype"].ToString() != "")
                        {
                            objReport.sQCApprovaltype = Request.QueryString["QCApprovaltype"].ToString();
                        }
                    }
                    if (Request.QueryString["FeederType"].ToString() != "")
                    {
                        objReport.sFeederType = Request.QueryString["FeederType"].ToString();
                    }
                    if (Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                        objReport.sGreaterVal = "TRUE";
                    }
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }
                    if (Request.QueryString["SchemaType"].ToString() != null && Request.QueryString["SchemaType"].ToString() != "")
                    {
                        objReport.sSchemeType = Request.QueryString["SchemaType"].ToString();
                    }
                    dt = objReport.DTCAddedReport(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpdtcAdded.SetDataSource(dt);
                        crpPrint.ReportSource = crpdtcAdded;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "CRDetails")
                {
                    DataTable dt = new DataTable();
                    DataTable dtFeederDetails = new DataTable();
                    clsCRReport objCrReport = new clsCRReport();
                    clsReports objReport = new clsReports();
                    string newDTC = "";
                    string oldDTC = "";
                    objReport.sFailId = Request.QueryString["FailureId"].ToString();
                    objReport.sDtcCode = Request.QueryString["DTCCODE"].ToString();
                    dtFeederDetails = objCrReport.GetFeederBifurcateDTC(Genaral.Decrypt(objReport.sDtcCode));
                    if (dtFeederDetails.Rows.Count > 0)
                    {
                        newDTC = dtFeederDetails.Rows[0]["FBD_NEW_DTC_CODE"].ToString();
                        oldDTC = dtFeederDetails.Rows[0]["FBD_OLD_DTC_CODE"].ToString();
                    }
                    dt = objReport.CRDetails(objReport, newDTC, oldDTC);
                    if (dt.Rows.Count > 0)
                    {
                        crpReportDetails.SetDataSource(dt);
                        crpPrint.ReportSource = crpReportDetails;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "Pending Analysis Report")     //TRANFORMER PERFORMANCE
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    DateTime DFromDate = new DateTime();
                    DateTime DToDate = new DateTime();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = Convert.ToDateTime(DFromDate).ToString("dd-MMM-yyyy");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DToDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = Convert.ToDateTime(DToDate).ToString("dd-MMM-yyyy");
                    }
                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }
                    if (Request.QueryString["StrReprierName"].ToString() != null && Request.QueryString["StrReprierName"].ToString() != "")
                    {
                        objReport.sRepriername = Request.QueryString["StrReprierName"].ToString();
                    }
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    objReport.sTempFromDate = objReport.sFromDate;
                    objReport.sTempTodate = objReport.sTodate;
                    if (Request.QueryString["FromDate"].ToString() == null || Request.QueryString["FromDate"].ToString() == "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                    }
                    else
                    {
                        objReport.sFromDate = DFromDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() == null || Request.QueryString["ToDate"].ToString() == "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                    }
                    else
                    {
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    dt = objReport.PENDINGWTREPARIER(objReport);
                    dt1 = objReport.TransformerWiseDetails(objReport);
                    if (dt1.Rows.Count > 0)
                    {
                        crpReprierPer.SetDataSource(dt);
                        crpReprierPer.OpenSubreport("Transformerwisedetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = crpReprierPer;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "Delivered Analysis Report")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = Convert.ToDateTime(DFromDate).ToString("dd-MMM-yyyy");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DtoDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = Convert.ToDateTime(DtoDate).ToString("dd-MMM-yyyy");
                    }
                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }
                    if (Request.QueryString["StrReprierName"].ToString() != null && Request.QueryString["StrReprierName"].ToString() != "")
                    {
                        objReport.sRepriername = Request.QueryString["StrReprierName"].ToString();
                    }
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    dt = objReport.ReperierCompleted(objReport);
                    dt1 = objReport.TransformerWiseDetailsCompleted(objReport);
                    if (dt1.Rows.Count > 0)
                    {
                        crpCompleted.SetDataSource(dt);
                        crpCompleted.OpenSubreport("TransformerwiseCompleteddetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = crpCompleted;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                //dtrMakeView
                if (Request.QueryString["id"] == "DTr make wise Reports")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["MakeValue"].ToString() != null && Request.QueryString["MakeValue"].ToString() != "")
                    {
                        objReport.sMake = Request.QueryString["MakeValue"].ToString();
                    }
                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }
                    dt = objReport.Printdtrwise(objReport);
                    dt1 = objReport.PrintMakeWiseDetails(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        objDtr.SetDataSource(dt);
                        objDtr.OpenSubreport("crpMakeWiseDetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = objDtr;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }


                //dtr RepairerWise Report
                if (Request.QueryString["id"] == "DTr RepairerWise")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    DataTable dtOthers = new DataTable();
                    DataTable dtAbstract = new DataTable();


                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");

                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");

                    }

                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {

                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }


                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    dt = objReport.PrintRePairerwise(objReport);
                    dtOthers = objReport.PrintRePairerOtherswise(objReport);
                    DataTable dt2 = new DataTable();
                    string strOthers = dtOthers.Rows[0]["FAILURE"].ToString();

                    if (dt.Rows.Count == 0)
                    {
                        dt.Rows.Add("NO REPAIRER NAME");
                        dt.Columns[1].ReadOnly = false;
                        dt.Rows[dt.Rows.Count - 1]["MORETHANONCE"] = "0";
                    }
                    if (strOthers != "")
                    {
                        dt.Rows.Add("FAILURE");
                        dt.Columns[1].ReadOnly = false;
                        dt.Rows[dt.Rows.Count - 1]["MORETHANONCE"] = strOthers;
                    }

                    DataTable dtFinal = new DataTable();
                    dtFinal = dt;
                    dtAbstract = objReport.PrintDtrRepairerWise(objReport);
                    if (dtFinal.Rows.Count > 0)
                    {
                        crpRepairerWise.SetDataSource(dtFinal);
                        crpRepairerWise.OpenSubreport("crpDtrRepairerWiseDetails.rpt").SetDataSource(dtAbstract);
                        crpPrint.ReportSource = crpRepairerWise;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"] == "FailureAbstract")
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    clsReports objReport = new clsReports();
                    ReportDocument repDoc = new ReportDocument();

                    objReport.sOfficeCode = objSession.OfficeCode;

                    if (Request.QueryString["Month"].ToString() != null && Request.QueryString["Month"].ToString() != "")
                    {
                        objReport.sMonth = Request.QueryString["Month"].ToString();
                        objReport.sCurrentMonth = DateTime.Now.ToString("yyyy-MM");
                    }
                    objReport.sReportType = Request.QueryString["ReportType"].ToString();

                    dt = objReport.FailureAbstract(objReport);
                    dt1 = objReport.FailMonthWiseAbstract(objReport);
                    if (dt.Rows.Count > 0)
                    {

                        crpFailureAbstract.SetDataSource(dt);

                        crpFailureAbstract.OpenSubreport("FailureAbstractMonthWise.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = crpFailureAbstract;
                        crpPrint.DataBind();
                        //crpFailureAbstract.SetParameterValue("ReportType", objReport.sReportType);
                        //string testpath = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;
                        //repDoc.Load(testpath + "\\IIITS.DTLMS.REPORTS\\FailureAbstract.rpt");//"C:\\Users\\jeevan.j\\Desktop\\SVN\\IIITS.DTLMS.REPORTS\\FailureAbstract.rpt"
                        //repDoc.SetDataSource(dt);
                        //repDoc.Subreports["FailureAbstractMonthWise.rpt"].SetDataSource(dt1);
                        //repDoc.SetParameterValue("ReportType", objReport.sReportType);

                        //repDoc.Subreports[0].SetParameterValue("ReportType", objReport.sReportType);

                        //crpPrint.DataBind();
                        //crpPrint.ID = "FailureAbstract-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }
                if (Request.QueryString["id"].ToString().Equals("PurchaseOrderOil"))
                {

                    DataTable purchaseorderoil = new DataTable();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string agency_name = string.Empty;
                    string agency_address = string.Empty;
                    string Division = string.Empty;
                    string Utilized_Amount = string.Empty;
                    string Budget_SanctionAmt = string.Empty;
                    string Available_Amount = string.Empty;
                    string Debit_Amount = string.Empty;
                    string sPono = Request.QueryString["Pono"].ToString();
                    string sOilqty = Request.QueryString["Oilqty"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string Agencyid = Request.QueryString["AgencyId"].ToString();

                    purchaseorderoil = objReport.PrintPurchaseorderoil(Agencyid, sOfficeCode, sPono);
                    Double Total = Convert.ToDouble(sOilqty) * 3.54;
                    if (purchaseorderoil.Rows.Count > 0)
                    {
                        agency_name = purchaseorderoil.Rows[0]["AGENCY_NAME"].ToString();
                        agency_address = purchaseorderoil.Rows[0]["AGENCY_ADDRESS"].ToString();
                        Division = purchaseorderoil.Rows[0]["DIVISION"].ToString();
                        Utilized_Amount = purchaseorderoil.Rows[0]["Utilized_Amount"].ToString();
                        Budget_SanctionAmt = purchaseorderoil.Rows[0]["Budget_SanctionAmt"].ToString();
                        Available_Amount = purchaseorderoil.Rows[0]["Available_Amount"].ToString();
                        Debit_Amount = purchaseorderoil.Rows[0]["Debit_Amount"].ToString();
                    }
                    //crppurchaseorderoil.SetParameterValue("PO_No", sPono);
                    //crppurchaseorderoil.SetParameterValue("PO_QUANTITY", sOilqty);
                    //crppurchaseorderoil.SetParameterValue("Total", Total);
                    //crppurchaseorderoil.SetParameterValue("dousername", "");
                    dt.Columns.Add("AGENCY_NAME");
                    dt.Columns.Add("AGENCY_ADDRESS");
                    dt.Columns.Add("PO_No");
                    dt.Columns.Add("PO_QUANTITY");
                    dt.Columns.Add("Total");
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("Available_Amount");
                    dt.Columns.Add("Debit_Amount");
                    dt.Columns.Add("Utilized_Amount");
                    dt.Columns.Add("Budget_SanctionAmt");

                    DataRow dRow = dt.NewRow();
                    //dRow["PO_No"] = sPono;
                    //dRow["PO_QUANTITY"] = sOilqty;
                    //dRow["Total"] = Total;
                    dRow[0] = agency_name;
                    dRow[1] = agency_address;
                    dRow[2] = sPono.ToUpper();
                    dRow[3] = sOilqty;
                    dRow[4] = Total;
                    dRow[5] = Division;
                    dRow[6] = Available_Amount;
                    dRow[7] = Debit_Amount;
                    dRow[8] = Utilized_Amount;
                    dRow[9] = Budget_SanctionAmt;
                    dt.Rows.Add(dRow);
                    //  purchaseorderoil.AcceptChanges();
                    crppurchaseorderoil.SetDataSource(dt);
                    crpPrint.ReportSource = crppurchaseorderoil;
                    crpPrint.DataBind();
                }


                if (Request.QueryString["id"] == "MisDTCCountFeeder")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["OfficeCode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    }
                    if (Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }
                    if (Request.QueryString["FromDate"].ToString() != "")
                    {
                        //objReport.sFromDate = Request.QueryString["FromDate"].ToString();

                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != "")
                    {
                        //objReport.sTodate = Request.QueryString["ToDate"].ToString();

                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DToDate = DToDate.AddDays(1);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }


                    dt = objReport.DTCCountDetails(objReport);

                    var tobeReplacedRows = (from failureReplacement in dt.AsEnumerable()
                                            where failureReplacement.Field<string>("FEEDER") != null
                                            select failureReplacement);

                    if (!(tobeReplacedRows.Any()))
                    {
                        ShowMsgBox("No Records Found");
                        return;
                    }


                    if (dt.Rows.Count > 0)
                    {
                        crpMISDTCCountFeeder.SetDataSource(dt);
                        crpMISDTCCountFeeder.SetParameterValue("fromDate", objReport.sFromDate);
                        crpMISDTCCountFeeder.SetParameterValue("toDate", objReport.sTodate);
                        crpPrint.ReportSource = crpMISDTCCountFeeder;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }


                #region DTC Added Abstract 
                if (Request.QueryString["id"] == "DTCAddedAbstractReport")
                {
                    DataTable dt = new DataTable();

                    clsReports objReport = new clsReports();
                    string repotType = string.Empty;

                    //objReport.sFromDate = Request.QueryString["FromMonth"].ToString();
                    //objReport.sTodate = Request.QueryString["ToMonth"].ToString();

                    if (Request.QueryString["FromMonth"].ToString() != null && Request.QueryString["FromMonth"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromMonth"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");

                    }
                    if (Request.QueryString["ToMonth"].ToString() != null && Request.QueryString["ToMonth"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToMonth"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ReportType"].ToString() != "")
                    {
                        objReport.sReportType = Request.QueryString["ReportType"].ToString();
                    }
                    if (Request.QueryString["ReportType"].ToString() != "")
                    {
                        strArr = Request.QueryString["ReportType"].ToString();
                    }

                    switch (objReport.sReportType)
                    {
                        case "1":
                            dt = objReport.CapacityAbstractDetails(objReport);
                            var finalDataCapacityAbstract = (from failureReplacement in dt.AsEnumerable()
                                                             where failureReplacement.Field<Decimal>("TC_CAPACITY").ToString().Contains("1")
                                                             select failureReplacement);
                            if (finalDataCapacityAbstract.Any())
                            {
                                // dt = finalDataRow.CopyToDataTable<DataRow>();
                                crpCapacityAbstract.SetDataSource(dt);
                                crpPrint.ReportSource = crpCapacityAbstract;
                                crpPrint.DataBind();
                            }
                            else
                            {
                                ShowMsgBox("No Records Found");
                            }

                            break;

                        case "2":
                            dt = objReport.WorkwiseAbstractDetails(objReport);
                            var finalWorkwiseAbstract = (from failureReplacement in dt.AsEnumerable()
                                                         where failureReplacement.Field<Decimal>("DT_CODE").ToString().Contains("1")
                                                         select failureReplacement);

                            if (finalWorkwiseAbstract.Any())
                            {
                                // dt = finalDataRow.CopyToDataTable<DataRow>();
                                crpWorkwiseAbstract.SetDataSource(dt);
                                crpPrint.ReportSource = crpWorkwiseAbstract;
                                crpPrint.DataBind();
                            }
                            else
                            {
                                ShowMsgBox("No Records Found");
                            }
                            break;
                        case "3":
                            dt = objReport.FeederOrCategoryAbstractDetails(objReport);
                            var finalFeederOrCategory = (from failureReplacement in dt.AsEnumerable()
                                                         where failureReplacement.Field<Decimal>("DT_CODE").ToString().Contains("1")
                                                         select failureReplacement);
                            if (finalFeederOrCategory.Any())
                            {
                                // dt = finalDataRow.CopyToDataTable<DataRow>();
                                crpFeederAbstract.SetDataSource(dt);
                                crpPrint.ReportSource = crpFeederAbstract;
                                crpPrint.DataBind();
                            }
                            else
                            {
                                ShowMsgBox("No Records Found");
                            }

                            break;
                        default:
                            ShowMsgBox("No Records Found");
                            break;


                    }
                    //if (objReport.sReportType == "1")
                    //{
                    //    dt = objReport.CapacityAbstractDetails(objReport);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        crpDTCAddedAbstract.SetDataSource(dt);
                    //        crpPrint.ReportSource = crpDTCAddedAbstract;
                    //        crpPrint.DataBind();

                    //    }
                    //    else
                    //    {
                    //        ShowMsgBox("No Records Found");
                    //    }
                    //}
                    //else if (objReport.sReportType == "2")
                    //{
                    //    dt = objReport.WorkwiseAbstractDetails(objReport);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        crpDTCAddedAbstract.SetDataSource(dt);
                    //        crpPrint.ReportSource = crpDTCAddedAbstract;
                    //        crpPrint.DataBind();

                    //    }
                    //    else
                    //    {
                    //        ShowMsgBox("No Records Found");
                    //    }
                    //}

                    //else if (objReport.sReportType == "3")
                    //{
                    //    dt = objReport.FeederOrCategoryAbstractDetails(objReport);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        crpDTCAddedAbstract.SetDataSource(dt);
                    //        crpPrint.ReportSource = crpDTCAddedAbstract;
                    //        crpPrint.DataBind();

                    //    }
                    //    else
                    //    {
                    //        ShowMsgBox("No Records Found");
                    //    }
                    //}

                }


                #endregion

                #region DTCFailure Report

                if (Request.QueryString["id"] == "FrequentTCFail")
                {

                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["FailType"].ToString() != null && Request.QueryString["FailType"].ToString() != "")
                    {
                        objReport.sFailureType = Request.QueryString["FailType"].ToString();
                    }
                    if (Request.QueryString["GuranteeType"].ToString() != null && Request.QueryString["GuranteeType"].ToString() != "")
                    {
                        objReport.sGuranteeType = Request.QueryString["GuranteeType"].ToString();
                    }
                    if (Request.QueryString["DTCCode"].ToString() != null && Request.QueryString["DTCCode"].ToString() != "")
                    {
                        objReport.sDtcCode = Request.QueryString["DTCCode"].ToString();
                    }
                    if (Request.QueryString["DTRCode"].ToString() != null && Request.QueryString["DTRCode"].ToString() != "")
                    {
                        objReport.sDtrCode = Request.QueryString["DTRCode"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();

                    dt = objReport.FrequentTcFail(objReport);
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();

                    if (dt.Rows.Count > 0)
                    {
                        crpDtcFailFrequent.SetDataSource(dt);
                        crpPrint.ReportSource = crpDtcFailFrequent;
                        crpPrint.DataBind();

                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                // Frequent Failure DTR Report

                if (Request.QueryString["id"] == "FrequentDTRFail")
                {

                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["FailType"].ToString() != null && Request.QueryString["FailType"].ToString() != "")
                    {
                        objReport.sFailureType = Request.QueryString["FailType"].ToString();
                    }
                    if (Request.QueryString["GuranteeType"].ToString() != null && Request.QueryString["GuranteeType"].ToString() != "")
                    {
                        objReport.sGuranteeType = Request.QueryString["GuranteeType"].ToString();
                    }
                    if (Request.QueryString["DTCCode"].ToString() != null && Request.QueryString["DTCCode"].ToString() != "")
                    {
                        objReport.sDtcCode = Request.QueryString["DTCCode"].ToString();
                    }
                    if (Request.QueryString["DTRCode"].ToString() != null && Request.QueryString["DTRCode"].ToString() != "")
                    {
                        objReport.sDtrCode = Request.QueryString["DTRCode"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();

                    dt = objReport.FrequentFailDTR(objReport);
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();

                    if (dt.Rows.Count > 0)
                    {

                        crpFrequentDTRFail.SetDataSource(dt);
                        crpPrint.ReportSource = crpFrequentDTRFail;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                #endregion


                //Cregister abstract report
                #region reg abstract
                //if (Request.QueryString["id"] == "RegAbstract")
                // {
                //     clsReports objReport = new clsReports();
                //     string Fromdate = string.Empty;
                //     string Todate = string.Empty;
                //     DataTable dt = new DataTable();
                //     if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                //     {
                //         objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                //     }
                //     if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                //     {
                //         objReport.sTodate = Request.QueryString["ToDate"].ToString();
                //     }

                //     if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                //     {

                //         objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                //     }

                //     Fromdate = Request.QueryString["FromDate"].ToString();
                //     Todate = Request.QueryString["ToDate"].ToString();

                //     dt = objReport.PrintRegAbstact(objReport);

                //     if (dt.Rows.Count > 0)
                //     {
                //         objreg.SetDataSource(dt);
                //         crpPrint.ReportSource = objreg;
                //         crpPrint.DataBind();
                //         crpPrint.ID = "RegAbstact-" + stroffcode + "-" + strTodayDate;

                //         ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                //         if (Fromdate != string.Empty)
                //         {
                //             crParameterDiscreteValue4.Value = "From  " + Fromdate;
                //         }
                //         else
                //         {
                //             crParameterDiscreteValue4.Value = "";
                //         }

                //         crParameterFieldDefinitions = objreg.DataDefinition.ParameterFields;
                //         crParameterFieldDefinition = crParameterFieldDefinitions["crpFromdate"];
                //         crParameterValues = crParameterFieldDefinition.CurrentValues;
                //         crParameterValues.Add(crParameterDiscreteValue4);
                //         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                //         ParameterDiscreteValue crParameterDiscreteValue2 = new ParameterDiscreteValue();
                //         if (Todate != string.Empty)
                //         {
                //             crParameterDiscreteValue2.Value = "To  " + Todate;
                //         }
                //         else
                //         {
                //             crParameterDiscreteValue2.Value = "";
                //         }

                //         crParameterFieldDefinitions = objreg.DataDefinition.ParameterFields;
                //         crParameterFieldDefinition = crParameterFieldDefinitions["crpTodate"];
                //         crParameterValues = crParameterFieldDefinition.CurrentValues;
                //         crParameterValues.Add(crParameterDiscreteValue2);
                //         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                //     }
                //     else
                //     {
                //         ShowMsgBox("No Records Found");
                //     }


                // }
                #endregion

            }
            catch (Exception ex)
            {
                StackFrame CallStack = new StackFrame(1, true);
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load" + strReport + " Line:" + CallStack.GetFileLineNumber());
            }

        }

        public DataTable LoadSignImages(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                clsReports objreport = new clsReports();
                List<string> sImagePaths = new List<string>();
                DataTable dtRoles = new DataTable();

                sImagePaths = objreport.PrintBlob(sOfficeCode);
                dtRoles = objreport.GetRoles();
                DataRow drow = dt.NewRow();
                dt.Rows.Add(drow);
                foreach (string str in sImagePaths)
                {
                    DataColumnCollection columns = dt.Columns;
                    string[] strRoles = str.Split('~');
                    if (!columns.Contains(strRoles[1]))
                    {
                        dt.Columns.Add(strRoles[1]);
                        drow[strRoles[1]] = strRoles[0];
                    }
                }
                dt = ContainColumn(dt, dtRoles);
                return dt;

                //crpBlob crpBlob = new crpBlob();
                //crpBlob.SetDataSource(dt);
                //crpPrint.ReportSource = crpBlob;
                //crpPrint.DataBind();
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSignImages");
                return dt;
            }
        }

        public DataTable ContainColumn(DataTable dt, DataTable dtRoles)
        {

            DataColumnCollection columns = dt.Columns;
            for (int i = 0; i < dtRoles.Rows.Count; i++)
            {
                string strColumn = Convert.ToString(dtRoles.Rows[i]["RO_NAME"]);
                if (!columns.Contains(strColumn))
                {
                    dt.Columns.Add(strColumn);
                }
            }
            return dt;
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {

            if (strReport == "EnumReport")
            {
                crpOper.Close();
                crpOper.Dispose();
            }
            if (strReport == "DetailedField")
            {
                crpDetailedFieldReport.Close();
                crpDetailedFieldReport.Dispose();
            }
            if (strReport == "DetailedStore")
            {
                crpDetailedStoreReport.Close();
                crpDetailedStoreReport.Dispose();
            }
            if (strReport == "LocOperator")
            {
                crpLocOperator.Close();
                crpLocOperator.Dispose();
            }
            if (strReport == "FieldLoc")
            {
                crpLocField.Close();
                crpLocField.Dispose();
            }
            if (strReport == "StoreLoc")
            {
                crpLocStore.Close();
                crpLocStore.Dispose();
            }
            if (strReport == "Estimation")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "EstimationSO")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "WorkOrderPreview")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "WorkOrder")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "GatePass")
            {
                crpGatepassReport.Close();
                crpGatepassReport.Dispose();
            }
            if (strReport == "CRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RIReport")
            {
                crpRIReport.Close();
                crpRIReport.Dispose();
            }
            if (strReport == "RIAckReport")
            {
                crpRIAck.Close();
                crpRIAck.Dispose();
            }
            if (strReport == "IndentReport")
            {
                crpIndent.Close();
                crpIndent.Dispose();
            }
            if (strReport == "InvoiceReport")
            {
                crpInvoice.Close();
                crpInvoice.Dispose();
            }
            if (strReport == "EnhanceEstimation")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceEstimationSO")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceCRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "EnhanceIndentReport")
            {
                crpEnhanceIndent.Close();
                crpEnhanceIndent.Dispose();
            }
            if (strReport == "EnhanceInvoiceReport")
            {
                crpEnhanceInvoice.Close();
                crpEnhanceInvoice.Dispose();
            }
            if (strReport == "RepairGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "StoreGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapInvoice")
            {
                crpScrapInvoice.Close();
                crpScrapInvoice.Dispose();
            }
            if (strReport == "InterStoreInvoice")
            {
                crpStoreInvoice.Close();
                crpStoreInvoice.Dispose();
            }
            if (strReport == "RecieveDTR")
            {
                crpRecieveDTr.Close();
                crpRecieveDTr.Dispose();
            }
            if (strReport == "InterStoreIndent")
            {
                crpStoreIndent.Close();
                crpStoreIndent.Dispose();
            }
            if (strReport == "AbstractReport")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "AbstractRptTcFailed")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTrReportMake")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTCReportFeeder")
            {
                crpDTCRep.Close();
                crpDTCRep.Dispose();
            }
            if (strReport == "TCFail")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "CRAbstract")
            {
                crpCRAbstract.Close();
                crpCRAbstract.Dispose();
            }
            if (strReport == "WorkOderReg")
            {
                crpWoRegDetails.Close();
                crpWoRegDetails.Dispose();
            }
            if (strReport == "DTCAddDetails")
            {
                crpAddDTC.Close();
                crpAddDTC.Dispose();
            }
            if (strReport == "CRDetails")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RegAbstract")
            {

                objreg.Close();
                objreg.Dispose();


            }

            if (strReport == "Pending Analysis Report")
            {
                crpReprierPer.Close();
                crpReprierPer.Dispose();
            }

            if (strReport == "Delivered Analysis Report")
            {
                crpCompleted.Close();
                crpCompleted.Dispose();
            }

            if (strReport == "DTr make wise Reports")
            {
                objDtr.Close();
                objDtr.Dispose();
            }


            if (strReport == "DTr RepairerWise")
            {
                crpRepairerWise.Close();
                crpRepairerWise.Dispose();
            }
            if (strReport == "FailureAbstract")
            {
                crpFailureAbstract.Close();
                crpFailureAbstract.Dispose();
            }

            if (strReport == "FrequentTCFail")
            {
                crpDtcFailFrequent.Close();
                crpDtcFailFrequent.Dispose();
            }
            if (strReport == "MisCommisionPending")
            {
                crpMisCommissionPending.Close();
                crpMisCommissionPending.Dispose();
            }

            if (strReport == "MisFaultyDtr")
            {
                crpMisFaultyDtrDetails.Close();
                crpMisFaultyDtrDetails.Dispose();
            }
            if (strReport == "FrequentDTRFail")
            {
                crpFrequentDTRFail.Close();
                crpFrequentDTRFail.Dispose();
            }
            if (strReport == "MisFailureReplacement")
            {
                crpMisPendingFailureReplacement.Close();
                crpMisPendingFailureReplacement.Dispose();
            }
            if (strReport == "MisRepairerPerformance")
            {
                crpRepairerPerformance.Close();
                crpRepairerPerformance.Dispose();
            }

            if (strReport == "FeederBifurcation")
            {
                crpobjFeederBifurcation.Close();
                crpobjFeederBifurcation.Dispose();
            }
            if (strReport == "TCFailDetails")
            {
                objFailRep.Close();
                objFailRep.Dispose();
            }
            if (strReport == "MisReplacableDTR")
            {
                objcrpMisReplacableDTR.Close();
                objcrpMisReplacableDTR.Dispose();
            }
            if (strReport == "FeederBifurcationSO")
            {
                objcrpFeederBifurcationSO.Close();
                objcrpFeederBifurcationSO.Dispose();
            }
            if (strReport == "PurchaseOrderOil")
            {
                crppurchaseorderoil.Close();
                crppurchaseorderoil.Dispose();
            }

        }

        protected void crpPrint_Unload(object sender, EventArgs e)
        {
            if (strReport == "EnumReport")
            {
                crpOper.Close();
                crpOper.Dispose();
            }
            if (strReport == "FeederBifurcation")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DetailedField")
            {
                crpDetailedFieldReport.Close();
                crpDetailedFieldReport.Dispose();
            }
            if (strReport == "DetailedStore")
            {
                crpDetailedStoreReport.Close();
                crpDetailedStoreReport.Dispose();
            }
            if (strReport == "LocOperator")
            {
                crpLocOperator.Close();
                crpLocOperator.Dispose();
            }
            if (strReport == "FieldLoc")
            {
                crpLocField.Close();
                crpLocField.Dispose();
            }
            if (strReport == "StoreLoc")
            {
                crpLocStore.Close();
                crpLocStore.Dispose();
            }
            if (strReport == "Estimation")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "EstimationSO")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "WorkOrderPreview")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "WorkOrder")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "GatePass")
            {
                crpGatepassReport.Close();
                crpGatepassReport.Dispose();
            }
            if (strReport == "CRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RIReport")
            {
                crpRIReport.Close();
                crpRIReport.Dispose();
            }
            if (strReport == "RIAckReport")
            {
                crpRIAck.Close();
                crpRIAck.Dispose();
            }
            if (strReport == "IndentReport")
            {
                crpIndent.Close();
                crpIndent.Dispose();
            }
            if (strReport == "InvoiceReport")
            {
                crpInvoice.Close();
                crpInvoice.Dispose();
            }
            if (strReport == "EnhanceEstimation")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceEstimationSO")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceCRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "EnhanceIndentReport")
            {
                crpEnhanceIndent.Close();
                crpEnhanceIndent.Dispose();
            }
            if (strReport == "EnhanceInvoiceReport")
            {
                crpEnhanceInvoice.Close();
                crpEnhanceInvoice.Dispose();
            }
            if (strReport == "RepairGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "StoreGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapInvoice")
            {
                crpScrapInvoice.Close();
                crpScrapInvoice.Dispose();
            }
            if (strReport == "InterStoreInvoice")
            {
                crpStoreInvoice.Close();
                crpStoreInvoice.Dispose();
            }
            if (strReport == "RecieveDTR")
            {
                crpRecieveDTr.Close();
                crpRecieveDTr.Dispose();
            }
            if (strReport == "InterStoreIndent")
            {
                crpStoreIndent.Close();
                crpStoreIndent.Dispose();
            }
            if (strReport == "AbstractReport")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "AbstractRptTcFailed")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTrReportMake")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTCReportFeeder")
            {
                crpDTCRep.Close();
                crpDTCRep.Dispose();
            }
            if (strReport == "TCFail")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "CRAbstract")
            {
                crpCRAbstract.Close();
                crpCRAbstract.Dispose();
            }
            if (strReport == "WorkOderReg")
            {
                crpWoRegDetails.Close();
                crpWoRegDetails.Dispose();
            }
            if (strReport == "DTCAddDetails")
            {
                crpAddDTC.Close();
                crpAddDTC.Dispose();
            }
            if (strReport == "CRDetails")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RegAbstract")
            {

                objreg.Close();
                objreg.Dispose();

            }

            if (strReport == "Pending Analysis Report")
            {
                crpReprierPer.Close();
                crpReprierPer.Dispose();
            }

            if (strReport == "Delivered Analysis Report")
            {
                crpCompleted.Close();
                crpCompleted.Dispose();
            }

            if (strReport == "DTr make wise Reports")
            {
                objDtr.Close();
                objDtr.Dispose();
            }

            if (strReport == "DTr RepairerWise")
            {
                crpRepairerWise.Close();
                crpRepairerWise.Dispose();
            }
            if (strReport == "FailureAbstract")
            {
                crpFailureAbstract.Close();
                crpFailureAbstract.Dispose();
            }
            if (strReport == "FrequentTCFail")
            {
                crpDtcFailFrequent.Close();
                crpDtcFailFrequent.Dispose();
            }
            if (strReport == "MisCommisionPending")
            {
                crpMisCommissionPending.Close();
                crpMisCommissionPending.Dispose();
            }
            if (strReport == "MisFaultyDtr")
            {
                crpMisFaultyDtrDetails.Close();
                crpMisFaultyDtrDetails.Dispose();
            }

            if (strReport == "FrequentDTRFail")
            {
                crpFrequentDTRFail.Close();
                crpFrequentDTRFail.Dispose();
            }
            if (strReport == "MisFailureReplacement")
            {
                crpMisPendingFailureReplacement.Close();
                crpMisPendingFailureReplacement.Dispose();
            }
            if (strReport == "MisRepairerPerformance")
            {
                crpRepairerPerformance.Close();
                crpRepairerPerformance.Dispose();
            }

            if (strReport == "FeederBifurcation")
            {
                crpobjFeederBifurcation.Close();
                crpobjFeederBifurcation.Dispose();
            }
            if (strReport == "TCFailDetails")
            {
                objFailRep.Close();
                objFailRep.Dispose();
            }
            if (strReport == "MisReplacableDTR")
            {
                objcrpMisReplacableDTR.Close();
                objcrpMisReplacableDTR.Dispose();
            }

            if (strReport == "FeederBifurcationSO")
            {
                objcrpFeederBifurcationSO.Close();
                objcrpFeederBifurcationSO.Dispose();
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
    }
}