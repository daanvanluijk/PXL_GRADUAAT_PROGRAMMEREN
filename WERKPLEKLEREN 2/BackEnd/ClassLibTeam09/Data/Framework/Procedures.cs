using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Data.Framework
{
    public static class Procedures
    {
        public enum OperationType
        {
            Select,
            Update,
            Insert,
            Delete,
        }

        #region Procedures
        public static SelectResult Select(string procedure, Dictionary<string, object> parameters) 
            => (SelectResult)BaseOperation(OperationType.Select, procedure, parameters);

        public static SelectResult Select(string procedure) 
            => Select(procedure, null);

        public static UpdateResult Update(string procedure, Dictionary<string, object> parameters) 
            => (UpdateResult)BaseOperation(OperationType.Update, procedure, parameters);

        public static UpdateResult Update(string procedure) 
            => Update(procedure, null);

        public static InsertResult Insert(string procedure, Dictionary<string, object> parameters) 
            => (InsertResult)BaseOperation(OperationType.Insert, procedure, parameters);

        public static InsertResult Insert(string procedure) 
            => Insert(procedure, null);

        public static DeleteResult Delete(string procedure, Dictionary<string, object> parameters)
            => (DeleteResult)BaseOperation(OperationType.Delete, procedure, parameters);

        public static DeleteResult Delete(string procedure)
            => Delete(procedure, null);
        #endregion

        #region Operations
        private static dynamic BaseOperation(OperationType operationType, string procedure, Dictionary<string, object> parameters)
        {
            SqlConnection connection = new SqlConnection(Settings.Settings.Database.ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(procedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }
            switch (operationType)
            {
                case OperationType.Select:
                    return SelectOperation(command, connection);
                case OperationType.Update:
                    return UpdateOperation(command, connection);
                case OperationType.Insert:
                    return InsertOperation(command, connection);
                case OperationType.Delete:
                    return DeleteOperation(command, connection);
                default:
                    break;
            }
            return new Exception("kapoet");
        }

        private static SelectResult SelectOperation(SqlCommand command, SqlConnection connection)
        {
            SelectResult result = new SelectResult();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet data = new DataSet();
            try
            {
                adapter.Fill(data);
                result.DataTable = data.Tables[0];
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }
            connection.Close();
            return result;
        }

        private static UpdateResult UpdateOperation(SqlCommand command, SqlConnection connection)
        {
            UpdateResult result = new UpdateResult();
            try
            {
                command.ExecuteNonQuery();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }
            connection.Close();
            return result;
        }

        private static InsertResult InsertOperation(SqlCommand command, SqlConnection connection)
        {
            InsertResult result = new InsertResult();
            try
            {
                command.ExecuteNonQuery();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }
            connection.Close();
            return result;
        }

        private static DeleteResult DeleteOperation(SqlCommand command, SqlConnection connection)
        {
            DeleteResult result = new DeleteResult();
            try
            {
                command.ExecuteNonQuery();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }
            connection.Close();
            return result;
        }
        #endregion
    }
}
