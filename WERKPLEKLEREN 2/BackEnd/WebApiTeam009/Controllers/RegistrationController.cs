using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibTeam09.Entities;
using System.Data;
using ClassLibTeam09.Data;
using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.TableManagers;

namespace WebApiTeam009.Controllers
{
    [ApiController]
    [Route("api/Registration")]
    public class RegistrationController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetUsers()
        {
            return Ok("get test");
        }

        [HttpPost]
        public ActionResult<User> AddUser(User user)
        {
            user = UsersManager.NormaliseUser(user);

            ValidateResult validateResult = UsersManager.ValidateUser(user);
            if (!validateResult.Succeeded) return Ok(validateResult.Errors.First());

            SelectResult selectResult = UsersManager.SelectAmountOfEmailsWhereEmail(user);
            if ((int)selectResult.DataTable.Rows[0][0] > 0) return Ok("Email bestaat al");

            InsertResult insertResult = UsersManager.InsertUser(user);

            selectResult = UsersManager.SelectUserWhereEmail(user);
            return selectResult.Succeeded ? Ok(UsersManager.ConvertDataRowToUser(selectResult.DataTable)) : (ActionResult<User>)Ok(selectResult.Errors);
        }
    }
}
