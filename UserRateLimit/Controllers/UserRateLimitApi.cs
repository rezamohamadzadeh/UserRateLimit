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

        /// <summary>
        /// check user black list state in db by ip
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult CheckUserLimit([FromQuery] RateLimitDto model)
        {
            try
            {
                HttpContext.Response.ContentType = "application/json";

                //var IP = Request.HttpContext.Connection.RemoteIpAddress;
                if (!ModelState.IsValid)
                    return BadRequest(WarningResult(ModelState));

                var result = _context.Tb_RateLimits.Where(d => d.UserIP == model.UserIP).OrderByDescending(d => d.LastLimit).FirstOrDefault();

                if (result != null)
                {
                    if (result.LastLimit > DateTime.Now.AddMinutes(-double.Parse((Configuration["RateLimitDateTime"].ToString()))))
                        return BadRequest(ForbiddenResult("You are allowed to submit a request every ten minutes"));
                }
                return Ok(SuccessResult());
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResult(ex.Message));
            }

        }

        /// <summary>
        /// Set user by ip and id in Black list db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<IActionResult> SetUserInBlackList([FromQuery] RateLimitDto model)
        {
            try
            {
                HttpContext.Response.ContentType = "application/json";

                if (!ModelState.IsValid)
                    return BadRequest(WarningResult(ModelState));

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


        ~UserRateLimitApi()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
