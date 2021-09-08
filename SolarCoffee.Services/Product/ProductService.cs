using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _db;

        public ProductService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        //! Retrieves All Product from database
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        //! Retrieves a Product frm the database by primary key
        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }

        //! Adds a new product to the database
        public ServiceResponse<Data.Models.Product>
        CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add (product);

                var newInventory =
                    new ProductInventory()
                    { Product = product, QuantityOnHand = 0, IdealOnHand = 10 };

                _db.ProductInventories.Add (newInventory);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Product> {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Saved new product",
                    IsSuccess = false
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product> {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccess = true
                };
            }
        }

        //! Archives a Product by setting boolean IsArchived to true
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try{
                var product = _db.Products.Find(id);
                product.IsArchived =true;
                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>{
                    Data=product,
                    Time=DateTime.UtcNow,
                    Message="Archived Product", 
                    isSuccess=true
                }
            }
            catch(Exception e){
                return new ServiceResponse<Data.Models.Product>{
                    Data=null,
                    Time=DateTime.UtcNow,
                    Message=e.StackTrace, 
                    isSuccess=false
                }
            }
        }
    }
}
