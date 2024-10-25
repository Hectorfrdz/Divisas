using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Divisas.Models
{
    public class Currency
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,4)")]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal SalePrice { get; set; }

        [MaxLength(255)]
        public string Flag { get; set; } = string.Empty;
    }
}
