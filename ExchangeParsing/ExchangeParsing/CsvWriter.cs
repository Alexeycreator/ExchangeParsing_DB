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
        csvBuilder.AppendLine($"{rate.NumberCode};{rate.LetterCode};{rate.Units};{rate.Currency};{rate.Rate}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }

    public void Write(string CSVFilePath, List<Stock> stocks)
    {
      StringBuilder csvBuilder = new StringBuilder();
      csvBuilder.AppendLine("NameStock;LastPrice;LastParcent");
      foreach (var stock in stocks)
      {
        csvBuilder.AppendLine($"{stock.Name};{stock.Price};{stock.Parcent}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }

    public void Write(string CSVFilePath, List<Bond> bonds)
    {
      StringBuilder csvBuilder = new StringBuilder();
      csvBuilder.AppendLine("SECURITY_TYPE;TYPE;SECID;SHORTNAME;NAME;REGNUMBER;PRIMARY_BOARDID;FACEVALUE;FACEUNIT;ISIN");
      foreach (var bond in bonds)
      {
        csvBuilder.AppendLine($"{bond.Security_type};{bond.Type};{bond.SecID};{bond.ShortName};{bond.FullName};{bond.Primary_boardID};{bond.FaceValue};{bond.FaceUnit};{bond.Isin}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }
  }
}
