using System.Xml;
using System.Collections.Generic;

namespace FinanceApi.Areas.Stocks.Dtos
{
    class YahooResponse
    {
        public YahooResultResponse? QuoteResponse { get; set; }
    }

    class YahooResultResponse
    {
        public List<StockResponse>? Result { get; set; }

        public YahooError? Error { get; set; }
    }

    class YahooFinanceResponse
    {
        public YahooResultResponse? Finance { get; set; }
    }

    class YahooError
    {
#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public string Code { get; set; }

        public string Description { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.
    }
}