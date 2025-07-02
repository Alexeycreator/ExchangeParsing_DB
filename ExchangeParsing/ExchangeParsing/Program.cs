using System;
using ExchangeParsing.CentralBank;
using ExchangeParsing.MoscowExchange;
using NLog;

namespace ExchangeParsing
{
  class Program
  {
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    static void Main(string[] args)
    {
      _logger.Info("Приложение запущено");
      Console.WriteLine($"Приложение 'ExchangeParsing' запущено");
      CurrencyCentralBank_Parser centralBank_Parser = new CurrencyCentralBank_Parser();
      centralBank_Parser.CentralBankParser();
      Stock_Bonds_Parser moscowExchange_Parser = new Stock_Bonds_Parser();
      moscowExchange_Parser.MoscowExchangeParser();
      Console.WriteLine("Приложение завершило работу");
      Console.ReadKey();
      _logger.Info("Приложение завершило работу");
    }
  }
}
