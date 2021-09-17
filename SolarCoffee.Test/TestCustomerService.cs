using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customer;
using Xunit;
using System.Linq;
using Moq;

namespace SolarCoffee.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void CustomerService_GetAllCustomers_GivenTheyExist()
        {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("gets_all")
                .Options;
            
            using var context =new SolarDbContext(options);

            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 123123});
            sut.CreateCustomer(new Customer {Id = -213});

            var allCustomers = sut.GetAllCustomers();
            
            allCustomers.Count.Should().Be(2);
        }

        [Fact]
        public void CustomerService_CreatesCustomer_GivenNewCustomerObject()
        {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("Add_writes_to_database")
                .Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 18645});
            context.Customers.Single().Id.Should().Be(18645);

        }

        [Fact]
        public void CustomerService_DeletesCustomer_GivenId()
        {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("deletes_one")
                .Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 18654});

            sut.DeleteCustomer(18654);
            var allCustomers=sut.GetAllCustomers();
            allCustomers.Count.Should().Be(0);
        }

        [Fact]
        public void CustomerService_OrdersByLastName_WhenGetAllCustomersInvoked()
        {
            // Arrange
            var data = new List<Customer>
            {
                new Customer {Id = 123, LastName = "Zulu"},
                new Customer {Id = 323, LastName = "Alpha"},
                new Customer {Id = -89, LastName = "Xu"},
                new Customer {Id = 456, LastName = "Gama"},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mockContext = new Mock<SolarDbContext>();

            mockContext.Setup(c => c.Customers)
                .Returns(mockSet.Object);
            
            // Act    
            var sut = new CustomerService(mockContext.Object);
            var customers = sut.GetAllCustomers();
            var allCustomers = sut.GetAllCustomers();

            // Assert
            customers.Count.Should().Be(4);
            customers[0].Id.Should().Be(323);
            customers[1].Id.Should().Be(456);
            customers[2].Id.Should().Be(-89);
            customers[3].Id.Should().Be(123);

        }
    }
}