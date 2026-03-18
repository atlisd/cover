using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cover.Shared.DTOs;

namespace Cover.Web.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public ApiClient(HttpClient http) => _http = http;

    public async Task<SetupStatusDto> GetSetupStatusAsync()
        => await _http.GetFromJsonAsync<SetupStatusDto>("api/setup/status", JsonOptions)
           ?? new SetupStatusDto(false);

    public async Task<List<UserDto>> SetupAsync(SetupRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/setup", request, JsonOptions);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<UserDto>>(JsonOptions) ?? [];
    }

    public async Task<List<UserDto>> GetUsersAsync()
        => await _http.GetFromJsonAsync<List<UserDto>>("api/users", JsonOptions) ?? [];

    public async Task<BalanceDto?> GetBalanceAsync()
    {
        var response = await _http.GetAsync("api/balance");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<BalanceDto>(JsonOptions);
    }

    public async Task<PagedResult<ExpenseDto>> GetExpensesAsync(int page = 1, int pageSize = 20, int? paidById = null)
    {
        var url = $"api/expenses?page={page}&pageSize={pageSize}";
        if (paidById.HasValue) url += $"&paidById={paidById}";
        return await _http.GetFromJsonAsync<PagedResult<ExpenseDto>>(url, JsonOptions)
               ?? new PagedResult<ExpenseDto>([], 0, page, pageSize);
    }

    public async Task<ExpenseDto?> GetExpenseAsync(int id)
    {
        var response = await _http.GetAsync($"api/expenses/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ExpenseDto>(JsonOptions);
    }

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/expenses", request, JsonOptions);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ExpenseDto>(JsonOptions))!;
    }

    public async Task<ExpenseDto> UpdateExpenseAsync(int id, UpdateExpenseRequest request)
    {
        var response = await _http.PutAsJsonAsync($"api/expenses/{id}", request, JsonOptions);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ExpenseDto>(JsonOptions))!;
    }

    public async Task DeleteExpenseAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/expenses/{id}");
        response.EnsureSuccessStatusCode();
    }
}
