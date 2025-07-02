using Newtonsoft.Json;

namespace ExchangeParsing.MoscowExchange.Models
{
  internal sealed class Stock
  {
    [JsonProperty("SECID")]
    public string Name { get; set; }

    [JsonProperty("LAST")]
    public string Price { get; set; }

    [JsonProperty("LASTTOPREVPRICE")]
    public string Parcent { get; set; }
  }
}
