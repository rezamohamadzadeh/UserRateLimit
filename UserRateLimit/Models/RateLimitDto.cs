using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserRateLimit.Models
{
    public class RateLimitDto
    {
        [Display(Name = "UserIP")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string UserIP { get; set; }

        public string UserId { get; set; }
    }
}
