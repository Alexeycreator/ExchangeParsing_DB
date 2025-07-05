using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.DataBase.Tables
{
  [Table("Address")]
  internal class Address
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("City")]
    public string City { get; set; }

    [Column("Street")]
    public string Street { get; set; }

    [Column("House")]
    public int House { get; set; }

    [Column("Apartment")]
    public int Apartment { get; set; }

    [JsonIgnore]
    public virtual ICollection<Client> Clients { get; set; }
  }
}
