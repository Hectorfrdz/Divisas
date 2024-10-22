using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Divisas.Models;

public class Transaction
{
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public long UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("Conversion")]
    public long? ConversionId { get; set; }
    public Conversion? Conversion { get; set; }

    [Required]
    [ForeignKey("Currency")]
    public long CurrencyId { get; set; }
    public Currency? Currency { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal ValueTransaction { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,6)")]
    public decimal TransactionRate { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
}

public enum TransactionType
{
    Buy,
    Sell
}