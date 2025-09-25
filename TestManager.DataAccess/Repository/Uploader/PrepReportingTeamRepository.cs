using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class PrepReportingTeamRepository(ApplicationDbContext _) :GenericRepository<Prep_ReportingTeam,int>(_), IPrepReportingTeamRepository
    {
        public async Task<IEnumerable<ReportingTeamDTO>> GetReportingTeamWithTemplateAndTemplateText()
        {
            //var reportingTeam = await _context.Prep_ReportingTeam
            //        .Include(rt => rt.Prep_ReportingTeamTemplate)                   
            //        .ThenInclude(rtt => rtt.PrepTemplate)
            //        .AsNoTracking()
            //        .ToListAsync();

            var result = await _context.Prep_ReportingTeam
                .Select(rt => new ReportingTeamDTO
                {
                    ReportingTeamId = rt.ReportingTeamId,
                    EmailName = rt.EmailName,
                    Email = rt.Email,
                    ReportingTeamName = rt.ReportingTeamName,
                    PrepReportingTeamTemplate = rt.TeamTemplates
                        .Select(rtt => new PrepReportingTeamTemplateDTO
                        {
                            ReportingTeamTemplateId = rtt.ReportingTeamTemplateId,
                            ReportingTeamId = rtt.ReportingTeamId,
                            TemplateId = rtt.TemplateId,
                        }).ToList(),
                    PrepReportingTeamUsers = rt.TeamUsers
                        .Select(rtu => new PrepReportingTeamUserDTO
                        {
                            ReportingTeamId = rtu.ReportingTeamId,
                            UserId = rtu.UserId                            
                        }).ToList()
                }).ToListAsync();


            return result;
        }

        public async Task<List<ReportingTeamDTO>> GetReportingTeam()
        {
            var query = from r in _context.Prep_ReportingTeam
                        select new ReportingTeamDTO
                        {
                            ReportingTeamId = r.ReportingTeamId,
                            ReportingTeamName = r.ReportingTeamName,
                            Email = r.Email,
                            EmailName = r.EmailName
                        };

            return await query.ToListAsync();
        }

        public async Task<ReportingTeamDTO> AddReportingTeam(ReportingTeamDTO reportingTeamDTO)
        {
            Prep_ReportingTeam rt = new()
            {
                ReportingTeamName = reportingTeamDTO.ReportingTeamName,
                Email = reportingTeamDTO.Email,
                EmailName = reportingTeamDTO.EmailName
            };

            await AddAsync(rt);
            reportingTeamDTO.ReportingTeamId = rt.ReportingTeamId;
            return reportingTeamDTO;
        }

        public async Task<ReportingTeamDTO> UpdateReportingTeam(ReportingTeamDTO reportingTeamDTO)
        {
            Prep_ReportingTeam? rt = await _context.Prep_ReportingTeam
                .Include(x => x.TeamTemplates)
                .Include(x => x.TeamUsers)
                .FirstOrDefaultAsync(rt => rt.ReportingTeamId == reportingTeamDTO.ReportingTeamId);

            if (rt == null) return null;

            rt.ReportingTeamName = reportingTeamDTO.ReportingTeamName;
            rt.EmailName = reportingTeamDTO.EmailName;
            rt.Email = reportingTeamDTO.Email;

            var existingUsers = rt.TeamUsers.ToDictionary(x => x.UserId);
            var existingTemplates = rt.TeamTemplates.ToDictionary(x => x.TemplateId);
            var inputUsers = reportingTeamDTO.PrepReportingTeamUsers.Select(x => x.UserId).ToHashSet();
            var inputTemplates = reportingTeamDTO.PrepReportingTeamTemplate.Select(x => x.TemplateId).ToHashSet();

            foreach (var user in existingUsers.Values) 
            {
                if (!inputUsers.Contains(user.UserId))
                {
                    rt.TeamUsers.Remove(user);
                }
            }
            foreach (var template in existingTemplates.Values)
            {
                if (!inputTemplates.Contains(template.TemplateId))
                {
                    rt.TeamTemplates.Remove(template);
                }
            }

            foreach (var userId in inputUsers)
            {
                if (!existingUsers.ContainsKey(userId))
                {
                    rt.TeamUsers.Add(new Prep_ReportingTeamUser() 
                    { 
                        UserId = userId,
                        ReportingTeam = rt,
                        ReportingTeamId = rt.ReportingTeamId
                    });
                }
            }
            foreach (var templateId in inputTemplates)
            {
                if (!existingTemplates.ContainsKey(templateId))
                {
                    rt.TeamTemplates.Add(new Prep_ReportingTeamTemplate() 
                    {
                        TemplateId = templateId,
                        Prep_ReportingTeam = rt,
                        ReportingTeamId = rt.ReportingTeamId
                    });
                }
            }



            await _context.SaveChangesAsync();
            return reportingTeamDTO;
        }

        public async Task<bool> DeleteReportingTeam(int id)
        {
            Prep_ReportingTeam? rt = await _context.Prep_ReportingTeam.FirstOrDefaultAsync(rt => rt.ReportingTeamId == id);

            if (rt == null) return false;

            _context.Remove(rt);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
