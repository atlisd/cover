using Cover.Shared.DTOs;

namespace Cover.Api.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<List<UserDto>> CreateUsersAsync(string name1, string name2);
    Task<UserDto> UpdateNameAsync(int id, string name);
    Task<bool> IsSetupCompleteAsync();
}
