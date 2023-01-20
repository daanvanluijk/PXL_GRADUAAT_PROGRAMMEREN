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
    public static class RoomsManager
    {
        public static readonly Dictionary<string, string[]> lookup = new Dictionary<string, string[]>()
        {
            ["ID"] = new string[3] { "@roomID", "RoomID", "roomID" },
            ["RP"] = new string[3] { "@roomprice", "RoomPrice", "roomPrice" },
            ["RC"] = new string[3] { "@roomcapacity", "RoomCapacity", "roomCapacity" },
            ["T"] = new string[3] { "@title", "Title", "title" },
            ["D"] = new string[3] { "@description", "Description", "description" },
        };

        #region Procedures
        public static SelectResult SelectRooms()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static InsertResult InsertRoom(Room room)
            => BaseManager.BaseProcedure(room, "RP,RC,T,D", Procedures.OperationType.Insert, lookup);

        public static UpdateResult UpdateRoomWhereRoomID(Room room)
            => BaseManager.BaseProcedure(room, "ID,RP,RC,T,D", Procedures.OperationType.Update, lookup);

        public static DeleteResult DeleteRoomWhereRoomID(Room room)
            => BaseManager.BaseProcedure(room, "ID", Procedures.OperationType.Delete, lookup);
        #endregion

        // Neemt een tabel en converteert deze naar een Room object op basis van de lookup dictionary
        public static Room ConvertDataRowToRoom(DataTable table)
            => BaseManager.ConvertTableToObject<Room>(table, lookup);
    }
}
