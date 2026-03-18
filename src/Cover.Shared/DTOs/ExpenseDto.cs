namespace Cover.Shared.DTOs;

public record ExpenseDto(
    int Id,
    string Description,
    long Amount,
    SplitType SplitType,
    int PaidById,
    string PaidByName,
    DateOnly Date,
    DateTime CreatedAt);
