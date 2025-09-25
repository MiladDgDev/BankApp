using System.Data;
using System.Diagnostics;
using BankSoftwareAdoSample.Models;

namespace BankSoftwareAdoSample.Services;

public class CustomerService
{
    private readonly UnitOfWork _unitOfWork;

    public UnitOfWork UnitOfWork => _unitOfWork;

    public CustomerService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<List<Customer>> GetAllCustomersAsync(SqlConnection sqlConnection,
        CancellationToken ct = default)
    {
        try
        {
            var customersTable = await _unitOfWork.CustomerRepository.GetAllCustomersAsync(sqlConnection, ct);

            var mappedCustomers = Customer.MapFromDataTable(customersTable);

            return mappedCustomers;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetAllCustomersAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<Customer?> GetCustomerByIdAsync(SqlConnection sqlConnection, Guid customerId,
        CancellationToken ct = default)
    {
        try
        {
            var customerTable =
                await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(sqlConnection, customerId, ct);
            if (customerTable.Rows.Count != 1)
                return null;

            var mappedCustomers = Customer.MapFromDataTable(customerTable);

            return mappedCustomers[0];
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error in {nameof(GetCustomerByIdAsync)}: {e.Message}");
            throw;
        }
    }

    public async Task<Customer?> AddCustomerAsync(SqlConnection sqlConnection, string firstName, string lastName,
        string street, string houseNumber, string zipCode, string city, string phoneNumber, string emailAddress,
        CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            sqlTransaction = sqlConnection.BeginTransaction();

            var newCustomerId = Guid.NewGuid();

            var newCustomer = new Customer(newCustomerId, firstName, lastName, street, houseNumber, zipCode, city,
                phoneNumber, emailAddress);

            var rowsAffected =
                await _unitOfWork.CustomerRepository.AddCustomerAsync(sqlConnection, newCustomer, sqlTransaction, ct);

            if (rowsAffected != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return null;
            }

            await sqlTransaction.CommitAsync(ct);

            return newCustomer;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();
            Debug.WriteLine($"Error in {nameof(AddCustomerAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }

    public async Task<bool> UpdateCustomerAsync(SqlConnection sqlConnection, Customer customer,
        CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            // get existing customer

            var existingCustomerTable =
                await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(sqlConnection, customer.CustomerId, ct);

            if (existingCustomerTable.Rows.Count != 1)
                return false;

            var mappedCustomers = Customer.MapFromDataTable(existingCustomerTable);
            var existingCustomer = mappedCustomers[0];

            sqlTransaction = sqlConnection.BeginTransaction();

            var rowsAffected =
                await _unitOfWork.CustomerRepository.UpdateCustomerAsync(sqlConnection, customer, sqlTransaction, ct);

            if (rowsAffected != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return false;
            }

            await sqlTransaction.CommitAsync(ct);
            return true;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();
            Debug.WriteLine($"Error in {nameof(UpdateCustomerAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }

    public async Task<bool> DeleteCustomerAsync(SqlConnection sqlConnection, Guid customerId,
        CancellationToken ct = default)
    {
        SqlTransaction? sqlTransaction = null;

        try
        {
            // get all account Ids
            var accountIdsTable =
                await _unitOfWork.AccountRepository.GetAllAccountIdsByCustomerIdAsync(sqlConnection, customerId, ct);

            List<Guid> accountIds = [];

            if (accountIdsTable.Rows.Count > 0)
            {
                foreach (DataRow row in accountIdsTable.Rows)
                {
                    if (row["AccountId"] != DBNull.Value)
                        accountIds.Add((Guid)row["AccountId"]);
                }
            }

            if (accountIds.Count != accountIdsTable.Rows.Count)
                return false;

            // start transaction
            sqlTransaction = sqlConnection.BeginTransaction();

            // delete all balance snapshots of all accounts of the customer
            var deletedSnapshotsCount =
                _unitOfWork.BalanceSnapshotRepository.DeleteByAccountIdsAsync(sqlConnection, accountIds, sqlTransaction,
                    ct);

            // delete all account transactions of all accounts of the customer
            var deletedTransactionsCount =
                _unitOfWork.AccountTransactionRepository.DeleteByAccountIdsAsync(sqlConnection, accountIds,
                    sqlTransaction, ct);

            // delete all accounts of the customer
            var deletedAccountsCount =
                await _unitOfWork.AccountRepository.DeleteBatchAsync(sqlConnection, accountIds, sqlTransaction,
                    ct);

            if (deletedAccountsCount != accountIds.Count)
            {
                await sqlTransaction.RollbackAsync(ct);
                return false;
            }

            // delete the customer
            var deletedCustomersCount =
                await _unitOfWork.CustomerRepository.DeleteCustomerAsync(sqlConnection, customerId, sqlTransaction, ct);

            if (deletedCustomersCount != 1)
            {
                await sqlTransaction.RollbackAsync(ct);
                return false;
            }

            await sqlTransaction.CommitAsync(ct);
            return true;
        }
        catch (Exception e)
        {
            sqlTransaction?.Rollback();
            Debug.WriteLine($"Error in {nameof(DeleteCustomerAsync)}: {e.Message}");
            throw;
        }
        finally
        {
            if (sqlTransaction != null)
                await sqlTransaction.DisposeAsync();
        }
    }
}