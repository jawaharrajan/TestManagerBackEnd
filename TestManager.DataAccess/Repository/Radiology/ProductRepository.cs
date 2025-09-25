using TestManager.DataAccess.Helper;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;


namespace TestManager.DataAccess.Repository.Radiology
{
    public class ProductRepository(ApplicationDbContext _,
        ICMS_SyncTrackingRepository cms_SyncTrackingRepository) : GenericRepository<Product, int>(_), IProductRepository
    {      
        public async Task<(List<ProductDTO> Products, int TotalCount)> GetProductsAsync(ProductFilterDto? filter = null)
        {
            var query = from P in _context.Product
                           join PT in _context.ProductType on P.ProductTypeId equals PT.ProductTypeId
                           join DM in _context.DICOMModality on P.ModalityId equals DM.DICOMModalityId
                           join ES in _context.EntityStatus on P.ProductID equals ES.InstanceId                           
                           where ES.EntityTypeId == 40 && ES.StatusId == 2700
                           select new ProductDTO
                           {
                               ProductID = P.ProductID,
                               Name = P.Name,
                               ProductTypeId = P.ProductTypeId,
                               ProductType = PT.Name,
                               ModalityId = P.ModalityId,
                               ModalityCode = DM.ModalityCode,
                               OhipCode01 = P.OhipCode01,
                               OhipTypeId = P.OhipTypeId,
                               ProductGroupId = P.ProductGroupId,
                               ProductSubgroupId = P.ProductSubgroupId,
                               OhipCode02 = P.OhipCode02,
                               OhipFacilityFee = P.OhipFacilityFee,
                               OhipProfessionalFee = P.OhipProfessionalFee
                               
                           };

            #region - check for Filters
            if (filter != null)
            {
                if (filter.DICOMModalityId.HasValue)
                {                  
                    query = query.Where(dm => dm.ModalityId == filter.DICOMModalityId);
                }
                if (filter.ProductTypeId.HasValue)
                {
                    query = query.Where(pt => pt.ProductTypeId == filter.ProductTypeId);
                }

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(p =>
                        p.Name.Contains(filter.SearchTerm) ||
                        p.OhipCode01.Contains(filter.SearchTerm));
                }

            }
            #endregion

            var totalCount = query.Count();

            var result = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (result, totalCount);
        }

        public async Task<List<ProductAddDTO>> GetProductstoAdd(int appointmentId)
        {
            var productsNotInCurrent = await
                    _context.Product
                        .Where(p => !_context.TransactionItem
                        .Any(ti => ti.ProductId == p.ProductID && ti.Transaction.InstanceId == appointmentId))
                            .Select(p => new ProductAddDTO
                            {
                                ProductId = p.ProductID,
                                Name = p.Name ?? "Unknown",
                                ModialityId = p.ModalityId ?? 0
                            })
                    .ToListAsync();

            return productsNotInCurrent;
        }

