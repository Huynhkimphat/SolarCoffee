using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Return a list of Customers from database
        /// </summary>
        /// <returns>List<Customer></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db
                .Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        /// <summary>
        /// Add new Customer record
        /// </summary>
        /// <param name="customer">Customer Instance</param>
        /// <returns>ServiceResponse<Customer></returns>
        public ServiceResponse<Data.Models.Customer>
        CreateCustomer(Data.Models.Customer customer)
        {
            var now = DateTime.UtcNow;
            try
            {
                _db.Customers.Add (customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer> {
                    Data = customer,
                    Time = now,
                    Message = "Saved new product",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer> {
                    Data = null,
                    Time = now,
                    Message = e.StackTrace,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Delete a customer record
        /// </summary>
        /// <param name="id">int customer id</param>
        /// <returns>ServiceResponse<bool></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var now = DateTime.UtcNow;
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return new ServiceResponse<bool> {
                    Data = false,
                    Time = now,
                    Message = "Customer to delete not found",
                    IsSuccess = false
                };
            }
            try
            {
                _db.Customers.Remove (customer);
                _db.SaveChanges();
                return new ServiceResponse<bool> {
                    Data = true,
                    Time = now,
                    Message = "Customer Deleted",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool> {
                    Data = false,
                    Time = now,
                    Message = e.StackTrace,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get a customer record By Primary Id
        /// </summary>
        /// <param name="id">int customer id</param>
        /// <returns>Customer</returns>
        public Data.Models.Customer GetCustomerById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}
