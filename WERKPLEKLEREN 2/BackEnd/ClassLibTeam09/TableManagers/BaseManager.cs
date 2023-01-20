using ClassLibTeam09.Data.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.TableManagers
{
    internal static class BaseManager
    {
        // BaseProcedure, deze method neemt de initialen van de parameters die in de stored procedure gebruikt moeten worden
        // Ook neemt de functie de "operationtype" om te bepalen welke procedure opgeroepen moet worden vanuit Procedures.cs
        internal static dynamic BaseProcedure(dynamic tableObject, string stringParameters, Procedures.OperationType operationType, Dictionary<string, string[]> lookup)
        {
            Dictionary<string, object> parameters = null;
            if (tableObject != null && stringParameters != null)
                parameters = GetParameters(tableObject, stringParameters, lookup);
            object result;
            string previousMethodName = MethodBase.GetCurrentMethod().Name;
            int i = 1;
            while (previousMethodName == MethodBase.GetCurrentMethod().Name)
            {
                previousMethodName = (new StackTrace()).GetFrame(i).GetMethod().Name;
                i++;
            }
            switch (operationType)
            {
                case Procedures.OperationType.Select:
                    result = Procedures.Select(previousMethodName, parameters);
                    break;
                case Procedures.OperationType.Update:
                    result = Procedures.Update(previousMethodName, parameters);
                    break;
                case Procedures.OperationType.Insert:
                    result = Procedures.Insert(previousMethodName, parameters);
                    break;
                case Procedures.OperationType.Delete:
                    result = Procedures.Delete(previousMethodName, parameters);
                    break;
                default:
                    result = new Exception("Was ge bezig maat?, geen operationtype?");
                    break;
            }
            return result;
        }

        internal static dynamic BaseProcedure(Procedures.OperationType operationType, Dictionary<string, string[]> lookup)
            => BaseProcedure(null, null, operationType, lookup);

        // Op basis van de klasse zijn lookup dictionary worden de parameter initialen omgezet naar de volledige namen
        // en hun waarden
        private static Dictionary<string, object> GetParameters(dynamic tableObject, string stringParameters, Dictionary<string, string[]> lookup)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (string parameter in stringParameters.Split(','))
            {
                try
                {
                    parameters[lookup[parameter][0]] = tableObject.GetType().GetProperty(lookup[parameter][1]).GetValue(tableObject, null);
                }
                catch (Exception e)
                {

                }
            }
            return parameters;
        }

        // Convert Table to object(s)
        public static T ConvertTableToObject<T>(DataTable table, Dictionary<string, string[]> lookup)
        {
            DataRow row = table.Rows[0];
            T t = (T)Activator.CreateInstance(typeof(T));
            foreach (string[] lookupString in lookup.Values)
            {
                if (!row.Table.Columns.Contains(lookupString[2])) continue;
                object value = row[lookupString[2]];
                typeof(T).GetProperty(lookupString[1]).SetValue(t, value, null);
            }
            return t;
        }

        public static T[] ConvertTableToObjects<T>(DataTable table, Dictionary<string, string[]> lookup)
        {
            List<T> ts = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T t = (T)Activator.CreateInstance(typeof(T));
                foreach (string[] lookupString in lookup.Values)
                {
                    if (!row.Table.Columns.Contains(lookupString[2])) continue;
                    object value = row[lookupString[2]];
                    typeof(T).GetProperty(lookupString[1]).SetValue(t, value, null);
                }
                ts.Add(t);
            }
            return ts.ToArray();
        }
    }
}
