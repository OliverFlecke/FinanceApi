using System.ComponentModel.DataAnnotations;

namespace FinanceApi.Areas.Stocks.Dtos
{
    public class StockDto
    {
        [Required]
#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public string Symbol { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.
    }
}
