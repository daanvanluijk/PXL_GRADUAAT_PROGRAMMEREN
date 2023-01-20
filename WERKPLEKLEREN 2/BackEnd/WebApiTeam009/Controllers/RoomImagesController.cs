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
    public class RoomImagesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<RoomImage[]> GetAllImages()
        {
            SelectResult result = RoomImagesManager.SelectRoomImages();
            return result.Succeeded ? RoomImagesManager.ConvertTableToRoomImages(result.DataTable) : Ok(result.Errors);
        }
    }
}
