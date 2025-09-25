using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class LetterRepository(ApplicationDbContext context): ILetterRepository
    {
        public async Task<int> AddLetter(PrepLetterDTO prepLetterDTO) 
        {
            var now = DateTime.Now;
            PrepLetter letter = new ()
            {
                Body = prepLetterDTO.Body,
                AppointmentId = prepLetterDTO.AppointmentId,
                CreatedDate = prepLetterDTO.CreatedDate ?? now,
                IsAccessible = prepLetterDTO.IsAccessible,
                LetterName = prepLetterDTO.LetterName,
                LetterTypeId = prepLetterDTO.LetterTypeId,
                OpenLetter = prepLetterDTO.OpenLetter,
                PatientId = prepLetterDTO.PatientId,
                ReceivedFromCrawford = prepLetterDTO.ReceivedFromCrawford,
                ReferralId = prepLetterDTO.ReferralId,
                SentToCrawford = prepLetterDTO.SentToCrawford,
                UserId = prepLetterDTO.UserId,
                AvailableOnMytestclient = prepLetterDTO.AvailableOnMytestclient ?? now,
            };
            
            await context.PrepLetter.AddAsync(letter);

            if (prepLetterDTO.Attachments != null && prepLetterDTO.Attachments.Any()) 
            {
                var attachments = prepLetterDTO.Attachments.Select(prepAttachmentDTO => new PrepAttachment()
                {
                    CreatedDate = DateTime.Now,
                    //AttachmentPDF = prepAttachmentDTO.AttachmentPDF,
                    Path = prepAttachmentDTO.Path, 
                    Description = prepAttachmentDTO.Description,
                    InstanceId = prepAttachmentDTO.InstanceId,
                    Letter = letter,
                    LetterTypeId = prepAttachmentDTO.InstanceId,
                    PassCode = prepAttachmentDTO.PassCode,
                    PatientId = prepAttachmentDTO.PatientId,
                    UserId = prepAttachmentDTO.UserId
                }).ToList();
                letter.Attachments = attachments;
                await context.PrepAttachment.AddRangeAsync(attachments);
            }

            await context.SaveChangesAsync();
            return letter.LetterId;
        }

        public async Task<PrepLetterDTO?> GetLetterById(int letterId)
        {
            PrepLetter? letter = await context.PrepLetter
                .Include(x => x.Attachments)
                .FirstOrDefaultAsync(x => x.LetterId == letterId);

            if (letter == null) 
            {
                return null;
            }

            return new PrepLetterDTO
            {
                Body = letter.Body,
                AppointmentId = letter.AppointmentId,
                CreatedDate = letter.CreatedDate,
                IsAccessible = letter.IsAccessible.GetValueOrDefault(),
                LetterName = letter.LetterName,
                LetterTypeId = letter.LetterTypeId,
                OpenLetter = letter.OpenLetter.GetValueOrDefault(),
                PatientId = letter.PatientId,
                ReceivedFromCrawford = letter.ReceivedFromCrawford.GetValueOrDefault(),
                ReferralId = letter.ReferralId,
                SentToCrawford = letter.SentToCrawford.GetValueOrDefault(),
                UserId = letter.UserId,
                LetterId = letter.LetterId,
                ViewedDate = letter.ViewedDate,
                AvailableOnMytestclient = letter.AvailableOnMytestclient,
                Attachments = letter.Attachments.Select(a => new PrepAttachmentDTO()
                {
                    CreatedDate = a.CreatedDate,
                    //AttachmentPDF = a.AttachmentPDF,
                    Path = a.Path,
                    Description = a.Description,
                    InstanceId = a.InstanceId,
                    LetterTypeId = a.InstanceId,
                    PassCode = a.PassCode,
                    PatientId = a.PatientId,
                    UserId = a.UserId
                }).ToList()
            };
        }

        public async Task RemoveLetter(int letterId)
        {
            await context.AccuroLabObservationGroup
                .Where(og => og.LetterId == letterId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(og => og.LetterId, default(int?)));

            await context.PrepAttachment
                .Where(a => a.LetterId == letterId)
                .ExecuteDeleteAsync();
            
            await context.PrepLetter
                .Where(a => a.LetterId == letterId)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<PrepLetterDTO>> GetLettersByPatientIdLetterTypeId(int patientId, int letterTypeId)
        {
            return await context.PrepLetter
                .Where(x => x.LetterTypeId == letterTypeId && x.PatientId == patientId)
                .Select(letter => new PrepLetterDTO
                {
                    Body = letter.Body,
                    AppointmentId = letter.AppointmentId,
                    CreatedDate = letter.CreatedDate,
                    IsAccessible = letter.IsAccessible.GetValueOrDefault(),
                    LetterName = letter.LetterName,
                    LetterTypeId = letter.LetterTypeId,
                    OpenLetter = letter.OpenLetter.GetValueOrDefault(),
                    PatientId = letter.PatientId,
                    ReceivedFromCrawford = letter.ReceivedFromCrawford.GetValueOrDefault(),
                    ReferralId = letter.ReferralId,
                    SentToCrawford = letter.SentToCrawford.GetValueOrDefault(),
                    UserId = letter.UserId,
                    LetterId = letter.LetterId,
                    ViewedDate = letter.ViewedDate,
                    AvailableOnMytestclient = letter.AvailableOnMytestclient,
                    Attachments = letter.Attachments.Select(a => new PrepAttachmentDTO()
                    {
                        CreatedDate = a.CreatedDate,
                        Path = a.Path,
                        //AttachmentPDF = a.AttachmentPDF,
                        Description = a.Description,
                        InstanceId = a.InstanceId,
                        LetterTypeId = a.InstanceId,
                        PassCode = a.PassCode,
                        PatientId = a.PatientId,
                        UserId = a.UserId
                    })
                }
            ).ToListAsync();
        }
    }
}
