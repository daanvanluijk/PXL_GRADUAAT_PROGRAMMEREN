using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Entities
{
   public class RoomImage
    {
        public int RoomId { get; set; }
        public int ImgId { get; set; }
        public string RoomTitle { get; set; }
        public byte[]  ImageUrl { get; set; }
        public string RoomDescription { get; set; }
        public double RoomPrice { get; set; }
        public int RoomCapacity { get; set; }
    }
}
