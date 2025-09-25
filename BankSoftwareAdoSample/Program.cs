global using Microsoft.Data.SqlClient;
using BankSoftwareAdoSample.Services;
using System.Diagnostics;

namespace BankSoftwareAdoSample;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        var databaseService = new DatabaseService();
        var unitOfWork = new UnitOfWork(databaseService);

        var accountService = new AccountService(unitOfWork);
        var customerService = new CustomerService(unitOfWork);
        var accountTransactionService = new AccountTransactionService(unitOfWork);
        var balanceSnapshotService = new BalanceSnapshotService(unitOfWork);

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(databaseService, unitOfWork, customerService, accountService, accountTransactionService, balanceSnapshotService));

    }
}