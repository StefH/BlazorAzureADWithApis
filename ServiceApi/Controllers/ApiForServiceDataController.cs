using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ServiceApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ApiForServiceDataController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string> { "app-app Service API data 1", "service API data 2" + DateTime.Now };
        }
    }
}
