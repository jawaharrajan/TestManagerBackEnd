using System.Text.Json.Serialization;

namespace TestManager.Domain.Model.EventHubModels
{

    public class CdcEntityStatus
    {
        [JsonPropertyName("__$start_lsn")]
        public string StartLsn { get; set; }

        [JsonPropertyName("__$seqval")]
        public string SeqVal { get; set; }

        [JsonPropertyName("__$operation")]
        public int Operation { get; set; }

        [JsonPropertyName("__$update_mask")]
        public string UpdateMask { get; set; }

        public int EntityStatusID { get; set; }
        public int EntityTypeID { get; set; }
        public int InstanceID { get; set; }
        public int StatusID { get; set; }
        public DateTime? CustomDate { get; set; }
        public int? WorkflowInstanceID { get; set; }
        public int? WorkflowID { get; set; }
        public string TableName { get; set; }
    }

}
