using Newtonsoft.Json;

namespace ExchangeParsing.MoscowExchange.Models
{
  internal sealed class Bond
  {
    [JsonProperty("SECURITY_TYPE")]
    public string Security_type { get; set; }

    [JsonProperty("TYPE")]
    public string Type { get; set; }

    [JsonProperty("SECID")]
    public string SecID { get; set; }

    [JsonProperty("SHORTNAME")]
    public string ShortName { get; set; }

    [JsonProperty("NAME")]
    public string FullName { get; set; }

    [JsonProperty("REGNUMBER")]
    public string RegNumber { get; set; }

    [JsonProperty("FACEUNIT")]
    public string FaceUnit { get; set; }

    [JsonProperty("ISIN")]
    public string Isin { get; set; }

    [JsonProperty("PRIMARY_BOARDID")]
    public string Primary_boardID { get; set; }

    [JsonProperty("FACEVALUE")]
    public string FaceValue { get; set; }
  }
}
