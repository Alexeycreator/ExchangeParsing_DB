using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeParsing.DataBase.Tables
{
  internal class ClientPortfolioShares
  {
    [Key, Column(Order = 1)]
    [ForeignKey("Client")]
    public int Client_Id { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("SecurityPortfolio")]

    public int Portfolio_Id { get; set; }

    [Column("SharesOwned")]
    public int SharesOwned { get; set; }

    [ForeignKey("Client_Id")]
    [JsonIgnore]
    public virtual Client Client { get; set; }

    [ForeignKey("Portfolio_Id")]
    [JsonIgnore]
    public virtual SecurityPortfolio SecurityPortfolio { get; set; }
  }
}
