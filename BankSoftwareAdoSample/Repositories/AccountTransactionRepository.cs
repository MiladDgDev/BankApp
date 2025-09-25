using BankSoftwareAdoSample.Models;
using BankSoftwareAdoSample.Services;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BankSoftwareAdoSample.Repositories
{
    public class AccountTransactionRepository
    {
        private readonly DatabaseService _databaseService;

        public AccountTransactionRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public AccountTransactionRepository()
        {
            _databaseService = new DatabaseService();
        }


        public async Task<DataTable> GetAllByAccountIdAsync(SqlConnection sqlConnection, Guid accountId,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM AccountTransactions WHERE AccountId = @AccountId";
                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];
                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAllByAccountIdAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetAllByAccountIdsAsync(SqlConnection sqlConnection, List<Guid> accountIds,
            CancellationToken ct = default)
        {
            try
            {
                var parameterString = string.Join(", ", accountIds.Select((id, index) => $"@AccountId{index}"));

                var query = $"SELECT * FROM AccountTransactions WHERE AccountId IN ({parameterString})";
                
                List<SqlParameter> sqlParameters = accountIds.Select((id, index) =>
                    new SqlParameter($"@AccountId{index}", SqlDbType.UniqueIdentifier) { Value = id }
                ).ToList();
                
                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAllByAccountIdsAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetAllByAccountIdAfterDateTimeAsync(SqlConnection sqlConnection, Guid accountId,
            DateTimeOffset afterDateTime,
            CancellationToken ct = default)
        {
            try
            {
                var query =
                    "SELECT * FROM AccountTransactions WHERE AccountId = @AccountId AND TransactionDateTime > @AfterDateTime";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId },
                    new SqlParameter("@AfterDateTime", SqlDbType.DateTimeOffset) { Value = afterDateTime }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAllByAccountIdAfterDateTimeAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetAllTransactionsAsync(SqlConnection sqlConnection,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM AccountTransactions";
                List<SqlParameter> sqlParameters = [];
                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAllTransactionsAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetTransactionByIdAsync(SqlConnection sqlConnection, Guid accountId,
            Guid transactionId,
            CancellationToken ct = default)
        {
            try
            {
                var query =
                    "SELECT * FROM AccountTransactions WHERE AccountId = @AccountId AND TransactionId = @TransactionId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId },
                    new SqlParameter("@TransactionId", SqlDbType.UniqueIdentifier) { Value = transactionId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetTransactionByIdAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> AddTransactionAsync(SqlConnection sqlConnection, AccountTransaction accountTransaction,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement =
                    "INSERT INTO AccountTransactions (TransactionId, AccountId, Amount, TransactionType, TransactionDateTime, Description) " +
                    "VALUES (@TransactionId, @AccountId, @Amount, @TransactionType, @TransactionDateTime, @Description)";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@TransactionId", SqlDbType.UniqueIdentifier)
                        { Value = accountTransaction.TransactionId },
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountTransaction.AccountId },
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Value = accountTransaction.Amount },
                    new SqlParameter("@TransactionType", SqlDbType.Int, 50)
                        { Value = (int)accountTransaction.TransactionType },
                    new SqlParameter("@TransactionDateTime", SqlDbType.DateTimeOffset)
                        { Value = accountTransaction.TransactionDateTime },
                    new SqlParameter("@Description", SqlDbType.NVarChar) { Value = accountTransaction.Description }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction,
                    ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(AddTransactionAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteTransactionByIdAsync(SqlConnection sqlConnection, Guid transactionId,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var query = "DELETE FROM AccountTransactions WHERE TransactionId = @TransactionId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@TransactionId", SqlDbType.UniqueIdentifier) { Value = transactionId }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, query, sqlParameters, sqlTransaction,
                    ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(DeleteTransactionByIdAsync)}: {ex.Message}");
                throw;
            }
        }
        
        public async Task<int> DeleteByAccountIdsAsync(SqlConnection sqlConnection, List<Guid> accountIds,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                if (accountIds.Count == 0)
                    return 0;

                var parametersString =
                    string.Join(", ", accountIds.Select((id, index) => $"@AccountId{index}"));

                var query = $"DELETE FROM AccountTransactions WHERE AccountId IN ({parametersString})";

                List<SqlParameter> sqlParameters = accountIds.Select((id, index) =>
                    new SqlParameter($"@AccountId{index}", SqlDbType.UniqueIdentifier) { Value = id }
                ).ToList();

                return await _databaseService.ExecuteStatementAsync(sqlConnection, query, sqlParameters, sqlTransaction,
                    ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(DeleteByAccountIdsAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteBatchAsync(SqlConnection sqlConnection, List<Guid> transactionIds,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                if (transactionIds.Count == 0)
                    return 0;

                var parametersString =
                    string.Join(", ", transactionIds.Select((id, index) => $"@TransactionId{index}"));

                var query = $"DELETE FROM AccountTransactions WHERE TransactionId IN ({parametersString})";

                List<SqlParameter> sqlParameters = transactionIds.Select((id, index) =>
                    new SqlParameter($"@TransactionId{index}", SqlDbType.UniqueIdentifier) { Value = id }
                ).ToList();

                return await _databaseService.ExecuteStatementAsync(sqlConnection, query, sqlParameters, sqlTransaction,
                    ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(DeleteBatchAsync)}: {ex.Message}");
                throw;
            }
        }
    }
}