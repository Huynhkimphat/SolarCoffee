using SolarCoffee.Data.Models;
using System.Collections.Generic;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        public List<ProductInventory> GetCurrenInventory();

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id,int adjustment);

        public ProductInventory GetInventoryByProductId(int id);

        public List<ProductInventorySnapshot> GetSnapshotsHistory();
    }
}