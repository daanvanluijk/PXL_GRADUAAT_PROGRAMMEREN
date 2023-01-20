using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.ObjectManagement
{
    public static class ObjectManagement
    {
        public static T[] CreateObjectsFromLineArray<T>(string[][] lines)
        {
            List<T> objects = new List<T>();
            foreach (string[] line in lines)
            {
                objects.Add(CreateObjectFromLine<T>(line));
            }
            return objects.ToArray();
        }

        public static T[] CreateObjectsFromLineArray<T>(string[][] lines, string[] include)
        {
            List<T> objects = new List<T>();
            foreach (string[] line in lines)
            {
                objects.Add(CreateObjectFromLine<T>(line, include));
            }
            return objects.ToArray();
        }

        public static T[] CreateObjectsFromDataView<T>(DataView view)
        {
            List<T> objects = new List<T>();
            foreach (DataRowView row in view)
            {
                var props = typeof(T).GetProperties();
                object[] parameters = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    parameters[i] = Convert.ChangeType(row[props[i].Name], props[i].PropertyType);
                }
                objects.Add((T)Activator.CreateInstance(typeof(T), parameters));
            }
            return objects.ToArray();
        }

        public static T[] CreateObjectsFromDataView<T>(DataView view, Dictionary<string, string> lookup)
        {
            List<T> objects = new List<T>();
            foreach (DataRowView row in view)
            {
                var props = typeof(T).GetProperties();
                object[] parameters = new object[props.Length];
                int j = 0;
                for (int i = 0; i < props.Length; i++)
                {
                    KeyValuePair<string, string> pair = lookup.ToArray()[j];
                    if (lookup.Where(x => x.Value.ToLower() == props[i].Name.ToLower()).Count() > 0)
                    {
                        parameters[i] = Convert.ChangeType(row[pair.Key], props.Where(x => x.Name.ToLower() == pair.Value.ToLower()).First().PropertyType);
                        j++;
                    }
                    else
                    {
                        parameters[i] = null;
                    }
                }
                objects.Add((T)Activator.CreateInstance(typeof(T), parameters));
            }
            return objects.ToArray();
        }

        public static T CreateObjectFromLine<T>(string[] line)
        {
            object[] parameters = new object[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                parameters[i] = Convert.ChangeType(line[i], typeof(T).GetProperties()[i].PropertyType);
            }
            return (T)Activator.CreateInstance(typeof(T), parameters);
        }

        public static T CreateObjectFromLine<T>(string[] line, string[] include)
        {
            var props = typeof(T).GetProperties();
            object[] parameters = new object[props.Length];
            int j = 0;
            for (int i = 0; i < props.Length; i++)
            {
                if (include.Where(x => x.ToLower() == props[i].Name.ToLower()).Count() > 0)
                {
                    parameters[i] = Convert.ChangeType(line[j], props.Where(x => x.Name.ToLower() == include[j].ToLower()).First().PropertyType);
                    j++;
                }
                else
                {
                    parameters[i] = null;
                }
            }
            return (T)Activator.CreateInstance(typeof(T), parameters);
        }
    }
}
