using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIITS.DTLMS
{
    public class clsSession
    {
        public  string  UserId { get; set; }
        public string FullName { get; set; }
        public string LoginName { get; set; }
        public string UserType { get; set; }
        public string OfficeCode { get; set; }
        public string RoleId { get; set; }
        public string OfficeName { get; set; }
        public string Designation { get; set; }
        public string OfficeNameWithType { get; set; } //Type Means Division,Sub Division etc
        public string sGeneralLog { get; set; }
        public string sTransactionLog { get; set; }
        public string sPasswordChangeRequest { get; set; }
        public string sPasswordChangeInDays { get; set; }
        public string sPassordAcceptance { get; set; }
        public string sProjectList { get; set; } // Included By Sandeep Becouse Of Common Login
        public string HRMSUsID { get; set; }

    }
}