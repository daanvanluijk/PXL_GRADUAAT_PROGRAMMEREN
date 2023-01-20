using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;
using ClassLibTeam09.Settings;
using ClassLibTeam09.TableManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTeam009.Controllers
{
    [Route("api/DeleteOrder")]
    [ApiController]
    public class DeleteOrderController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Order[]> DeleteOrder(Order order)
        {
            DeleteResult result = OrdersManager.DeleteOrderWhereOrderID(order);
            return result.Succeeded ? Ok(order) : Ok(result.Errors);
        }



    }
}
