using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSoftwareAdoSample.Models
{
    public class BalanceSnapshot
    {
        public Guid BalanceSnapshotId { get; set; }
        public Guid AccountId { get; set; }
        public DateTimeOffset SnapshotDateTime { get; set; }
        public decimal Balance { get; set; }
        public string SnapshotHash { get; set; }

        public BalanceSnapshot(Guid balanceSnapshotId, Guid accountId, DateTimeOffset snapshotDateTime,
            decimal balance,
            string snapshotHash)
        {
            BalanceSnapshotId = balanceSnapshotId;
            AccountId = accountId;
            SnapshotDateTime = snapshotDateTime;
            Balance = balance;
            SnapshotHash = snapshotHash;
        }

        public static string GenerateSnapshotHash(Guid accountId, DateTimeOffset snapshotDateTime,
            List<AccountTransaction> transactions, decimal balance)
        {
            string rawData =
                $"{accountId}{snapshotDateTime.ToUnixTimeSeconds()}{balance}{string.Join(string.Empty, transactions.Select(t => t.TransactionId))}";

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public static List<BalanceSnapshot> MapFromDataTable(DataTable table)
        {
            var result = new List<BalanceSnapshot>();

            try
            {
                if (table.Rows.Count == 0)
                    throw new ArgumentException("DataTable is empty.");

                foreach (DataRow row in table.Rows)
                {
                    var balanceSnapshotId = (Guid)row["BalanceSnapshotId"];
                    var accountId = (Guid)row["AccountId"];
                    var snapshotDateTime = (DateTimeOffset)row["SnapshotDateTime"];
                    var balance = Convert.ToDecimal(row["Balance"]);
                    var snapshotHash = (string)row["SnapshotHash"];

                    result.Add(new BalanceSnapshot(balanceSnapshotId, accountId, snapshotDateTime, balance,
                        snapshotHash));
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