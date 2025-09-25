using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using TestManager.Domain.Model.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;

namespace TestManager.DataAccess;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLog { get; set; }
    public virtual DbSet<Address> Address { get; set; }
    public virtual DbSet<Andrologist> Andrologist { get; set; }
    public virtual DbSet<Appointment> Appointment { get; set; }
    public virtual DbSet<AppointmentRule> AppointmentRules { get; set; }
    public virtual DbSet<AppointmentType> AppointmentType { get; set; }
    public virtual DbSet<Contact> Contact { get; set; }
    public virtual DbSet<ContactType> ContactType { get; set; }
    public virtual DbSet<DICOMModality> DICOMModality { get; set; }
    public virtual DbSet<Doctor> Doctor { get; set; }
    public virtual DbSet<EntityStatus> EntityStatus { get; set; }
    public virtual DbSet<EntityTypes> EntityTypes { get; set; }
    public virtual DbSet<ErrorLog> ErrorLog { get; set; }
    public virtual DbSet<Invoice> Invoice { get; set; }
    public virtual DbSet<Locations> Location { get; set; }
    public virtual DbSet<Note> Note { get; set; }
    public virtual DbSet<Patient> Patient { get; set; }

    public virtual DbSet<PrepAttachment> PrepAttachment { get; set; }
    public virtual DbSet<PrepLetter> PrepLetter { get; set; }
    public virtual DbSet<PrepLetterType> PrepLetterType { get; set; }
    public virtual DbSet<Prep_ReportingTeam> Prep_ReportingTeam { get; set; }
    public virtual DbSet<Prep_ReportingTeamUser> Prep_ReportingTeamUser { get; set; }
    public virtual DbSet<Prep_ReportingTeamTemplate> Prep_ReportingTeamTemplate { get; set; }
    public virtual DbSet<PrepTemplate> PrepTemplate { get; set; }
    public virtual DbSet<Prep_ResourceCategory> Prep_ResourceCategory { get; set; }
    public virtual DbSet<Prep_EducationMaterial> Prep_EducationMaterial { get; set; }
    public virtual DbSet<Prep_ClientEducation> Prep_ClientEducation { get; set; }
    public virtual DbSet<PrepEmailLog> PrepEmailLog { get; set; }
    public virtual DbSet<AccuroLabObservationResultsActivity> AccuroLabObservationResultsActivity { get; set; }
    public  virtual DbSet<Advice> Advice { get; set; }
    public virtual DbSet<PrepUploadReports> PrepUploadReports { get; set; }

    public virtual DbSet<Prm_AdditionalTest> Prm_AdditionalTest { get; set; }

    public virtual DbSet<Prm_Audit> Prm_Audit { get; set; }

    public virtual DbSet<Prm_ClientAdditionalTest> Prm_ClientAdditionalTest { get; set; }

    public virtual DbSet<Prm_ClientLetter> Prm_ClientLetter { get; set; }

    public virtual DbSet<Prm_ClientMedicalIssue> Prm_ClientMedicalIssue { get; set; }
    
    public virtual DbSet<Prm_Fitness> Prm_Fitness { get; set; }
    public virtual DbSet<Prm_FitnessHtmlReport> Prm_FitnessHtmlReport { get; set; }

    public virtual DbSet<Prm_Letter> Prm_Letter { get; set; }

    public virtual DbSet<Prm_PaedsConsent> Prm_PaedsConsent { get; set; }

    public virtual DbSet<Prm_PaedsFitness> Prm_PaedsFitness { get; set; }

    public virtual DbSet<Prm_PaedsNursing> Prm_PaedsNursing { get; set; }

    public virtual DbSet<Prm_PaedsPhycology> Prm_PaedsPhycology { get; set; }

    public virtual DbSet<Prm_PaedsPhycologyNote> Prm_PaedsPhycologyNote { get; set; }

    public virtual DbSet<Prm_StandardMedicalIssue> Prm_StandardMedicalIssue { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<Status> Status { get; set; }

    public virtual DbSet<Transaction> Transaction { get; set; }

    public virtual DbSet<TransactionItem> TransactionItem { get; set; }

    public virtual DbSet<User> User { get; set; }  

    public virtual DbSet<Role> Role { get; set; }
    public virtual DbSet<UserRole> UserRole { get; set; }
    public virtual DbSet<RoleReport> RoleReport { get; set; }
    public virtual DbSet<SequenceValue> SequenceValue { get; set; }

    public virtual DbSet<ProductType> ProductType { get; set; }
    public virtual DbSet<ProductGroup> ProductGroup { get; set; }
    public virtual DbSet<CMS_SyncTracking> CMS_SyncTracking { get; set; }

    public virtual DbSet<NurseCommunicationType> NurseCommunicationType { get; set; }
    public virtual DbSet<AccuroLabObservation> AccuroLabObservation { get; set; }
    public virtual DbSet<AccuroLabObservationGroup> AccuroLabObservationGroup { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing!");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(p => p.Id);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Address01).HasMaxLength(500);
            entity.Property(e => e.Address02).HasMaxLength(500);
            entity.Property(e => e.BusinessName).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(250);
            entity.Property(e => e.Country).HasMaxLength(60);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.ProvinceState).HasMaxLength(50);
        });

        modelBuilder.Entity<Andrologist>(entity =>
        {
            entity.Ignore(p => p.Id);
            entity.HasKey(p => p.AndrologistID);
            //entity.HasQueryFilter(a => !a.IsDeleted);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.AccuroId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AdditionalConfirmationText).IsUnicode(false);
            entity.Property(e => e.AppointmentStyle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AppointmentTypeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApptTypeChangeDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalNotes).IsUnicode(false);
            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BadExperience)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookedByUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cancelled)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClinicId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Complaint).IsUnicode(false);
            entity.Property(e => e.ComplaintNotes).IsUnicode(false);
            entity.Property(e => e.ConfirmationSent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedExitMSTrans)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoctorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExitTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HSID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InitialCommunication).IsUnicode(false);
            entity.Property(e => e.InvoluntaryReschedules)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsAccessibleAvailable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsAppliedAutoProduct)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdated)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NonClinic)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NurseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OptimisticLockField)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OriginalDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Paid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PatientId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PayOnArrival)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentCoverage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentRequired)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneConsult)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrepPacketSent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PreviousBadExperience)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProviderId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.ReferralContactByPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferrerDoctorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReminderEmailAddendum).IsUnicode(false);
            entity.Property(e => e.RoomId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ScheduleID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Scheduled)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SoldByUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusIcon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusIconTimer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalElapsed)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VIP)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VoluntaryReschedules)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WaitTImeWarningTimeout)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WaitTimeExtremeTimeout)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.mytestclientView)
                .HasMaxLength(50)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Transaction)
            .WithOne(t => t.Appointment)
            .HasForeignKey<Transaction>(t => t.InstanceId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithOne(p => p.Appointment)
                .HasForeignKey<Appointment>(a => a.PatientId);

            // entity.HasMany(a => a.Notes)
            //.WithOne(n => n.Appointment)
            //.HasForeignKey(n => n.InstanceID);

            entity
                .HasMany(a => a.Notes)
                .WithOne(n => n.Appointment)
                .HasForeignKey(n => n.InstanceID)
                .OnDelete(DeleteBehavior.NoAction); // optional
        });

        modelBuilder.Entity<AppointmentRule>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AppointmentRule>()
                .HasKey(p => p.Id);

            //modelBuilder.Entity<AppointmentRule>()
            //    .HasQueryFilter(a => a.IsActive);
        });

        modelBuilder.Entity<AppointmentType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.AbleToCancelledHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccuroId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccuroOfficeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Active)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddOn)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AppointmentTypeSubGroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApptBackColorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApptForeColorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApptGroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AutoAddNRF)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AutoEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookingEmailFallback)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookingEmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CancellationFee)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckForReferrals)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DontCloseRecallsIfCantContact)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EnhancedPatientManagerInterface)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Floor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HSID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsCHA)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsParentAppt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsSearchable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LISBarCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LearnMore)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LinkedToCha)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_DoctorGender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_DoctorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_DoctorRequired)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_FollowupAppointmentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_InitialAppointmentMonthInterval)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_InitialAppointmentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_InitialIncludeFollowup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_InitialIncludeFollowupName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnlineBooking_ReasonRequired)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrigCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PMArrivalStation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Point)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Preparation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallApptTypeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_CorpEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_CorpPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_CorpPostalMail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_CorpVoiceMail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_PrivEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_PrivPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_PrivPostalMail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallTimeline_PrivVoiceMail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReminderEmailCalendarDays)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReminderEmailFallback)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReminderEmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReminderEmailLeadTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequiresMedicalReport)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequiresProduct)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequiresProductType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequiresReason)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SearchApptGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SecondEmailReminder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeMins)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalScheduleSlots)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TriggersRecallTask)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TriggersTentativeAppointment)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WaitingTimeExtremeTimeout)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WaitingTimeWarningTimeout)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.mytestclient)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ContactValue).HasMaxLength(200);
        });

        modelBuilder.Entity<ContactType>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ContactType1)
                .HasMaxLength(100)
                .HasColumnName("ContactType");
        });

        modelBuilder.Entity<DICOMModality>(entity =>
        {
            entity.HasKey(e => e.DICOMModalityId);

            entity.Ignore(e => e.Id);

            //entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.ModalityCode).HasMaxLength(50);
            entity.Property(e => e.ProcedureCode).HasMaxLength(50);
            entity.Property(e => e.RoomCode).HasMaxLength(10);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Affiliation).HasMaxLength(200);
            entity.Property(e => e.CPSO).HasMaxLength(10);
            entity.Property(e => e.DesignatedNurse).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Initials).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.License).HasMaxLength(20);
            entity.Property(e => e.OhipGroup).HasMaxLength(10);
            entity.Property(e => e.OhipProvider).HasMaxLength(100);
            entity.Property(e => e.OhipSpecialtyCode).HasMaxLength(5);
            entity.Property(e => e.PlatinumNurse).HasMaxLength(200);
            entity.Property(e => e.ProviderType).HasMaxLength(10);
            entity.Property(e => e.ReferralCoordinator).HasMaxLength(200);
        });

        modelBuilder.Entity<EntityStatus>(entity =>
        {
            //entity.Property(e => e.EntityStatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<EntityTypes>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Entity).HasDefaultValue(0);
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Method).HasMaxLength(500);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.InvoiceID);

        });

        modelBuilder.Entity<Locations>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.NoteID);
            entity.HasQueryFilter(e => e.NoteType == 2 && e.EntityTypeID == 3);
            entity
                .HasOne(e => e.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserID)
                .HasPrincipalKey(u => u.UserId);

        });

        modelBuilder.Entity<Patient>(entity =>
        {
            modelBuilder.Entity<Patient>()
                .HasKey(e => e.PatientId);
            modelBuilder.Entity<Patient>()
                .Ignore(e => e.Id);

            entity.Property(e => e.PatientId).ValueGeneratedNever();
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AlternateAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.AlternateExt)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.AlternatePhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Assistant)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AssistantAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.AssistantEmail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AssistantExtension)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.AssistantPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BusinesCity)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BusinessAddress01)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusinessAddress02)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusinessAddress03)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusinessAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCountry)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.BusinessExt)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.BusinessPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BusinessPostalCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BusinessProvince)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cell)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CellAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ConfidentialEmail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyName)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyRelationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ethnicity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FaxAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.FileNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Grade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HSID)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HealthCardNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HealthCardEncrypt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Healthcard)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HealthcardVersion)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Initials)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.InternationalPhone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastAppointmentWithFamilyDoctor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .IsUnicode(false);
            entity.Property(e => e.ManualFlagNoRecall)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Marital_Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nickname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NoRecall)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OldGeneralEmail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.OtherHealthNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pharmacy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PharmacyAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PharmacyPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Photofile).IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PreferredPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryAreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryExt)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryPhoneType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PronouncedAs)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecallNote).IsUnicode(false);
            entity.Property(e => e.Salutation)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.SecondaryPhoneType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Suffix)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.WorkPosition)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.WorkStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PrepAttachment>(entity =>
        {
            entity.ToTable("Prep_Attachment", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.AttachmentId);
            entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");
            entity.Property(e => e.LetterTypeId).HasColumnName("LetterTypeID");
            entity.Property(e => e.LetterId).HasColumnName("LetterID");
            entity.Property(e => e.InstanceId).HasColumnName("InstanceID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedDate).HasColumnName("CreateDate");
            entity.Property(e => e.AttachmentPDF).HasColumnType("image");
            entity.Property(e => e.PassCode).HasMaxLength(50);
            
            entity.HasOne(a => a.Letter)
                .WithMany(l => l.Attachments)
                .HasForeignKey(a => a.LetterId);
        });

        modelBuilder.Entity<PrepLetter>(entity =>
        {
            entity.ToTable("Prep_Letter", "cmsuploader");
            entity.Property(e => e.LetterId).HasColumnName("LetterID");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.LetterId);
            entity.Property(e => e.LetterTypeId).HasColumnName("LetterTypeID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.ReferralId).HasColumnName("ReferralID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedDate).HasColumnName("Createddate");
            entity.Property(e => e.AvailableOnMytestclient).HasColumnName("AvailableOnMytestclient");
            entity.Property(e => e.LetterName).HasMaxLength(200);
            entity.Property(e => e.ViewedDate).HasColumnName("Vieweddate");
        });

        modelBuilder.Entity<PrepLetterType>(entity =>
        {
            entity.HasKey(e => e.LetterTypeId);
        });

        modelBuilder.Entity<Prep_ReportingTeam>(entity =>
        {
            entity.ToTable("Prep_ReportingTeam", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.ReportingTeamId);
            entity.HasMany(e => e.TeamUsers)
            .WithOne(eu => eu.ReportingTeam)
            .HasForeignKey(eu => eu.ReportingTeamId);
            entity.HasMany(e => e.TeamTemplates)
            .WithOne()
            .HasForeignKey(eu => eu.ReportingTeamId);
        });

        modelBuilder.Entity<Prep_ReportingTeamUser>(entity =>
        {
            entity.ToTable("Prep_ReportingTeamUser", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.ReportingTeamUserId);
            entity.HasOne(e => e.ReportingTeam)
               .WithMany(rt => rt.TeamUsers)
               .HasForeignKey(e => e.ReportingTeamId);
        });

        modelBuilder.Entity<Prep_ReportingTeamTemplate>(entity =>
        {
            entity.ToTable("Prep_ReportingTeamTemplate", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.ReportingTeamTemplateId);
            //entity.HasOne(pt => pt.PrepTemplate)
            //.WithOne()
            //.HasForeignKey<PrepTemplate>(pt => pt.TemplateId);
            modelBuilder.Entity<Prep_ReportingTeamTemplate>()
               .HasOne(rtt => rtt.Prep_ReportingTeam)
               .WithMany(rt => rt.TeamTemplates)
               .HasForeignKey(rtt => rtt.ReportingTeamId);

            modelBuilder.Entity<Prep_ReportingTeamTemplate>()
            .HasOne(rtt => rtt.PrepTemplate)
            .WithMany()
            .HasForeignKey(rtt => rtt.TemplateId);

        });

        modelBuilder.Entity<PrepTemplate>(entity =>
        {
            entity.ToTable("Prep_Template", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.TemplateId);
            //entity.Property(e => e.Subject1).HasMaxLength(500);
            //entity.Property(e => e.Subject2).HasMaxLength(500);
        });

        modelBuilder.Entity<Prep_ResourceCategory>(entity =>
        {
            entity.ToTable("Prep_ResourceCategory", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.ConditionCategoryId);
            entity.Property(e => e.ConditionCategoryId).HasColumnName("ConditionCategoryID");
            entity.Property(e => e.Description).HasMaxLength(50);
            //entity.HasMany(rc => rc.Prep_EducationMaterials) // <-- Navigation
            //  .WithOne(em => em.Prep_ResourceCategory)
            //  .HasForeignKey(em => em.ConditionCategoryId);
        });

        modelBuilder.Entity<Prep_EducationMaterial>(entity =>
        {
            entity.ToTable("Prep_EducationMaterial", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.EducationMaterialId);
            entity.HasQueryFilter(e => e.ConditionCategoryId > 0);
            entity.Property(e => e.EducationMaterialId).HasColumnName("EducationMaterialID");
            entity.Property(e => e.ConditionCategoryId).HasColumnName("ConditionCategoryID");
            entity.Property(e => e.Pdf).HasColumnType("image").HasColumnName("pdf");
            entity.Property(e => e.Url).HasColumnName("url");
            
            //entity.HasOne(em => em.Prep_ResourceCategory)
            //    .WithMany(rc => rc.Prep_EducationMaterials)
            //    .HasForeignKey(em => em.ConditionCategoryId);
        });

        modelBuilder.Entity<Prep_ClientEducation>(entity =>
        {
            entity.ToTable("Prep_ClientEducation", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.HasKey(e => e.ClientEducationId);
            entity.Property(e => e.ClientEducationId).HasColumnName("ClientEducationID");
            entity.Property(e => e.EducationMaterialId).HasColumnName("EducationMaterialID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.InActiveByUserId).HasColumnName("InActiveByUserID");
            entity.HasOne<Prep_EducationMaterial>(ce => ce.EducationMaterial)
            .WithMany()
            .HasForeignKey(em => em.EducationMaterialId);

        });

        modelBuilder.Entity<NurseCommunicationType>(entity =>
        {
            entity.ToTable("NurseCommunicationType", "cmsuploader");
            entity.Ignore(n => n.Id);
            entity.HasKey(n => n.NurseCommunicationTypeId);
        });

        modelBuilder.Entity<PrepUploadReports>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ACT).HasMaxLength(250);
            entity.Property(e => e.Genetic).HasMaxLength(250);
            entity.Property(e => e.Nomytestclient).HasMaxLength(50);
            entity.Property(e => e.poc).HasMaxLength(250);
        });

        modelBuilder.Entity<PrepEmailLog>(entity =>
        {
            entity.ToTable("Prep_EmailLog", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.Property(e => e.EmailLogId).HasColumnName("EmailLogID");
            entity.HasKey(e => e.EmailLogId);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Advice>(entity =>
        {
            entity.ToTable("Advice", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.Property(e => e.AdviceId).HasColumnName("AdviceID");
            entity.HasKey(e => e.AdviceId);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.NurseCommunicationTypeId).HasColumnName("NurseCommunicationTypeID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
        });

        modelBuilder.Entity<AccuroLabObservationResultsActivity>(entity =>
        {
            entity.ToTable("Accuro_lab_observation_results_activity", "cmsuploader");
            entity.Ignore(e => e.Id);
            entity.Property(e => e.ObservationResultsLogId).HasColumnName("observation_results_log_id");
            entity.HasKey(e => e.ObservationResultsLogId);
            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.CollectionDate).HasColumnName("collection_date");
            entity.Property(e => e.Activity).HasColumnName("activity");
            entity.Property(e => e.CreatedDate).HasColumnName("createddate");
            entity.Property(e => e.UserId).HasColumnName("userId");

        });

        modelBuilder.Entity<Prm_AdditionalTest>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<Prm_Audit>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<Prm_ClientAdditionalTest>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.LabRequisition).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(50);
            entity.Property(e => e.Test_Full_Name).HasMaxLength(200);
            entity.Property(e => e.TimeFrame).HasMaxLength(50);
        });

        modelBuilder.Entity<Prm_ClientLetter>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Signature).HasMaxLength(300);
        });

        modelBuilder.Entity<Prm_ClientMedicalIssue>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.IssueName).HasMaxLength(500);
        });

        modelBuilder.Entity<Prm_Fitness>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.BodyStation).HasMaxLength(300);
        });

        modelBuilder.Entity<Prm_FitnessHtmlReport>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<Prm_Letter>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Signature).HasMaxLength(300);
        });

        modelBuilder.Entity<Prm_PaedsConsent>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.LiveWithName1).HasMaxLength(200);
            entity.Property(e => e.LiveWithName2).HasMaxLength(200);
            entity.Property(e => e.LiveWithName3).HasMaxLength(200);
            entity.Property(e => e.LiveWithName4).HasMaxLength(200);
            entity.Property(e => e.LiveWithRelationship1).HasMaxLength(200);
            entity.Property(e => e.LiveWithRelationship2).HasMaxLength(200);
            entity.Property(e => e.LiveWithRelationship3).HasMaxLength(200);
            entity.Property(e => e.LiveWithRelationship4).HasMaxLength(200);
            entity.Property(e => e.ShareInfoName1).HasMaxLength(200);
            entity.Property(e => e.ShareInfoName2).HasMaxLength(200);
            entity.Property(e => e.ShareInfoName3).HasMaxLength(200);
            entity.Property(e => e.ShareInfoName4).HasMaxLength(200);
            entity.Property(e => e.ShareInfoRelationship1).HasMaxLength(200);
            entity.Property(e => e.ShareInfoRelationship2).HasMaxLength(200);
            entity.Property(e => e.ShareInfoRelationship3).HasMaxLength(200);
            entity.Property(e => e.ShareInfoRelationship4).HasMaxLength(200);
        });

        modelBuilder.Entity<Prm_PaedsFitness>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AerobicFitness).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Coordination).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CurrentLevelActivity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FlexedArmHang).HasMaxLength(100);
            entity.Property(e => e.FlexedArmHangValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HandGrip).HasMaxLength(100);
            entity.Property(e => e.HandGripValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MuscularStrength).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StandingLongJump).HasMaxLength(100);
            entity.Property(e => e.StandingLongJumpValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TenMeterShuttleRun).HasMaxLength(100);
            entity.Property(e => e.TenMeterShuttleRunValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Treadmill).HasMaxLength(100);
            entity.Property(e => e.TreadmillValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VerticalJump).HasMaxLength(100);
            entity.Property(e => e.VerticalJumpValue).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Prm_PaedsNursing>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.BaselineConcussionTesting).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BodyMassIndex).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BodyMassIndexPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BodyPart).HasMaxLength(50);
            entity.Property(e => e.DiastolicBloodPressure).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DiastolicBloodPressureRange).HasMaxLength(50);
            entity.Property(e => e.DoctorDiastolicBloodPressure).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DoctorSystolicBloodPressure).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ElectronicMediaHoursDaily).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HeartRate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Height).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HeightPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.RestingECG).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RestingElectrocardiogram).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SystolicBloodPressure).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SystolicBloodPressureRange).HasMaxLength(50);
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WeightPercentile).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Prm_PaedsPhycology>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.BascAdaptiveSkills).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BascAdaptiveSkillsPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BascBehavioralSymptomIndex).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BascBehavioralSymptomIndexPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BascExternalizingProblems).HasMaxLength(50);
            entity.Property(e => e.BascExternalizingProblemsPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BascInternalizingProblems).HasMaxLength(50);
            entity.Property(e => e.BascInternalizingProblemsPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ChildScore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ParentScore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportEmotionalSymptomIndex).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportEmotionalSymptomIndexPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportInattentionHyperactivity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportInattentionHyperactivityPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportInternalizingProblems).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportInternalizingProblemsPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportPersonalAdjustment).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportPersonalAdjustmentPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportSchoolProblems).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SelfReportSchoolProblemsPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WasiPerceptualPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WasiPerceptualTscore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WasiVerbalPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WasiVerbalTscore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WechslerReadingComprehensionSubtestTscore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WechslerWordReadingPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerMathSkillsPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerMathSkillsTscore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerReadingCompositePercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerReadingComprehensionTscore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerSpellingPercentile).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerSpellingTscore).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WecshlerWordReadingTscore).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Prm_PaedsPhycologyNote>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<Prm_StandardMedicalIssue>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.IssueMappingName).HasMaxLength(200);
            entity.Property(e => e.IssueMappingValue).HasMaxLength(200);
            entity.Property(e => e.IssueName).HasMaxLength(200);
            entity.Property(e => e.IssueTypeName).HasMaxLength(200);
            entity.Property(e => e.IssueTypeOrder).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductTypes");
            entity.HasKey(p => p.ProductTypeId);
            entity.Ignore(p => p.Id);
        });

        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.ToTable("ProductGroups");
            entity.HasKey(p => p.ProductGroupId);
            entity.Ignore(p => p.Id);
            entity.Property(e => e.ProductGroupName).HasColumnName("ProductGroup");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.ProductID);
            entity.Ignore(p => p.Id);
            entity.HasQueryFilter(p => p.BaseProductId == null && p.ModalityId > 0);
            //entity.Property(e => e.AccountId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.AccountPays);
            //entity.Property(e => e.AccpacItem)
            //    .HasMaxLength(100)
            //    .IsUnicode(false);
            //entity.Property(e => e.AppointmentTypeId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.CancelCutOff)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.ConfirmationEmailAttachment).IsUnicode(false);
            //entity.Property(e => e.ConfirmationEmailTemplateId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            entity.Property(e => e.DiscountAmount)
                .HasColumnType("decimal(18, 2");
            //entity.Property(e => e.DiscountTimeFrame)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.IsShowCHA5YearsPrice)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.ModalityId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.Name)
            //    .HasMaxLength(150)
            //    .IsUnicode(false);
            entity.Property(e => e.NewPriceEffectiveDate)
                .HasColumnType("datetime2(7)");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime2(7)");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime2(7)");
            //entity.Property(e => e.OhipCode01)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.OhipCode02)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            entity.Property(e => e.OhipFacilityFee)
                .HasColumnType("decimal(18, 2");
            //entity.Property(e => e.OhipTypeId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.PrepPackTemplateId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.ProductSubgroupId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            //entity.Property(e => e.ReminderEmailAttachment).IsUnicode(false);
            //entity.Property(e => e.ReminderEmailTemplateId)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
            entity.Property(e => e.OhipProfessionalFee)
                .HasColumnType("decimal(18, 2");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => new { e.StatusId, e.EntityTypeId });
            modelBuilder.Entity<Status>()
                .Ignore(p => p.Id);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.mytestclientStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(p => p.TransactionId);
            entity.Ignore(p => p.Id);
            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.HasMany(t => t.TransactionItems)
            .WithOne(ti => ti.Transaction)
            .HasForeignKey(ti => ti.TransactionId);


        });

        modelBuilder.Entity<TransactionItem>(entity =>
        {
            entity.HasKey(e => e.TransactionItemId);
            entity.Ignore(e => e.Id);
            entity.Property(e => e.AccessionNo).HasMaxLength(20);
            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccpacItem).HasMaxLength(100);
            entity.Property(e => e.Auxiliary).IsUnicode(false);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.NonInvoiceContent).IsUnicode(false);
            entity.Property(e => e.OhipCode01).HasMaxLength(5);
            entity.Property(e => e.OhipCode02).HasMaxLength(5);
            entity.Property(e => e.OhipCode03).HasMaxLength(5);
            entity.Property(e => e.OhipDiagnosticCode01).HasMaxLength(5);
            entity.Property(e => e.OhipDiagnosticCode02).HasMaxLength(5);
            entity.Property(e => e.OhipDiagnosticCode03).HasMaxLength(5);
            entity.Property(e => e.OhipFacilityFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OhipProfessionalFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OhipReferringProvider).HasMaxLength(100);
            entity.Property(e => e.PACSProcedureId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.HasQueryFilter(a => !a.IsDeleted);

            entity.HasOne(ti => ti.Product)
                .WithMany(p => p.TransactionItems)
                .HasForeignKey(ti => ti.ProductId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Ignore(e => e.Id);
            entity.Property(e => e.AccuroUserName).HasMaxLength(50);
            entity.Property(e => e.Credentials).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Fax).HasMaxLength(30);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.JobTitle).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.LongDistanceCode).HasMaxLength(10);
            entity.Property(e => e.Manager).HasMaxLength(200);
            entity.Property(e => e.DirectAreaCode).HasMaxLength(3);
            entity.Property(e => e.DirectPhone).HasMaxLength(8);
            entity.Property(e => e.DirectExtension).HasMaxLength(50);
            entity.Property(e => e.UserGoupd01).HasMaxLength(100);
            entity.Property(e => e.UserGroup02).HasMaxLength(100);
            entity.Property(e => e.UserGroup03).HasMaxLength(100);
            entity.Property(e => e.UserGroup04).HasMaxLength(100);
            entity.Property(e => e.UserGroup05).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(e => e.RoleId);
            entity.Ignore(e => e.Id);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");
            entity.HasKey(e => e.UserRoleId);
            entity.Ignore(e => e.Id);
        });

        modelBuilder.Entity<RoleReport>(entity =>
        {
            entity.ToTable("RoleReport");
            entity.HasKey(e => new { e.RoleID, e.Report });
            entity.Ignore(e => e.Id);
        });

        modelBuilder.Entity<AccuroLabObservation>(entity =>
        {
            entity.ToTable("Accuro_lab_observation", "cmsuploader");
            entity.HasKey(e => e.TipsObservationId);
            entity.Property(e => e.TipsObservationId).HasColumnName("tips_observation_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id").HasConversion<decimal>();
            entity.Property(e => e.ObservationNote).HasColumnName("observation_note");
            entity.Property(e => e.ResultId).HasColumnName("result_id").HasConversion<decimal>();
            entity.Property(e => e.Label).HasColumnName("label");
            entity.Property(e => e.ObservationId).HasColumnName("observation_id").HasConversion<decimal>();
            entity.Property(e => e.ObservationDate).HasColumnName("observation_date");
            entity.Property(e => e.ObservationFlag).HasColumnName("observation_flag");
            entity.Property(e => e.ObservationValue).HasColumnName("observation_value");
            entity.Property(e => e.ObservationUnits).HasColumnName("observation_units");
            entity.Property(e => e.ObservationReferenceRange).HasColumnName("observation_reference_range");
            entity.Property(e => e.OrderNum).HasColumnName("order_num").HasConversion<decimal>();
            entity.Property(e => e.ObservationalSubIdNumber).HasColumnName("observationalSubIdNumber").HasConversion<decimal>();
            entity.Property(e => e.ObsDisplayRefRange).HasColumnName("obsDisplayRefRange");
            entity.Property(e => e.ObservationalResultStatus).HasColumnName("observationalResultStatus");
            entity.Ignore(e => e.Id);
        });

        modelBuilder.Entity<AccuroLabObservationGroup>(entity =>
        {
            entity.ToTable("Accuro_lab_observation_group", "cmsuploader");
            entity.HasKey(e => e.TipsObservationGroupId);
            entity.Property(e => e.TipsObservationGroupId).HasColumnName("tips_observation_group_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id").HasConversion<decimal>();
            entity.Property(e => e.BaseGroupId).HasColumnName("base_group_id").HasConversion<decimal>();
            entity.Property(e => e.Reviewer).HasColumnName("reviewer").HasConversion<decimal>();
            entity.Property(e => e.ReviewerName).HasColumnName("reviewer_name");
            entity.Property(e => e.ReviewDate).HasColumnName("review_date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id").HasConversion<decimal>();
            entity.Property(e => e.TestId).HasColumnName("test_id").HasConversion<decimal>();
            entity.Property(e => e.SourceId).HasColumnName("source_id").HasConversion<decimal>();
            entity.Property(e => e.OrderProvider).HasColumnName("order_provider");
            entity.Property(e => e.UserNotes).HasColumnName("user_notes");
            entity.Property(e => e.UniversalSerialNum).HasColumnName("universal_serial_num");
            entity.Property(e => e.SappId).HasColumnName("sapp_id");
            entity.Property(e => e.FillerOrderNum).HasColumnName("filler_order_num");
            entity.Property(e => e.OrderGroup).HasColumnName("order_group");
            entity.Property(e => e.CollectionDate).HasColumnName("collection_date");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransferTipsDate).HasColumnName("transfer_tips_date");
            entity.Property(e => e.TestName).HasColumnName("test_name");
            entity.Property(e => e.SourceName).HasColumnName("source_name");
            entity.Property(e => e.DoNotUpload).HasColumnName("do_not_upload");
            entity.Property(e => e.DoNotUploadNote).HasColumnName("do_not_upload_note");
            entity.Property(e => e.LetterId).HasColumnName("letter_Id");
            entity.Property(e => e.UploadedToMytestclient).HasColumnName("uploaded_to_mytestclient");
            entity.Property(e => e.DoNotUploadDate).HasColumnName("do_not_upload_date");
            entity.Property(e => e.OrderProviderWithId).HasColumnName("order_provider_with_id");
            entity.Property(e => e.ActiveVersion).HasColumnName("active_version");
            entity.HasMany(e => e.Observations)
            .WithOne()
            .HasForeignKey(e => e.GroupId)
            .HasPrincipalKey(e => e.GroupId);
            entity.Ignore(e => e.Id);
        });
        modelBuilder.Entity<CMS_SyncTracking>(entity =>
        {
            entity.ToTable("CMS_SyncTrackingTable");
            entity.HasKey(e => e.Id);

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
