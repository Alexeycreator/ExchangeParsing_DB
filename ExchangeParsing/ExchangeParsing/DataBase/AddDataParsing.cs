using ExchangeParsing.CentralBank;
using ExchangeParsing.MoscowExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Configuration;

namespace ExchangeParsing.DataBase
{
  internal sealed class AddDataParsing
  {
    private List<CurrencyModel> currency;
    private List<Stock> stocks;
    private List<Bond> bonds;
    private DB_Connect _dbContext = new DB_Connect();
    private Logger _logger = LogManager.GetCurrentClassLogger();

    public AddDataParsing(List<CurrencyModel> _currency)
    {
      currency = _currency;
    }

    public AddDataParsing(List<Stock> _stocks, List<Bond> _bonds)
    {
      stocks = _stocks;
      bonds = _bonds;
    }

    public void Push(string _typeExchange)
    {
      _logger.Info($"Добавление полученных данных в БД");
      try
      {
        bool isConnected = _dbContext.Database.Exists();
        if (isConnected)
        {
          _logger.Info($"Подключение к БД прошло успешно");
          switch (_typeExchange)
          {
            case "sb":
              if (stocks != null && stocks.Any())
              {
                foreach (var stock in stocks)
                {
                  var existingStocks = _dbContext.Stocks.FirstOrDefault(s => s.Name == stock.Name && s.Percent == stock.Percent);
                  if (existingStocks != null)
                  {
                    continue;
                  }
                  else
                  {
                    _dbContext.Stocks.Add(stock);
                  }
                }
                int savedCountStocks = _dbContext.SaveChanges();
                _logger.Info($"Добавлено {savedCountStocks} акций");
              }
              else
              {
                throw new FormatException($"Данные об акциях пустые и не могут быть добавлены в таблицу 'Stocks'.");
              }
              if (bonds != null && bonds.Any())
              {
                foreach (var bond in bonds)
                {
                  var existingBonds = _dbContext.Bonds.FirstOrDefault(b => b.SecID == bond.SecID && b.Isin == bond.Isin);
                  if (existingBonds != null)
                  {
                    continue;
                  }
                  else
                  {
                    _dbContext.Bonds.Add(bond);
                  }
                }
                int savedCountBonds = _dbContext.SaveChanges();
                _logger.Info($"Добавлено {savedCountBonds} облигаций");
              }
              else
              {
                throw new FormatException($"Данные об облигациях пустые и не могут быть добавлены в таблицу 'Bonds'.");
              }
              break;
            case "cb":
              if (currency != null && currency.Any())
              {
                _dbContext.Currencies.AddRange(currency);
                int savedCountCurrency = _dbContext.SaveChanges();
                _logger.Info($"Успешно добавлено {savedCountCurrency} валют");
              }
              else
              {
                throw new FormatException($"Данные о валютах пустые и не могут быть добавлены в таблицу 'Currency'.");
              }
              break;
            default: break;
          }
        }
        else
        {
          _logger.Info($"Подключиться к БД не удалось. Строка подключения: {ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString}");
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex.Message);
      }
    }
  }
}
