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
    public class CustomerRepository
    {
        private readonly DatabaseService _databaseService;

        public CustomerRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public CustomerRepository()
        {
            _databaseService = new DatabaseService();
        }

        public async Task<DataTable> GetAllCustomersAsync(SqlConnection sqlConnection, CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM Customers";

                List<SqlParameter> sqlParameters = [];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetAllCustomersAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetCustomerByIdAsync(SqlConnection sqlConnection, Guid customerId,
            CancellationToken ct = default)
        {
            try
            {
                var query = "SELECT * FROM Customers WHERE CustomerId = @CustomerId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = customerId }
                ];

                return await _databaseService.ReadDataAsync(sqlConnection, query, sqlParameters, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(GetCustomerByIdAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> AddCustomerAsync(SqlConnection sqlConnection, Customer customer,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var query =
                    "INSERT INTO Customers (CustomerId, FirstName, LastName, Street, HouseNumber, ZipCode, City, PhoneNumber, EmailAddress) " +
                    "VALUES (@CustomerId, @FirstName, @LastName, @Street, @HouseNumber, @ZipCode, @City, @PhoneNumber, @EmailAddress)";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = customer.CustomerId },
                    new SqlParameter("@FirstName", SqlDbType.NVarChar, 100) { Value = customer.FirstName },
                    new SqlParameter("@LastName", SqlDbType.NVarChar, 100) { Value = customer.LastName },
                    new SqlParameter("@Street", SqlDbType.NVarChar, 200) { Value = customer.Street },
                    new SqlParameter("@HouseNumber", SqlDbType.NVarChar, 25) { Value = customer.HouseNumber },
                    new SqlParameter("@ZipCode", SqlDbType.NVarChar, 25) { Value = customer.ZipCode },
                    new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = customer.City },
                    new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 50) { Value = customer.PhoneNumber },
                    new SqlParameter("@EmailAddress", SqlDbType.NVarChar, 100) { Value = customer.EmailAddress }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, query, sqlParameters, sqlTransaction,
                    ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(AddCustomerAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateCustomerAsync(SqlConnection sqlConnection, Customer customer,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement = "UPDATE Customers SET " +
                                      "FirstName = @FirstName, " +
                                      "LastName = @LastName, " +
                                      "Street = @Street, " +
                                      "HouseNumber = @HouseNumber, " +
                                      "ZipCode = @ZipCode, " +
                                      "City = @City, " +
                                      "PhoneNumber = @PhoneNumber, " +
                                      "EmailAddress = @EmailAddress " +
                                      "WHERE CustomerId = @CustomerId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = customer.CustomerId },
                    new SqlParameter("@FirstName", SqlDbType.NVarChar, 100) { Value = customer.FirstName },
                    new SqlParameter("@LastName", SqlDbType.NVarChar, 100) { Value = customer.LastName },
                    new SqlParameter("@Street", SqlDbType.NVarChar, 200) { Value = customer.Street },
                    new SqlParameter("@HouseNumber", SqlDbType.NVarChar, 25) { Value = customer.HouseNumber },
                    new SqlParameter("@ZipCode", SqlDbType.NVarChar, 25) { Value = customer.ZipCode },
                    new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = customer.City },
                    new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 50) { Value = customer.PhoneNumber },
                    new SqlParameter("@EmailAddress", SqlDbType.NVarChar, 100) { Value = customer.EmailAddress }
                ];


                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(UpdateCustomerAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteCustomerAsync(SqlConnection sqlConnection, Guid customerId,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                var statement = "DELETE FROM Customers WHERE CustomerId = @CustomerId";

                List<SqlParameter> sqlParameters =
                [
                    new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = customerId }
                ];

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(DeleteCustomerAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteBatchAsync(SqlConnection sqlConnection, List<Guid> customerIds,
            SqlTransaction? sqlTransaction = null, CancellationToken ct = default)
        {
            try
            {
                if (customerIds.Count == 0)
                    return 0;

                var parameterString =
                    string.Join(", ", customerIds.Select((id, index) => $"@CustomerId{index}").ToList());

                var statement = $"DELETE FROM Customers WHERE CustomerId IN ({parameterString})";

                List<SqlParameter> sqlParameters = customerIds.Select((id, index) =>
                    new SqlParameter($"@CustomerId{index}", SqlDbType.UniqueIdentifier) { Value = id }
                ).ToList();

                return await _databaseService.ExecuteStatementAsync(sqlConnection, statement, sqlParameters,
                    sqlTransaction, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {nameof(DeleteBatchAsync)}: {ex.Message}");
                throw;
            }
        }
    }
}