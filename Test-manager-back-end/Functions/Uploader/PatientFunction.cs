using TestManager.Domain.DTO.Uploader;
using TestManager.Functions.Common;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.Uploader
{
    public class PatientFunction(IPatientService patientService, ILogger<PatientFunction> logger) : BaseFunction(logger)
    {
        [Function("UploaderGetPatients")]
        public async Task<IActionResult> GetPatients([HttpTrigger(AuthorizationLevel.Function, "get", Route = "patients")] HttpRequest req)
        {
            var filters = new PatientFilterDTO();
            if (req.QueryString.HasValue)
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
                filters = new PatientFilterDTO
                {
                    SearchTerm = query["searchTerm"],
                    Page = int.TryParse(query["page"], out var p) ? p : 1,
                    PageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 10,
                    SortBy = query["sortBy"]
                };
            }

            logger.LogInformation("Fetching all Appointments");

            return await ExecutePagedAsync<PatientDTO>(
               async () =>
               {
                   var (data, total) = await patientService.GetPatientsAsync(filters);
                   return (data, total);
               },
               filters.Page, filters.PageSize, "Get All Patients with Appointments"
            );      
        }

        [Function("UploaderGetPatientById")]
        public async Task<IActionResult> GetPatientByIdAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "patients/{id}")] HttpRequest req, int? id) 
        {           
            if (id.HasValue)
            {
                return await ExecuteSafeAsync(
                        async () =>
                        {
                            var patient = await patientService.GetPatientByIdAsync(id.Value) ??
                                throw new KeyNotFoundException($"Patient for Id: {id} Not found");
                            return patient;
                        }, $"Get Patient detail for {id}"
                    );
            }
            else
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Patient Id is required.", false));
        }
    }
}
