using TestManager.DataAccess.Helper;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class TransactionItemRepository(ApplicationDbContext _,
        ISequenceRepository sequenceRepository,
        IInvoiceRepository invoiceRepository,
        ICMS_SyncTrackingRepository cms_SyncTrackingRepository) : GenericRepository<TransactionItem, int>(_), ITransactionItemRepository
    {
        public async Task<AppointmentAddProductsDTO> TransactionItemAddProducts(AppointmentAddProductsDTO appointmentAddProductsDTO)
        {
                        
            decimal fillerValue  = 0.00m;
            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            try
            {
                // Get the Invoice ID for the Transaction
                int invoiceId = await invoiceRepository.GetInvoiceIdAsync(appointmentAddProductsDTO.TransactionId);

                // get Sequence Id's ahead of execution to avoid insert and update
                int N = appointmentAddProductsDTO.Products.Count;

                var sql = $"SELECT NEXT VALUE FOR dbo.seqTransItemId AS Id FROM (VALUES {string.Join(",", Enumerable.Repeat("(1)", N))}) AS x(n)";
                var sequenceValues = await _context.Database.SqlQueryRaw<long>(sql).ToListAsync();

                // 2. Map IDs to TransactionItems
                var transactionItems = appointmentAddProductsDTO.Products
                    .Select((p, index) =>
                    {
                        var seqId = sequenceValues[index];
                        var accession = appointmentAddProductsDTO.JoinProducts
                            ? sequenceValues.Min().ToString()
                            : seqId.ToString();

                        return new TransactionItem
                        {
                            TransactionItemId = (int)seqId, // manually set sequence
                            TransactionId = appointmentAddProductsDTO.TransactionId,
                            ProductId = p.ProductID,
                            Description = p.ProductName,
                            DateCreated = estDate,
                            LastUpdated = estDate,
                            Auxiliary = string.Empty,
                            AccpacItem = string.Empty,
                            OhipCode01 = string.Empty,
                            OhipCode02 = string.Empty,
                            OhipFacilityFee = fillerValue,
                            OhipProfessionalFee = fillerValue,
                            OhipTypeId = 1,
                            SubmittedToMOH = 0,
                            InvoiceID = invoiceId,
                            AccountId = null,
                            UserId = -1,
                            AccessionNo = accession
                        };
                    }).ToList();

                // 3. Insert in one call
                await _context.TransactionItem.AddRangeAsync(transactionItems);
                await _context.SaveChangesAsync();

                // Map accession numbers back by ProductID
                foreach (var dtoProduct in appointmentAddProductsDTO.Products)
                {
                    var match = transactionItems.FirstOrDefault(t => t.ProductId == dtoProduct.ProductID);
                    if (match != null && int.TryParse(match.AccessionNo, out int accessionNumber))
                    {
                        dtoProduct.AccessionNumber = accessionNumber;
                    }
                }
            }
            catch (SqlException se)
            {                
                throw;

            }
            catch (Exception ex)
            {
                throw;
            }

            return appointmentAddProductsDTO;
        }

        public async Task<AppointmentJoinProductsDTO> TransactionItemJoinProducts(AppointmentJoinProductsDTO appointmentJoinProductsDTO)
        {                
            // Step 1: Find the lowest AccessionNo
            var lowestAccessionNo = appointmentJoinProductsDTO.TransactionItems.Count != 0
                 ? appointmentJoinProductsDTO.TransactionItems.MinBy(ti => ti.TransactionItemId).AccessionNumber
                 : 0; 

            if (lowestAccessionNo == 0)
                return null;

            // Step 1a - check the CMS_tracking table to for real TIPS Id if CMSId is Stale
            List<int> realTipsIds = new List<int>();

            foreach (var item in appointmentJoinProductsDTO.TransactionItems)
            {
                realTipsIds.Add(await cms_SyncTrackingRepository.GetTIPSId("TransactionItem",item.TransactionItemId));
            }

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            // Step 2: Bulk update
            //await _context.TransactionItem
            //    .Where(ti => appointmentJoinProductsDTO.TransactionItems.Select(p => p.TransactionItemId).Contains(ti.TransactionItemId))
            //    .ExecuteUpdateAsync(setters => setters
            //        .SetProperty(ti => ti.AccessionNo, lowestAccessionNo.ToString())
            //        .SetProperty(ti => ti.LastUpdated, estDate)
            //    );


            await _context.TransactionItem
                .Where(ti => realTipsIds.Contains(ti.TransactionItemId))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(ti => ti.AccessionNo, lowestAccessionNo.ToString())
                    .SetProperty(ti => ti.LastUpdated, estDate)
                );

            // Map lowest accession number back to TransactionItem
            foreach (var dtoProduct in appointmentJoinProductsDTO.TransactionItems)
            {                
                dtoProduct.AccessionNumber = lowestAccessionNo;                
            }

            return appointmentJoinProductsDTO;
        }

        public async Task<AppointmentDeleteProductsDTO> AppointmentDeleteProducts(AppointmentDeleteProductsDTO appointmentDeleteProductsDTO)
        {
            if (appointmentDeleteProductsDTO.TransactionItems == null || appointmentDeleteProductsDTO.TransactionItems.Count == 0)
                return null;

            // Step 1 - check the CMS_tracking table to for real TIPS Id if CMSId is Stale
            List<int> realTipsIds = new List<int>();

            foreach (var item in appointmentDeleteProductsDTO.TransactionItems)
            {
                realTipsIds.Add(await cms_SyncTrackingRepository.GetTIPSId("TransactionItem", item.TransactionItemId));
            }

            //return await _context.TransactionItem
            //    .Where(ti => appointmentDeleteProductsDTO.TransactionItems.Contains(ti.TransactionItemId))
            //    .ExecuteUpdateAsync(setters => setters.SetProperty(ti => ti.IsDeleted, true));

            await _context.TransactionItem
            .Where(ti => realTipsIds.Contains(ti.TransactionItemId))
            .ExecuteDeleteAsync();

            return appointmentDeleteProductsDTO;
        }

        private async Task<int> GetTransactionItemIDFromSequence()
        {
            int id = await sequenceRepository.GetNextValueFromSequence("seqTransItemId");
            return id;
        }
    }
}
