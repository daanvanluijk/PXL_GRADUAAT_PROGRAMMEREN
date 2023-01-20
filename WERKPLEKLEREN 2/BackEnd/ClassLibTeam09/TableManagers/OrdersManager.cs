using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.TableManagers
{
    public static class OrdersManager
    {
        public static readonly Dictionary<string, string[]> lookup = new Dictionary<string, string[]>()
        {
            ["OID"] = new string[3] { "@orderID", "OrderID", "orderID" },
            ["UID"] = new string[3] { "@userID", "UserID", "userID" },
            ["RID"] = new string[3] { "@roomID", "RoomID", "roomID" },
            ["CID"] = new string[3] { "@checkindate", "CheckinDate", "checkinDate" },
            ["COD"] = new string[3] { "@checkoutdate", "CheckoutDate", "checkoutDate" },
            ["AA"] = new string[3] { "@adultamount", "AdultAmount", "adultAmount" },
            ["CA"] = new string[3] { "@childrenamount", "ChildrenAmount", "childrenAmount" },
            ["PD"] = new string[3] { "@paymentdate", "PaymentDate", "paymentDate" },
            ["RP"] = new string[3] { "@roomprice", "RoomPrice", "roomPrice" },
            ["RT"] = new string[3] { "@roomtitle", "RoomTitle", "title" },
        };

        #region Procedures
        public static SelectResult SelectOrders()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static SelectResult SelectUnavailableOrdersWithMultiverseTimeTravel()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static SelectResult SelectOrderWhereUserIDCheckInDateCheckoutDateAdultAmountChildrenAmount(Order order)
            => BaseManager.BaseProcedure(order, "UID,CID,COD,AA,CA", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectOrdersWherePaidAndUserID(Order order)
            => BaseManager.BaseProcedure(order, "UID", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectOrdersWhereLessThan24hAndNotYetPaidAndUserID(Order order)
            => BaseManager.BaseProcedure(order, "UID", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectAmountOfOrdersWhereLessThan24hAndNotYetPaidAndUserID(Order order)
            => BaseManager.BaseProcedure(order, "UID", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectAvailabiltyOfOrderWhereRoomIDAndCheckinDateAndCheckoutDate(Order order)
            => BaseManager.BaseProcedure(order, "RID,CID,COD", Procedures.OperationType.Select, lookup);

        public static UpdateResult UpdateOrdersPaidWhereUserID(Order order)
            => BaseManager.BaseProcedure(order, "UID", Procedures.OperationType.Update, lookup);

        public static DeleteResult DeleteOrderWhereOrderID(Order order)
            => BaseManager.BaseProcedure(order, "OID", Procedures.OperationType.Delete, lookup);

        public static InsertResult InsertOrder(Order order)
            => BaseManager.BaseProcedure(order, "UID,CID,COD,AA,CA", Procedures.OperationType.Insert, lookup);

        public static InsertResult InsertOrderNotYetPaid(Order order)
            => BaseManager.BaseProcedure(order, "UID,CID,COD,AA,CA,PD", Procedures.OperationType.Insert, lookup);

        public static InsertResult InsertRoomOrder(Order order)
            => BaseManager.BaseProcedure(order, "OID,RID", Procedures.OperationType.Insert, lookup);
        #endregion

        // Neemt een tabel en converteert deze naar een array van Order objecten op basis van de lookup dictionary
        public static Order[] ConvertTableToOrders(DataTable table)
            => BaseManager.ConvertTableToObjects<Order>(table, lookup);
    }
}
