using TestManager.DataAccess.Repository.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class ApplyAppointmentRules(ApplicationDbContext context, ILogger<ApplyAppointmentRules> logger) : IApplyAppointmetRules
    {
        public async Task<bool> RunAppointmentRules()
        {
            try
            {
                using var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "EXEC dbo.ApplyAppointmentRules";

                using var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        logger.LogInformation($"Processed AppointmentId: {(reader.GetInt32(0))} successfully.");
                    }
                }
                else
                {
                    logger.LogInformation($"No Appointments found that need Appointment Rules to be applied.");
                }

                return true;
            }
            catch(SqlException se)
            {
                logger.LogError($"SQL Error Number: {se.Number}; SQL Error Code: {se.ErrorCode}; SQL Line Number: {se.LineNumber}; SQL Data: {se.Data}");
            }
            catch(Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }
            return false;
        }
    }
}
