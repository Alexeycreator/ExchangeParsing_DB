using ExchangeParsing.DataBase.Tables;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeParsing.MoscowExchange.Models
{
  [Table("Bonds")]
  internal class Bond
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [JsonProperty("SECURITY_TYPE")]
    [Column("SecurityType")]
    public string Security_type { get; set; }

    [JsonProperty("TYPE")]
    [Column("Type")]
    public string Type { get; set; }

    [JsonProperty("SECID")]
    [Column("SecID")]
    public string SecID { get; set; }

    [JsonProperty("SHORTNAME")]
    [Column("ShortName")]
    public string ShortName { get; set; }

    [JsonProperty("NAME")]
    [Column("FullName")]
    public string FullName { get; set; }

    [JsonProperty("REGNUMBER")]
    [Column("RegNumber")]
    public string RegNumber { get; set; }

    [JsonProperty("PRIMARY_BOARDID")]
    [Column("PrimaryBoardID")]
    public string Primary_boardID { get; set; }

    [JsonProperty("FACEVALUE")]
    [Column("FaceValue")]
    public string FaceValue { get; set; }

    [JsonProperty("FACEUNIT")]
    [Column("FaceUnit")]
    public string FaceUnit { get; set; }

    [JsonProperty("ISIN")]
    [Column("Isin")]
    public string Isin { get; set; }

    [Column("SecuritiePortfolio_Id")]
    public int SecuritiePortfolio_Id { get; set; }

    [ForeignKey("SecuritiePortfolio_Id")]
    [JsonIgnore]
    public virtual SecurityPortfolio SecurityPortfolio { get; set; }
  }
}
