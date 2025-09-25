using TestManager.Domain.Model.UserManagement;
namespace TestManager.Domain.Model
{
    public partial class Note : BaseEntity<int>
    {
        public int NoteID { get; set; }
        public int EntityTypeID { get; set; }
        public int InstanceID {get; set; }
        public string? Text { get; set; }
        public int? UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int NoteType { get; set; }
        public int Type { get; set; }
        public string? ExtraInfo { get; set; }
        public string? ExtraInfo2 { get; set; }

        public Appointment? Appointment { get; set; }

        public User? User { get; set; }
    }
}
