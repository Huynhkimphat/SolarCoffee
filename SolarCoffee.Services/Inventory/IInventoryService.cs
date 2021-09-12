using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        public List<ProductInventory> GetCurrenInventory();

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);

        public ProductInventory GetInventoryByProductId(int id);

        public List<ProductInventorySnapshot> GetSnapshotsHistory();
    }
}