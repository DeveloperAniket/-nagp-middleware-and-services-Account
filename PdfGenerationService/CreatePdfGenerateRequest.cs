using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationService
{
    public class CreatePdfGenerateEventRequest
    {
        public required int AccountNumber { get; set; }

        public required Guid RequestId { get; set; }
    }
}