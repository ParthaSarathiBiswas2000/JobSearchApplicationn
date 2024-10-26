﻿using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FilterJobMV
    {
        public FilterJobMV()
        {
            Result = new List<PostJobTable>();
        }
        public int JobCategoryID { get; set; }
        public int JobNatureID { get; set; }
        public int NoOfDays { get; set; }
        public List<PostJobTable> Result {  get; set; }
    }
}