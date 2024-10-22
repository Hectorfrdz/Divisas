using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Divisas.Models;

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

    [Column(TypeName = "decimal(18,4)")]
    public decimal Value { get; set; }

    public ICollection<Conversion> FromConversions { get; set; } = new List<Conversion>();

    public ICollection<Conversion> ToConversions { get; set; } = new List<Conversion>();

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
