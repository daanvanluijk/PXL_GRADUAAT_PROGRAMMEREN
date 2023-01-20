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
    [Route("api/DetailsKamer")]
    [ApiController]
    public class DetailsKamerController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Order> AddOrder(Order order)
        {
            SelectResult result = OrdersManager.SelectAvailabiltyOfOrderWhereRoomIDAndCheckinDateAndCheckoutDate(order);
            if (!result.Succeeded) return Ok("kapot!!");
            if ((int)result.DataTable.Rows[0][0] == 0) return Ok("Kamer is niet beschikbaar!");

            OrdersManager.InsertOrderNotYetPaid(order);

            result = OrdersManager.SelectOrderWhereUserIDCheckInDateCheckoutDateAdultAmountChildrenAmount(order);
            Order[] orders = OrdersManager.ConvertTableToOrders(result.DataTable);
            order.OrderID = orders[0].OrderID;

            OrdersManager.InsertRoomOrder(order);

            return Ok(order);
        }
    }
}
