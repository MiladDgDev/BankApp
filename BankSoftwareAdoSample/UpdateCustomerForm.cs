using BankSoftwareAdoSample.Models;
using BankSoftwareAdoSample.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankSoftwareAdoSample
{
    public partial class UpdateCustomerForm : Form
    {
        private Customer _customer;
        private readonly DatabaseService _databaseService;
        private readonly CustomerService _customerService;
        private readonly CancellationToken _ct;

        public EventHandler? UpdateSuccessful;

        public UpdateCustomerForm(Customer customer, DatabaseService databaseService, CustomerService customerService, CancellationToken ct)
        {
            _customer = customer;
            _databaseService = databaseService;
            _customerService = customerService;

            InitializeComponent();
            updateCustomerBtn.Left = (this.Width - updateCustomerBtn.Width) / 2;
        }

        private void UpdateCustomerForm_Load(object sender, EventArgs e)
        {
            firstNameTextBox.Text = _customer.FirstName;
            lastNameTextBox.Text = _customer.LastName;
            streetTextBox.Text = _customer.Street;
            houseNoTextBox.Text = _customer.HouseNumber;
            zipCodeTextBox.Text = _customer.ZipCode;
            cityTextBox.Text = _customer.City;
            phoneNoTextBox.Text = _customer.PhoneNumber;
            emailTextBox.Text = _customer.EmailAddress;
        }

        private async void updateCustomerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var customerPatchObj = new Customer(
                    _customer.CustomerId,
                    firstNameTextBox.Text,
                    lastNameTextBox.Text,
                    streetTextBox.Text,
                    houseNoTextBox.Text,
                    zipCodeTextBox.Text,
                    cityTextBox.Text,
                    phoneNoTextBox.Text,
                    emailTextBox.Text
                    );

                var result = await _databaseService.ExecuteConnectionAsync(
                        _customerService.UpdateCustomerAsync, customerPatchObj, _ct);

                if (result is not true)
                {
                    MessageBox.Show("Failed to update customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                var msgResult = MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UpdateSuccessful?.Invoke(this, EventArgs.Empty);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("An error occurred while updating the customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
