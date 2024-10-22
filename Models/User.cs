using System;
using System.ComponentModel.DataAnnotations;

namespace Divisas.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Lastname { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; } = UserRole.Guest;

    public ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public enum UserRole
{
    Admin,
    Guest
}