using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using ExchangeParsing.MoscowExchange.Models;

namespace ExchangeParsing.MoscowExchange
{
  internal sealed class Stock_Bonds_Parser
  {
    private readonly string csvFilePathStocks = Path.Combine(Directory.GetCurrentDirectory(), "MoscowExchange", "Stocks");
    private readonly string csvFilePathBonds = Path.Combine(Directory.GetCurrentDirectory(), "MoscowExchange", "Bonds");
    private string dateGetParsing = DateTime.Now.ToShortDateString();
    private Logger _logger = LogManager.GetCurrentClassLogger();
    private CsvWriter csvWriter = new CsvWriter();
    private readonly string _urlStock = $@"https://iss.moex.com/iss/engines/stock/markets/shares/boards/tqbr/securities.json?iss.only=marketdata&iss.meta=off&iss.json=extended&marketdata.columns=SECID%2CLAST%2CLASTTOPREVPRICE&sort_column=VALTODAY&sort_order=desc&first=18";
    private string[] _urlBonds = {
      $@"https://iss.moex.com/iss/emitters/484/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/269/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/3588/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/711/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/10763/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/770/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/1074/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/9848/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/739/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/10712/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/1399/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/15523/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/12604/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/1158/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/886/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/651/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/10761/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru",
      $@"https://iss.moex.com/iss/emitters/1242/securities.jsonp?iss.meta=off&iss.json=extended&callback=JSON_CALLBACK&lang=ru"
    };
    private string urlMoscowExchange = $@"https://www.moex.com/";
    private string urlCentralBank = $@"https://www.cbr.ru/";
    private int countStocks = 0;

    public Stock_Bonds_Parser()
    {
      if (!Directory.Exists(csvFilePathStocks))
      {
        Directory.CreateDirectory(csvFilePathStocks);
      }
      string fileName = $"Stock_{dateGetParsing}.csv";
      csvFilePathStocks = Path.Combine(csvFilePathStocks, fileName);
      if (!File.Exists(csvFilePathStocks))
      {
        File.Create(csvFilePathStocks).Close();
      }
      if (!Directory.Exists(csvFilePathBonds))
      {
        Directory.CreateDirectory(csvFilePathBonds);
      }
      string _fileName = $"Bond_{dateGetParsing}.csv";
      csvFilePathBonds = Path.Combine(csvFilePathBonds, _fileName);
      if (!File.Exists(csvFilePathBonds))
      {
        File.Create(csvFilePathBonds).Close();
      }
    }

    private List<Stock> GetStock()
    {
      _logger.Info("Запущен процесс получения акций");
      List<Stock> allStocks = new List<Stock>();
      try
      {
        using (HttpClient httpClient = new HttpClient())
        {
          _logger.Info($"Подключение к данным по адресу: {_urlStock}");
          var response = httpClient.GetStringAsync(_urlStock).GetAwaiter().GetResult();
          if (response == null)
          {

          }
          else
          {
            _logger.Info("Извлечение данных");
            var jsonArrayStock = JsonConvert.DeserializeObject<List<object>>(response);
            if (jsonArrayStock != null)
            {
              _logger.Info("Данные получены");
              var marketDataStock = jsonArrayStock[1].ToString();
              if (marketDataStock != null)
              {
                _logger.Info("Корректные данные для считывания");
                var stockData = JsonConvert.DeserializeObject<StockModel>(marketDataStock);
                if (stockData != null)
                {
                  _logger.Info("Успешная десериализация данных");
                  _logger.Info($"Запись в файл: {csvFilePathStocks}");
                  var stocks = stockData.StateStock.Select(s => new Stock
                  {
                    Name = s.Name,
                    Price = s.Price.ToString(),
                    Parcent = s.Parcent.ToString()
                  }).ToList();
                  csvWriter.Write(csvFilePathStocks, stocks);
                  _logger.Info($"Данные записаны: {stocks.Count} акций");
                  countStocks = stocks.Count;
                  allStocks.AddRange(stocks);
                }
                else
                {
                  throw new FormatException($"Данные {stockData} не удалось десериализовать");
                }
              }
              else
              {
                throw new FormatException($"Выбраны неверные данные json-файла. {marketDataStock}");
              }
            }
            else
            {
              throw new FormatException($"Данные пустые: {jsonArrayStock}");
            }
          }
        }
      }
      catch (FormatException ex)
      {
        _logger.Error(ex.Message);
      }
      catch (Exception ex)
      {
        _logger.Error(ex.Message);
      }
      Console.WriteLine($"Данные сайта {urlCentralBank} получены");
      return allStocks;
    }

