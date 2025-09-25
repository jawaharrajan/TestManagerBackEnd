using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class SequenceRepository(ApplicationDbContext _) : GenericRepository<SequenceValue, int>(_), ISequenceRepository
    {
        public async Task<int> GetNextValueFromSequence(string sequenceSPName)
        {
            // Define the output parameter
            var outputParameter = new SqlParameter("@OutputValue", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var sql = $"EXEC {sequenceSPName} @OutputValue OUTPUT;";

            int result = 0;
            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql, outputParameter);
                if(outputParameter.Value != DBNull.Value)
                {
                    result = (int)outputParameter.Value;
                }
            }
            catch (SqlException se)
            {
                throw new InvalidOperationException($"Stored procedure {sequenceSPName} did not return a value. Error: {se.Message}"); ;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Stored procedure {sequenceSPName} did not return a value. Error: {ex.Message}");
            }          
            return result;
        }
    }
}
