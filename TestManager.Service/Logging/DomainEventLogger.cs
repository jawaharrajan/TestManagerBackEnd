using Serilog;

namespace TestManager.Service.Logging
{
   public static class DomainEventLogger
    {
        public static void LogAppointmentUpdated(string username, string appointmentId, string productName, string productId)
        {
            Log.ForContext("Username", username)
               .ForContext("AppointmentId", appointmentId)
               .ForContext("Action", "insert")
               .ForContext("ProductAdded", productName)
               .ForContext("ProductIdAdded", productId)
               .Information("AppointmentUpdated");
        }

        public static void LogDomainEvent(string eventName, Dictionary<string, object> data, Exception? exception = null)
        {
            ILogger logger = Log.Logger;
            logger = Log.ForContext("IsDomainEvent", true);

            foreach (var kvp in data)
            {
                logger = logger.ForContext(kvp.Key, kvp.Value, destructureObjects: true);
            }

            if (exception != null)
            {
                logger.Error(exception, eventName);
            }
            else
            {
                logger.Information(eventName);
            }
        }
    }
}
