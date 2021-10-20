using FinanceApi.Areas.User.Dtos;

namespace FinanceApi.Areas.User.Services;

public interface IUserService
{
    Task<UserResponse?> GetGithubUser(string? username);
}
