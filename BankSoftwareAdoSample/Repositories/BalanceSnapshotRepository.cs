using BankSoftwareAdoSample.Models;
using BankSoftwareAdoSample.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSoftwareAdoSample.Repositories
{
    public class BalanceSnapshotRepository
    {
        private readonly DatabaseService _databaseService;

        public BalanceSnapshotRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public BalanceSnapshotRepository()
        {
            _databaseService = new DatabaseService();
        }

        public async Task<DataTable> GetAllBalanceSnapshotsByAccountIdAsync(SqlConnection sqlConnection,
            Guid accountId, CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM BalanceSnapshots WHERE AccountId = @AccountId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAllBalanceSnapshotsByAccountIdAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetLatestBalanceSnapshotByAccountIdAsync(SqlConnection sqlConnection,
            Guid accountId, CancellationToken ct = default)
        {
            try
            {
                var query =
                    "SELECT TOP 1 * FROM BalanceSnapshots WHERE AccountId = @AccountId ORDER BY SnapshotDateTime DESC";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(GetLatestBalanceSnapshotByAccountIdAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> AddBalanceSnapshotAsync(SqlConnection sqlConnection, BalanceSnapshot balanceSnapshot,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement =
                    "INSERT INTO BalanceSnapshots (BalanceSnapshotId, AccountId, Balance, SnapshotDateTime, SnapshotHash) " +
                    "VALUES (@BalanceSnapshotId, @AccountId, @Balance, @SnapshotDateTime, @SnapshotHash)";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@BalanceSnapshotId", SqlDbType.UniqueIdentifier)
                        { Value = balanceSnapshot.BalanceSnapshotId },
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = balanceSnapshot.AccountId },
                    new SqlParameter("@Balance", SqlDbType.Decimal) { Value = balanceSnapshot.Balance },
                    new SqlParameter("@SnapshotDateTime", SqlDbType.DateTimeOffset)
                        { Value = balanceSnapshot.SnapshotDateTime },
                    new SqlParameter("@SnapshotHash", SqlDbType.NVarChar) { Value = balanceSnapshot.SnapshotHash }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(AddBalanceSnapshotAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> DeleteByAccountIdAsync(SqlConnection sqlConnection, Guid accountId,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement = "DELETE FROM BalanceSnapshots WHERE AccountId = @AccountId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@AccountId", SqlDbType.UniqueIdentifier) { Value = accountId }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(DeleteByAccountIdAsync)}: {e.Message}");
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

                string parameterString = string.Join(", ",
                    accountIds.Select((id, index) => $"@AccountId{index}"));

                var statement = $"DELETE FROM BalanceSnapshots WHERE AccountId IN ({parameterString})";

                List<SqlParameter> sqlParameters = accountIds.Select((id, index) =>
                    new SqlParameter($"@AccountId{index}", SqlDbType.UniqueIdentifier) { Value = id }).ToList();

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(DeleteByAccountIdsAsync)}: {e.Message}");
                throw;
            }
        }

        public async Task<int> DeleteBatchAsync(SqlConnection sqlConnection, List<Guid> balanceSnapshotIds, SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                if (balanceSnapshotIds.Count == 0)
                    return 0;

                var parameterString = string.Join(", ",
                    balanceSnapshotIds.Select((id, index) => $"@BalanceSnapshotId{index}"));

                var statement = $"DELETE FROM BalanceSnapshots WHERE AccountId IN ({parameterString})";

                List<SqlParameter> sqlParameters = balanceSnapshotIds.Select((id, index) =>
                    new SqlParameter($"@BalanceSnapshotId{index}", SqlDbType.UniqueIdentifier) { Value = id }
                ).ToList();

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in {nameof(DeleteBatchAsync)}: {e.Message}");
                throw;
            }
        }
    }
}