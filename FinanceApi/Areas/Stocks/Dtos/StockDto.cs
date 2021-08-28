using System.ComponentModel.DataAnnotations;

namespace FinanceApi.Areas.Stocks.Dtos
{
    public class StockDto
    {
#pragma warning disable CS8618 // Consider declaring the property as nullable.
        [Required]
        public string Symbol { get; set; }

        public string Language { get; set; }
        public string Region { get; set; }
        public string QuoteType { get; set; }
        public string QuoteSourceName { get; set; }
        public bool Triggerable { get; set; }
        public string Currency { get; set; }
        public string MarketState { get; set; }
        public string ShortName { get; set; }
        public double FirstTradeDateMilliseconds { get; set; }
        public double FiftyTwoWeekLowChangePercent { get; set; }
        public double PriceHint { get; set; }
        public double PostMarketChangePercent { get; set; }
        public double PostMarketTime { get; set; }
        public double PostMarketPrice { get; set; }
        public double PostMarketChange { get; set; }
        public double RegularMarketChange { get; set; }
        public double RegularMarketChangePercent { get; set; }
        public double RegularMarketTime { get; set; }
        public double RegularMarketPrice { get; set; }
        public double RegularMarketDayHigh { get; set; }
        public string RegularMarketDayRange { get; set; }
        public double RegularMarketDayLow { get; set; }
        public double RegularMarketVolume { get; set; }
        public double RegularMarketPreviousClose { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double BidSize { get; set; }
        public double AskSize { get; set; }
        public string FullExchangeName { get; set; }
        public string FinancialCurrency { get; set; }
        public double RegularMarketOpen { get; set; }
        public double AverageDailyVolume3Month { get; set; }
        public double AverageDailyVolume10Day { get; set; }
        public double FiftyTwoWeekLowChange { get; set; }
        public double EpsTrailingTwelveMonths { get; set; }
        public double EpsForward { get; set; }
        public double EpsCurrentYear { get; set; }
        public double PriceEpsCurrentYear { get; set; }
        public double SharesOutstanding { get; set; }
        public double BookValue { get; set; }
        public double FiftyDayAverage { get; set; }
        public double FiftyDayAverageChange { get; set; }
        public double FiftyDayAverageChangePercent { get; set; }
        public double TwoHundredDayAverage { get; set; }
        public double TwoHundredDayAverageChange { get; set; }
        public double TwoHundredDayAverageChangePercent { get; set; }
        public double MarketCap { get; set; }
        public double ForwardPE { get; set; }
        public double PriceToBook { get; set; }
        public double SourceInterval { get; set; }
        public double ExchangeDataDelayedBy { get; set; }
        public string Exchange { get; set; }
        public string LongName { get; set; }
        public string MessageBoardId { get; set; }
        public string ExchangeTimezoneName { get; set; }
        public string ExchangeTimezoneShortName { get; set; }
        public double GmtOffSetMilliseconds { get; set; }
        public string Market { get; set; }
        public bool EsgPopulated { get; set; }
        public string FiftyTwoWeekRange { get; set; }
        public double FiftyTwoWeekHighChange { get; set; }
        public double FiftyTwoWeekHighChangePercent { get; set; }
        public double FiftyTwoWeekLow { get; set; }
        public double FiftyTwoWeekHigh { get; set; }
        public double DividendDate { get; set; }
        public double EarningsTimestamp { get; set; }
        public double EarningsTimestampStart { get; set; }
        public double EarningsTimestampEnd { get; set; }
        public double TrailingAnnualDividendRate { get; set; }
        public double TrailingPE { get; set; }
        public double TrailingAnnualDividendYield { get; set; }
        public string AverageAnalystRating { get; set; }
        public bool Tradeable { get; set; }
        public string DisplayName { get; set; }
    }
}
