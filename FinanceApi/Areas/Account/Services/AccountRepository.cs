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
    public async Task<IEnumerable<AccountWithEntriesResponse>> GetAccountsWithEntries(string userId)
    {
        _logger.LogInformation($"Getting account entries for user '{userId}'");

        var accounts = await _context.Account
            .Include(a => a.Entries)
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.SortKey)
            .ToListAsync();

        return accounts.Select(a => a.ToAccountWithEntriesResponse());
    }

    /// <inheritdoc/>
    public async Task<Guid> AddAccount(string userId, AddAccountRequest request)
    {
        _logger.LogInformation($"Adding account '{request.Name}' for user '{userId}'");

        var entity = await _context.Account.SingleOrDefaultAsync(x =>
            x.UserId == userId
            && x.Name == request.Name
            && x.Type == request.Type);

        if (entity is not null)
        {
            return entity.Id;
        }

        var account = _context.Account.Add(new()
        {
            UserId = userId,
            Name = request.Name,
            Type = request.Type,
            Currency = request.Currency!,
            SortKey = request.SortKey ?? await _context.Account.Where(a => a.UserId == userId).CountAsync()
        });
        await _context.SaveChangesAsync();

        return account.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task UpdateAccounts(string userId, IList<UpdateAccountRequest> request)
    {
        _logger.LogInformation($"Updating accounts for {userId}. Accounts: {string.Join(", ", request.Select(x => x.Id))}");

        foreach (var r in request)
        {
            var account = r.FromUpdateAccountRequest();
            _context.Attach(account);
            var entry = _context.Entry(account);

            if (r.Type is not null) entry.Property(a => a.Type).IsModified = true;
            if (r.Name is not null) entry.Property(a => a.Name).IsModified = true;
            if (r.Currency is not null) entry.Property(a => a.Currency).IsModified = true;
            if (r.SortKey is not null) entry.Property(a => a.SortKey).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddAccountEntry(string userId, AddAccountEntryRequest request)
    {
        _logger.LogInformation($"Adding account entry for account '{request.AccountId}' for user '{userId}'");

        var account = await _context.Account.FindAsync(request.AccountId);

        if (account is null || account.UserId != userId)
        {
            throw new EntityNotFoundException($"Account with id '{request.AccountId}' could not be found");
        }

        var entry = await _context.AccountEntry
            .Include(e => e.Account)
            .SingleOrDefaultAsync(x =>
                x.Date == request.Date
                && x.AccountId == request.AccountId
                && x.Account!.UserId == userId);

        if (entry is null)
        {
            _context.AccountEntry.Add(new()
            {
                AccountId = request.AccountId,
                Date = request.Date,
                Amount = request.Amount,
            });
        }
        else
        {
            entry.Amount = request.Amount;
        }

        await _context.SaveChangesAsync();
    }
}