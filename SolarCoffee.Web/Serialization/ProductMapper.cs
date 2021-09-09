using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    public static class ProductMapper
    {
        // Maps a Product data model to a ProductModel View model
        public static ProductModel
        SerializationProductModel(Data.Models.Product product)
        {
            return new ProductModel {
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

        // Maps a ProductModel View model to a Product data model
        public static Data.Models.Product
        SerializationProductModel(ProductModel product)
        {
            return new Data.Models.Product {
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
