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
    [Route("api/PaswoordBevestigen")]
    [ApiController]
    public class PaswoordBevestigenController : ControllerBase
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
            if (!validateResult.Succeeded)
            {
                return Ok(validateResult.Errors);
            }

            UpdateResult updateResult = UsersManager.UpdateUserWhereUserID(user);
            return Ok(updateResult);
        }
    }
}
