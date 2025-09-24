using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_manage.Models
{
    [Table("fees_structure")]
    public class FeesStructure
    {
        [Key]
        public int S_No { get; set; }

        [ForeignKey("Masterdata")]
        public int CategoryId { get; set; }  // FK to masterdata

        [NotMapped] // Not in DB, just for display
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public int? Amount { get; set; }

        public string Standard { get; set; }

        public string Academic_year { get; set; }

        public int? Term_Fees { get; set; }

        public string Payment_Mode { get; set; }

        public string Created_by { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Created_on { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Fine_Amount { get; set; }

        public bool? Active { get; set; }

        public bool? InActive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Total_Payable { get; set; }

        // Navigation property
        public virtual MasterData Masterdata { get; set; }
    }
}
