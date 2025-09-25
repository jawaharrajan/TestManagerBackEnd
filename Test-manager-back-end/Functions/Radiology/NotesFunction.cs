using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology;

public class NotesFunction(ILogger<NotesFunction> logger, INoteService noteService) : BaseFunction(logger)
{

    [Function("GetNotes")]
    public async Task<IActionResult> GetNotes([HttpTrigger(AuthorizationLevel.Function, "get", Route = "notes")] HttpRequest req)
    {
        logger.LogInformation($"Fetching Notes");
        var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
        if (!int.TryParse(query["entityTypeId"], out var entityTypeId)) 
        {
            return new BadRequestObjectResult(
               new ApiResponse<string>("Invalid parameter: entityTypeId", false));
        }
        if (!int.TryParse(query["instanceId"], out var instanceId))
        {
            return new BadRequestObjectResult(
               new ApiResponse<string>("Invalid parameter: instanceId.", false));
        }
        var notes = await noteService.GetNotes(entityTypeId, instanceId);
        return new OkObjectResult(notes);
    }

    [Function("AddNotes")]
    public async Task<IActionResult> AddAppointmentNotes([HttpTrigger(AuthorizationLevel.Function, "post", Route = "notes")] HttpRequest req)
    {
        var note = await req.ReadFromJsonAsync<NoteDTO>();
        if (note is null || note.InstanceID == 0 || note.Text == string.Empty /*|| !(note.UserID > 0)*/ )
        {
            logger.LogWarning("AddAppointmentNotes: received empty payload");            
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: InstanceID or Text or UserID cannot be null or empty.", false));
        }

        logger.LogInformation($"Add a new Note to a Appointment ID: {note.InstanceID}");

        return await ExecuteSafeAsync(
            async () =>
            {
                var result = await noteService.AddAppointmentNotesAsync(note);
                if (result == false) throw new Exception($"Unable to send new note to testclient API for Synchronization with TIPS");

                return result;
            }, $"Adding Notes via testclient API");
    }
}