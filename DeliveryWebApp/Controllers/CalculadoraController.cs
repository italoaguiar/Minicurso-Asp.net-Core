using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculadoraController : ControllerBase
    {
        [HttpGet("{a}/{b}")]
        public ActionResult<double> Soma(double a, double b)
        {
            return a + b;
        }

        [HttpGet("ParesEntre/{a}/{b}")]
        public ActionResult<List<int>> ParesEntre(int a, int b)
        {
            List<int> list = new List<int>();
            for(int i = 0; i<b; i++)
            {
                if(i%2 == 0)
                {
                    list.Add(i);
                }
            }
            return list;
        }

    }
}