using System;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace VLTPAuth
{
  public class OracleService : IOracleService
  {
    private readonly ILogger<OracleService> _logger;

    public OracleService(ILogger<OracleService> logger)
    {
        _logger = logger;
    }

    public void InsertRow(string userId, string ssn)
    {
        _logger.LogInformation("[OracleService][InsertRow] => BEGIN ...");

        //Create a connection to Oracle			
        string connString = "User Id=hr;Password=hr;Data Source=localhost:1521/orclpdb.home;";

        _logger.LogInformation("[OracleService][InsertRow] => Begin connString:");
        _logger.LogInformation(connString);
        _logger.LogInformation("[OracleService][InsertRow] => End connString:");
    
        OracleConnection conn = null;
        try
        {
            using (conn = new OracleConnection(connString))
            {
                conn.Open();

                OracleParameter userIdParam = new OracleParameter();
                userIdParam.OracleDbType = OracleDbType.Varchar2;
                userIdParam.Value = userId;

                OracleParameter ssnParam = new OracleParameter();
                ssnParam.OracleDbType = OracleDbType.Varchar2;
                ssnParam.Value = ssn;    

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO HR.VLTP_AUTH(USER_ID, SSN) VALUES(:1, :2)";

                cmd.Parameters.Add(userIdParam);
                cmd.Parameters.Add(ssnParam);

                _logger.LogInformation("[OracleService][InsertRow] => Calling cmd.ExecuteNonQuery() ...");
                int execResult = cmd.ExecuteNonQuery();
                 _logger.LogInformation("[OracleService][InsertRow] => cmd.ExecuteNonQuery() returned: " + execResult);
            }
        }
        catch (Exception ex)
        {
           _logger.LogInformation("********************************************************************************");
           _logger.LogInformation("[OracleService][InsertRow] => Begin Exception:");
           _logger.LogInformation(ex.Message);
           _logger.LogInformation(ex.StackTrace);
           _logger.LogInformation("[OracleService][InsertRow] => End Exception:");
           _logger.LogInformation("********************************************************************************");           
        }
        finally
        {
            if ( conn != null)
            {
                conn.Close();
            }
        }
        _logger.LogInformation("[OracleService][InsertRow] => ... END");

    }
  }
}