using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO;
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
    public class PrepTemplateRepository(ApplicationDbContext _) : GenericRepository<PrepTemplate, int>(_), IPrepTemplateRepository
    {
        public async Task<List<PrepTemplateDTO>> GetPrepTemplatesAysnc()
        {
            var result = from pt in _context.PrepTemplate
                              select new PrepTemplateDTO
                              {
                                  TemplateId = pt.TemplateId,
                                  Description = pt.Description,
                                  Subject1 = pt.Subject1,
                                  Subject2 = pt.Subject2,
                                  Text = pt.Text,
                                  TypeId = pt.TypeId
                              };

            return await result.ToListAsync();
        }

        public async Task<PrepTemplateDTO> GetPrepTemplateByIdAysnc(int id)
        {
            var result = await (from pt in _context.PrepTemplate
                        where pt.TemplateId == id
                        select new PrepTemplateDTO
                        {
                            TemplateId = pt.TemplateId,
                            Description = pt.Description,
                            Text = pt.Text,
                            TypeId = pt.TypeId,
                            Subject1 = pt.Subject1,
                            Subject2 = pt.Subject2
                        }).FirstOrDefaultAsync();

            return result ?? new PrepTemplateDTO();
        }

        public async Task<PrepTemplateDTO> AddPrepTemplateAsync(PrepTemplateDTO prepTemplateDTO)
        {
            PrepTemplate pt = new PrepTemplate
            {                
                Description = prepTemplateDTO.Description,
                Text = prepTemplateDTO.Text,
                Subject1 = prepTemplateDTO.Subject1,
                Subject2 = prepTemplateDTO.Subject2,
                TypeId = prepTemplateDTO.TypeId,
                Inactive = false
                
            };

            await AddAsync(pt);
            prepTemplateDTO.TemplateId = pt.TemplateId;
            return prepTemplateDTO;
        }

        public async Task<PrepTemplateDTO?>UpdatePrepTemplateAsync(PrepTemplateDTO prepTemplateDTO)
        {

            PrepTemplate? pt = await _context.PrepTemplate.FirstOrDefaultAsync(P => P.TemplateId == prepTemplateDTO.TemplateId);

            if (pt == null) return null;

            pt.Description = prepTemplateDTO.Description;
            pt.Text = prepTemplateDTO.Text;
            pt.TypeId = prepTemplateDTO.TypeId;
            pt.Subject1 = prepTemplateDTO.Subject1;
            pt.Subject2 = prepTemplateDTO.Subject2;

            await _context.SaveChangesAsync();
            return prepTemplateDTO;
        }

        public async Task<bool> DeletePrepTemplateAsync(int id)
        {
            PrepTemplate? pt = _context.PrepTemplate.FirstOrDefault(
                p => p.TemplateId == id);

            if (pt == null) return false;

            _context.Remove(pt);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
