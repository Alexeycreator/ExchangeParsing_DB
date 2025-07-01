using System;
using ExchangeParsing.CentralBank;
using NLog;

namespace ExchangeParsing
{
  class Program
  {
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    static void Main(string[] args)
    {
      _logger.Info("Приложение запущено");
      CurrencyCentralBank_Parser bank_Parser = new CurrencyCentralBank_Parser();
      bank_Parser.CentralBankParser();
      Console.WriteLine("Приложение завершило работу");
      Console.ReadKey();
      _logger.Info("Приложение завершило работу");
    }
  }
}
