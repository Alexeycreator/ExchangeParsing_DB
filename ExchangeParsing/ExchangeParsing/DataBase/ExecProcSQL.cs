using NLog;
using System;

namespace ExchangeParsing.DataBase
{
  internal sealed class ExecProcSQL
  {
    private DB_Connect _dbContext = new DB_Connect();
    private Logger _logger = LogManager.GetCurrentClassLogger();

    public void ExecProc()
    {
      _logger.Info("Выполнение процедуры пересчета позиций день-клиент-портфель");
      try
      {
        _dbContext.Database.ExecuteSqlCommand("EXEC proc_HistoryPortfolioExchange");
        _logger.Info("Процедура выполнена");
      }
      catch (Exception ex)
      {
        _logger.Error(ex.Message);
      }
    }
  }
}
