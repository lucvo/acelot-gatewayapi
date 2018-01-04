using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CustomerServices.Controllers
{
    [Route("")]
    [Authorize]
    public class CustomersController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
			return new string[] { "Catcher Wong", "James Li" };
		}

        // GET api/values/5
        [HttpGet("[controller]/{id}")]
        public string Get(int id)
        {
			return $"Catcher Wong - {id}";
		}

        
    }
}
