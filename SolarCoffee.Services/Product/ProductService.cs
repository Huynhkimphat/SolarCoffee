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

		/// <summary>
		/// Retrieves All Product from database
		/// </summary>
		/// <returns>List<Product></returns>
		public List<Data.Models.Product> GetAllProducts()
		{
			return _db.Products.ToList();
		}

		/// <summary>
		/// Retrieves a Product frm the database by primary key
		/// </summary>
		/// <returns>Product</returns>
		public Data.Models.Product GetProductById(int id)
		{
			return _db.Products.Find(id);
		}

		/// <summary>
		/// Adds a new product to the database
		/// </summary>
		/// <param name="product"></param>
		/// <returns>ServiceResponse<Product></returns>
		public ServiceResponse<Data.Models.Product>
		CreateProduct(Data.Models.Product product)
		{
			var now = DateTime.UtcNow;
			try
			{
				_db.Products.Add(product);

				var newInventory =
					new ProductInventory()
					{ Product = product, QuantityOnHand = 0, IdealOnHand = 10 };

				_db.ProductInventories.Add(newInventory);
				_db.SaveChanges();
				return new ServiceResponse<Data.Models.Product>
				{
					Data = product,
					Time = now,
					Message = "Saved new product",
					IsSuccess = true
				};
			}
			catch (Exception e)
			{
				return new ServiceResponse<Data.Models.Product>
				{
					Data = product,
					Time = now,
					Message = e.StackTrace,
					IsSuccess = false
				};
			}
		}

		/// <summary>
		/// Archives a Product by setting boolean IsArchived to true
		/// </summary>
		/// <param name="id"></param>
		/// <returns>ServiceResponse<Product></returns>
		public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
		{
			var now = DateTime.UtcNow;
			try
			{
				var product = _db.Products.Find(id);
				product.IsArchived = true;
				_db.SaveChanges();

				return new ServiceResponse<Data.Models.Product>
				{
					Data = product,
					Time = now,
					Message = "Archived Product",
					IsSuccess = true
				};
			}
			catch (Exception e)
			{
				return new ServiceResponse<Data.Models.Product>
				{
					Data = null,
					Time = now,
					Message = e.StackTrace,
					IsSuccess = false
				};
			}
		}
	}
}
