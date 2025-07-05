using ExchangeParsing.DataBase.Tables;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.CentralBank
{

  [Table("Currency")]
  internal class CurrencyModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Digital_code")]
    public string DigitalCode { get; set; }

    [Column("Letter_code")]
    public string LetterCode { get; set; }

    [Column("Units")]
    public int Units { get; set; }

    [Column("Currency")]
    public string Currency { get; set; }

    [Column("Rate")]
    public double Rate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Portfolio_Currency> Portfolio_Currencies { get; set; }
  }
}
