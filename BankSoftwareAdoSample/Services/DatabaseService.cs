using System.Data;
using System.Configuration;
using System.Diagnostics;

namespace BankSoftwareAdoSample.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BankDb"].ConnectionString;
        }

        public async Task<TResult> ExecuteConnectionAsync<TResult>(
            Func<SqlConnection, CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            await using var sqlConnection = new SqlConnection(_connectionString);

            try
            {
                await sqlConnection.OpenAsync(cancellationToken);

                return await action(sqlConnection, cancellationToken);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }


        public async Task<TResult> ExecuteConnectionAsync<TInput, TResult>(
            Func<SqlConnection, TInput, CancellationToken, Task<TResult>> action, TInput input,
            CancellationToken cancellationToken = default)
        {
            await using var sqlConnection = new SqlConnection(_connectionString);

            try
            {
                await sqlConnection.OpenAsync(cancellationToken);

                return await action(sqlConnection, input, cancellationToken);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }

        public async Task<DataTable> ReadDataAsync(SqlConnection sqlConnection, string query,
            List<SqlParameter> sqlParameters, CancellationToken cancellationToken = default)
        {
            DataTable dataTable = new DataTable();

            try
            {
                if (sqlConnection.State is not ConnectionState.Open)
                    await sqlConnection.OpenAsync(cancellationToken);

                await using SqlCommand sqlCommand = new(query, sqlConnection);

                sqlCommand.Parameters.AddRange([.. sqlParameters]);

                using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                dataTable.Load(sqlDataReader);

                await sqlDataReader.CloseAsync();

                return dataTable;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<int> ExecuteStatementAsync(SqlConnection sqlConnection, string statement,
            List<SqlParameter> sqlParameters, SqlTransaction? transaction = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (sqlConnection.State is not ConnectionState.Open)
                    await sqlConnection.OpenAsync(cancellationToken);

                await using SqlCommand sqlCommand = new(statement, sqlConnection);

                if (transaction is not null)
                    sqlCommand.Transaction = transaction;

                sqlCommand.Parameters.AddRange([.. sqlParameters]);

                return await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(SqlConnection sqlConnection, string query,
            List<SqlParameter> sqlParameters, CancellationToken ct = default)
        {
            try
            {
                if (sqlConnection.State is not ConnectionState.Open)
                    await sqlConnection.OpenAsync(ct);

                await using SqlCommand sqlCommand = new(query, sqlConnection);
                sqlCommand.Parameters.AddRange([.. sqlParameters]);

                object? result = await sqlCommand.ExecuteScalarAsync(ct);

                if (result is null || result == DBNull.Value)
                    return default!;

                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}