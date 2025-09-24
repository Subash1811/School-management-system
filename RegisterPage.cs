using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Student_manage.Models
{
    [Table("student_register")]
    public class StudentRegister
    {
        [Key]
         public int STU_ID { get; set; }

        [Required(ErrorMessage = "*First Name field required")]
        [StringLength(50)]
        public string FIRST_NAME { get; set; }

        [StringLength(50)]
        public string LAST_NAME { get; set; }

        [Required(ErrorMessage = "*Date of Birth field required")]
        [DataType(DataType.Date)]
        public DateTime DATE_OF_BIRTH { get; set; }

        [Required(ErrorMessage = "*Gender field Required")]
        [StringLength(10)]
        public string GENDER { get; set; }

        [Required(ErrorMessage = "*Age field Required")]
        public int AGE { get; set; }

        [Required(ErrorMessage = "*Email field required")]
        [StringLength(100)]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "*Phone number field required")]
        public long? PHONE { get; set; }
        

        [Required(ErrorMessage = "*Address field Required")]
        [StringLength(225)]
        public string ADDRESS { get; set; }

        public int? City_ID { get; set; }
        [ForeignKey("City_ID")]
        public virtual City_Master City { get; set; }

        public int? State_ID { get; set; }
        [ForeignKey("State_ID")]
        public virtual State_Master State { get; set; }

        [Required(ErrorMessage = "*Pincode field Required")]
        public int? PINCODE { get; set; }

        [Required(ErrorMessage = "*Parents field Required")]
        [StringLength(50)]
        public string PARENTS { get; set; }

        [Required(ErrorMessage = "*Relation field Required")]
        [StringLength(50)]
        public string RELATIONSHIP { get; set; }

       

      
        [Display(Name = "Created By")]
        public string Created_by { get; set; }

      
        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime Created_on { get; set; }
        public bool? Active { get; set; }
        public bool? InActive { get; set; }
        // StudentRegister.cs
        

    }
}