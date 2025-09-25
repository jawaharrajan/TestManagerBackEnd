using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Service;
using TestManager.Service.ActivityLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Globalization;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology
{
    public class AppointmentsFunction(IAppointmentService appointmentService, ILogger<AppointmentsFunction> logger) : BaseFunction(logger)
    {       

        [Function("GetAppointments")]
        public async Task<IActionResult> GetAppointments([HttpTrigger(AuthorizationLevel.Function, "get", Route ="appointments/radiology")] 
        HttpRequest req, int? id)
        {
            //// EnrichLoggingFromRequest(req, enricher);

            var filters = new AppointmentFilterDto();
            if (req.QueryString.HasValue)
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
                filters = new AppointmentFilterDto
                {
                    StatusId = int.TryParse(query["statusId"], out var sid) ? sid : null,
                    AppointmentTypeIds = string.IsNullOrEmpty(query["appointmentTypes"]) ? [] : query["appointmentTypes"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(),
                    AppointmentDate = string.IsNullOrEmpty(query["appointmentDate"]) ? null : ParseFlexibleDate(query["appointmentDate"]),
                    LocationIds = string.IsNullOrEmpty(query["locationIds"]) ? [] : query["locationIds"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(),
                    SearchTerm = query["searchTerm"],
                    Page = int.TryParse(query["page"], out var p) ? p : 1,
                    PageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 20,
                    SortBy = query["sortBy"]
                };
            }

            logger.LogInformation("Fetching all Appointments");

            return await ExecutePagedAsync<AppointmentDto>(
                async () =>
                {
                    var (data, total) = await appointmentService.GetAppointmentsForRadiologyAsync(filters);
                    return (data,total);
                }, filters.Page, filters.PageSize, "Get All Appointments"
            );
        }


        [Function("GetAppointmentsForUploader")]
        public async Task<IActionResult> GetAppointmentsForUploader([HttpTrigger(AuthorizationLevel.Function, "get", Route ="appointments/uploader")]
        HttpRequest req, int? id)
        {
            var filters = new AppointmentFilterDto();
            if (req.QueryString.HasValue)
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
                filters = new AppointmentFilterDto
                {
                    StatusId = int.TryParse(query["statusId"], out var sid) ? sid : null,
                    AppointmentTypeIds = string.IsNullOrEmpty(query["appointmentTypes"]) ? [] : query["appointmentTypes"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(),
                    AppointmentDate = string.IsNullOrEmpty(query["appointmentDate"]) ? null : ParseFlexibleDate(query["appointmentDate"]),
                    LocationIds = string.IsNullOrEmpty(query["locationIds"]) ? [] : query["locationIds"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(),
                    SearchTerm = query["searchTerm"],
                    Page = int.TryParse(query["page"], out var p) ? p : 1,
                    PageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 20,
                    SortBy = query["sortBy"]
                };
            }

            logger.LogInformation("Fetching all Appointments");

            return await ExecutePagedAsync<UploaderAppointmentDto>(
                async () =>
                {
                    var (data, total) = await appointmentService.GetAppointmentsForUploaderAsync(filters);
                    return (data, total);
                }, filters.Page, filters.PageSize, "Get All Appointments"
            );
        }

        [Function("GetAppointmentById")]
        public async Task<IActionResult> GetAppointmentById([HttpTrigger(AuthorizationLevel.Function, "get", Route ="appointments/{id:int}")]
        HttpRequest req, int? id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            if (id.HasValue)
            {
                return await ExecuteSafeAsync(
                        async () =>
                        {
                            var appointmentDetail = await appointmentService.GetAppointmentWithDetailsAsync(id.Value) ??
                                throw new KeyNotFoundException($"Appointment details for Id: {id} Not found");
                            return appointmentDetail;
                        }, $"Get Appointment detail for {id}");
            }
            else
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Appointment Id is required.", false));
        }

        [Function("GetProductsForAppointment")]
        public async Task<IActionResult> GetAvailableProductsForAppointment([HttpTrigger(AuthorizationLevel.Function,
            "get", Route = "appointments/{Id}/products")] HttpRequest req, int Id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Fetching Products that can be added to an Appointment");

            return await ExecuteSafeAsync(
                async () =>
                {
                    try
                    {
                        var availableProducts = await appointmentService.GetProductsToAdd(Id) ??
                        throw new KeyNotFoundException($"Appointment Id: {Id} Not found");
                        return availableProducts;
                    }
                    catch (Exception e) 
                    {
                        throw;
                    }
                   
                    
                }, $"Get available products for Appointment: {Id}");
        }

        [Function("AppointmentJoinProducts")]
        public async Task<IActionResult> AppointmentJoinProducts([HttpTrigger(AuthorizationLevel.Function,
            "put", Route = "appointments/{Id}/transactionItem/join")] HttpRequest req, int Id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation($"Join Products under one accession Number for an Appointment with ID: {Id}");
            
            var appointmentJoinProductDTO = await req.ReadFromJsonAsync<AppointmentJoinProductsDTO>();

            if (appointmentJoinProductDTO is null || appointmentJoinProductDTO.AppointmentId <= 0 ||
                 appointmentJoinProductDTO.TransactionItems == null || appointmentJoinProductDTO.TransactionItems.Count == 0 )
            {                
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: AppointmentID and Products list are required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var result = await appointmentService.AppointmentJoinProducts(appointmentJoinProductDTO);
                    if (result == 0) throw new Exception($"Unable to deteremine Accession number for Products of AppointmentId {Id}");

                    return Id;
                }, $"Joined products for Appointment: {Id}");
        }

        [Function("AppointmentAddProducts")]
        public async Task<IActionResult> AppointmentAddProducts([HttpTrigger(AuthorizationLevel.Function,
            "post", Route = "appointments/{Id}/transactionItem/add")] HttpRequest req, int Id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation($"Adding Products (TransactionItems) to a Transaction for an Appointment with ID: {Id}");

            var data = await req.ReadFromJsonAsync<AppointmentAddProductsDTO>();

            if (data is null || data.TransactionId <= 0 || !data.Products.Any())
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: TransactionId and Product list are required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var result = await appointmentService.AppointmentAddProducts(data,Id);
                    if (result == 0) throw new Exception($"Unable to Add Products for AppointmentId {Id}");

                    return Id;
                }, $"Added products for Appointment: {Id}");
        }

        [Function("DeleteProducts")]
        public async Task<IActionResult> AppointmentDeleteProducts([HttpTrigger(AuthorizationLevel.Function,
            "delete", Route ="appointments/{Id}/transactionItem/delete")] HttpRequest req, int Id)
        {
            //// EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation($"Delete Products for a given Appointment with ID : {Id} ");

            var data = await req.ReadFromJsonAsync<AppointmentDeleteProductsDTO>();

            if (data is null || data.AppointmentId <= 0 || data.TransactionItems.Count == 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: TransactionId and Product list are required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var result = await appointmentService.AppointmentDeleteProducts(data);
                    if (result == 0) throw new Exception($"Unable to delete Products for AppointmentId {Id}");

                    return Id;
                }, $"Deleted products for Appointment with Id: {Id}"
            );
        }

        private static DateTime? ParseFlexibleDate(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            var formats = new[] { "yyyy-MM-dd", "yyyy-MMM-dd", "dd-MM-yyyy", "dd-MMM-yyyy", "MM-dd-yyyy" };
            return DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                ? dt
                : null;
        }
    }
}
