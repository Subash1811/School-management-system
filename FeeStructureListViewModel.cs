using Student_manage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_manage.ViewModels
{
    public class FeesStructureListViewModel
    {
        public int S_No { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? Amount { get; set; }
        public string Standard { get; set; }
        public string Academic_year { get; set; }
        public int? Term_Fees { get; set; }
        public string Payment_Mode { get; set; }
        public string Created_by { get; set; }
        public DateTime? Created_on { get; set; }
        public int Fine_Amount { get; set; }
        public int Total_Payable { get; set; }
        
            public List<FeesStructure> FeeStructures { get; set; } = new List<FeesStructure>();
       

    }
}
