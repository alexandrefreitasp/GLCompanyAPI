using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLCompanyAPI.Models
{
    [Table("Companies")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Exchange { get; set; }

        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; }

        [Required]
        [MaxLength(12)]
        [RegularExpression("^[A-Z]{2}[A-Z0-9]{10}$", ErrorMessage = "ISIN must start with two letters followed by ten alphanumeric characters.")]
        public string Isin { get; set; }

        [MaxLength(100)]
        public string? Website { get; set; }
    }
}