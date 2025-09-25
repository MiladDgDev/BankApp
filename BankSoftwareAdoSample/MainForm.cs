
using BankSoftwareAdoSample.Models;
using BankSoftwareAdoSample.Services;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace BankSoftwareAdoSample
{
    public partial class MainForm : Form
    {
        private readonly DatabaseService _databaseService;
        private readonly UnitOfWork _unitOfWork;

        private readonly CustomerService _customerService;
        private readonly AccountService _accountService;
        private readonly AccountTransactionService _accountTransactionService;
        private readonly BalanceSnapshotService _balanceSnapshotService;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private DataTable _customersTable = new();

        public MainForm(DatabaseService databaseService, UnitOfWork unitOfWork, CustomerService customerService, AccountService accountService, AccountTransactionService accountTransactionService, BalanceSnapshotService balanceSnapshotService)
        {

            _databaseService = databaseService;
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _accountService = accountService;
            _accountTransactionService = accountTransactionService;
            _balanceSnapshotService = balanceSnapshotService;
            _cancellationTokenSource = new CancellationTokenSource();
            InitializeComponent();

            customersLabel.Left = (this.Width - customersLabel.Width) / 3;

        }

        private async Task ReloadCustomers()
        {
            try
            {
                _customersTable = await _databaseService.ExecuteConnectionAsync(_unitOfWork.CustomerRepository.GetAllCustomersAsync);

                customerDataGridView.DataSource = _customersTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void fetchAllCustomersBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await ReloadCustomers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addCustomerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using var addCustomerForm = new AddCustomerForm(_databaseService, _customerService, _cancellationTokenSource.Token);

                addCustomerForm.CustomerAdded += async (s, args) =>
                {
                    await ReloadCustomers();
                };

                addCustomerForm.ShowDialog(this);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCustomerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCustomerRow = customerDataGridView.CurrentRow?.DataBoundItem as DataRowView;

                if (selectedCustomerRow is null) return;

                var customerId = (Guid)selectedCustomerRow["CustomerId"];
                var firstName = (string)selectedCustomerRow["FirstName"];
                var lastName = (string)selectedCustomerRow["LastName"];
                var street = (string)selectedCustomerRow["Street"];
                var houseNumber = (string)selectedCustomerRow["HouseNumber"];
                var zipCode = (string)selectedCustomerRow["ZipCode"];
                var city = (string)selectedCustomerRow["City"];
                var phoneNumber = (string)selectedCustomerRow["PhoneNumber"];
                var emailAddress = (string)selectedCustomerRow["EmailAddress"];
                var selectedCustomer = new Customer(customerId, firstName, lastName, street, houseNumber, zipCode, city, phoneNumber, emailAddress);


                using var updateCustomerForm = new UpdateCustomerForm(selectedCustomer, _databaseService, _customerService, _cancellationTokenSource.Token);

                updateCustomerForm.UpdateSuccessful += async (s, args) =>
                {
                    await ReloadCustomers();
                };

                updateCustomerForm.ShowDialog(this);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void deleteCustomerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCustomerRow = customerDataGridView.CurrentRow?.DataBoundItem as DataRowView;

                if (selectedCustomerRow is null) return;

                var customerId = (Guid)selectedCustomerRow["CustomerId"];

                var result = await _databaseService.ExecuteConnectionAsync(_customerService.DeleteCustomerAsync, customerId, _cancellationTokenSource.Token);

                if (result is not true)
                {
                    MessageBox.Show("Failed to delete customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await ReloadCustomers();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
