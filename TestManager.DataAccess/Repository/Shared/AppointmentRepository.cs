using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.DataAccess.Sort;

namespace TestManager.DataAccess.Repository.Shared
{
    public class AppointmentRepository(ApplicationDbContext context) : GenericRepository<Appointment, int>(context), IAppointmentRepository
    {
        public async Task<AppointmentDetailDto> GetAppointmentWithDetailsAsync(int Id)
        {
            #region - flat return
            var flatData = await (
                from a in context.Appointment
                join at in context.AppointmentType on a.AppointmentTypeId equals at.Id
                join t in context.Transaction on a.Id equals t.InstanceId
                join ti in context.TransactionItem on t.TransactionId equals ti.TransactionId into transactionItemGroup
                from ti in transactionItemGroup.DefaultIfEmpty()
                join p in context.Patient on a.PatientId equals (int?)p.PatientId into patientGroup
                from p in patientGroup.DefaultIfEmpty()
                join es in context.EntityStatus on a.Id equals es.InstanceId
                join s in context.Status on es.StatusId equals s.StatusId
                join d in context.Doctor on a.DoctorId equals d.Id
                where t.EntityTypeId == 3 && es.EntityTypeId == 3 && a.Id == Id               
                select new
                {
                    a.Id,
                    a.Date,
                    a.ArrivalNotes,
                    PID = p.PatientId,
                    PhotoFile = p.Photofile,
                    p.FirstName,
                    p.LastName,
                    at.Name,
                    at.Code,
                    DoctorFullName = d.LastName + ", " + d.FirstName,
                    es.StatusId,
                    StatusName = s.Name,
                    t.TransactionId,
                    t.Reason,
                    TransactionItem = ti != null && ti.Product.ModalityId > 0 && ti.Product.BaseProductId == null
                    ?  new
                    {
                        ti.TransactionItemId,
                        ti.Description,
                        ti.ProductId,
                        ti.Auxiliary,
                        ti.UserId,
                        ti.AccessionNo,
                        ti.DateCreated,
                        ti.LastUpdated
                    } : null
                }
            ).ToListAsync();

            var first = flatData.FirstOrDefault();

            if (first == null)
                return null;            

            var result = new AppointmentDetailDto
            {
                Id = first.Id,
                Date = first.Date,
                ArrivalNotes = first.ArrivalNotes,
                PID = first.PID,
                PhotoFile = first.PhotoFile,
                PatientFirstName = first.FirstName,
                PatientLastName = first.LastName,
                AppointmentType = first.Name,
                AppointmentTypeCode = first.Code,
                DoctorFullName = first.DoctorFullName,
                StatusId = first.StatusId,
                StatusName = first.StatusName,
                Transactions = new List<TransactionDto>
                {
                    new TransactionDto
                    {
                        TransactionId = first.TransactionId,
                        Reason = first.Reason ?? string.Empty,
                        TransactionItems = flatData
                            .Where(x => x.TransactionItem != null)
                            .Select(x => new TransactionItemDto
                            {
                                TransactionItemId = x.TransactionItem.TransactionItemId,
                                Description = x.TransactionItem.Description ?? string.Empty,
                                ProductId = x.TransactionItem.ProductId ?? 0,
                                Auxiliary = x.TransactionItem.Auxiliary ?? string.Empty,
                                UserId = x.TransactionItem.UserId ?? 0,
                                AccessionNo = x.TransactionItem.AccessionNo,
                                DateCreated = x.TransactionItem.DateCreated ?? DateTime.MinValue,
                                LastUpdated = x.TransactionItem.LastUpdated ?? DateTime.MinValue
                            })
                            .ToList()
                    }
                }
            };
            #endregion

            return result;

        }

