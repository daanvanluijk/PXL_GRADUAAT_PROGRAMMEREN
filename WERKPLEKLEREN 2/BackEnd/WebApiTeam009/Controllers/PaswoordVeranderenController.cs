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
    [Route("api/PaswoordVeranderen")]
    [ApiController]
    public class PaswoordVeranderenController : ControllerBase
    {
        [HttpPost, Route("Get")]
        public ActionResult<User> GetUser(User user)
        {
            SelectResult result = UsersManager.SelectUserWhereUserIDAndPassword(user);
            return result.Succeeded ? Ok(UsersManager.ConvertDataRowToUser(result.DataTable)) : (ActionResult<User>)Ok(result.Errors);
        }
    }
}
