using ExchangeParsing.CentralBank;
using ExchangeParsing.MoscowExchange.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExchangeParsing
{
  internal sealed class CsvWriter
  {
    public void Write(string CSVFilePath, List<CurrencyModel> rates)
    {
      StringBuilder csvBuilder = new StringBuilder();
      csvBuilder.AppendLine("DigitalCode;LetterCode;Units;Currency;Rate");
      foreach (var rate in rates)
      {
        csvBuilder.AppendLine($"{rate.DigitalCode};{rate.LetterCode};{rate.Units};{rate.Currency};{rate.Rate}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }

    public void Write(string CSVFilePath, List<Stock> stocks)
    {
      StringBuilder csvBuilder = new StringBuilder();
      csvBuilder.AppendLine("NameStock;LastPrice;LastPriceChangePercent;SecuritiePortfolio_Id");
      foreach (var stock in stocks)
      {
        csvBuilder.AppendLine($"{stock.Name};{stock.Price};{stock.Percent};{stock.SecuritiePortfolio_Id}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }

    public void Write(string CSVFilePath, List<Bond> bonds)
    {
      StringBuilder csvBuilder = new StringBuilder();
      csvBuilder.AppendLine("SecurityType;Type;SecID;ShortName;FullName;RegNumber;PrimaryBoardID;FaceValue;FaceUnit;Isin;SecuritiePortfolio_Id");
      foreach (var bond in bonds)
      {
        csvBuilder.AppendLine($"{bond.Security_type};{bond.Type};{bond.SecID};{bond.ShortName};{bond.FullName};{bond.Primary_boardID};{bond.FaceValue};{bond.FaceUnit};{bond.Isin};{bond.SecuritiePortfolio_Id}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }
  }
}