        public async Task<(List<AppointmentDto> Appointments, int TotalCount)> GetAppointmentsForRadiologyAsync(AppointmentFilterDto filter)
        {
            var patients = context.Patient.AsQueryable();
            #region - check for Filters
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                filter.SearchTerm = filter.SearchTerm.Trim();
                //Check for ClientID (PatientID)
                if (int.TryParse(filter.SearchTerm, out int patientId))
                {
                    patients = patients.Where(a => a.PatientId == patientId);
                }
                else
                {
                    if (filter.SearchTerm.Contains(','))
                    {
                        var criteria = filter.SearchTerm.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        var lastNameSearch = criteria.ElementAtOrDefault(0)?.Trim();
                        var firstNameSearch = criteria.ElementAtOrDefault(1)?.Trim();
                        if (!string.IsNullOrEmpty(lastNameSearch))
                        {
                            patients = patients.Where(p => p.LastName.Contains(lastNameSearch));
                        }
                        if (!string.IsNullOrEmpty(firstNameSearch)) 
                        {
                            patients = patients.Where(p => p.FirstName.Contains(firstNameSearch));
                        }
                    }
                    else
                    {
                        patients = patients.Where(a =>
                        a.FirstName.Contains(filter.SearchTerm) ||
                        a.LastName.Contains(filter.SearchTerm));
                    }
                }
            }
            var patientsQuery = patients.Select(x => new 
            {
                x.PatientId,
                x.Photofile,
                x.FirstName,
                x.LastName,
                x.Healthcard,
                x.HealthcardVersion,
                x.Birthdate,
                x.Gender,
                x.HasMytestclient
            });
            #endregion

            var query = from a in context.Appointment.AsNoTracking()
                        join at in context.AppointmentType on a.AppointmentTypeId equals at.Id
                        join t in context.Transaction on a.Id equals t.InstanceId
                        join p in patientsQuery on a.PatientId equals p.PatientId
                        join es in context.EntityStatus on a.Id equals es.InstanceId
                        join s in context.Status on es.StatusId equals s.StatusId
                        join d in context.Doctor on a.DoctorId equals d.Id
                        where t.EntityTypeId == 3 && es.EntityTypeId == 3
                        where filter.AppointmentDate != null ? a.Date.Date == filter.AppointmentDate : true
                        where filter.StatusId.HasValue ? es.StatusId == filter.StatusId : es.StatusId != 1308
                        where filter.AppointmentTypeIds.Any() ? filter.AppointmentTypeIds.Contains(a.AppointmentTypeId) : true
                        where filter.LocationIds.Any() ? filter.LocationIds.Contains(a.LocationId) : true
                        select new AppointmentDto
                        {
                            Id = a.Id,
                            TransactionId = t.TransactionId,
                            Date = a.Date,
                            PID = p.PatientId,
                            PhotoFile = p.Photofile,
                            PatientFirstName = p.FirstName,
                            HasMytestclient = p.HasMytestclient == true,
                            Gender = p.Gender ?? string.Empty,
                            PatientLastName = p.LastName ?? string.Empty,
                            HealthCardNumber = p.Healthcard ?? string.Empty,
                            HealthCardVersion =  p.HealthcardVersion ?? string.Empty,
                            Birthdate = p.Birthdate,
                            AppointmentType = at.Name ?? string.Empty,
                            AppointmentTypeCode = at.Code ?? string.Empty,
                            DoctorFullName = d.LastName + ", " + d.FirstName,
                            StatusId = es.StatusId,
                            StatusName = s.Name ?? string.Empty,
                            LocationId = a.LocationId,
                            IsAppliedAutoProduct = a.IsAppliedAutoProduct != null ? a.IsAppliedAutoProduct : 0,
                            TransactionItems = t.TransactionItems
                                .Where(ti => ti.Product.ModalityId != null && ti.Product.ModalityId > 0)
                                .Select(ti => new TransactionItemDto()
                                {
                                    TransactionItemId = ti.TransactionItemId,
                                    AccessionNo = ti.AccessionNo ?? string.Empty,
                                    Auxiliary = ti.Auxiliary ?? string.Empty,
                                    DateCreated = ti.DateCreated ?? null,
                                    Description = ti.Description ?? string.Empty,
                                    LastUpdated = ti.LastUpdated ?? null,
                                    ProductId = ti.ProductId ?? null,
                                    UserId = ti.UserId ?? null
                                })
                        };  
            

