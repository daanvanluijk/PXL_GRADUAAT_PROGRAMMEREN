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
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Order[]> GetAllOrders()
        {
            SelectResult result = OrdersManager.SelectOrders();
            return result.Succeeded ? OrdersManager.ConvertTableToOrders(result.DataTable) : Ok(result.Errors);
        }

        [HttpPost, Route("NotPaid")]
        public ActionResult<Order[]> GetOrdersWhereUserID(Order order)
        {
            SelectResult result = OrdersManager.SelectOrdersWhereLessThan24hAndNotYetPaidAndUserID(order);
            return result.Succeeded ? OrdersManager.ConvertTableToOrders(result.DataTable) : Ok(result.Errors);
        }

        [HttpPost, Route("Paid")]
        public ActionResult<Order[]> GetOrdersWherePaidAndWhereUserID(Order order)
        {
            SelectResult result = OrdersManager.SelectOrdersWherePaidAndUserID(order);
            return result.Succeeded ? OrdersManager.ConvertTableToOrders(result.DataTable) : Ok(result.Errors);
        }

        [HttpPost, Route("NotPaidCount")]
        public ActionResult<object> GetOrderCountWhereUserID(Order order)
        {
            SelectResult result = OrdersManager.SelectAmountOfOrdersWhereLessThan24hAndNotYetPaidAndUserID(order);
            return result.Succeeded ? (int)result.DataTable.Rows[0][0] : Ok(result.Errors);
        }
    }
}