    private List<Bond> GetBonds()
    {
      _logger.Info("Запущен процесс получения облигаций");
      List<Bond> allBonds = new List<Bond>();
      try
      {
        int countUrlBonds = _urlBonds.Count();
        int countDataBonds = 0;
        int _totalBonds = 0;
        if (countStocks == countUrlBonds)
        {
          using (HttpClient httpClient = new HttpClient())
          {

            foreach (var _url in _urlBonds)
            {
              _logger.Info($"Подключение к данным по адресу: {_url}");
              var response = httpClient.GetStringAsync(_url).GetAwaiter().GetResult();
              if (response == null)
              {
                var _response = httpClient.GetAsync(_url).GetAwaiter().GetResult();
                string content = _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new FormatException($"Ошибка подключения. {content}");
              }
              else
              {
                _logger.Info($"Подключение прошло успешно.");
                if (response.StartsWith("JSON_CALLBACK(") && response.EndsWith(")"))
                {
                  response = response.Substring("JSON_CALLBACK(".Length, response.Length - "JSON_CALLBACK(".Length - 1);
                }
                _logger.Info("Извлечение данных");
                var jsonArrayBonds = JsonConvert.DeserializeObject<List<object>>(response);
                if (jsonArrayBonds != null)
                {
                  _logger.Info("Данные получены");
                  var securitiesDataBonds = jsonArrayBonds[1].ToString();
                  if (securitiesDataBonds != null)
                  {
                    _logger.Info("Корректные данные для считывания");
                    var securitiesBonds = JsonConvert.DeserializeObject<BondModel>(securitiesDataBonds);
                    if (securitiesBonds != null)
                    {
                      _logger.Info("Успешная десериализация данных");
                      var _bonds = securitiesBonds.StateBonds.Select(b => new Bond
                      {
                        Security_type = b.Security_type,
                        Type = b.Type,
                        SecID = b.SecID,
                        ShortName = b.ShortName,
                        FullName = b.FullName,
                        RegNumber = b.RegNumber,
                        Primary_boardID = b.Primary_boardID,
                        FaceValue = b.FaceValue,
                        FaceUnit = b.FaceUnit,
                        Isin = b.Isin
                      }).ToList();
                      var totalBonds = securitiesBonds.SecuritiesCursorBonds[0].Total;
                      var filteredBonds = _bonds.Where(b => b.Security_type == "Биржевая облигация").ToList();
                      allBonds.AddRange(filteredBonds);
                      string nameBonds = _bonds.First().ShortName;
                      _logger.Info($"Добавлено {filteredBonds.Count} биржевых облигаций {nameBonds} из {totalBonds} других облигаций");
                      countDataBonds = allBonds.Count;
                      _totalBonds += Convert.ToInt32(totalBonds);
                    }
                    else
                    {
                      throw new FormatException($"Данные {securitiesBonds} не удалось десериализовать");
                    }
                  }
                  else
                  {
                    throw new FormatException($"Выбраны неверные данные json-файла. {securitiesDataBonds}");
                  }
                }
                else
                {
                  throw new FormatException($"Данные пустые: {jsonArrayBonds}");
                }
              }
            }
            _logger.Info($"Все данные успешно считаны. {countDataBonds} биржевых облигаций из {_totalBonds} разных типов облигаций");
            _logger.Info($"Запись в файл: {csvFilePathBonds}");
            csvWriter.Write(csvFilePathBonds, allBonds);
            _logger.Info($"Данные записаны.");
          }
        }
        else
        {
          throw new FormatException($"Количество акций изменилось ({countStocks}). Можно получить облигации лишь {countUrlBonds} акций.");
        }

      }
      catch (FormatException ex)
      {
        _logger.Error(ex.Message);
      }
      catch (Exception ex)
      {
        _logger.Error(ex.Message);
      }
      Console.WriteLine($"Данные сайта {urlMoscowExchange} получены");
      return allBonds;
    }

    private void GetInfoBonds()
    {
      //получение всех облигаций
      List<Stock> stocks = GetStock();
      List<Bond> bonds = GetBonds();
      int sberBondCount = bonds.Count();
      Console.WriteLine($"{sberBondCount}");
    }

    public void MoscowExchangeParser()
    {
      GetInfoBonds();
    }
  }
}
