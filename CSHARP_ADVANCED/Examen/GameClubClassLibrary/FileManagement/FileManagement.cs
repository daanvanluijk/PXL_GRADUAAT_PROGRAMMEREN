using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.FileManagement
{
    public static class FileManagement
    {
        #region FileOpenBrol
        public static string FilePath(string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            OpenFileDialog fileDialog = OpenFile(initialDirectory, filters);
            return fileDialog.FileName;
        }

        public static string FileAsString(string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            OpenFileDialog fileDialog = OpenFile(initialDirectory, filters);
            return fileDialog.FileName != string.Empty ? File.ReadAllText(fileDialog.FileName) : string.Empty;
        }

        public static string[] FileAsLines(string initialDirectory = null, Dictionary<string, string> filters = null, bool skipFirst = false)
        {
            OpenFileDialog fileDialog = OpenFile(initialDirectory, filters);
            string[] lines = File.ReadAllLines(fileDialog.FileName);
            if (skipFirst) lines = RemoveFirstLine(lines);
            return lines;
        }

        public static string[][] FileAsLinesSplitByCharacter(char character, string initialDirectory = null, Dictionary<string, string> filters = null, bool skipFirst = false)
            => FileAsLinesSplitByCharacter(OpenFile(initialDirectory, filters).FileName, character, skipFirst);

        public static string[][] FileAsLinesSplitByCharacter(string path, char character, bool skipFirst = false)
        {
            string[] lines = File.ReadAllLines(path);
            if (skipFirst) lines = RemoveFirstLine(lines);
            string[][] result = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].Split(character);
            }
            return result;
        }

        public static string[][] FileAsLinesSplitByString(string _string, string initialDirectory = null, Dictionary<string, string> filters = null, bool skipFirst = false)
        {
            OpenFileDialog fileDialog = OpenFile(initialDirectory, filters);
            string[] lines = File.ReadAllLines(fileDialog.FileName);
            if (skipFirst) lines = RemoveFirstLine(lines);
            string[][] result = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].Split(new string[] { _string }, StringSplitOptions.None);
            }
            return result;
        }
        #endregion

        #region FileOpslaanBrol
        public static void SaveObjectsAsFile(object[][] objects, char character, string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            string _string = string.Empty;
            foreach (object[] o in objects)
            {
                foreach (object s in o)
                {
                    _string += s.ToString() + character;
                }
                _string = _string.Substring(0, _string.Length - 1);
                _string += "\n";
            }
            SaveFileDialog fileDialog = SaveFile(initialDirectory, filters);
            SaveString(_string, fileDialog);
        }

        public static void SaveTableAsXML(DataTable table, string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            SaveFileDialog fileDialog = SaveFile(initialDirectory, filters);
            table.WriteXml(fileDialog.FileName);
        }

        public static void SaveDataSetAsXML(DataSet set, string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            SaveFileDialog fileDialog = SaveFile(initialDirectory, filters);
            set.WriteXml(fileDialog.FileName);
        }

        public static void SaveObjectAsFile(object _object, char character, string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            string _string = string.Empty;
            SaveFileDialog fileDialog = SaveFile(initialDirectory, filters);
            SaveString(_string, fileDialog);
        }
        #endregion

        private static OpenFileDialog OpenFile(string initialDirectory = null, Dictionary<string, string> filters = null)
            => (OpenFileDialog)ConfigureFileDialog(new OpenFileDialog(), initialDirectory, filters);

        private static SaveFileDialog SaveFile(string initialDirectory = null, Dictionary<string, string> filters = null)
            => (SaveFileDialog)ConfigureFileDialog(new SaveFileDialog(), initialDirectory, filters);

        private static void SaveString(string _string, SaveFileDialog fileDialog)
        {
            File.WriteAllText(fileDialog.FileName, _string);
        }

        private static dynamic ConfigureFileDialog(FileDialog fileDialog, string initialDirectory = null, Dictionary<string, string> filters = null)
        {
            if (initialDirectory != null) fileDialog.InitialDirectory = initialDirectory;
            if (filters != null)
            {
                string filterString = "";
                foreach (KeyValuePair<string, string> filter in filters)
                {
                    filterString += $"{filter.Key} ({filter.Value})|";
                    filterString += $"{filter.Value}|";
                }
                fileDialog.Filter = filterString.Substring(0, filterString.Length - 1);
            }
            fileDialog.ShowDialog();
            return fileDialog;
        }

        private static string[] RemoveFirstLine(string[] strings)
        {
            List<string> list = strings.ToList<string>();
            list.RemoveAt(0);
            return list.ToArray();
        }
    }
}
