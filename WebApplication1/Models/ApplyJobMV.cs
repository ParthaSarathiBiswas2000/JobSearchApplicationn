using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ApplyJobMV
    {
        public int JobApplyID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public string JobCategoryName { get; set; }
        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public System.DateTime JobApplyDateTime { get; set; }
        public int JobApplyStatusID { get; set; }
        public System.DateTime JobApplyStatusUpdateDateTime { get; set; }
        public string JobApplyStatusUpdateReason { get; set; }
        public int PostJobID { get; set; }
    }
}