using FinanceApi.Areas.Account.Models;

namespace FinanceApi.Areas.Account.Dtos;

public record AccountWithEntriesResponse : AccountResponse
{
    public AccountWithEntriesResponse(
        Guid Id,
        string Name,
        AccountType Type,
        string Currency,
        IEnumerable<EntryResponse>? entries = null)
        : base(Id, Name, Type, Currency)
    {
        Entries = entries ?? Array.Empty<EntryResponse>();
    }

    public IEnumerable<EntryResponse> Entries { get; set; }

}
