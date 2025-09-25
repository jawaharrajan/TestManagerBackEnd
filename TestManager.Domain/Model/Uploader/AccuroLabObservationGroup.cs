namespace TestManager.Domain.Model.Uploader
{
    public class AccuroLabObservationGroup : BaseEntity<int>
    {
        public int TipsObservationGroupId { get; set; } // [tips_observation_group_id]
        public long? GroupId { get; set; } // [group_id]
        public long? BaseGroupId { get; set; } // [base_group_id]
        public long? Reviewer { get; set; } // [reviewer]
        public string? ReviewerName { get; set; } // [reviewer_name]
        public DateTime? ReviewDate { get; set; } // [review_date]
        public long? PatientId { get; set; } // [patient_id]
        public long? TestId { get; set; } // [test_id]
        public long? SourceId { get; set; } // [source_id]
        public string? OrderProvider { get; set; } // [order_provider]
        public string? UserNotes { get; set; } // [user_notes]
        public string? UniversalSerialNum { get; set; } // [universal_serial_num]
        public string? SappId { get; set; } // [sapp_id]
        public string? FillerOrderNum { get; set; } // [filler_order_num]
        public string? OrderGroup { get; set; } // [order_group]
        public DateTime? CollectionDate { get; set; } // [collection_date]
        public DateTime? TransactionDate { get; set; } // [transaction_date]
        public DateTime? TransferTipsDate { get; set; } // [transfer_tips_date]
        public string? TestName { get; set; } // [test_name]
        public string? SourceName { get; set; } // [source_name]
        public bool? DoNotUpload { get; set; } // [do_not_upload]
        public string? DoNotUploadNote { get; set; } // [do_not_upload_note]
        public int? LetterId { get; set; } // [letter_Id]
        public DateTime? UploadedToMytestclient { get; set; } // [uploaded_to_mytestclient]
        public DateTime? DoNotUploadDate { get; set; } // [do_not_upload_date]
        public string? OrderProviderWithId { get; set; } // [order_provider_with_id]
        public bool? ActiveVersion { get; set; } // [active_version]
        public ICollection<AccuroLabObservation> Observations { get; set; } = [];
    }
}
