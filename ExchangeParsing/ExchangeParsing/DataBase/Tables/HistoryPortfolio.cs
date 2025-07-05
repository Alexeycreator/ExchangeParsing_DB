using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.DataBase.Tables
{
  [Table("HistoryPortfolio")]
  internal class HistoryPortfolio
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("DateSavePortfolioClient")]
    public DateTime DateSavePortfolioClient { get; set; }

    [Column("Client_Id")]
    public int Client_Id { get; set; }

    [Column("SecuritiePortfolio_Id")]
    public int SecuritiePortfolio_Id { get; set; }

    [Column("Details")]
    public string Details { get; set; }

    [ForeignKey("Client_Id")]
    public virtual Client Client { get; set; }

    [ForeignKey("SecuritiePortfolio_Id")]
    [JsonIgnore]
    public virtual SecurityPortfolio SecurityPortfolio { get; set; }
  }
}