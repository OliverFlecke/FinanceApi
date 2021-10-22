using FinanceApi.Areas.Account.Dtos;
using FinanceApi.Areas.Account.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Areas.Account.Services;

class AccountRepository : IAccountRepository
{
    readonly ILogger<AccountRepository> _logger;
    readonly FinanceContext _context;

    public AccountRepository(
        ILogger<AccountRepository> logger,
        FinanceContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AccountWithEntriesResponse>> GetAccountsWithEntries(int userId)
    {
        _logger.LogInformation($"Getting account entries for user '{userId}'");

        var accounts = await _context.Account
            .Include(a => a.Entries)
            .Where(a => a.UserId == userId)
            .ToListAsync();

        return accounts.Select(a => a.ToAccountWithEntriesResponse());
    }
}