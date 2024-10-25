using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Divisas.Models
{
    public class Transaction
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [ForeignKey("FromCurrency")]
        public long FromCurrencyId { get; set; }
        public Currency? FromCurrency { get; set; }

        [Required]
        [ForeignKey("ToCurrency")]
        public long ToCurrencyId { get; set; }
        public Currency? ToCurrency { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal AmountConverted { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal ConvertedValue { get; set; }

        // Tipo de cambio aplicado (conversion_rate)
        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal ConversionRate { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
