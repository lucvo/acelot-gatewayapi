using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProductServices.Controllers
{
    [Route("")]
    [Authorize]
    public class ProductsController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
			return new string[] { "Surface Book 2", "Mac Book Pro" };
		}
    }
}
