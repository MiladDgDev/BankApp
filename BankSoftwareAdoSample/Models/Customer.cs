using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSoftwareAdoSample.Models
{
    public class Customer
    {
        public Guid CustomerId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Street { get; private set; }
        public string HouseNumber { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }

        public Customer(Guid customerId, string firstName, string lastName, string street, string houseNumber,
            string zipCode, string city, string phoneNumber, string emailAddress)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            HouseNumber = houseNumber;
            ZipCode = zipCode;
            City = city;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public static List<Customer> MapFromDataTable(DataTable table)
        {
            var result = new List<Customer>();
            
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var customerId = (Guid)row["CustomerId"];
                    var firstName = (string)row["FirstName"];
                    var lastName = (string)row["LastName"];
                    var street = (string)row["Street"];
                    var houseNumber = (string)row["HouseNumber"];
                    var zipCode = (string)row["ZipCode"];
                    var city = (string)row["City"];
                    var phoneNumber = (string)row["PhoneNumber"];
                    var emailAddress = (string)row["EmailAddress"];


                    result.Add(new Customer(customerId, firstName, lastName, street, houseNumber, zipCode, city,
                        phoneNumber,
                        emailAddress));
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}