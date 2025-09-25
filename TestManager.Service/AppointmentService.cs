using Microsoft.Data.SqlClient;
using TestManager.Service.Logging;
using TestManager.Domain.DTO;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Interfaces;
using TestManager.DataAccess.Helper;

namespace TestManager.Service
{
    public interface IAppointmentService
    {
        public Task<(List<AppointmentDto> Appointments, int TotalCount)> GetAppointmentsForRadiologyAsync(AppointmentFilterDto? filter = null);
        public Task<(List<UploaderAppointmentDto> Appointments, int TotalCount)> GetAppointmentsForUploaderAsync(AppointmentFilterDto filter);


        public Task<AppointmentDetailDto> GetAppointmentWithDetailsAsync(int Id);

        public Task<List<ProductAddDTO>> GetProductsToAdd(int appointmentId);

        public Task<int> AppointmentJoinProducts(AppointmentJoinProductsDTO appointmentJoinProducts);
        
        public Task<int> AppointmentAddProducts(AppointmentAddProductsDTO appointmentAddProducts, int appointmentId);

        public Task<int> AppointmentDeleteProducts(AppointmentDeleteProductsDTO appointmentDeleteProductsDTO);
    }

    public class AppointmentService(IAppointmentRepository appointmentRepository, 
        ITransactionItemRepository transactionItemRepository,
        IActivityLogRepository activityLogRepository,
        IUserContextService userContextService) : IAppointmentService
    {               
        public async Task<(List<AppointmentDto> Appointments, int TotalCount)> GetAppointmentsForRadiologyAsync(AppointmentFilterDto? filter = null)
        {
            var appointments = await appointmentRepository.GetAppointmentsForRadiologyAsync(filter);
            DomainEventLogger.LogDomainEvent("GetAppointment", new Dictionary<string, object>
            {
                {"action", "Get" },
            });

            return (appointments.Appointments, appointments.TotalCount);               
        }

        public async Task<(List<UploaderAppointmentDto> Appointments, int TotalCount)> GetAppointmentsForUploaderAsync(AppointmentFilterDto filter) 
        {
            var appointments = await appointmentRepository.GetAppointmentsForUploaderAsync(filter);
            DomainEventLogger.LogDomainEvent("GetAppointmentForUploader", new Dictionary<string, object>
            {
                {"action", "Get" },
            });

            return (appointments.Appointments, appointments.TotalCount);
        }

        public async Task<AppointmentDetailDto> GetAppointmentWithDetailsAsync(int Id)
        {
            var appointmentDetails = await appointmentRepository.GetAppointmentWithDetailsAsync(Id);

            //DomainEventLogger.LogDomainEvent("GetAppointmentDetails", new Dictionary<string, object>
            //{
            //    {"AppointmentId",Id },
            //    {"Action", "Get" },
            //});
            return appointmentDetails;
        }

        public async Task<List<ProductAddDTO>>GetProductsToAdd(int appointmentId)
        {
            var availableProducts = await appointmentRepository.GetProductsToAdd(appointmentId);

            //DomainEventLogger.LogDomainEvent("GetProductsForAppointment", new Dictionary<string, object>
            //{
            //    {"AppointmentId",appointmentId },
            //    {"Action", "Get" },
            //    {"Available Products", availableProducts.ToList() }
            //});
            return availableProducts;
        }

        public async Task<int> AppointmentJoinProducts(AppointmentJoinProductsDTO appointmentJoinProducts)
        {
            string productSummary = string.Empty;

            try
            {
                var result = await transactionItemRepository.TransactionItemJoinProducts(appointmentJoinProducts);

                if (result != null)
                {
                    productSummary = "Joined: " + string.Join(", ",
                    appointmentJoinProducts.TransactionItems.Select(p =>
                            $"Product: {p.ProductName} - Accession Number: {p.AccessionNumber}"));

                    DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

                    await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
                    {
                        ActivityDate = estDate,
                        SQLAction = "Update",
                        EntityTypeId = 3,
                        InstanceId = appointmentJoinProducts.AppointmentId,
                        EntityAction = productSummary,
                        UserEmail = userContextService.Email
                    });


                    DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentJoinProducts.AppointmentId },
                        { "Action", "Update" },
                        { "Joined Product(s)",productSummary }
                    });
                    return 1;
                }

                return 0;
            }
            catch (SqlException se)
            {
                DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentJoinProducts.AppointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)",  productSummary},
                        { "Result" , "Failed" },
                        { "Error", se.Message }
                    });
                return 0;
            }
            catch (Exception ex)
            {
                DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentJoinProducts.AppointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)", productSummary },
                        { "Result", "Failed" },
                        { "Error", ex.Message }
                    });
                return 0;
            }
        }

        public async Task<int> AppointmentAddProducts(AppointmentAddProductsDTO appointmentAddProducts, int appointmentId)
        {
            string addProductSummary = string.Empty;
            try
            {               
                var result = await transactionItemRepository.TransactionItemAddProducts(appointmentAddProducts);
                if(result != null)
                {
                    addProductSummary = "Added: " + string.Join(", ",
                        appointmentAddProducts.Products.Select(p =>
                                $"Product: {p.ProductName} - Accession Number: {p.AccessionNumber}"));

                    DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

                    await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
                    {
                        ActivityDate = estDate,
                        SQLAction = "Insert",
                        EntityTypeId = 3,
                        InstanceId = appointmentId,
                        EntityAction = addProductSummary,
                        UserEmail = userContextService.Email
                    });

                    DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)", addProductSummary },
                        { "Result" , "Success" }
                    });
                    return 1;
                }
                else
                    return 0;
            }
            catch(SqlException se)
            {
                DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)",  addProductSummary},
                        { "Result" , "Failed" },
                        { "Error", se.Message }
                    });
                return 0;
            }
            catch(Exception ex)
            {
                DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)", addProductSummary }, 
                        { "Result", "Failed" },
                        { "Error", ex.Message }
                    });
                return 0;
            }
        }

        public async Task<int> AppointmentDeleteProducts(AppointmentDeleteProductsDTO appointmentDeleteProductsDTO)
        {
            string productSummary = string.Empty;

            var  result = await transactionItemRepository.AppointmentDeleteProducts(appointmentDeleteProductsDTO);

            try
            {
                if(result != null)
                {
                    productSummary = "Deleted: " + string.Join(", ",
                        appointmentDeleteProductsDTO.TransactionItems.Select(p =>
                                $"Product: {p.ProductName} - Accession Number: {p.AccessionNumber}"));

                    DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

                    await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
                    {
                        ActivityDate = estDate,
                        SQLAction = "Delete",
                        EntityTypeId = 3,
                        InstanceId = appointmentDeleteProductsDTO.AppointmentId,
                        EntityAction = productSummary,
                        UserEmail = userContextService.Email
                    });

                    DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                    {
                        { "AppointmentId", appointmentDeleteProductsDTO.AppointmentId },
                        { "Action", "Delete" },
                        { "Deleted Product(s)", productSummary },
                    });
                    return 1;
                }
                else
                    return 0;
            }
            catch (SqlException se)
            {
                DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                {
                        { "AppointmentId", appointmentDeleteProductsDTO.AppointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)",  productSummary},
                        { "Result" , "Failed" },
                        { "Error", se.Message }
                    });
                return 0;
            }
            catch (Exception ex)
            {
                DomainEventLogger.LogDomainEvent("AppointmentUpdated", new Dictionary<string, object>
                {
                        { "AppointmentId", appointmentDeleteProductsDTO.AppointmentId },
                        { "Action", "Insert" },
                        { "Added Product(s)", productSummary },
                        { "Result", "Failed" },
                        { "Error", ex.Message }
                    });
                return 0;
            }
        }
    }
}
