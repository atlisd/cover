using Cover.Shared.DTOs;

namespace Cover.Api.Models;

public class Expense
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public long Amount { get; set; }
    public int PaidById { get; set; }
    public User PaidBy { get; set; } = null!;
    public SplitType SplitType { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
