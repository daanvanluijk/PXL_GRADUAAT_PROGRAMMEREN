using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;
using ClassLibTeam09.Settings;
using ClassLibTeam09.TableManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTeam009.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IconsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<RoomBedtype[]> GetAllIcons()
        {
            SelectResult result = RoomBedtypesManager.SelectRoomBedtypes();
            return result.Succeeded ? RoomBedtypesManager.ConvertTableToRoomBedtypes(result.DataTable) : Ok(result.Errors);
        }
    }
}
