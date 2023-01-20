using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string PlaceName { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
    }
}
