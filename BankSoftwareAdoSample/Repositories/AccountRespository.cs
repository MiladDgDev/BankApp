using BankSoftwareAdoSample.Models;
using BankSoftwareAdoSample.Services;
using System.Data;
using System.Diagnostics;

namespace BankSoftwareAdoSample.Repositories
{
    public class AccountRespository
    {
        private readonly DatabaseService _databaseService;

        public AccountRespository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public AccountRespository()
        {
            _databaseService = new DatabaseService();
        }

        public async Task<DataTable> GetAccountByIdAsync(SqlConnection sqlConnection, Guid accountId,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM Accounts WHERE AccountId = @AccountId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAccountByIdAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetAllAccountsByCustomerIdAsync(SqlConnection sqlConnection, Guid customerId,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM Accounts WHERE CustomerId = @CustomerId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = customerId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(GetAllAccountsByCustomerIdAsync)}: {e.Message}");
                throw;
            }
        }
        
        public async Task<DataTable> GetAllAccountIdsByCustomerIdAsync(SqlConnection sqlConnection, Guid customerId,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT AccountId FROM Accounts WHERE CustomerId = @CustomerId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = customerId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(GetAllAccountIdsByCustomerIdAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetAllAccountsAsync(SqlConnection sqlConnection, CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM Accounts";

                return await _databaseService.ReadDataAsync(sqlConnection, query, [], ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(GetAllAccountsAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> CreateAccountAsync(SqlConnection sqlConnection, Account account,
            SqlTransaction? transaction = null, CancellationToken ct = default)
        {
            try
            {
                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = account.AccountId },
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = account.CustomerId },
                    new SqlParameter("@AccountNumber", SqlDbType.NVarChar) { Value = account.AccountNumber },
                    new SqlParameter("@Iban", SqlDbType.NVarChar) { Value = account.Iban },
                    new SqlParameter("@OpeningBalance", SqlDbType.Decimal) { Value = account.OpeningBalance },
                    new SqlParameter("@CreatedAt", SqlDbType.DateTimeOffset) { Value = account.CreatedAt },
                    new SqlParameter("@IsActive", SqlDbType.Bit) { Value = account.IsActive }
                ];

                var statement =
                    "INSERT INTO Accounts (AccountId, CustomerId, AccountNumber, Iban, OpeningBalance, CreatedAt, IsActive) " +
                    "VALUES (@AccountId, @CustomerId, @AccountNumber, @Iban, @OpeningBalance, @CreatedAt, @IsActive)";


                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    transaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(CreateAccountAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> DeactivateAccountAsync(SqlConnection sqlConnection, Guid accountId,
            SqlTransaction? transaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement = "UPDATE Accounts SET IsActive = 0 WHERE AccountId = @AccountId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    transaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(DeactivateAccountAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<string> GetNewAccountNumberAsync(SqlConnection sqlConnection,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT NEXT VALUE FOR AccountNumberSequence AS NewAccountNumber";

                var result = await _databaseService.ExecuteScalarAsync<int>(sqlConnection, query, [], ct);

                return result.ToString().PadLeft(10, '0');
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(GetNewAccountNumberAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> DeleteAccountAsync(SqlConnection sqlConnection, Guid accountId,
            SqlTransaction? transaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement = "DELETE FROM Accounts WHERE AccountId = @AccountId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    transaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(DeleteAccountAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> DeleteBatchAsync(SqlConnection sqlConnection, List<Guid> accountIds,
            SqlTransaction? transaction = null,
            CancellationToken ct = default)
        {
            if (accountIds.Count == 0)
                return 0;

            var parametersStr = string.Join(", ", accountIds.Select((id, index) => $"@AccountId{index}"));

            try
            {
                var statement = $"DELETE FROM Accounts WHERE AccountId IN ({parametersStr})";

                List<SqlParameter> sqlParameters = accountIds.Select((id, index) =>
                    new SqlParameter($"@AccountId{index}", SqlDbType.UniqueIdentifier) { Value = id }
                ).ToList();

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    transaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(DeleteBatchAsync)}: {e.Message}");
                throw;
            }
        }
    }
}