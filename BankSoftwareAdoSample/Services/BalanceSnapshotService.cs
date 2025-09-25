using System.Diagnostics;
using BankSoftwareAdoSample.Models;

namespace BankSoftwareAdoSample.Services;

public class BalanceSnapshotService
{
    private readonly UnitOfWork _unitOfWork;

    public BalanceSnapshotService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BalanceSnapshot?> GetLatestBalanceSnapshotAsync(SqlConnection sqlConnection, Guid accountId,
        CancellationToken ct = default)
    {
        try
        {
            var snapshotTable =
                await _unitOfWork.BalanceSnapshotRepository.GetLatestBalanceSnapshotByAccountIdAsync(sqlConnection,
                    accountId, ct);
            if (snapshotTable.Rows.Count != 1)
                return null;

            var mapTable = BalanceSnapshot.MapFromDataTable(snapshotTable);

            return mapTable[0];
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetLatestBalanceSnapshotAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<List<BalanceSnapshot>> GetAllBalanceSnapshotsByAccountIdAsync(SqlConnection sqlConnection,
        Guid accountId, CancellationToken ct = default)
    {
        try
        {
            var snapshotsTable =
                await _unitOfWork.BalanceSnapshotRepository.GetAllBalanceSnapshotsByAccountIdAsync(sqlConnection,
                    accountId, ct);

            var mapTable = BalanceSnapshot.MapFromDataTable(snapshotsTable);
            return mapTable;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetAllBalanceSnapshotsByAccountIdAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<BalanceSnapshot?> AddBalanceSnapshotAsync(SqlConnection sqlConnection, Guid accountId,
        CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            // get the account
            var accountTable = await _unitOfWork.AccountRepository.GetAccountByIdAsync(sqlConnection, accountId, ct);

            if (accountTable.Rows.Count != 1)
                return null;

            var mappedAccountTables = Account.MapFromDataTable(accountTable);
            var account = mappedAccountTables[0];

            // get the latest snapshot
            var latestSnapshot = await GetLatestBalanceSnapshotAsync(sqlConnection, accountId, ct);

            // get all transactions after the latest snapshot
            var allTransactions =
                await _unitOfWork.AccountTransactionRepository.GetAllByAccountIdAfterDateTimeAsync(sqlConnection,
                    accountId, latestSnapshot?.SnapshotDateTime ?? account.CreatedAt, ct);

            var mappedTransactions = AccountTransaction.MapFromDataTable(allTransactions);

            if (mappedTransactions.Count != allTransactions.Rows.Count)
                return null;

            var newSnapshotId = Guid.NewGuid();
            var createdDateTime = DateTimeOffset.UtcNow;

            // calculate balance
            var balance = Account.CalculateBalance(account, latestSnapshot, mappedTransactions);

            // create snapshot hash
            var newSnapshotHash =
                BalanceSnapshot.GenerateSnapshotHash(accountId, createdDateTime, mappedTransactions, balance);

            var newBalanceSnapshot =
                new BalanceSnapshot(Guid.NewGuid(), accountId, createdDateTime, balance, newSnapshotHash);

            sqlTransaction = sqlConnection.BeginTransaction();

            var rowsAffected = await _unitOfWork.BalanceSnapshotRepository.AddBalanceSnapshotAsync(sqlConnection,
                newBalanceSnapshot, sqlTransaction, ct);

            if (rowsAffected != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return null;
            }

            await sqlTransaction.CommitAsync(ct);

            return newBalanceSnapshot;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();
            Debug.WriteLine($"Error in {nameof(AddBalanceSnapshotAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }
}