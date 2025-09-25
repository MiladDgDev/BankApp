using BankSoftwareAdoSample.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankSoftwareAdoSample.Models
{
    public class Account
    {
        public Guid AccountId { get; private set; }

        public Guid CustomerId { get; private set; }

        public string AccountNumber { get; private set; }

        public string Iban { get; private set; }

        public Decimal OpeningBalance { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public bool IsActive { get; private set; }


        public Account(Guid accountId, Guid customerId, string accountNumber, decimal openingBalance,
            DateTimeOffset createdAt, bool isActive)
        {
            AccountId = accountId;
            CustomerId = customerId;
            AccountNumber = accountNumber;
            Iban = GenerateIban(AccountNumber);
            OpeningBalance = openingBalance;
            CreatedAt = createdAt;
            IsActive = isActive;
        }

        public static string GenerateIban(string accountNumber)
        {
            const string countryCode = "DE";
            const string bankCode = "10020030";

            return $"{countryCode}{bankCode}{accountNumber}";
        }

        public static decimal CalculateBalance(Account account, BalanceSnapshot? latestBalanceSnapshot,
            List<AccountTransaction> transactions)
        {
            var startingBalance = account.OpeningBalance;
            var startingDateTime = account.CreatedAt;

            if (latestBalanceSnapshot is not null)
            {
                startingBalance = latestBalanceSnapshot.Balance;
                startingDateTime = latestBalanceSnapshot.SnapshotDateTime;
            }

            var calculatedBalance = startingBalance;

            foreach (var transaction in transactions.Where(t => t.TransactionDateTime > startingDateTime))
            {
                if (transaction.TransactionType is TransactionType.Incoming or TransactionType.Deposit)
                    calculatedBalance += transaction.Amount;

                if (transaction.TransactionType is TransactionType.Transfer or TransactionType.Withdrawal)
                    calculatedBalance -= transaction.Amount;
            }

            return calculatedBalance;
        }

      public static List<Account> MapFromDataTable(DataTable table)
        {
            var accounts = new List<Account>();
            try
            {
                if (table.Rows.Count == 0)
                    throw new ArgumentException("DataTable is empty.");
        
                foreach (DataRow row in table.Rows)
                {
                    var accountId = (Guid)row["AccountId"];
                    var customerId = (Guid)row["CustomerId"];
                    var accountNumber = (string)row["AccountNumber"];
                    var openingBalance = Convert.ToDecimal(row["OpeningBalance"]);
                    var createdAt = (DateTimeOffset)row["CreatedAt"];
                    var isActive = Convert.ToBoolean(row["IsActive"]);
        
                    accounts.Add(new Account(accountId, customerId, accountNumber, openingBalance, createdAt, isActive));
                }
                return accounts;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}