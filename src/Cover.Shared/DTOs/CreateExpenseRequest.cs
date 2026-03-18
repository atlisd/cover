namespace Cover.Shared.DTOs;

public record CreateExpenseRequest(
    string Description,
    long Amount,
    SplitType SplitType,
    int PaidById,
    DateOnly Date);
