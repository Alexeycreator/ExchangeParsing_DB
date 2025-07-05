using ExchangeParsing.CentralBank;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.DataBase.Tables
{
  [Table("Portfolio_Currency")]
  internal class Portfolio_Currency
  {
    [Key, Column(Order = 1)]
    [ForeignKey("SecurityPortfolio")]
    public int Portfolio_Id { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("CurrencyModel")]

    public int Currency_Id { get; set; }

    [Column("Amount")]
    public int Amount { get; set; }

    [ForeignKey("Portfolio_Id")]
    [JsonIgnore]
    public virtual SecurityPortfolio SecurityPortfolio { get; set; }

    [ForeignKey("Currency_Id")]
    [JsonIgnore]
    public virtual CurrencyModel CurrencyModel { get; set; }
  }
}
