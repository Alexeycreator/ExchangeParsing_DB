using ExchangeParsing.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using System.Linq;

namespace ExchangeParsing.DataBase
{
  internal sealed class InsertDataTables
  {
    private readonly string csvFilePath_Client = Path.Combine(Directory.GetCurrentDirectory(), "DataTable", "Client.csv");
    private readonly string csvFilePath_Address = Path.Combine(Directory.GetCurrentDirectory(), "DataTable", "Address.csv");
    private readonly string csvFilePath_SecurityPortfolio = Path.Combine(Directory.GetCurrentDirectory(), "DataTable", "SecurityPortfolio.csv");
    private readonly string csvFilePath_PortfolioCurrency = Path.Combine(Directory.GetCurrentDirectory(), "DataTable", "Portfolio_Currency.csv");
    private readonly string csvFilePath_ClientPortfolioShares = Path.Combine(Directory.GetCurrentDirectory(), "DataTable", "ClientPortfolioShares.csv");
    private DB_Connect _dbContext = new DB_Connect();
    private Logger _logger = LogManager.GetCurrentClassLogger();
    private List<Client> clients = new List<Client>();
    private List<Address> addresses = new List<Address>();
    private List<SecurityPortfolio> securityPortfolios = new List<SecurityPortfolio>();
    private List<Portfolio_Currency> portfolio_Currencies = new List<Portfolio_Currency>();
    private List<ClientPortfolioShares> clientPortfolioShares = new List<ClientPortfolioShares>();
    private int countPortfolio = 0;

    private void CheckInsert(string _typeExchange)
    {
      switch (_typeExchange)
      {
        case "cb":
          foreach (var adr in addresses)
          {
            Convert.ToInt32(adr.Apartment);
            Convert.ToInt32(adr.House);
            var existingAddress = _dbContext.Addresses.FirstOrDefault(a => a.Apartment == adr.Apartment && a.House == adr.House && a.City == adr.City && a.Street == adr.Street);
            if (existingAddress != null)
            {
              continue;
            }
            else
            {
              _dbContext.Addresses.Add(adr);
            }
          }
          _dbContext.SaveChanges();
          _logger.Info("Данные адресов добавлены в БД");

          foreach (var client in clients)
          {
            var existingClients = _dbContext.Clients.FirstOrDefault(c => c.Age == client.Age && c.SecondName == client.SecondName &&
                                  c.FirstName == client.FirstName && c.SurName == client.SurName && c.NumberPhone == client.NumberPhone);
            var existingClientsNumber = _dbContext.Clients.FirstOrDefault(c => c.NumberPhone == client.NumberPhone);
            if (existingClients != null)
            {
              continue;
            }
            else if (existingClientsNumber != null)
            {
              throw new FormatException("Данные клиента не могут быть добавлены. Данный номер телефона уже существует в БД");
            }
            else
            {
              _dbContext.Clients.Add(client);
            }
          }
          _dbContext.SaveChanges();
          _logger.Info("Данные клиентов добавлены в БД");
          break;
        case "sb":
          int _countPort = 0;
          foreach (var secPort in securityPortfolios)
          {
            _countPort++;
            var existingSecPortfolio = _dbContext.SecurityPortfolios.FirstOrDefault(sp => sp.Name == secPort.Name && sp.Price == secPort.Price &&
                                        sp.Currency == secPort.Currency && sp.TotalShares == secPort.TotalShares);
            if(existingSecPortfolio != null)
            {
              continue;
            }
            else
            {
              _dbContext.SecurityPortfolios.Add(secPort);
            }        
          }
          _dbContext.SaveChanges();
          countPortfolio = _countPort;
          _logger.Info("Данные о портфелях добавлены");

          foreach(var portCurr in portfolio_Currencies)
          {
            var existingPortCurr = _dbContext.Portfolio_Currencies.FirstOrDefault(pc => pc.Portfolio_Id == portCurr.Portfolio_Id && pc.Currency_Id == portCurr.Currency_Id);
            if (existingPortCurr != null)
            {
              continue;
            }
            else
            {
              _dbContext.Portfolio_Currencies.Add(portCurr);    
            }
          }
          _dbContext.SaveChanges();
          _logger.Info("Данные о связях между портфелями и валютами добавлены");

          foreach(var sharesClient in clientPortfolioShares)
          {
            var existingSharesClient = _dbContext.ClientPortfolioShares.FirstOrDefault(sc => sc.Client_Id == sharesClient.Client_Id && sc.Portfolio_Id == sharesClient.Portfolio_Id && sc.SharesOwned == sharesClient.SharesOwned);
            if(existingSharesClient != null)
            {
              continue;
            }
            else
            {
              _dbContext.ClientPortfolioShares.Add(sharesClient);
            }
          }
          _dbContext.SaveChanges();
          _logger.Info("Данные о долях клиентов добавлены");
          break;
        default: break;
      }

    }

