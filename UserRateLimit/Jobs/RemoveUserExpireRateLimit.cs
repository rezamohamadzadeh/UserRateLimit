using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRateLimit.Jobs
{
    [DisallowConcurrentExecution]
    public class RemoveUserExpireRateLimit : IJob
    {
        public IConfiguration Configuration { get; }

        public RemoveUserExpireRateLimit(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public Task Execute(IJobExecutionContext context)
        {
            var option = new DbContextOptionsBuilder<ApplicationDbContext>();
            option.UseSqlite(Configuration.GetConnectionString("RateLimit"));
            using (ApplicationDbContext _context = new ApplicationDbContext(option.Options))
            {
                var userLimits = _context.Tb_RateLimits.Where(d => d.LastLimit < DateTime.Now.AddMinutes(-double.Parse((Configuration["RateLimitDateTime"].ToString())))).ToList();
                if (userLimits.Count > 0)
                {
                    _context.RemoveRange(userLimits);
                    _context.SaveChanges();
                }
            }
            return Task.CompletedTask;

        }
    }
}
