using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibTeam09.Entities;
using System.Data;
using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.TableManagers;

namespace WebApiTeam009.Controllers
{
    [ApiController]
    [Route("api/Profile")]
    public class ProfileController : ControllerBase
    {
        [HttpPost, Route("Get")]
        public ActionResult<User> GetUser(User user)
        {
            SelectResult result = UsersManager.SelectUserWhereUserIDAndPassword(user);
            return result.Succeeded ? Ok(UsersManager.ConvertDataRowToUser(result.DataTable)) : (ActionResult<User>)Ok(result.Errors);
        }

        [HttpPost("{id}")]
        public ActionResult<User> UpdateUser(User user, int ID)
        {
            user.UserId = ID;

            user = UsersManager.NormaliseUser(user);

            ValidateResult validateResult = UsersManager.ValidateUser(user, false);
            if (!validateResult.Succeeded) return Ok(validateResult.Errors);

            UpdateResult updateResult = UsersManager.UpdateUserWhereUserID(user);
            return Ok(updateResult);
        }
    }
}
