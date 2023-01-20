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
    public static class RoomBedtypesManager
    {
        public static readonly Dictionary<string, string[]> lookup = new Dictionary<string, string[]>()
        {
            ["ID"] = new string[3] { "@roomID", "RoomID", "roomID" },
            ["BID"] = new string[3] { "@bedtypeID", "BedtypeId", "bedtypeID"},
            ["IU"] = new string[3] { "@iconurl", "IconsUrl", "iconUrl" },
            ["BN"] = new string[3] { "@bedtypename", "BedTypeName", "bedtypeName" },
        };

        #region Procedures
        public static SelectResult SelectRoomBedtypes()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static SelectResult SelectBedtypeIDWhereRoomID(RoomBedtype roomBedtype)
            => BaseManager.BaseProcedure(roomBedtype, "ID", Procedures.OperationType.Select, lookup);

        public static InsertResult InsertRoomBedtype(RoomBedtype roomBedtype)
            => BaseManager.BaseProcedure(roomBedtype, "ID,BID", Procedures.OperationType.Insert, lookup);

        public static DeleteResult DeleteRoomBedtypeWhereRoomID(RoomBedtype roomBedtype)
            => BaseManager.BaseProcedure(roomBedtype, "ID", Procedures.OperationType.Delete, lookup);
        #endregion

        // Neemt een tabel en converteert deze naar een array van RoomBedtype objecten op basis van de lookup dictionary
        public static RoomBedtype[] ConvertTableToRoomBedtypes(DataTable table)
            => BaseManager.ConvertTableToObjects<RoomBedtype>(table, lookup);
    }
}
