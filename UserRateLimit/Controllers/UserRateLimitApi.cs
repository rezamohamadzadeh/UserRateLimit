using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserRateLimit.Models;

namespace UserRateLimit.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserRateLimitApi : JsonActions
    {
        private readonly ApplicationDbContext _context;

        public IConfiguration Configuration { get; }

        public UserRateLimitApi(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;            
        }


        public async Task<IActionResult> CheckUserLimit([FromQuery] RateLimitDto model)
        {
            try
            {
                //var IP = Request.HttpContext.Connection.RemoteIpAddress;

                var result = _context.Tb_RateLimits.Where(d => d.UserIP == model.UserIP).OrderByDescending(d => d.LastLimit).FirstOrDefault();

                if (result != null)
                {
                    if (result.LastLimit > DateTime.Now.AddMinutes(-double.Parse((Configuration["RateLimitDateTime"].ToString()))))
                        return BadRequest(ForbiddenResult("You are allowed to submit a request every ten minutes"));
                }

                var rateLimit = new Tb_RateLimit()
                {
                    LastLimit = DateTime.Now,
                    UserIP = model.UserIP,
                    UserId = model.UserId
                };

                await _context.AddAsync(rateLimit);
                await _context.SaveChangesAsync();

                return Ok(SuccessResult());
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResult(ex.Message));
            }

        }
    }
}
