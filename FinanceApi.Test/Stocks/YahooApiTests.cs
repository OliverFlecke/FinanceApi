using System.Text.Json;
using FinanceApi.Areas.Stocks.Dtos;
using FluentAssertions;
using Xunit;

namespace FinanceApi.Test
{
    public class YahooApiTests
    {
        [Fact]
        public void Deserialize_Error_Test()
        {
            // Arrange
            var content = "{\"quoteResponse\":{\"result\":[],\"error\": { \"code\": \"Some code\", \"description\": \"Some description\" }}}";

            // Act
            var obj = JsonSerializer.Deserialize<YahooResponse>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            // Assert
            obj!.QuoteResponse!.Error.Should().BeEquivalentTo(new YahooError()
            {
                Code = "Some code",
                Description = "Some description",
            });
        }

        [Fact]
        public void Deserialize_AAPL_Test()
        {
            // Arrange
            var content = "{\"quoteResponse\":{\"result\":[{\"language\":\"en-US\",\"region\":\"US\",\"quoteType\":\"EQUITY\",\"quoteSourceName\":\"Nasdaq Real Time Price\",\"triggerable\":true,\"currency\":\"USD\",\"regularMarketVolume\":54200035,\"bid\":148.48,\"shortName\":\"Apple Inc.\",\"marketState\":\"CLOSED\",\"exchange\":\"NMS\",\"longName\":\"Apple Inc.\",\"messageBoardId\":\"finmb_24937\",\"exchangeTimezoneName\":\"America/New_York\",\"exchangeTimezoneShortName\":\"EDT\",\"gmtOffSetMilliseconds\":-14400000,\"market\":\"us_market\",\"esgPopulated\":false,\"firstTradeDateMilliseconds\":345479400000,\"priceHint\":2,\"postMarketChangePercent\":-0.0874865,\"postMarketTime\":1630108796,\"postMarketPrice\":148.47,\"postMarketChange\":-0.130005,\"regularMarketChange\":1.0600128,\"regularMarketChangePercent\":0.71845794,\"regularMarketTime\":1630094403,\"regularMarketPrice\":148.6,\"regularMarketDayHigh\":148.75,\"regularMarketDayRange\":\"146.83 - 148.75\",\"regularMarketDayLow\":146.83,\"regularMarketPreviousClose\":147.54,\"ask\":148.52,\"bidSize\":11,\"askSize\":10,\"fullExchangeName\":\"NasdaqGS\",\"financialCurrency\":\"USD\",\"regularMarketOpen\":147.48,\"averageDailyVolume3Month\":76253310,\"averageDailyVolume10Day\":63160237,\"fiftyTwoWeekLowChange\":45.500008,\"fiftyTwoWeekLowChangePercent\":0.4413192,\"fiftyTwoWeekRange\":\"103.1 - 151.68\",\"fiftyTwoWeekHighChange\":-3.0799866,\"fiftyTwoWeekHighChangePercent\":-0.02030582,\"fiftyTwoWeekLow\":103.1,\"fiftyTwoWeekHigh\":151.68,\"dividendDate\":1628726400,\"earningsTimestamp\":1627403400,\"earningsTimestampStart\":1635332340,\"earningsTimestampEnd\":1635768000,\"trailingAnnualDividendRate\":0.835,\"trailingPE\":29.091623,\"trailingAnnualDividendYield\":0.0056594824,\"epsTrailingTwelveMonths\":5.108,\"epsForward\":5.67,\"epsCurrentYear\":5.58,\"priceEpsCurrentYear\":26.630825,\"sharesOutstanding\":16530199552,\"bookValue\":3.882,\"fiftyDayAverage\":147.10638,\"fiftyDayAverageChange\":1.4936218,\"fiftyDayAverageChangePercent\":0.010153345,\"twoHundredDayAverage\":133.241,\"twoHundredDayAverageChange\":15.359009,\"twoHundredDayAverageChangePercent\":0.115272395,\"marketCap\":2456387846144,\"forwardPE\":26.208113,\"priceToBook\":38.27924,\"sourceInterval\":15,\"exchangeDataDelayedBy\":0,\"averageAnalystRating\":\"1.9 - Buy\",\"tradeable\":false,\"displayName\":\"Apple\",\"symbol\":\"AAPL\"}],\"error\":null}}";

            // Act
            var obj = JsonSerializer.Deserialize<YahooResponse>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            // Assert
            obj!.QuoteResponse.Should().NotBeNull();
        }
    }
}