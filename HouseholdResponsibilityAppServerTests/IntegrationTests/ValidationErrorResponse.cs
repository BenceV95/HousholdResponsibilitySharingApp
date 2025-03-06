using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.ResponseModels
{
    public class ValidationErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
