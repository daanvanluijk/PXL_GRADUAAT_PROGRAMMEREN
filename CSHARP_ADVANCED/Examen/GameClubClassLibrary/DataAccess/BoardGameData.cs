using GameClubClassLibrary.Entities;
using Shpielerij.Disconnected;
using Shpielerij.FileManagement;
using Shpielerij.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.DataAccess
{
    public static class BoardGameData
    {
        public static DataTable BoardGameDataTable { get; set; }
        private static List<BoardGame> boardGames = new List<BoardGame>();
        public static List<BoardGame> LinqBoardGames { get; set; }
        private static string[] csvLookup = new string[]
        {
            "Id",
            "Rank",
            "Source",
            "Title",
            "ReleaseYear",
            "Description",
            "GeekRating",
            "AverageRating",
            "NumberOfVoters",
            "DistributorPrice",
        };

        public static void InitialiiseerBoardGameData(string bgCsvPath)
        {
            string[][] lines = FileManagement.FileAsLinesSplitByCharacter(bgCsvPath, ';', true);
            BoardGameDataTable = new DataTable();
            Disconnected.AddColumnsToDataTable(BoardGameDataTable, typeof(BoardGame));
            //BoardGameId;Rank;ImageSource;Title;ReleaseYear;Description;GeekRating;AvgerageRating;NumVoters;DistributorPrice
            foreach (string[] row in lines)
            {
                BoardGame game = new BoardGame();
                for (int i = 0; i < csvLookup.Length; i++)
                {
                    var prop = typeof(BoardGame).GetProperty(csvLookup[i]);
                    prop.SetValue(game, Convert.ChangeType(row[i], prop.PropertyType));
                }
                boardGames.Add(game);
            }
            LinqBoardGames = boardGames;
            foreach (BoardGame game in boardGames)
            {
                DataRow row = BoardGameDataTable.NewRow();
                for (int i = 0; i < csvLookup.Length; i++)
                {
                    row[csvLookup[i]] = typeof(BoardGame).GetProperty(csvLookup[i]).GetValue(game);
                }
                BoardGameDataTable.Rows.Add(row);
            }
        }

        public static List<BoardGame> GetBoardGameList()
        {
            return boardGames;
        }

        public static BoardGame BoardGameByIndex(int index)
        {
            return LinqBoardGames[index >= 0 ? index : 0];
        }
    }
}
