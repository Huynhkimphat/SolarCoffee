using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
	public class InventoryService : IInventoryService
	{
		private readonly SolarDbContext _db;

		private readonly ILogger<InventoryService> _logger;

		public InventoryService(
			SolarDbContext dbContext,
			ILogger<InventoryService> logger
		)
		{
			_db = dbContext;
			_logger = logger;
		}

		/// <summary>
		/// Return all current inventory from the database
		/// </summary>
		/// <returns></returns>
		public List<ProductInventory> GetCurrenInventory()
		{
			return _db
				.ProductInventories
				.Include(pi => pi.Product)
				.Where(pi => !pi.Product.IsArchived)
				.ToList();
		}

		/// <summary>
		/// Updates number of units available of the provided product id
		/// Adjust QuantityOnHand by adjustment value
		/// </summary>
		/// <param name="id">productId</param>
		/// <param name="adjustment">number of units added / removed from inventory</param>
		/// <returns>ServiceResponse<ProductInventory></returns>
		public ServiceResponse<ProductInventory>
		UpdateUnitsAvailable(int id, int adjustment)
		{
			var now = DateTime.UtcNow;
			try
			{
				var inventory =
					_db
						.ProductInventories
						.Include(inv => inv.Product)
						.First(inv => inv.Product.Id == id);

				inventory.QuantityOnHand += adjustment;

				try
				{
					CreateSnapshot(inventory);
				}
				catch (Exception e)
				{
					_logger.LogError("Error creating inventory snapshot");
					_logger.LogError(e.StackTrace);
				}

				_db.SaveChanges();

				return new ServiceResponse<ProductInventory>
				{
					IsSuccess = true,
					Time = now,
					Data = inventory,
					Message = $"Product {id} inventory adjusted"
				};
			}
			catch
			{
				return new ServiceResponse<ProductInventory>
				{
					IsSuccess = false,
					Time = now,
					Data = null,
					Message = "Error Updating Product QuantityOnHand"
				};
			}
		}

		/// <summary>
		/// Gets a ProductInventory instance by Product ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ProductInventory GetInventoryByProductId(int id)
		{
			return _db
				.ProductInventories
				.Include(pi => pi.Product)
				.FirstOrDefault(pi => pi.Product.Id == id);
		}

		/// <summary>
		/// Return Snapshot history for the previous 6 hours
		/// </summary>
		/// <returns></returns>
		public List<ProductInventorySnapshot> GetSnapshotsHistory()
		{
			var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
			return _db
				.ProductInventorySnapshots
				.Include(snap => snap.Product)
				.Where(snap =>
					snap.SnapshotTime > earliest && !snap.Product.IsArchived)
				.ToList();
		}

		/// <summary>
		/// Create a Snapshot record using the provided ProductInventory instance
		/// </summary>
		/// <param name="inventory"></param>
		private void CreateSnapshot(ProductInventory inventory)
		{
			var now = DateTime.UtcNow;
			var snapshot =
				new ProductInventorySnapshot
				{
					SnapshotTime = now,
					Product = inventory.Product,
					QuantityOnHand = inventory.QuantityOnHand
				};
			_db.Add(snapshot);
		}
	}
}
