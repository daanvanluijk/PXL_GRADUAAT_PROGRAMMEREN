using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ClassLibTeam09.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ClassLibTeam09.Settings;
using ClassLibTeam09.TableManagers;
using ClassLibTeam09.Data.Framework;

namespace WebApiTeam009.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCardController : ControllerBase
    {
        [HttpGet]
        public ActionResult<RoomImage[]> GetAllRooms()
        {
            SelectResult result = RoomImagesManager.SelectRoomImages();
            return result.Succeeded ? RoomImagesManager.ConvertTableToRoomImages(result.DataTable) : Ok(result.Errors);
        }
    }
}
