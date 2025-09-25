using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManager.Enum;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class AccuroObservationRepository(ApplicationDbContext context) : IAccuroObservationRepository
    {
        public async Task<(List<AccuroLabPatientCollectionDTO> AccuroObservations, int TotalCount)> GetAccuroObservations(AccuroObservationFilterDto filter)
        {
            var query = from obs in context.AccuroLabObservationGroup
                        join p in context.Patient on obs.PatientId equals p.AccuroId
                        join l in context.PrepLetter on obs.LetterId equals l.LetterId into letters
                        from l in letters.DefaultIfEmpty()
                        where p.Birthdate != null
                        select new
                        {
                            obs.ReviewDate,
                            obs.Reviewer,
                            obs.SourceId,
                            obs.LetterId,
                            obs.DoNotUpload,
                            l.CreatedDate,
                            l.AvailableOnMytestclient,
                            p.Birthdate,
                            obs.FillerOrderNum,
                            obs.OrderProvider,
                            obs.SourceName,
                            obs.ReviewerName,
                            p.FirstName,
                            p.LastName,
                            p.Photofile,
                            obs.CollectionDate,
                            p.AccuroId,
                            p.PatientId,
                            HasMytestclient = p.HasMytestclient == true
                        };

            var age18TodayBirthdate = DateOnly.FromDateTime(DateTime.Today.AddYears(-18));
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(obs => obs.LetterId == null && obs.DoNotUpload == null);
                filter.SearchTerm = filter.SearchTerm.Trim();
                //Check for ClientID (PatientID)
                if (int.TryParse(filter.SearchTerm, out int patientId))
                {
                    query = query.Where(a => a.PatientId == patientId);
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
                            query = query.Where(p => p.LastName.Contains(lastNameSearch));
                        }
                        if (!string.IsNullOrEmpty(firstNameSearch))
                        {
                            query = query.Where(p => p.FirstName.Contains(firstNameSearch));
                        }
                    }
                    else
                    {
                        query = query.Where(a => 
                            a.FirstName.Contains(filter.SearchTerm) || 
                            a.LastName.Contains(filter.SearchTerm));
                    }
                }
            }
            else 
            {
                switch (filter.ResultStatus)
                {
                    case AccuroObservationResultStatus.UploadedResults:
                        query = query.Where(x => x.LetterId != null);
                        break;
                    case AccuroObservationResultStatus.DoNotUpload:
                        query = query.Where(x => x.DoNotUpload == true);
                        break;
                    case AccuroObservationResultStatus.ReviewedResults:
                        query = query
                            .Where(x => x.LetterId == null)
                            .Where(x => x.DoNotUpload != true);
                        break;
                    case AccuroObservationResultStatus.Delayed48Hours:
                        query = query
                            .Where(x => x.CreatedDate != null)
                            .Where(x => x.AvailableOnMytestclient > DateTime.Now)
                            .Where(x => x.CreatedDate.Value.Date != x.AvailableOnMytestclient.Value.Date); 
                        break;
                    default:
                        break;
                }
                if (filter.ResultStatus != AccuroObservationResultStatus.Delayed48Hours) 
                {
                    if (filter.ReviewedDate.HasValue)
                    {
                        query = query.Where(x => x.ReviewDate != null && x.ReviewDate.Value.Date == filter.ReviewedDate.Value.Date);
                    }
                    if (filter.ReviewedPhysician.HasValue)
                    {
                        query = query.Where(obs => obs.Reviewer == filter.ReviewedPhysician.Value);
                    }
                    if (filter.ExternalLab.HasValue)
                    {
                        query = query.Where(obs => obs.SourceId == filter.ExternalLab.Value);
                    }
                }
            }

            if (filter.Pediatric)
            {
                query = query.Where(x => x.Birthdate != null && x.Birthdate.Value > age18TodayBirthdate);
            }
            
            var groupedOrdersQuery = from x in query
                                     group new
                                     {
                                         x.FillerOrderNum,
                                         x.OrderProvider,
                                         x.ReviewDate,
                                         x.SourceName,
                                         x.ReviewerName,
                                         x.LetterId,
                                         x.DoNotUpload,
                                         x.Birthdate,
                                         x.AvailableOnMytestclient
                                     }
                                     by new AccuroLabPatientCollectionDTO
                                     {
                                         PatientFirstName = x.FirstName,
                                         PatientLastName = x.LastName,
                                         PhotoFile = x.Photofile,
                                         CollectionDate = x.CollectionDate,
                                         AccuroId = x.AccuroId,
                                         PatientId = x.PatientId,
                                         HasMytestclient = x.HasMytestclient == true
                                     };

            var totalCount = await groupedOrdersQuery.CountAsync();
            if (totalCount == 0) 
            {
                return ([], totalCount);
            }
            var queryData = await groupedOrdersQuery
                //.Skip((filter.Page - 1) * filter.PageSize)
                //.Take(filter.PageSize)
                .ToListAsync();
            List<AccuroLabPatientCollectionDTO> result = [];
            foreach (var ordersGroup in queryData)
            {
                var patientCollection = ordersGroup.Key;
                HashSet<AccuroLabOrdersSummary> ordersSummary = [];
                HashSet<string> orders = [];
                foreach (var order in ordersGroup) 
                {
                    patientCollection.Birthdate = order.Birthdate;
                    ordersSummary.Add(new AccuroLabOrdersSummary()
                    {
                        OrderProvider = order.OrderProvider,
                        ReviewDate = order.ReviewDate,
                        ReviewerName = order.ReviewerName,
                        SourceName = order.SourceName,
                        LetterId = order.LetterId,
                        DoNotUpload = order.DoNotUpload,
                        AvailableOnMytestclient = order.AvailableOnMytestclient
                    });
                    orders.Add(order.FillerOrderNum);
                }
                patientCollection.OrdersSummary = ordersSummary;
                patientCollection.Orders = orders;
                result.Add(patientCollection);
            }

            return (result, totalCount);
        }

        public async Task UpdateObservationGroupsUploads(AccuroObservationGroupPatchDTO patchDTO) 
        {
            DateTime? doNotUploadDate = patchDTO.DoNotUpload ? DateTime.Now : null;
            string? doNotUploadNote = patchDTO.DoNotUpload ? patchDTO.DoNotUploadNote : null;
            await context.AccuroLabObservationGroup
                .Where(og => patchDTO.Orders.Contains(og.FillerOrderNum))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(og => og.LetterId, patchDTO.LetterId)
                    .SetProperty(og => og.DoNotUpload, patchDTO.DoNotUpload)
                    .SetProperty(og => og.DoNotUploadDate, doNotUploadDate)
                    .SetProperty(og => og.DoNotUploadNote, doNotUploadNote)
                );
        }

        public async Task<AccuroObservationsDropdownsDTO> GetDropdownsByReviewDate(DateTime reviewDate) 
        {
            var query = await context.AccuroLabObservationGroup
                .Where(x => x.ReviewDate != null && x.ReviewDate.Value.Date == reviewDate)
                .Select(x => new 
                { 
                    x.Reviewer,
                    x.ReviewerName,
                    x.SourceId,
                    x.SourceName
                }).Distinct().ToListAsync();

            AccuroObservationsDropdownsDTO result = new();
            foreach (var row in query)
            {
                if (row.Reviewer.HasValue && !result.Reviewers.ContainsKey(row.Reviewer.Value))
                {
                    result.Reviewers.Add(row.Reviewer.Value, row.ReviewerName);
                }

                if (row.SourceId.HasValue && !result.ExternalLabs.ContainsKey(row.SourceId.Value))
                {
                    result.ExternalLabs.Add(row.SourceId.Value, row.SourceName);
                }
            }

            return result;
        }
        
        public async Task<IEnumerable<AccuroLabObservationGroupDTO>> GetPatientOrdersResults(int patientId, IEnumerable<string> orderIds) 
        {
            var query = from g in context.AccuroLabObservationGroup
                        join p in context.Patient on g.PatientId equals p.AccuroId
                        join l in context.PrepLetter on g.LetterId equals l.LetterId into letters
                        from l in letters.DefaultIfEmpty()
                        where p.PatientId == patientId
                        where orderIds.Contains(g.FillerOrderNum)
                        select new AccuroLabObservationGroupDTO()
                        {
                            ReviewDate = g.ReviewDate,
                            ActiveVersion = g.ActiveVersion,
                            BaseGroupId = g.BaseGroupId,
                            CollectionDate = g.CollectionDate,
                            DoNotUpload = g.DoNotUpload,
                            DoNotUploadDate = g.DoNotUploadDate,
                            DoNotUploadNote = g.DoNotUploadNote,
                            FillerOrderNum = g.FillerOrderNum,
                            GroupId = g.GroupId,
                            LetterId = g.LetterId,
                            Letter = l != null ? new PrepLetterDTO()
                            {
                                Body = l.Body,
                                LetterName = l.LetterName,
                                AppointmentId = l.AppointmentId,
                                AvailableOnMytestclient = l.AvailableOnMytestclient,
                                CreatedDate = l.CreatedDate
                            } : null,
                            OrderGroup = g.OrderGroup,
                            OrderProvider = g.OrderProvider,
                            OrderProviderWithId = g.OrderProviderWithId,
                            Reviewer = g.Reviewer,
                            ReviewerName = g.ReviewerName,
                            SappId = g.SappId,
                            SourceId = g.SourceId,
                            SourceName = g.SourceName,
                            TestId = g.TestId,
                            TestName = g.TestName,
                            TipsObservationGroupId = g.TipsObservationGroupId,
                            TransactionDate = g.TransactionDate,
                            TransferTipsDate = g.TransferTipsDate,
                            UniversalSerialNum = g.UniversalSerialNum,
                            UploadedToMytestclient = g.UploadedToMytestclient,
                            UserNotes = g.UserNotes,
                            Delayed48h = l.CreatedDate != null && l.AvailableOnMytestclient != null && l.CreatedDate.Value.Date != l.AvailableOnMytestclient.Value.Date,
                            Observations = g.Observations.Select(obs => new AccuroLabObservationDTO()
                            {
                                GroupId = obs.GroupId,
                                Label = obs.Label,
                                ObsDisplayRefRange = obs.ObsDisplayRefRange,
                                ObservationalResultStatus = obs.ObservationalResultStatus,
                                ObservationalSubIdNumber = obs.ObservationalSubIdNumber,
                                ObservationDate = obs.ObservationDate,
                                ObservationFlag = obs.ObservationFlag,
                                ObservationId = obs.ObservationId,
                                ObservationNote = obs.ObservationNote,
                                ObservationReferenceRange = obs.ObservationReferenceRange,
                                ObservationUnits = obs.ObservationUnits,
                                ObservationValue = obs.ObservationValue,
                                OrderNum = obs.OrderNum,
                                ResultId = obs.ResultId,
                                TipsObservationId = obs.TipsObservationId
                            })
                        };

            return await query.ToListAsync();
        }
        
        public async Task<IEnumerable<AccuroLogDTO>> GetPatientUploadedLogs(int patientId)
        {
            var query = from obs in context.AccuroLabObservationGroup
                        join p in context.Patient on obs.PatientId equals p.AccuroId
                        join l in context.PrepLetter on obs.LetterId equals l.LetterId into letters
                        from l in letters.DefaultIfEmpty()
                        where p.PatientId == patientId
                        where obs.DoNotUpload == true || obs.LetterId > 0
                        select new
                        {
                            OrderedBy = obs.OrderProvider,
                            ReviewedDate = obs.ReviewDate,
                            DoNotUpload = obs.DoNotUpload,
                            DoNotUploadDate = obs.DoNotUploadDate,
                            LetterId = obs.LetterId,
                            OrderId = obs.FillerOrderNum,
                            ViewedDate = l.ViewedDate,
                            ResultType = "Lab Results",
                            UploadedDate = l.CreatedDate,
                            Delay48hours = l.CreatedDate != null && l.AvailableOnMytestclient != null && l.CreatedDate.Value.Date != l.AvailableOnMytestclient.Value.Date,
                        };
            var queryResult = await query.Distinct().ToListAsync();
            var groups = queryResult.GroupBy(x => new AccuroLogDTO()
            {
                Delay48hours = x.Delay48hours,
                DoNotUpload = x.DoNotUpload,
                DoNotUploadDate = x.DoNotUploadDate,
                LetterId = x.LetterId,
                ResultType = x.ResultType,
                UploadedDate = x.UploadedDate,
                ViewedDate = x.ViewedDate
            });
            List<AccuroLogDTO> result = [];
            foreach (var group in groups)
            {
                group.Key.LogsSummary = group.Select(x => new OrderLogSummary() 
                {
                    OrderedBy = x.OrderedBy,
                    ReviewedDate = x.ReviewedDate,
                    OrderId = x.OrderId
                });
                result.Add(group.Key);
            }

            return result;
        }

    }
}
