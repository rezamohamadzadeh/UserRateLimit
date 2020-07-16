using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRateLimit.Models
{
    public class Tb_RateLimit
    {
        public Tb_RateLimit()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string UserIP { get; set; }

        public string UserId { get; set; }

        public DateTime LastLimit { get; set; }
    }
}
