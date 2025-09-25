using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service.Uploader
{
    public interface IPrepReportingTeamService
    {
        Task<IEnumerable<ReportingTeamDTO>> GetReportingTeamWithTemplateAndTemplateText();
        Task<List<ReportingTeamDTO>> GetReportingTeam();
        Task<ReportingTeamDTO> AddReportingTeam(ReportingTeamDTO reportingTeamDTO);
        Task<ReportingTeamDTO> UpdateReportingTeam(ReportingTeamDTO reportingTeamDTO);
        Task<bool> DeleteReportingTeam(int id);
    }
    public class PrepReportingTeamService(IPrepReportingTeamRepository prepReportingTeamRepository) : IPrepReportingTeamService
    {
        public async Task<IEnumerable<ReportingTeamDTO>>GetReportingTeamWithTemplateAndTemplateText()
        {
            return await prepReportingTeamRepository.GetReportingTeamWithTemplateAndTemplateText();
        }

        public async Task<List<ReportingTeamDTO>> GetReportingTeam()
        {
            return await prepReportingTeamRepository.GetReportingTeam();
        }

        public async Task<ReportingTeamDTO> AddReportingTeam(ReportingTeamDTO reportingTeamDTO)
        {
            return await prepReportingTeamRepository.AddReportingTeam(reportingTeamDTO);
        }

        public async Task<ReportingTeamDTO> UpdateReportingTeam(ReportingTeamDTO reportingTeamDTO)
        {
            return await prepReportingTeamRepository.UpdateReportingTeam(reportingTeamDTO);
        }

        public async Task<bool> DeleteReportingTeam(int id)
        {
            return await prepReportingTeamRepository.DeleteReportingTeam(id);
        }
    }
}