        public async Task<ProductDTO> AddProduct(ProductDTO productDTO)
        {
            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            Product product = new()
            {
                BaseProductId = productDTO.BaseProductId,

                ProductTypeId = productDTO.ProductTypeId,

                ProductGroupId = productDTO.ProductGroupId,

                AccpacItem = productDTO.AccpacItem,

                Name = productDTO.Name,

                AppointmentTypeId = productDTO.AppointmentTypeId,

                CancelCutOff = productDTO.CancelCutOff,

                NewPriceEffectiveDate = estDate,

                DiscountTimeFrame = productDTO.DiscountTimeFrame,

                DiscountAmount = productDTO.DiscountAmount,

                AccountId = productDTO.AccountId,

                AccountPays = false,
                
                AccountPaysCancellation = false,

                FamilyCoverage = false,

                OhipCode01 = productDTO.OhipCode01,

                OhipCode02 = productDTO.OhipCode02,

                OhipTypeId = productDTO.OhipTypeId,

                OhipFacilityFee = productDTO.OhipFacilityFee,

                ReminderEmailTemplateId = productDTO.ReminderEmailTemplateId,

                ReminderEmailAttachment = productDTO.ReminderEmailAttachment,

                PrepPackTemplateId = productDTO.PrepPackTemplateId,

                ConfirmationEmailTemplateId = productDTO.ConfirmationEmailTemplateId,

                ConfirmationEmailAttachment = productDTO.ConfirmationEmailAttachment,

                DateCreated = estDate,

                LastUpdated = estDate,

                ShowDiagnosticFields = false,

                ShowReferringDoctor = false,

                IsShowCHA5YearsPrice = productDTO.IsShowCHA5YearsPrice,

                ModalityId = productDTO.ModalityId,

                ProductSubgroupId = productDTO.ProductSubgroupId,

                OhipProfessionalFee = productDTO.OhipProfessionalFee
            };

            await AddAsync(product);
            await _context.SaveChangesAsync();

            productDTO.ProductID = product.ProductID;

            EntityStatus es = new()
            {
                InstanceId = product.ProductID,
                EntityTypeId = 40,
                StatusId = 2700,
                CustomDate = estDate
            };

            await _context.EntityStatus.AddAsync(es);
            await _context.SaveChangesAsync();

            return productDTO;
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO productDTO)
        {

            //Check CMS tracking table if Product record is processed in TIPS or not
            int tipsId = await cms_SyncTrackingRepository.GetTIPSId("Product", productDTO.ProductID);

            Product? product = _context.Product.FirstOrDefault(
               p => p.ProductID == tipsId
               );

            if (product == null) return null;

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            product.BaseProductId = productDTO.BaseProductId;

            product.ProductTypeId = productDTO.ProductTypeId;

            product.ProductGroupId = productDTO.ProductGroupId;

            product.AccpacItem = productDTO.AccpacItem;

            product.Name = productDTO.Name;

            product.AppointmentTypeId = productDTO.AppointmentTypeId;

            product.CancelCutOff = productDTO.CancelCutOff;

            //product.NewPriceEffectiveDate = productDTO.NewPriceEffectiveDate;

            product.DiscountTimeFrame = productDTO.DiscountTimeFrame;

            product.DiscountAmount = productDTO.DiscountAmount;

            product.AccountId = productDTO.AccountId;

            //product.AccountPays = productDTO.AccountPays;

            //product.AccountPaysCancellation = productDTO.AccountPaysCancellation;

            //product.FamilyCoverage = productDTO.FamilyCoverage;

            product.OhipCode01 = productDTO.OhipCode01;

            product.OhipCode02 = productDTO.OhipCode02;

            product.OhipTypeId = productDTO.OhipTypeId;

            product.OhipFacilityFee = productDTO.OhipFacilityFee;

            product.ReminderEmailTemplateId = productDTO.ReminderEmailTemplateId;

            product.ReminderEmailAttachment = productDTO.ReminderEmailAttachment;

            product.PrepPackTemplateId = productDTO.PrepPackTemplateId;

            product.ConfirmationEmailTemplateId = productDTO.ConfirmationEmailTemplateId;

            product.ConfirmationEmailAttachment = productDTO.ConfirmationEmailAttachment;

            //product.DateCreated = productDTO.DateCreated;

            product.LastUpdated = estDate;

            //product.ShowDiagnosticFields = productDTO.ShowDiagnosticFields;

            //product.ShowReferringDoctor = productDTO.ShowReferringDoctor;

            product.IsShowCHA5YearsPrice = productDTO.IsShowCHA5YearsPrice;

            product.ModalityId = productDTO.ModalityId;

            product.ProductSubgroupId = productDTO.ProductGroupId;

            product.OhipProfessionalFee = productDTO.OhipProfessionalFee;

            await _context.SaveChangesAsync();
            return productDTO;
        }

        public async Task<bool> DeleteProduct(int Id)
        {
            //Check CMS tracking table if Product record is processed in TIPS or not
            int tipsId = await cms_SyncTrackingRepository.GetTIPSId("Product", Id);

            Product? product = _context.Product                
                .FirstOrDefault(d => d.ProductID == tipsId);

            if (product == null) return false;


            var entityStatus = await (
                from ES in _context.EntityStatus
                where ES.EntityTypeId == 40 && ES.InstanceId == Id
                select ES).FirstOrDefaultAsync();

            entityStatus.StatusId = 2701;

            _context.EntityStatus.Update(entityStatus);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
