using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Entities
{
    public class Room
    {
        public int RoomID { get; set; }
        public int RoomPrice { get; set; }
        public int RoomCapacity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
