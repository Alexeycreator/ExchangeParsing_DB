using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using NLog;
using System.IO;
using HtmlAgilityPack;

namespace ExchangeParsing.CentralBank
{
  internal sealed class CurrencyCentralBank_Parser
  {
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private static string dateGetRate = DateTime.Now.ToShortDateString();
    private string _urlCentralBank;
    private readonly HttpClient _httpClient = new HttpClient();
    private CsvWriter csvWriter = new CsvWriter();
    private readonly string csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CentralBank");
    private List<CurrencyModel> _currencyModels;

    public CurrencyCentralBank_Parser()
    {
      if (!Directory.Exists(csvFilePath))
      {
        Directory.CreateDirectory(csvFilePath);
      }
      string fileName = $"Rate_{dateGetRate}.csv";
      csvFilePath = Path.Combine(csvFilePath, fileName);
      if (!File.Exists(csvFilePath))
      {
        File.Create(csvFilePath).Close();
      }
    }

    private List<CurrencyModel> GetRate()
    {
      _logger.Info("Процесс получения курса валют запущен");
      List<CurrencyModel> currencyModels = new List<CurrencyModel>();
      try
      {
        string urlCentralBank = $"https://www.cbr.ru/currency_base/daily/?UniDbQuery.Posted=True&UniDbQuery.To={dateGetRate}";
        _urlCentralBank = urlCentralBank;
        _logger.Info($"Подключение к данным по адресу: {_urlCentralBank}");
        var _httpResponseMessage = _httpClient.GetAsync(_urlCentralBank).Result;
        if (_httpResponseMessage.IsSuccessStatusCode)
        {
          _logger.Info($"Подключение прошло успешно. {_httpResponseMessage.StatusCode}");
          var _htmlResponse = _httpResponseMessage.Content.ReadAsStringAsync().Result;
          if (!string.IsNullOrEmpty(_htmlResponse))
          {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(_htmlResponse);
            var _container = document.GetElementbyId("content");
            if (_container != null)
            {
              var tableBody = document.GetElementbyId("content").ChildNodes.FindFirst("tbody").ChildNodes.Where(x => x.Name == "tr").Skip(1).ToArray();
              _logger.Info("Извлечение данных");
              try
              {
                foreach (var tableRow in tableBody)
                {
                  var _cellNumberCode = tableRow.SelectSingleNode(".//td[1]").InnerText;
                  var _cellLetterCode = tableRow.SelectSingleNode(".//td[2]").InnerText;
                  var _cellUnits = tableRow.SelectSingleNode(".//td[3]").InnerText;
                  var _cellCurrency = tableRow.SelectSingleNode(".//td[4]").InnerText;
                  var _cellRate = tableRow.SelectSingleNode(".//td[5]").InnerText;
                  currencyModels.Add(new CurrencyModel
                  {
                    NumberCode = _cellCurrency,
                    LetterCode = _cellLetterCode,
                    Units = _cellUnits,
                    Currency = _cellCurrency,
                    Rate = _cellRate
                  });
                }
                if (currencyModels != null)
                {
                  _logger.Info($"Данные успешно получены. Количество: {currencyModels.Count}");
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
            }
            else
            {
              throw new FormatException($"Контент данных пустой или не получилось корректно его получить");
            }
          }
          else
          {
            throw new FormatException($"Не удалось получить ответ от страницы");
          }
        }
        else
        {
          throw new FormatException($"Подключиться на получилось. {_httpResponseMessage.StatusCode}");
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

      return currencyModels;
    }

    public void CentralBankParser()
    {
      try
      {
        _currencyModels = GetRate();
        _logger.Info($"Запись в файл по пути: {csvFilePath}");
        csvWriter.Write(csvFilePath, _currencyModels);
        _logger.Info($"Данные записаны в файл. Количество {_currencyModels.Count} из {_currencyModels.Count}");
      }
      catch (Exception ex)
      {
        _logger.Error(ex.Message);
      }
    }
  }
}
