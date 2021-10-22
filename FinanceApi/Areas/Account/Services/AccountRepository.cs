using FinanceApi.Areas.Account.Dtos;
using FinanceApi.Areas.Account.Extensions;
using FinanceApi.Areas.Account.Models;
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

    /// <inheritdoc/>
    public async Task<Guid> AddAccount(int userId, string name, AccountType type)
    {
        _logger.LogInformation($"Adding account '{name}' for user '{userId}'");

        var entity = await _context.Account.SingleOrDefaultAsync(x =>
            x.UserId == userId
            && x.Name == name
            && x.Type == type);

        if (entity is not null)
        {
            return entity.Id;
        }

        var account = _context.Account.Add(new()
        {
            UserId = userId,
            Name = name,
            Type = type,
        });
        await _context.SaveChangesAsync();

        return account.Entity.Id;
    }

}