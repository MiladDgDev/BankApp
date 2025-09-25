using System.Diagnostics;
using BankSoftwareAdoSample.Enums;
using BankSoftwareAdoSample.Models;

namespace BankSoftwareAdoSample.Services;

public class AccountTransactionService
{
    private readonly UnitOfWork _unitOfWork;

    public AccountTransactionService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountTransaction?> GetTransactionByIdsAsync(SqlConnection sqlConnection, Guid transactionId,
        Guid accountId,
        CancellationToken ct = default)
    {
        try
        {
            var transactionTable =
                await _unitOfWork.AccountTransactionRepository.GetTransactionByIdAsync(sqlConnection, transactionId,
                    accountId, ct);
            if (transactionTable.Rows.Count != 1)
                return null;

            var mapTable = AccountTransaction.MapFromDataTable(transactionTable);

            return mapTable[0];
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetTransactionByIdsAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<List<AccountTransaction>> GetAllTransactionsByAccountIdAsync(SqlConnection sqlConnection,
        Guid accountId, CancellationToken ct = default)
    {
        try
        {
            var transactionsTable =
                await _unitOfWork.AccountTransactionRepository.GetAllByAccountIdAsync(sqlConnection, accountId, ct);

            var mapTable = AccountTransaction.MapFromDataTable(transactionsTable);
            return mapTable;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetAllTransactionsByAccountIdAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<AccountTransaction?> AddTransactionAsync(SqlConnection sqlConnection, Guid accountId, decimal amount,
        string description, TransactionType transactionType, CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;
        
        try
        {
            var transactionId = Guid.NewGuid();
            var transactionDateTime = DateTimeOffset.UtcNow;
            var newTransaction = new AccountTransaction(transactionId, accountId, transactionDateTime, amount,
                description, transactionType);
            
            sqlTransaction = sqlConnection.BeginTransaction();
            
            var addResult = await _unitOfWork.AccountTransactionRepository.AddTransactionAsync(sqlConnection,
                newTransaction, sqlTransaction, ct);

            if (addResult != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return null;
            }
            
            await sqlTransaction.CommitAsync(ct);
            
            return newTransaction;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();
            Debug.WriteLine($"Error in {nameof(AddTransactionAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }
    
    public async Task<bool> DeleteTransactionAsync(SqlConnection sqlConnection, Guid transactionId, Guid accountId,
        CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            sqlTransaction = sqlConnection.BeginTransaction();

            // get all transactions
            var allAccountTransactions = await GetAllTransactionsByAccountIdAsync(sqlConnection, accountId, ct);

            if (allAccountTransactions.Count == 0)
                return false;

            // find target transaction
            if (allAccountTransactions.Exists(t => t.TransactionId == transactionId) is false)
                return false;

            var targetTransaction =
                allAccountTransactions.First(t => t.TransactionId == transactionId && t.AccountId == accountId);

            // create a balance snapshot
            BalanceSnapshot? latestSnapshot = null;

            var latestSnapshotDataTable =
                await _unitOfWork.BalanceSnapshotRepository.GetLatestBalanceSnapshotByAccountIdAsync(
                    sqlConnection, targetTransaction.AccountId, ct);

            if (latestSnapshotDataTable.Rows.Count > 1)
                return false;

            if (latestSnapshotDataTable.Rows.Count == 1)
            {
                var mappedSnapshots = BalanceSnapshot.MapFromDataTable(latestSnapshotDataTable);

                if (mappedSnapshots.Count != latestSnapshotDataTable.Rows.Count)
                    return false;

                latestSnapshot = mappedSnapshots[0];
            }

            var targetAccountDataTable =
                await _unitOfWork.AccountRepository.GetAccountByIdAsync(sqlConnection, accountId, ct);

            if (targetAccountDataTable.Rows.Count != 1)
                return false;

            var mappedAccounts = Account.MapFromDataTable(targetAccountDataTable);

            if (mappedAccounts.Count != 1)
                return false;

            var targetAccount = mappedAccounts[0];

            var latestBalance = Account.CalculateBalance(targetAccount, latestSnapshot, allAccountTransactions);

            // Delete the transaction
            var deleteResult =
                await _unitOfWork.AccountTransactionRepository.DeleteTransactionByIdAsync(sqlConnection,
                    transactionId, sqlTransaction, ct);

            if (deleteResult != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return false;
            }

            allAccountTransactions.Remove(targetTransaction);

            var snapshotTime = DateTimeOffset.UtcNow;

            var snapshotHash = BalanceSnapshot.GenerateSnapshotHash(
                targetAccount.AccountId,
                snapshotTime,
                allAccountTransactions,
                latestBalance);

            var newBalanceSnapshot = new BalanceSnapshot(
                Guid.NewGuid(),
                targetAccount.AccountId,
                snapshotTime,
                latestBalance,
                snapshotHash);

            // add new balance snapshot to the db

            var addSnapshotResult = await _unitOfWork.BalanceSnapshotRepository.AddBalanceSnapshotAsync(sqlConnection,
                newBalanceSnapshot, sqlTransaction, ct);

            if (addSnapshotResult != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return false;
            }

            // commit transaction
            await sqlTransaction.CommitAsync(ct);
            return true;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();

            Debug.WriteLine($"Error in {nameof(DeleteTransactionAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }
}