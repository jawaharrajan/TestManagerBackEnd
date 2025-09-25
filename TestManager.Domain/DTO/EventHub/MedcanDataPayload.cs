using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.EventHub
{
    public class testclientDataPayload
    {
        public int actionid { get; set; }
        public int? entityid { get; set; } = 0;
        public string? entitytype { get; set; } = string.Empty;
        public string? data { get; set; }
        public int? subentityid { get; set; } = 0;
        public string? subentitytype { get; set; }
        public string? _comment { get; set; }
    }

    public class testclientProductJoinDataPayload
    {
        public int actionid { get; set; }
        public int? entityid { get; set; } = 0;
        public string? entitytype { get; set; } = string.Empty;
        public string? data { get; set; }
        public string? subentityid { get; set; } = string.Empty;
        public string? subentitytype { get; set; }
        public string? _comment { get; set; }
    }
}
