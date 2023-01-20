using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Oeps, iemand heeft hier roomID ook in gepropt
// En nu moet dit er in blijven staan voor backwards compatibility
// Ach ja
namespace ClassLibTeam09.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int ChildrenAmount { get; set; }
        public int AdultAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public double RoomPrice { get; set; }
        public string RoomTitle { get; set; }

    }
}
