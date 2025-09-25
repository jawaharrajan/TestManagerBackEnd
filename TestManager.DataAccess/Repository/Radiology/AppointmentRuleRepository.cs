using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class AppointmentRuleRepository(ApplicationDbContext _) : GenericRepository<AppointmentRule, int>(_), IAppointmentRuleRepository
    {
        public async Task<List<AppointmentRuleDTO>> GetAllAppointmentRules()
        {
            var query = from a in _context.AppointmentRules.AsNoTracking()
                        join at in _context.AppointmentType on a.AppointmentTypeID equals at.Id
                        join p in _context.Product on a.ProductID equals p.ProductID
                        orderby at.Name
                        where a.IsActive
                        select new AppointmentRuleDTO
                        {
                            Id = a.Id,
                            AppointmentTypeId = a.AppointmentTypeID,
                            AppointmentTypeName = at.Name,
                            ProductId = a.ProductID,
                            ProductName = p.Name,
                            AccessionNoFlag = a.AccessionNoFlag.GetValueOrDefault(),
                            AgeFrom = a.AgeFrom.GetValueOrDefault(),
                            AgeTo = a.AgeTo.GetValueOrDefault(),
                            AddDaysToAge = a.AddDaysToAge.GetValueOrDefault(),
                            IsChairman = a.IsChairman.GetValueOrDefault(),
                            IsMale = a.IsMale,
                            IsFemale = a.IsFemale,
                            IsActive = a.IsActive,
                            NonSmokerRole = a.NonSmokerRole.GetValueOrDefault(),                            
                            SmokerRole = a.SmokerRole.GetValueOrDefault(),
                            ToBeEvery = a.ToBeEvery.GetValueOrDefault()
                        };
            return await query.ToListAsync();
        }

        public async Task<AppointmentRuleDTO> AddAppointmentRule(AppointmentRuleDTO appointmentRuleDto)
        {
            AppointmentRule appointmentRule = new()
            {
                AccessionNoFlag = appointmentRuleDto.AccessionNoFlag,
                AddDaysToAge = appointmentRuleDto.AddDaysToAge,
                AgeFrom = appointmentRuleDto.AgeFrom,
                AgeTo = appointmentRuleDto.AgeTo,
                AppointmentTypeID = appointmentRuleDto.AppointmentTypeId,
                ProductID = appointmentRuleDto.ProductId,
                IsMale = appointmentRuleDto.IsMale,
                IsFemale = appointmentRuleDto.IsFemale,
                IsChairman = appointmentRuleDto.IsChairman,
                IsActive = true,                
                NonSmokerRole = appointmentRuleDto.NonSmokerRole,
                SmokerRole = appointmentRuleDto.SmokerRole,
                ToBeEvery = appointmentRuleDto.ToBeEvery
            };

            await AddAsync(appointmentRule);
            appointmentRuleDto.Id = appointmentRule.Id;
            return appointmentRuleDto;
        }

        public async Task<AppointmentRuleDTO> UpdateAppointmentRule(AppointmentRuleDTO appointmentRuleDto)
        {
            AppointmentRule? appointmentRule = _context.AppointmentRules.FirstOrDefault(
                a => a.Id == appointmentRuleDto.Id);

            if (appointmentRule == null) return null;
            //var productInfo = await (from p in _context.Product
            //                         join pt in _context.ProductType on p.ProductTypeId equals pt.ProductTypeId
            //                         where p.ProductID == appointmentRuleDto.ApptAutoProductID
            //                         select new
            //                         {
            //                             Name = p.Name,
            //                             ProductTypeName = pt.Name
            //                         }).FirstOrDefaultAsync();
           

            appointmentRule.AccessionNoFlag = appointmentRuleDto.AccessionNoFlag;
            appointmentRule.AddDaysToAge = appointmentRuleDto.AddDaysToAge;
            appointmentRule.AgeFrom = appointmentRuleDto.AgeFrom;
            appointmentRule.AgeTo = appointmentRuleDto.AgeTo;
            appointmentRule.AppointmentTypeID = appointmentRuleDto.AppointmentTypeId;
            appointmentRule.ProductID = appointmentRuleDto.ProductId;
            appointmentRule.IsMale = appointmentRuleDto.IsMale;
            appointmentRule.IsFemale = appointmentRuleDto.IsFemale;
            appointmentRule.IsChairman = appointmentRuleDto.IsChairman;            
            appointmentRule.NonSmokerRole = appointmentRuleDto.NonSmokerRole;
            appointmentRule.SmokerRole = appointmentRuleDto.SmokerRole;
            appointmentRule.ToBeEvery = appointmentRuleDto.ToBeEvery;

            await _context.SaveChangesAsync();
            return appointmentRuleDto;
        }

        public async Task<bool> DeleteAppointmentRule(int id)
        {
            AppointmentRule? appointmentRule = _context.AppointmentRules.FirstOrDefault(
               a => a.Id == id);

            if (appointmentRule == null) return false;

            appointmentRule.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
