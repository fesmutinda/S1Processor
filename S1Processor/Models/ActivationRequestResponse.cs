using S1Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1Processor.Models
{
    public class ActivationRequestResponse
    {
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public List<ActivationRequestsItem> ActivationRequests { get; set; }
    }
}
