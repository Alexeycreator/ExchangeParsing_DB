using ExchangeParsing.CentralBank;
using ExchangeParsing.MoscowExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Configuration;
using System.Data.Entity;

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
                int updatedCountStocks = 0;
                int addedCountStocks = 0;
                foreach (var stock in stocks)
                {
                  var existingStock = _dbContext.Stocks.FirstOrDefault(s => s.Name == stock.Name);
                  if (existingStock != null)
                  {
                    if (existingStock.Percent != stock.Percent || existingStock.Price != stock.Price || existingStock.SecuritiePortfolio_Id != stock.SecuritiePortfolio_Id)
                    {
                      existingStock.Percent = stock.Percent;
                      existingStock.Price = stock.Price;
                      existingStock.SecuritiePortfolio_Id = stock.SecuritiePortfolio_Id;
                      _dbContext.Entry(existingStock).State = EntityState.Modified;
                      updatedCountStocks++;
                    }
                  }
                  else
                  {
                    _dbContext.Stocks.Add(stock);
                    addedCountStocks++;
                  }
                }
                int savedCountStocks = _dbContext.SaveChanges();
                _logger.Info($"Обработано акций: {savedCountStocks} (добавлено: {addedCountStocks}, обновлено: {updatedCountStocks})");
              }

              else
              {
                throw new FormatException($"Данные об акциях пустые и не могут быть добавлены в таблицу 'Stocks'.");
              }
              if (bonds != null && bonds.Any())
              {
                int updatedCountBonds = 0;
                int addedCountBonds = 0;
                foreach (var bond in bonds)
                {
                  var existingBonds = _dbContext.Bonds.FirstOrDefault(b => b.SecID == bond.SecID && b.Isin == bond.Isin);
                  if (existingBonds != null)
                  {
                    if (existingBonds.ShortName != bond.ShortName || existingBonds.FullName != bond.FullName || existingBonds.RegNumber != bond.RegNumber || existingBonds.FaceUnit != bond.FaceUnit || existingBonds.SecuritiePortfolio_Id != bond.SecuritiePortfolio_Id ||
                        existingBonds.Primary_boardID != bond.Primary_boardID || existingBonds.Security_type != bond.Security_type || existingBonds.Type != bond.Type || existingBonds.FaceValue != bond.FaceValue)
                    {
                      existingBonds.Security_type = bond.Security_type;
                      existingBonds.Type = bond.Type;
                      existingBonds.ShortName = bond.ShortName;
                      existingBonds.FullName = bond.FullName;
                      existingBonds.RegNumber = bond.RegNumber;
                      existingBonds.Primary_boardID = bond.Primary_boardID;
                      existingBonds.FaceValue = bond.FaceValue;
                      existingBonds.FaceUnit = bond.FaceUnit;
                      existingBonds.SecuritiePortfolio_Id = bond.SecuritiePortfolio_Id;
                      _dbContext.Entry(existingBonds).State = EntityState.Modified;
                      updatedCountBonds++;
                    }
                  }
                  else
                  {
                    _dbContext.Bonds.Add(bond);
                    addedCountBonds++;
                  }
                }
                int savedCountBonds = _dbContext.SaveChanges();
                _logger.Info($"Обработано облигаций: {savedCountBonds} (добавлено: {addedCountBonds}, обновлено: {updatedCountBonds})");
              }
              else
              {
                throw new FormatException($"Данные об облигациях пустые и не могут быть добавлены в таблицу 'Bonds'.");
              }
              break;
            case "cb":
              if (currency != null && currency.Any())
              {
                int updatedCountCurrency = 0;
                int addedCountCurrency = 0;
                foreach (var rate in currency)
                {
                  var existingCurrency = _dbContext.Currencies.FirstOrDefault(c => c.DigitalCode == rate.DigitalCode);
                  if (existingCurrency != null)
                  {
                    if (existingCurrency.Rate != rate.Rate)
                    {
                      existingCurrency.Rate = rate.Rate;
                      updatedCountCurrency++;
                    }
                  }
                  else
                  {
                    _dbContext.Currencies.Add(rate);
                    addedCountCurrency++;
                  }
                }
                int savedCountCurrency = _dbContext.SaveChanges();
                _logger.Info($"Обработано валют: {savedCountCurrency} (добавлено: {addedCountCurrency}, обновлено: {updatedCountCurrency})");
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
