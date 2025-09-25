﻿namespace TestManager.Domain.DTO.Email
{
    public class EmailDTO
    {
        public required string To { get; set; }
        public required string FromEmail { get; set; }
        public required string FromName { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}
