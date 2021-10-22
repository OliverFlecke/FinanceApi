using FinanceApi.Areas.Account.Dtos;
using FinanceApi.Areas.Account.Models;

namespace FinanceApi.Areas.Account.Services;

public interface IAccountRepository
{
    Task<IEnumerable<AccountWithEntriesResponse>> GetAccountsWithEntries(int userId);

    /// <summary>
    /// Add an account for the given user with a name and a type.
    /// </summary>
    /// <param name="userId">User to add the account for.</param>
    /// <param name="name">Name of the account.</param>
    /// <param name="type">Type of the account.</param>
    /// <returns>Id of the newly created account.</returns>
    Task<Guid> AddAccount(int userId, string name, AccountType type);
}
