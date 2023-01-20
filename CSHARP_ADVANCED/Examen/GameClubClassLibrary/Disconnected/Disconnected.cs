using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.Disconnected
{
    public static class Disconnected
    {
        public static void AddColumnsToDataTable(DataTable table, Dictionary<string, Type> columnNameTypePairs)
        {
            foreach (KeyValuePair<string, Type> pair in columnNameTypePairs)
            {
                DataColumn column = new DataColumn(pair.Key, pair.Value);
                table.Columns.Add(column);
            }
        }

        public static void AddColumnsToDataTable(DataTable table, Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                DataColumn column = new DataColumn(prop.Name, prop.PropertyType);
                table.Columns.Add(column);
            }
        }

        public static void AddObjectsAsRowsToDataTable(DataTable table, dynamic[] objects)
        {
            foreach (dynamic _object in objects)
            {
                AddObjectAsRow(table, _object);
            }
        }

        public static void AddListAsRowsToDataTable(DataTable table, List<dynamic> list)
        {
            foreach (dynamic _object in list)
            {
                AddObjectAsRow(table, _object);
            }
        }

        public static void AddStringArraysAsRowsToDataTable(DataTable table, string[][] rows)
        {
            foreach (string[] row in rows)
            {
                DataRow dataRow = table.NewRow();
                int i = 0;
                foreach (string prop in row)
                {
                    dataRow[i] = Convert.ChangeType(prop, table.Columns[i].DataType);
                    i++;
                }
                table.Rows.Add(row);
            }
        }

        private static void AddObjectAsRow(DataTable table, dynamic _object)
        {
            DataRow row = table.NewRow();
            int i = 0;
            Type test = _object.GetType();
            foreach (var prop in _object.GetType().GetProperties())
            {
                row[i] = prop.GetValue(_object);
                i++;
            }
            table.Rows.Add(row);
        }
    }
}
