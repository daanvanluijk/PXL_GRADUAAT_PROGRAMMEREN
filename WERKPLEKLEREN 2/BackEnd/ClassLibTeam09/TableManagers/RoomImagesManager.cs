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
    public static class RoomImagesManager
    {
        public static readonly Dictionary<string, string[]> lookup = new Dictionary<string, string[]>()
        {
            ["RID"] = new string[3] { "@roomID", "RoomId", "roomID" },
            ["IID"] = new string[3] { "@imgID", "ImgId", "ImgID" },
            ["RT"] = new string[3] { "@roomtitle", "RoomTitle", "title" },
            ["IU"] = new string[3] { "@imageurl", "ImageUrl", "ImageUrl" },
            ["RD"] = new string[3] { "@roomdescription", "RoomDescription", "description" },
            ["RP"] = new string[3] { "@roomprice", "RoomPrice", "roomPrice" },
            ["RC"] = new string[3] { "@roomcapacity", "RoomCapacity", "roomCapacity" },
        };

        #region Procedures
        public static SelectResult SelectRoomImages()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static SelectResult SelectRoomImageWhereRoomID(RoomImage roomImage)
            => BaseManager.BaseProcedure(roomImage, "RID", Procedures.OperationType.Select, lookup);

        public static InsertResult InsertRoomImage(RoomImage roomImage)
            => BaseManager.BaseProcedure(roomImage, "RID,IID", Procedures.OperationType.Insert, lookup);

        public static DeleteResult DeleteRoomImageWhereImgID(RoomImage roomImage)
            => BaseManager.BaseProcedure(roomImage, "IID", Procedures.OperationType.Delete, lookup);
        #endregion

        // Neemt een tabel en converteert deze naar een array van RoomImage objecten op basis van de lookup dictionary
        public static RoomImage[] ConvertTableToRoomImages(DataTable table)
            => BaseManager.ConvertTableToObjects<RoomImage>(table, lookup);
    }
}
