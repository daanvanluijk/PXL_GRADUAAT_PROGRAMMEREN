using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ClassLibTeam09.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ClassLibTeam09.Settings;
using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.TableManagers;

namespace WebApiTeam009.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetaalController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Order[]> GetAllOrders()
        {
            return Ok("get test");
        }

        [HttpPost]
        public ActionResult<Order[]> UpdateOrdersPaid(Order order)
        {
            UpdateResult result = OrdersManager.UpdateOrdersPaidWhereUserID(order);
            return Ok(result.Succeeded);
        }
    }
}
