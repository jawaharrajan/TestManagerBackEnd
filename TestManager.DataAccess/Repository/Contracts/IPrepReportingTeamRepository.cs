using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPrepReportingTeamRepository : IGenericRepository<Prep_ReportingTeam, int>
    {
        Task<IEnumerable<ReportingTeamDTO>> GetReportingTeamWithTemplateAndTemplateText();
        Task<List<ReportingTeamDTO>> GetReportingTeam();
        Task<ReportingTeamDTO> AddReportingTeam(ReportingTeamDTO reportingTeamDTO);
        Task<ReportingTeamDTO> UpdateReportingTeam(ReportingTeamDTO reportingTeamDTO);
        Task<bool> DeleteReportingTeam(int id);
    }
}
