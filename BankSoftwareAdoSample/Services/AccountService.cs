using System.Data;
using System.Diagnostics;
using BankSoftwareAdoSample.Models;

namespace BankSoftwareAdoSample.Services;

public class AccountService
{
    private readonly UnitOfWork _unitOfWork;

    public AccountService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Account?> GetAccountByIdAsync(SqlConnection sqlConnection, Guid accountId,
        CancellationToken ct = default)
    {
        try
        {
            var accountTable = await _unitOfWork.AccountRepository.GetAccountByIdAsync(sqlConnection, accountId, ct);
            if (accountTable.Rows.Count != 1)
                return null;

            var mapTable = Account.MapFromDataTable(accountTable);

            return mapTable[0];
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetAccountByIdAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<List<Account>> GetAllAccountsAsync(SqlConnection sqlConnection, CancellationToken ct = default)
    {
        try
        {
            var accountsTable = await _unitOfWork.AccountRepository.GetAllAccountsAsync(sqlConnection, ct);

            var mapTable = Account.MapFromDataTable(accountsTable);
            return mapTable;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetAllAccountsAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<List<Account>> GetAllAccountsByCustomerIdAsync(SqlConnection sqlConnection, Guid customerId,
        CancellationToken ct = default)
    {
        try
        {
            var accountsTable =
                await _unitOfWork.AccountRepository.GetAllAccountsByCustomerIdAsync(sqlConnection, customerId, ct);

            var mapTable = Account.MapFromDataTable(accountsTable);
            return mapTable;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetAllAccountsByCustomerIdAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<Account?> CreateAccountAsync(SqlConnection sqlConnection, Guid customerId,
        decimal openingBalance, CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            var accountNumber = await _unitOfWork.AccountRepository.GetNewAccountNumberAsync(sqlConnection, ct);
            var creationTime = DateTimeOffset.UtcNow;

            var account = new Account(Guid.NewGuid(), customerId, accountNumber, openingBalance, creationTime,
                true);

            sqlTransaction = sqlConnection.BeginTransaction();

            var createdRows =
                await _unitOfWork.AccountRepository.CreateAccountAsync(sqlConnection, account, sqlTransaction, ct);

            if (createdRows != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return null;
            }

            await sqlTransaction.CommitAsync(ct);
            
            return account;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();
            Debug.WriteLine($"Error in {nameof(CreateAccountAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }

    public async Task<bool> DeleteAccountAsync(SqlConnection sqlConnection, Guid accountId,
        CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            sqlTransaction = sqlConnection.BeginTransaction();

            // delete balance snapshots
            var balanceSnapshotsTable =
                await _unitOfWork.BalanceSnapshotRepository.GetAllBalanceSnapshotsByAccountIdAsync(sqlConnection,
                    accountId, ct);

            if (balanceSnapshotsTable.Rows.Count > 0)
            {
                var balanceSnapshotIds = balanceSnapshotsTable.AsEnumerable()
                    .Select(row => row.Field<Guid>("BalanceSnapshotId"))
                    .ToList();

                var deletedSnapshotsCount = await _unitOfWork.BalanceSnapshotRepository.DeleteBatchAsync(sqlConnection,
                    balanceSnapshotIds,
                    sqlTransaction, ct);

                if (deletedSnapshotsCount != balanceSnapshotsTable.Rows.Count)
                {
                    await sqlTransaction.RollbackAsync(ct);
                    return false;
                }
            }

            // delete account transactions

            var accountTransactionsTable =
                await _unitOfWork.AccountTransactionRepository.GetAllByAccountIdAsync(sqlConnection, accountId, ct);

            if (accountTransactionsTable.Rows.Count > 0)
            {
                var accountTransactionIds = accountTransactionsTable.AsEnumerable()
                    .Select(row => row.Field<Guid>("TransactionId"))
                    .ToList();

                var deletedTransactionsCount = await _unitOfWork.AccountTransactionRepository.DeleteBatchAsync(
                    sqlConnection, accountTransactionIds,
                    sqlTransaction, ct);

                if (accountTransactionsTable.Rows.Count != deletedTransactionsCount)
                {
                    await sqlTransaction.RollbackAsync(ct);
                    return false;
                }
            }

            // delete account

            var deletedAccountsCount =
                await _unitOfWork.AccountRepository.DeleteAccountAsync(sqlConnection, accountId, sqlTransaction, ct);

            if (deletedAccountsCount != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return false;
            }

            await sqlTransaction.CommitAsync(ct);

            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(DeleteAccountAsync)}: {e.Message}");
            sqlTransaction?.Rollback();

            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }
}