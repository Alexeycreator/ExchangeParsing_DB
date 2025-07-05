using ExchangeParsing.DataBase.Tables;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.MoscowExchange.Models
{
  [Table("Stocks")]
  internal class Stock
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [JsonProperty("SECID")]
    [Column("NameStock")]
    public string Name { get; set; }

    [JsonProperty("LAST")]
    [Column("LastPrice")]
    public decimal Price { get; set; }

    [JsonProperty("LASTTOPREVPRICE")]
    [Column("LastPriceChangePercent")]
    public decimal Percent { get; set; }

    [Column("SecuritiePortfolio_Id")]
    public int SecuritiePortfolio_Id { get; set; }

    [ForeignKey("SecuritiePortfolio_Id")]
    [JsonIgnore]
    public virtual SecurityPortfolio SecurityPortfolio { get; set; }
  }
}
