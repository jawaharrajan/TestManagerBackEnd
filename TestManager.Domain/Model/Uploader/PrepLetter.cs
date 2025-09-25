using System;
using System.Collections.Generic;

namespace TestManager.Domain.Model.Uploader;

public partial class PrepLetter : BaseEntity<int>
{
    public int LetterId { get; set; }

    public int? LetterTypeId { get; set; }

    public string? LetterName { get; set; }

    public string? Body { get; set; }

    public int? ReferralId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public bool? OpenLetter { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? AvailableOnMytestclient { get; set; }
    
    public bool? IsAccessible { get; set; }

    public bool? SentToCrawford { get; set; }

    public bool? ReceivedFromCrawford { get; set; }
    public DateTime? ViewedDate { get; set; }
    public ICollection<PrepAttachment> Attachments { get; set; }
    
}
