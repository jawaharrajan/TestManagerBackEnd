using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
   public class PrepEducationMaterialRepository(ApplicationDbContext _) : GenericRepository<Prep_EducationMaterial, int>(_) ,IPrepEducationMaterialRepository
   {
        public async Task<List<PrepEducationMaterialDTO>> GetEducationMaterial()
        {            
            return await _context.Prep_EducationMaterial
                .OrderBy(x => x.Description)
                .Select(em => new PrepEducationMaterialDTO
                {
                    EducationMaterialId = em.EducationMaterialId,
                    Description = em.Description,
                    Url = em.Url,
                    Path = em.Path,
                    ConditionCategoryId = em.ConditionCategoryId,
                    TypeId = em.TypeId
                })
                .ToListAsync();
        }

        public async Task<PrepEducationMaterialDTO?> GetEducationMaterialById(int educationMaterialId)
        {
            return await _context.Prep_EducationMaterial
                .Select(em => new PrepEducationMaterialDTO
                {
                    EducationMaterialId = em.EducationMaterialId,
                    Description = em.Description,
                    //Pdf = em.Pdf,
                    Path = em.Path,
                    Url = em.Url,
                    ConditionCategoryId = em.ConditionCategoryId,
                    TypeId = em.TypeId
                })
                .FirstOrDefaultAsync(em => em.EducationMaterialId == educationMaterialId);
        }

        public async Task<PrepEducationMaterialDTO> AddEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO)
        {
            Prep_EducationMaterial em = new()
            {                
                Description = prepEducationMaterialDTO.Description,
                //Pdf = prepEducationMaterialDTO.Pdf,
                Path = prepEducationMaterialDTO.Path,
                Url = prepEducationMaterialDTO.Url,
                ConditionCategoryId = prepEducationMaterialDTO.ConditionCategoryId,
                TypeId = prepEducationMaterialDTO.TypeId
            };

            await AddAsync(em);
            prepEducationMaterialDTO.EducationMaterialId = em.EducationMaterialId;
            return prepEducationMaterialDTO;
        }
        
        public async Task<PrepEducationMaterialDTO> UpdateEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO)
        {
            Prep_EducationMaterial? em = await
                _context.Prep_EducationMaterial                
                .FirstOrDefaultAsync(em => em.EducationMaterialId == prepEducationMaterialDTO.EducationMaterialId);

            if (em == null) return null;
 
            em.Description = prepEducationMaterialDTO.Description;
            //if (prepEducationMaterialDTO?.Pdf != null && prepEducationMaterialDTO.Pdf.Length > 0)
            //{
            //    em.Pdf = prepEducationMaterialDTO.Pdf;
            //}
            
            em.Path = prepEducationMaterialDTO.Path;
            em.Url = prepEducationMaterialDTO.Url;
            em.ConditionCategoryId = prepEducationMaterialDTO.ConditionCategoryId;
            em.TypeId = prepEducationMaterialDTO.TypeId;

            await _context.SaveChangesAsync();
            return prepEducationMaterialDTO;
        }

        public async Task<bool> DeleteEducationMaterial(int id)
        {
            Prep_EducationMaterial? em = await _context.Prep_EducationMaterial.FirstOrDefaultAsync(em => em.EducationMaterialId == id);

            if (em == null) return false;

            _context.Remove(em);
            await _context.SaveChangesAsync();
            return true;
        }


   }
}
