using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.DataBase.Tables
{
  [Table("Client")]
  internal class Client
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("SecondName")]
    public string SecondName { get; set; }

    [Column("FirstName")]
    public string FirstName { get; set; }

    [Column("SurName")]
    public string SurName { get; set; }

    [Column("Age")]
    public int Age { get; set; }

    [Column("NumberPhone")]
    [StringLength(11)]
    public string NumberPhone { get; set; }

    [Column("Address_Id")]
    public int Address_Id { get; set; }

    [ForeignKey("Address_Id")]
    public virtual Address Address { get; set; }

    [JsonIgnore]
    public virtual ICollection<HistoryPortfolio> HistoryPortfolios { get; set; }
  }
}
