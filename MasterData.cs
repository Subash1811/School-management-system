using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Student_manage.Models
{

    [Table("MasterData")]
    public class MasterData
    {
        [Key]
        public int Id { get; set; }

        public string Admission_Category {get;set;}


    }
}