            #region check for Sorting
            var sortList = SortParser.Parse<AppointmentSortField>(filter.SortBy);
            query = query.ApplySorting(sortList, AppointmentSortMappings.RadiologyMap);
            #endregion

            var totalCount = query.Count();

            var result = totalCount > 0
                ? await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync()
                : new List<AppointmentDto>();

            return (result, totalCount);
        }

        public async Task<(List<UploaderAppointmentDto> Appointments, int TotalCount)> GetAppointmentsForUploaderAsync(AppointmentFilterDto filter)
        {
            var patients = context.Patient.AsQueryable();
            #region - check for Filters
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                filter.SearchTerm = filter.SearchTerm.Trim();
                //Check for ClientID (PatientID)
                if (int.TryParse(filter.SearchTerm, out int patientId))
                {
                    patients = patients.Where(a => a.PatientId == patientId);
                }
                else
                {
                    if (filter.SearchTerm.Contains(','))
                    {
                        var criteria = filter.SearchTerm.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        var lastNameSearch = criteria.ElementAtOrDefault(0)?.Trim();
                        var firstNameSearch = criteria.ElementAtOrDefault(1)?.Trim();
                        if (!string.IsNullOrEmpty(lastNameSearch))
                        {
                            patients = patients.Where(p => p.LastName.Contains(lastNameSearch));
                        }
                        if (!string.IsNullOrEmpty(firstNameSearch))
                        {
                            patients = patients.Where(p => p.FirstName.Contains(firstNameSearch));
                        }
                    }
                    else
                    {
                        patients = patients.Where(a =>
                        a.FirstName.Contains(filter.SearchTerm) ||
                        a.LastName.Contains(filter.SearchTerm));
                    }
                }
            }
            var patientsQuery = patients.Select(x => new
            {
                x.PatientId,
                x.Photofile,
                x.FirstName,
                x.LastName,
                x.Healthcard,
                x.HealthcardVersion,
                x.Birthdate,
                x.Gender,
                x.HasMytestclient,
                HasLetters = context.PrepLetter.Any(l => l.PatientId == x.PatientId)
            });
            #endregion

            var query = from a in context.Appointment.AsNoTracking()
                        join at in context.AppointmentType on a.AppointmentTypeId equals at.Id
                        join t in context.Transaction on a.Id equals t.InstanceId
                        join p in patientsQuery on a.PatientId equals p.PatientId
                        join es in context.EntityStatus on a.Id equals es.InstanceId
                        join s in context.Status on es.StatusId equals s.StatusId
                        where t.EntityTypeId == 3 && es.EntityTypeId == 3
                        where filter.AppointmentDate != null ? a.Date.Date == filter.AppointmentDate : true
                        where filter.StatusId.HasValue ? es.StatusId == filter.StatusId : es.StatusId != 1308
                        where filter.AppointmentTypeIds.Any() ? filter.AppointmentTypeIds.Contains(a.AppointmentTypeId) : true
                        where filter.LocationIds.Any() ? filter.LocationIds.Contains(a.LocationId) : true
                        select new UploaderAppointmentDto
                        {
                            Id = a.Id,
                            Date = a.Date,
                            PID = p.PatientId,
                            PhotoFile = p.Photofile,
                            PatientFirstName = p.FirstName,
                            HasMytestclient = p.HasMytestclient == true,
                            Gender = p.Gender ?? string.Empty,
                            PatientLastName = p.LastName ?? string.Empty,
                            HealthCardNumber = p.Healthcard ?? string.Empty,
                            HealthCardVersion = p.HealthcardVersion ?? string.Empty,
                            Birthdate = p.Birthdate,
                            AppointmentType = at.Name ?? string.Empty,
                            AppointmentTypeCode = at.Code ?? string.Empty,
                            StatusId = es.StatusId,
                            StatusName = s.Name ?? string.Empty,
                            HasLetters = p.HasLetters
                        };


