using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    public static class ProductMapper
    {
        /// <summary>
        ///     Maps a Product data model to a ProductModel View model
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static ProductModel
            SerializeProductModel(Product product)
        {
            return new ProductModel
            {
                Id = product.Id,
                CreateOn = product.CreateOn,
                UpdateOn = product.UpdateOn,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                IsTaxable = product.IsTaxable,
                IsArchived = product.IsArchived
            };
        }

        /// <summary>
        ///     Maps a ProductModel View model to a Product data model
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static Product
            SerializeProductModel(ProductModel product)
        {
            return new Product
            {
                Id = product.Id,
                CreateOn = product.CreateOn,
                UpdateOn = product.UpdateOn,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                IsTaxable = product.IsTaxable,
                IsArchived = product.IsArchived
            };
        }
    }
}