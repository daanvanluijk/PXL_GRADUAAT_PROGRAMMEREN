using Shpielerij.SQL;
using Shpielerij.SQL.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.SQL
{
    public static class SQLCommands
    {
        public static TableResult GetTableFromQuery(string query, Dictionary<string, object> parameters)
        {
            TableResult result = new TableResult();
            try
            {
                if (!query.ToLower().StartsWith("select")) throw new Exception("Query is geen select query!");
                SqlCommand command = ConstructSqlCommand(query, parameters);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                command.Connection.Close();
                DataTable table = dataSet.Tables[0];
                result.Table = table;
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.AddError(e);
            }
            return result;
        }

        public static TableResult GetTableFromQuery(string query) => GetTableFromQuery(query, null);

        public static ObjectsResult<T> GetObjectsFromQuery<T>(string query, Dictionary<string, object> parameters)
        {
            ObjectsResult<T> result = new ObjectsResult<T>();
            try
            {
                if (!query.ToLower().StartsWith("select")) throw new Exception("Query is geen select query!");
                SqlCommand command = ConstructSqlCommand(query, parameters);
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    List<T> ts = new List<T>();
                    while (reader.Read())
                    {
                        T t = (T)Activator.CreateInstance(typeof(T));
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (reader[prop.Name] != null)
                            {
                                prop.SetValue(t, reader[prop.Name]);
                            }
                        }
                        ts.Add(t);
                    }
                    result.Objects = ts.ToArray();
                    result.Succeeded = true;
                }
                catch (Exception e)
                {
                    result.AddError(e);
                }
                finally
                {
                    reader.Close();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                result.AddError(e);
            }
            return result;
        }

        public static ObjectsResult<T> GetObjectsFromQuery<T>(string query) => GetObjectsFromQuery<T>(query, null);

        public static NonQueryResult ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            NonQueryResult result = new NonQueryResult();
            try
            {
                SqlCommand command = ConstructSqlCommand(query, parameters);
                result.RowsAffected = command.ExecuteNonQuery();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.AddError(e);
            }
            return result;
        }

        private static SqlCommand ConstructSqlCommand(string query, Dictionary<string, object> parameters)
        {
            try
            {
                SqlConnection connection = new SqlConnection(Settings.ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
                return command;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}