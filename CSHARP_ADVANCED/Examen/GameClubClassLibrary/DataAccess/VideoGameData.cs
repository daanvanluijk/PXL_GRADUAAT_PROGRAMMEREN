using GameClubClassLibrary.Entities;
using Shpielerij.Disconnected;
using Shpielerij.FileManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.DataAccess
{
    public static class VideoGameData
    {
        public static DataTable VideoGameDataTable { get; set; }
        private static List<VideoGame> videoGames = new List<VideoGame>();
        private static string[] csvLookup = new string[]
        {
            "Id",
            "Rank",
            "Source",
            "Title",
            "ReleaseYear",
            "IsSinglePlayerOnly",
            "GeekRating",
            "AverageRating",
            "NumberOfVoters",
        };

        public static void InitialiiseerVideoGameData(string vgCsvPath)
        {
            string[][] lines = FileManagement.FileAsLinesSplitByCharacter(vgCsvPath, ';', true);
            VideoGameDataTable = new DataTable();
            Disconnected.AddColumnsToDataTable(VideoGameDataTable, typeof(VideoGame));
            //VideoGameId; Rank; ImageSource; Title; ReleaseYear; SingleplayerOnly; GeekRating; AverageRating; NumVoters
            foreach (string[] row in lines)
            {
                VideoGame game = new VideoGame();
                for (int i = 0; i < csvLookup.Length; i++)
                {
                    var prop = typeof(VideoGame).GetProperty(csvLookup[i]);
                    prop.SetValue(game, Convert.ChangeType(row[i], prop.PropertyType));
                }
                videoGames.Add(game);
            }
            foreach (VideoGame game in videoGames)
            {
                DataRow row = VideoGameDataTable.NewRow();
                for (int i = 0; i < csvLookup.Length; i++)
                {
                    row[csvLookup[i]] = typeof(VideoGame).GetProperty(csvLookup[i]).GetValue(game);
                }
                VideoGameDataTable.Rows.Add(row);
            }
        }

        public static List<VideoGame> GetVideoGameList()
        {
            return videoGames;
        }

        public static VideoGame VideoGameByIndex(int index)
        {
            return videoGames[index >= 0 ? index : 0];
        }
    }
}
