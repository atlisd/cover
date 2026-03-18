using Cover.Shared.DTOs;

namespace Cover.Api.Services;

public interface IBalanceService
{
    Task<BalanceDto?> GetBalanceAsync();
}
