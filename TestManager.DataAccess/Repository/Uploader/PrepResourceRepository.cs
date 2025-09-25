using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class PrepResourceRepository(ApplicationDbContext _) : GenericRepository<Prep_ResourceCategory, int>(_), IPrepResourceRepository
    {
        public async Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterialByResourceId(int resourceCategory)
        {
            var result = await (
                from rc in _context.Prep_ResourceCategory
                join em in _context.Prep_EducationMaterial
                    on rc.ConditionCategoryId equals em.ConditionCategoryId into emGroup
                where rc.ConditionCategoryId == resourceCategory
                select new PrepResourceDTO
                {
                    ConditionCategoryId = rc.ConditionCategoryId,
                    Description = rc.Description,
                    InActive = rc.InActive,
                    EducationMaterials = emGroup.Select(em => new PrepEducationMaterialDTO
                    {
                        EducationMaterialId = em.EducationMaterialId,
                        Description = em.Description,
                        Url = em.Url,
                        Path = em.Path,
                        ConditionCategoryId = em.ConditionCategoryId,
                        TypeId = em.TypeId
                    }).ToList()
                }).ToListAsync();

            return result;

        }

        public async Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterials()
        {
            var result = await (
                from rc in _context.Prep_ResourceCategory
                join em in _context.Prep_EducationMaterial
                    on rc.ConditionCategoryId equals em.ConditionCategoryId into emGroup
                select new PrepResourceDTO
                {
                    ConditionCategoryId = rc.ConditionCategoryId,
                    Description = rc.Description,
                    InActive = rc.InActive,
                    EducationMaterials = emGroup.Select(em => new PrepEducationMaterialDTO
                    {
                        EducationMaterialId = em.EducationMaterialId,
                        Description = em.Description,
                        Url = em.Url,
                        Path = em.Path,
                        ConditionCategoryId = em.ConditionCategoryId,
                        TypeId = em.TypeId
                    }).ToList()
                }).ToListAsync();

            return result;
        }
    }
}
