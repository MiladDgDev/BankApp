using BankSoftwareAdoSample.Repositories;

namespace BankSoftwareAdoSample.Services;

public class UnitOfWork
{
    private readonly AccountRespository _accountRepository;
    private readonly AccountTransactionRepository _accountTransactionRepository;
    private readonly BalanceSnapshotRepository _balanceSnapshotRepository;
    private readonly CustomerRepository _customerRepository;

    public UnitOfWork(DatabaseService databaseService)
    {
        _accountRepository = new AccountRespository(databaseService);
        _accountTransactionRepository = new AccountTransactionRepository(databaseService);
        _balanceSnapshotRepository = new BalanceSnapshotRepository(databaseService);
        _customerRepository = new CustomerRepository(databaseService);
    }

    public AccountRespository AccountRepository => _accountRepository;
    public AccountTransactionRepository AccountTransactionRepository => _accountTransactionRepository;
    public BalanceSnapshotRepository BalanceSnapshotRepository => _balanceSnapshotRepository;
    public CustomerRepository CustomerRepository => _customerRepository;
}