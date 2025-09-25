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
    public partial class AddCustomerForm : Form
    {
        private readonly DatabaseService _databaseService;
        private readonly CustomerService _customerService;
        private CancellationToken _ct;

        public event EventHandler? CustomerAdded;

        public AddCustomerForm(DatabaseService databaseService, CustomerService customerService, CancellationToken ct)
        {
            _databaseService = databaseService;
            _customerService = customerService;
            _ct = ct;

            InitializeComponent();
            addCustomerBtn.Left = (this.Width - addCustomerBtn.Width) / 2;
        }



        private async void addCustomerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var firstName = firstNameTextBox.Text;
                var lastName = lastNameTextBox.Text;
                var street = streetTextBox.Text;
                var houseNumber = houseNoTextBox.Text;
                var zipCode = zipCodeTextBox.Text;
                var city = cityTextBox.Text;
                var phoneNumber = phoneNoTextBox.Text;
                var emailAddress = emailTextBox.Text;


                var addedCustomer = await _databaseService.ExecuteConnectionAsync(
                    async (connection, token) =>
                    {
                        return await _customerService.AddCustomerAsync(connection,
                            firstName,
                            lastName,
                            street,
                            houseNumber,
                            zipCode,
                            city,
                            phoneNumber,
                            emailAddress,
                            token);

                    }, _ct);

                if (addedCustomer is null)
                {
                    MessageBox.Show("Failed to add customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CustomerAdded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("An error occurred while adding the customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
