using Newtonsoft.Json;

namespace ExchangeParsing.MoscowExchange.Models
{
  internal sealed class CursorBond
  {
    [JsonProperty("TOTAL")]
    public string Total { get; set; }
  }
}