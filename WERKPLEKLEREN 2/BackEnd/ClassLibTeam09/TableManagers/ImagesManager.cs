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
    public static class ImagesManager
    {
        public static readonly Dictionary<string, string[]> lookup = new Dictionary<string, string[]>()
        {
            ["ID"] = new string[3] { "@imgID", "ImgId", "ImgID" },
            ["IU"] = new string[3] { "@imageUrl", "ImageUrl", "ImageUrl" },
        };

        #region Procedures
        public static SelectResult SelectLastImage()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static SelectResult SelectImageWhereImgID(Image image)
            => BaseManager.BaseProcedure(image, "ID", Procedures.OperationType.Select, lookup);

        public static InsertResult InsertImage(Image image)
            => BaseManager.BaseProcedure(image, "IU", Procedures.OperationType.Insert, lookup);
        #endregion

        public static Image ConvertDataRowToImage(DataTable table)
            => BaseManager.ConvertTableToObject<Image>(table, lookup);
    }
}
