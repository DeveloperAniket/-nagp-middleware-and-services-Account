﻿namespace AccountService.Dtos
{
    public class CreatePdfGenerateEventRequest
    {
        public required int AccountNumber { get; set; }

        public required Guid RequestId { get; set; }
    }
}
