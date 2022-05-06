using FinanceApi.Areas.Account.Dtos;

namespace FinanceApi.Areas.Account.Services;

public interface IAccountRepository
{
    Task<IEnumerable<AccountWithEntriesResponse>> GetAccountsWithEntries(int userId);

    /// <summary>
    /// Add an account for the given user with a name and a type.
    /// </summary>
    /// <param name="userId">User to add the account for.</param>
    /// <param name="request">Request to add account.</param>
    /// <returns>Id of the newly created account.</returns>
    Task<Guid> AddAccount(int userId, AddAccountRequest request);

    /// <summary>
    /// Update a list of accounts.
    /// </summary>
    /// <param name="userId">User id of the owner of the accounts.</param>
    /// <param name="request">Request with the accounts</param>
    /// <returns></returns>
    Task UpdateAccounts(int userId, IList<UpdateAccountRequest> request);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddAccountEntry(int userId, AddAccountEntryRequest request);
}
