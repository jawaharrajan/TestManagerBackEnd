using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestManager.Service.EventHubservices
{
    public interface ITableProcessor
    {
        bool CanHandle(string tableName);
        Task ProcessAsync(List<JsonElement> jsonElements);
    }
}
