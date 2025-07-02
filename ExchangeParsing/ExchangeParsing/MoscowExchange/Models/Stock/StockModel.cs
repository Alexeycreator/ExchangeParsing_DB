using ExchangeParsing.MoscowExchange.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExchangeParsing.MoscowExchange
{
  internal sealed class StockModel
  {
    [JsonProperty("marketdata")]
    public List<Stock> StateStock { get; set; }
  }
}