            #region check for Sorting
            var sortList = SortParser.Parse<AppointmentSortField>(filter.SortBy);
            query = query.ApplySorting(sortList, AppointmentSortMappings.UploaderMap);
            #endregion

            var totalCount = query.Count();

            var result = totalCount > 0
                ? await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync()
                : new List<UploaderAppointmentDto>();

            return (result, totalCount);
        }

        public async Task<List<ProductAddDTO>> GetProductsToAdd(int appointmentId)
        {
            var productsToAdd = await (
                from p in _context.Product
                join es in _context.EntityStatus on p.ProductID equals es.InstanceId
                join s in _context.Status on es.StatusId equals s.StatusId
                where es.EntityTypeId == 40 && es.StatusId == 2700 
                where !_context.TransactionItem
                    .Any(ti => ti.ProductId == p.ProductID && 
                        ti.Product.ProductTypeId != 1 &&
                        ti.Transaction.InstanceId == appointmentId && 
                        ti.Transaction.EntityTypeId == 3)
                select new ProductAddDTO
                {
                    ProductId = p.ProductID,
                    Name = p.Name ?? "Unknown",
                    ModialityId = p.ModalityId ?? 0,
                    ProductTypeId = p.ProductTypeId
                }).ToListAsync();

            return productsToAdd;
        }

        public async Task<List<NoteDTO>> GetAppointmentNotes(int appointmentId)
        {
            return await _context.Note
                .Where(n => n.EntityTypeID == 3)
                .Where(n => n.InstanceID == appointmentId)
                .Select(n => new NoteDTO()
                {
                    CreateDate = n.CreateDate,
                    InstanceID = n.InstanceID,
                    EntityTypeID =  n.EntityTypeID,
                    NoteID = n.NoteID,
                    NoteType = n.NoteType,
                    Text = n.Text,
                    UserID = n.UserID
                }).ToListAsync();
        }


        //private IQueryable<Appointment> ApplyFilters(AppointmentFilterDto filter)
        //{
        //    FilterConfigOptions filterConfig = options.Value;

        //    var predicate = PredicateBuilder.New<Appointment>(true);

        //    foreach (var filterType in filterConfig.FilterPrecedence)
        //    {
        //        switch (filterType)
        //        {
        //            case "StatusId":
        //                if (filter.StatusId.HasValue)
        //                    predicate = predicate.And(x => x.StatusId == filter.StatusId);
        //                break;

        //            case "AppointmentType":
        //                if (!string.IsNullOrEmpty(filter.AppointmentType))
        //                    predicate = predicate.And(x => x.AppointmentType == filter.AppointmentType);
        //                break;

        //            case "AppointmentDate":
        //                if (filter.AppointmentDate.HasValue)
        //                {
        //                    var date = filter.AppointmentDate.Value.Date;
        //                    var nextDay = date.AddDays(1);
        //                    predicate = predicate.And(x => x.Date >= date && x.Date < nextDay);
        //                }
        //                break;

        //            case "SearchTerm":
        //                if (!string.IsNullOrEmpty(filter.SearchTerm))
        //                {
        //                    var term = filter.SearchTerm.ToLower();
        //                    predicate = predicate.And(x =>
        //                        (x.PatientFirstName ?? "").ToLower().Contains(term) ||
        //                        (x.PatientLastName ?? "").ToLower().Contains(term));
        //                }
        //                break;
        //        }
        //    }

        //    return context.Set<Appointment>().AsExpandable().Where(predicate);
        //}
    }
}
