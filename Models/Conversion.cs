using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Divisas.Models;

public class Conversion
{
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public long UserId { get; set; }
    public User? User { get; set; }

    [Required]
    [ForeignKey("FromCurrency")]
    public long FromCurrencyId { get; set; }
    public Currency? FromCurrency { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal FromAmount { get; set; }

    [Required]
    [ForeignKey("ToCurrency")]
    public long ToCurrencyId { get; set; }
    public Currency? ToCurrency { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal ConvertedTo { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,6)")]
    public decimal ConversionRate { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
