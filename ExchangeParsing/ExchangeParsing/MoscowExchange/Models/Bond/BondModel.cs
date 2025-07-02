using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeParsing.MoscowExchange.Models
{
  internal sealed class BondModel
  {
    [JsonProperty("securities")]
    public List<Bond> StateBonds { get; set; }

    [JsonProperty("securities.cursor")]
    public List<CursorBond> SecuritiesCursorBonds { get; set; }
  }
}
