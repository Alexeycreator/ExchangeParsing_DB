using ExchangeParsing.CentralBank;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExchangeParsing
{
  internal sealed class CsvWriter
  {
    public void Write(string CSVFilePath, List<CurrencyModel> wells)
    {
      StringBuilder csvBuilder = new StringBuilder();
      csvBuilder.AppendLine("Цифр. код;Букв. код;Единиц;Валюта;Курс");
      foreach (var well in wells)
      {
        csvBuilder.AppendLine($"{well.NumberCode};{well.LetterCode};{well.Units};{well.Currency};{well.Well}");
      }
      File.WriteAllText(CSVFilePath, csvBuilder.ToString());
    }
  }
}
