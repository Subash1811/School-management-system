using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_manage.Models
{
    [Table("state_master")]
    public class State_Master
    {
        [Key]
        public int State_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string State_Name { get; set; }

        [StringLength(25)]
        public string Created_by { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Created_on { get; set; }

        public bool? Active { get; set; }

        public bool? InActive { get; set; }

        public virtual ICollection<StudentRegister> Students { get; set; }
        public virtual ICollection<City_Master> Cities { get; set; }
    }
}
