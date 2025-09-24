using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_manage.Models;
using PagedList;

namespace Student_manage.ViewModels
{
    public class StudentSearchViewModel
    {
        [Display(Name = "Student ID")]
        public int? StudentID { get; set; }

        [Display(Name = "First Name")]
        public string FIRST_NAME { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DATE_OF_BIRTH { get; set; }

        [Display(Name = "Phone Number")]
        public long? PHONE { get; set; }
        public List<StudentRegister> Results { get; set; } = new List<StudentRegister>();
        
    }
}