    private void ReaderCsv(string _typeExchange)
    {
      switch (_typeExchange)
      {
        case "cb":

          #region StreamReader_Address

          StreamReader readerAddresses = new StreamReader(csvFilePath_Address);
          readerAddresses.ReadLine();
          _logger.Info($"Получение данных из файла {Path.GetFileName(csvFilePath_Address)}");
          while (!readerAddresses.EndOfStream)
          {
            var line = readerAddresses.ReadLine();
            var values = line.Split(';');
            try
            {
              Address address = new Address
              {
                City = values[0],
                Street = values[1],
                House = Convert.ToInt32(values[2]),
                Apartment = Convert.ToInt32(values[3])
              };
              addresses.Add(address);
            }
            catch (Exception ex)
            {
              _logger.Error($"Ошибка при обработке строки: {line}. {ex.Message}");
            }
          }

          #endregion

          #region StreamReader_Clients

          StreamReader readerClients = new StreamReader(csvFilePath_Client);
          readerClients.ReadLine();
          _logger.Info($"Получение данных из файла {Path.GetFileName(csvFilePath_Client)}");
          while (!readerClients.EndOfStream)
          {
            var line = readerClients.ReadLine();
            var values = line.Split(';');
            try
            {
              Client client = new Client
              {
                SecondName = values[0],
                FirstName = values[1],
                SurName = values[2],
                Age = Convert.ToInt32(values[3]),
                NumberPhone = values[4],
                Address_Id = Convert.ToInt32(values[5])
              };
              clients.Add(client);
            }
            catch (Exception ex)
            {
              _logger.Error($"Ошибка при обработке строки: {line}. {ex.Message}");
            }
          }

          #endregion

          break;
        case "sb":

          #region StreamReader_SecurityPortfolio

          StreamReader readerSecurityPortfolio = new StreamReader(csvFilePath_SecurityPortfolio);
          readerSecurityPortfolio.ReadLine();
          _logger.Info($"Получение данных из файла {Path.GetFileName(csvFilePath_SecurityPortfolio)}");
          while (!readerSecurityPortfolio.EndOfStream)
          {
            var line = readerSecurityPortfolio.ReadLine();
            var values = line.Split(';');
            try
            {
              SecurityPortfolio secPortfolio = new SecurityPortfolio
              {
                Name = values[0],
                Price = Convert.ToInt32(values[1]),
                Currency = values[2],
                TotalShares = Convert.ToInt32(values[3])
              };
              securityPortfolios.Add(secPortfolio);
            }
            catch (Exception ex)
            {
              _logger.Error($"Ошибка при обработке строки: {line}. {ex.Message}");
            }
          }

          #endregion

          #region StreamReader_PortfolioCurrency

          StreamReader readerPortfolioCurrency = new StreamReader(csvFilePath_PortfolioCurrency);
          readerPortfolioCurrency.ReadLine();
          _logger.Info($"Получение данных из файла {Path.GetFileName(csvFilePath_PortfolioCurrency)}");
          while (!readerPortfolioCurrency.EndOfStream)
          {
            var line = readerPortfolioCurrency.ReadLine();
            var values = line.Split(';');
            try
            {
              Portfolio_Currency portfolio_Cur = new Portfolio_Currency
              {
                Portfolio_Id = Convert.ToInt32(values[0]),
                Currency_Id = Convert.ToInt32(values[1])
              };
              portfolio_Currencies.Add(portfolio_Cur);
            }
            catch (Exception ex)
            {
              _logger.Error($"Ошибка при обработке строки: {line}. {ex.Message}");
            }
          }

          #endregion


          #region StreamReader_ClientPortfolioShares

          StreamReader readerClientPortfolioShares = new StreamReader(csvFilePath_ClientPortfolioShares);
          readerClientPortfolioShares.ReadLine();
          _logger.Info($"Получение данных из файла {Path.GetFileName(csvFilePath_ClientPortfolioShares)}");
          while (!readerClientPortfolioShares.EndOfStream)
          {
            var line = readerClientPortfolioShares.ReadLine();
            var values = line.Split(';');
            try
            {
              ClientPortfolioShares sharesClient = new ClientPortfolioShares
              {
                Client_Id = Convert.ToInt32(values[0]),
                Portfolio_Id = Convert.ToInt32(values[1]),
                SharesOwned = Convert.ToInt32(values[2])
              };
              clientPortfolioShares.Add(sharesClient);
            }
            catch (Exception ex)
            {
              _logger.Error($"Ошибка при обработке строки: {line}. {ex.Message}");
            }
          }

          #endregion

          break;
        default: break;
      }
    }

    public void Push(string _typeExchange)
    {
      try
      {
        ReaderCsv(_typeExchange);
        switch (_typeExchange)
        {
          case "cb":
            if (addresses == null)
            {
              throw new FormatException("Данные адресов пустые");
            }
            if (clients == null)
            {
              throw new FormatException("Данные клиентов пустые");
            }
            else
            {
              CheckInsert(_typeExchange);
            }
            break;
          case "sb":
            if (securityPortfolios == null)
            {
              throw new FormatException("Данные портфелей пустые");
            }
            else if (portfolio_Currencies == null)
            {
              throw new FormatException("Данные связи между портфелем и валютой пустые");
            }
            else if (securityPortfolios == null)
            {
              throw new FormatException("Данные долях клиентов пустые");
            }
            else
            {
              CheckInsert(_typeExchange);
            }
            break;
          default: break;
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

    public int CountPortfolio()
    {
      return countPortfolio;
    }
  }
}
