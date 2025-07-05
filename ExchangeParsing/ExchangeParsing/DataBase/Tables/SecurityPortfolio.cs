using ExchangeParsing.MoscowExchange.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.DataBase.Tables
{
  [Table("SecurityPortfolio")]
  internal class SecurityPortfolio
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Client_Id")]
    public int Client_Id { get; set; }

    [ForeignKey("Client_Id")]
    [JsonIgnore]
    public virtual Client Client { get; set; }

    [JsonIgnore]
    public virtual ICollection<Bond> Bonds { get; set; }

    [JsonIgnore]
    public virtual ICollection<Stock> Stocks { get; set; }

    [JsonIgnore]
    public virtual ICollection<Portfolio_Currency> Portfolio_Currencies { get; set; }

    [JsonIgnore]
    public virtual ICollection<HistoryPortfolio> HistoryPortfolios { get; set; }
  }
}
