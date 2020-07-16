using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRateLimit.Models
{
    public class RateLimitDto
    {
        public string UserIP { get; set; }

        public string UserId { get; set; }
    }
}
