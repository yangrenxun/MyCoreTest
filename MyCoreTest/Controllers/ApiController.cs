using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyCoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            var result = Add(2,3);
            return new JsonResult("OK");
        }

        public int Add(int a, int b) => a + b;

    }
}