using BankSoftwareAdoSample.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSoftwareAdoSample.Models
{
    public class AccountTransaction
    {
        public Guid TransactionId { get; private set; }

        public Guid AccountId { get; private set; }

        public DateTimeOffset TransactionDateTime { get; private set; }

        public decimal Amount { get; private set; }

        public string Description { get; private set; }

        public TransactionType TransactionType { get; private set; }

        public AccountTransaction(Guid transactionId, Guid accountId, DateTimeOffset transactionDateTime,
            decimal amount, string description, TransactionType transactionType)
        {
            TransactionId = transactionId;
            AccountId = accountId;
            TransactionDateTime = transactionDateTime;
            Amount = amount;
            Description = description;
            TransactionType = transactionType;
        }

        public static List<AccountTransaction> MapFromDataTable(DataTable table)
        {
            var result = new List<AccountTransaction>();
            
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var transactionTypeInt = (int)row["TransactionType"];
                    
                    var enumExists = Enum.IsDefined(typeof(TransactionType), transactionTypeInt);
                    
                    if (!enumExists)
                    {
                        throw new ArgumentOutOfRangeException($"Invalid TransactionType value: {transactionTypeInt}");
                    }
                    
                    var transactionType = (TransactionType)transactionTypeInt;

                    var accountTransaction = new AccountTransaction(
                        (Guid)row["TransactionId"],
                        (Guid)row["AccountId"],
                        (DateTimeOffset)row["TransactionDateTime"],
                        (decimal)row["Amount"],
                        row["Description"].ToString() ?? string.Empty,
                        transactionType
                    );
                    
                    result.Add(accountTransaction);
                }
                
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}