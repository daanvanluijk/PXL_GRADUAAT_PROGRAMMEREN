using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;
using ClassLibTeam09.TableManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTeam009.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetUsers()
        {
            return Ok("get test");
        }

        [HttpPost] // ontvangt
        public ActionResult<User> LoginUser(User user)
        {
            SelectResult result = UsersManager.SelectUserWhereEmailAndPassword(user);
            return Ok(result.Succeeded && result.DataTable.Rows.Count > 0 ? UsersManager.ConvertDataRowToUser(result.DataTable) : "Gebruiker niet gevonden!");
        }


    }
}
